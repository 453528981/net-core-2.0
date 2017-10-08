using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 文章
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Article : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 类型
        ///</summary>
        public int Type { get; set; }

        ///<summary>
        /// 名称 方便后台使用
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题 前台使用
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 标签
        ///</summary>
        public string Label { get; set; }

        ///<summary>
        /// 链接
        ///</summary>
        public string Link { get; set; }

        ///<summary>
        /// 图标
        ///</summary>
        public string Icon { get; set; }

        ///<summary>
        /// 图片
        ///</summary>
        public string Picture { get; set; }

        ///<summary>
        /// 关键词
        ///</summary>
        public string Keyword { get; set; }

        ///<summary>
        /// 内容摘要
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        /// 详细内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        /// 来源
        ///</summary>
        public string Source { get; set; }

        ///<summary>
        /// 作者
        ///</summary>
        public string Author { get; set; }

        ///<summary>
        /// 开始时间
        ///</summary>
        public DateTime StartedOn { get; set; }

        ///<summary>
        /// 结束时间
        ///</summary>
        public DateTime StoppedOn { get; set; }

        ///<summary>
        /// 浏览数量
        ///</summary>
        public int ViewCount { get; set; }

        ///<summary>
        /// 点赞数量
        ///</summary>
        public int LikeCount { get; set; }

        ///<summary>
        /// 收藏数量
        ///</summary>
        public int CollectCount { get; set; }

        ///<summary>
        /// 评论数量
        ///</summary>
        public int CommentCount { get; set; }

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
        /// 状态 0可用 1不可用 2违禁
        ///</summary>
        public byte Status { get; set; }

        ///<summary>
        /// 用户id
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 创建者
        ///</summary>
        public string CreatedBy { get; set; }

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

        #endregion
    }


}