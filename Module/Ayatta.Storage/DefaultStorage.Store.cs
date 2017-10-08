using Dapper;
using System;
using System.Text;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage.Param;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        #region 商品

        /// <summary>
        /// 商品创建
        /// </summary>
        /// <param name="o">商品</param>
        /// <returns></returns>
        public int ItemCreate(Item o)
        {
            return Try(nameof(ItemCreate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Insert("Item")
                            .Column("SpuId", o.SpuId)
                            .Column("CatgId", o.CatgId)
                            .Column("CatgRId", o.CatgRId)
                            .Column("CatgFId", o.CatgFId ?? string.Empty)
                            .Column("CatgFName", o.CatgFName ?? string.Empty)
                            .Column("BrandId", o.BrandId)
                            .Column("BrandName", o.BrandName)
                            .Column("BrandInitial", o.BrandInitial ?? string.Empty)
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code)
                            .Column("Barcode", o.Barcode)
                            .Column("Keyword", o.Keyword)
                            .Column("Summary", o.Summary)
                            .Column("Picture", o.Picture)
                            .Column("ItemImgStr", o.ItemImgStr)
                            .Column("PropImgStr", o.PropImgStr)
                            .Column("Width", o.Width)
                            .Column("Depth", o.Depth)
                            .Column("Height", o.Height)
                            .Column("Weight", o.Weight)
                            .Column("Country", o.Country)
                            .Column("Location", o.Location)
                            .Column("IsBonded", o.IsBonded)
                            .Column("IsOversea", o.IsOversea)
                            .Column("IsTiming", o.IsTiming)
                            .Column("IsVirtual", o.IsVirtual)
                            .Column("IsAutoFill", o.IsAutoFill)
                            .Column("SupportCod", o.SupportCod)
                            .Column("FreightFree", o.FreightFree)
                            .Column("FreightTplId", o.FreightTplId)
                            .Column("SubStock", o.SubStock)
                            .Column("Showcase", o.Showcase)
                            .Column("OnlineOn", o.OnlineOn)
                            .Column("OfflineOn", o.OfflineOn)
                            .Column("RewardRate", o.RewardRate)
                            .Column("HasInvoice", o.HasInvoice)
                            .Column("HasWarranty", o.HasWarranty)
                            .Column("HasGuarantee", o.HasGuarantee)
                            .Column("SaleCount", o.SaleCount)
                            .Column("CollectCount", o.CollectCount)
                            .Column("ConsultCount", o.ConsultCount)
                            .Column("CommentCount", o.CommentCount)
                            .Column("PropStr", o.PropStr)
                            .Column("PropAlias", o.PropAlias)
                            .Column("InputId", o.InputId)
                            .Column("InputStr", o.InputStr)
                            .Column("SearchPropId", o.SearchPropId ?? string.Empty)
                            .Column("SearchPropStr", o.SearchPropStr ?? string.Empty)
                            .Column("Meta", o.Meta)
                            .Column("Label", o.Label)
                            .Column("Related", o.Related)
                            .Column("Prority", o.Prority)
                            .Column("SellerId", o.SellerId)
                            .Column("SellerName", o.SellerName)
                            .Column("Status", o.Status)
                            .Column("CreatedOn", o.CreatedOn)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)

                            .ToCommand(true, tran);

                            var status = true;
                            var id = conn.ExecuteScalar<int>(cmd);

                            if (id > 0)
                            {

                                cmd = SqlBuilder.Insert("ItemDesc")
                                .Column("Id", id)
                                .Column("Detail", o.Desc.Detail)
                                .Column("Manual", o.Desc.Manual)
                                .Column("Photo", o.Desc.Photo)
                                .Column("Story", o.Desc.Story)
                                .Column("Notice", o.Desc.Notice)
                                .Column("CreatedOn", o.Desc.CreatedOn)
                                .Column("ModifiedBy", o.Desc.ModifiedBy)
                                .Column("ModifiedOn", o.Desc.ModifiedOn)
                                .ToCommand(false, tran);
                                status = conn.Execute(cmd) > 0;
                                if (status)
                                {
                                    foreach (var sku in o.Skus)
                                    {
                                        cmd = SqlBuilder.Insert("Sku")
                                        .Column("SpuId", sku.SpuId)
                                        .Column("ItemId", id)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("SaleCount", sku.SaleCount)
                                        .Column("SellerId", sku.SellerId)
                                        .Column("Status", sku.Status)
                                        .Column("CreatedOn", sku.CreatedOn)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .ToCommand(false, tran);

                                        status = conn.Execute(cmd) > 0;
                                        if (!status)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (id > 0 && status)
                            {
                                tran.Commit();
                                return id;
                            }
                            tran.Rollback();
                            return 0;
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            throw e;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 商品更新
        /// </summary>
        /// <param name="o">商品</param>
        /// <param name="deletedIds">待删除的Sku Id集合</param>
        /// <returns></returns>
        public bool ItemUpdate(Item o, int[] deletedIds)
        {
            return Try(nameof(ItemUpdate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Update("Item")
                            .Column("SpuId", o.SpuId)
                            .Column("CatgId", o.CatgId)
                            .Column("CatgRId", o.CatgRId)
                            .Column("CatgFId", o.CatgFId ?? string.Empty)
                            .Column("CatgFName", o.CatgFName ?? string.Empty)
                            .Column("BrandId", o.BrandId)
                            .Column("BrandName", o.BrandName)
                            .Column("BrandInitial", o.BrandInitial ?? string.Empty)
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code)
                            .Column("Barcode", o.Barcode)
                            .Column("Keyword", o.Keyword)
                            .Column("Summary", o.Summary)
                            .Column("Picture", o.Picture)
                            .Column("ItemImgStr", o.ItemImgStr)
                            .Column("PropImgStr", o.PropImgStr)
                            .Column("Width", o.Width)
                            .Column("Depth", o.Depth)
                            .Column("Height", o.Height)
                            .Column("Weight", o.Weight)
                            .Column("Country", o.Country)
                            .Column("Location", o.Location)
                            .Column("IsBonded", o.IsBonded)
                            .Column("IsOversea", o.IsOversea)
                            .Column("IsTiming", o.IsTiming)
                            .Column("IsVirtual", o.IsVirtual)
                            .Column("IsAutoFill", o.IsAutoFill)
                            .Column("SupportCod", o.SupportCod)
                            .Column("FreightFree", o.FreightFree)
                            .Column("FreightTplId", o.FreightTplId)
                            .Column("SubStock", o.SubStock)
                            .Column("Showcase", o.Showcase)
                            .Column("OnlineOn", o.OnlineOn)
                            .Column("OfflineOn", o.OfflineOn)
                            .Column("RewardRate", o.RewardRate)
                            .Column("HasInvoice", o.HasInvoice)
                            .Column("HasWarranty", o.HasWarranty)
                            .Column("HasGuarantee", o.HasGuarantee)
                            //.Column("SaleCount",o.SaleCount)
                            //.Column("CollectCount",o.CollectCount)
                            //.Column("ConsultCount",o.ConsultCount)
                            //.Column("CommentCount",o.CommentCount)
                            .Column("PropStr", o.PropStr)
                            .Column("PropAlias", o.PropAlias)
                            .Column("InputId", o.InputId)
                            .Column("InputStr", o.InputStr)
                            .Column("SearchPropId", o.SearchPropId ?? string.Empty)
                            .Column("SearchStr", o.SearchPropStr ?? string.Empty)
                            .Column("Meta", o.Meta)
                            .Column("Label", o.Label)
                            .Column("Related", o.Related)
                            .Column("Prority", o.Prority)
                            .Column("Status", o.Status)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)
                            .Where("Id=@id and SellerId=@sellerId", new { o.Id,o.SellerId })

                            .ToCommand(tran);

                            var status = conn.Execute(cmd) > 0;

                            if (status)
                            {
                                cmd = SqlBuilder.Update("ItemDesc")
                                .Column("Detail", o.Desc.Detail)
                                .Column("Manual", o.Desc.Manual)
                                .Column("Photo", o.Desc.Photo)
                                .Column("Story", o.Desc.Story)
                                .Column("Notice", o.Desc.Notice)
                                .Column("ModifiedBy", o.Desc.ModifiedBy)
                                .Column("ModifiedOn", o.Desc.ModifiedOn)
                                .Where("Id=@id", new { o.Id })
                                .ToCommand(tran);
                                status = conn.Execute(cmd) > 0;

                                if (status && o.Skus.Any())
                                {
                                    foreach (var sku in o.Skus.Where(x => x.Id == 0))
                                    {
                                        cmd = SqlBuilder.Insert("Sku")

                                        .Column("SpuId", sku.SpuId)
                                        .Column("ItemId", o.Id)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("SaleCount", sku.SaleCount)
                                        .Column("Status", sku.Status)
                                        .Column("SellerId", o.SellerId)
                                        .Column("CreatedOn", sku.CreatedOn)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .ToCommand(false, tran);

                                        status = conn.Execute(cmd) > 0;

                                        if (!status)
                                        {
                                            break;
                                        }
                                    }
                                    foreach (var sku in o.Skus.Where(x => x.Id > 0))
                                    {
                                        cmd = SqlBuilder.Update("Sku")
                                        .Column("SpuId", sku.SpuId)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("Status", sku.Status)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .Where("Id=@Id", new { sku.Id })
                                        .ToCommand(tran);

                                        conn.Execute(cmd);
                                    }
                                }
                                var sql = "update Sku set stock=0,status=2 where id in @deletedIds";
                                cmd = SqlBuilder.Raw(sql, new { deletedIds }).ToCommand(tran);
                                conn.Execute(cmd);
                            }
                            if (status)
                            {
                                tran.Commit();
                                return true;
                            }
                            tran.Rollback();
                            return false;
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 商品获取
        /// </summary>
        /// <param name="id">商品id</param>
        /// <param name="includeSkus">是否包含Sku</param>
        /// <returns></returns>
        public Item ItemGet(int id, bool includeSkus = false)
        {
            return Try(nameof(ItemGet), () =>
            {
                if (includeSkus)
                {
                    var sql = @"select * from item where id=@id;select * from sku where itemid=@id;";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = StoreConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Item>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Skus = reader.Read<Sku>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var cmd = SqlBuilder
                    .Select("*").From("item")
                    .Where("id=@id", new { id })
                    .ToCommand();
                    return StoreConn.QueryFirstOrDefault<Item>(cmd);
                }
            });
        }

        public IPagedList<Item.Tiny> ItemTinyList(int page = 1, int size = 20, string keyword = null, int? catgId = null, int? brandId = null, int? sellerId = null, Prod.Status[] status = null, int[] include = null, int[] exclude = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            var sb = SqlBuilder
            .Select("id,catgid,brandid,brandname,code,name,stock,price,retailprice,barcode,keyword,summary,picture")
            .From("item");

            if (!string.IsNullOrEmpty(keyword))
            {
                if (Regex.IsMatch(keyword, "^\\d+$"))
                {
                    sb.Where("Id=@id", new { id = keyword });
                }
                else
                {
                    sb.Where("Name LIKE @Name", new { name = keyword + "%" });
                }
            }
            if (catgId.HasValue && catgId.Value > 0)
            {
                sb.Where("CatgId=@CatgId", new { catgId });
            }
            if (brandId.HasValue && brandId.Value > 0)
            {
                sb.Where("BrandId=@brandId", new { brandId });
            }
            if (sellerId.HasValue)
            {
                sb.Where("SellerId=@sellerId", new { sellerId });
            }
            if (status != null)
            {
                if (status.Length == 1 && status[0] > 0)
                {
                    sb.Where("Status=@status", new { id = include[0] });
                }
                if (status.Length > 1)
                {
                    var str = string.Join(",", status);
                    sb.Where("Status in (" + str + ")");
                }
            }
            if (include != null)
            {
                if (include.Length == 1 && include[0] > 0)
                {
                    sb.Where("Id=@id", new { id = include[0] });
                }
                if (include.Length > 1)
                {
                    var str = string.Join(",", include);
                    sb.Where("Id in (" + str + ")");
                }
            }
            if (exclude != null)
            {
                if (exclude.Length == 1 && exclude[0] > 0)
                {
                    sb.Where("Id<>@id", new { id = exclude[0] });
                }
                if (exclude.Length > 1)
                {
                    var str = string.Join(",", include);
                    sb.Where("Id not in (" + str + ")");
                }
            }

            //sb.OrderBy("Id DESC");

            var cmd = sb.ToCommand(page, size);

            return StoreConn.PagedList<Item.Tiny>(page, size, cmd);
        }

        /*
        public IPagedList<Item> ItemSearch(ProdItemSearchParam param)
        {
            var sb = SqlBuilder
            .Select("*")
            .From("item");
            if (param.UserId.HasValue)
            {
                sb.Where("SellerId=@userid", new { param.UserId });
            }
            if (param.Id > 0)
            {
                sb.Where("Id=@id", new { param.Id });
            }
            if (!string.IsNullOrEmpty(param.Name))
            {
                sb.Where("Name LIKE @Name", new { name = param.Name + "%" });
            }
            if (!string.IsNullOrEmpty(param.Code))
            {
                sb.Where("Code=@code", new { param.Code });
            }
            if (param.CRId > 0)
            {
                sb.Where("CatgRId=@CatgRId", new { CatgRId = param.CRId });
            }
            if (param.PriceFrom > 0)
            {
                sb.Where("Price>=@PriceFrom", new { param.PriceFrom });
            }
            if (param.PriceEnd > 0)
            {
                sb.Where("Price<=@PriceEnd", new { param.PriceEnd });
            }
            if (param.SaleFrom > 0)
            {
                sb.Where("SaleCount>=@SaleFrom", new { param.SaleFrom });
            }
            if (param.SaleEnd > 0)
            {
                sb.Where("SaleCount>=@SaleEnd", new { param.SaleEnd });
            }
            if (param.OrderBy != null && param.OrderBy.Count > 0)
            {
                var tmp = new StringBuilder();
                foreach (var o in param.OrderBy)
                {
                    tmp.AppendFormat("[{0}] {1},", o.Key, o.Value ? "ASC" : "DESC");
                }
                sb.OrderBy(sb.ToString().TrimEnd(','));
            }
            else
            {
                sb.OrderBy("Id DESC");
            }
            var cmd = sb.ToCommand(param.PageIndex, param.PageSize);

            return StoreConn.PagedList<Item>(param.PageIndex, param.PageSize, cmd);

        }
        */
        #endregion

        #region 商品Sku
        /// <summary>
        /// 商品Sku获取
        /// </summary>
        /// <param name="id">商品Sku id</param>
        /// <returns></returns>
        public Sku SkuGet(int id)
        {
            return Try(nameof(SkuGet), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("sku")
                .Where("id=@id", new { id })
                .ToCommand();
                return StoreConn.QueryFirstOrDefault<Sku>(cmd);
            });
        }

        /// <summary>
        /// 商品Sku获取
        /// </summary>
        /// <param name="itemId">商品id</param>
        /// <returns></returns>
        public IList<Sku> SkuList(int itemId)
        {
            return Try(nameof(SkuList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("sku")
                .Where("ItemId=@ItemId", new { itemId })
                .ToCommand();
                return StoreConn.Query<Sku>(cmd).ToList();
            });
        }

        #endregion

        #region 商品描述
        /// <summary>
        ///商品描述获取
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public ItemDesc ItemDescGet(int id)
        {
            return Try(nameof(ItemDescGet), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("itemdesc")
                .Where("id=@id", new { id })
                .ToCommand();
                return StoreConn.QueryFirstOrDefault<ItemDesc>(cmd);
            });
        }
        #endregion

        #region 商品评价

        ///<summary>
        /// 商品评价汇总摘要 是否存在
        ///</summary>
        ///<param name="id">商品id</param>
        ///<returns></returns>
        public bool ItemCommentExist(int id)
        {
            return Try(nameof(ItemCommentExist), () =>
            {
                var cmd = SqlBuilder.Select("1")
                .From("ItemComment")
                .Where("Id=@id", new { id })
                .ToCommand();

                return StoreConn.ExecuteScalar<bool>(cmd);
            });
        }

        ///<summary>
        /// 商品评价汇总摘要 获取
        ///</summary>
        ///<param name="id">商品id</param>
        ///<returns></returns>
        public ItemComment ItemCommentGet(int id)
        {
            return Try(nameof(ItemCommentGet), () =>
            {
                var sql = @"select * from ItemComment where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.QueryFirstOrDefault<ItemComment>(cmd);
            });
        }

        /// <summary>
        /// 商品评价汇总摘要 创建
        /// </summary>
        ///<param name="id">商品id</param>
        /// <returns></returns>
        public bool ItemCommentCreate(int id)
        {
            return Try(nameof(ItemCommentCreate), () =>
            {
                var cmd = SqlBuilder.Insert("ItemComment")
                .Column("Id", id)
                .Column("ImgCount", 0)
                .Column("SumCount", 0)
                .Column("CountA", 0)
                .Column("CountB", 0)
                .Column("CountC", 0)
                .Column("CountD", 0)
                .Column("CountE", 0)
                .Column("TagData", string.Empty)
                .Column("CreatedOn", DateTime.Now)
                .Column("ModifiedOn", string.Empty)
                .ToCommand();

                return StoreConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 商品评价 创建
        ///</summary>
        ///<param name="o">商品评价详情</param>
        ///<returns></returns>
        public int CommentCreate(Comment o)
        {
            return Try(nameof(CommentCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Comment")
                .Column("Score", o.Score)
                .Column("Content", o.Content ?? string.Empty)
                .Column("OrderId", o.OrderId)
                .Column("ItemId", o.ItemId)
                .Column("ItemImg", o.ItemImg)
                .Column("ItemName", o.ItemName)
                .Column("SkuId", o.SkuId)
                .Column("SkuProp", o.SkuProp)
                .Column("TagData", o.TagData ?? string.Empty)
                .Column("ImageData", o.ImageData ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("LikeCount", o.LikeCount)
                .Column("ReplyCount", o.ReplyCount)
                .Column("RewardPoint", o.RewardPoint)
                .Column("Reply", o.Reply ?? string.Empty)
                .Column("ReplyTime", o.ReplyTime)
                .Column("Append", o.Append ?? string.Empty)
                .Column("AppendTime", o.AppendTime)
                .Column("UserId", o.UserId)
                .Column("UserName", o.UserName)
                .Column("SellerId", o.SellerId)
                .Column("SellerName", o.SellerName)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy ?? string.Empty)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 商品评价 分页
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="skuId"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IPagedList<Comment> CommentPagedList(int itemId = 0, int skuId = 0, int type = 0, int page = 1, int size = 20)
        {
            return Try(nameof(CommentPagedList), () =>
            {
                var cmd = SqlBuilder.Select("*").From("Comment")
                .Where(itemId > 0, "ItemId=@itemId", new { itemId })
                .Where(skuId > 0, "SkuId=@skuId", new { skuId })

                .ToCommand(page, size);
                return TradeConn.PagedList<Comment>(page, size, cmd);
            });
        }

        ///<summary>
        /// 评价回复 创建
        ///</summary>
        ///<param name="o">评价回复</param>
        ///<returns></returns>
        public int CommentReplyCreate(CommentReply o)
        {
            return Try(nameof(CommentReplyCreate), () =>
            {
                var cmd = SqlBuilder.Insert("CommentReply")
                .Column("Pid", o.Pid)
                .Column("Path", o.Path)
                .Column("Depth", o.Depth)
                .Column("ItemId", o.ItemId)
                .Column("CommentId", o.CommentId)
                .Column("Content", o.Content ?? string.Empty)
                .Column("UserId", o.UserId)
                .Column("UserName", o.UserName)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy ?? string.Empty)
                .Column("CreatedOn", o.CreatedOn)
                .ToCommand(true);
                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }


        #endregion

        #region 商品咨询
        ///<summary>
        /// 商品咨询创建
        ///</summary>
        ///<param name="o">商品咨询</param>
        ///<returns></returns>
        public int ConsultationCreate(Consultation o)
        {
            return Try(nameof(ConsultationCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Consultation")
                .Column("SkuId", o.SkuId)
                .Column("ItemId", o.ItemId)
                .Column("GroupId", o.GroupId)
                .Column("UserId", o.UserId)
                .Column("UserName", o.UserName)
                .Column("Question", o.Question)
                .Column("Reply", o.Reply)
                .Column("ReplyFlag", o.ReplyFlag)
                .Column("Replier", o.Replier)
                .Column("RepliedOn", o.RepliedOn)
                .Column("SellerId", o.SellerId)
                .Column("SellerName", o.SellerName)
                .Column("Useful", o.Useful)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);

                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 商品咨询回复
        /// </summary>
        /// <param name="id">商品咨询id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="reply">回复内容</param>
        /// <param name="replier">回复者</param>
        /// <param name="replyFlag">回复处理标识</param>
        /// <returns></returns>
        public bool ConsultationReply(int id, int sellerId, string reply, string replier, byte replyFlag)
        {
            return Try(nameof(ConsultationReply), () =>
            {
                var cmd = SqlBuilder.Update("Consultation")
                .Column("Reply", reply)
                .Column("ReplyFlag", replyFlag)
                .Column("Replier", replier)
                .Column("RepliedOn", DateTime.Now)
                .Column("Status", 0)
                .Where("Id=@id and SellerId=@sellerId", new { id, sellerId })
               .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        #endregion

        #region ItemMini
        public ItemMini ItemMiniGet(int id)
        {
            return Try(nameof(ItemMiniGet), () =>
            {
                var sql = @"select Id,SpuId,CatgRId,CatgId,BrandId,BrandName,Name,Title,Stock,Price,AppPrice,RetailPrice,Code,Barcode,Keyword,Summary,Picture,ItemImgStr,PropImgStr,Status,PropStr,InputId,InputStr,SellerId from item where id=@id;
                select Id,Stock,Price,AppPrice,RetailPrice,Code,Barcode,PropId,PropStr,Status from sku where itemid=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                using (var reader = StoreConn.QueryMultiple(cmd))
                {
                    var o = reader.Read<ItemMini>().FirstOrDefault();
                    if (o != null)
                    {
                        o.Skus = reader.Read<ItemMini.Sku>().ToList();
                    }
                    return o;
                }
            });
        }

        public bool ItemMiniUpdate(ItemMini o)
        {
            return Try(nameof(ItemMiniUpdate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder
                            .Update("Item")
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code ?? string.Empty)
                            .Column("Barcode", o.Barcode ?? string.Empty)
                            .Where("Id=@id and SellerId=@sellerId", new { o.Id, o.SellerId })
                            .ToCommand(tran);

                            var v = conn.Execute(cmd) > 0;

                            if (v && o.Skus != null && o.Skus.Any())
                            {
                                foreach (var sku in o.Skus)
                                {
                                    cmd = SqlBuilder
                                    .Update("Sku")
                                    .Column("Stock", sku.Stock)
                                    .Column("Price", sku.Price)
                                    .Column("AppPrice", o.AppPrice)
                                    .Column("RetailPrice", o.RetailPrice)
                                    .Column("Code", sku.Code ?? string.Empty)
                                    .Column("Barcode", sku.Barcode ?? string.Empty)
                                    .Where("Id=@id and ItemId=@itemId and SellerId=@sellerId", new { sku.Id, itemId = o.Id, o.SellerId })
                                    .ToCommand(tran);

                                    v = conn.Execute(cmd) > 0;

                                    if (!v)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (v)
                            {
                                tran.Commit();
                                return true;
                            }
                            tran.Rollback();
                            return false;
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            });
        }

        #endregion

        /*
        public Prod.Current ProdCurrentGet(int id, Platform Platform)
        {
            var o = new Prod.Current();
            var sql = @"select Id,Stock,Price,AppPrice,RetailPrice,SellerId from item where id=@id;
            select Id,Stock,Price,AppPrice,RetailPrice from sku where itemid=@id;";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            using (var reader = StoreConn.QueryMultiple(cmd))
            {
                o = reader.Read<Prod.Current>().FirstOrDefault();
                if (o != null)
                {
                    o.Skus = reader.Read<Prod.Current.Sku>().ToList();
                }
            }

            sql = @"
            select b.Id,b.Platform,a.ItemId,a.Global,a.Value,a.SkuJson
            from SpecialPriceItem a inner join SpecialPrice b
            on a.ParentId=b.Id
            where a.Status=1 and b.Status=1 
            and b.StartedOn<now() and b.StoppedOn>now()
            and b.SellerId=@sellerId
            ";
            var specials = PromotionConn.Query<Prod.Current.Special>(sql, new { o.SellerId }).ToList();

            return o;


        }
    */

        #region 官方活动计划

        ///<summary>
        /// 官方活动计划 创建
        ///</summary>
        ///<param name="o">ActPlan</param>
        ///<returns></returns>
        public int ActPlanCreate(ActPlan o)
        {
            return Try(nameof(ActPlanCreate), () =>
            {
                var cmd = SqlBuilder.Insert("ActPlan")
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("WarmUp", o.WarmUp)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("OpendOn", o.OpendOn)
                .Column("ClosedOn", o.ClosedOn)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 官方活动计划 更新
        ///</summary>
        ///<param name="o">ActPlan</param>
        ///<returns></returns>
        public bool ActPlanUpdate(ActPlan o)
        {
            return Try(nameof(ActPlanUpdate), () =>
            {
                var cmd = SqlBuilder.Update("ActPlan")
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("WarmUp", o.WarmUp)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("OpendOn", o.OpendOn)
                .Column("ClosedOn", o.ClosedOn)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 官方活动计划 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ActPlanStatusUpdate(int id, bool status)
        {
            return Try(nameof(ActPlanStatusUpdate), () =>
            {
                var sql = @"update ActPlan set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 官方活动计划 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ActPlanDelete(int id)
        {
            return Try(nameof(ActPlanDelete), () =>
            {
                var sql = @"delete from ActItem where PlanId=@id;delete from ActPlan where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 官方活动计划 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public ActPlan ActPlanGet(int id)
        {
            return Try(nameof(ActPlanGet), () =>
            {
                var sql = @"select * from ActPlan where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.QueryFirstOrDefault<ActPlan>(cmd);
            });
        }

        /// <summary>
        /// 官方活动计划 获取
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeItems">是否包含条目</param>
        /// <param name="current">是否筛选当前时间生效的条目</param>
        /// <returns></returns>
        public ActPlan ActPlanGet(string id, bool includeItems, bool current = false)
        {
            return Try(nameof(ActPlanGet), () =>
            {
                if (includeItems)
                {
                    var sql = @"select * from ActPlan where id=@id;select * from ActItem where PlanId=@id and Status=1";

                    var cmd = SqlBuilder.Raw(sql, new { id })
                    .Append(current, "StartedOn<=now() and StoppedOn>=now()")
                    .Append(";")
                    .ToCommand();
                    using (var reader = StoreConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<ActPlan>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<ActItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var sql = @"select * from ActPlan where id=@id";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    return StoreConn.QueryFirstOrDefault<ActPlan>(cmd);
                }
            });
        }

        /// <summary>
        /// 官方活动计划 分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<ActPlan> ActPlanPagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(ActPlanPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("ActPlan")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand(page, size);

                return StoreConn.PagedList<ActPlan>(page, size, cmd);
            });
        }

        ///<summary>
        /// 官方活动条目 创建
        ///</summary>
        ///<param name="o">ActItem</param>
        ///<returns></returns>
        public int ActItemCreate(ActItem o)
        {
            return Try(nameof(ActItemCreate), () =>
            {
                var cmd = SqlBuilder.Insert("ActItem")
                .Column("Type", o.Type)
                .Column("PlanId", o.PlanId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title??string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("DataKey", o.DataKey ?? string.Empty)
                .Column("DataVal", o.DataVal ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("SellerId", o.SellerId)
                .Column("SellerName", o.SellerName)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);

                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 官方活动条目 更新
        ///</summary>
        ///<param name="o">ActItem</param>
        ///<returns></returns>
        public bool ActItemUpdate(ActItem o)
        {
            return Try(nameof(ActItemUpdate), () =>
            {
                var cmd = SqlBuilder.Update("ActItem")
                .Column("Type", o.Type)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("DataKey", o.DataKey ?? string.Empty)
                .Column("DataVal", o.DataVal ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 官方活动条目 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ActItemStatusUpdate(int id, bool status)
        {
            return Try(nameof(ActItemStatusUpdate), () =>
            {
                var sql = @"update ActItem set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 官方活动条目 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ActItemDelete(int id)
        {
            return Try(nameof(ActItemDelete), () =>
            {
                var sql = @"delete from ActItem where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 官方活动条目 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public ActItem ActItemGet(int id)
        {
            return Try(nameof(ActItemGet), () =>
            {
                var sql = @"select * from ActItem where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.QueryFirstOrDefault<ActItem>(cmd);
            });
        }

        /// <summary>
        /// 官方活动条目 分页
        /// </summary>
        /// <param name="planId">活动计划id</param>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<ActItem> ActItemPagedList(string planId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(ActItemPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("ActItem")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .Where("PlanId=@planId", new { planId })                
                .ToCommand(page, size);
                return StoreConn.PagedList<ActItem>(page, size, cmd);
            });
        }

        #endregion

        #region 文章
        ///<summary>
        /// 文章 创建
        ///</summary>
        ///<param name="o">Article</param>
        ///<returns></returns>
        public int ArticleCreate(Article o)
        {
            return Try(nameof(ArticleCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Article")
                .Column("Type", o.Type)
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Label", o.Label ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Keyword", o.Keyword ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Content", o.Content ?? string.Empty)
                .Column("Source", o.Source ?? string.Empty)
                .Column("Author", o.Author ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("ViewCount", o.ViewCount)
                .Column("LikeCount", o.LikeCount)
                .Column("CollectCount", o.CollectCount)
                .Column("CommentCount", o.CommentCount)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("UserId", o.UserId)
                .Column("CreatedBy", o.CreatedBy ?? string.Empty)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 文章 更新
        ///</summary>
        ///<param name="o">Article</param>
        ///<returns></returns>
        public bool ArticleUpdate(Article o)
        {
            return Try(nameof(ArticleUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Article")
                .Column("Type", o.Type)
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Label", o.Label ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Keyword", o.Keyword ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Content", o.Content ?? string.Empty)
                .Column("Source", o.Source ?? string.Empty)
                .Column("Author", o.Author ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 文章状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ArticleStatusUpdate(int id, byte status)
        {
            return Try(nameof(ArticleStatusUpdate), () =>
            {
                var sql = @"update Article set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 文章 删除
        /// </summary>
        ///<param name="id">id</param>
        ///<param name="userId">userId</param>
        /// <returns></returns>
        public bool ArticleDelete(int id, int userId = 0)
        {
            return Try(nameof(ArticleDelete), () =>
            {
                var cmd = SqlBuilder
                .Delete("Article")
                .Where("Id=@id", new { id })
                .Where(userId > 0, "UserId=@userId", new { userId })
                .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 文章 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Article ArticleGet(int id)
        {
            return Try(nameof(ArticleGet), () =>
            {
                var sql = @"select * from Article where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return StoreConn.QueryFirstOrDefault<Article>(cmd);
            });
        }


        /// <summary>
        /// 文章 分页
        /// </summary>
        /// <param name="planId">模块id</param>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public IPagedList<Article> ArticlePagedList(int page = 1, int size = 20, string keyword = null, bool? status = null, int userId = 0)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(ArticlePagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Article")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .Where(userId > 0, "UserId=@userId", new { userId })
                .ToCommand(page, size);
                return StoreConn.PagedList<Article>(page, size, cmd);
            });
        }
        #endregion
    }
}