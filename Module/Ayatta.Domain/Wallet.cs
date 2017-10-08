using System;
using ProtoBuf;
using System.Collections.Generic;

namespace Ayatta.Domain
{

    #region Account
    ///<summary>
    /// Account
    ///</summary>
    [ProtoContract]
    public class Account : IEntity<int>
    {

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 余额
        ///</summary>
        [ProtoMember(2)]
        public decimal Cash { get; set; }

        ///<summary>
        /// 被冻结的余额 下单时使用了余额支付 但余额不足以支付整个订单 还需网银/现金支付 等情况下
        ///</summary>
        [ProtoMember(3)]
        public decimal FrozenCash { get; set; }

        ///<summary>
        /// 积分
        ///</summary>
        [ProtoMember(4)]
        public int Point { get; set; }

        ///<summary>
        /// 被冻结的积分 下单使用了积分 但订单还未支付 等情况下
        ///</summary>
        [ProtoMember(5)]
        public int FrozenPoint { get; set; }

        ///<summary>
        /// 金币
        ///</summary>
        [ProtoMember(6)]
        public int Coin { get; set; }

        ///<summary>
        /// 支付密码
        ///</summary>
        [ProtoMember(7)]
        public string Password { get; set; }

        ///<summary>
        /// 帐号状态 1可用 0不可用
        ///</summary>
        [ProtoMember(8)]
        public bool Status { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(9)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(10)]
        public DateTime ModifiedOn { get; set; }


        public decimal AvailableMoney { get { return 0; } }

    }

    #endregion

    #region CashFlow
    ///<summary>
    /// CashFlow
    ///</summary>
    [ProtoContract]
    public class CashFlow : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 收入/支出 分组 当Amount大于0时 1充值获得 2支付退还获得(订单退款/通知延迟导致多次支付成功等) 3别的用户转账获得 有限制 当Amount小于0时 1支付订单 2代别人支付订单 3转账给别的用户 4提现 5充值金币 6给别的用户充值金币 7充值游戏 8给别的用户充值游戏
        ///</summary>
        [ProtoMember(2)]
        public int Type { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(3)]
        public int UserId { get; set; }

        ///<summary>
        /// 发生金额 大于0为收入 小于0为支出
        ///</summary>
        [ProtoMember(4)]
        public decimal Amount { get; set; }

        ///<summary>
        /// 发生后余额
        ///</summary>
        [ProtoMember(5)]
        public decimal Remain { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        [ProtoMember(6)]
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        [ProtoMember(7)]
        public string Message { get; set; }

        ///<summary>
        /// 相关数据 json格式
        ///</summary>
        [ProtoMember(8)]
        public string RawData { get; set; }

        ///<summary>
        /// 关联的Id 订单号 等
        ///</summary>
        [ProtoMember(9)]
        public string RelatedId { get; set; }

        ///<summary>
        /// 别人转账 或 为别人代付 的UserId
        ///</summary>
        [ProtoMember(10)]
        public int OtherUserId { get; set; }

        ///<summary>
        /// 别人转账 或 为别人代付 的UserName
        ///</summary>
        [ProtoMember(11)]
        public string OtherUserName { get; set; }

        ///<summary>
        /// 备注
        ///</summary>
        [ProtoMember(12)]
        public string Remark { get; set; }

        ///<summary>
        /// 创建者 UserName
        ///</summary>
        [ProtoMember(13)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(14)]
        public DateTime CreatedOn { get; set; }

    }

    #endregion

    #region PointFlow
    ///<summary>
    /// PointFlow
    /// created on 2016-07-16 12:59:57
    ///</summary>
    [ProtoContract]
    public class PointFlow : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 收入/支出 分组
        ///</summary>
        [ProtoMember(2)]
        public int Type { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(3)]
        public int UserId { get; set; }

        ///<summary>
        /// 发生积分数 大于0为收入 小于0为支出
        ///</summary>
        [ProtoMember(4)]
        public int Amount { get; set; }

        ///<summary>
        /// 可用积分(获得的积分在有效期内可使用值)
        ///</summary>
        [ProtoMember(5)]
        public int Usable { get; set; }

        ///<summary>
        /// 发生后积分余额
        ///</summary>
        [ProtoMember(6)]
        public int Remain { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        [ProtoMember(7)]
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        [ProtoMember(8)]
        public string Message { get; set; }

        ///<summary>
        /// 相关数据 json格式
        ///</summary>
        [ProtoMember(9)]
        public string RawData { get; set; }

        ///<summary>
        /// 关联的Id 订单号 等
        ///</summary>
        [ProtoMember(10)]
        public string RelatedId { get; set; }

        ///<summary>
        /// 获得的积分有效期
        ///</summary>
        [ProtoMember(11)]
        public DateTime ExpiredOn { get; set; }

        ///<summary>
        /// 备注
        ///</summary>
        [ProtoMember(12)]
        public string Remark { get; set; }

        ///<summary>
        /// 创建者 UserName
        ///</summary>
        [ProtoMember(13)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(14)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(15)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(16)]
        public DateTime ModifiedOn { get; set; }

    }

    #endregion

    #region 支付单
    ///<summary>
    /// 支付单
    ///</summary>
    [ProtoContract]
    public class Payment : IEntity<string>
    {

        ///<summary>
        /// 支付流水号
        ///</summary>
        [ProtoMember(1)]
        public string Id { get; set; }

        ///<summary>
        /// 第三方支付流水号 支付宝 财富通等 支付成功后更新该字段
        ///</summary>
        [ProtoMember(2)]
        public string No { get; set; }

        ///<summary>
        /// 支付类型 0为订单支付 1为帐户余额充值 2为金币充值
        ///</summary>
        [ProtoMember(3)]
        public int Type { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(4)]
        public int UserId { get; set; }

        ///<summary>
        /// 支付方式 0 None 1使用网银 2手机充值卡等
        ///</summary>
        [ProtoMember(5)]
        public byte Method { get; set; }

        ///<summary>
        /// 支付金额
        ///</summary>
        [ProtoMember(6)]
        public decimal Amount { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        [ProtoMember(7)]
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        [ProtoMember(8)]
        public string Message { get; set; }

        ///<summary>
        /// 相关数据 订单 充值等数据 json格式
        ///</summary>
        [ProtoMember(9)]
        public string RawData { get; set; }

        ///<summary>
        /// 支付平台银行Id
        ///</summary>
        [ProtoMember(10)]
        public int BankId { get; set; }

        ///<summary>
        /// 支付平台银行编码 不同的支付平台 同一银行银行编码不同
        ///</summary>
        [ProtoMember(11)]
        public string BankCode { get; set; }

        ///<summary>
        /// 支付平台银行名称
        ///</summary>
        [ProtoMember(12)]
        public string BankName { get; set; }

        ///<summary>
        /// 0为None 1为储蓄卡 2为信用卡
        ///</summary>
        [ProtoMember(13)]
        public byte BankCard { get; set; }

        ///<summary>
        /// 支付平台Id
        ///</summary>
        [ProtoMember(14)]
        public int PlatformId { get; set; }

        ///<summary>
        /// 全国神州行充值卡 卡号17位 联通一卡充 卡号15位
        ///</summary>
        [ProtoMember(15)]
        public string CardNo { get; set; }

        ///<summary>
        /// 充值卡卡密
        ///</summary>
        [ProtoMember(16)]
        public string CardPwd { get; set; }

        ///<summary>
        /// 充值卡面值
        ///</summary>
        [ProtoMember(17)]
        public decimal CardAmount { get; set; }

        ///<summary>
        /// 关联的Id 订单号 充值单号 等
        ///</summary>
        [ProtoMember(18)]
        public string RelatedId { get; set; }

        ///<summary>
        /// Ip
        ///</summary>
        [ProtoMember(19)]
        public string IpAddress { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(20)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1成功 0失败
        ///</summary>
        [ProtoMember(21)]
        public bool Status { get; set; }

        ///<summary>
        /// 创建者 UserName
        ///</summary>
        [ProtoMember(22)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(23)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(24)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(25)]
        public DateTime ModifiedOn { get; set; }


        #region

        /// <summary>
        /// 用于支付的状态参数
        /// 支付平台会原样返回
        /// 数据库不需该字段
        /// </summary>
        public string State { get; set; } = "state";

        /// <summary>
        /// Payment Notes
        /// </summary>
        public virtual IList<PaymentNote> Notes { get; set; }
        #endregion

        #region
        /// <summary>
        /// The inc.
        /// </summary>
        private static int inc;
        /// <summary>
        /// The inclock.
        /// </summary>
        private static readonly object IncLock = new object();

        /// <summary>
        /// Generate an increment.
        /// </summary>
        /// <returns>
        /// The increment.
        /// </returns>
        private static int GenerateInc()
        {
            lock (IncLock)
            {
                if (inc > 9999)
                {
                    inc = 0;
                }
                else
                {
                    inc++;
                }
                return inc;
            }
        }

        #endregion

        /// <summary>
        /// 生成一个新的PayId
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            var now = DateTime.Now;
            var i = GenerateInc();
            return now.ToString("yyMMddHHmmss") + i.ToString("0000");
        }
    }

    #endregion

    #region PaymentNote
    ///<summary>
    /// PaymentNote
    ///</summary>
    [ProtoContract]
    public class PaymentNote : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 支付流水号
        ///</summary>
        [ProtoMember(2)]
        public string PayId { get; set; }

        ///<summary>
        /// 第三方支付流水号 支付宝 财富通等
        ///</summary>
        [ProtoMember(3)]
        public string PayNo { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(4)]
        public int UserId { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        [ProtoMember(5)]
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        [ProtoMember(6)]
        public string Message { get; set; }

        ///<summary>
        /// 支付平台 支付信息
        ///</summary>
        [ProtoMember(7)]
        public string RawData { get; set; }

        ///<summary>
        /// 扩展
        ///</summary>
        [ProtoMember(8)]
        public string Extra { get; set; }

        ///<summary>
        /// 创建者
        ///</summary>
        [ProtoMember(9)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(10)]
        public DateTime CreatedOn { get; set; }


    }

    #endregion
}