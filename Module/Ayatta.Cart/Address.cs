using ProtoBuf;

namespace Ayatta.Cart
{

    #region 收货地址
    [ProtoContract]
    public class Address
    {
        /// <summary>
        /// 用户地址Id
        /// </summary>
        [ProtoMember(1)]

        public int Id { get; set; } = 0;

        ///<summary>
        /// 行政区编码
        ///</summary>
        [ProtoMember(2)]
        public string RegionId { get; set; } = string.Empty;

        ///<summary>
        /// 省
        ///</summary>
        [ProtoMember(3)]
        public string Province { get; set; } = string.Empty;

        ///<summary>
        /// 市
        ///</summary>
        [ProtoMember(4)]
        public string City { get; set; } = string.Empty;

        ///<summary>
        /// 区
        ///</summary>
        [ProtoMember(5)]
        public string District { get; set; } = string.Empty;

        ///<summary>
        /// 街道门牌号
        ///</summary>
        [ProtoMember(6)]
        public string Street { get; set; } = string.Empty;

        ///<summary>
        /// 邮编
        ///</summary>
        [ProtoMember(7)]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// 固定电话
        /// </summary>
        [ProtoMember(8)]

        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 移动电话
        /// </summary>
        [ProtoMember(9)]

        public string Mobile { get; set; } = string.Empty;

        /// <summary>
        /// Consignee
        /// </summary>
        [ProtoMember(10)]

        public string Consignee { get; set; } = string.Empty;

    }
    #endregion

}