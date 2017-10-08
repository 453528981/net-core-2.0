using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {

        #region 用户
        /// <summary>
        /// 用户创建
        /// </summary>
        /// <param name="o">用户</param>
        /// <returns></returns>
        public int UserCreate(User o)
        {
            return Try(nameof(UserCreate), () =>
            {
                using (var conn = PassportConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Insert("User")
                                .Column("Guid", o.Guid)
                                .Column("Name", o.Name)
                                .Column("Email", o.Email)
                                .Column("Mobile", o.Mobile)
                                .Column("Nickname", o.Nickname)
                                .Column("Password", o.Password)
                                .Column("Role", o.Role)
                                .Column("Grade", o.Grade)
                               //.Column("RoleId", o.RoleId)
                                .Column("ParentId", o.ParentId)
                                .Column("Limitation", o.Limitation)
                                .Column("Permission", o.Permission)
                                .Column("Avatar", o.Avatar)
                                .Column("Status", o.Status)
                                .Column("AuthedOn", o.AuthedOn)
                                .Column("CreatedBy", o.CreatedBy)
                                .Column("CreatedOn", o.CreatedOn)
                                .Column("ModifiedBy", o.ModifiedBy)
                                .Column("ModifiedOn", o.ModifiedOn)
                                .ToCommand(true, tran);

                            var status = false;
                            var id = conn.ExecuteScalar<int>(cmd);

                            if (id > 0 && o.Profile != null)
                            {
                                var profile = o.Profile;
                                cmd = SqlBuilder.Insert("UserProfile")
                                    .Column("Id", id)
                                    .Column("Code", profile.Code)
                                    .Column("Name", profile.Name)
                                    .Column("Gender", profile.Gender)
                                    .Column("Marital", profile.Marital)
                                    .Column("Birthday", profile.Birthday)
                                    .Column("Phone", profile.Phone)
                                    .Column("Mobile", profile.Mobile)
                                    .Column("RegionId", profile.RegionId)
                                    .Column("Street", profile.Street)
                                    .Column("SignUpIp", profile.SignUpIp)
                                    .Column("SignUpBy", profile.SignUpBy)
                                    .Column("TraceCode", profile.TraceCode)
                                    .Column("LastSignInIp", profile.LastSignInIp)
                                    .Column("LastSignInOn", profile.LastSignInOn)
                                    .ToCommand(false, tran);
                                status = conn.Execute(cmd) > 0;

                            }
                            if (id > 0 && status)
                            {
                                tran.Commit();
                                return id;
                            }
                            tran.Rollback();
                            return 0;
                        }
                        catch (System.Exception e)
                        {
                            tran.Rollback();
                            throw e;
                        }
                    }
                }

            });
        }

        /// <summary>
        /// 用户获取
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public User UserGet(int id)
        {
            return Try(nameof(UserGet), () =>
            {
                var sql = @"select * from user where id=@id";

                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();

                return PassportConn.QueryFirstOrDefault<User>(cmd);
            });
        }

        /// <summary>
        /// 用户获取
        /// </summary>
        /// <param name="guid">用户guid</param>
        /// <returns></returns>
        public User UserGet(string guid)
        {
            return Try(nameof(UserGet), () =>
            {
                var sql = @"select * from user where guid=@guid";

                var cmd = SqlBuilder.Raw(sql, new { guid }).ToCommand();

                return PassportConn.QueryFirstOrDefault<User>(cmd);
            });
        }

        /// <summary>
        /// 用户获取
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public User UserGet(string uid, string password)
        {
            return Try(nameof(UserGet), () =>
            {
                var sql = @"select * from user where password=@password and (name=@uid or mobile=@uid or email=@uid)";

                var cmd = SqlBuilder.Raw(sql, new { uid, password }).ToCommand();

                return PassportConn.QueryFirstOrDefault<User>(cmd);
            });
        }

        /// <summary>
        /// 用户Id获取
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号</param>        
        /// <returns></returns>
        public int UserIdGet(string uid)
        {
            return Try(nameof(UserIdGet), () =>
            {
                var sql = @"select id from user where name=@uid or mobile=@uid or email=@uid limit 1";

                var cmd = SqlBuilder.Raw(sql, new { uid }).ToCommand();

                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }


        /// <summary>
        /// 用户Id获取
        /// 用于验证使用第三方帐号登录的用户是否已在系统内创建
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="createdBy">创建者</param>
        /// <returns></returns>
        public int UserIdGet(string name, string createdBy)
        {
            return Try(nameof(UserIdGet), () =>
            {
                var sql = @"select id from user where name=@name and createdBy=@createdBy";

                var cmd = SqlBuilder.Raw(sql, new { name, createdBy }).ToCommand();

                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 用户名是否已存在
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号</param>
        /// <returns></returns>
        public bool UserExist(string uid)
        {
            return Try(nameof(UserExist), () =>
            {
                var sql = @"select 0 from user where name=@uid 
                    union
                    select 0 from user where mobile=@uid
                    union
                    select 0 from user where email=@uid
                    ";

                var cmd = SqlBuilder.Raw(sql, new { uid }).ToCommand();

                return PassportConn.Query<int>(cmd).Any();
            });
        }


        /// <summary>
        /// 用户昵称更新
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="nickname">昵称</param>
        /// <returns></returns>
        public bool UserNicknameUpdate(int id, string nickname)
        {
            return Try(nameof(UserNicknameUpdate), () =>
            {
                var sql = @"update user set nickname=@nickname where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id, nickname }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 用户登录密码更新
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public bool UserPasswordUpdate(int id, string password)
        {
            return Try(nameof(UserPasswordUpdate), () =>
            {
                var sql = @"update user set password=@password where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id, password }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 用户绑定手机号码更新
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="mobile">有效手机号码</param>
        /// <returns></returns>
        public bool UserMobileUpdate(int id, string mobile)
        {
            return Try(nameof(UserMobileUpdate), () =>
            {
                var sql = @"update user set mobile=@mobile where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id, mobile }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 用户绑定Email更新
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="email">有效电子邮箱</param>
        /// <returns></returns>
        public bool UserEmailUpdate(int id, string email)
        {
            return Try(nameof(UserEmailUpdate), () =>
            {
                var sql = @"update user set email=@email where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id, email }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 用户图像更新
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="avatar">图像url</param>
        /// <returns></returns>
        public bool UserAvatarUpdate(int id, bool avatar)
        {
            return Try(nameof(UserEmailUpdate), () =>
            {
                var sql = @"update user set avatar=@avatar where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id, avatar }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        #endregion

        #region 用户扩展信息

        ///<summary>
        /// 用户扩展信息更新
        ///</summary>
        ///<param name="o">用户扩展信息</param>
        ///<returns></returns>
        public bool UserProfileUpdate(UserProfile o)
        {
            return Try(nameof(UserProfileUpdate), () =>
            {
                var cmd = SqlBuilder.Update("UserProfile")
                .Column("Code", o.Code)
                .Column("Name", o.Name)
                .Column("Gender", o.Gender)
                .Column("Marital", o.Marital)
                .Column("Birthday", o.Birthday)
                .Column("Phone", o.Phone)
                .Column("Mobile", o.Mobile)
                .Column("RegionId", o.RegionId)
                .Column("Street", o.Street)
                .Column("SignUpIp", o.SignUpIp)
                .Column("SignUpBy", o.SignUpBy)
                .Column("TraceCode", o.TraceCode)
                .Column("LastSignInIp", o.LastSignInIp)
                .Column("LastSignInOn", o.LastSignInOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户扩展信息获取
        ///</summary>
        ///<param name="id">用户id</param>
        ///<returns></returns>
        public UserProfile UserProfileGet(int id)
        {
            return Try(nameof(UserProfileGet), () =>
            {
                var sql = @"select * from userprofile where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserProfile>(cmd);
            });
        }

        #endregion

        #region 用户预注册信息

        ///<summary>
        /// 用户预注册信息创建
        ///</summary>
        ///<param name="o">用户预注册信息</param>
        ///<returns></returns>
        public int UserPreCreate(UserPre o)
        {
            return Try(nameof(UserPreCreate), () =>
            {
                var cmd = SqlBuilder.Insert("UserPre")
                .Column("Id", o.Id)
                .Column("Name", o.Name)
                .Column("Password", o.Password)
                .Column("Browser", o.Browser)
                .Column("UserAgent", o.UserAgent)
                .Column("IpAddress", o.IpAddress)
                .Column("UrlReferrer", o.UrlReferrer)
                .Column("MediaId", o.MediaId)
                .Column("TraceCode", o.TraceCode)
                .Column("CreatedOn", o.CreatedOn)
                .ToCommand(true);
                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 用户预注册信息获取
        ///</summary>
        ///<param name="id">用户预注册信息id</param>
        ///<returns></returns>
        public UserPre UserPreGet(string id)
        {
            return Try(nameof(UserPreGet), () =>
            {
                var sql = @"select * from UserPre where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserPre>(cmd);
            });
        }

        #endregion

        #region 用户第三方授权信息 

        ///<summary>
        /// 用户第三方授权信息创建
        ///</summary>
        ///<param name="o">用户第三方授权信息</param>
        ///<returns></returns>
        public bool UserOAuthCreate(UserOAuth o)
        {
            return Try(nameof(UserOAuthCreate), () =>
            {
                var cmd = SqlBuilder.Insert("UserOAuth")
                .Column("Id", o.Id)
                .Column("OpenId", o.OpenId)
                .Column("Provider", o.Provider)
                .Column("OpenName", o.OpenName)
                .Column("Scope", o.Scope)
                .Column("AccessToken", o.AccessToken)
                .Column("RefreshToken", o.RefreshToken)
                .Column("ExpiredOn", o.ExpiredOn)
                .Column("Extra", o.Extra)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户第三方授权信息更新
        ///</summary>
        ///<param name="o">用户第三方授权信息</param>
        ///<returns></returns>
        public bool UserOAuthUpdate(UserOAuth o)
        {
            return Try(nameof(UserOAuthUpdate), () =>
            {
                var cmd = SqlBuilder.Update("UserOAuth")
                .Column("OpenName", o.OpenName)
                .Column("Scope", o.Scope)
                .Column("AccessToken", o.AccessToken)
                .Column("RefreshToken", o.RefreshToken)
                .Column("ExpiredOn", o.ExpiredOn)
                .Column("Extra", o.Extra)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id and OpenId=@openId and Provider=@provider", new { o.Id, o.OpenId, o.Provider })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户第三方授权信息更新获取
        ///</summary>
        ///<param name="provider">provider</param>
        ///<param name="openId">openId</param>
        ///<returns></returns>
        public UserOAuth UserOAuthGet(string provider, string openId)
        {
            return Try(nameof(UserOAuthGet), () =>
            {
                var sql = @"select * from UserOAuth where provider=@provider and  openId=@openId";
                var cmd = SqlBuilder.Raw(sql, new { openId, provider }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserOAuth>(cmd);
            });
        }

        #endregion

        #region 用户地址
        ///<summary>
        /// 用户地址创建
        ///</summary>
        ///<param name="o">UserAddress</param>
        ///<returns></returns>
        public int UserAddressCreate(UserAddress o)
        {
            return Try(nameof(UserAddressCreate), () =>
            {
                var cmd = SqlBuilder.Insert("UserAddress")
                .Column("UserId", o.UserId)
                .Column("GroupId", o.GroupId)
                .Column("Consignee", o.Consignee)
                .Column("CompanyName", o.CompanyName)
                .Column("CountryId", o.CountryId)
                .Column("RegionId", o.RegionId)
                .Column("Province", o.Province)
                .Column("City", o.City)
                .Column("District", o.District)
                .Column("Street", o.Street)
                .Column("PostalCode", o.PostalCode)
                .Column("Phone", o.Phone)
                .Column("Mobile", o.Mobile)
                .Column("IsDefault", o.IsDefault)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 用户地址更新
        ///</summary>
        ///<param name="o">UserAddress</param>
        ///<returns></returns>
        public bool UserAddressUpdate(UserAddress o)
        {
            return Try(nameof(UserAddressUpdate), () =>
            {
                var cmd = SqlBuilder.Update("UserAddress")
                //.Column("GroupId", o.GroupId)
                .Column("Consignee", o.Consignee)
                .Column("CompanyName", o.CompanyName ?? string.Empty)
                //.Column("CountryId", o.CountryId)
                .Column("RegionId", o.RegionId)
                .Column("Province", o.Province)
                .Column("City", o.City)
                .Column("District", o.District)
                .Column("Street", o.Street)
                .Column("PostalCode", o.PostalCode)
                .Column("Phone", o.Phone ?? string.Empty)
                .Column("Mobile", o.Mobile ?? string.Empty)
                .Column("IsDefault", o.IsDefault)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id and UserId=@userId", new { o.Id, o.UserId })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户地址获取
        ///</summary>
        ///<param name="id">用户地址id</param>
        ///<returns></returns>
        public UserAddress UserAddressGet(int id)
        {
            return Try(nameof(UserAddressGet), () =>
            {
                var sql = @"select * from UserAddress where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserAddress>(cmd);
            });
        }

        ///<summary>
        /// 用户地址获取
        ///</summary>
        ///<param name="id">用户地址id</param>
        ///<param name="userId">用户id</param>
        ///<returns></returns>
        public UserAddress UserAddressGet(int id, int userId)
        {
            return Try(nameof(UserAddressGet), () =>
            {
                var sql = @"select * from UserAddress where Id=@id and UserId=@userId";
                var cmd = SqlBuilder.Raw(sql, new { id, userId }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserAddress>(cmd);
            });
        }

        ///<summary>
        /// 用户地址获取
        ///</summary>
        ///<param name="userId">用户id</param>
        ///<returns></returns>
        public IList<UserAddress> UserAddressList(int userId)
        {
            return UserAddressList(userId, 0);
        }

        /// <summary>
        /// 用户地址获取
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="groupId">地址分组 0为收货地址 2发货地址 3退货地址</param>
        /// <returns></returns>
        public IList<UserAddress> UserAddressList(int userId, byte? groupId = null)
        {
            return Try(nameof(UserAddressList), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("UserAddress")
                .Where("UserId=@userId", new { userId })
                .Where(groupId.HasValue, "GroupId=@groupId", new { groupId })
                .ToCommand();
                return PassportConn.Query<UserAddress>(cmd).ToList();
            });
        }

        /// <summary>
        /// 用户地址删除
        /// </summary>
        /// <param name="id">用户地址id</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public bool UserAddressDelete(int id, int? userId)
        {
            return Try(nameof(UserAddressDelete), () =>
            {
                var cmd = SqlBuilder.Delete("UserAddress")
                .Where("Id=@id", new { id })
                .Where(userId.HasValue, "UserId=@userId", new { userId })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 用户默认地址设置
        /// </summary>
        /// <param name="id">用户地址id</param>
        /// <param name="userId">用户id</param>
        /// <param name="groupId">地址分组 0为收货地址 2发货地址 3退货地址</param>
        /// <returns></returns>
        public bool UserAddressSetDefault(int id, int userId, byte groupId)
        {
            var sql = @"update UserAddress set IsDefault=0 where UserId=@userId and GroupId=@groupId and Id<>@id;
                        update UserAddress set IsDefault=1 where UserId=@userId and GroupId=@groupId and Id=@id;";

            return Try(nameof(UserAddressSetDefault), () =>
            {
                var cmd = SqlBuilder.Raw(sql, new { id, userId, groupId }).ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }


        /// <summary>
        /// 用户地址数量获取
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="groupId">地址分组 0为收货地址 2发货地址 3退货地址</param>
        /// <returns></returns>
        public int UserAddressCount(int userId, byte groupId)
        {
            return Try(nameof(UserAddressGet), () =>
            {
                var sql = @"select count(0) as Count from UserAddress where UserId=@userId and GroupId=@groupId";
                var cmd = SqlBuilder.Raw(sql, new { userId, groupId }).ToCommand();
                return PassportConn.QueryFirstOrDefault<int>(cmd);
            });
        }

        #endregion

        #region 用户发票模版
        ///<summary>
        /// 用户发票模版创建
        ///</summary>
        ///<param name="o">用户发票模版</param>
        ///<returns></returns>
        public int UserInvoiceCreate(UserInvoice o)
        {
            return Try(nameof(UserInvoiceCreate), () =>
            {
                var cmd = SqlBuilder.Insert("UserInvoice")
                .Column("UserId", o.UserId)
                .Column("GroupId", o.GroupId)
                .Column("Title", o.Title)
                .Column("Content", o.Content)
                .Column("IsDefault", o.IsDefault)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 用户发票模版更新
        ///</summary>
        ///<param name="o">用户发票模版</param>
        ///<returns></returns>
        public bool UserInvoiceUpdate(UserInvoice o)
        {
            return Try(nameof(UserInvoiceUpdate), () =>
            {
                var cmd = SqlBuilder.Update("UserInvoice")
                .Column("UserId", o.UserId)
                .Column("GroupId", o.GroupId)
                .Column("Title", o.Title)
                .Column("Content", o.Content)
                .Column("IsDefault", o.IsDefault)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id and UserId=@userId", new { o.Id, o.UserId })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户发票模版获取
        ///</summary>
        ///<param name="id">用户发票模版id</param>
        ///<returns></returns>
        public UserInvoice UserInvoiceGet(int id)
        {
            return Try(nameof(UserInvoiceGet), () =>
            {
                var sql = @"select * from UserInvoice where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserInvoice>(cmd);
            });
        }

        #endregion        

        #region 用户收藏信息 
        ///<summary>
        /// 用户收藏信息创建
        ///</summary>
        ///<param name="o">用户收藏信息</param>
        ///<returns></returns>
        public int UserFavoriteCreate(UserFavorite o)
        {
            return Try(nameof(UserFavoriteCreate), () =>
            {
                var cmd = SqlBuilder.Insert("UserFavorite")
                .Column("UserId", o.UserId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Value", o.Value)
                .Column("Extra", o.Extra)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return PassportConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 用户收藏信息更新
        ///</summary>
        ///<param name="o">用户收藏信息</param>
        ///<returns></returns>
        public bool UserFavoriteUpdate(UserFavorite o)
        {
            return Try(nameof(UserFavoriteUpdate), () =>
            {
                var cmd = SqlBuilder.Update("UserFavorite")
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Value", o.Value)
                .Column("Extra", o.Extra)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id and UserId=@userId", new { o.Id, o.UserId })
                .ToCommand();
                return PassportConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 用户收藏信息获取
        ///</summary>
        ///<param name="id">用户收藏信息id</param>
        ///<returns></returns>
        public UserFavorite UserFavoriteGet(int id)
        {
            return Try(nameof(UserFavoriteGet), () =>
            {
                var sql = @"select * from UserFavorite where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return PassportConn.QueryFirstOrDefault<UserFavorite>(cmd);
            });
        }

        ///<summary>
        /// 用户收藏信息获取
        ///</summary>
        ///<param name="userId">用户id</param>
        ///<returns></returns>
        public IList<UserFavorite> UserFavoriteList(int userId)
        {
            return Try(nameof(UserFavoriteList), () =>
            {
                var sql = @"select * from UserFavorite where userId=@userId";
                var cmd = SqlBuilder.Raw(sql, new { userId }).ToCommand();
                return PassportConn.Query<UserFavorite>(cmd).ToList();
            });
        }

        #endregion
    }
}