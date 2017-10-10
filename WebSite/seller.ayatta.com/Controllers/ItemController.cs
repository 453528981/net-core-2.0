using System;
using Ayatta;
using System.IO;
using System.Linq;
using System.Text;
using Ayatta.Domain;
using Ayatta.Storage;
using Newtonsoft.Json;
using System.Net.Http;
using Ayatta.Extension;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("item")]

    public class ItemController : BaseController
    {
        public ItemController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<ItemController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }
        /*
        [HttpGet("/items")]
        public IActionResult Index(ProdItemSearchParam param)
        {

            var model = new ItemModel.Index();
            if (param.Keyword.IsMatch(@"^\d+$"))
            {
                param.Id = param.Keyword.AsInt();
            }
            else
            {
                param.Name = param.Keyword;
            }
            model.Param = param;
            model.Items = DefaultStorage.ItemSearch(param);
            return View(model);
        }
        */

        [HttpGet("/item/{id}")]
        public IActionResult Item(int id = 0, int catgId = 0, int spuId = 0)
        {
            var data = new ItemMini();
            var model = new ItemModel.Item(data);

            model.CatgId = catgId;
            model.ItemDesc = new ItemDesc();

            if (id > 0)
            {
                data = DefaultStorage.ItemMiniGet(id);

                model.Data = data;
                model.CatgId = data.CatgId;

                model.ItemDesc = DefaultStorage.ItemDescGet(id);
            }
            return View(model);
        }

        [HttpPost("/item/{id}")]
        public IActionResult Item(int id, int catgId, int spuId, Item item)
        {
            var result = new Result<string>();
            if (id < 0)
            {
                result.Message = "参数错误(Id)";
                return Json(result);
            }
            if (catgId < 0)
            {
                result.Message = "参数错误(CatgId)";
                return Json(result);
            }
            if (!User)
            {
                result.Message = "未登录或登录超时";
                return Json(result);
            }

            var form = Request.Form;
            var time = DateTime.Now;
            var propPrefix = "prop.";
            var itemPrefix = "item.";

            if (item.Name.IsNullOrEmpty() || item.Name.Length > 30)
            {
                result.Data = "name";
                return Json(result);
            }
            if (!item.Title.IsNullOrEmpty() && item.Title.Length > 30)
            {
                result.Data = "title";
                return Json(result);
            }
            if (item.Stock < 1 || item.Stock > 99999999)
            {
                result.Data = "stock";
                return Json(result);
            }
            if (item.RetailPrice < 1 || item.RetailPrice > 99999999)
            {
                result.Data = "retailPrice";
                return Json(result);
            }
            if (item.Price < 1 || item.Price > 99999999)
            {
                result.Data = "price";
                return Json(result);
            }
            if (item.AppPrice < 1 || item.AppPrice > 99999999)
            {
                result.Data = "appPrice";
                return Json(result);
            }
            var summary = form[itemPrefix + "summary"].ToString();
            if (summary.IsNullOrEmpty() || (summary.Length < 5 || summary.Length > 200))
            {
                result.Data = "summary";
                return Json(result);
            }

            var subStock = Convert.ToByte(form[itemPrefix + "subStock"].ToString());

            var isTiming = false;
            var onlineTime = time;
            var itemStatus = Prod.Status.Online;
            var onlineTimeField = form[itemPrefix + "online"];
            if (onlineTimeField == "0" || onlineTimeField == "1")
            {
                if (onlineTimeField == "1")
                {
                    itemStatus = Prod.Status.Offline;
                }
            }
            else if (!DateTime.TryParse(onlineTimeField, out onlineTime))
            {
                //var error = new { name = prefix + "onlineTime", message = "请选择日期及时间！" };
                //errors.Add(error);
                result.Data = "onlinetime";
                result.Message = "请选择日期及时间！";
                return Json(result);
            }
            else
            {
                isTiming = true;
            }

            var data = DefaultStorage.CatgMiniGet(catgId);
            if (data == null || !data.Props.Any())
            {
                result.Message = "参数错误！";
                return Json(result);
            }

            var catgIds = GetCatgIds(catgId);
            var catgRId = catgIds.FirstOrDefault();
            var catgs = GetCatgs(catgId);
            item.CatgFId = string.Join(",", catgIds);
            item.CatgFName= string.Join(",", catgs.Select(x=>x.Name));

            var props = new List<Prod.Prop>();
            
            var propStrBuilder = new StringBuilder();

            var searchPropIdBuilder = new StringBuilder();
            var searchPropStrBuilder = new StringBuilder();

            foreach (var p in data.Props)
            {
                if (p.IsEnumProp && p.Values != null)
                {
                    var val = form[propPrefix + p.Id].ToArray().Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    var array = val.Select(x => Convert.ToInt32(x));

                    foreach (var o in array)
                    {
                        foreach (var v in p.Values)
                        {
                            if (o > 0 && o == v.Id)
                            {
                                var prop = new Prod.Prop();
                                prop.PId = p.Id;
                                prop.VId = v.Id;
                                prop.PName = p.Name;
                                prop.VName = v.Name;
                                prop.Extra = string.Empty;
                                if (p.IsSaleProp)
                                {
                                    prop.Extra = "sale";
                                }
                                if (p.IsColorProp)
                                {
                                    prop.Extra += " color";
                                }
                                if (p.Id == 20000)
                                {
                                    item.BrandId = v.Id;
                                    item.BrandName = v.Name;

                                    //item.BrandStr = new Prod.Brand(v.Id, "", v.Name).ToString();
                                }
                                props.Add(prop);

                                if (!p.IsSaleProp)
                                {
                                    propStrBuilder.Append(p.Id + ":" + v.Id + ":" + p.Name + ":" + v.Name + ";");
                                }
                                if (p.IsSearchProp)
                                {
                                    searchPropIdBuilder.Append(p.Id + ":" + v.Id + ";");
                                    searchPropStrBuilder.Append(p.Id + ":" + v.Id + ":" + p.Name + ":" + v.Name + ";");
                                }
                            }
                        }
                    }
                }
            }

            var skus = new List<Sku>();

            var saleProps = props.Where(x => x.Extra.Contains("sale")).ToList();

            var saleKeys = saleProps.OrderBy(x => x.PId).GroupBy(g => g.PId).Select(g => g.Select(x => g.Key + ":" + x.VId).ToArray()).ToList();

            var skuKeys = GetAllSkuKeys(saleKeys);
            if (skuKeys != null && skuKeys.Any())
            {
                foreach (var skuKey in skuKeys)
                {
                    if (skuKey.IsNullOrEmpty()) continue;

                    var stockParam = "sku.stock." + skuKey;
                    var priceParam = "sku.price." + skuKey;
                    var appPriceParam = "sku.appPrice." + skuKey;
                    var retailPriceParam = "sku.retailPrice." + skuKey;
                    var codeParam = "sku.code." + skuKey;
                    var barcodeParam = "sku.barcode." + skuKey;

                    decimal price, appPrice, retailPrice;

                    var stockVal = form[stockParam].ToString().Trim();
                    var priceVal = form[priceParam].ToString().Trim();
                    var appPriceVal = form[appPriceParam].ToString().Trim();
                    var codeVal = form[codeParam].ToString().Trim();
                    var barcodeVal = form[barcodeParam].ToString().Trim();
                    var retailPriceVal = form[retailPriceParam].ToString().Trim();

                    if (stockVal.IsNullOrEmpty() || !int.TryParse(stockVal, out int stock) || priceVal.IsNullOrEmpty() || !decimal.TryParse(priceVal, out price) || appPriceVal.IsNullOrEmpty() || !decimal.TryParse(appPriceVal, out appPrice) || retailPriceVal.IsNullOrEmpty() || !decimal.TryParse(retailPriceVal, out retailPrice))
                    {
                        continue;
                    }

                    var nvs = skuKey.Split(';');
                    var propstr = string.Empty;
                    foreach (var nv in nvs)
                    {
                        var array = nv.Split(':');
                        var pid = array[0].AsInt();
                        var vid = array[1].AsInt();
                        var pname = saleProps.First(o => o.PId == pid).PName;
                        var vname = saleProps.First(o => o.VId == vid).VName;
                        propstr += $"{pid}:{vid}:{pname}:{vname};";
                    }

                    var sku = new Sku();

                    sku.CatgId = catgId;
                    sku.CatgRId = catgRId;
                    sku.BrandId = item.BrandId;
                    sku.Stock = stock;
                    sku.Price = price;
                    sku.AppPrice = appPrice;
                    sku.RetailPrice = retailPrice;
                    sku.Code = codeVal ?? string.Empty;
                    sku.Barcode = barcodeVal ?? string.Empty;
                    sku.PropId = skuKey;
                    sku.PropStr = propstr.TrimEnd(';');
                    sku.SaleCount = 0;
                    sku.Status = itemStatus;
                    sku.SellerId = User.Id;
                    sku.CreatedOn = time;
                    sku.ModifiedBy = User.Name;
                    sku.ModifiedOn = time;

                    skus.Add(sku);
                }
            }


            var colorImgDic = new Dictionary<string, string>();
            var colorProps = props.Where(x => x.Extra.Contains("color")).ToList();
            foreach (var p in colorProps)
            {
                var key = p.PId + ":" + p.VId;
                var colorParam = "color.img." + key;
                var val = form[colorParam].ToString();

                if (!val.IsNullOrEmpty())
                {
                    colorImgDic.Add(key, val);
                }
            }

            var itemImgs = form["item.img"].ToArray().Where(x => !string.IsNullOrEmpty(x)).Take(5).ToArray();
            var picture = itemImgs.FirstOrDefault();
            var itemImgStr = string.Join(";", itemImgs);


            item.SpuId = spuId;
            item.CatgId = catgId;
            item.CatgRId = catgRId;
            item.Name = item.Name.StripHtml();
            item.Title = item.Title.StripHtml();
            item.Code = item.Code ?? string.Empty;
            item.Barcode = item.Barcode ?? string.Empty;
            item.Keyword = "";
            item.Summary = summary;
            item.Picture = picture;
            item.ItemImgStr = itemImgStr;
            item.PropImgStr = JsonConvert.SerializeObject(colorImgDic);
            item.Width = 0;
            item.Depth = 0;
            item.Height = 0;
            item.Weight = 0;
            item.Country = "";
            item.Location = "";
            item.IsBonded = false;
            item.IsOversea = false;
            item.IsTiming = isTiming;
            item.IsVirtual = false;
            item.IsAutoFill = false;
            item.SupportCod = false;
            item.FreightFree = false;
            item.FreightTplId = 1;

            item.SubStock = subStock;
            item.Showcase = 1;
            item.OnlineOn = time;
            item.OfflineOn = time.AddDays(15);
            item.RewardRate = 0.5m;
            item.HasInvoice = false;
            item.HasWarranty = false;
            item.HasGuarantee = false;

            item.SaleCount = 0;
            item.CollectCount = 0;
            item.ConsultCount = 0;
            item.CommentCount = 0;

            item.PropStr = propStrBuilder.ToString().TrimEnd(';');
            item.PropAlias = string.Empty;
            item.InputId = "";
            item.InputStr = "";
            item.SearchPropId = searchPropIdBuilder.ToString().TrimEnd(';');
            item.SearchPropStr = searchPropStrBuilder.ToString().TrimEnd(';');

            item.Meta = "";
            item.Label = "";
            item.Related = "";

            item.Prority = 0;
            item.Status = itemStatus;
            item.CreatedOn = time;
            item.ModifiedBy = "";
            item.ModifiedOn = time;

            item.Skus = skus;
            item.SellerId =  User.Id;
            item.SellerName =  User.Name;
            item.Desc.CreatedOn = time;
            item.Desc.ModifiedBy =User.Name;
            item.Desc.ModifiedOn = time;

            if (id > 0)
            {
                item.Id = id;
                var propIds = item.Skus.Select(x => x.PropId);

                var oldSkus = DefaultStorage.SkuList(id);

                var oldSkuPropIds = oldSkus.Select(x => x.PropId);

                var dic = oldSkus.ToDictionary(x => x.PropId, x => x.Id);
                foreach (var sku in item.Skus)
                {
                    if (dic.ContainsKey(sku.PropId))
                    {
                        sku.Id = dic[sku.PropId];//更新
                    }
                    else
                    {
                        sku.Id = 0;//新增
                    }
                }

                var deletedPropIds = oldSkuPropIds.Except(propIds);//删除的

                var deletedIds = oldSkus.Where(x => deletedPropIds.Contains(x.PropId)).Select(x => x.Id);

                result.Status = DefaultStorage.ItemUpdate(item, deletedIds.ToArray());
                return Json(result);
            }

            var newId = DefaultStorage.ItemCreate(item);
            result.Data = newId.ToString();
            result.Status = newId > 0;

            return Json(result);
        }

        [HttpGet("mini/{id}")]
        public IActionResult Mini(int id)
        {
            var model = DefaultStorage.ItemMiniGet(id);

            return PartialView(model);
        }

        [HttpPost("mini/{id}")]
        public IActionResult Mini(int id, ItemMini item)
        {
            var result = new Result();
            item.Id = id;
            item.SellerId = 1;
            item.Code = item.Code ?? string.Empty;
            item.Barcode = item.Barcode ?? string.Empty;
            if (item.Skus != null && item.Skus.Any())
            {
                item.Code = string.Empty;
                item.Barcode = string.Empty;
                item.Stock = item.Skus.Sum(x => x.Stock);
                item.Price = item.Skus.Min(x => x.Price);
                item.AppPrice = item.Skus.Min(x => x.AppPrice);
                item.RetailPrice = item.Skus.Min(x => x.RetailPrice);
            }
            result.Status = DefaultStorage.ItemMiniUpdate(item);
            return Json(result);
        }

        protected string[] GetAllSkuKeys(IList<string[]> keys)
        {
            var len = keys.Count;
            if (len < 1)
            {
                return null;
            }
            if (len == 1)
            {
                return keys[0];
            }
            if (len > 1)
            {
                var first = keys[0];
                var next = keys[1];
                var array = new List<string>();
                foreach (var t in first)
                {
                    array.AddRange(next.Select(x => t + ';' + x));
                }
                if (len == 2)
                {
                    return array.ToArray();
                }
                keys.RemoveAt(0);
                keys.RemoveAt(1);
                keys[0] = array.ToArray();

                keys[0] = GetAllSkuKeys(keys);
            }
            return keys[0];
        }

        /// <summary>
        /// 商品图片上传
        /// </summary>
        /// <param name="param"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("upload/{param}")]
        public async Task<ActionResult> Upload(string param, IFormFile image = null)
        {
            var result = new Result<string, string>();

            if (User)
            {
                var userId = User.Id;
                if (image != null)
                {
                    var fileName = image.FileName.ToLower();

                    var extension = Path.GetExtension(fileName);// fileName.Substring(fileName.LastIndexOf('.'));
                    var extensions = new[] { ".gif", ".png", ".jpg", ".jpeg" };
                    if (extensions.Contains(extension))
                    {
                        var size = image.Length;
                        if (size > 1024 * 1024)
                        {
                            result.Message = "图片类型只能为gif,png,jpg,jpeg，且大小不超过1M！";
                        }
                        else
                        {
                            var name = Guid.NewGuid() + extension;

                            try
                            {
                                var weedFs = WeedFs.Instance;

                                var pathName = string.Format("/{0}/default/{1}", userId, name);

                                var r = await weedFs.Upload(pathName, image.OpenReadStream());
                                if (r.Status)
                                {
                                    result.Status = true;
                                    result.Data = WebSite.Asset + "/" + r.Data.Replace(",", "/") + extension;
                                    result.Extra = param;
                                }

                            }
                            catch (Exception e)
                            {
                                result.Message = e.Message;
                            }
                        }
                    }
                    else
                    {
                        result.Message = "图片类型只能为gif,png,jpg,jpeg，且大小不超过1M！";
                    }
                }
                else
                {
                    result.Message = "请选择图片！";
                }
            }
            else
            {
                result.Message = "请登录！";
            }

            var data = JsonConvert.SerializeObject(result);
            var js = string.Format("<script type='text/javascript'>window.parent.ayatta.item.imageUploadCallback({0});</script>", data);
            return Content(js, "text/html");
        }
    }
}