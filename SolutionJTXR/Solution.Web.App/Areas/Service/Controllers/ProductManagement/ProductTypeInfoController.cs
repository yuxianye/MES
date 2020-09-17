using OSharp.Utility.Data;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-产品类别")]
    public class ProductTypeInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 产品类别信息契约
        /// </summary>
        public IProductTypeInfoContract ProductTypeInfoContract { get; set; }

        [HttpPost]
        [Description("服务-产品类别-根据ID查询")]
        public IHttpActionResult GetProductTypeInfo(ProductTypeInfo info)
        {
            ProductTypeInfo Info = ProductTypeInfoContract.ProductTypeInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取产品类别数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-产品类别-分页数据")]
        public IHttpActionResult GetProductTypeList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductTypeInfoContract.ProductTypeInfos, m => new
                {
                    m.Id,
                    m.ProductTypeName,
                    m.ProductTypeCode,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取产品类别信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取产品类别信息列表数据失败！" + ex.ToString()));
            }
        }       

        [Description("服务-产品类别-增加")]

        public async Task<IHttpActionResult> Add(params ProductTypeInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductTypeInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-产品类别-修改")]
        public async Task<IHttpActionResult> Update(params ProductTypeInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProductTypeInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-产品类别-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProductTypeInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
