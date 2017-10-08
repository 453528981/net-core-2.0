using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// OAuthProvider
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class OAuthProvider : IEntity<string>
    {
        ///<summary>
        /// Id (qq sina等)
        ///</summary>
        public string Id { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// ClientId
        ///</summary>
        public string ClientId { get; set; }

        ///<summary>
        /// ClientSecret
        ///</summary>
        public string ClientSecret { get; set; }

        ///<summary>
        /// Scope
        ///</summary>
        public string Scope { get; set; }

        ///<summary>
        /// CallbackEndpoint
        ///</summary>
        public string CallbackEndpoint { get; set; }

        ///<summary>
        /// BaseUrl
        ///</summary>
        public string BaseUrl { get; set; }

        ///<summary>
        /// AuthorizationEndpoint
        ///</summary>
        public string AuthorizationEndpoint { get; set; }

        ///<summary>
        /// TokenEndpoint
        ///</summary>
        public string TokenEndpoint { get; set; }

        ///<summary>
        /// UserEndpoint
        ///</summary>
        public string UserEndpoint { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记
        ///</summary>
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 状态 true可用 false不可用
        ///</summary>
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }
    }
}