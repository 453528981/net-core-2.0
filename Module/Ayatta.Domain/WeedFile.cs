using System;
using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Domain
{
    ///<summary>
    /// Weed 文件
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class WeedFile : IEntity<string>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public string Id { get; set; }

        ///<summary>
        /// 用户Id
        ///</summary>
        public int Uid { get; set; }

        ///<summary>
        /// 目录Id
        ///</summary>
        public int Did { get; set; }

        ///<summary>
        /// 扩展名
        ///</summary>
        public string Ext { get; set; }

        ///<summary>
        /// Url
        ///</summary>
        public string Url { get; set; }

        ///<summary>
        /// 大小
        ///</summary>
        public int Size { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标记
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

        #region Readonly
        [ProtoIgnore]
        public bool IsImg
        {
            get
            {
                return imgexts.Contains(Ext);
            }
        }
        #endregion

        private static string[] imgexts = new string[] { ".gif", ".png", ".jpg", ".jpeg" };

    }
}
