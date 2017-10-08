using System;
using ProtoBuf;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    ///<summary>
    /// 用户地址
    ///</summary>
    [ProtoContract]
    public class UserAddress : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(2)]
        public int UserId { get; set; }

        ///<summary>
        /// 地址分组 0为收货地址 2发货地址 3退货地址
        ///</summary>
        [ProtoMember(3)]
        public byte GroupId { get; set; }

        ///<summary>
        /// 收货人
        ///</summary>
        [ProtoMember(4)]
        public string Consignee { get; set; }

        ///<summary>
        /// 公司名称
        ///</summary>
        [ProtoMember(5)]
        public string CompanyName { get; set; }

        ///<summary>
        /// 国家代码
        ///</summary>
        [ProtoMember(6)]
        public int CountryId { get; set; }

        ///<summary>
        /// 行政区编码
        ///</summary>
        [ProtoMember(7)]
        public string RegionId { get; set; }

        ///<summary>
        /// 省
        ///</summary>
        [ProtoMember(8)]
        public string Province { get; set; }

        ///<summary>
        /// 市
        ///</summary>
        [ProtoMember(9)]
        public string City { get; set; }

        ///<summary>
        /// 区
        ///</summary>
        [ProtoMember(10)]
        public string District { get; set; }

        ///<summary>
        /// 街道门牌号
        ///</summary>
        [ProtoMember(11)]
        public string Street { get; set; }

        ///<summary>
        /// 邮编
        ///</summary>
        [ProtoMember(12)]
        public string PostalCode { get; set; }

        ///<summary>
        /// 固定电话号码
        ///</summary>
        [ProtoMember(13)]
        public string Phone { get; set; }

        ///<summary>
        /// 移动电话号码
        ///</summary>
        [ProtoMember(14)]
        public string Mobile { get; set; }

        ///<summary>
        /// 是否为默认地址
        ///</summary>
        [ProtoMember(15)]
        public bool IsDefault { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(16)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(17)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(18)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address => Province + City + District + Street;

        /// <summary>
        /// 省Id
        /// </summary>
        public string ProvinceId
        {
            get
            {
                if (!string.IsNullOrEmpty(RegionId))
                {
                    return RegionId.Substring(0, 2);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 市Id
        /// </summary>
        public string CityId
        {
            get
            {
                if (!string.IsNullOrEmpty(RegionId))
                {
                    return RegionId.Substring(0, 4);
                }
                return string.Empty;
            }
        }

        public override bool Equals(object obj)
        {
            var target = obj as UserAddress;
            if (target != null)
            {
                return (Consignee == target.Consignee && CountryId == target.CountryId && RegionId == target.RegionId &&
                        Street == target.Street && PostalCode == target.PostalCode &&
                        Phone == target.Phone && Mobile == target.Mobile);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id ^ UserId;
        }

    }
}