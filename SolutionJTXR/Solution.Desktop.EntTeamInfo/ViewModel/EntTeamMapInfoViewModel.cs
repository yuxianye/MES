using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntTeamInfo.Model;
using Solution.Desktop.EntEmployeeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;

namespace Solution.Desktop.EntTeamInfo.ViewModel
{
    public class EntTeamMapInfoViewModel : VmBase
    {  /// <summary>
       /// 构造函数
       /// </summary>
        public EntTeamMapInfoViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSaveCommand, OnCanExecuteSaveCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            EntTeamChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnEntTeamChangedCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.EntTeamInfo = parameter as EntTeamInfoModel;
            if (Equals(EntTeamInfo, null))
            {
            }
            else
            {
                getEntTeamInfoPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                getEntEmployeePageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                SelectCheckedNodes();
            }

        }

        #region 班组信息列表
        private ObservableCollection<EntTeamInfoModel> entTeamInfoList = new ObservableCollection<EntTeamInfoModel>();
        public ObservableCollection<EntTeamInfoModel> EntTeamInfoList
        {
            get { return entTeamInfoList; }
            set { Set(ref entTeamInfoList, value); }
        }
        #endregion

        #region 人员列表
        private ObservableCollection<EntEmployeeInfoModel> entEmployeeInfoListAll = new ObservableCollection<EntEmployeeInfoModel>();
        public ObservableCollection<EntEmployeeInfoModel> EntEmployeeInfoListAll
        {
            get { return entEmployeeInfoListAll; }
            set { Set(ref entEmployeeInfoListAll, value); }
        }
        #endregion

        #region 班组信息模型
        /// <summary>
        /// 班组信息模型
        /// </summary>
        private EntTeamInfoModel entTeamInfo = new EntTeamInfoModel();
        /// <summary>
        /// 班组信息模型
        /// </summary>
        public EntTeamInfoModel EntTeamInfo
        {
            get { return entTeamInfo; }
            set
            {
                Set(ref entTeamInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 人员模型
        /// <summary>
        /// 人员模型
        /// </summary>
        private EntEmployeeInfoModel entEmployeeInfo = new EntEmployeeInfoModel();
        /// <summary>
        /// 人员模型
        /// </summary>
        public EntEmployeeInfoModel EntEmployeeInfo
        {
            get { return entEmployeeInfo; }
            set
            {
                Set(ref entEmployeeInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion       

        #region 班组信息-人员配置模型
        /// <summary>
        /// 班组信息-人员配置模型
        /// </summary>
        private EntTeamMapInfoModel entTeamMapInfo = new EntTeamMapInfoModel();
        /// <summary>
        /// 班组信息-人员配置模型
        /// </summary>
        public EntTeamMapInfoModel EntTeamMapInfo
        {
            get { return entTeamMapInfo; }
            set
            {
                Set(ref entTeamMapInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 班组信息已配置人员数据列表
        private ObservableCollection<EntTeamMapInfoModel> existEntTeamMapInfoList = new ObservableCollection<EntTeamMapInfoModel>();
        public ObservableCollection<EntTeamMapInfoModel> ExistEntTeamMapInfoList
        {
            get { return existEntTeamMapInfoList; }
            set { Set(ref existEntTeamMapInfoList, value); }
        }
        #endregion

        public ObservableCollection<EntEmployeeInfoModel> SelectedEntEmployeeInfoList { get; set; } = new ObservableCollection<EntEmployeeInfoModel>();

        /// <summary>
        /// 确认命令(关闭弹窗)
        /// </summary>
        public ICommand ConfirmCommand { get; set; }
        /// <summary>
        /// 保存命令（不关闭弹窗）
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 班组操作改变命令
        /// </summary>
        public ICommand EntTeamChangedCommand { get; set; }


        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return EntTeamInfo == null ? false : EntTeamInfo.IsValidated;
        }
        private bool OnCanExecuteSaveCommand()
        {
            return EntTeamInfo == null ? false : EntTeamInfo.IsValidated;
        }
        private void OnEntTeamChangedCommand()
        {
            SelectCheckedNodes();
        }


        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得班组信息分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEntTeamInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntTeamInfoModel>>>(GlobalData.ServerRootUri + "EntTeamInfo/GetEntTeamInfoList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取班组信息数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("班组信息数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EntTeamInfoList = new ObservableCollection<EntTeamInfoModel>(result.Data.Data);
                }
                else
                {
                    EntTeamInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntTeamInfoList = new ObservableCollection<EntTeamInfoModel>();
                UiMessage = result?.Message ?? "查询班组信息数据失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取人员分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEntEmployeePageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntEmployeeInfoModel>>>(GlobalData.ServerRootUri + "EntEmployeeInfo/GetEntEmployeeList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取人员列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("人员列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EntEmployeeInfoListAll?.Clear();
                    foreach (var data in result.Data.Data)
                    {
                        EntEmployeeInfoModel pro = new EntEmployeeInfoModel();
                        pro = data;
                        pro.PropertyChanged += OnPropertyChangedCommand;
                        EntEmployeeInfoListAll.Add(pro);
                    }
                }
                else
                {
                    EntEmployeeInfoListAll?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntEmployeeInfoListAll = new ObservableCollection<EntEmployeeInfoModel>();
                UiMessage = result?.Message ?? "查询人员列表失败，请联系管理员！";
            }
        }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            EntTeamMapInfo.EntEmployeeInfoList = SelectedEntEmployeeInfoList;
            EntTeamMapInfo.EntTeamInfo = EntTeamInfo;
            EntTeamMapInfo.EntTeamInfo_Id = EntTeamInfo.Id;

            /*  if (EntTeamMapInfoModel.ProductionProcessInfo_ID == null)
              {
                  EntTeamMapInfoModel.ProductionProcessInfo_ID = Guid.Empty;
              }*/
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntTeamInfo/Setting",
                 Utility.JsonHelper.ToJson(new List<EntTeamMapInfoModel> { EntTeamMapInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntTeamMapInfoModel>(EntTeamMapInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                UiMessage = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(UiMessage);
            }
        }
        /// <summary>
        /// 执行保存命令函数
        /// </summary>
        private void OnExecuteSaveCommand()
        {
            EntTeamMapInfo.EntEmployeeInfoList = SelectedEntEmployeeInfoList;
            EntTeamMapInfo.EntTeamInfo = EntTeamInfo;
            EntTeamMapInfo.EntTeamInfo_Id = EntTeamInfo.Id;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntTeamInfo/Setting",
                Utility.JsonHelper.ToJson(new List<EntTeamMapInfoModel> { EntTeamMapInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntTeamMapInfoModel>(EntTeamMapInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.DataChanged);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                UiMessage = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(UiMessage);
            }
        }
        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("EntEmployeeName", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("EntEmployeeCode", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);
            pageRepuestParams.FilterGroup = filterGroup;
            getEntEmployeePageData(1, 1000);
            SelectCheckedNodes();
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as EntEmployeeInfoModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    SelectedEntEmployeeInfoList.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    SelectedEntEmployeeInfoList.Remove(selectedObj);
                }
            }
        }
        private EntTeamMapInfoModel teammapmodel = new EntTeamMapInfoModel();
        private void SelectCheckedNodes()
        {
            SelectedEntEmployeeInfoList.Clear();
            //  processmapmodel.EntTeamInfo_ID = EntTeamInfo.Id;
            teammapmodel.EntTeamInfo = EntTeamInfo;
            for (int i = 0; i < EntEmployeeInfoListAll.Count; i++)
            {
                EntEmployeeInfoListAll[i].IsChecked = false;
            }

            GetEntTeamMapInfoByIds(teammapmodel);

            if (ExistEntTeamMapInfoList.Any())
            {
                foreach (var data in ExistEntTeamMapInfoList)
                {
                    for (int i = 0; i < EntEmployeeInfoListAll.Count; i++)
                    {
                        if (!Equals(data.EntEmployeeInfo, null))
                        {
                            if (data.EntEmployeeInfo.Id == EntEmployeeInfoListAll[i].Id)
                            {
                                EntEmployeeInfoListAll[i].IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        private void GetEntTeamMapInfoByIds(EntTeamMapInfoModel model)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            FilterRule filterRule = new FilterRule("EntTeamInfo.Id", model.EntTeamInfo.Id, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntTeamMapInfoModel>>>(GlobalData.ServerRootUri + "EntTeamMapInfo/GetEntTeamMapInfoList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取已配置人员信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("已配置人员信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    ExistEntTeamMapInfoList = new ObservableCollection<EntTeamMapInfoModel>(result.Data.Data);
                }
                else
                {
                    ExistEntTeamMapInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ExistEntTeamMapInfoList = new ObservableCollection<EntTeamMapInfoModel>();
                UiMessage = result?.Message ?? "查询信息失败，请联系管理员！";
            }
        }
    }
}