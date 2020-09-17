using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentProgramInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Desktop.EquSparePartTypeInfo.Model;
using Solution.Desktop.EquipmentTypeInfo.Model;

namespace Solution.Desktop.EquipmentProgramInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquipmentProgramInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentProgramInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getPageData(1, 200);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.EquipmentProgramModel = parameter as EquipmentProgramModel;
        }
        #region 设备程序模型

        //private EquipmentProgramModel EquipmentProgramModel = new EquipmentProgramModel();
        //private EntProductionLineInfoModel entproductionlineInfoModel = new EntProductionLineInfoModel();

        private EquipmentProgramModel equipmentProgramModel = new EquipmentProgramModel();
        /// <summary>
        /// 设备程序模型
        /// </summary>
        public EquipmentProgramModel EquipmentProgramModel
        {
            get { return equipmentProgramModel; }
            set
            {
                Set(ref equipmentProgramModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 设备类别信息模型,用于类别数据显示
        private ObservableCollection<EquipmentTypeModel> equipmenttypeInfoList = new ObservableCollection<EquipmentTypeModel>();

        /// <summary>
        /// 设备类别数据
        /// </summary>
        public ObservableCollection<EquipmentTypeModel> EquipmenttypeInfoList
        {
            get { return equipmenttypeInfoList; }
            set { Set(ref equipmenttypeInfoList, value); }
        }
        /// <summary>
        /// 处理下来列表枚举(数量单位)
        /// </summary>
        //public System.Array programTypeEnum = Enum.GetValues(typeof(ProgramTypeEnum.ProgramType));

        //public System.Array ProgramType//EquRunningStateTypes
        //{
        //    get { return programTypeEnum; }
        //    set
        //    {
        //        Set(ref programTypeEnum, value);
        //    }
        //}
        //#region 设备类别信息模型,用于类别数据显示
        //private ObservableCollection<EquSparePartTypeModel> equipmentInfoList = new ObservableCollection<EquSparePartTypeModel>();

        ///// <summary>
        ///// 设备类别数据
        ///// </summary>
        //public ObservableCollection<EquSparePartTypeModel> EquipmentInfoList
        //{
        //    get { return equipmentInfoList; }
        //    set { Set(ref equipmentInfoList, value); }
        //}




        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquipmentTypeModel>>>(GlobalData.ServerRootUri + "EquipmentTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquipmenttypeInfoList = new ObservableCollection<EquipmentTypeModel>(result.Data.Data);

                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    EquipmenttypeInfoList?.Clear();
                    //TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmenttypeInfoList = new ObservableCollection<EquipmentTypeModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
            }
        }
        #endregion

        //        /// <summary>
        //        /// 取得分页数据
        //        /// </summary>
        //        /// <param name="pageIndex"></param>
        //        /// <param name="pageSize"></param>
        //        private void getPageData(int pageIndex, int pageSize)
        //        {
        //#if DEBUG
        //            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //            stopwatch.Start();
        //#endif
        //            pageRepuestParams.SortField = "LastUpdatedTime";
        //            pageRepuestParams.SortOrder = "desc";

        //            pageRepuestParams.PageIndex = pageIndex;
        //            pageRepuestParams.PageSize = pageSize;

        //            
        //            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquSparePartTypeModel>>>(GlobalData.ServerRootUri + "EquSparePartTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

        //#if DEBUG
        //            stopwatch.Stop();
        //            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
        //            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
        //#endif

        //            if (!Equals(result, null) && result.Successed)
        //            {
        //                Application.Current.Resources["UiMessage"] = result?.Message;
        //                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //                if (result.Data.Data.Any())
        //                {
        //                    //TotalCounts = result.Data.Total;
        //                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
        //                    EquipmentInfoList = new ObservableCollection<EquSparePartTypeModel>(result.Data.Data);

        //                    //TotalCounts = result.Data.Total;
        //                }
        //                else
        //                {
        //                    EquipmentInfoList?.Clear();
        //                    //TotalCounts = 0;
        //                    Application.Current.Resources["UiMessage"] = "未找到数据";
        //                }
        //            }
        //            else
        //            {
        //                //操作失败，显示错误信息
        //                EquipmentInfoList = new ObservableCollection<EquSparePartTypeModel>();
        //                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
        //            }
        //        }
        //        #endregion

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>


        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return EquipmentProgramModel == null ? false : EquipmentProgramModel.IsValidated;
        }

        /// <summary>
        /// 设备类别信息下拉列表改变
        /// </summary>
        public ICommand equipmentTypeSelectionChangedCommand { get; set; }

        /// <summary>
        /// 生产线信息下拉列表改变
        /// </summary>
        public ICommand productionLineSelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentProgramInfo/Update",
                Utility.JsonHelper.ToJson(new List<EquipmentProgramModel> { EquipmentProgramModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EquipmentProgramModel>(EquipmentProgramModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }
        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
    }
}
