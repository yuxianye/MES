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
    [Description("服务-产品管理")]
    public class ProductInfoController : ServiceBaseApiController
    {

        /// <summary>
        /// 产品信息契约
        /// </summary>
        public IProductInfoContract ProductInfoContract { get; set; }

        [HttpPost]
        [Description("服务-产品管理-根据ID查询")]
        public IHttpActionResult GetProductInfo(ProductInfo info)
        {
            ProductInfo Info = ProductInfoContract.ProductInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取产品数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-产品管理-分页数据")]
        public IHttpActionResult GetProductInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductInfoContract.ProductInfos, m => new
                {
                    m.Id,
                    m.ProductName,
                    m.ProductCode,
                    ProductType_Id = m.ProductType.Id,
                    m.ProductType.ProductTypeName,
                    m.ProductType.ProductTypeCode,
                    m.Specification,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取产品信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取产品信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-产品管理-增加")]

        public async Task<IHttpActionResult> Add(params ProductInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-产品管理-修改")]
        public async Task<IHttpActionResult> Update(params ProductInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProductInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-产品管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProductInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
