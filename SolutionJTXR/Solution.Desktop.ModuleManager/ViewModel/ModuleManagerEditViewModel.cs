using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.ModuleManager.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.FunctionManager.Model;
using System.Collections.ObjectModel;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;
using System.Linq;

namespace Solution.Desktop.ModuleManager.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class ModuleManagerEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModuleManagerEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.ModuleManagerModel = parameter as ModuleManagerModel;


            if (ModuleManagerModel.ParentId != null)
            {
                getPageData(1, 1000000);
                getFunctionPageData(1, 1000000);
                SelectCheckedFunctions();
                this.ParentComboxInfo = ParentComboxInfoList.Where(x => x.ParentId.Equals(ModuleManagerModel.ParentId.ToString())).FirstOrDefault();
            }
        }


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

        private ParentComboxData parentComboxData = new ParentComboxData();
        public ParentComboxData ParentComboxInfo
        {
            get { return parentComboxData; }
            set
            {
                if (value == null)
                {
                    //this.ParentComboxInfo = ParentComboxInfoList.Where(x => x.ParentId.Equals(ModuleManagerModel.Id.ToString())).FirstOrDefault();
                    //Set(ref parentComboxData, ParentComboxInfoList.Where(x => x.ParentId.Equals(ModuleManagerModel.ParentId.ToString())).FirstOrDefault());
                }
                else
                {
                    Set(ref parentComboxData, value);
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private List<ParentComboxData> parentComboxDatas = new List<ParentComboxData>();
        public List<ParentComboxData> ParentComboxInfoList
        {
            get { return parentComboxDatas; }
            set
            {
                Set(ref parentComboxDatas, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #region 功能信息模型
        /// <summary>
        /// 功能信息模型
        /// </summary>
        private FunctionModel functionInfo = new FunctionModel();
        /// <summary>
        /// 功能信息模型
        /// </summary>
        public FunctionModel FunctionInfo
        {
            get { return functionInfo; }
            set { Set(ref functionInfo, value); }
        }
        #endregion

        #region 功能信息信息模型,用于列表数据显示
        private ObservableCollection<FunctionModel> functionInfoList = new ObservableCollection<FunctionModel>();

        /// <summary>
        /// 功能信息信息数据
        /// </summary>
        public ObservableCollection<FunctionModel> FunctionInfoList
        {
            get { return functionInfoList; }
            set { Set(ref functionInfoList, value); }
        }
        #endregion

        public ObservableCollection<FunctionModel> LocalFunctionInfoList { get; set; } = new ObservableCollection<FunctionModel>();

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }


        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return ModuleManagerModel == null ? false : ModuleManagerModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            ModuleManagerModel.ParentId = Convert.ToInt16(ParentComboxInfo.ParentId);
            ModuleManagerModel.FunctionModels = LocalFunctionInfoList;

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ModuleManager/Update",
                Utility.JsonHelper.ToJson(ModuleManagerModel ));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ModuleManagerModel>(ModuleManagerModel, MessengerToken.DataChanged);
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
                    ModuleManagerInfoList = new ObservableCollection<ModuleManagerModel>(result.Data.Data);
                    foreach (var data in ModuleManagerInfoList)
                    {
                        ParentComboxData parentComboxData = new ParentComboxData();
                        parentComboxData.ParentId = data.Id.ToString();
                        parentComboxData.Name = data.Name;
                        if (data.ParentId == null)
                        {
                            ParentComboxInfoList.Add(parentComboxData);
                        }
                    }
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

        private void getFunctionPageData(int pageIndex, int pageSize)
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<FunctionModel>>>(GlobalData.ServerRootUri + "FunctionManager/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取功能信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("功能信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    FunctionInfoList = new ObservableCollection<FunctionModel>();
                    foreach (var data in result.Data.Data)
                    {
                        FunctionModel modelData = new FunctionModel();
                        modelData = data;
                        modelData.PropertyChanged += OnPropertyChangedCommand;
                        FunctionInfoList.Add(modelData);
                    }
                }
                else
                {
                    FunctionInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                FunctionInfoList = new ObservableCollection<FunctionModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询功能信息失败，请联系管理员！";
            }
        }

        public List<FunctionModel> GetFunctionModelsByModuleId(int moduleId)
        {
            
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<FunctionModel>>>(GlobalData.ServerRootUri + $"ModuleManager/GetFunctionList/{moduleId}");
            return result.Data;
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as FunctionModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    LocalFunctionInfoList.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    LocalFunctionInfoList.Remove(selectedObj);
                }
            }
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            if (ModuleManagerModel.ParentId != null)
            {
                FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
                FilterRule filterRuleName = new FilterRule("Name", txt.Trim(), FilterOperate.Contains);
                FilterRule filterRuleController = new FilterRule("Controller", txt.Trim(), FilterOperate.Contains);
                FilterRule filterRuleAction = new FilterRule("Action", txt.Trim(), FilterOperate.Contains);
                filterGroup.Rules.Add(filterRuleName);
                filterGroup.Rules.Add(filterRuleController);
                filterGroup.Rules.Add(filterRuleAction);
                pageRepuestParams.FilterGroup = filterGroup;
                getFunctionPageData(1, 1000000);
                SelectCheckedFunctions();
            }
        }

        private void SelectCheckedFunctions()
        {
            LocalFunctionInfoList = new ObservableCollection<FunctionModel>();
            var allModuleFunctions = GetFunctionModelsByModuleId(ModuleManagerModel.Id);
            if (allModuleFunctions.Any())
            {
                foreach (var data in allModuleFunctions)
                {
                    for (int i = 0; i < FunctionInfoList.Count; i++)
                    {
                        if (data.Id == FunctionInfoList[i].Id)
                        {
                            FunctionInfoList[i].IsChecked = true;
                        }
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


    public class ParentComboxData
    {
        public string ParentId { get; set; }
        public string Name { get; set; }

    }

}
