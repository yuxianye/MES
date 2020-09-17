using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Models;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Services
{
    /// <summary>
    /// 仓库货区信息服务
    /// </summary>
    public class MatWareHouseAreaInfoService : IMatWareHouseAreaInfoContract
    {

        /// <summary>
        /// 仓库货区信息实体仓储
        /// </summary>
        public IRepository<MatWareHouseAreaInfo, Guid> MatWareHouseAreaRepository { get; set; }
        //
        public IRepository<MatWareHouseInfo, Guid> MatWareHouseInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }

        /// <summary>
        /// 查询仓库货区信息
        /// </summary>
        public IQueryable<MatWareHouseAreaInfo> MatWareHouseAreaInfos
        {
            get { return MatWareHouseAreaRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMatWareHouseAreaExists(Expression<Func<MatWareHouseAreaInfo, bool>> predicate, Guid id)
        {
            return MatWareHouseAreaRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加仓库货区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatWareHouseAreaInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseAreaCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库货区编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseAreaName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库货区名称，该组数据不被存储。");

                if ( dtoData.IsGenerageLocation != null && dtoData.IsGenerageLocation.Value &&
                     dtoData.WareHouseLocationCodeType == 0 )
                    return new OperationResult(OperationResultType.Error, "请正确选择库位命名方式，该组数据不被存储。");
                //
                if (MatWareHouseAreaRepository.CheckExists(x => x.WareHouseAreaCode == dtoData.WareHouseAreaCode))
                    return new OperationResult(OperationResultType.Error, $"仓库货区编号 {dtoData.WareHouseAreaCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseAreaRepository.CheckExists(x => x.WareHouseAreaName == dtoData.WareHouseAreaName))
                    return new OperationResult(OperationResultType.Error, $"仓库货区名称 {dtoData.WareHouseAreaName} 的数据已存在，该组数据不被存储。");

                if (dtoData.LayerNumber == 0)
                    return new OperationResult(OperationResultType.Error, $"货架层数不能为0，该组数据不被存储。");

                if (dtoData.LayerNumber > 10)
                    return new OperationResult(OperationResultType.Error, $"货架层数不能大于10，该组数据不被存储。");

                if (dtoData.ColumnNumber == 0)
                    return new OperationResult(OperationResultType.Error, $"货架列数不能为0，该组数据不被存储。");

                if (dtoData.ColumnNumber > 20)
                    return new OperationResult(OperationResultType.Error, $"货架列数不能大于20，该组数据不被存储。");

                if (dtoData.LocationLoadBearing > 1000)
                    return new OperationResult(OperationResultType.Error, $"库位承载不能大于1000，该组数据不被存储。");

                //
                dtoData.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouse_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouse, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的仓库货区类型不存在,该组数据不被存储。");
                }
            }
            MatWareHouseAreaRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseAreaRepository.InsertAsync(inputDtos);
            //
            foreach (var dtoData2 in inputDtos)
            {
                if (dtoData2.IsGenerageLocation != null && dtoData2.IsGenerageLocation.Value )
                {
                    if (dtoData2.WareHouseLocationCodeType == 1)
                    {
                        for (int i = 1; i <= dtoData2.LayerNumber; i++)
                        {
                            for (int j = 1; j <= dtoData2.ColumnNumber; j++)
                            {
                                MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                                matwarehouselocationInfo.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData2.MatWareHouse_Id).FirstOrDefault();
                                //
                                matwarehouselocationInfo.MatWareHouseArea = MatWareHouseAreaRepository.TrackEntities.Where(m => m.WareHouseAreaCode == dtoData2.WareHouseAreaCode).FirstOrDefault();
                                //
                                matwarehouselocationInfo.WareHouseLocationCode = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaCode + "_" + string.Format($"{j:000}", j) + "_" + string.Format($"{i:00}", i);
                                matwarehouselocationInfo.WareHouseLocationName = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaName + "_" + string.Format($"{j:000}", j) + "_" + string.Format($"{i:00}", i);
                                //
                                matwarehouselocationInfo.WareHouseLocationType = matwarehouselocationInfo.MatWareHouseArea.WareHouseLocationType;
                                //matwarehouselocationInfo.WareHouseLocationStatus = 4;
                                //
                                matwarehouselocationInfo.CreatorUserId = matwarehouselocationInfo.MatWareHouseArea.CreatorUserId;
                                matwarehouselocationInfo.CreatedTime = DateTime.Now;
                                //
                                matwarehouselocationInfo.IsUse = true;
                                //
                                matwarehouselocationInfo.Remark = "系统自动添加";
                                var result0 = await MatWareHouseLocationInfoRepository.InsertAsync(matwarehouselocationInfo);
                            }
                        }
                    }
                    else if (dtoData2.WareHouseLocationCodeType == 2)
                    {
                        for (int i = 1;i<= dtoData2.LocationQuantity; i++)
                        { 
                            MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                            matwarehouselocationInfo.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData2.MatWareHouse_Id).FirstOrDefault();
                            //
                            matwarehouselocationInfo.MatWareHouseArea = MatWareHouseAreaRepository.TrackEntities.Where(m => m.WareHouseAreaCode == dtoData2.WareHouseAreaCode).FirstOrDefault();
                            //
                            matwarehouselocationInfo.WareHouseLocationCode = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaCode + "_" + string.Format($"{i:000}", i);
                            matwarehouselocationInfo.WareHouseLocationName = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaName + "_" + string.Format($"{i:000}", i);
                            //
                            matwarehouselocationInfo.WareHouseLocationType = matwarehouselocationInfo.MatWareHouseArea.WareHouseLocationType;
                            //matwarehouselocationInfo.WareHouseLocationStatus = 4;
                            //
                            matwarehouselocationInfo.CreatorUserId = matwarehouselocationInfo.MatWareHouseArea.CreatorUserId;
                            matwarehouselocationInfo.CreatedTime = DateTime.Now;
                            //
                            matwarehouselocationInfo.IsUse = true;
                            //
                            matwarehouselocationInfo.Remark = "系统自动添加";
                            var result0 = await MatWareHouseLocationInfoRepository.InsertAsync(matwarehouselocationInfo);
                        }
                    }
                }
            }
            //
            MatWareHouseAreaRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新仓库货区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateMatWareHouseAreas(params MatWareHouseAreaInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatWareHouseAreaInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseAreaCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库货区编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseAreaName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库货区名称，该组数据不被存储。");
                //
                if (dtoData.IsGenerageLocation != null && dtoData.IsGenerageLocation.Value &&
                    dtoData.WareHouseLocationCodeType == 0)
                    return new OperationResult(OperationResultType.Error, "请正确选择库位命名方式，该组数据不被存储。");
                //
                if (MatWareHouseAreaRepository.CheckExists(x => x.WareHouseAreaCode == dtoData.WareHouseAreaCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库货区编号 {dtoData.WareHouseAreaCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseAreaRepository.CheckExists(x => x.WareHouseAreaName == dtoData.WareHouseAreaName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库货区名称 {dtoData.WareHouseAreaName} 的数据已存在，该组数据不被存储。");

                if (dtoData.LayerNumber == 0)
                    return new OperationResult(OperationResultType.Error, $"货架层数不能为0，该组数据不被存储。");

                if (dtoData.LayerNumber > 10)
                    return new OperationResult(OperationResultType.Error, $"货架层数不能大于10，该组数据不被存储。");

                if (dtoData.ColumnNumber == 0)
                    return new OperationResult(OperationResultType.Error, $"货架列数不能为0，该组数据不被存储。");

                if (dtoData.ColumnNumber > 20)
                    return new OperationResult(OperationResultType.Error, $"货架列数不能大于20，该组数据不被存储。");

                if (dtoData.LocationLoadBearing > 1000)
                    return new OperationResult(OperationResultType.Error, $"库位承载不能大于1000，该组数据不被存储。");
            }
            //
            MatWareHouseAreaRepository.UnitOfWork.BeginTransaction();
            //
            MatWareHouseInfo matwarehouseinfo = new MatWareHouseInfo();
            foreach (var dtoData2 in inputDtos)
            {
                matwarehouseinfo = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData2.MatWareHouse_Id).FirstOrDefault();
                dtoData2.MatWareHouse = matwarehouseinfo;
                //
                if (dtoData2.IsGenerageLocation != null && dtoData2.IsGenerageLocation.Value)
                {
                    if (dtoData2.WareHouseLocationCodeType == 1)
                    {
                        for (int i = 1; i <= dtoData2.LayerNumber; i++)
                        {
                            for (int j = 1; j <= dtoData2.ColumnNumber; j++)
                            {
                                MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                                matwarehouselocationInfo.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData2.MatWareHouse_Id).FirstOrDefault();
                                //
                                matwarehouselocationInfo.MatWareHouseArea = MatWareHouseAreaRepository.TrackEntities.Where(m => m.WareHouseAreaCode == dtoData2.WareHouseAreaCode).FirstOrDefault();
                                //
                                matwarehouselocationInfo.WareHouseLocationCode = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaCode + "_" + string.Format($"{j:000}", j) + "_" + string.Format($"{i:00}", i);
                                matwarehouselocationInfo.WareHouseLocationName = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaName + "_" + string.Format($"{j:000}", j) + "_" + string.Format($"{i:00}", i);
                                //
                                matwarehouselocationInfo.WareHouseLocationType = matwarehouselocationInfo.MatWareHouseArea.WareHouseLocationType;
                                //matwarehouselocationInfo.WareHouseLocationStatus = 4;
                                //
                                matwarehouselocationInfo.CreatorUserId = matwarehouselocationInfo.MatWareHouseArea.CreatorUserId;
                                matwarehouselocationInfo.CreatedTime = DateTime.Now;
                                //
                                matwarehouselocationInfo.IsUse = true;
                                //
                                matwarehouselocationInfo.Remark = "系统自动添加";
                                var result0 = await MatWareHouseLocationInfoRepository.InsertAsync(matwarehouselocationInfo);
                            }
                        }
                    }
                    else if (dtoData2.WareHouseLocationCodeType == 2)
                    {
                        for (int i = 1; i <= dtoData2.LocationQuantity; i++)
                        {
                            MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                            matwarehouselocationInfo.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData2.MatWareHouse_Id).FirstOrDefault();
                            //
                            matwarehouselocationInfo.MatWareHouseArea = MatWareHouseAreaRepository.TrackEntities.Where(m => m.WareHouseAreaCode == dtoData2.WareHouseAreaCode).FirstOrDefault();
                            //
                            matwarehouselocationInfo.WareHouseLocationCode = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaCode + "_" + string.Format($"{i:000}", i);
                            matwarehouselocationInfo.WareHouseLocationName = matwarehouselocationInfo.MatWareHouseArea.WareHouseAreaName + "_" + string.Format($"{i:000}", i);
                            //
                            matwarehouselocationInfo.WareHouseLocationType = matwarehouselocationInfo.MatWareHouseArea.WareHouseLocationType;
                            //matwarehouselocationInfo.WareHouseLocationStatus = 4;
                            //
                            matwarehouselocationInfo.CreatorUserId = matwarehouselocationInfo.MatWareHouseArea.CreatorUserId;
                            matwarehouselocationInfo.CreatedTime = DateTime.Now;
                            //
                            matwarehouselocationInfo.IsUse = true;
                            //
                            matwarehouselocationInfo.Remark = "系统自动添加";
                            var result0 = await MatWareHouseLocationInfoRepository.InsertAsync(matwarehouselocationInfo);
                        }
                    }
                }
            }
            //
            var result = await MatWareHouseAreaRepository.UpdateAsync(inputDtos);
            MatWareHouseAreaRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除仓库货区信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteMatWareHouseAreas(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatWareHouseAreaRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseAreaRepository.DeleteAsync(ids);
            MatWareHouseAreaRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
