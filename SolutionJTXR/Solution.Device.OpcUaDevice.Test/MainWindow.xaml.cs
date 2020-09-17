using Solution.Device.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solution.Device.OpcUaDevice.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //sw = new StreamWriter(fs, Encoding.Default);

            //swLog = new StreamWriter(fsLog, Encoding.Default);
            //swLog.AutoFlush = true;

            //System.Threading.Tasks.Task.Run(new Action(intiOpcClientHelper));
            intiOpcClientHelper();
            InitOpcDataitems();

            upMessageDelgate = new UpMessageDelgate(upMessage);
            upOpcDataItemsDelgate = new UpOpcDataItemsDelgate(upOpcDataItems);
            upOpcDataItemDelgate = new UpOpcDataItemDelgate(upOpcDataItem);
            //System.Threading.Tasks.Task.Run(new Action(writeLogAsync));
        }

        //FileStream fsLog = new FileStream($"{DateTime.Now.ToString("yyyyMMdd")}.log", FileMode.OpenOrCreate);
        //StreamWriter swLog;
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="xContent">内容</param>
        /// <param name="xFilePath">路径</param>
        public void WriteFile(string xContent)
        {
            //lock (lockObject)
            //{
            logStringQueue.Enqueue(xContent);

            //swLog.WriteAsync(xContent);

            //}

        }

        private void writeLogAsync()
        {
            Task.Factory.StartNew(() =>
            {
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    try
                    {

                        while (logStringQueue.Any())
                        {
                            logStringQueue.TryDequeue(out string log);
                            sb.Append(log);
                        }

                        //swLog.WriteAsync(sb.ToString());
                        sb.Clear();

                    }
                    catch (Exception ex)
                    {
                        //Logger.Error($"处理订阅信息异常！ + \n {ex.ToString()}");
                    }
                    //Task.Delay(10);

                    System.Threading.Thread.Sleep(1000);
                }
            });
        }
        ConcurrentQueue<string> logStringQueue = new ConcurrentQueue<string>();


        //FileStream fs = new FileStream("log.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
        //StreamWriter swData;
        private delegate void UpMessageDelgate(string a);
        private UpMessageDelgate upMessageDelgate;

        private delegate void UpOpcDataItemsDelgate(IList<OpcUaDeviceOutParamEntity> opcDataItems);
        private UpOpcDataItemsDelgate upOpcDataItemsDelgate;

        private delegate void UpOpcDataItemDelgate(OpcUaDeviceOutParamEntity opcDataItem);
        private UpOpcDataItemDelgate upOpcDataItemDelgate;

        private const string dateString = "yyyy-MM-dd HH:mm:ss ffff ";

        private void InitOpcDataitems()
        {
            StringBuilder sb = new StringBuilder();

#if DEBUG
            sb.Append("Channel_1.Device_1.Bool_1;1000;False;True;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_2;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_3;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_4;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_5;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel1.Device1.Tag1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel1.Device1.Tag2;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("S7:[S7 connection_52]DB800,X0.1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("S7 [S7_connection_52]DB800,X0.2;1000;0;0;Unknow");
            sb.AppendLine();
            txtOpcDataItems.Text = sb.ToString();
#else 
            txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);
#endif
            txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);

            sb.Clear();
            sb = null;

            WriteFile("数据点加载完成！");
        }

        private OpcUaDeviceHelper opcUaDeviceHelper;
        //private OpcUaDeviceHelper opcUaDeviceHelper 
        private const int updateRateGroup1 = 1000;
        private const int updateRateGroup2 = 1000;
        private const int updateRateGroup3 = 1000;

        private void intiOpcClientHelper()
        {
            opcUaDeviceHelper = new OpcUaDeviceHelper();
            opcUaDeviceHelper.Notification += OpcUaDeviceHelper_Notification; ;
            //opcUaDeviceHelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcUaDeviceHelper.OnDataChanged += OpcClienthelper_OnDataChanged;
            //var servers1 = OpcHelper.OpcClientHelper.GetOpcServers();
            //var servers2 = OpcHelper.OpcClientHelper.GetOpcServers("127.0.0.1");
            //clienthelper.Connect(servers1.First());
        }

        private void OpcUaDeviceHelper_Notification(Core.IDevice device, Core.DeviceEventArgs<Core.IDeviceParam> deviceEventArgs)
        {
            var deviceConnectParamEntityBase = deviceEventArgs.Data as DeviceConnectParamEntityBase;
            if (!Equals(deviceConnectParamEntityBase, null))
            {
                //if (deviceConnectParamEntityBase.StatusCode != 0)
                upMessage($"设备：{device.ToString()} 连接状态{deviceConnectParamEntityBase.ToString()}");

            }


            var deviceOutputParamEntityBase = deviceEventArgs.Data as DeviceOutputParamEntityBase;

            if (!Equals(deviceOutputParamEntityBase, null))
            {
                upMessage($"设备输出数据{device.ToString()}  {deviceEventArgs.Data.ToString()}");
            }
        }

        //private void OpcClienthelper_OnLogHappened(object sender, OpcHelper.OpcLogEventArgs e)
        //{
        //    string message = DateTime.Now.ToString(dateString) + e.Log + System.Environment.NewLine;
        //    try
        //    {
        //        asyncUpMessage(message);
        //    }
        //    catch (AggregateException ex)
        //    {
        //        //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
        //    }
        //}

        //private void asyncUpMessage(string message)
        //{
        //    upMessageDelgate.BeginInvoke(message, new AsyncCallback((result) =>
        //    {
        //        upMessageDelgate.EndInvoke(result);

        //    }), message);
        //    //this.Dispatcher.BeginInvoke(upMessageDelgate, message);
        //    //this.Dispatcher.Invoke(new Action(() =>
        //    //{
        //    //    this.txtMessage.Text = txtMessage.Text.Insert(0, message);
        //    //    int index = this.txtMessage.Text.LastIndexOf('\n');
        //    //    if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
        //    //    {
        //    //        this.txtMessage.Text = this.txtMessage.Text.Remove(index);
        //    //    }
        //    //}));
        //}

        private void upMessage(string message)
        {
            //lock (lockObject)
            //{
            //sw.WriteAsync(message);
            //    //sw.Write(message);

            //}

            // this.Dispatcher.Invoke(new Action(() =>
            //{
            //     //this.txtMessage.Text = txtMessage.Text.Insert(0, message);
            //     //int index = this.txtMessage.Text.LastIndexOf('\n');
            //     //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
            //     //{
            //     //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
            //     //}
            //     this.txtMessage.AppendText(message);
            //    this.txtMessage.AppendText(System.Environment.NewLine);
            //    txtMessage.ScrollToEnd();
            //     //int index = this.txtMessage.Text.LastIndexOf('\n');
            //     //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
            //     //{
            //     //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
            //     //}

            // }));

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                //this.txtMessage.Text = txtMessage.Text.Insert(0, message);
                //int index = this.txtMessage.Text.LastIndexOf('\n');
                //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
                //{
                //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
                //}
                this.txtMessage.AppendText(DateTime.Now.ToString() + message + System.Environment.NewLine);
                //this.txtMessage.AppendText(System.Environment.NewLine);
                txtMessage.ScrollToEnd();
                if (txtMessage.LineCount > 500)
                {
                    txtMessage.Text = null;

                }
                //int index = this.txtMessage.Text.LastIndexOf('\n');
                //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
                //{
                //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
                //}

            }));

            //System.Console.Write(message);
            //System.Diagnostics.Debug.Print(message);
            //WriteFile( message);
            //System.IO.File.AppendAllText("log.log", message);

            //this.txtMessage.Text = txtMessage.Text.Insert(0, message);
            //int index = this.txtMessage.Text.LastIndexOf('\n');
            //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
            //{
            //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
            //}
        }

        private void asyncUpOpcDataItems(IEnumerable<OpcUaDeviceOutParamEntity> opcDataItem)
        {
            this.Dispatcher.BeginInvoke(upOpcDataItemsDelgate, opcDataItem);
        }

        ObservableCollection<OpcUaDeviceOutParamEntity> dataGridDataSource = new ObservableCollection<OpcUaDeviceOutParamEntity>();
        private void upOpcDataItems(IEnumerable<OpcUaDeviceOutParamEntity> opcDataItem)
        {
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";

            //System.IO.File.AppendAllText("log.log", message);

            //gvOpcDataItems.ItemsSource = null;
            //gvOpcDataItems.ItemsSource = opcDataItem;
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            //this.Dispatcher.BeginInvoke );
            this.Dispatcher.Invoke(new Action(() =>
            {
                //dataGridDataSource
                gvOpcDataItems.ItemsSource = null;
                gvOpcDataItems.ItemsSource = dataGridDataSource;
                //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            }));
        }
        //OpcDataItem tm = new OpcDataItem("test", 1000, "0", "0", OpcResult.Unknow);
        private void upOpcDataItem(OpcUaDeviceOutParamEntity opcDataItem)
        {
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";

            //System.IO.File.AppendAllText("log.log", message);

            //gvOpcDataItems.ItemsSource = null;
            //gvOpcDataItems.ItemsSource = opcDataItem;
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            //this.Dispatcher.BeginInvoke );
            this.Dispatcher.Invoke(new Action(() =>
            {
                //dataGridDataSource
                //var v =  dataGridDataSource.First(a => a.Name == opcDataItem.Name) ;
                //  v = opcDataItem;

                //dataGridDataSource.Add(opcDataItem);

                //dataGridDataSource.Remove(opcDataItem);
                //var v = dataGridDataSource.FirstOrDefault(a => a.Name == opcDataItem.Name);
                //v = opcDataItem;
                //gvOpcDataItems.ItemsSource = null;
                //gvOpcDataItems.ItemsSource = dataGridDataSource;
                //this.txtb.Text = "(" + dataGridDataSource.Count(a => a.Quality == OpcResult.S_OK) + "/" + dataGridDataSource.Count() + ")";
            }));



        }

        //private void OpcClienthelper_OnErrorHappened(object sender, OpcHelper.OpcErrorEventArgs e)
        //{
        //    string message = DateTime.Now.ToString(dateString) + e.Message + (e.Exception == null ? "" : e.Exception.StackTrace) + System.Environment.NewLine;
        //    try
        //    {
        //        asyncUpMessage(message);
        //    }
        //    catch (AggregateException ex)
        //    {
        //        //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
        //    }
        //}

        //private void OpcClienthelper_OnDataChanged(object sender, OpcHelper.OpcDataEventArgs e)
        //{
        //    string message = DateTime.Now.ToString(dateString) + e.OpcResult + " " + (e.OpcDataItem == null ? " " : e.OpcDataItem.ToString()) + System.Environment.NewLine;


        //    try
        //    {
        //        int.TryParse(e.OpcDataItem.OldValue.ToString(), out int old);
        //        int.TryParse(e.OpcDataItem.NewValue.ToString(), out int newv);
        //        if (old + 3 != newv)
        //        {
        //            System.Diagnostics.Debug.Print($"值不连续{e.OpcDataItem.ToString()}");
        //        }
        //        else
        //        {
        //            //System.Diagnostics.Debug.Print($"连续{e.OpcDataItem.ToString()}");
        //            WriteFile(message);

        //        }

        //        //asyncUpMessage(message);

        //        //asyncUpOpcDataItems(this.opcClienthelper.OpcDataItems);

        //        //upOpcDataItemDelgate(e.OpcDataItem);
        //    }
        //    catch (AggregateException ex)
        //    {
        //        //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
        //    }
        //}



        /// <summary>
        /// 查询服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchOpcServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cboxOpcServers.ItemsSource = new string[] { "opc.tcp://192.168.1.234:49320" };
                cboxOpcServers.SelectedIndex = 0;
                upMessage("查询服务器完成！");

                //var servers1 = OpcHelper.OpcClientHelper.GetOpcServers();
                ////var servers2 = OpcHelper.OpcClientHelper.GetOpcServers("127.0.0.1");
                //if (!Equals(null, servers1) && servers1.Count() > 0)
                //{
                //    foreach (var v in servers1)
                //    {
                //        string message = DateTime.Now.ToString(dateString) + "可用的OPC服务器：" + v + System.Environment.NewLine;
                //        asyncUpMessage(message);
                //    }

                //    cboxOpcServers.ItemsSource = null;

                //    cboxOpcServers.ItemsSource = servers1;
                //    if (servers1.Count() > 0)
                //    {
                //        cboxOpcServers.SelectedIndex = 0;
                //    }
                //}
                //else
                //{
                //    asyncUpMessage(DateTime.Now.ToString(dateString) + "未找到可用的OPC服务器。" + System.Environment.NewLine);
                //}

            }
            catch (Exception ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {

            //opcUaDeviceHelper.Connect(cboxOpcServers.SelectedItem == null ? null : cboxOpcServers.SelectedItem.ToString());
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;
            DeviceConnectParamEntityBase deviceConnectParamEntity = new OpcUaDeviceConnectParamEntity();
            deviceConnectParamEntity.DeviceUrl = cboxOpcServers.SelectedItem == null ? null : cboxOpcServers.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(deviceConnectParamEntity.DeviceUrl))
            {
                upMessage($"请填写服务器地址！");
                return;
            }

            if (opcUaDeviceHelper.IsConnected)
            {
                upMessage($"服务器已经连接！");
                return;
            }
            var connectResult = opcUaDeviceHelper.Connect<DeviceConnectParamEntityBase, DeviceConnectParamEntityBase>(deviceConnectParamEntity).Result;
            upMessage($"连接设备结果！{connectResult.ToString()}");

            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;


        }

        /// <summary>
        /// 断开服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.OnErrorHappened -= OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnLogHappened -= OpcClienthelper_OnLogHappened;
            //opcClienthelper.DisConnect();
            //opcUaDeviceHelper.DisConnectAsync();
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;

            DeviceConnectParamEntityBase deviceConnectParamEntity = new OpcUaDeviceConnectParamEntity();
            if (opcUaDeviceHelper.IsConnected)
            {
                var connectResult = opcUaDeviceHelper.DisConnect<DeviceConnectParamEntityBase, DeviceConnectParamEntityBase>(deviceConnectParamEntity).Result;
                upMessage($"断开设备结果！{connectResult.ToString()}");
            }
            else
            {
                upMessage($"已经断开！");
            }
        }

        /// <summary>
        /// 订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDataItems_Click(object sender, RoutedEventArgs e)
        {

            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup3,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispose_Click(object sender, RoutedEventArgs e)
        {
            opcUaDeviceHelper.Dispose();
        }

        /// <summary>
        /// 增加订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnReAddDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup2,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup2,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 减少订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnDeleteDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 取消所有订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnbtnNoDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem>
            //{
            //    //new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",100,"","", OpcHelper.OpcResult.Unknow),
            //    //new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",200,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (opcUaDeviceHelper.IsConnected)
                {
                    if (MessageBox.Show("正在通讯中，确定要退出么？\r\r退出后所有通讯将关闭！", "OPC测试助手", MessageBoxButton.OKCancel, MessageBoxImage.Question)
                          == MessageBoxResult.OK)
                    {
                        //e.Cancel = true;
                        opcUaDeviceHelper.Dispose();
                        //sw.Close();
                        //fs.Close();
                        //fs.Flush();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }


                //swLog.Flush();
                //swLog.Close();
                //fsLog.Close();

            }
            catch
            {

            }

            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.OnLogHappened -= OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.DisConnect();
            //opcClienthelper.Dispose();
        }

        /// <summary>
        /// 增加无效订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnAddInvalidDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_20",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),

            //});
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnWriteDataItem_Click(object sender, RoutedEventArgs e)
        {
            if (!opcUaDeviceHelper.IsConnected)
            {
                upMessage("请先连接服务器");
                return;
            }

            DeviceInputParamEntityBase deviceInputParamEntityBase = new OpcUaDeviceInParamEntity();
            deviceInputParamEntityBase.NodeId = "ns=2;s=通道 1.设备 1.标记 00001";
            //deviceInputParamEntityBase.ValueType = typeof(UInt16);// clientDataEntity.ValueType;
            deviceInputParamEntityBase.Value = (UInt16)1;

            //OpcUaDeviceHelper opcUaDevice = GetOpcUaDevice(clientDataEntity.KeyId);
            //var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
            //    .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
            //    .DeviceNode.DeviceServerInfo.DeviceServerName;
            var opcUaDeviceOutParamEntity = opcUaDeviceHelper.Write<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(deviceInputParamEntityBase).Result;

            upMessage(opcUaDeviceOutParamEntity.ToString());


            DeviceInputParamEntityBase deviceInputParamEntityBase2 = new OpcUaDeviceInParamEntity();
            deviceInputParamEntityBase2.NodeId = "ns=2;s=通道 1.设备 1.标记 00002";
            //deviceInputParamEntityBase2.ValueType = typeof(UInt16);// clientDataEntity.ValueType;
            deviceInputParamEntityBase2.Value = (UInt16)1;

            //OpcUaDeviceHelper opcUaDevice = GetOpcUaDevice(clientDataEntity.KeyId);
            //var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
            //    .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
            //    .DeviceNode.DeviceServerInfo.DeviceServerName;
            var opcUaDeviceOutParamEntities =
                opcUaDeviceHelper.Writes<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(new DeviceInputParamEntityBase[] { deviceInputParamEntityBase, deviceInputParamEntityBase2 }).Result;

            foreach (var v in opcUaDeviceOutParamEntities)
            {
                upMessage(v.ToString());

            }




            //DeviceInputParamEntityBase deviceInputParamEntityBase = new OpcUaDeviceInParamEntity();
            //deviceInputParamEntityBase.NodeId = "ns=2;s=通道 1.设备 1.标记 00001";
            //deviceInputParamEntityBase.ValueType = typeof(UInt16);// clientDataEntity.ValueType;
            //deviceInputParamEntityBase.Value = 1;

            ////OpcUaDeviceHelper opcUaDevice = GetOpcUaDevice(clientDataEntity.KeyId);
            ////var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
            ////    .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
            ////    .DeviceNode.DeviceServerInfo.DeviceServerName;
            //var opcUaDeviceOutParamEntity = opcUaDeviceHelper.Write<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(deviceInputParamEntityBase).Result;

            //upMessage(opcUaDeviceOutParamEntity.ToString());



            //clientDataEntity.Value = opcUaDeviceOutParamEntity.Value;
            //clientDataEntity.StatusCode = opcUaDeviceOutParamEntity.StatusCode;


            //OpcDataItem opcDataItem = null;
            //OpcResult opcResult = OpcResult.Unknow;
            //if (Equals(null, opcUaDeviceHelper.OpcDataItems) || opcUaDeviceHelper.OpcDataItems.Count < 1)
            //{
            //    message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            //}
            //else
            //{
            //    opcDataItem = opcUaDeviceHelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
            //    //bool newValue = (DateTime.Now.Millisecond % 2) == 0 ? true : false;
            //    //bool newValue = !tmpValue;
            //    //tmpValue = newValue;
            //    //System.Diagnostics.Debug.Print(tmpValue.ToString());
            //    opcResult = opcUaDeviceHelper.Write(opcDataItem, 1);
            //    message = DateTime.Now.ToString(dateString) + "写入完成 " + opcResult + " " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;

            //}
        }

        /// <summary>
        /// 读取实时数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadDataItem_Click(object sender, RoutedEventArgs e)
        {
            //string message = null;

            //string message = null;
            if (!opcUaDeviceHelper.IsConnected)
            {
                upMessage("请先连接服务器");
                return;
            }

            DeviceInputParamEntityBase deviceInputParamEntityBase = new DeviceInputParamEntityBase();
            deviceInputParamEntityBase.NodeId = "ns=2;s=通道 1.设备 1.标记 00001";
            //deviceInputParamEntityBase.ValueType = typeof(UInt16);
            //deviceInputParamEntityBase.ValueType = clientDataEntity.ValueType;
            //OpcUaDeviceHelper opcUaDevice = GetOpcUaDevice(clientDataEntity.KeyId);
            //var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
            //    .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
            //    .DeviceNode.DeviceServerInfo.DeviceServerName;
            var opcUaDeviceOutParamEntity = opcUaDeviceHelper.Read<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(deviceInputParamEntityBase).Result;

            upMessage(opcUaDeviceOutParamEntity.ToString());




            DeviceInputParamEntityBase deviceInputParamEntityBase2 = new DeviceInputParamEntityBase();
            deviceInputParamEntityBase2.NodeId = "ns=2;s=通道 1.设备 1.标记 00002";
            //deviceInputParamEntityBase2.ValueType = typeof(UInt16);


            var opcUaDeviceOutParamEntities =
                opcUaDeviceHelper.Reads<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(new DeviceInputParamEntityBase[] { deviceInputParamEntityBase, deviceInputParamEntityBase2 }).Result;
            foreach (var v in opcUaDeviceOutParamEntities)
            {
                upMessage(v.ToString());
            }

            //if (!opcUaDeviceHelper.IsConnected)
            //{
            //    message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
            //    asyncUpMessage(message);
            //    return;
            //}
            //OpcDataItem opcDataItem;
            //if (Equals(null, opcUaDeviceHelper.OpcDataItems) || opcUaDeviceHelper.OpcDataItems.Count < 1)
            //{
            //    message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            //}
            //else
            //{
            //    //正常读取
            //    opcDataItem = opcUaDeviceHelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
            //    opcDataItem.Name = opcDataItem.Name;
            //    opcDataItem = opcUaDeviceHelper.Read(opcDataItem);
            //    message = DateTime.Now.ToString("HH:mm:ss ffff ") + "读完成 " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;
            //}
            //asyncUpMessage(message);
            //if (!Equals(null, opcUaDeviceHelper.OpcDataItems) && opcUaDeviceHelper.OpcDataItems.Count > 0)
            //{
            //    //无效读取
            //    var opcDataItem2 = opcUaDeviceHelper.OpcDataItems.LastOrDefault().Clone() as OpcDataItem;
            //    opcDataItem2.Name = opcDataItem2.Name + "xxx";
            //    opcDataItem2 = opcUaDeviceHelper.Read(opcDataItem2);
            //    message = DateTime.Now.ToString(dateString) + "读完成 " + (opcDataItem2 == null ? " " : opcDataItem2.ToString()) + System.Environment.NewLine;
            //    asyncUpMessage(message);
            //}
        }

        /// <summary>
        /// 读取缓存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCacheDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message = null; ;
            if (!opcUaDeviceHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                upMessage(message);
                return;
            }
            //if (Equals(null, opcUaDeviceHelper.OpcDataItems) || opcUaDeviceHelper.OpcDataItems.Count < 1)
            //{
            //    message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            //}
            //else
            //{
            //    var opcDataItem = opcUaDeviceHelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
            //    message = DateTime.Now.ToString(dateString) + "读完成 " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;
            //}
            upMessage(message);
        }

        private void btnUpdateDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!opcUaDeviceHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                upMessage(message);
                return;
            }
            //var strList = txtOpcDataItems.Text.Split(System.Environment.NewLine.ToCharArray());
            var strList = txtOpcDataItems.Text.Split('\r', '\n');

            //List<OpcDataItem> opcDataItems = new List<OpcDataItem>(strList.Count());
            //txtOpcDataItems .Text.Split (System.Environment.NewLine):
            dynamic[] dynamics;//= new dynamic[strList.Count()];
                               //foreach (var strOpcDataItem in strList)
                               //{
                               //    var strOpcDataItemTmp = strOpcDataItem.Split(';');
                               //    if (strOpcDataItemTmp.Count() < 2)
                               //    {
                               //        continue;
                               //    }
                               //    OpcDataItem opcDataItem =
                               //        new OpcDataItem(strOpcDataItemTmp[0], int.Parse(strOpcDataItemTmp[1]), strOpcDataItemTmp[2], strOpcDataItemTmp[3], (OpcResult)Enum.Parse(typeof(OpcResult), strOpcDataItemTmp[4]));
                               //    opcDataItems.Add(opcDataItem);
            List<OpcUaNodeEntity> nodelist = new List<OpcUaNodeEntity>();

            foreach (var str in strList)
            {

                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }
                dynamic nodeEntity = new OpcUaNodeEntity();
                var nodeEntityStr = str.Split('|');
                if (nodeEntityStr.Count() < 2)
                {
                    return;
                }
                nodeEntity.NodeId = nodeEntityStr[0];//..n $"ns=2;s=通道 1.设备 1.标记 {(i + 1).ToString().PadLeft(5, '0')}";
                                                     //nodeEntity.ValueType = Type.GetType($"System.{deviceNodes[i].DataType}", false);
                nodeEntity.Interval = int.Parse(nodeEntityStr[1]);
                nodeEntity.ValueType = typeof(UInt16);
                nodelist.Add(nodeEntity);
            }
            dynamics = nodelist.ToArray();
            if (!opcUaDeviceHelper.IsConnected)
            {
                upMessage($"服务器未连接！");
                return;
            }

            var registerResult = opcUaDeviceHelper.RegisterNodes<DeviceOutputParamEntityBase>(dynamics).Result;
            upMessage($"注册数据点结果！{registerResult.ToString()}");

            //}
            //opcUaDeviceHelper.RegisterOpcDataItemsAsync(opcDataItems);
            //dataGridDataSource = new ObservableCollection<OpcDataItem>(opcDataItems);
            //gvOpcDataItems.ItemsSource = dataGridDataSource;
            //this.txtb.Text = "(" + dataGridDataSource.Count(a => a.Quality == OpcResult.S_OK) + "/" + dataGridDataSource.Count() + ")";

            //for (int i = 0; i < dynamics.Count(); i++)
            //{
            //    //var nodeEntity = Assembly.Load(deviceServer.DeviceDriveAssemblyName).
            //    //CreateInstance($"{deviceServer.DeviceDriveAssemblyName}.{deviceServer.DeviceServerName}NodeEntity") as dynamic;
            //    dynamic nodeEntity = new OpcUaNodeEntity();
            //    var nodeEntityStr = strList[i].Split('|');
            //    if (nodeEntityStr.Count() < 2)
            //    {
            //        return;
            //    }
            //    nodeEntity.NodeId = nodeEntityStr[0];//..n $"ns=2;s=通道 1.设备 1.标记 {(i + 1).ToString().PadLeft(5, '0')}";
            //    //nodeEntity.ValueType = Type.GetType($"System.{deviceNodes[i].DataType}", false);
            //    nodeEntity.Interval = int.Parse(nodeEntityStr[1]);
            //    dynamics[i] = nodeEntity;
            //}







        }

        private void btnCreateData_Click(object sender, RoutedEventArgs e)
        {

            if (System.IO.File.Exists("数据点.txt"))
            {
                System.IO.File.Copy("数据点.txt", $"数据点{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
            }

            int lineCount = 2000;
            int.TryParse(txtCreateDataCount.Text, out lineCount);
            lineCount = lineCount + 1;
            List<string> lines = new List<string>(lineCount);
            for (int i = 1; i < lineCount; i++)
            {
                lines.Add($"ns=2;s=通道 1.设备 1.标记 {i.ToString().PadLeft(5, '0')}|100");
            }


            System.IO.File.WriteAllLines("数据点.txt", lines, Encoding.Default);

            upMessage($"数据点已生成:{lineCount}");

            //sb.Append("Channel_1.Device_1.Bool_1;1000;False;True;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_2;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_3;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_4;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_5;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel1.Device1.Tag1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel1.Device1.Tag2;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("S7:[S7 connection_52]DB800,X0.1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("S7 [S7_connection_52]DB800,X0.2;1000;0;0;Unknow");
            //sb.AppendLine();



            //txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt");












        }
    }
}
