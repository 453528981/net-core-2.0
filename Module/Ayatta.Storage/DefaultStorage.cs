using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Ayatta.Storage
{
    /// <summary>
    /// 默认数据存贮
    /// </summary>
    public partial class DefaultStorage : BaseStorage
    {
        /// <summary>
        /// 默认数据存贮
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        public DefaultStorage(IOptions<StorageOptions> optionsAccessor, ILogger<DefaultStorage> logger) : base(optionsAccessor, logger)
        {

        }
    }
}