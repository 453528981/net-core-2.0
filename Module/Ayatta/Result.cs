namespace Ayatta
{
    /// <summary>
    /// 简单状态类
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 简单状态类
        /// </summary>
        public Result()
        {

        }

        /// <summary>
        /// 简单状态类
        /// </summary>
        /// <param name="status">状态</param>
        public Result(bool status)
            : this(status, string.Empty)
        {

        }

        /// <summary>
        /// 简单状态类
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="message">状态信息</param>
        public Result(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Status = false;
            Message = message;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="message"></param>
        public void Success(string message=null)
        {
            Status = true;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static implicit operator bool(Result result)
        {
            return result.Status;
        }
    }

    /// <summary>
    /// 简单状态泛型类
    /// </summary>
    /// <typeparam name="T">泛型数据类型</typeparam>
    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result()
        {

        }

        public Result(bool status)
            : this(status, default(T), string.Empty)
        {
        }

        public Result(bool status, string messge)
            : this(status, default(T), messge)
        {
        }

        public Result(bool status, T data)
            : this(status, data, string.Empty)
        {
        }

        public Result(bool status, T data, string message)
            : base(status, message)
        {
            Data = data;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="message"></param>
        public void Success(T data, string message)
        {
            Data = data;
            Success(message);
        }
    }

    /// <summary>
    /// 简单状态泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TExtra"></typeparam>
    public sealed class Result<T, TExtra> : Result<T>
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public TExtra Extra { get; set; }

        public Result() : this(false)
        {

        }
        public Result(bool status) : base(status, default(T), string.Empty)
        {
        }
    }
}