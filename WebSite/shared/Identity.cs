namespace Ayatta.Web
{
    /// <summary>
    /// 用户身份
    /// </summary>
    public sealed class Identity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; }

        internal Identity(int id, string guid = null, string name = null, string displayName = null)
        {
            Id = id;
            Guid = guid;
            Name = name ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="identity"></param>
        public static implicit operator bool(Identity identity)
        {
            return identity.Id > 0 && !string.IsNullOrEmpty(identity.Guid) && !string.IsNullOrEmpty(identity.Name);
        }
    }
}
