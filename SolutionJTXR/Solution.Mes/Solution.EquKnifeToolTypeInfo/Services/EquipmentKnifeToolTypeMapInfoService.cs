using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipKnifeToolInfo.Contracts;
using Solution.EquipKnifeToolInfo.Dtos;
using Solution.EquipKnifeToolInfo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Solution.EquipmentManagement.Models;

namespace Solution.EquipKnifeToolInfo.Services
{
    public class EquipmentKnifeToolTypeMapService : IEquipmentKnifeToolTypeMapContract
    {

        public IRepository<EquipmentKnifeToolTypeMap, Guid> EquipmentKnifeToolTypeMapRepository { get; set; }
        public IRepository<EquipmentInfo, Guid> EquipmentInfoRepository { get; set; }
        public IRepository<KnifeToolTypeInfo, Guid> KnifeToolTypeInfoRepository { get; set; }

        /// <summary>
        /// 查询装备信息
        /// </summary>
        public IQueryable<EquipmentKnifeToolTypeMap> EquipmentKnifeToolTypeMaps
        {
            get { return EquipmentKnifeToolTypeMapRepository.Entities; }
        }
        /// <summary>
        /// 增加刀具信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquipmentKnifeToolTypeMap(params EquipmentKnifeToolTypeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.KnifeCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写刀具信息编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.KnifeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写刀具存在问题信息，该组数据不被存储。");
                //if (EquipmentKnifeToolTypeMapRepository.CheckExists(x => x.KnifeCode == dtoData.KnifeCode))
                //    return new OperationResult(OperationResultType.Error, $"刀具信息编号 {dtoData.KnifeCode} 的数据已存在，该组数据不被存储。");
                //if (EquipmentKnifeToolTypeMapRepository.CheckExists(x => x.KnifeName == dtoData.KnifeName))
                //    return new OperationResult(OperationResultType.Error, $"刀具问题信息名称 {dtoData.KnifeName} 的数据已存在，该组数据不被存储。");
                dtoData.Knifetooltypeinfo = KnifeToolTypeInfoRepository.GetByKey(dtoData.KnifeToolTypeInfo_Id);
                if (Equals(dtoData.Knifetooltypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的刀具状态信息不存在,该组数据不被存储。");
                }
                //******************************************************
                dtoData.Equipmentinfo = EquipmentInfoRepository.GetByKey(dtoData.EquipmentInfo_Id);
                if (Equals(dtoData.Equipmentinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备状态信息不存在,该组数据不被存储。");
                }

            }
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentKnifeToolTypeMapRepository.InsertAsync(inputDtos);
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查刀具信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具信息编号</param>
        /// <returns>刀具信息是否存在</returns>
        public bool CheckEquipmentKnifeToolTypeMapExists(Expression<Func<EquipmentKnifeToolTypeMap, bool>> predicate, Guid id)
        {
            return EquipmentKnifeToolTypeMapRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除刀具信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquipmentKnifeToolTypeMap(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentKnifeToolTypeMapRepository.DeleteAsync(ids);
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 逻辑删除刀具信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicDeleteKnifeTool(params KnifeToolInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;
        //    try
        //    {
        //        EquipmentKnifeToolTypeMapRepository.UnitOfWork.BeginTransaction();
        //        count = await EquipmentKnifeToolTypeMapRepository.RecycleAsync(equiinfo);
        //        EquipmentKnifeToolTypeMapRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in equiinfo)
        //    {
        //        names.Add(data.KnifeToolName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 逻辑还原刀具信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicRestoreKnifeTool(params KnifeToolInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;

        //    try
        //    {
        //        EquipmentKnifeToolTypeMapRepository.UnitOfWork.BeginTransaction();
        //        count = await EquipmentKnifeToolTypeMapRepository.RestoreAsync(equiinfo);
        //        EquipmentKnifeToolTypeMapRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in equiinfo)
        //    {
        //        names.Add(data.KnifeToolName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        public async Task<OperationResult> UpdateEquipmentKnifeToolTypeMap(params EquipmentKnifeToolTypeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.KnifeCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写刀具信息编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.KnifeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写刀具存在问题信息，该组数据不被存储。");
                //if (EquipmentKnifeToolTypeMapRepository.CheckExists(x => x.KnifeCode == dtoData.KnifeCode))
                //    return new OperationResult(OperationResultType.Error, $"刀具信息编号 {dtoData.KnifeCode} 的数据已存在，该组数据不被存储。");
                //if (EquipmentKnifeToolTypeMapRepository.CheckExists(x => x.KnifeName == dtoData.KnifeName))
                //    return new OperationResult(OperationResultType.Error, $"刀具问题信息名称 {dtoData.KnifeName} 的数据已存在，该组数据不被存储。");
                dtoData.Knifetooltypeinfo = KnifeToolTypeInfoRepository.GetByKey(dtoData.KnifeToolTypeInfo_Id);
                if (Equals(dtoData.Knifetooltypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的刀具状态信息不存在,该组数据不被存储。");
                }
                //******************************************************
                dtoData.Equipmentinfo = EquipmentInfoRepository.GetByKey(dtoData.EquipmentInfo_Id);
                if (Equals(dtoData.Equipmentinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备状态信息不存在,该组数据不被存储。");
                }

            }
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentKnifeToolTypeMapRepository.UpdateAsync(inputDtos);
            EquipmentKnifeToolTypeMapRepository.UnitOfWork.Commit();
            return result;

      

        }
    }
}
