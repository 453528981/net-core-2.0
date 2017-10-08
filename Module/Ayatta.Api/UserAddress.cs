
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Api
{
    #region 用户收货地址列表
    /// <summary>
    /// 用户收货地址列表 响应
    /// </summary>
    public sealed class UserAddressListResponse : Response
    {
        /// <summary>
        /// 用户收货地址
        /// </summary>
        public IList<UserAddress> Data { get; set; }

    }

    /// <summary>
    /// 用户收货地址列表 请求
    /// </summary>
    public sealed class UserAddressListRequest : Request<UserAddressListResponse>
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        //public byte GroupId { get; set; }        
    }
    #endregion

    #region 用户收货地址创建
    /// <summary>
    /// 用户收货地址创建 响应
    /// </summary>
    public sealed class UserAddressCreateResponse : Response
    {
        /// <summary>
        /// 创建成功后的Id
        /// </summary>
        public int Data { get; set; }

    }

    /// <summary>
    /// 用户收货地址创建 请求
    /// </summary>
    public sealed class UserAddressCreateRequest : Request<UserAddressCreateResponse>
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 行政区编码
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// 街道门牌号
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 是否设置为默认地址
        /// </summary>
        public bool IsDefault { get; set; }

    }
    #endregion

    #region 用户收货地址更新
    /// <summary>
    /// 用户收货地址更新 响应
    /// </summary>
    public sealed class UserAddressUpdateResponse : Response
    {
        /*
        /// <summary>
        /// 是否更新成功
        /// </summary>
        public bool Data { get; set; }
        */
    }

    /// <summary>
    /// 用户收货地址更新 请求
    /// </summary>
    public sealed class UserAddressUpdateRequest : Request<UserAddressUpdateResponse>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 行政区编码
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// 街道门牌号
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 是否设置为默认地址
        /// </summary>
        public bool IsDefault { get; set; }

    }
    #endregion

    #region 用户收货地址删除
    /// <summary>
    /// 用户收货地址删除 响应
    /// </summary>
    public sealed class UserAddressDeleteResponse : Response
    {
        /*
        /// <summary>
        /// 是否删除成功
        /// </summary>
        public bool Data { get; set; }
        */

    }

    /// <summary>
    /// 用户收货地址删除 请求
    /// </summary>
    public sealed class UserAddressDeleteRequest : Request<UserAddressDeleteResponse>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
    #endregion

    #region 用户收货地址设为默认
    /// <summary>
    /// 用户收货地址设为默认 响应
    /// </summary>
    public sealed class UserAddressSetDefaultResponse : Response
    {

    }

    /// <summary>
    /// 用户收货地址设为默认 请求
    /// </summary>
    public sealed class UserAddressSetDefaultRequest : Request<UserAddressSetDefaultResponse>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
    #endregion
}