using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;
using System;

namespace Ayatta.Storage
{

    public partial class DefaultStorage
    {
        #region 店铺优惠
        /// <summary>
        /// 创建店铺优惠活动
        /// </summary>
        /// <param name="o">店铺优惠活动</param>
        /// <returns></returns>
        public int PromotionActivityCreate(Promotion.Activity o)
        {
            if (o.Global)
            {
                o.ItemScope = string.Empty;
            }
            if (o.LimitType == Promotion.LimitType.None)
            {
                o.LimitValue = 0;
            }
            return Try(nameof(PromotionActivityCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Activity")
                .Column("Type", o.Type)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Global", o.Global)
                .Column("WarmUp", o.WarmUp)
                .Column("Infinite", o.Infinite)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Platform", o.Platform)
                .Column("MediaScope", o.MediaScope ?? string.Empty)
                .Column("ItemScope", o.ItemScope ?? string.Empty)
                .Column("LimitType", o.LimitType)
                .Column("LimitValue", o.LimitValue)
                .Column("FreightFree", o.FreightFree)
                .Column("FreightFreeExclude", o.FreightFreeExclude ?? string.Empty)
                .Column("ExternalUrl", o.ExternalUrl ?? string.Empty)
                .Column("RuleData", o.RuleData)
                .Column("SellerId", o.SellerId)
                .Column("SellerName", o.SellerName)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return PromotionConn.ExecuteScalar<int>(cmd);

            });
        }

        /// <summary>
        /// 更新店铺优惠活动
        /// </summary>
        /// <param name="o">店铺优惠活动</param>
        /// <returns></returns>
        public bool PromotionActivityUpdate(Promotion.Activity o)
        {
            if (o.LimitType == Promotion.LimitType.None)
            {
                o.LimitValue = 0;
            }
            return Try(nameof(PromotionActivityUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Activity")
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Global", o.Global)
                .Column("WarmUp", o.WarmUp)
                .Column("Infinite", o.Infinite)
                .Column("Picture", o.Picture)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Platform", o.Platform)
                .Column("MediaScope", o.MediaScope)
                .Column("ItemScope", o.ItemScope)
                .Column("LimitType", o.LimitType)
                .Column("LimitValue", o.LimitValue)
                .Column("FreightFree", o.FreightFree)
                .Column("FreightFreeExclude", o.FreightFreeExclude)
                .Column("ExternalUrl", o.ExternalUrl)
                .Column("RuleData", o.RuleData)
                //.Column("SellerId", o.SellerId)
                //.Column("SellerName", o.SellerName)
                .Column("Status", o.Status)
                //.Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            });
        }

        
        /// <summary>
        /// 设置店铺优惠活动为不可用
        /// </summary>
        /// <param name="id">店铺优惠活动Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public bool PromotionActivityDisable(int id, int? sellerId = null)
        {
            return Try(nameof(PromotionActivityUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Activity")
                     .Column("Status=0")
                    .Where("Id=@Id and Seller=@SellerId", new { id })
                    .Where(sellerId.HasValue && sellerId.Value > 0, "Seller=@SellerId", new { sellerId })
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 获取店铺优惠活动数量
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="global">是否为全店活动</param>
        /// <param name="status">活动状态</param>
        /// <returns></returns>
        public int PromotionActivityCount(int sellerId, bool? global = null, bool? status = null)
        {
            return Try(nameof(PromotionActivityCount), () =>
            {
                var cmd = SqlBuilder.Select("count(0)")
                        .From("Activity")
                        .Where("SellerId=@sellerId", new { sellerId })
                        .Where(global.HasValue, "Global=@global", new { global })
                        .Where(status.HasValue, "Status=@status", new { status })
                        .ToCommand();
                return PromotionConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 获取店铺优惠活动
        /// </summary>
        /// <param name="id">店铺优惠活动Id</param>
        /// <param name="includeRules">是否同时获取店铺优惠规则</param>
        /// <returns></returns>
        public Promotion.Activity PromotionActivityGet(int id)
        {
            return Try(nameof(PromotionActivityGet), () =>
            {
                var cmd = SqlBuilder.Select("*")
                   .From("Activity")
                   .Where("Id=@id", new { id })
                   .ToCommand();
                return PromotionConn.QueryFirst<Promotion.Activity>(cmd);
            });
        }


        /// <summary>
        /// 获取店铺优惠活动
        /// </summary>
        /// <param name="index">当前页</param>
        /// <param name="size">分页大小</param>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="status">状态 true为可用正在进行中和即将开始 false为不可用或已结束</param>
        /// <returns></returns>
        public IPagedList<Promotion.Activity> PromotionActivityPagedList(int index, int size, int? sellerId = null, bool? status = null)
        {
            return Try(nameof(PromotionActivityPagedList), () =>
            {
                var cmd = SqlBuilder.Select("*")
                    .From("Activity")
                    .Where(sellerId.HasValue, "SellerId=@sellerId", new { sellerId })
                    .Where(status.HasValue && status.Value, "(Status=1 and StoppedOn>now())")
                    .Where(status.HasValue && !status.Value, "(Status=0 or StoppedOn<now())")
                    .ToCommand(index, size);
                return PromotionConn.PagedList<Promotion.Activity>(index, size, cmd);
            });
        }

        /*
        /// <summary>
        /// 创建店铺优惠活动规则
        /// </summary>
        /// <param name="o">店铺优惠活动规则</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleCreate(Promotion.Default.Rule o)
        {
            try
            {
                var cmd = SqlBuilder
                    .Insert("[Default.Rule]")
                    .Column("ParentId", o.ParentId)
                    .Column("Upon", o.Upon)
                    .Column("Value", o.Value)
                    .Column("SendGift", o.SendGift)
                    .Column("GiftJson", o.GiftData)
                    .Column("SendCoupon", o.SendCoupon)
                    .Column("CouponJson", o.CouponData)
                    .Column("SellerId", o.SellerId)
                    .Column("Status", o.Status)
                    .Column("CreatedBy", o.CreatedBy)
                    .Column("CreatedOn", o.CreatedOn)
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }

        /// <summary>
        /// 更新店铺优惠活动规则
        /// </summary>
        /// <param name="o">店铺优惠活动规则</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleUpdate(Promotion.Default.Rule o)
        {
            try
            {
                var cmd = SqlBuilder
                    .Update("[Default.Rule]")
                    .Column("Upon", o.Upon)
                    .Column("Value", o.Value)
                    .Column("SendGift", o.SendGift)
                    .Column("GiftJson", o.GiftData)
                    .Column("SendCoupon", o.SendCoupon)
                    .Column("CouponJson", o.CouponData)
                    .Column("Status", o.Status)
                    .Where("Id=@Id", new { o.Id })
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }

        /// <summary>
        /// 获取店铺优惠活动规则
        /// </summary>
        /// <param name="id">店铺优惠活动规则Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public Promotion.Default.Rule PromotionDefaultRuleGet(int id, int? sellerId = null)
        {
            try
            {
                var cmd = SqlBuilder
                    .Select("*")
                    .From("[Default.Rule]")
                    .Where("Id=@Id", new { id })
                    .Where(sellerId.HasValue, "SellerId=@sellerId", new { sellerId })
                    .ToCommand();
                return PromotionConn.Query<Promotion.Default.Rule>(cmd).FirstOrDefault();
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return null;
        }

        /// <summary>
        /// 设置店铺优惠活动规则为不可用
        /// </summary>
        /// <param name="id">店铺优惠活动规则Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleDisable(int id, int? sellerId = null)
        {
            var sql = "update [Default.Rule] set [Status]=0 where Id=@Id";
            if (sellerId.HasValue)
            {
                sql = "update [Default.Rule] set [Status]=0 where Id=@Id and Seller=@SellerId";
            }
            var cmd = SqlBuilder.Raw(sql).Append(sellerId.HasValue, "", new { sellerId }).ToCommand();
            try
            {
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }
        */

        #endregion

        /// <summary>
        /// 获取搭配组合套餐
        /// </summary>
        /// <param name="id">搭配组合套餐Id</param>
        /// <param name="includeItems">是否包含套餐附属商品信息</param>
        /// <returns></returns>
        public Promotion.Package PromotionPackageGet(int id, bool includeItems = false)
        {
            if (includeItems)
            {
                const string sql = @"select * from Package where Id=@id;select * from PackageItem where ParentId=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return
                    StoreConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id,
                        o => o.ParentId, (a, b) =>
                        {
                            a.Items = b.ToList();
                        }).FirstOrDefault();
            }
            else
            {
                var cmd = SqlBuilder.Select("*")
                        .From("Package")
                        .Where("Id=@id", new { id })
                        .ToCommand();

                return PromotionConn.Query<Promotion.Package>(cmd).FirstOrDefault();
            }
        }

        #region 获取卖家当前时间可用的促销

        /// <summary>
        /// 获取卖家当前时间可用的限购信息
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.LimitBuy> PromotionLimitBuyList(int sellerId)
        {

            const string sql = @"select * from LimitBuy where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.LimitBuy>(cmd).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的特价
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.SpecialPrice> PromotionSpecialPriceList(int sellerId)
        {
            const string sql = @"select * from SpecialPrice where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from SpecialPriceItem a inner join SpecialPrice b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.SpecialPrice, Promotion.SpecialPrice.Item, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Items = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的搭配组合套餐
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Package> PromotionPackageList(int sellerId)
        {
            const string sql = @"select * from Package where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from PackageItem a inner join Package b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Items = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的店铺优惠活动
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Activity> PromotionActivityList(int sellerId)
        {
            const string sql = @"select * from Activity where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Activity>(cmd).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的购物车促销
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.CartActivity> PromotionCartActivityList(int sellerId)
        {
            const string sql = @"select * from CartActivity where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from CartActivityRule a inner join CartActivity b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.CartActivity, Promotion.CartActivity.Rule, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Rules = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();
        }
        #endregion


    }
}