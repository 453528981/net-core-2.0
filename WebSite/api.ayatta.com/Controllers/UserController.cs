using System;
using Ayatta.Api;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;


namespace Ayatta.Web.Controllers
{
    public class UserController : BaseController
    {

        public UserController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<UserController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

        [HttpPost("user-get")]
        public UserGetResponse UserGet([FromBody]UserGetRequest req)
        {
            var rep = new UserGetResponse();

            rep.Data = DefaultStorage.UserGet(req.Id);

            return rep;
        }

        #region 用户是否已存在
        /// <summary>
        /// 用户是否已存在
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-exist")]
        public UserExistResponse UserExist([FromBody]UserExistRequest req)
        {
            var rep = new UserExistResponse();
            if (req.Uid.IsEmpty())
            {
                rep.Error("请输入用户名");
                return rep;
            }
            rep.Data = DefaultStorage.UserExist(req.Uid);

            return rep;
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-signin")]
        public UserSignInResponse UserSignIn([FromBody]UserSignInRequest req)
        {
            var rep = new UserSignInResponse();

            if (req.IsOAuth)
            {
                var param = req.OAuthParam;
                var temp = DefaultStorage.UserOAuthGet(param.Provider, param.OpenId);
                if (temp != null)
                {
                    rep.Data = DefaultStorage.UserGet(temp.Id);
                    if (rep.Data != null)
                    {
                        Task.Factory.StartNew(() => SyncUserOAuth(temp.Id, param));
                    }
                }
                else
                {
                    var user = new User();
                    var profile = new UserProfile();

                    var now = DateTime.Now;

                    user.Guid = Guid.NewGuid().ToString();// ObjectId.New();
                    user.Name = param.OpenId;
                    user.Email = string.Empty;
                    user.Mobile = string.Empty;
                    user.Nickname = param.OpenName;
                    user.Password = string.Empty;
                    user.Role = UserRole.Buyer;
                    user.Grade = UserGrade.One;
                    user.Limitation = UserLimitation.None;
                    user.Permission = UserPermission.None;
                    user.Avatar = string.Empty;
                    user.Status = UserStatus.Normal;
                    user.AuthedOn = null;
                    user.CreatedBy = param.Provider;
                    user.CreatedOn = now;
                    user.ModifiedBy = "app.api";

                    profile.Code = string.Empty;
                    profile.Name = string.Empty;
                    profile.Gender = Gender.Secrect;
                    profile.Marital = Marital.Secrect;
                    profile.Birthday = null;
                    profile.Phone = string.Empty;
                    profile.Mobile = string.Empty;
                    profile.RegionId = string.Empty;
                    profile.Street = string.Empty;
                    profile.SignUpIp = req.Ip;
                    profile.SignUpBy = 0;
                    profile.TraceCode = req.TraceCode;
                    profile.LastSignInIp = req.Ip;
                    profile.LastSignInOn = now;

                    user.Profile = profile;

                    var id = DefaultStorage.UserCreate(user);
                    if (id > 0)
                    {
                        user.Id = id;
                        profile.Id = id;
                        rep.Data = user;
                        Task.Factory.StartNew(() => CreateUserOAuth(id, param));
                    }
                    else
                    {
                        rep.Error("关联用户失败", 201);
                    }
                }
            }
            else
            {
                if (req.Uid.IsEmpty())
                {
                    rep.Error("请输入登录帐号");
                    return rep;
                }
                if (req.Pwd.IsEmpty())
                {
                    rep.Error("请输入登录密码");
                    return rep;
                }
                var user = DefaultStorage.UserGet(req.Uid, req.Pwd);
                if (user != null)
                {
                    rep.Data = user;
                }
                else
                {
                    rep.Error("用户名或密码错误", 200);
                    return rep;
                }
            }
            return rep;

        }

        private bool SyncUserOAuth(int userId, UserSignInRequest.AuthParam param)
        {
            var o = new UserOAuth();
            o.Id = userId;
            o.Provider = param.Provider;
            o.AccessToken = param.AccessToken;
            o.RefreshToken = param.RefreshToken;
            o.ExpiredOn = DateTime.Now.AddSeconds(param.Expire);
            return DefaultStorage.UserOAuthUpdate(o);
        }

        private bool CreateUserOAuth(int userId, UserSignInRequest.AuthParam param)
        {
            var now = DateTime.Now;
            var o = new UserOAuth();
            o.Id = userId;
            o.OpenId = param.OpenId;
            o.Provider = param.Provider;
            o.OpenName = param.OpenName;
            o.Scope = param.Scope;
            o.AccessToken = param.AccessToken;
            o.RefreshToken = param.RefreshToken;
            o.ExpiredOn = DateTime.Now.AddSeconds(param.Expire);
            o.CreatedOn = now;
            o.ModifiedBy = string.Empty;
            o.ModifiedOn = now;
            return DefaultStorage.UserOAuthCreate(o);
        }

        #endregion

        #region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-signup")]
        public UserSignUpResponse UserSignUp([FromBody]UserSignUpRequest req)
        {
            var time = DateTime.Now;
            var rep = new UserSignUpResponse();

            if (!req.Mobile.IsMobile())
            {
                rep.Error("请输入正确的手机号码");
                return rep;
            }
            if (!req.Pwd.IsPassword())
            {
                rep.Error("密码为6-18个字符（至少包含两个字母、两个数字、一个特殊字符）");
                return rep;
            }

            var valid = ValidateSmsCaptcha(req.Guid, req.Mobile, req.Captcha);
            if (!valid)
            {
                rep.Error("短信验证码不正确或已过期");
                return rep;
            }

            var userId = DefaultStorage.UserIdGet(req.Uid);
            if (userId > 0)
            {
                rep.Error("该手机号码已被使用，请换一个");
                return rep;
            }
            var pwd = req.Pwd;

            var user = new User();
            var profile = new UserProfile();
            //var extra = new UserExtra();

            user.Guid = Guid.NewGuid().ToString();
            user.Name = req.Uid;
            user.Email = string.Empty;
            user.Mobile = req.Mobile;
            user.Nickname = string.Empty;
            user.Password = pwd;

            user.Role = UserRole.Buyer;
            user.Grade = UserGrade.None;
            user.Limitation = UserLimitation.None;
            user.Permission = UserPermission.None;

            user.Avatar = string.Empty;
            user.Status = UserStatus.Normal;

            user.AuthedOn = null;
            user.CreatedBy = string.Empty;
            user.CreatedOn = time;
            user.ModifiedBy = string.Empty;
            user.ModifiedOn = time;

            profile.Code = string.Empty;
            profile.Name = string.Empty;
            profile.Gender = Gender.Secrect;
            profile.Marital = Marital.Secrect;
            profile.Birthday = null;
            profile.Phone = string.Empty;
            profile.Mobile = string.Empty;
            profile.RegionId = string.Empty;
            profile.Street = string.Empty;

            profile.SignUpIp = string.Empty;
            profile.SignUpBy = 1;
            profile.TraceCode = "";
            profile.LastSignInIp = string.Empty;
            profile.LastSignInOn = time;

            user.Profile = profile;
            //user.Extra = extra;

            var id = DefaultStorage.UserCreate(user);
            if (id > 0)
            {
                rep.Data = DefaultStorage.UserGet(id);
            }
            else
            {
                rep.Error("注册失败，请稍后再试");
                return rep;
            }

            return rep;
        }

        #endregion

        #region 收货地址

        /// <summary>
        /// 收货地址列表
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-address-list")]
        public UserAddressListResponse UserAddressList([FromBody]UserAddressListRequest req)
        {
            var rep = new UserAddressListResponse();
            if (req.UserId < 0)
            {
                rep.Error("参数（UserId）有误");
                return rep;
            }
            var data = DefaultStorage.UserAddressList(req.UserId, 0);

            rep.Data = data.OrderByDescending(x => x.IsDefault).ToList();
            return rep;
        }

        /// <summary>
        /// 收货地址创建
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-address-create")]
        public UserAddressCreateResponse UserAddressCreate([FromBody]UserAddressCreateRequest req)
        {
            var max = 8;
            var now = DateTime.Now;

            var rep = new UserAddressCreateResponse();
            if (req.UserId < 0)
            {
                rep.Error("参数（UserId）有误");
                return rep;
            }
            var count = DefaultStorage.UserAddressCount(req.UserId, 0);
            if (count > max)
            {
                rep.Data = max;
                rep.Error("超出可设置收货地址数量限制");
                return rep;
            }

            //获取省市区名称
            var list = RegionList();
            var pid = req.RegionId.Substring(0, 2) + "0000";
            var cid = req.RegionId.Substring(0, 4) + "00";
            var dic = req.RegionId;
            var province = list.FirstOrDefault(x => x.Id == pid).Name;
            var city = list.FirstOrDefault(x => x.Id == cid).Name;
            var district = list.FirstOrDefault(x => x.Id == dic).Name;
            var o = new UserAddress
            {
                UserId = req.UserId,
                GroupId = 0,
                Consignee = req.Consignee,
                CompanyName = req.CompanyName,
                CountryId = 86,
                RegionId = req.RegionId,
                Province = province,
                City = city,
                District = district,
                Street = req.Street,
                PostalCode = req.PostalCode,
                Phone = req.Phone,
                Mobile = req.Mobile,
                IsDefault = req.IsDefault,
                CreatedOn = now,
                ModifiedBy = "",
                ModifiedOn = now
            };
            var id = DefaultStorage.UserAddressCreate(o);
            if (id > 0)
            {
                rep.Data = id;//返回用户地址表自增长id
                if (o.IsDefault)
                {
                    DefaultStorage.UserAddressSetDefault(id, o.UserId, 0);
                }
            }
            else
            {
                rep.Error("创建收货地址失败");
                return rep;
            }
            return rep;
        }

        /// <summary>
        /// 收货地址更新
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-address-update")]
        public UserAddressUpdateResponse UserAddressUpdate([FromBody]UserAddressUpdateRequest req)
        {
            var now = DateTime.Now;

            var rep = new UserAddressUpdateResponse();
            if (req.Id < 0)
            {
                rep.Error("参数（Id）有误");
                return rep;
            }
            if (req.UserId < 0)
            {
                rep.Error("参数（UserId）有误");
                return rep;
            }
            var o = DefaultStorage.UserAddressGet(req.Id, req.UserId);
            if (o != null)
            {
                //获取省市区名称
                var list = RegionList();

                var pid = req.RegionId.Substring(0, 2) + "0000";
                var cid = req.RegionId.Substring(0, 4) + "00";
                var dic = req.RegionId;
                var province = list.FirstOrDefault(x => x.Id == pid).Name;
                var city = list.FirstOrDefault(x => x.Id == cid).Name;
                var district = list.FirstOrDefault(x => x.Id == dic).Name;

                o.Consignee = req.Consignee;
                o.CompanyName = req.CompanyName;
                o.CountryId = 86;
                o.RegionId = req.RegionId;
                o.Province = province;
                o.City = city;
                o.District = district;
                o.Street = req.Street;
                o.PostalCode = req.PostalCode;
                o.Phone = req.Phone;
                o.Mobile = req.Mobile;
                o.IsDefault = req.IsDefault;
                o.ModifiedBy = req.UserId.ToString();
                o.ModifiedOn = now;

                var statue = DefaultStorage.UserAddressUpdate(o);
                if (!statue)
                {
                    rep.Error("创建收货地址失败");
                    return rep;
                }
                if (o.IsDefault)
                {
                    DefaultStorage.UserAddressSetDefault(o.Id, o.UserId, 0);
                }
            }

            else
            {
                rep.Error("未找到相应地址信息");
                return rep;
            }

            return rep;
        }

        /// <summary>
        /// 收货地址更新
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-address-delete")]
        public UserAddressDeleteResponse UserAddressDelete([FromBody]UserAddressDeleteRequest req)
        {
            var now = DateTime.Now;

            var rep = new UserAddressDeleteResponse();
            if (req.Id < 0)
            {
                rep.Error("参数（Id）有误");
                return rep;
            }
            if (req.UserId < 0)
            {
                rep.Error("参数（UserId）有误");
                return rep;
            }
            var statue = DefaultStorage.UserAddressDelete(req.Id, req.UserId);
            if (!statue)
            {
                rep.Error("删除收货地址失败");
                return rep;
            }
            return rep;
        }

        /// <summary>
        /// 收货地址设为默认
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("user-address-set-default")]
        public UserAddressSetDefaultResponse UserAddressSetDefault([FromBody]UserAddressSetDefaultRequest req)
        {
            var rep = new UserAddressSetDefaultResponse();
            if (req.Id < 0)
            {
                rep.Error("参数（Id）有误");
                return rep;
            }
            if (req.UserId < 0)
            {
                rep.Error("参数（UserId）有误");
                return rep;
            }
            var statue = DefaultStorage.UserAddressSetDefault(req.Id, req.UserId, 0);

            if (!statue)
            {
                rep.Error("设置默认收货地址失败");
                return rep;
            }

            return rep;
        }

        #endregion

        #region
        /// <summary>
        /// 校验短信验证码是否有效
        /// </summary>
        /// <param name="guid">guid</param>
        /// <param name="mobile">手机号</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        private bool ValidateSmsCaptcha(string guid, string mobile, string captcha)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(captcha))
            {
                return false;
            }
            var key = $"{mobile}-{guid}";
            return DefaultCache.GetString(key) == captcha;
        }
        #endregion

    }
}