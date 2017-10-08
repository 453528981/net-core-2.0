using System;

namespace Ayatta.Domain
{
    #region User
    /// <summary>
    /// 用户角色 买家 卖家 平台管理员
    /// </summary>
    [Flags]
    public enum UserRole : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// 买家
        /// </summary>
        Buyer = 1,

        /// <summary>
        /// 卖家
        /// </summary>
        Seller = 2,

        /// <summary>
        /// 管理员
        /// </summary>
        Administrator = 3
    }

    /// <summary>
    /// 用户级别
    /// </summary>
    public enum UserGrade : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// 一级
        /// </summary>
        One = 1,

        /// <summary>
        /// 二级
        /// </summary>
        Two = 2
    }

    /// <summary>
    /// 用户限制
    /// </summary>
    [Flags]
    public enum UserLimitation : byte
    {
        /// <summary>
        /// 无限制
        /// </summary>
        None = 0
    }

    /// <summary>
    /// 商家特殊许可
    /// </summary>
    [Flags]
    public enum UserPermission : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// 竞拍
        /// </summary>
        Auction = 1
    }



    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 未通过手机 邮箱验证
        /// </summary>
        Invalid = 1,

        /// <summary>
        /// 被系统隔离 无法下单
        /// </summary>
        Isolated = 2,

        /// <summary>
        /// 被系统禁用 帐号异常或违规
        /// </summary>
        Forbidden = 3,

        /// <summary>
        /// 被系统删除 无法进行任何操作
        /// </summary>
        Deleted = 255

    }

    /// <summary>
    /// 用户性别
    /// </summary>
    public enum Gender : byte
    {
        /// <summary>
        /// 保密
        /// </summary>

        Secrect = 0,

        /// <summary>
        /// 男
        /// </summary>
        Male = 1,

        /// <summary>
        /// 女
        /// </summary>
        Female = 2
    }

    /// <summary>
    /// 用户婚姻状况
    /// </summary>
    public enum Marital : byte
    {
        /// <summary>
        /// 保密
        /// </summary>
        Secrect = 0,

        /// <summary>
        /// 单身
        /// </summary>
        Single = 1,

        /// <summary>
        /// 已婚
        /// </summary>
        Married = 2
    }

    /// <summary>
    /// 用户收/发/退货地址
    /// </summary>
    public enum UserAddressGroup : byte
    {
        /// <summary>
        /// 收货地址
        /// </summary>
        Receive = 0,
        /// <summary>
        /// 发货地址
        /// </summary>
        Send = 1,
        /// <summary>
        /// 退货地址
        /// </summary>
        Refund = 2
    }
    #endregion

    #region 平台
    /// <summary>
    /// 平台
    /// </summary>
    [Flags]
    public enum Platform : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Pc
        /// </summary>
        Pc = 1,

        /// <summary>
        /// Wap
        /// </summary>
        Wap = 2,

        /// <summary>
        /// App
        /// </summary>
        App = 4,

        All = Pc | Wap | App
    }
    #endregion

    #region 付款方式
    /// <summary>
    /// 付款方式 20以上为其它支付平台或方式
    /// </summary>
    public enum PaymentType : byte
    {
        /// <summary>
        /// 初始值
        /// </summary>
        None = 0,

        /// <summary>
        /// 支付宝支付
        /// </summary>
        Alipay = 1,

        /// <summary>
        /// 微信支付
        /// </summary>
        Weixin = 2,

        /// <summary>
        /// 财富通支付
        /// </summary>
        Tenpay = 3,

        /// <summary>
        /// 余额支付
        /// </summary>
        Wallet = 4,

        /// <summary>
        /// 积分支付
        /// </summary>
        Point = 5,

        /// <summary>
        /// 货到付款
        /// </summary>
        COD = 6,

        /// <summary>
        /// 银行转帐
        /// </summary>
        BankTransfer = 7,

        /// <summary>
        /// 邮局汇款
        /// </summary>
        PostOffice = 8,

        /*
        /// <summary>
        ///  竞购获得的拍品兑换为积分或免费拍币
        /// </summary>
        Exchange = 30
        */

    }
    #endregion

    /// <summary>
    /// 物流方式
    /// </summary>
    public enum ShipmentType : byte
    {
        /// <summary>
        /// 初始值
        /// </summary>
        None = 0,

        /// <summary>
        /// 无需物流发货(游戏充值 手机充值等)
        /// </summary>
        Virtual = 1,

        /// <summary>
        /// 快递
        /// </summary>
        Express = 2,

        /// <summary>
        /// EMS
        /// </summary>
        Ems = 3,

        /// <summary>
        /// 平邮
        /// </summary>
        Post = 4,

        /// <summary>
        /// 自提
        /// </summary>
        SelfFetch = 5,

        /// <summary>
        /// 其它
        /// </summary>
        Other = 255
    }

    /// <summary>
    /// Note限定范围
    /// </summary>
    [Flags]
    public enum NoteScope : byte
    {

        /// <summary>
        /// 只有买家可见
        /// </summary>
        Buyer = 1,

        /// <summary>
        /// 只有卖家可见
        /// </summary>
        Seller = 2,

        /// <summary>
        /// 只有管理员可见
        /// </summary>
        Manager = 4,

        /// <summary>
        /// 都可见
        /// </summary>
        All = Buyer | Seller | Manager,
    }

}
