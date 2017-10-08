using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        #region 类目
        public Catg CatgGet(int id)
        {
            var sql = @"select * from catg where id=@id;
                select * from catgprop where catgid=@id order by Priority;
                select * from catgpropvalue where catgid=@id order by Priority;
                ";
            using (var conn=BaseConn)
            using (var multi = BaseConn.QueryMultiple(sql, new { id }, commandTimeout: 60))
            {
                var catg = multi.Read<Catg>().FirstOrDefault();
                var props = multi.Read<Catg.Prop>().ToList();
                var propValues = multi.Read<Catg.Prop.Value>().ToList();

                conn.Close();

                if (catg != null)
                {
                    foreach (var prop in props)
                    {
                        prop.Values = propValues.Where(x => x.PropId == prop.Id).ToList();
                    }
                    catg.Props = props;
                }
                return catg;
            }
        }

        public Catg.Mini CatgMiniGet(int id)
        {
            var sql = @"select Id,Name,IsParent,ParentId from catg where id=@id;
                select Id,ParentPid,ParentVid,Name,Must,Multi,IsKeyProp,IsSaleProp,IsEnumProp,IsItemProp,IsColorProp,IsInputProp,IsSearchProp from catgprop where catgid=@id order by Priority;
                select Id,PropId,Name from catgpropvalue where catgid=@id order by Priority;
                ";
            using (var conn=BaseConn)
            using (var multi = BaseConn.QueryMultiple(sql, new { id }, commandTimeout: 60))
            {
                var catg = multi.Read<Catg.Mini>().FirstOrDefault();
                var props = multi.Read<Catg.Mini.Prop>().ToList();
                var propValues = multi.Read<Catg.Mini.Prop.Value>().ToList();

                conn.Close();

                if (catg != null)
                {
                    foreach (var prop in props)
                    {
                        prop.Values = propValues.Where(x => x.PropId == prop.Id).ToList();
                    }
                    catg.Props = props;
                }
                return catg;
            }
        }

        public IList<Catg.Tiny> CatgTinyList()
        {
            var sql = @"select id,name,parentid from catg where status=0";

            return BaseConn.Query<Catg.Tiny>(sql).ToList();
        }
        public IList<Catg> CatgList()
        {
            var sql = @"select * from catg order by Priority";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<Catg>(cmd).ToList();
        }

        public IList<Catg> CatgChildren(int catgId)
        {
            var sql = @"select * from catg where parentId=@catgId order by Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId }).ToCommand();
            return BaseConn.Query<Catg>(cmd).ToList();
        }

        public IList<Catg.Prop> CatgPropList(int catgId)
        {
            var sql = @"select * from catgprop where catgid=@catgId order by IsKeyProp desc,IsSaleProp desc,Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId }).ToCommand();
            return BaseConn.Query<Catg.Prop>(cmd).ToList();
        }
        public IList<Catg.Prop.Value> CatgPropValueList(int catgId, int propId)
        {
            var sql = @"select * from catgpropvalue where catgid=@catgId and propid=@propId order by Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId, propId }).ToCommand();
            return BaseConn.Query<Catg.Prop.Value>(cmd).ToList();
        }
        #endregion

        #region 第三方登录
        ///<summary>
        /// 第三方登录 创建
        ///</summary>
        ///<param name="o">OAuthProvider</param>
        ///<returns></returns>
        public bool OAuthProviderCreate(OAuthProvider o)
        {
            return Try(nameof(OAuthProviderCreate), () =>
            {
                var cmd = SqlBuilder.Insert("OAuthProvider")
                .Column("Id", o.Id)
                .Column("Name", o.Name)
                .Column("ClientId", o.ClientId)
                .Column("ClientSecret", o.ClientSecret)
                .Column("Scope", o.Scope ?? string.Empty)
                .Column("CallbackEndpoint", o.CallbackEndpoint)
                .Column("BaseUrl", o.BaseUrl)
                .Column("AuthorizationEndpoint", o.AuthorizationEndpoint)
                .Column("TokenEndpoint", o.TokenEndpoint)
                .Column("UserEndpoint", o.UserEndpoint ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });

        }

        ///<summary>
        /// 第三方登录 更新
        ///</summary>
        ///<param name="o">OAuthProvider</param>
        ///<returns></returns>
        public bool OAuthProviderUpdate(OAuthProvider o)
        {
            return Try(nameof(OAuthProviderUpdate), () =>
            {
                var cmd = SqlBuilder.Update("OAuthProvider")
                .Column("Name", o.Name)
                .Column("ClientId", o.ClientId)
                .Column("ClientSecret", o.ClientSecret)
                .Column("Scope", o.Scope ?? string.Empty)
                .Column("CallbackEndpoint", o.CallbackEndpoint)
                .Column("BaseUrl", o.BaseUrl)
                .Column("AuthorizationEndpoint", o.AuthorizationEndpoint)
                .Column("TokenEndpoint", o.TokenEndpoint)
                .Column("UserEndpoint", o.UserEndpoint ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });

        }

        ///<summary>
        /// 第三方登录 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool OAuthProviderDelete(string id)
        {
            return Try(nameof(OAuthProviderDelete), () =>
            {
                var sql = @"delete from OAuthProvider where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 第三方登录 是否存在
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool OAuthProviderExist(string id)
        {
            return Try(nameof(OAuthProviderExist), () =>
            {
                var sql = @"select 1 from OAuthProvider where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }


        ///<summary>
        /// 第三方登录 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public OAuthProvider OAuthProviderGet(string id)
        {
            return Try(nameof(OAuthProviderGet), () =>
            {
                var sql = @"select * from OAuthProvider where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<OAuthProvider>(cmd);
            });
        }

        ///<summary>
        /// 第三方登录 列表
        ///</summary>
        ///<returns></returns>
        public IList<OAuthProvider> OAuthProviderList()
        {
            return Try(nameof(OAuthProviderList), () =>
            {
                var sql = @"select * from OAuthProvider";
                var cmd = SqlBuilder.Raw(sql).ToCommand();
                return BaseConn.Query<OAuthProvider>(cmd).ToList();
            });
        }

        #endregion

        #region 帮助
        ///<summary>
        /// 帮助 创建
        ///</summary>
        ///<param name="o">帮助</param>
        ///<returns></returns>
        public int HelpCreate(Help o)
        {
            return Try(nameof(HelpCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Help")
                .Column("Pid", o.Pid)
                .Column("Path", o.Path ?? string.Empty)
                .Column("Depth", o.Depth)
                .Column("GroupId", o.GroupId)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Title", o.Title)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Content", o.Content ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });

        }

        ///<summary>
        /// 帮助 更新
        ///</summary>
        ///<param name="o">帮助</param>
        ///<returns></returns>
        public bool HelpUpdate(Help o)
        {
            return Try(nameof(HelpUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Help")
                .Column("Pid", o.Pid)
                .Column("Path", o.Path ?? string.Empty)
                .Column("Depth", o.Depth)
                .Column("GroupId", o.GroupId)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Title", o.Title)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Content", o.Content ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });

        }

        /// <summary>
        /// 帮助 Path 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool HelpPathUpdate(int id, string path)
        {
            return Try(nameof(AdModulePathUpdate), () =>
            {
                var sql = @"update Help set Path=@path where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, path }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 帮助 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool HelpDelete(int id)
        {
            return Try(nameof(HelpDelete), () =>
            {
                var sql = @"delete from Help where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 帮助 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Help HelpGet(int id)
        {
            return Try(nameof(HelpGet), () =>
            {
                var sql = @"select * from Help where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Help>(cmd);
            });
        }

        public IList<int> HelpPidList(int pid)
        {           
            return Try(nameof(HelpPidList), () =>
            {
                var sql = @"select id from help where id in(select path from help where id=@pid)";
                return BaseConn.Query<int>(sql,new { pid}).ToList();
            });
        }

        /// <summary>
        /// 帮助 分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<Help> HelpPagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(HelpPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Help")
                .Where(!string.IsNullOrEmpty(keyword), "Title=@keyword", new { keyword })//Regex.IsMatch(keyword, "^\\d+$")
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand(page, size);

                return BaseConn.PagedList<Help>(page, size, cmd);
            });
        }

        #endregion

        #region 国家
        ///<summary>
        /// 国家 创建
        ///</summary>
        ///<param name="o">国家</param>
        ///<returns></returns>
        public bool CountryCreate(Country o)
        {
            return Try(nameof(CountryCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Country")
                .Column("Id", o.Id)
                .Column("Code", o.Code)
                .Column("Name", o.Name)
                .Column("Flag", o.Flag)
                .Column("EnName", o.EnName)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 国家 更新
        ///</summary>
        ///<param name="o">国家</param>
        ///<returns></returns>
        public bool CountryUpdate(Country o)
        {
            return Try(nameof(CountryCreate), () =>
            {
                var cmd = SqlBuilder.Update("Country")
                .Column("Code", o.Code)
                .Column("Name", o.Name)
                .Column("Flag", o.Flag)
                .Column("EnName", o.EnName)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 国家 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool CountryDelete(int id)
        {
            return Try(nameof(CountryDelete), () =>
            {
                var sql = @"delete from Country where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 国家 获取
        ///</summary>
        ///<param name="id">三位数字代码</param>
        ///<returns></returns>
        public Country CountryGet(int id)
        {
            return Try(nameof(CountryGet), () =>
            {
                var sql = @"select * from Country where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Country>(cmd);
            });
        }

        ///<summary>
        /// 国家 获取
        ///</summary>
        ///<param name="code">三位字母代码</param>
        ///<returns></returns>
        public Country CountryGet(string code)
        {
            return Try(nameof(CountryGet), () =>
            {
                var sql = @"select * from Country where code=@code";
                var cmd = SqlBuilder.Raw(sql, new { code }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Country>(cmd);
            });
        }

        ///<summary>
        /// 国家 是否存在
        ///</summary>
        ///<param name="id">三位数字代码</param>
        ///<returns></returns>
        public bool CountryExist(int id)
        {
            return Try(nameof(CountryExist), () =>
            {
                var sql = @"select 1 from Country where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }

        ///<summary>
        /// 国家 是否存在
        ///</summary>
        ///<param name="code">三位字母代码</param>
        ///<returns></returns>
        public bool CountryExist(string code)
        {
            return Try(nameof(CountryExist), () =>
            {
                var sql = @"select 1 from Country where code=@code";
                var cmd = SqlBuilder.Raw(sql, new { code }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }

        /// <summary>
        /// 国家 分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<Country> CountryPagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(CountryPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Country")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })//Regex.IsMatch(keyword, "^\\d+$")
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand(page, size);

                return BaseConn.PagedList<Country>(page, size, cmd);
            });
        }

        #endregion

        #region 中国行政区(省市区)
        ///<summary>
        /// 行政区 创建
        ///</summary>
        ///<param name="o">行政区</param>
        ///<returns></returns>
        public bool RegionCreate(Region o)
        {
            return Try(nameof(RegionCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Region")
                .Column("Id", o.Id)
                .Column("ParentId", o.ParentId)
                .Column("Name", o.Name)
                .Column("PostalCode", o.PostalCode)
                .Column("GroupId", o.GroupId)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 行政区 更新
        ///</summary>
        ///<param name="o">行政区</param>
        ///<returns></returns>
        public bool RegionUpdate(Region o)
        {
            return Try(nameof(RegionUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Region")
                .Column("ParentId", o.ParentId)
                .Column("Name", o.Name)
                .Column("PostalCode", o.PostalCode)
                .Column("GroupId", o.GroupId)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id and ParentId=@parentId", new { o.Id, o.ParentId })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 行政区 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Region RegionGet(string id)
        {
            return Try(nameof(RegionGet), () =>
            {
                var sql = @"select * from Region where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Region>(cmd);
            });
        }

        ///<summary>
        /// 行政区 列表
        ///</summary>
        ///<returns></returns>
        public IList<Region> RegionList()
        {
            return Try(nameof(RegionList), () =>
            {
                var sql = @"select * from Region";
                var cmd = SqlBuilder.Raw(sql).ToCommand();
                return BaseConn.Query<Region>(cmd).ToList();
            });
        }

        /// <summary>
        /// 行政区 列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public IList<Region> RegionList(string parentId)
        {
            return Try(nameof(RegionList), () =>
            {
                var sql = @"select * from Region where ParentId=@parentId";
                var cmd = SqlBuilder.Raw(sql, new { parentId }).ToCommand();
                return BaseConn.Query<Region>(cmd).ToList();
            });
        }

        /// <summary>
        /// 行政区 分页
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="size">分页大小</param>
        /// <param name="parentId">parentId</param>
        /// <returns></returns>
        public IPagedList<Region> RegionPagedList(int page = 1, int size = 50, string parentId = null)
        {
            return Try(nameof(RegionPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Region")
                .Where(!string.IsNullOrEmpty(parentId), "parentId=@parentId", new { parentId })
                .ToCommand(page, size);
                return BaseConn.PagedList<Region>(page, size, cmd);
            });
        }

        #endregion

        #region 银行
        ///<summary>
        /// 银行 创建
        ///</summary>
        ///<param name="o">银行</param>
        ///<returns></returns>
        public int BankCreate(Bank o)
        {
            return Try(nameof(BankCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Bank")
                .Column("Name", o.Name)
                .Column("IconSrc", o.IconSrc ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 银行 更新
        ///</summary>
        ///<param name="o">银行</param>
        ///<returns></returns>
        public bool BankUpdate(Bank o)
        {
            return Try(nameof(BankUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Bank")
                .Column("Name", o.Name)
                .Column("IconSrc", o.IconSrc ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 银行 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool BankDelete(int id)
        {
            return Try(nameof(BankDelete), () =>
            {
                var sql = @"delete from Bank where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 银行 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Bank BankGet(int id)
        {
            return Try(nameof(BankGet), () =>
            {
                var sql = @"select * from Bank where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Bank>(cmd);
            });
        }

        /// <summary>
        /// 银行 列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<Bank> BankPagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(BankPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Bank")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })//Regex.IsMatch(keyword, "^\\d+$")
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand(page, size);
                return BaseConn.PagedList<Bank>(page, size, cmd);
            });
        }

        #endregion

        #region 支付平台
        ///<summary>
        /// 支付平台 创建
        ///</summary>
        ///<param name="o">PaymentPlatform</param>
        ///<returns></returns>
        public int PaymentPlatformCreate(PaymentPlatform o)
        {
            return Try(nameof(PaymentPlatformCreate), () =>
            {
                var cmd = SqlBuilder.Insert("PaymentPlatform")
                .Column("Name", o.Name)
                .Column("IconSrc", o.IconSrc ?? string.Empty)
                .Column("MerchantId", o.MerchantId)
                .Column("PrivateKey", o.PrivateKey ?? string.Empty)
                .Column("PublicKey", o.PublicKey ?? string.Empty)
                .Column("GatewayUrl", o.GatewayUrl)
                .Column("CallbackUrl", o.CallbackUrl)
                .Column("NotifyUrl", o.NotifyUrl)
                .Column("QueryUrl", o.QueryUrl ?? string.Empty)
                .Column("RefundUrl", o.RefundUrl ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 支付平台 更新
        ///</summary>
        ///<param name="o">PaymentPlatform</param>
        ///<returns></returns>
        public bool PaymentPlatformUpdate(PaymentPlatform o)
        {
            return Try(nameof(PaymentPlatformUpdate), () =>
            {
                var cmd = SqlBuilder.Update("PaymentPlatform")
                .Column("Name", o.Name)
                .Column("IconSrc", o.IconSrc ?? string.Empty)
                .Column("MerchantId", o.MerchantId)
                .Column("PrivateKey", o.PrivateKey ?? string.Empty)
                .Column("PublicKey", o.PublicKey ?? string.Empty)
                .Column("GatewayUrl", o.GatewayUrl)
                .Column("CallbackUrl", o.CallbackUrl)
                .Column("NotifyUrl", o.NotifyUrl)
                .Column("QueryUrl", o.QueryUrl ?? string.Empty)
                .Column("RefundUrl", o.RefundUrl ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 支付平台 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool PaymentPlatformDelete(int id)
        {
            return Try(nameof(PaymentPlatformDelete), () =>
            {
                var sql = @"delete from PaymentPlatform where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 支付平台 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public PaymentPlatform PaymentPlatformGet(int id)
        {
            return Try(nameof(PaymentPlatformGet), () =>
            {
                var sql = @"select * from PaymentPlatform where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<PaymentPlatform>(cmd);
            });
        }


        /// <summary>
        /// 支付平台 列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<PaymentPlatform> PaymentPlatformList(string keyword = null, bool? status = null)
        {
            return Try(nameof(PaymentPlatformList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("PaymentPlatform")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand();
                return BaseConn.Query<PaymentPlatform>(cmd).ToList();
            });
        }

        #endregion

        #region 支付平台银行
        ///<summary>
        /// 支付平台银行 创建
        ///</summary>
        ///<param name="o">支付平台银行</param>
        ///<returns></returns>
        public int PaymentBankCreate(PaymentBank o)
        {
            return Try(nameof(PaymentBankCreate), () =>
            {
                var cmd = SqlBuilder.Insert("PaymentBank")
                .Column("BankId", o.BankId)
                .Column("PlatformId", o.PlatformId)
                .Column("Code", o.Code)
                .Column("Description", o.Description)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 支付平台银行 更新
        ///</summary>
        ///<param name="o">支付平台银行</param>
        ///<returns></returns>
        public bool PaymentBankUpdate(PaymentBank o)
        {
            return Try(nameof(PaymentBankUpdate), () =>
            {
                var cmd = SqlBuilder.Update("PaymentBank")
                .Column("BankId", o.BankId)
                .Column("PlatformId", o.PlatformId)
                .Column("Code", o.Code)
                .Column("Description", o.Description)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 支付平台银行 删除
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool PaymentBankDelete(int id)
        {
            return Try(nameof(PaymentPlatformDelete), () =>
            {
                var sql = @"delete from PaymentBank where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 支付平台银行 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public PaymentBank PaymentBankGet(int id)
        {
            return Try(nameof(PaymentBankGet), () =>
            {
                var sql = @"select * from PaymentBank where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<PaymentBank>(cmd);
            });
        }

        /// <summary>
        /// 支付平台银行 列表
        /// </summary>
        /// <param name="platformId">支付平台id</param>
        /// <returns></returns>
        public IList<PaymentBank> PaymentBankList(int platformId)
        {
            return Try(nameof(PaymentBankGet), () =>
            {
                var sql = @"select * from PaymentBank a inner join PaymentPlatform b on a.PlatformId=b.Id inner join PaymentBank c on a.BankId=c.Id where a.PlatformId=@platformId";
                var cmd = SqlBuilder.Raw(sql).ToCommand();
                return BaseConn.Query<PaymentBank, PaymentPlatform, Bank>(sql, new { platformId }).ToList();
            });
        }

        #endregion

        #region 幻灯片

        ///<summary>
        /// 幻灯片 创建
        ///</summary>
        ///<param name="o">幻灯片</param>
        ///<returns></returns>
        public bool SlideCreate(Slide o)
        {
            return Try(nameof(SlideCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Slide")
                .Column("Id", o.Id)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Width", o.Width)
                .Column("Height", o.Height)
                .Column("Thumb", o.Thumb)
                .Column("ThumbW", o.ThumbW)
                .Column("ThumbH", o.ThumbH)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();

                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片 更新
        ///</summary>
        ///<param name="o">Slide</param>
        ///<returns></returns>
        public bool SlideUpdate(Slide o)
        {
            return Try(nameof(SlideUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Slide")
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Width", o.Width)
                .Column("Height", o.Height)
                .Column("Thumb", o.Thumb)
                .Column("ThumbW", o.ThumbW)
                .Column("ThumbH", o.ThumbH)
                .Column("Description", o.Description ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 幻灯片 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool SlideDelete(string id)
        {
            return Try(nameof(SlideDelete), () =>
            {
                var sql = @"delete from SlideItem where SlideId=@id;delete from Slide where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片 是否存在
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public bool SlideExist(string id)
        {
            return Try(nameof(OAuthProviderExist), () =>
            {
                var sql = @"select 1 from Slide where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }

        /// <summary>
        /// 幻灯片 获取
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeItems">是否包含条目</param>
        /// <param name="current">是否筛选当前时间生效的条目</param>
        /// <returns></returns>
        public Slide SlideGet(string id, bool includeItems, bool current = false)
        {
            return Try(nameof(SlideGet), () =>
            {
                if (includeItems)
                {
                    var where = current ? " and StartedOn<=now() and StoppedOn>=now()" : string.Empty;
                    var sql = @"select * from Slide where id=@id;select * from SlideItem where SlideId=@id  and Status=1" + where + ";";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = BaseConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Slide>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<SlideItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var sql = @"select * from Slide where id=@id";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    return BaseConn.QueryFirstOrDefault<Slide>(cmd);
                }
            });
        }

        /// <summary>
        /// 幻灯片 分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>

        public IPagedList<Slide> SlidePagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(SlidePagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("Slide")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })//Regex.IsMatch(keyword, "^\\d+$")
                .Where(status.HasValue, "Status=@status", new { status })               
                .ToCommand(page, size);
                return BaseConn.PagedList<Slide>(page, size, cmd);
            });
        }

        ///<summary>
        /// 幻灯片条目 创建
        ///</summary>
        ///<param name="o">幻灯片条目</param>
        ///<returns></returns>
        public int SlideItemCreate(SlideItem o)
        {
            return Try(nameof(SlideItemCreate), () =>
            {
                var cmd = SqlBuilder.Insert("SlideItem")
                .Column("SlideId", o.SlideId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("NavUrl", o.NavUrl)
                .Column("ImageSrc", o.ImageSrc)
                .Column("ThumbSrc", o.ThumbSrc ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 幻灯片条目 更新
        ///</summary>
        ///<param name="o">幻灯片条目</param>
        ///<returns></returns>
        public bool SlideItemUpdate(SlideItem o)
        {
            return Try(nameof(SlideItemUpdate), () =>
            {
                var cmd = SqlBuilder.Update("SlideItem")
                .Column("SlideId", o.SlideId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("NavUrl", o.NavUrl)
                .Column("ImageSrc", o.ImageSrc)
                .Column("ThumbSrc", o.ThumbSrc ?? string.Empty)
                .Column("Description", o.Description ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 幻灯片条目 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool SlideItemDelete(string id)
        {
            return Try(nameof(SlideItemDelete), () =>
            {
                var sql = @"delete * from SlideItem where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片条目 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public SlideItem SlideItemGet(int id)
        {
            return Try(nameof(SlideItemGet), () =>
            {
                var sql = @"select * from SlideItem where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<SlideItem>(cmd);
            });
        }

        /// <summary>
        /// 幻灯片条目 分页
        /// </summary>
        /// <param name="slideId">幻灯片id</param>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<SlideItem> SlideItemPagedList(string slideId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(SlideItemPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .Where("SlideId=@slideId", new { slideId })
                .From("SlideItem")
                .ToCommand(page, size);
                return BaseConn.PagedList<SlideItem>(page, size, cmd);
            });
        }


        #endregion

        #region 广告模块        

        ///<summary>
        /// 广告模块 创建
        ///</summary>
        ///<param name="o">AdModule</param>
        ///<returns></returns>
        public int AdModuleCreate(AdModule o)
        {
            return Try(nameof(AdModuleCreate), () =>
            {
                var cmd = SqlBuilder.Insert("AdModule")
                .Column("Pid", o.Pid)
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Path", o.Path ?? string.Empty)
                .Column("Depth", o.Depth)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 广告模块 更新
        ///</summary>
        ///<param name="o">AdModule</param>
        ///<returns></returns>
        public bool AdModuleUpdate(AdModule o)
        {
            return Try(nameof(AdModuleUpdate), () =>
            {
                var cmd = SqlBuilder.Update("AdModule")
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Path", o.Path ?? string.Empty)
                .Column("Depth", o.Depth)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }


        /// <summary>
        /// 广告模块 Path 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool AdModulePathUpdate(int id, string path)
        {
            return Try(nameof(AdModulePathUpdate), () =>
            {
                var sql = @"update AdModule set Path=@path where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, path }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 广告模块 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool AdModuleStatusUpdate(int id, bool status)
        {
            return Try(nameof(AdModuleStatusUpdate), () =>
            {
                var sql = @"update AdModule set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 广告模块 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool AdModuleDelete(int id)
        {
            return Try(nameof(AdModuleDelete), () =>
            {
                var sql = @"delete from AdModule where Pid=@id;delete from AdModule where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 广告模块 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public AdModule AdModuleGet(int id)
        {
            return Try(nameof(AdModuleGet), () =>
            {
                var sql = @"select * from AdModule where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<AdModule>(cmd);
            });
        }

        /// <summary>
        /// 广告模块 获取
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeItems">是否包含条目</param>
        /// <param name="current">是否筛选当前时间生效的条目</param>
        /// <returns></returns>
        public AdModule AdModuleGet(int id, bool includeItems, bool current = false)
        {
            return Try(nameof(AdModuleGet), () =>
            {
                if (includeItems)
                {
                    var sql = @"select * from AdModule where id=@id;select * from AdItem where ModuleId=@id";

                    var cmd = SqlBuilder.Raw(sql, new { id })
                    .Append(current, "Status=1 and StartedOn<=now() and StoppedOn>=now()")
                    .Append(";")
                    .ToCommand();
                    using (var reader = BaseConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<AdModule>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<AdItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var sql = @"select * from AdModule where id=@id";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    return BaseConn.QueryFirstOrDefault<AdModule>(cmd);
                }
            });
        }

        /*
        /// <summary>
        /// 广告模块父级信息 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IDictionary<int, string> AdModuleParentDicGet(int id)
        {
            var dic = new Dictionary<int, string>();
            var sql = "SELECT ParentId from AdModule where Id=@id";
            var i = id;
            while (i > 0)
            {
                i = BaseConn.ExecuteScalar<int>(sql, new { id = i });
                dic.Add(i, "");
            }
            return dic;
            /*
            
            var sql = @"
            SELECT b.id, b.name 
            FROM ( 
                SELECT 
                    @r AS id, 
                    (SELECT @r := parentid FROM AdModule WHERE id = @r) AS pid, 
                    @l := @l + 1 AS lv
                FROM 
                    (SELECT @r := 5, @l := 0) vars,AdModule h 
                WHERE @r <> 0) a 
            INNER JOIN AdModule b ON a.id = b.id
            ORDER BY a.lv DESC";
           // var cmd = SqlBuilder.Raw(sql, new { r = id, l = 0 }).ToCommand();
            //var temp = BaseConn.Query(sql, new { r = id, l = 0 });
            //return dic;
            using (var conn = BaseConn as MySql.Data.MySqlClient.MySqlConnection)
            {
                conn.Open();
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
               // cmd.Parameters.AddWithValue("r", id);
                //cmd.Parameters.AddWithValue("l", 0);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dic.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
            return dic;
           
        }
        */

        /// <summary>
        /// 广告模块 列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <param name="includeSelf">是否包含本身</param>
        /// <returns></returns>
        public IList<AdModule> AdModuleList(params int[] ids)
        {
            return Try(nameof(AdModuleList), () =>
            {
                var sql = @"select * from AdModule";
                if (ids != null && ids.Length > 0)
                {
                    if (ids.Length > 1)
                    {
                        sql = "select * from AdModule where id=" + ids[0];
                    }
                    else
                    {
                        var str = string.Join(",", ids);
                        sql = "select * from AdModule where id in (" + str + ")";
                    }
                }
                return BaseConn.Query<AdModule>(sql).ToList();
            });
        }

        public IDictionary<int, int> AdModuleIdDic()
        {
            var dic = new Dictionary<int, int>();

            return Try(nameof(AdModuleIdDic), () =>
            {
                var sql = @"SELECT Id,Pid from AdModule";
                using (var reader = BaseConn.ExecuteReader(sql))
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
        /// 广告模块 分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<AdModule> AdModulePagedList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(AdModulePagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("AdModule")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .ToCommand(page, size);

                return BaseConn.PagedList<AdModule>(page, size, cmd);
            });
        }

        ///<summary>
        /// 广告条目 创建
        ///</summary>
        ///<param name="o">AdItem</param>
        ///<returns></returns>
        public int AdItemCreate(AdItem o)
        {
            return Try(nameof(AdItemCreate), () =>
            {
                var cmd = SqlBuilder.Insert("AdItem")
                .Column("Type", o.Type)
                .Column("ModuleId", o.ModuleId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title??string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("DataKey", o.DataKey ?? string.Empty)
                .Column("DataVal", o.DataVal ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 广告条目 更新
        ///</summary>
        ///<param name="o">AdItem</param>
        ///<returns></returns>
        public bool AdItemUpdate(AdItem o)
        {
            return Try(nameof(AdItemUpdate), () =>
            {
                var cmd = SqlBuilder.Update("AdItem")
                .Column("Type", o.Type)
                .Column("ModuleId", o.ModuleId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title ?? string.Empty)
                .Column("Link", o.Link ?? string.Empty)
                .Column("Icon", o.Icon ?? string.Empty)
                .Column("DataKey", o.DataKey ?? string.Empty)
                .Column("DataVal", o.DataVal ?? string.Empty)
                .Column("Picture", o.Picture ?? string.Empty)
                .Column("Summary", o.Summary ?? string.Empty)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 广告条目 状态 更新
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool AdItemStatusUpdate(int id, bool status)
        {
            return Try(nameof(AdItemStatusUpdate), () =>
            {
                var sql = @"update AdItem set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 广告条目 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool AdItemDelete(int id)
        {
            return Try(nameof(AdItemDelete), () =>
            {
                var sql = @"delete from AdItem where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 广告条目 获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public AdItem AdItemGet(int id)
        {
            return Try(nameof(AdItemGet), () =>
            {
                var sql = @"select * from AdItem where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<AdItem>(cmd);
            });
        }

        /// <summary>
        /// 广告条目 列表
        /// </summary>
        /// <param name="current"></param>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public IList<AdItem> AdItemList(bool current = false, params int[] moduleIds)
        {
            return Try(nameof(AdItemList), () =>
            {
                var cmd = SqlBuilder.Select("*").From("AdItem")
                .Where(current, "Status=1 and StartedOn<=now() and StoppedOn>=now()")
                .Where(moduleIds != null && moduleIds.Length == 1, "ModuleId=" + moduleIds[0])
                .Where(moduleIds != null && moduleIds.Length > 1, "ModuleId in (" + string.Join(",", moduleIds) + ")")
                .ToCommand();
                return BaseConn.Query<AdItem>(cmd).ToList();
            });
        }

        /// <summary>
        /// 广告条目 分页
        /// </summary>
        /// <param name="planId">模块id</param>
        /// <param name="page">页码</param>
        /// <param name="size">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IPagedList<AdItem> AdItemPagedList(int moduleId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(ActItemPagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("AdItem")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .Where(moduleId > 0, "ModuleId=@moduleId", new { moduleId })                
                .ToCommand(page, size);
                return BaseConn.PagedList<AdItem>(page, size, cmd);
            });
        }

        #endregion

    }
}