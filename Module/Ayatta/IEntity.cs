namespace Ayatta
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {

    }

    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="T">主键类型</typeparam>
    public interface IEntity<T> : IEntity
    {
        /// <summary>
        /// Id 主键 只能为log,int,short,string,Guid
        /// </summary>
        T Id { get; set; }
    }
}