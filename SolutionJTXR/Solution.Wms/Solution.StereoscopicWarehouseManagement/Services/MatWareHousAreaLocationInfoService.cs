using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StereoscopicWarehouseManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using Solution.StoredInWarehouseManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StereoscopicWarehouseManagement.Services
{
    /// <summary>
    /// 仓位图信息服务
    /// </summary>
    public class MatWareHousAreaLocationInfoService : IMatWareHousAreaLocationInfoContract
    {
        public IRepository<MatWareHouseAreaInfo, Guid> MatWareHouseAreaInfoRepository { get; set; }
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }

        public IRepository<MatPalletInfo, Guid> MatPalletRepository { get; set; }
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }

        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }
        //
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        //
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }


        /// <summary>
        /// 初始化仓位图信息
        /// </summary>
        /// <returns></returns>
        public List<MatWareHousAreaLocationInfoOutputDto> Ini1(Guid id)
        {
            List<MatWareHousAreaLocationInfoOutputDto> matwarehousarealocationinfoList = new List<MatWareHousAreaLocationInfoOutputDto>();
            //
            foreach (MatWareHouseAreaInfo matwarehouseareaInfo in MatWareHouseAreaInfoRepository.Entities.Where(m => (!(id == Guid.Empty) ? m.Id.ToString().Contains(id.ToString()) : true) ))
            {
                Guid MatWareHouseArea_Id = matwarehouseareaInfo.Id;
                MatWareHouseAreaInfo MatWareHouseArea = MatWareHouseAreaInfoRepository.TrackEntities.Where(m => m.Id == MatWareHouseArea_Id).FirstOrDefault();
                //List<MatWareHouseLocationInfo> matwarehouselocationinfoList = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.MatWareHouseArea.Id == MatWareHouseArea_Id)
                //                                                                                                              .OrderBy(m => m.WareHouseLocationCode).ToList();
                //库位 库存 基础数据
                //var matwarehouselocationinfoList = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.MatWareHouseArea.Id == MatWareHouseArea_Id)
                //                                                                                   .OrderBy(m => m.WareHouseLocationCode.Length == 8 ? m.WareHouseLocationCode : m.WareHouseLocationCode.Substring(9,2) + m.WareHouseLocationCode.Substring(5, 3))
                //                                                                                   .Select(m => new
                //var matwarehouselocationinfoList = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.MatWareHouseArea.Id == MatWareHouseArea_Id)
                //                                                                                   .OrderBy(m => m.WareHouseLocationCode.Substring(m.WareHouseLocationCode..LastIndexOf("_") + 1,m.WareHouseLocationCode.Length).Length == 3 ? 
                //                                                                                                          m.WareHouseLocationCode :
                //                                                                                                          m.WareHouseLocationCode.Substring(m.WareHouseLocationCode.LastIndexOf("_") + 1, 2) +
                //                                                                                                          m.WareHouseLocationCode.Substring(m.WareHouseLocationCode.LastIndexOf("_", m.WareHouseLocationCode.LastIndexOf("_") - 1) + 1, 3) )
                //                                                                                   .Select(m => new
                var matwarehouselocationinfoList = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.MatWareHouseArea.Id == MatWareHouseArea_Id)
                                                                                                   .OrderBy(m => m.Id)
                                                                                                   .Select(m => new
               {

                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                    m.WareHouseLocationCode,
                    m.WareHouseLocationName,
                    m.WareHouseLocationType,
                    //m.WareHouseLocationStatus,

                    LayerNumber = m.MatWareHouseArea.LayerNumber,
                    ColumnNumber = m.MatWareHouseArea.ColumnNumber,

                    PalletCode = MatPalletRepository.TrackEntities.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletRepository.TrackEntities.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    StorageQuantity = MaterialBatchInfoRepository.TrackEntities.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Sum(m2 => m2.Quantity),
                    FullPalletQuantity = MaterialBatchInfoRepository.TrackEntities.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.Material.FullPalletQuantity).FirstOrDefault(),

                    m.IsUse,

                }).ToList();

                //库位 层 数据
                for (int i = 0; i < MatWareHouseArea.LayerNumber; i++)
                {
                    MatWareHousAreaLocationInfoOutputDto matwarehousarealocationInfo = new MatWareHousAreaLocationInfoOutputDto();
                    //
                    matwarehousarealocationInfo.MatWareHouseArea = MatWareHouseArea;
                    //
                    matwarehousarealocationInfo.WareHouseAreaCode = MatWareHouseArea.WareHouseAreaCode;
                    matwarehousarealocationInfo.WareHouseAreaName = MatWareHouseArea.WareHouseAreaName;

                    matwarehousarealocationInfo.WareHouseCode = MatWareHouseArea.MatWareHouse.WareHouseCode;
                    matwarehousarealocationInfo.WareHouseName = MatWareHouseArea.MatWareHouse.WareHouseName;

                    matwarehousarealocationInfo.LayerNumber = string.Format($"{i + 1:00}", i + 1);
                    matwarehousarealocationInfo.ColumnNumber = MatWareHouseArea.ColumnNumber == null ? 0 : MatWareHouseArea.ColumnNumber.Value;
                    matwarehousarealocationInfo.WareHouseColumns = new List<string>();
                    //
                    for (int iii = 0; iii < matwarehousarealocationInfo.ColumnNumber; iii ++ )
                    {
                        string warehousecolumn = "";
                        matwarehousarealocationInfo.WareHouseColumns.Add(warehousecolumn);
                    }
                    //
                    matwarehousarealocationinfoList.Add(matwarehousarealocationInfo);
                }
                //
                //库位 库存 基础数据 顺序号
                int ii = 0;
                //
                //库位 列 基础数据
                for (int i = 0; i < MatWareHouseArea.LayerNumber; i++)
                {
                    for (int j = 0; j < MatWareHouseArea.ColumnNumber; j++)
                    {
                        string sWareHouseLocationCode = "";
                        string sPalletCode = "";
                        string sStorageQuantity = "";
                        string sFullPalletQuantity = "";
                        string sIsUse = "";
                        //
                        if ( matwarehouselocationinfoList[ii] != null )
                        {
                            sWareHouseLocationCode = matwarehouselocationinfoList[ii].WareHouseLocationCode;
                            if (string.IsNullOrEmpty(sWareHouseLocationCode))
                            {
                                sWareHouseLocationCode = "-";
                            }
                            //
                            sPalletCode = matwarehouselocationinfoList[ii].PalletCode;
                            if (string.IsNullOrEmpty(sPalletCode))
                            {
                                sPalletCode = "-";
                            }
                            //
                            sStorageQuantity = matwarehouselocationinfoList[ii].StorageQuantity.ToString();
                            if (string.IsNullOrEmpty(sStorageQuantity))
                            {
                                sStorageQuantity = "-";
                            }
                            //
                            sFullPalletQuantity = matwarehouselocationinfoList[ii].FullPalletQuantity.ToString();
                            if (string.IsNullOrEmpty(sFullPalletQuantity))
                            {
                                sFullPalletQuantity = "-";
                            }
                            //
                            sIsUse = matwarehouselocationinfoList[ii].IsUse.ToString();
                            //
                            sWareHouseLocationCode = sWareHouseLocationCode + " " + sPalletCode + " " + sStorageQuantity + " " + sFullPalletQuantity + " " + sIsUse;
                            matwarehousarealocationinfoList[i].WareHouseColumns[j] = sWareHouseLocationCode;
                        }
                        ii++;
                    }
                }
            }
            ////////////////////////////////////////////////////
            return matwarehousarealocationinfoList;
        }


        /// <summary>
        /// 初始化仓位图信息
        /// </summary>
        /// <returns></returns>
        public List<MatWareHousAreaLocationItemInfoOutputDto> Ini2(string asid)
        {
            List<MatWareHousAreaLocationItemInfoOutputDto> matwarehousarealocationinfoList = new List<MatWareHousAreaLocationItemInfoOutputDto>();
            //
            var matwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => !asid.Equals("") ? m.WareHouseLocationCode.Contains(asid) : true)
                                                                                           .Select(m => new
            {
                WareHouseCode = m.MatWareHouse.WareHouseCode,
                WareHouseName = m.MatWareHouse.WareHouseName,

                WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                m.WareHouseLocationCode,
                m.WareHouseLocationName,
                m.WareHouseLocationType,
                //m.WareHouseLocationStatus,

                PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                MaterialCode = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.Material.MaterialCode).FirstOrDefault(),
                MaterialName = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.Material.MaterialName).FirstOrDefault(),
                MaterialUnit = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.Material.MaterialUnit).FirstOrDefault(),

                FullPalletQuantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.Material.FullPalletQuantity).FirstOrDefault(),
                StorageQuantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Sum(m2 => m2.Quantity),
                InStorageBillCode = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.MaterialInStorage.InStorageBillCode).FirstOrDefault(),

                BatchCode = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Select(m2 => m2.BatchCode).FirstOrDefault(),

                                                                                               
                m.IsUse,
                m.Remark,
            }).ToList();

            foreach (var matwarehouseareaInfo in matwarehouselocationInfo)
            {
                MatWareHousAreaLocationItemInfoOutputDto matwarehousarealocationiteminfoinputdto = new MatWareHousAreaLocationItemInfoOutputDto();
                //
                matwarehousarealocationiteminfoinputdto.PalletCode = matwarehouseareaInfo.PalletCode;
                matwarehousarealocationiteminfoinputdto.PalletName = matwarehouseareaInfo.PalletName;
                matwarehousarealocationiteminfoinputdto.WareHouseLocationCode = matwarehouseareaInfo.WareHouseLocationCode;
                matwarehousarealocationiteminfoinputdto.WareHouseLocationName = matwarehouseareaInfo.WareHouseLocationName;
                matwarehousarealocationiteminfoinputdto.MaterialCode = matwarehouseareaInfo.MaterialCode;
                matwarehousarealocationiteminfoinputdto.MaterialName = matwarehouseareaInfo.MaterialName;
                matwarehousarealocationiteminfoinputdto.Quantity = matwarehouseareaInfo.StorageQuantity == null? null : matwarehouseareaInfo.StorageQuantity;
                matwarehousarealocationiteminfoinputdto.MaterialUnit = matwarehouseareaInfo.MaterialUnit == null ? 0 : matwarehouseareaInfo.MaterialUnit.Value;
                matwarehousarealocationiteminfoinputdto.InStorageBillCode = matwarehouseareaInfo.InStorageBillCode;
                matwarehousarealocationiteminfoinputdto.FullPalletQuantity = matwarehouseareaInfo.FullPalletQuantity;
                matwarehousarealocationiteminfoinputdto.BatchCode = matwarehouseareaInfo.BatchCode;

                //
                matwarehousarealocationinfoList.Add(matwarehousarealocationiteminfoinputdto);
            }
            ////////////////////////////////////////////////////
            //
            return matwarehousarealocationinfoList;
        }
    }
}
