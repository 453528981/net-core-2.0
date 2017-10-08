using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// UserFavorite
    ///</summary>
    [ProtoContract]
    public class UserFavorite : IEntity<int>
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
        /// 类型 0商品 2品牌 3店铺
        ///</summary>
        [ProtoMember(3)]
        public byte GroupId { get; set; }

        ///<summary>
        /// 商品 品牌 店铺 名称
        ///</summary>
        [ProtoMember(4)]
        public string Name { get; set; }

        ///<summary>
        /// 商品Id 品牌Id 店铺Id 
        ///</summary>
        [ProtoMember(5)]
        public string Value { get; set; }

        ///<summary>
        /// 扩展信息 商品编号等
        ///</summary>
        [ProtoMember(6)]
        public string Extra { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(7)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(8)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(9)]
        public DateTime ModifiedOn { get; set; }
    }
}