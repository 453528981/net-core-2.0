using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {

        #region 用户 角色 模块 功能

        ///<summary>
        /// 角色 是否存在
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool RoleExist(int id)
        {
            return Try(nameof(RoleExist), () =>
            {
                var cmd = SqlBuilder.Select("1")
                .From("Role")
                .Where("Id=@id", new { id })
                .ToCommand();
                return RbacConn.ExecuteScalar<bool>(cmd);
            });
        }

        ///<summary>
        /// 角色 创建
        ///</summary>
        ///<param name="o">Role</param>
        ///<returns></returns>
        public int RoleCreate(RBAC.Role o)
        {
            return Try(nameof(RoleCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Role")                
                .Column("Name", o.Name)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Status", o.Status)
                .ToCommand(true);
                return RbacConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 角色 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool RoleStatusUpdate(int id, bool status)
        {
            return Try(nameof(RoleStatusUpdate), () =>
            {
                var sql = @"update Role set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 模块 创建
        ///</summary>
        ///<param name="o">Module</param>
        ///<returns></returns>
        public int ModuleCreate(RBAC.Module o)
        {
            return Try(nameof(ModuleCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Module")
                .Column("Pid", o.Pid)
                .Column("Name", o.Name)
                .Column("Path", o.Path ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Depth", o.Depth)
                .Column("Priority", o.Priority)
                .Column("Status", o.Status)
                .ToCommand(true);
                return RbacConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 模块 更新
        ///</summary>
        ///<param name="o">Module</param>
        ///<returns></returns>
        public bool ModuleUpdate(RBAC.Module o)
        {
            return Try(nameof(ModuleUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Module")
                .Column("Name", o.Name)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Status", o.Status)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }


        /// <summary>
        /// 模块 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ModuleStatusUpdate(int id, bool status)
        {
            return Try(nameof(ModuleStatusUpdate), () =>
            {
                var sql = @"update Module set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 模块 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public RBAC.Module ModuleGet(int id)
        {
            return Try(nameof(ModuleGet), () =>
            {
                var sql = @"select * from Module where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return RbacConn.QueryFirstOrDefault<RBAC.Module>(cmd);
            });
        }

        /// <summary>
        /// 模块 列表
        /// </summary>
        /// <returns></returns>
        public IList<RBAC.Module> ModuleList()
        {
            return Try(nameof(ModuleList), () =>
            {
                var sql = @"select * from Module";
                var cmd = SqlBuilder.Raw(sql).ToCommand();
                return RbacConn.Query<RBAC.Module>(cmd).ToList();
            });
        }

        /// <summary>
        /// 模块 列表
        /// </summary>
        /// <returns></returns>
        public IList<RBAC.Module> ModuleWidthFuncList()
        {
            var sql = @"select * from Module;select * from Func;";
            return Try(nameof(ModuleWidthFuncList), () =>
            {
                var cmd = SqlBuilder.Raw(sql).ToCommand();
                using (var reader = TradeConn.QueryMultiple(cmd))
                {
                    var ms = reader.Read<RBAC.Module>();
                    if (ms != null)
                    {
                        var fs = reader.Read<RBAC.Func>();
                        if (fs != null)
                        {
                            foreach (var m in ms)
                            {
                                m.Funcs = fs.Where(x => x.Mid == m.Id).ToList();
                            }
                        }
                    }
                    return ms.ToList();
                }
            });
        }


        /// <summary>
        /// 模块 iddic
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, int> ModuleIdDic()
        {
            var dic = new Dictionary<int, int>();

            return Try(nameof(ModuleIdDic), () =>
            {
                var sql = @"SELECT Id,Pid from Module";
                using (var reader = RbacConn.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        dic.Add(reader.GetInt32(0), reader.GetInt32(1));
                    }
                }
                return dic;
            });
        }

        /// <summary>
        /// 模块 Path 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool ModulePathUpdate(int id, string path)
        {
            return Try(nameof(ModulePathUpdate), () =>
            {
                var sql = @"update Module set Path=@path where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, path }).ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 功能 是否存在
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool FuncExist(string id)
        {
            return Try(nameof(FuncExist), () =>
            {
                var cmd = SqlBuilder.Select("1")
                .From("Func")
                .Where("Id=@id", new { id })
                .ToCommand();
                return RbacConn.ExecuteScalar<bool>(cmd);
            });
        }

        ///<summary>
        /// 功能 创建
        ///</summary>
        ///<param name="o">Func</param>
        ///<returns></returns>
        public bool FuncCreate(RBAC.Func o)
        {
            return Try(nameof(FuncCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Func")
                .Column("Id", o.Id)
                .Column("Mid", o.Mid)
                .Column("Name", o.Name)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Status", o.Status)
                .ToCommand();
                return RbacConn.Execute(cmd)>0;
            });
        }

        ///<summary>
        /// 功能 更新
        ///</summary>
        ///<param name="o">Module</param>
        ///<returns></returns>
        public bool FuncUpdate(RBAC.Func o)
        {
            return Try(nameof(FuncUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Func")
                .Column("Name", o.Name)
                .Column("Summary", o.Summary ?? string.Empty)              
                .Column("Priority", o.Priority)
                .Column("Status", o.Status)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 功能 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool FuncStatusUpdate(int id, bool status)
        {
            return Try(nameof(FuncStatusUpdate), () =>
            {
                var sql = @"update Func set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return RbacConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 功能 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public RBAC.Func FuncGet(string id)
        {
            return Try(nameof(FuncGet), () =>
            {
                var sql = @"select * from Func where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return RbacConn.QueryFirstOrDefault<RBAC.Func>(cmd);
            });
        }

        /// <summary>
        /// 功能 列表
        /// </summary>
        /// <param name="mid">模块id</param>
        /// <returns></returns>
        public IList<RBAC.Func> FuncList(int mid)
        {
            return Try(nameof(FuncList), () =>
            {
                var sql = @"select * from Func where mid=@mid";
                var cmd = SqlBuilder.Raw(sql, new { mid }).ToCommand();
                return RbacConn.Query<RBAC.Func>(cmd).ToList();
            });
        }

        /// <summary>
        /// 用户角色功能列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IList<UserRoleFunc> UserRoleFuncList(int userId)
        {
            var sql = @"
            select rf.*,m.Id as ModuleId
            ,m.Pid as ModuleParentId
            ,r.Name as RoleName
            ,f.Name as FuncName
            ,m.Name as ModuleName
            ,m.Icon as ModuleIcon
            ,m.Link as ModuleLink
            from UserRole ur
            inner join RoleFunc rf on ur.RoleId=rf.RoleId
            inner join Role r on rf.RoleId=r.Id
            inner join Func f on rf.FuncId=f.Id
            inner join Module m on f.Mid=m.Id
            where ur.UserId=@userId";
            return Try(nameof(UserRoleFuncList), () =>
            {
                var cmd = SqlBuilder.Raw(sql, new { userId }).ToCommand();
                return RbacConn.Query<UserRoleFunc>(cmd).ToList();
            });
        }

        #endregion
    }
}