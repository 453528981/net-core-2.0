using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;

namespace Ayatta.Web.Controllers
{
    [Route("sys")]
    public class SysController : BaseController
    {
        public SysController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SysController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        #region 国家

        [HttpGet("country-list")]
        public IActionResult CountryList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new CountryListModel();
            model.Keyword = keyword;
            model.Countries = DefaultStorage.CountryPagedList(page, size, keyword, status);
            return View(model);
        }

        [HttpGet("country-detail/{id?}")]
        public IActionResult CountryDetail(int? id)
        {
            var model = new CountryDetailModel();
            model.Country = new Country();
            if (id.HasValue && id.Value > 0)
            {
                model.Country = DefaultStorage.CountryGet(id.Value);
            }
            return View(model);
        }

        [HttpPost("country-detail/{i?}")]
        public async Task<IActionResult> CountryDetail(int? i, Country model)
        {
            var now = DateTime.Now;

            var result = new Result();
            if (!i.HasValue)
            {
                if (model.Id.IsNullOrEmpty())
                {
                    result.Error("请输入三位数字代码");
                    return Json(result);
                }
            }
            if (model.Code.IsNullOrEmpty())
            {
                result.Error("请输入三位字母代码");
                return Json(result);
            }

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入中文名称");
                return Json(result);
            }
            if (model.EnName.IsNullOrEmpty())
            {
                result.Error("请输入英文名称");
                return Json(result);
            }

            if (i.HasValue && i.Value > 0)
            {
                var old = DefaultStorage.CountryGet(i.Value);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.CountryUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }
            model.Id = i.ToString();
            model.Extra = string.Empty;
            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            result.Status = DefaultStorage.CountryCreate(model);
            if (result.Status)
            {
                result.Success();
            }

            return Json(result);
        }


        [HttpPost("country-delete/{id}")]
        public IActionResult CountryDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.CountryDelete(id);
            return Json(result);
        }
        #endregion

        #region 中国行政区
        [HttpGet("region-list")]
        public IActionResult RegionList()
        {
            //var model = new CountryListModel();
            //model.Countries = DefaultStorage.CountryList(page, size, keyword, status);
            return View();
        }

        [HttpGet("region-data")]
        public IActionResult RegionData()
        {
            var data = DefaultStorage.RegionList();
            var nodes = data.Select(x => new Node { Id = x.Id, Text = x.Name, Pid = x.ParentId }).ToHierarchy("CHN");
            return Json(nodes);
        }

        [HttpGet("region-detail/{id?}")]
        public IActionResult RegionDetail(string id)
        {
            var model = new Model<Region>();
            if (!id.IsNullOrEmpty())
            {
                model.Data = DefaultStorage.RegionGet(id);
            }
            return PartialView(model);
        }
        #endregion

        #region 银行

        [HttpGet("bank-list")]
        public IActionResult BankList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new BankListModel();
            model.Keyword = keyword;
            model.Banks = DefaultStorage.BankPagedList(page, size, keyword, status);
            return View(model);
        }

        [HttpGet("bank-detail/{id?}")]
        public IActionResult BankDetail(int? id)
        {
            var model = new BankDetailModel();
            model.Bank = new Bank();
            if (id.HasValue && id.Value > 0)
            {
                model.Bank = DefaultStorage.BankGet(id.Value);
            }
            return View(model);
        }

        [HttpPost("bank-detail/{id?}")]
        public async Task<IActionResult> BankDetail(int? id, Bank model)
        {
            var now = DateTime.Now;

            var result = new Result();


            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入中文名称");
                return Json(result);
            }

            if (id.HasValue && id.Value > 0)
            {
                var old = DefaultStorage.BankGet(id.Value);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.BankUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }

            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            var newId = DefaultStorage.BankCreate(model);
            result.Status = newId > 0;
            if (result.Status)
            {
                result.Success();
            }

            return Json(result);
        }


        [HttpPost("bank-delete/{id}")]
        public IActionResult BankDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.BankDelete(id);
            return Json(result);
        }
        #endregion

        #region 支付平台

        [HttpGet("payment-platform-list")]
        public IActionResult PaymentPlatformList(string keyword = null, bool? status = null)
        {
            var model = new PaymentPlatformListModel();
            model.Keyword = keyword;
            model.PaymentPlatforms = DefaultStorage.PaymentPlatformList(keyword, status);
            return View(model);
        }

        /*
        [HttpGet("payment-platform-data")]
        public IActionResult PaymentPlatformData()
        {
            var data = DefaultStorage.PaymentPlatformList();
            var nodes = data.Select(x => new Node { Id = x.Id.ToString(), Text = x.Name, ParentId = string.Empty }).ToHierarchy(string.Empty);
            return Json(nodes);
        }
        */

        [HttpGet("payment-platform-detail/{id?}")]
        public IActionResult PaymentPlatformDetail(int? id)
        {
            var model = new PaymentPlatformDetailModel();
            model.PaymentPlatform = new PaymentPlatform();
            if (id.HasValue && id.Value > 0)
            {
                model.PaymentPlatform = DefaultStorage.PaymentPlatformGet(id.Value);
            }
            return View(model);
        }

        [HttpPost("payment-platform-detail/{id?}")]
        public async Task<IActionResult> PaymentPlatformDetail(int? id, PaymentPlatform model)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }
            if (model.MerchantId.IsNullOrEmpty())
            {
                result.Error("请输入商户号");
                return Json(result);
            }
            if (model.GatewayUrl.IsNullOrEmpty())
            {
                result.Error("请输入网关URL");
                return Json(result);
            }
            if (model.CallbackUrl.IsNullOrEmpty())
            {
                result.Error("请输入回调RUL");
                return Json(result);
            }
            if (model.NotifyUrl.IsNullOrEmpty())
            {
                result.Error("请输入通知URL");
                return Json(result);
            }

            if (id.HasValue && id.Value > 0)
            {
                var old = DefaultStorage.PaymentPlatformGet(id.Value);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.PaymentPlatformUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }

            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            var newId = DefaultStorage.PaymentPlatformCreate(model);
            result.Status = newId > 0;
            if (result.Status)
            {
                result.Success();
            }
            return Json(result);
        }


        [HttpPost("payment-platform-delete/{id}")]
        public IActionResult PaymentPlatformDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.PaymentPlatformDelete(id);
            return Json(result);
        }
        #endregion

        #region 第三方登录

        [HttpGet("oauth-provider-list")]
        public IActionResult OAuthProviderList()
        {
            var model = new OAuthProviderListModel();

            model.OAuthProviders = DefaultStorage.OAuthProviderList();
            return View(model);
        }

        [HttpGet("oauth-provider-detail/{id?}")]
        public IActionResult OAuthProviderDetail(string id)
        {
            var model = new OAuthProviderDetailModel();
            model.OAuthProvider = new OAuthProvider();
            if (!id.IsNullOrEmpty())
            {
                model.OAuthProvider = DefaultStorage.OAuthProviderGet(id);
            }
            return View(model);
        }

        [HttpPost("oauth-provider-detail/{i?}")]
        public async Task<IActionResult> OAuthProviderDetail(string i, OAuthProvider model)
        {
            var now = DateTime.Now;
            var result = new Result();
            if (i.IsNullOrEmpty() && model.Id.IsNullOrEmpty())
            {
                result.Error("请输入Id");
                return Json(result);
            }
            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }
            if (model.ClientId.IsNullOrEmpty())
            {
                result.Error("请输入ClientId");
                return Json(result);
            }
            if (model.ClientSecret.IsNullOrEmpty())
            {
                result.Error("请输入ClientSecret");
                return Json(result);
            }

            if (!i.IsNullOrEmpty())
            {
                var old = DefaultStorage.OAuthProviderGet(i);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.OAuthProviderUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }
            var exist = DefaultStorage.OAuthProviderExist(model.Id.Trim());
            if (exist)
            {
                result.Message = "Id已存在";
                return Json(result);
            }

            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            result.Status = DefaultStorage.OAuthProviderCreate(model);

            if (result.Status)
            {
                result.Success();
            }

            return Json(result);
        }


        [HttpPost("oauth-provider-delete/{id}")]
        public IActionResult OAuthProviderDel(string id)
        {
            var result = new Result();
            result.Status = DefaultStorage.OAuthProviderDelete(id);
            return Json(result);
        }
        #endregion

        #region  Slide
        [HttpGet("slide-list")]
        public IActionResult SlideList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new SlideListModel();
            model.Keyword = keyword;
            model.Slides = DefaultStorage.SlidePagedList(page, size, keyword, status);
            return View(model);
        }

        [HttpGet("slide-detail/{id?}")]
        public IActionResult SlideDetail(string id)
        {
            var now = DateTime.Now;
            var model = new SlideDetailModel();
            model.Slide = new Slide();
            if (!id.IsNullOrEmpty())
            {
                model.Slide = DefaultStorage.SlideGet(id, false);
            }
            return View(model);
        }


        [HttpPost("slide-detail/{i?}")]
        public async Task<IActionResult> SlideDetail(string i, Slide model)
        {
            var now = DateTime.Now;
            var result = new Result();
            if (i.IsNullOrEmpty() && model.Id.IsNullOrEmpty())
            {
                result.Error("请输入Id");
                return Json(result);
            }
            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }
            if (model.Title.IsNullOrEmpty())
            {
                result.Error("请输入标题");
                return Json(result);
            }


            if (!i.IsNullOrEmpty())
            {
                var old = DefaultStorage.SlideGet(i, false);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.SlideUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }
            var exist = DefaultStorage.SlideExist(model.Id.Trim());
            if (exist)
            {
                result.Message = "Id已存在";
                return Json(result);
            }

            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            result.Status = DefaultStorage.SlideCreate(model);

            if (result.Status)
            {
                result.Success();
            }

            return Json(result);
        }


        [HttpPost("slide-delete/{id}")]
        public IActionResult SlideDel(string id)
        {
            var result = new Result();
            result.Status = DefaultStorage.SlideDelete(id);
            return Json(result);
        }

        [HttpGet("slide-item-list/{slideId}")]
        public IActionResult SlideItemList(string slideId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new SlideItemListModel();
            model.SlideId = slideId;
            model.Keyword = keyword;
            model.Items = DefaultStorage.SlideItemPagedList(slideId, page, size, keyword, status);
            return View(model);
        }

        [HttpGet("slide/{slideId}/item/{id?}")]
        public IActionResult SlideItemDetail(string slideId, int id)
        {
            var now = DateTime.Now;
            var model = new SlideItemDetailModel();
            model.SlideItem = new SlideItem();
            model.SlideItem.StartedOn = now;
            model.SlideItem.StoppedOn = now.AddDays(5);
            if (id > 0)
            {
                model.SlideItem = DefaultStorage.SlideItemGet(id);
            }
            return View(model);
        }

        [HttpPost("slide/{slideId}/item/{id?}")]
        public async Task<IActionResult> SlideItemDetail(string slideId, int id, SlideItem model)
        {
            var now = DateTime.Now;
            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }
            if (model.Title.IsNullOrEmpty())
            {
                result.Error("请输入标题");
                return Json(result);
            }

            if (id > 0)
            {
                var old = DefaultStorage.SlideItemGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.SlideItemUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }


            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            var newId = DefaultStorage.SlideItemCreate(model);
            result.Status = newId > 0;

            if (result.Status)
            {
                result.Success();
            }

            return Json(result);
        }

        #endregion

        #region Help

        [HttpGet("help-list")]
        public IActionResult HelpList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new HelpListModel();
            model.Keyword = keyword;
            model.Helps = DefaultStorage.HelpPagedList(page, size, keyword, status);
            return View(model);
        }

        [HttpGet("help-detail/{id?}")]
        public IActionResult HelpDetail(int? id)
        {
            var model = new HelpDetailModel();
            model.Help = new Help();
            if (id.HasValue && id.Value > 0)
            {
                model.Help = DefaultStorage.HelpGet(id.Value);
            }
            return View(model);
        }

        [HttpPost("help-detail/{id?}")]
        public async Task<IActionResult> HelpDetail(int? id, Help model, int pid = 0)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Title.IsNullOrEmpty())
            {
                result.Error("请输入中文名称");
                return Json(result);
            }

            if (!model.Content.IsNullOrEmpty())
            {
                model.Content = model.Content.RemoveHtml();
            }

            if (id.HasValue && id.Value > 0 && pid == 0)
            {
                var old = DefaultStorage.HelpGet(id.Value);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.HelpUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }

            if (id.HasValue && id.Value == 0 && pid == 0)
            {
                result.Message = "参数错误";
                return Json(result);

            }
            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;

            var hs = DefaultStorage.HelpPidList(pid);

            model.Depth = hs.Count + 1;
            model.Path = string.Join(",", hs);

            var newId = DefaultStorage.HelpCreate(model);

            if (result.Status = newId > 0)
            {
                hs.Add(newId);//补全path
                var path = string.Join(",", hs);
                DefaultStorage.HelpPathUpdate(newId, path);
                result.Success();
            }

            return Json(result);
        }


        [HttpPost("help-delete/{id}")]
        public IActionResult HelpDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.HelpDelete(id);
            return Json(result);
        }
        #endregion

        #region Module
        [HttpGet("module-list")]
        public IActionResult ModuleList()
        {
            return View();
        }

        [HttpGet("module-data")]
        public IActionResult ModuleData()
        {
            var data = DefaultStorage.ModuleList();
            var nodes = data.Select(x => new Node { Id = x.Id.ToString(), Text = x.Name, Pid = x.Pid.ToString() }).ToHierarchy("0");
            return Json(nodes);
        }

        [HttpGet("module-detail/{id?}")]
        public IActionResult ModuleDetail(int id, int pid = 0)
        {
            var data = new RBAC.Module();
            var model = new Model<RBAC.Module, int>(data, pid);

            if (id > 0)
            {
                model.Data = DefaultStorage.ModuleGet(id);
            }
            return PartialView(model);
        }

        [HttpPost("module-detail/{id}")]
        public async Task<IActionResult> ModuleDetail(int id, RBAC.Module model, int pid = 0)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }

            if (id > 0 && pid == 0)
            {
                var old = DefaultStorage.ModuleGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.ModuleUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }

            if (id == 0 && pid == 0)
            {
                result.Message = "参数错误";
                return Json(result);

            }
            model.Pid = pid;


            var hs = GetParendIds(pid);
            model.Depth = hs.Count + 1;
            model.Path = string.Join(",", hs);

            var newId = DefaultStorage.ModuleCreate(model);

            if (result.Status = newId > 0)
            {
                hs.Add(newId);//补全path
                var path = string.Join(",", hs);
                DefaultStorage.ModulePathUpdate(newId, path);
                result.Success();
            }

            return Json(result);
        }

        private IList<int> GetParendIds(int id)
        {
            var dic = DefaultStorage.ModuleIdDic();
            var hs = new HashSet<int>();
            hs.Add(id);
            var i = id;
            while (i > 0)
            {
                i = dic[i];
                if (i > 0)
                {
                    hs.Add(i);
                }
            }
            return hs.Reverse().ToList();
        }

        [HttpGet("module/{moduleId}/func-list")]
        public IActionResult FuncList(int moduleId)
        {
            var data = DefaultStorage.FuncList(moduleId);
           
            var model = new Model<IList<RBAC.Func>, int>(data, moduleId);
            
            return PartialView(model);
        }

        [HttpGet("module/{moduleId}/func-detail/{id?}")]
        public IActionResult FuncDetail(string id, int moduleId)
        {
            var data = new RBAC.Func();
            
            var model = new Model<RBAC.Func, int>(data, moduleId);

            if (!id.IsEmpty())
            {
                model.Data = DefaultStorage.FuncGet(id);
            }
            return PartialView(model);
        }

        [HttpPost("module/{moduleId}/func-detail/{id?}")]
        public async Task<IActionResult> FuncDetail(string id, RBAC.Func model, int moduleId = 0)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }

            if (!id.IsEmpty() && moduleId == 0)
            {
                var old = DefaultStorage.FuncGet(id);
                if (old == null)
                {
                    result.Error("数据不存在");
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.FuncUpdate(old);
                    if (!result.Status)
                    {
                        result.Error("更新失败");
                        return Json(result);
                    }
                }
                else
                {
                    result.Error("参数有误");
                    return Json(result);
                }
                return Json(result);
            }

            if (id.IsEmpty() && moduleId == 0)
            {
                result.Message = "参数错误";
                return Json(result);

            }

            model.Mid = moduleId;

            result.Status = DefaultStorage.FuncCreate(model);
            if (!result)
            {
                result.Error("添加失败");
                return Json(result);
            }
            return Json(result);
        }

        #endregion
    }
}