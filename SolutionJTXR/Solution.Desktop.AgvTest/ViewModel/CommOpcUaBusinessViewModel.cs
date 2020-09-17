using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.AgvTest.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Device.Core;
using Solution.Device.OpcUaDevice;
using Solution.Utility;
using Solution.Utility.Socket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.AgvTest.ViewModel
{
    public class CommOpcUaBusinessViewModel : PageableViewModelBase
    {
        public CommOpcUaBusinessViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            SocketInit();
        }

        #region OpcUa业务数据模型
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        private CommOpcUaBusinessModel commOpcUaBusinessInfo;// = new EnterpriseModel();
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        public CommOpcUaBusinessModel CommOpcUaBusinessInfo
        {
            get { return commOpcUaBusinessInfo; }
            set { Set(ref commOpcUaBusinessInfo, value); }
        }
        #endregion

        #region OpcUa业务数据模型,用于列表数据显示
        private ObservableCollection<CommOpcUaBusinessModel> commOpcUaBusinessInfoList = new ObservableCollection<CommOpcUaBusinessModel>();

        /// <summary>
        /// OpcUa业务数据
        /// </summary>
        public ObservableCollection<CommOpcUaBusinessModel> CommOpcUaBusinessInfoList
        {
            get { return commOpcUaBusinessInfoList; }
            set { Set(ref commOpcUaBusinessInfoList, value); }
        }
        #endregion

        private string _clickBtnName = "连接";

        public string ClickBtnName
        {
            get { return _clickBtnName; }
            set { Set(ref _clickBtnName,value); }
        }

        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand ConnectCommand { get; set; }

      
        private void initCommand()
        {
            ConnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConnectCommand, OnCanExecuteConnectCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<CommOpcUaBusinessModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(CommOpcUaBusinessModel CommOpcUaBusinessModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = CommOpcUaBusinessInfoList.FirstOrDefault(a => a.Id == CommOpcUaBusinessModel.Id);
            this.CommOpcUaBusinessInfo = CommOpcUaBusinessInfoList.FirstOrDefault();
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
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaBusinessModel>>>(GlobalData.ServerRootUri + "CommOpcUaBusiness/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取OpcUa业务数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("OpcUa业务数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    CommOpcUaBusinessInfoList = new ObservableCollection<CommOpcUaBusinessModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    CommOpcUaBusinessInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                CommOpcUaBusinessInfoList = new ObservableCollection<CommOpcUaBusinessModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询OpcUa业务数据失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteConnectCommand()
        {
            if (client != null)
            {
                if (connectResult ^= true)
                {
                    Task<bool> resultTask = client.ConnectAsync();
                    bool result = resultTask.Result;
                    Application.Current.Resources["UiMessage"] = result ? "连接成功！" : "连接失败！";
                    if (!result)
                    {
                        connectResult = false;
                    }

                }
                else
                {
                    Task<bool> resultTask = client.CloseAsync();
                    bool result = resultTask.Result;
                    Application.Current.Resources["UiMessage"] = result ? "断开连接成功！" : "断开连接失败！";
                    if (!result)
                    {
                        connectResult = true;
                    }
                }
            }
            else
            {
                Application.Current.Resources["UiMessage"] = "操作失败！Socket Client为空！";
            }
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConnectCommand()
        {
            return true;
        }

        #endregion

        #region Tcp客户端读取OPC数据

        private bool connectResult = false;
        private SocketClientHelper client = new SocketClientHelper(Utility.ConfigHelper.GetAppSetting("ServerIp"), Convert.ToInt32(Utility.ConfigHelper.GetAppSetting("ServerPort")));

        private void SocketInit()
        {
            client.Connected += OnConnect;
            client.Closed += OnClose;
            client.OnDataReceived += OnReceive;
            client.Error += OnError;
        }

        private void SubScription()
        {
            SocketJsonParamEntity socketJsonParamEntity = new SocketJsonParamEntity();
            //socketJsonParamEntity.KeyId = Guid.Parse("2B68E863-6F5D-E811-8BA9-F8A005475E49");
            //socketJsonParamEntity.NodeId = "ns=2;s=TestChannel.TestDevice.word_1";
            socketJsonParamEntity.FunctionCode = FuncCode.SubScription;
            List<Guid> subIdList = new List<Guid>();
            for (int i = 0; i < CommOpcUaBusinessInfoList.Count; i++)
            {
                subIdList.Add(CommOpcUaBusinessInfoList[i].Id);
            }

            socketJsonParamEntity.SubScriptionList = subIdList;

            client.Send(GetMessage(Utility.JsonHelper.ToJson(socketJsonParamEntity)));
        }

        private String GetMessage(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
        }

        private void OnError(object sender, Exception e)
        {
            Application.Current.Resources["UiMessage"] = e.ToString();
        }

        private void OnConnect(object sender, EventArgs args)
        {
            SubScription();
            ClickBtnName = "断开";
            Application.Current.Resources["UiMessage"] = "连接成功！";
        }

        private void OnClose(object sender, EventArgs args)
        {
            ClickBtnName = "连接";
            Application.Current.Resources["UiMessage"] = "连接已断开！";
        }

        private void OnReceive(object sender, DataEventArgs args)
        {
            Task.Run(()=> 
            {
                String jsonStr = args.PackageInfo.Data;
                SocketJsonParamEntity socketJsonParamEntity = Utility.JsonHelper.FromJson<SocketJsonParamEntity>(jsonStr);
                switch (socketJsonParamEntity.FunctionCode)
                {
                    case FuncCode.Read:
                        break;
                    case FuncCode.Write:
                        break;
                    case FuncCode.SubScription:
                        {
                            if (socketJsonParamEntity.StatusCode == (uint)DeviceStatusCode.SubscriptionOK)
                            {
                                Application.Current.Resources["UiMessage"] = "订阅成功！";
                            }
                            else
                            {
                                for (int i = 0; i < CommOpcUaBusinessInfoList.Count; i++)
                                {
                                    if (CommOpcUaBusinessInfoList[i].NodeId.Equals(socketJsonParamEntity.KeyId))
                                    {
                                        CommOpcUaBusinessInfoList[i].Value = socketJsonParamEntity.Value.ToString();
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

            });
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

        /// <summary>
        /// 重写以实现释放派生类资源的逻辑
        /// </summary>
        protected override void Disposing()
        {
            if (client != null && connectResult)
            {
                Task<bool> resultTask = client.CloseAsync();
                bool result = resultTask.Result;
            }
        }

    }
}
