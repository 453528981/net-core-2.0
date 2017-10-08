using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web.Api
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
}
