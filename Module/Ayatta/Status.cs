namespace Ayatta
{
    /// <summary>
    /// 多状态类
    /// </summary>
    public class Status
    {
        /// <summary>
        /// 状态码 0为正常
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Status()
            : this(0, string.Empty)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        public Status(int code)
            : this(code, string.Empty)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">状态信息</param>
        public Status(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static implicit operator bool(Status status)
        {
            return status.Code == 0;
        }
    }

    /// <summary>
    /// 多状态泛型类
    /// </summary>
    /// <typeparam name="T">泛型数据类型</typeparam>
    public class Status<T> : Status
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Status()
            : this(0, string.Empty, default(T))
        {

        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        public Status(int code)
            : this(code, string.Empty, default(T))
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="data">数据</param>
        public Status(T data)
            : this(0, string.Empty, data)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="data">数据</param>
        public Status(int code, T data)
            : this(code, string.Empty, data)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">状态信息</param>
        /// <param name="data">数据</param>
        public Status(int code, string message, T data)
            : base(code, message)
        {
            Data = data;
        }

    }

    /// <summary>
    /// 多状态泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TExtra"></typeparam>
    public sealed class Status<T, TExtra> : Status<T>
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public TExtra Extra { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Status()
            : this(0, string.Empty, default(T), default(TExtra))
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        public Status(int code)
            : this(code, string.Empty, default(T), default(TExtra))
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="extra">扩展数据</param>
        public Status(T data, TExtra extra)
            : this(0, string.Empty, data, extra)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="data">数据</param>
        /// <param name="extra">扩展数据</param>
        public Status(int code, T data, TExtra extra)
            : this(code, string.Empty, data, extra)
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">状态信息</param>
        /// <param name="data">数据</param>
        /// <param name="extra">扩展数据</param>
        public Status(int code, string message, T data, TExtra extra)
            : base(code, message, data)
        {
            Extra = extra;
        }
    }
}
