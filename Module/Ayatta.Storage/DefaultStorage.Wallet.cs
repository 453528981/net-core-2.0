using Dapper;
using Ayatta.Domain;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        #region 账户

        #endregion

        #region 支付
        ///<summary>
        /// 支付信息创建
        ///</summary>
        ///<param name="o">支付信息</param>
        ///<returns></returns>
        public bool PaymentCreate(Payment o)
        {
            return Try(nameof(PaymentCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Payment")
                .Column("Id", o.Id)
                .Column("No", o.No)
                .Column("Type", o.Type)
                .Column("UserId", o.UserId)
                .Column("Method", o.Method)
                .Column("Amount", o.Amount)
                .Column("Subject", o.Subject)
                .Column("Message", o.Message)
                .Column("RawData", o.RawData)
                .Column("BankId", o.BankId)
                .Column("BankCode", o.BankCode)
                .Column("BankName", o.BankName)
                .Column("BankCard", o.BankCard)
                .Column("PlatformId", o.PlatformId)
                .Column("CardNo", o.CardNo)
                .Column("CardPwd", o.CardPwd)
                .Column("CardAmount", o.CardAmount)
                .Column("RelatedId", o.RelatedId)
                .Column("IpAddress", o.IpAddress)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
                return WalletConn.Execute(cmd) > 0;
                /*
                var status = WalletConn.Execute(cmd) > 0;
                if (status)
                {
                    OnCreated("payment-created", o);
                }
                return status;
                */
            });
        }

        /// <summary>
        /// 支付信息获取
        /// </summary>
        /// <param name="id">支付信息id</param>
        /// <returns></returns>
        public Payment PaymentGet(string id)
        {
            return Try(nameof(PaymentGet), () =>
            {
                var sql = @"select * from Payment where id=@id";

                return WalletConn.QueryFirstOrDefault<Payment>(sql, new { id });
            });
        }

        /// <summary>
        /// 支付信息状态获取
        /// </summary>
        /// <param name="id">支付信息id</param>
        /// <returns></returns>
        public bool PaymentStatusGet(string id)
        {
            return Try(nameof(PaymentStatusGet), () =>
            {
                var sql = @"select status from Payment where id=@id";

                return WalletConn.QueryFirstOrDefault<bool>(sql, new { id });
            });
        }

        /// <summary>
        /// 支付信息状态更新
        /// </summary>
        /// <param name="id">支付信息Id</param>
        /// <param name="no">支付平台交易号 支付宝 财富通等 支付成功后更新该字段</param>
        /// <param name="status">支付状态</param>
        /// <returns></returns>
        public bool PaymentStatusUpdate(string id, string no, bool status)
        {
            return Try(nameof(PaymentStatusUpdate), () =>
            {
                var sql = @"update Payment set no=@no,status=@status where id=@id";

                return WalletConn.Execute(sql, new { id, no, status }) > 0;
            });
        }

        #endregion

        #region 支付Note
        ///<summary>
        /// 支付Note创建
        ///</summary>
        ///<param name="o">支付Note</param>
        ///<returns></returns>
        public int PaymentNoteCreate(PaymentNote o)
        {
            return Try(nameof(PaymentNoteCreate), () =>
            {
                var cmd = SqlBuilder.Insert("PaymentNote")
                .Column("PayId", o.PayId)
                .Column("PayNo", o.PayNo)
                .Column("UserId", o.UserId)
                .Column("Subject", o.Subject)
                .Column("Message", o.Message)
                .Column("RawData", o.RawData)
                .Column("Extra", o.Extra)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .ToCommand(true);
                return WalletConn.ExecuteScalar<int>(cmd);
            });
        }


        #endregion

    }
}