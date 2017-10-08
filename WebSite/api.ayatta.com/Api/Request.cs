namespace Ayatta.Web.Api
{
    /// <summary>
    /// 请求基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Request<T> : IRequest where T : Response
    {

        /// <summary>
        /// 设备及环境
        /// </summary>
        public Meta Meta { get; set; }

    }
}