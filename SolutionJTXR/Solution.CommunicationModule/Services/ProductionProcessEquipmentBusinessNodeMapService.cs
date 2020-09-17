using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Services
{
    public class ProductionProcessEquipmentBusinessNodeMapService : IProductionProcessEquipmentBusinessNodeMapContract
    {
        public IRepository<ProductionProcessEquipmentBusinessNodeMap, Guid> ProductionProcessEquipmentBusinessNodeMapRepository { get; set; }
        public IRepository<DeviceNode, Guid> DeviceNodeRepository { get; set; }
        public IRepository<BusinessNode, Guid> BusinessNodeRepository { get; set; }

        /// <summary>
        /// 获取OpcUa业务数据关联数据点信息查询数据集
        /// </summary>
        public IQueryable<ProductionProcessEquipmentBusinessNodeMap> BusinessNodeMaps => ProductionProcessEquipmentBusinessNodeMapRepository.Entities;

        /// <summary>
        /// 添加OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddBusinessNodeMaps(params ProductionProcessEquipmentBusinessNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessEquipmentBusinessNodeMapRepository.InsertAsync(inputDtos);
            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 检查OpcUa业务数据关联数据点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>OpcUa业务数据关联数据点信息是否存在</returns>
        public bool CheckBusinessNodeMapExists(Expression<Func<ProductionProcessEquipmentBusinessNodeMap, bool>> predicate, Guid id) => ProductionProcessEquipmentBusinessNodeMapRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteBusinessNodeMaps(params Guid[] ids)
        {
            ids.CheckNotNull("ids");

            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessEquipmentBusinessNodeMapRepository.DeleteAsync(ids);
            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新OpcUa业务数据关联数据点信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditBusinessNodeMaps(params ProductionProcessEquipmentBusinessNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessEquipmentBusinessNodeMapRepository.UpdateAsync(inputDtos);
            ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }
        /// <summary>
        /// 设备业务数据点配置
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Setting(params EquipmentBusinessNodeMapManageInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            OperationResult result = new OperationResult();
            OperationResult result2 = new OperationResult();
            foreach (var inputDto in inputDtos)
            {
                int count = inputDto.DeviceNodeList.Count();
                if (count >= 0)
                {
                    ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                    var oldmaplist = ProductionProcessEquipmentBusinessNodeMapRepository.TrackEntities.Where(x => x.Equipment_Id == inputDto.Equipment_Id && x.BusinessNode.Id == inputDto.BusinessNode_Id && x.ProductionProcessInfo_Id == inputDto.ProductionProcessInfo_Id);
                    int count0 = oldmaplist.Count();
                    if (count0 > 0)
                    {
                        Guid[] mapIds = new Guid[count0];
                        mapIds = oldmaplist.Select(x => x.Id).ToArray();
                        result2 = await ProductionProcessEquipmentBusinessNodeMapRepository.DeleteAsync(mapIds);
                    }
                    if (count == 0 && count0 > 0)
                    {
                        result = result2;
                    }
                    if ((result2.Successed || count0 == 0) && count > 0)
                    {
                        ProductionProcessEquipmentBusinessNodeMapInputDto[] map_dtos = new ProductionProcessEquipmentBusinessNodeMapInputDto[count];
                        for (int i = 0; i < count; i++)
                        {
                            ProductionProcessEquipmentBusinessNodeMapInputDto edto = new ProductionProcessEquipmentBusinessNodeMapInputDto();
                            edto.Equipment_Id = inputDto.Equipment_Id;
                            var id = inputDto.DeviceNodeList[i].Id;
                            edto.DeviceNode = DeviceNodeRepository.TrackEntities.Where(m => m.Id == id).FirstOrDefault();
                            edto.BusinessNode = BusinessNodeRepository.TrackEntities.Where(m => m.Id == inputDto.BusinessNode_Id).FirstOrDefault();
                            edto.ProductionProcessInfo_Id = inputDto.ProductionProcessInfo_Id;
                            edto.CreatorUserId = inputDto.CreatorUserId;
                            edto.CreatedTime = inputDto.CreatedTime;
                            edto.LastUpdatedTime = inputDto.LastUpdatedTime;
                            edto.LastUpdatorUserId = inputDto.LastUpdatorUserId;
                            if (Equals(edto.BusinessNode, null))
                            {
                                return new OperationResult(OperationResultType.Error, "对应的通讯业务信息不存在,该组数据不被存储。");
                            }
                            if (Equals(edto.DeviceNode, null))
                            {
                                return new OperationResult(OperationResultType.Error, "对应的数据点不存在,该组数据不被存储。");
                            }
                            //if (Equals(edto.ProductionProcessInfo_Id, Guid.Empty))
                            //{
                            //    return new OperationResult(OperationResultType.Error, "对应的工序为空,该组数据不被存储。");
                            //}
                            map_dtos[i] = edto;
                        }

                        result = await ProductionProcessEquipmentBusinessNodeMapRepository.InsertAsync(map_dtos);
                    }
                    ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, $"设备业务点表数据异常,该组数据不被存储。");
                }
            }
            return result;
        }
        /// <summary>
        /// 逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleBusinessNodeMap(params ProductionProcessEquipmentBusinessNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await ProductionProcessEquipmentBusinessNodeMapRepository.RecycleAsync(data);
                ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var entity in data)
            {
                names.Add(entity.BusinessNode.BusinessName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑删除成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 恢复逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreBusinessNodeMap(params ProductionProcessEquipmentBusinessNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await ProductionProcessEquipmentBusinessNodeMapRepository.RestoreAsync(data);
                ProductionProcessEquipmentBusinessNodeMapRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var entity in data)
            {
                names.Add(entity.BusinessNode.BusinessName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);


        }







    }
}
