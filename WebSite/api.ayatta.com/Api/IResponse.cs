using Ayatta;

namespace Ayatta.Web.Api
{
    public interface IResponse
    {
        /// <summary>
        /// 状态码 0为正常
        /// </summary>
        uint Code { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        string Message { get; set; }
    }
}