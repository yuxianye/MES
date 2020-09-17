using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquKnifeToolTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EquKnifeToolTypeInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquKnifeToolTypeInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquKnifeToolTypeInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //getPageData(1, 50);
        }

        #region 刀具类别信息模型
        private KnifeToolTypeModel equipmentModel = new KnifeToolTypeModel();
        /// <summary>
        /// 刀具类别信息模型
        /// </summary>
        public KnifeToolTypeModel KnifeToolTypeModel
        {
            get { return equipmentModel; }
            set
            {
                Set(ref equipmentModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
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
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return KnifeToolTypeModel == null ? false : KnifeToolTypeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquKnifeToolTypeInfo/Add",
                Utility.JsonHelper.ToJson(new List<KnifeToolTypeModel> { KnifeToolTypeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<KnifeToolTypeModel>(KnifeToolTypeModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
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
        /// <summary>
        /// 为了使用下拉列表，借鉴新增的代码段
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 企业信息模型,用于列表数据显示
        private ObservableCollection<KnifeToolTypeModel> equKnifeToolTypeInfoList = new ObservableCollection<KnifeToolTypeModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<KnifeToolTypeModel> EquKnifeToolTypeInfoList
        {
            get { return equKnifeToolTypeInfoList; }
            set { Set(ref equKnifeToolTypeInfoList, value); }
        }

        ///******************************************************************88
        #region 设备信息模型,用于列表数据显示
        private ObservableCollection<KnifeToolTypeModel> equipmentInfoList1 = new ObservableCollection<KnifeToolTypeModel>();




        #endregion
        #region 当前浏览页面的页码

        private int totalCounts = 0;
        /// <summary>
        /// 所有记录的个数
        /// </summary>
        public int TotalCounts
        {
            get { return totalCounts; }
            set { Set(ref totalCounts, value); }
        }
        #endregion
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


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
           
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<KnifeToolTypeModel>>>(GlobalData.ServerRootUri + "EquKnifeToolTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取企业信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("企业信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {

                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquKnifeToolTypeInfoList = new ObservableCollection<KnifeToolTypeModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EquKnifeToolTypeInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquKnifeToolTypeInfoList = new ObservableCollection<KnifeToolTypeModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
            }
        }
    }
}
#endregion