namespace Ayatta.Web.Api
{
    /// <summary>
    /// 响应基类
    /// </summary>
    public abstract class Response : IResponse
    {
        /// <summary>
        /// 状态码 0为正常
        /// </summary>
        public uint Code { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Message { get; set; }

        #region
        public Response()
            : this(0, string.Empty)
        {

        }

        public Response(uint code)
            : this(code, string.Empty)
        {

        }

        public Response(uint code, string message)
        {
            Code = code;
            Message = message;

        }

        #endregion

        public static implicit operator bool(Response rep)
        {
            return rep.Code == 0;
        }

        /*
        public static T New<T>(uint code, string message) where T: ApiResponse, new()
        {
            var t= new T();
            t.Code = code;
            t.Message = message;
            return t;
        }
        */
    }
}