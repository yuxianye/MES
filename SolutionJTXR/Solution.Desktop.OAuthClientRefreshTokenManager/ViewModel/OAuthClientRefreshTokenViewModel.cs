using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.OAuthClientRefreshTokenManager.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.OAuthClientRefreshTokenManager.ViewModel
{
    public class OAuthClientRefreshTokenViewModel : PageableViewModelBase
    {
        public OAuthClientRefreshTokenViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region 客户端Token信息模型
        /// <summary>
        /// 客户端Token信息模型
        /// </summary>
        private Model.OAuthClientRefreshTokenModel oAuthClientRefreshTokenInfo;// = new EnterpriseModel();
        /// <summary>
        /// 客户端Token信息模型
        /// </summary>
        public OAuthClientRefreshTokenModel OAuthClientRefreshTokenInfo
        {
            get { return oAuthClientRefreshTokenInfo; }
            set { Set(ref oAuthClientRefreshTokenInfo, value); }
        }
        #endregion

        #region 客户端Token信息信息模型,用于列表数据显示
        private ObservableCollection<Model.OAuthClientRefreshTokenModel> oAuthClientRefreshTokenInfoList = new ObservableCollection<Model.OAuthClientRefreshTokenModel>();

        /// <summary>
        /// 客户端Token信息信息数据
        /// </summary>
        public ObservableCollection<Model.OAuthClientRefreshTokenModel> OAuthClientRefreshTokenInfoList
        {
            get { return oAuthClientRefreshTokenInfoList; }
            set { Set(ref oAuthClientRefreshTokenInfoList, value); }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// 可恢复的删除命令
        /// </summary>
        //public ICommand RecycleCommand { get; set; }

        /// <summary>
        /// 物理删除命令
        /// </summary>
        public ICommand PhysicalDeleteCommand { get; set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// 导入命令
        /// </summary>
        public ICommand ImportCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand ExportCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            //RecycleCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRecycleCommand, OnCanExecuteRecycleCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<Model.OAuthClientRefreshTokenModel>(this, MessengerToken.DataChanged, this.dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(Model.OAuthClientRefreshTokenModel commSocketServerModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = OAuthClientRefreshTokenInfoList.FirstOrDefault(a => a.Id == commSocketServerModel.Id);
            this.OAuthClientRefreshTokenInfo = OAuthClientRefreshTokenInfoList.FirstOrDefault();
            ////新增、不存在的数据插入到第一行便于查看
            //if (Equals(tmpModel, null))
            //{
            //    this.EnterpriseInfoList.Insert(0, enterpriseModel);
            //    //this.EnterpriseInfoList.Insert(0, enterpriseModel);
            //    EnterpriseInfoList.RemoveAt(this.EnterpriseInfoList.Count - 1);
            //}
            //else
            //{
            //    //修改的更新后置于第一行，便于查看
            //    tmpModel = enterpriseModel;
            //    EnterpriseInfoList.Move(EnterpriseInfoList.IndexOf(tmpModel), 0);
            //    tmpModel = enterpriseModel;
            //}
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            //Messenger.Default.Unregister<ViewInfo>(this, Model.MessengerToken.Navigate, Navigate);

            //Messenger.Default.Unregister<object>(this, Model.MessengerToken.ClosePopup, ClosePopup);

            //Messenger.Default.Unregister<MenuInfo>(this, Model.MessengerToken.SetMenuStatus, SetMenuStatus);
        }
        #endregion

        #region 分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 分页执行函数
        /// </summary>
        /// <param name="e"></param>
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
            getPageData(e.PageIndex, e.PageSize);
        }
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

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<Model.OAuthClientRefreshTokenModel>>>(GlobalData.ServerRootUri + "OAuthClientRefreshTokenManager/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取客户端Token信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("客户端Token信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    OAuthClientRefreshTokenInfoList = new ObservableCollection<Model.OAuthClientRefreshTokenModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    OAuthClientRefreshTokenInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                OAuthClientRefreshTokenInfoList = new ObservableCollection<Model.OAuthClientRefreshTokenModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询客户端Token信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteAddCommand()
        {
            //客户端Token与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Add");
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAddCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                //与服务端对应
                if (base.MenuModule.Functions.Any(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Add"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 执行编辑命令
        /// </summary>
        private void OnExecuteEditCommand()
        {
            //Controller和Action 与配置文件对应
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Update");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.OAuthClientRefreshTokenInfo.Clone();
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEditCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Update"))
                {
                    if (Equals(OAuthClientRefreshTokenInfo, null))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行逻辑删除命令
        /// </summary>
        //private async void OnExecuteRecycleCommand()
        //{
        //    var window = (MetroWindow)Application.Current.MainWindow;
        //    var dialogResult = await window.ShowMessageAsync("数据删除"
        //        , string.Format("确定要删除【{0}】这条数据么？删除后可在管理界面恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", OAuthClientRefreshTokenInfo.Client_Name)
        //        , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "OAuthClientRefreshTokenManager/Recycle", Utility.JsonHelper.ToJson(new List<Model.OAuthClientRefreshTokenModel>() { OAuthClientRefreshTokenInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            Application.Current.Resources["UiMessage"] = result?.Message;
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            OAuthClientRefreshTokenInfo = null;
        //        }
        //        else
        //        {
        //            //操作失败，显示错误信息
        //            Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //        }
        //    }

        //}

        /// <summary>
        /// 是否可以执行逻辑删除命令
        /// </summary>
        /// <returns></returns>
        //private bool OnCanExecuteRecycleCommand()
        //{
        //    if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Recycle"))
        //        {
        //            if (Equals(OAuthClientRefreshTokenInfo, null))
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private void OnExecutePhysicalDeleteCommand()
        {
            if (MessageBox.Show("是否删除该客户端令牌数据？", "数据删除", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {

                //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "OAuthClientRefreshTokenManager/Remove", Utility.JsonHelper.ToJson(new List<CommSocketServerModel>() { OAuthClientRefreshTokenInfo }));
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "OAuthClientRefreshTokenManager/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { OAuthClientRefreshTokenInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    OAuthClientRefreshTokenInfo = null;
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                }
            }

        }

        /// <summary>
        /// 是否可以执行物理删除命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePhysicalDeleteCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Remove"))
                {
                    if (Equals(OAuthClientRefreshTokenInfo, null))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void OnExecuteRefreshCommand()
        {
            pageRepuestParams.FilterGroup = null;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        /// <summary>
        /// 执行导入命令
        /// </summary>
        private void OnExecuteImportCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行导入命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteImportCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "OAuthClientRefreshTokenManager" && a.Action == "Add"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行导出命令
        /// </summary>
        private void OnExecuteExportCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行导出命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteExportCommand()
        {
            if (Equals(OAuthClientRefreshTokenInfoList, null) || !OAuthClientRefreshTokenInfoList.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行打印命令
        /// </summary>
        private void OnExecutePrintCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行打印命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePrintCommand()
        {
            if (Equals(OAuthClientRefreshTokenInfoList, null) || !OAuthClientRefreshTokenInfoList.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("Name", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleArea = new FilterRule("Area", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleController = new FilterRule("Controller", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleAction = new FilterRule("Action", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleArea);
            filterGroup.Rules.Add(filterRuleController);
            filterGroup.Rules.Add(filterRuleAction);

            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);

        }

        #endregion

        /// <summary>
        /// 清理资源
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
            unRegisterMessenger();
        }

    }
}
