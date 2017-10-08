using System;

namespace Ayatta.Sms
{

    public class SmsMessage
    {
        /// <summary>
        /// 唯一识别码
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 消息/模版
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否多个
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// 发送失败的手机号
        /// </summary>
        public string Failure { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Status Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }

        #region
        /// <summary>
        /// The inclock.
        /// </summary>
        private static readonly object Inclock = new object();

        /// <summary>
        /// The inc.
        /// </summary>
        private static int inc;

        /// <summary>
        /// Generate an increment.
        /// </summary>
        /// <returns>
        /// The increment.
        /// </returns>
        private static int GenerateInc()
        {
            lock (Inclock)
            {
                if (inc > 9999)
                {
                    inc = 0;
                }
                else
                {
                    inc++;
                }
                return inc;
            }
        }
        #endregion

        /// <summary>
        /// 生成一个新的Id
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            var now = DateTime.Now;
            var i = GenerateInc();
            return now.ToString("yyyyMMddHHmmss") + i.ToString("0000");
        }
    }

    /// <summary>
    /// 消息状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,

        /// <summary>
        /// 成功
        /// </summary>
        Successful = 2,
    }

    public class SmsResut
    {
        public string Guid { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        public static implicit operator bool(SmsResut result)
        {
            return result.Status;
        }
    }
}
