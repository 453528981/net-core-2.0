using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("address")]
    public class AddressController : BaseController
    {
        public AddressController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<AddressController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        /// <summary>
        /// 收货地址列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public ActionResult AddressList()
        {
            var identity = User;
            var model = new AddressListModel();
            var addresses = DefaultStorage.UserAddressList(identity.Id, 0).OrderByDescending(x=>x.IsDefault);
            model.Addresses = addresses.ToList();
            return View(model);
        }

        /// <summary>
        /// 收货地址详情
        /// </summary>
        /// <param name="id">地址id</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public IActionResult AddressDetail(int id)
        {
            var model = new AddressDetailModel();
            model.Address = new UserAddress();
            model.Provinces = DefaultStorage.RegionList("86");
            model.Citys = new List<Region>();
            model.Districts = new List<Region>();
            if (id > 0)
            {
                var address = DefaultStorage.UserAddressGet(id, User.Id);

                model.Address = address;
                model.Citys = DefaultStorage.RegionList(address.ProvinceId);
                model.Districts = DefaultStorage.RegionList(address.CityId);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 新增/编辑地址
        /// </summary>
        /// <param name="id">地址id</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("detail/{id}")]
        public async Task<IActionResult> AddressDetail(int id, UserAddress model)
        {
            var result = new Result();
            if (id < 0)
            {
                result.Message = "参数有误（id）！";
                return Json(result);
            }
            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.Consignee))
            {
                result.Message = "请填写收货人！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.RegionId))
            {
                result.Message = "请选择省/市/区！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.Street))
            {
                result.Message = "请填写详细地址！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.Phone) && string.IsNullOrEmpty(model.Mobile))
            {
                result.Message = "请填写移动电话或固定电话！";
                return Json(result);
            }

            //获取省市区名称
            var list = RegionList();
            var pid = model.RegionId.Substring(0, 2);
            var cid = model.RegionId.Substring(0, 4);
            var dic = model.RegionId;
            var province = list.FirstOrDefault(x => x.Id == pid).Name;
            var city = list.FirstOrDefault(x => x.Id == cid).Name;
            var district = list.FirstOrDefault(x => x.Id == dic).Name;

            if (id > 0)
            {
                var old = DefaultStorage.UserAddressGet(id, identity.Id);
                var updated = await TryUpdateModelAsync(old);
                if (!updated)
                {
                    result.Message = "未登录或登录超时！";
                    return Json(result);
                }

                old.Province = province;
                old.City = city;
                old.District = district;

                var status = DefaultStorage.UserAddressUpdate(old);
                if (status)
                {
                    result.Status = true;
                }
                else
                {
                    result.Message = "更新收货地址失败！";
                }
            }
            else
            {
                var max = 8;
                var count = DefaultStorage.UserAddressCount(identity.Id, 0);
                if (count > max)
                {
                    result.Message = "超出可设置收货地址数量限制！";
                    return Json(result);
                }

                var address = new UserAddress();

                address.UserId = identity.Id;
                address.GroupId = 0;
                address.Consignee = model.Consignee;
                address.CompanyName = model.CompanyName ?? "";
                address.CountryId = 86;
                address.RegionId = model.RegionId;
                address.Province = province;
                address.City = city;
                address.District = district;
                address.Street = model.Street;
                address.PostalCode = model.PostalCode;
                address.Phone = model.Phone;
                address.Mobile = model.Mobile;
                address.IsDefault = model.IsDefault;
                address.CreatedOn = DateTime.Now;
                address.ModifiedBy = identity.Name;
                address.ModifiedOn = DateTime.Now;
                var newId = DefaultStorage.UserAddressCreate(address);
                if (newId > 0)
                {
                    result.Status = true;
                }
                else
                {
                    result.Message = "添加收货地址失败！";
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="id">地址id</param>
        /// <returns></returns>
        [HttpPost("set-default/{id}")]
        public ActionResult SetDefault(int id)
        {
            var result = new Result();

            if (id < 0)
            {
                result.Message = "参数有误！";
                return Json(result);
            }

            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }

            var status = DefaultStorage.UserAddressSetDefault(id, identity.Id, 0);
            if (status)
            {
                result.Status = true;
            }
            else
            {
                result.Message = "设置失败，请稍后重试！";
            }
            return Json(result);
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="id">地址id</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var result = new Result();
            if (id < 0)
            {
                result.Message = "参数有误！";
                return Json(result);
            }

            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }
            var status = DefaultStorage.UserAddressDelete(id, identity.Id);
            if (status)
            {
                result.Status = true;
            }
            else
            {
                result.Message = "删除失败，请稍后重试！";
            }
            return Json(result);
        }
    }
}
