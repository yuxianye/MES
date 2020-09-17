using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommSocketServer.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Desktop.RoleManager.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.UserManager.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class UserAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getRolePageData(1, 1000000);
        }

        #region 用户信息模型
        /// <summary>
        /// 用户信息模型
        /// </summary>
        private UserModel _userInfo = new UserModel();
        /// <summary>
        /// 用户信息模型
        /// </summary>
        public UserModel UserInfo
        {
            get { return _userInfo; }
            set
            {
                Set(ref _userInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 角色信息模型
        /// <summary>
        /// 角色信息模型
        /// </summary>
        private RoleModel _roleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 角色信息模型
        /// </summary>
        public RoleModel RoleInfo
        {
            get { return _roleInfo; }
            set { Set(ref _roleInfo, value); }
        }
        #endregion

        #region 角色信息模型,用于列表数据显示
        private ObservableCollection<RoleModel> _roleInfoList = new ObservableCollection<RoleModel>();

        /// <summary>
        /// 角色信息信息数据
        /// </summary>
        public ObservableCollection<RoleModel> RoleInfoList
        {
            get { return _roleInfoList; }
            set { Set(ref _roleInfoList, value); }
        }
        #endregion

        #region 角色信息列表，用于选取角色

        /// <summary>
        /// 用于选取角色数据
        /// </summary>
        private ObservableCollection<RoleModel> _dataSource = new ObservableCollection<RoleModel>();

        public ObservableCollection<RoleModel> DataSource
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
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return UserInfo == null ? false : UserInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            UserInfo.CreatedTime = DateTime.Now;
            UserInfo.EmailConfirmed = true;
            UserInfo.PhoneNumberConfirmed = true;
            UserInfo.Roles = DataSource;

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "UserManager/Add",
                Utility.JsonHelper.ToJson(new List<UserModel> { UserInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<UserModel>(UserInfo, MessengerToken.DataChanged);
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
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getRolePageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RoleModel>>>(GlobalData.ServerRootUri + "RoleManager/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取角色信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("角色信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    foreach (var data in result.Data.Data)
                    {
                        RoleModel role = new RoleModel();
                        role = data;
                        role.PropertyChanged += OnPropertyChangedCommand;
                        RoleInfoList.Add(role);
                    }
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    RoleInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                RoleInfoList = new ObservableCollection<RoleModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询角色信息失败，请联系管理员！";
            }
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as RoleModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    DataSource.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    DataSource.Remove(selectedObj);
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
