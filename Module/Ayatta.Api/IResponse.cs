using Ayatta;

namespace Ayatta.Api
{
    public interface IResponse
    {
        /// <summary>
        /// 状态码 0为正常 默认0
        /// </summary>
        byte Code { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        string Message { get; set; }
    }
}