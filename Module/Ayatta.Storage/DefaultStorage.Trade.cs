using Dapper;
using System;
using System.Linq;
using Ayatta.Domain;

namespace Ayatta.Storage
{

    public partial class DefaultStorage
    {
        #region 订单

        ///<summary>
        /// 创建订单
        ///</summary>
        ///<param name="o">订单</param>
        ///<returns></returns>
        public bool OrderCreate(Order o)
        {
            return Try(nameof(OrderCreate), () =>
            {
                using (var conn = TradeConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Insert("OrderInfo")
                            .Column("Id", o.Id)
                            .Column("Type", o.Type)
                            .Column("Quantity", o.Quantity)
                            .Column("SubTotal", o.SubTotal)
                            .Column("Freight", o.Freight)
                            .Column("Tax", o.Tax)
                            .Column("Discount", o.Discount)
                            .Column("Total", o.Total)
                            .Column("Paid", o.Paid)
                            .Column("PayId", o.PayId)
                            .Column("PaidOn", o.PaidOn)
                            .Column("PointUse", o.PointUse)
                            .Column("PointRealUse", o.PointRealUse)
                            .Column("PointReward", o.PointReward)
                            .Column("Coupon", o.Coupon)
                            .Column("CouponUse", o.CouponUse)
                            .Column("GiftCard", o.GiftCard)
                            .Column("GiftCardUse", o.GiftCardUse)
                            .Column("PromotionData", o.PromotionData)
                            .Column("Weight", o.Weight)
                            .Column("ETicket", o.ETicket)
                            .Column("IsVirtual", o.IsVirtual)
                            .Column("IsBonded", o.IsBonded)
                            .Column("IsOversea", o.IsOversea)
                            .Column("PaymentType", o.PaymentType)
                            .Column("PaymentData", o.PaymentData)
                            .Column("ShipmentType", o.ShipmentType)
                            .Column("ShipmentData", o.ShipmentData)
                            .Column("ExpiredOn", o.ExpiredOn)
                            .Column("ConsignedOn", o.ConsignedOn)
                            .Column("FinishedOn", o.FinishedOn)
                            .Column("InvoiceType", o.InvoiceType)
                            .Column("InvoiceTitle", o.InvoiceTitle)
                            .Column("InvoiceContent", o.InvoiceContent)
                            .Column("InvoiceStatus", o.InvoiceStatus)
                            .Column("LogisticsNo", o.LogisticsNo)
                            .Column("LogisticsType", o.LogisticsType)
                            .Column("LogisticsCode", o.LogisticsCode)
                            .Column("LogisticsName", o.LogisticsName)
                            .Column("Receiver", o.Receiver)
                            .Column("ReceiverPhone", o.ReceiverPhone)
                            .Column("ReceiverMobile", o.ReceiverMobile)
                            .Column("ReceiverRegionId", o.ReceiverRegionId)
                            .Column("ReceiverProvince", o.ReceiverProvince)
                            .Column("ReceiverCity", o.ReceiverCity)
                            .Column("ReceiverDistrict", o.ReceiverDistrict)
                            .Column("ReceiverStreet", o.ReceiverStreet)
                            .Column("ReceiverPostalCode", o.ReceiverPostalCode)
                            .Column("BuyerId", o.BuyerId)
                            .Column("BuyerName", o.BuyerName)
                            .Column("BuyerFlag", o.BuyerFlag)
                            .Column("BuyerMemo", o.BuyerMemo)
                            .Column("BuyerRated", o.BuyerRated)
                            .Column("BuyerMessage", o.BuyerMessage)
                            .Column("SellerId", o.SellerId)
                            .Column("SellerName", o.SellerName)
                            .Column("SellerFlag", o.SellerFlag)
                            .Column("SellerMemo", o.SellerMemo)
                            .Column("HasReturn", o.HasReturn)
                            .Column("HasRefund", o.HasRefund)
                            .Column("CancelId", o.CancelId)
                            .Column("CancelReason", o.CancelReason)
                            .Column("RelatedId", o.RelatedId)
                            .Column("MediaId", o.MediaId)
                            .Column("TraceCode", o.TraceCode)
                            .Column("IpAddress", o.IpAddress)
                            .Column("Extra", o.Extra)
                            .Column("Status", o.Status)
                            .Column("CreatedBy", o.CreatedBy)
                            .Column("CreatedOn", o.CreatedOn)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)

                            .ToCommand(false, tran);


                            var status = conn.Execute(cmd) > 0;

                            if (status)
                            {
                                foreach (var item in o.Items)
                                {
                                    cmd = SqlBuilder.Insert("OrderItem")
                                    .Column("Id", item.Id)
                                    .Column("OrderId", item.OrderId)
                                    .Column("SpuId", item.SpuId)
                                    .Column("ItemId", item.ItemId)
                                    .Column("SkuId", item.SkuId)
                                    .Column("CatgRId", item.CatgRId)
                                    .Column("CatgMId", item.CatgMId)
                                    .Column("CatgId", item.CatgId)
                                    .Column("PackageId", item.PackageId)
                                    .Column("PackageName", item.PackageName)
                                    .Column("Code", item.Code)
                                    .Column("Name", item.Name)
                                    .Column("Price", item.Price)
                                    .Column("PriceShow", item.PriceShow)
                                    .Column("Quantity", item.Quantity)
                                    .Column("Tax", item.Tax)
                                    .Column("Adjust", item.Adjust)
                                    .Column("Discount", item.Discount)
                                    .Column("Total", item.Total)
                                    .Column("TaxRate", item.TaxRate)
                                    .Column("Picture", item.Picture)
                                    .Column("PropText", item.PropText)
                                    .Column("IsGift", item.IsGift)
                                    .Column("IsVirtual", item.IsVirtual)
                                    .Column("IsService", item.IsService)
                                    .Column("PromotionData", item.PromotionData)
                                    .Column("ExpiredOn", item.ExpiredOn)
                                    .Column("ConsignedOn", item.ConsignedOn)
                                    .Column("FinishedOn", item.FinishedOn)
                                    .Column("LogisticsNo", item.LogisticsNo)
                                    .Column("LogisticsName", item.LogisticsName)
                                    .Column("ReturnId", item.ReturnId)
                                    .Column("ReturnStatus", item.ReturnStatus)
                                    .Column("RefundId", item.RefundId)
                                    .Column("RefundStatus", item.RefundStatus)
                                    .Column("BuyerId", item.BuyerId)
                                    .Column("BuyerName", item.BuyerName)
                                    .Column("BuyerRated", item.BuyerRated)
                                    .Column("SellerId", item.SellerId)
                                    .Column("SellerName", item.SellerName)
                                    .Column("Extra", item.Extra)
                                    .Column("Status", item.Status)
                                    .Column("CreatedOn", item.CreatedOn)
                                    .Column("ModifiedBy", item.ModifiedBy)
                                    .Column("ModifiedOn", item.ModifiedOn)
                                    .ToCommand(false, tran);

                                    status = conn.Execute(cmd) > 0;
                                    if (!status)
                                    {
                                        break;
                                    }
                                }
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

        /*
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        public bool OrderStatusUpdate(string id, OrderStatus status)
        {
            return Try(nameof(OrderStatusUpdate), () =>
            {
                var sql = @"update orderinfo set status=@statue where id=@id";

                return TradeConn.Execute(sql, new { id, status }) > 0;
            });
        }
        */

        /// <summary>
        /// 更新订单已付金额及状态
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="amount">本次支付金额</param>
        /// <param name="done">是否已付清(付清后如果是在线支付订单则会更新订单状态为已付款待发货并更新订单有效期)</param>
        /// <returns></returns>
        public bool OrderPaid(string id, decimal amount, bool done)
        {
            var time = DateTime.Now;
            var expire = time.AddDays(7);
            return Try(nameof(OrderPaid), () =>
            {
                var sql = @"update orderinfo set Paid=Paid+@amount,PaidOn=@time where status=2 and id=@id";
                if (done)
                {
                    sql = @"update orderinfo set Paid=Paid+@amount,PaidOn=@time,ExpiredOn=@expire,Status=3 where status=2 and id=@id";
                }
                return TradeConn.Execute(sql, new { id, amount, time, expire }) > 0;
            });
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="userId">用户id</param>
        /// <param name="isSeller">是否为卖家</param>
        /// <param name="includeItems">是否包含明细</param>
        /// <returns></returns>
        public Order OrderGet(string id, int userId, bool isSeller, bool includeItems = false)
        {
            return Try(nameof(OrderGet), () =>
            {
                if (includeItems)
                {
                    var sql = @"select * from orderinfo where id=@id and BuyerId=@userId;select * from orderitem where orderid=@id and BuyerId=@userId;";
                    if (isSeller)
                    {
                        sql = @"select * from orderinfo where id=@id and SellerId=@userId;select * from orderitem where orderid=@id and SellerId=@userId;;";
                    }
                    var cmd = SqlBuilder.Raw(sql, new { id, userId }).ToCommand();
                    using (var reader = TradeConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Order>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<OrderItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var cmd = SqlBuilder
                    .Select("*").From("orderinfo")
                    .Where("id=@id", new { id })
                    .Where(isSeller ? "SellerId=@userId" : "BuyerId=@userId", new { userId })
                    .ToCommand();
                    return TradeConn.QueryFirstOrDefault<Order>(cmd);
                }
            });
        }

        /// <summary>
        /// 获取订单备注 Flag Memo Message
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="userId">用户id</param>
        /// <param name="isSeller">是否为卖家</param>
        /// <returns></returns>
        public Magic<byte, string, string> OrderMemoGet(string id, int userId, bool isSeller)
        {
            return Try(nameof(OrderMemoGet), () =>
            {
                var fields = "BuyerFlag as First,BuyerMemo as Second,BuyerMessage as Third";
                if (isSeller)
                {
                    fields = "SellerFlag as First,SellerMemo as Second,'' as Third";
                }
                var cmd = SqlBuilder.Select(fields)
                .From("orderinfo")
                .Where("Id=@Id", new { id })
                .Where(isSeller, "SellerId=@userId", new { userId })
                .Where(!isSeller, "BuyerId=@userId", new { userId })
                .ToCommand(0);

                return TradeConn.QueryFirstOrDefault<Magic<byte, string, string>>(cmd);
            });
        }

        /// <summary>
        /// 更新订单备注
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="userId">用户id</param>
        /// <param name="isSeller">是否为卖家</param>
        /// <param name="flag">标识</param>
        /// <param name="memo">备注</param>
        /// <returns></returns>
        public bool OrderMemoUpdate(string id, int userId, bool isSeller, byte flag, string memo)
        {
            return Try(nameof(OrderMemoGet), () =>
            {
                var cmd = SqlBuilder.Update("orderinfo")
                .Column(isSeller, "SellerFlag", flag)
                .Column(isSeller, "SellerMemo", memo)
                .Column(!isSeller, "BuyerFlag", flag)
                .Column(!isSeller, "BuyerMemo", memo)
                .Where(isSeller ? "SellerId=@userId" : "BuyerId=@userId", new { userId })
                .Where("Id=@Id", new { id })
                .ToCommand();

                return TradeConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="userId">用户id</param>
        /// <param name="isSeller">是否为卖家</param>
        /// <returns></returns>
        public OrderStatus OrderStatusGet(string id, int userId, bool isSeller)
        {
            return Try(nameof(OrderStatusGet), () =>
            {
                var cmd = SqlBuilder
                .Select("Status")
                .From("orderinfo")
                .Where("Id=@Id", new { id })
                .Where(isSeller ? "SellerId=@userId" : "BuyerId=@userId", new { userId })
                .ToCommand(0);

                return TradeConn.ExecuteScalar<OrderStatus>(cmd);
            });
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="userId">用户id</param>
        /// <param name="isSeller">是否为卖家</param>
        /// <param name="cancelId">取消类型 0为none 1为系统取消 2为买家取消 3为卖家取消</param>
        /// <param name="cancelReason">取消原因</param>
        /// <returns></returns>
        public bool OrderCancel(string id, int userId, bool isSeller, byte cancelId, string cancelReason = "")
        {
            return Try(nameof(OrderMemoGet), () =>
            {
                var cmd = SqlBuilder.Update("orderinfo")
                .Column("Status", OrderStatus.Canceled)
                .Column("CancelId", cancelId)
                .Column("CancelReason", cancelReason)
                .Column("FinishedOn", DateTime.Now)
                .Where(isSeller ? "SellerId=@userId" : "BuyerId=@userId", new { userId })
                .Where("Id=@Id", new { id })
                .ToCommand();

                return TradeConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 延迟订单有效期
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="sellerId"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public bool OrderDelay(string id, int sellerId, int day)
        {
            return Try(nameof(OrderDelay), () =>
            {
                var cmd = SqlBuilder.Update("orderinfo")
                .Column("ExpiredOn=date_add(now(),interval " + day + " day)")
                .Where("Id=@Id and SellerId=@sellerId", new { id, sellerId })
                .ToCommand();

                return TradeConn.Execute(cmd) > 0;
            });
        }

        public IPagedList<Order> OrderPagedList()
        {
            return Try(nameof(OrderPagedList), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("orderinfo")
                .ToCommand(1, 50);
                var list = TradeConn.PagedList<Order>(1, 50, cmd);
                var ids = list.Select(x => x.Id).Aggregate((a, b) => "'" + a + "','" + b + "'");
                var sql = "select * from orderitem where orderid in(" + ids + ")";
                var items = TradeConn.Query<OrderItem>(sql);
                foreach (var o in list)
                {
                    o.Items = items.Where(x => x.OrderId == o.Id).ToList();
                }
                return list;
            });
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public OrderMini OrderMiniGet(string id, int userId)
        {
            var fields = "Id,Type,Quantity,SubTotal,Freight,Tax,Discount,Total,Paid,PayId,PaidOn,PointUse,PointRealUse,PointReward,Coupon,CouponUse,GiftCard,GiftCardUse,Weight,ETicket,IsVirtual,IsBonded,IsOversea,PaymentType,ShipmentType,ExpiredOn,BuyerId,BuyerName,SellerId,SellerName,MediaId,TraceCode,Status,CreatedOn";

            return Try(nameof(OrderMiniGet), () =>
            {
                var cmd = SqlBuilder
                .Select(fields)
                .From("orderinfo")
                .Where("Id=@Id", new { id })
                .ToCommand(0);
                return TradeConn.QueryFirstOrDefault<OrderMini>(cmd);
            });
        }
        #endregion

        #region 订单Note
        ///<summary>
        /// 订单Note创建
        ///</summary>
        ///<param name="o">订单Note</param>
        ///<returns></returns>
        public int OrderNoteCreate(OrderNote o)
        {
            return Try(nameof(OrderNoteCreate), () =>
            {
                var cmd = SqlBuilder.Insert("OrderNote")
                .Column("Type", o.Type)
                .Column("UserId", o.UserId)
                .Column("OrderId", o.OrderId)
                .Column("Subject", o.Subject)
                .Column("Message", o.Message)
                .Column("Extra", o.Extra)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .ToCommand(true);
                return TradeConn.ExecuteScalar<int>(cmd);
            });
        }

        #endregion
    }
}