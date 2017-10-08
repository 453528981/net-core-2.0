namespace Ayatta.Api
{
    /// <summary>
    /// 设备及环境
    /// </summary>
    public sealed class Meta
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceId { get; set; }
    }
    public interface IRequest
    {
        /// <summary>
        /// 设备及环境
        /// </summary>
        Meta Meta { get; set; }

    }
}