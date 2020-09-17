﻿using OSharp.Utility.Data;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-设备类别")]
    public class EquipmentTypeInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 设备类别信息契约
        /// </summary>
        public IEquipmentTypeInfoContract EquipmentTypeInfoContract { get; set; }

        [Description("服务-设备类别-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EquipmentTypeInfoContract.EquipmentTypeInfos, m => new
                {
                    m.Id,
                    m.EquipmentTypeName,
                    m.EquipmentTypeCode,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备类别列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备类别列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-设备类别-增加")]
        public async Task<IHttpActionResult> Add(params EquipmentTypeInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquipmentTypeInfoContract.AddEquipmentType(dto);
            return Json(result);
        }

        [Description("服务-设备类别-修改")]
        public async Task<IHttpActionResult> Update(params EquipmentTypeInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });

            var result = await EquipmentTypeInfoContract.UpdateEquipmentType(dto);
            return Json(result);
        }


        [Description("服务-设备类别-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquipmentTypeInfoContract.DeleteEquipmentType(ids);
            return Json(result);
        }
    }
}
