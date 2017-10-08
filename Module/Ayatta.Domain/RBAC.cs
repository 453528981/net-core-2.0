using System;
using ProtoBuf;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public static class RBAC
    {
        /// <summary>
        /// 角色
        /// </summary>
        public class Role : IEntity<int>
        {
            #region Properties

            ///<summary>
            /// Id
            ///</summary>
            public int Id { get; set; }

            ///<summary>
            /// 名称
            ///</summary>
            public string Name { get; set; }

            ///<summary>
            /// 摘要
            ///</summary>
            public string Summary { get; set; }

            /// <summary>
            /// 排序优先级
            /// </summary>
            public int Priority { get; set; }

            ///<summary>
            /// 状态 ture可用 false不可用
            ///</summary>
            public bool Status { get; set; }

            #endregion
        }

        /// <summary>
        /// 模块
        /// </summary>
        public class Module : IEntity<int>
        {
            #region Properties

            ///<summary>
            /// Id
            ///</summary>
            public int Id { get; set; }

            ///<summary>
            /// 父Id
            ///</summary>
            public int Pid { get; set; }

            ///<summary>
            /// 名称
            ///</summary>
            public string Name { get; set; }

            ///<summary>
            /// 路径
            ///</summary>
            public string Path { get; set; }

            ///<summary>
            /// 图标
            ///</summary>
            public string Icon { get; set; }

            ///<summary>
            /// 链接
            ///</summary>
            public string Link { get; set; }

            ///<summary>
            /// 深度
            ///</summary>
            public int Depth { get; set; }

            /// <summary>
            /// 排序优先级
            /// </summary>
            public int Priority { get; set; }

            ///<summary>
            /// 状态 ture可用 false不可用
            ///</summary>
            public bool Status { get; set; }

            #endregion

            public virtual IList<Func> Funcs { get; set; }
        }

        /// <summary>
        /// 功能
        /// </summary>
        public class Func : IEntity<string>
        {
            #region Properties

            ///<summary>
            /// Id
            ///</summary>
            public string Id { get; set; }

            ///<summary>
            /// 模块Id
            ///</summary>
            public int Mid { get; set; }

            ///<summary>
            /// 名称
            ///</summary>
            public string Name { get; set; }

            ///<summary>
            /// 摘要
            ///</summary>
            public string Summary { get; set; }

            /// <summary>
            /// 排序优先级
            /// </summary>
            public int Priority { get; set; }

            ///<summary>
            /// 状态 ture可用 false不可用
            ///</summary>
            public bool Status { get; set; }

            public string dd { get; set; }

            #endregion
        }       

    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]

    public class UserRoleFunc
    {

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string FuncId { get; set; }

        public string FuncName { get; set; }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public string ModuleIcon { get; set; }

        public string ModuleLink { get; set; }

        public int ModulePriority { get; set; }

        public int ModuleParentId { get; set; }

    }

    public static partial class Extension
    {
        public static bool HasRole(this IEnumerable<UserRoleFunc> data, int roleId)
        {
            if (data == null || data.Count() == 0)
            {
                return false;
            }
            return data.Any(x => x.RoleId == roleId);
        }

        public static bool HasFunc(this IEnumerable<UserRoleFunc> data, string funcId)
        {
            if (data == null || data.Count() == 0)
            {
                return false;
            }
            return data.Any(x => x.FuncId == funcId);
        }

        public static IList<RBAC.Module> ToModuleList(this IEnumerable<UserRoleFunc> data)
        {
            var dic = new Dictionary<int, RBAC.Module>();
            if (data != null)
            {
                foreach (var o in data)
                {
                    if (!dic.ContainsKey(o.ModuleId))
                    {
                        var m = new RBAC.Module();
                        m.Id = o.ModuleId;
                        m.Name = o.ModuleName;
                        m.Icon = o.ModuleIcon;
                        m.Link = o.ModuleLink;
                        m.Pid = o.ModuleParentId;
                        m.Priority = o.ModulePriority;
                        dic.Add(o.ModuleId, m);
                    }
                }
            }
            
            return dic.Values.ToList();
        }

       

        public static IDictionary<string, string> GetFuncDic(this IEnumerable<UserRoleFunc> data, int moduleId)
        {
            var dic = new Dictionary<string, string>();
            foreach (var o in data)
            {
                if (!dic.ContainsKey(o.FuncId))
                {
                    dic.Add(o.FuncId, o.FuncName);
                }
            }
            return dic;
        }
    }
}
