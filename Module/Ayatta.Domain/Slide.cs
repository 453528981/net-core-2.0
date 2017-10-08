using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Domain
{
    ///<summary>
    /// 幻灯片
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Slide : IEntity<string>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public string Id { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 宽
        ///</summary>
        public int Width { get; set; }

        ///<summary>
        /// 高
        ///</summary>
        public int Height { get; set; }

        ///<summary>
        /// 是否有缩略图
        ///</summary>
        public bool Thumb { get; set; }

        ///<summary>
        /// 缩略图高
        ///</summary>
        public int ThumbW { get; set; }

        ///<summary>
        /// 缩略图宽
        ///</summary>
        public int ThumbH { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        public string Description { get; set; }

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

        #endregion

        /// <summary>
        /// 支付平台
        /// </summary>
        public virtual IList<SlideItem> Items { get; set; }

        public static string NewId
        {
            get { return DateTime.Now.ToString("yyMMddHHmm"); }
            
        }
    }

    ///<summary>
    /// 幻灯片条目
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SlideItem : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// SlideId
        ///</summary>
        public string SlideId { get; set; }

        /// <summary>
        /// 分组id
        /// </summary>
        public int GroupId { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 导航URL
        ///</summary>
        public string NavUrl { get; set; }

        ///<summary>
        /// 图片Src
        ///</summary>
        public string ImageSrc { get; set; }

        ///<summary>
        /// 缩略图Src
        ///</summary>
        public string ThumbSrc { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        public string Description { get; set; }

        ///<summary>
        /// 开始时间
        ///</summary>
        public DateTime StartedOn { get; set; }

        ///<summary>
        /// 结束时间
        ///</summary>
        public DateTime StoppedOn { get; set; }

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

        #endregion
    }
}
