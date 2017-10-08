using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Api
{
    #region 文件上传
    /// <summary>
    /// 文件上传 响应
    /// </summary>
    public sealed class FileUploadResponse : Response
    {
        /// <summary>
        /// 文件绝对路径
        /// </summary>
        public string Data { get; set; }
    }

    /// <summary>
    /// 文件上传 请求
    /// </summary>
    public sealed class FileUploadRequest : Request<FileUploadResponse>
    {
        /// <summary>
        /// 事件（服务端使用该值使用不同规则进行上传检查）
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 文件域
        /// </summary>
        public string File { get; set; }
        
    }
    #endregion

    #region 行政区列表
    /// <summary>
    /// 行政区列表 响应
    /// </summary>
    public sealed class RegionListResponse : Response
    {
        /// <summary>
        /// 行政区列表
        /// </summary>
        public IList<Region> Data { get; set; }

        /// <summary>
        /// 层级行政区
        /// </summary>
        public China China { get; set; }
    }

    /// <summary>
    /// 行政区列表 请求
    /// </summary>
    public sealed class RegionListRequest : Request<RegionListResponse>
    {
        /// <summary>
        /// 父Id(值为空则取全部)
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 层级的
        /// </summary>
        public bool Hierarchical { get; set; }

    }
    #endregion
}
