using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.RoleManager.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.ModuleManager.Model;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;

namespace Solution.Desktop.RoleManager.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class RoleEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            getPageData(1, 1000000);
        }

        public override void OnParamterChanged(object parameter)
        {
            this.RoleModel = parameter as RoleModel;
            GetSelectedModules();
        }

        #region 角色信息模型
        /// <summary>
        /// 角色信息模型
        /// </summary>
        private RoleModel roleModel;// = new EnterpriseModel();
        /// <summary>
        /// 角色信息模型
        /// </summary>
        public RoleModel RoleModel
        {
            get { return roleModel; }
            set
            {
                Set(ref roleModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion


        #region 模块信息模型
        /// <summary>
        /// 模块信息模型
        /// </summary>
        private ModuleManagerModel moduleManagerModel = new ModuleManagerModel();
        /// <summary>
        /// 模块信息模型
        /// </summary>
        public ModuleManagerModel ModuleManagerModel
        {
            get { return moduleManagerModel; }
            set
            {
                Set(ref moduleManagerModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 模块信息模型,用于列表数据显示
        private ObservableCollection<ModuleManagerModel> moduleManagerInfoList = new ObservableCollection<ModuleManagerModel>();

        /// <summary>
        /// 模块信息信息数据
        /// </summary>
        public ObservableCollection<ModuleManagerModel> ModuleManagerInfoList
        {
            get { return moduleManagerInfoList; }
            set { Set(ref moduleManagerInfoList, value); }
        }
        #endregion

        #region 模块信息临时存储
        private ObservableCollection<ModuleManagerModel> _dataSource = new ObservableCollection<ModuleManagerModel>();

        public ObservableCollection<ModuleManagerModel> DataSource
        {
            get { return _dataSource; }
            set { Set(ref _dataSource, value); }
        }
        #endregion

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 搜索命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return RoleModel == null ? false : RoleModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            //RoleModel.IsLocked = false;
            ObservableCollection<ModuleManagerModel> ModuleManagerModelList = new ObservableCollection<ModuleManagerModel>();
            foreach (var data in DataSource)
            {
                if (data.IsChecked)
                {
                    ModuleManagerModelList.Add(data);
                }
            }
            RoleModel.ModuleManagerModels = ModuleManagerModelList;

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "RoleManager/Update",
                Utility.JsonHelper.ToJson(new List<RoleModel> { RoleModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<RoleModel>(RoleModel, MessengerToken.DataChanged);
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
        /// 分页请求参数
        /// </summary>
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
            //pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ModuleManagerModel>>>(GlobalData.ServerRootUri + "ModuleManager/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取模块信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("模块信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ObservableCollection<ModuleManagerModel> dataList = new ObservableCollection<ModuleManagerModel>(result.Data.Data);
                    //ModuleManagerInfoList = new ObservableCollection<ModuleManagerModel>();
                    DataSource = new ObservableCollection<ModuleManagerModel>();
                    ModuleManagerInfoList = GetModuleManagerModels(null, dataList, 10);
                    
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    ModuleManagerInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ModuleManagerInfoList = new ObservableCollection<ModuleManagerModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询模块信息失败，请联系管理员！";
            }
        }

        public List<int> GetModuleIdsByRoleId(int roleId)
        {
            
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<int>>>(GlobalData.ServerRootUri + $"ModuleManager/GetRolesByModuleId/{roleId}");
            return result.Data;
        }

        private ObservableCollection<ModuleManagerModel> GetModuleManagerModels(int? parentId, ObservableCollection<ModuleManagerModel> dataList, int MarginLeft)
        {
            ObservableCollection<ModuleManagerModel> searchDataList = new ObservableCollection<ModuleManagerModel>();
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].ParentId == parentId)
                {
                    searchDataList.Add(dataList[i]);
                }
            }

            for (int i = 0; i < searchDataList.Count; i++)
            {
                if (searchDataList[i].ParentId == parentId)
                {
                    searchDataList[i].MarginLeft = MarginLeft;

                    searchDataList[i].PropertyChanged += OnPropertyChangedCommand;
                    DataSource.Add(searchDataList[i]);

                    var subDatas = GetModuleManagerModels(searchDataList[i].Id, dataList, MarginLeft + 20);
                    searchDataList[i].ModuleManagerModels = subDatas;
                }
            }

            return searchDataList;
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var selectedObj = sender as ModuleManagerModel;
            if (selectedObj == null)
                return;
            if (e.PropertyName.Equals("IsChecked"))
            {
                if (selectedObj.IsChecked)
                {
                    int next = DataSource.IndexOf(selectedObj) + 1;
                    if (selectedObj.ModuleManagerModels.Count > 0)
                    {
                        for (int i = 0; i < selectedObj.ModuleManagerModels.Count; i++)
                        {
                            var p = selectedObj.ModuleManagerModels[i];
                            if (DataSource.FirstOrDefault(t => t.Id == p.Id) != null)
                            {
                                return;
                            }
                            p.PropertyChanged += OnPropertyChangedCommand;
                            p.IsChecked = true;
                            DataSource.Insert(next++, p);
                        }
                    }
                    else
                    {
                        var p = selectedObj;
                        if (DataSource.FirstOrDefault(t => t.Id == p.Id) != null)
                        {
                            for (int j = 0; j < DataSource.Count; j++)
                            {
                                if (DataSource[j].Id == p.Id)
                                {
                                    DataSource[j].IsSelected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (!selectedObj.IsChecked)
                {
                    for (int i = 0; i < selectedObj.ModuleManagerModels.Count; i++)
                    {
                        var p = selectedObj.ModuleManagerModels[i];
                        p.IsChecked = false;
                        RemoveNode(p);
                    }
                }
            }
        }

        private void RemoveNode(ModuleManagerModel module)
        {
            for (int i = 0; i < module.ModuleManagerModels.Count; i++)
            {
                var p = module.ModuleManagerModels[i];
                RemoveNode(p);
            }
            for (int i = 0; i < DataSource.Count; i++)
            {
                var p = DataSource[i];
                if (p.Id == module.Id)
                    DataSource.Remove(p);
            }
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("Name", txt.Trim(),FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
       
            GetSelectedModules();
        }

        /// <summary>
        /// 获取已经选择的Modules数据
        /// </summary>
        private void GetSelectedModules()
        {
            //DataSource = new ObservableCollection<ModuleManagerModel>();
            List<int> ModuleIds = GetModuleIdsByRoleId(RoleModel.Id);
            foreach (var id in ModuleIds)
            {
                for (int i = 0; i < DataSource.Count; i++)
                {
                    if (id == DataSource[i].Id)
                    {
                        DataSource[i].IsChecked = true;
                        break;
                    }
                }
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
