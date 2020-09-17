using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
//using Solution.Desktop.Core;
//using Solution.Desktop.Core;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// 主控制器（主页面）
    /// 主要流程和功能：
    /// 1.登陆管理，登录页面的显示，登陆页面结果通过消息发送到本主控制器（主页面），然后通过web接口获取当前用户权限菜单。
    /// 2.初始化功能菜单，
    /// 3.页面导航,根据用户权限动态生成子页面，每个功能只能显示一个子页面，没有权限的导航菜单和子页面不显示，重复点击菜单时，调出已经实例的子页面。
    /// 4.页面管理,子页面用docker形式，根据用户权限管理子页面，每个功能只能显示一个子页面，没有权限的菜单和按钮等不显示，
    /// 5.子页面职责，每个子页面的逻辑由各自负责处理，如果子页面需要更新主页面的菜单状态等，通过消息发送给主控制器（主页面）。
    /// 6.子页面需要的数据、实时信息、报警等内容通过子页面自己新建通讯实例和访问web api，并负责释放相关资源，主控制器在关闭子页面时调用dispos接口。
    /// </summary>
    public class MainViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewModel()
        {
            registerMessenger();
            startLogin();
        }

        #region 注册和取消注册MVVMLifht消息

        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<ViewInfo>(this, MessengerToken.Navigate, navigate);
            Messenger.Default.Register<LoginUser>(this, MessengerToken.LoginSuccess, loginSuccess);
            Messenger.Default.Register<bool>(this, MessengerToken.LoginExit, loginExit);
            Messenger.Default.Register<bool>(this, MessengerToken.ClosePopup, closePopView);
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<ViewInfo>(this, MessengerToken.Navigate, navigate);
            Messenger.Default.Unregister<LoginUser>(this, MessengerToken.LoginSuccess, loginSuccess);
            Messenger.Default.Unregister<bool>(this, MessengerToken.LoginExit, loginExit);
            Messenger.Default.Unregister<bool>(this, MessengerToken.ClosePopup, closePopView);
        }

        #endregion

        /// <summary>
        /// 开始登陆
        /// </summary>
        private void startLogin()
        {

#if DEBUG
            //调试时验证设计模式,发布
            if (!DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow))
            {
#endif
                ViewInfo viewInfo = new ViewInfo(
                    "用户登陆",
                    ViewType.PopupNoTitle,
                    "Solution.Desktop.View",
                    "Solution.Desktop.View.LoginView",
                    "Solution.Desktop.ViewModel",
                    "Solution.Desktop.ViewModel.LoginViewModel",
                    "pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_32X32.ico");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
#if DEBUG
            }
#endif
        }


        private WindowButtonCommands windowButtonCommands = (Application.Current.MainWindow as MetroWindow).WindowButtonCommands;

        /// <summary>
        /// 按钮
        /// </summary>
        public WindowButtonCommands WindowButtonCommands
        {
            get { return windowButtonCommands; }
            set { Set(ref windowButtonCommands, value); }
        }

        #region 当前登录用户
        public LoginUser currentLoginUser;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public LoginUser CurrentLoginUser
        {
            get { return currentLoginUser; }
            set { Set(ref currentLoginUser, value); }
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        /// <param name="loginUser"></param>
        private void loginSuccess(LoginUser loginUser)
        {
            init();
            closePopView(true);
        }

        /// <summary>
        /// 登陆界面退出
        /// </summary>
        /// <param name="loginResult"></param>
        private void loginExit(bool loginResult)
        {
            closePopView(true);
            Application.Current.Shutdown();
        }
        #endregion

        #region 版本

        private string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// 当前版本
        /// </summary>
        public string Version
        {
            get { return version; }
            set { Set(ref version, value); }
        }
        #endregion

        /// <summary>
        /// 初始化的总入口，登陆成功之后初始化，初始化时需要按流程顺序
        /// </summary>
        private void init()
        {
            Application.Current.Resources["UiMessage"] = "初始化完成！";
            try
            {
                CurrentLoginUser = GlobalData.CurrentLoginUser;
                loadMenuFunctionViewInfoMaps();
                initCommand();
                initMenuItems();
                //initTopContentControl();
                initMianContentControl();

                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
            catch (Exception ex)
            {

                Application.Current.Resources["UiMessage"] = "系统初始化错误，请联系管理员！" + ex.Message;
                LogHelper.Error(Application.Current.Resources["UiMessage"].ToString(), ex);
            }
        }

        /// <summary>
        /// 初始化主页面使用的Command
        /// </summary>
        private void initCommand()
        {
            MenuCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<TreeViewItem>(OnExecuteMenuCommand);
            ModifyPasswordCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteModifyPasswordCommand);
            LogoutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLogoutCommand);
            CloseCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCloseCommand);
            SettingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSettingCommand);
        }

        #region 修改密码、注销，关闭、系统设置 命令

        public ICommand ModifyPasswordCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        private void OnExecuteModifyPasswordCommand()
        {
            try
            {
                ViewInfo viewInfo = new ViewInfo("修改密码", ViewType.Popup, "Solution.Desktop.View", "Solution.Desktop.View.ModifyPasswordView", "Solution.Desktop.ViewModel", "Solution.Desktop.ViewModel.ModifyPasswordViewModel", "pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "打开设置页面失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);
            }
        }

        private async void OnExecuteLogoutCommand()
        {
            try
            {
                var window = (MetroWindow)Application.Current.MainWindow;
                var dialogResult = await window.ShowMessageAsync("注销用户"
                    , "确定要注销用户么？" + System.Environment.NewLine + "确认注销点击【是】，继续使用点击【否】。"
                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    Application.Current.Shutdown(1);
                }
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "注销用户失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("注销用户失败！请与管理员联系！", ex);
            }
        }

        private async void OnExecuteCloseCommand()
        {
            try
            {

                var window = (MetroWindow)Application.Current.MainWindow;
                var dialogResult = await window.ShowMessageAsync("关闭系统"
                    , "确定要关闭么？" + System.Environment.NewLine + "确认关闭点击【是】，继续使用点击【否】。"
                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "关闭系统失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("关闭系统失败！请与管理员联系！", ex);
            }
        }

        private void OnExecuteSettingCommand()
        {
            try
            {
                ViewInfo viewInfo = new ViewInfo("系统设置", ViewType.Popup, "Solution.Desktop.View", "Solution.Desktop.View.SettingsView", "Solution.Desktop.ViewModel", "Solution.Desktop.ViewModel.SettingsViewModel", "pack://application:,,,/Solution.Desktop.Resource;component/Images/Settings2_32x32.png");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "打开设置页面失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);
            }
        }

        #endregion

        /// <summary>
        /// 页面导航跳转，生成新页面
        /// </summary>
        /// <param name="viewInfo">页面信息</param>
        private void navigate(ViewInfo viewInfo)
        {
            Application.Current.MainWindow.Cursor = Cursors.Wait;

            UserControlBase view = null;
            if (Equals(viewInfo, null))
            {
                Application.Current.Resources["UiMessage"] = "未找到页面信息，请确认MenuFunctionViewInfoMap.json配置正确！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
                return;
            }
            //已经打开则激活
            if (layoutDocumentPane.Children.Any(a => a.Title == viewInfo.DisplayName.ToString()))
            {
                var tmp = layoutDocumentPane.Children.FirstOrDefault(a => a.Title == viewInfo.DisplayName.ToString());
                tmp.IsActive = true;

                Application.Current.Resources["UiMessage"] = $"{viewInfo.DisplayName} 页面已经打开，设置激活成功！";
                Application.Current.MainWindow.Cursor = Cursors.Arrow;

                return;
            }

            try
            {
                view = System.Reflection.Assembly.Load(viewInfo.ViewAssemblyName)
                      .CreateInstance(viewInfo.ViewName) as UserControlBase;

            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "未找到页面：" + viewInfo.ViewName + ex.Message;
                LogHelper.Error(Application.Current.Resources["UiMessage"] + viewInfo.ToString(), ex);
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
                return;
            }

            if (Equals(view, null))
            {
                Application.Current.Resources["UiMessage"] = "未找到页面：" + viewInfo.ViewName;
                LogHelper.Info(Application.Current.Resources["UiMessage"] + viewInfo.ToString());
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
                return;
            }

            VmBase viewModel = null;
            try
            {
                viewModel = System.Reflection.Assembly.Load(viewInfo.ViewModelAssemblyName)
                       .CreateInstance(viewInfo.ViewModelName) as VmBase;
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "未找到ViewModel：" + viewInfo.ViewModelName;
                LogHelper.Error(Application.Current.Resources["UiMessage"] + viewInfo.ToString(), ex);
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
                return;
            }
            if (Equals(viewModel, null))
            {
                Application.Current.Resources["UiMessage"] = "未找到ViewModel：" + viewInfo.ViewModelName;
                LogHelper.Info(Application.Current.Resources["UiMessage"] + viewInfo.ToString());
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
                return;
            }
            //Application.Current.Resources["UiMessage"] = viewInfo.DisplayName;
            //LogHelper.Info(Application.Current.Resources["UiMessage"] + viewInfo.ToString());
            //view.Parameter = viewInfo.Parameter;
            //viewModel.Parameter = view.Parameter;
            viewModel.Parameter = viewInfo.Parameter;
            viewModel.MenuModule = viewInfo.MenuModule;
            view.DataContext = viewModel;
            //根据页面类型生成页面
            switch (viewInfo.ViewType)
            {
                case ViewType.PopupNoTitle: //没有标题栏的页面
                    MahApps.Metro.Controls.MetroWindow popupNoTitleWindows = new MahApps.Metro.Controls.MetroWindow();
                    popupNoTitleWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    popupNoTitleWindows.Style = Utility.Windows.ResourceHelper.FindResource(@"CleanWindowStyleKey") as Style;
                    popupNoTitleWindows.GlowBrush = Utility.Windows.ResourceHelper.FindResource(@"AccentColorBrush") as System.Windows.Media.Brush;
                    popupNoTitleWindows.Owner = Application.Current.MainWindow.IsInitialized == false ? null : Application.Current.MainWindow;
                    popupNoTitleWindows.ResizeMode = ResizeMode.NoResize;
                    popupNoTitleWindows.IsCloseButtonEnabled = false;
                    popupNoTitleWindows.ShowInTaskbar = popupNoTitleWindows.Owner == null ? true : false;
                    popupNoTitleWindows.MouseDown += PopupWindows_MouseDown;
                    popupNoTitleWindows.ShowCloseButton = false;
                    popupNoTitleWindows.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    popupNoTitleWindows.Width = view.Width;
                    popupNoTitleWindows.Height = view.Height;
                    popupNoTitleWindows.TitlebarHeight = 0;
                    //有限实用配置的图标，没有则使用默认的图标，没有则使用主窗体的
                    popupNoTitleWindows.Icon =
                        Utility.Windows.BitmapImageHelper.GetBitmapImage(viewInfo.Icon)
                        ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico")
                        ?? Application.Current.MainWindow.Icon;
                    popupNoTitleWindows.Focus();
                    popupNoTitleWindows.Content = view;
                    popupNoTitleWindows.Title = viewInfo.DisplayName;
                    popupViewStack.Push(popupNoTitleWindows);
                    popupNoTitleWindows.Closed += Window_Closed;
                    popupNoTitleWindows.ShowDialog();
                    if (!Equals(view, null))
                    {
                        view.Dispose();
                        view = null;
                        GC.Collect();
                    }
                    Application.Current.MainWindow.Cursor = Cursors.Arrow;
                    break;
                case ViewType.Popup://模式对话框
                    MahApps.Metro.Controls.MetroWindow popupWindows = new MahApps.Metro.Controls.MetroWindow();
                    popupWindows.TitlebarHeight = 25;
                    popupWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    popupWindows.Style = Utility.Windows.ResourceHelper.FindResource(@"CleanWindowStyleKey") as Style;
                    popupWindows.WindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                    popupWindows.NonActiveWindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                    popupWindows.Background = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsBackgroundColorBrush") as System.Windows.Media.Brush;
                    popupWindows.GlowBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsGlowBrush") as System.Windows.Media.Brush;
                    popupWindows.Owner = Application.Current.MainWindow.IsInitialized == false ? null : Application.Current.MainWindow;
                    popupWindows.ResizeMode = ResizeMode.NoResize;
                    popupWindows.IsCloseButtonEnabled = true;
                    popupWindows.ShowCloseButton = true;
                    popupWindows.Width = view.Width + popupWindows.BorderThickness.Left + popupWindows.BorderThickness.Right;
                    popupWindows.Height = view.Height + popupWindows.TitlebarHeight;
                    popupWindows.Title = viewInfo.DisplayName;
                    //有则使用配置的图标，没有则使用默认的图标，再没有则使用主窗体的图标
                    popupWindows.Icon =
                        Utility.Windows.BitmapImageHelper.GetBitmapImage(viewInfo.Icon)
                        ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico")
                        ?? Application.Current.MainWindow.Icon;
                    popupWindows.ShowInTaskbar = false;
                    popupWindows.Focus();
                    popupWindows.Content = view;
                    popupViewStack.Push(popupWindows);
                    popupWindows.Closed += Window_Closed;
                    popupWindows.ShowDialog();
                    if (!Equals(view, null))
                    {
                        view.Dispose();
                        view = null;
                        GC.Collect();
                    }
                    Application.Current.MainWindow.Cursor = Cursors.Arrow;
                    break;

                case ViewType.DockableView: //可停靠视图
                    LayoutDocument layoutDocument = new LayoutDocument();
                    layoutDocument.Title = viewInfo.DisplayName;
                    //有则使用配置的图标，没有则使用默认的图标，再没有则使用主窗体的图标
                    layoutDocument.IconSource =
                        Utility.Windows.BitmapImageHelper.GetBitmapImage(viewInfo.Icon)
                        ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico")
                        ?? Application.Current.MainWindow.Icon;
                    layoutDocument.Content = view;
                    layoutDocumentPane.Children.Add(layoutDocument);
                    layoutDocument.IsActive = true;
                    layoutDocument.CanClose = viewInfo.CanClose.HasValue ? (bool)viewInfo?.CanClose.Value : true;
                    layoutDocument.Closed += LayoutDocument_Closed;
                    Application.Current.MainWindow.Cursor = Cursors.Arrow;
                    break;

                case ViewType.SingleWindow://单个视图。主要为了显示帮助窗口
                    MahApps.Metro.Controls.MetroWindow singleWindows = new MahApps.Metro.Controls.MetroWindow();
                    singleWindows.TitlebarHeight = 25;
                    singleWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    singleWindows.Style = Utility.Windows.ResourceHelper.FindResource(@"CleanWindowStyleKey") as Style;
                    singleWindows.GlowBrush = Utility.Windows.ResourceHelper.FindResource(@"AccentColorBrush") as System.Windows.Media.Brush;
                    singleWindows.Content = view;
                    singleWindows.Width = view.Width + singleWindows.BorderThickness.Left + singleWindows.BorderThickness.Right;
                    singleWindows.Height = view.Height + singleWindows.TitlebarHeight;
                    singleWindows.Title = viewInfo.DisplayName;
                    //有则使用配置的图标，没有则使用默认的图标，再没有则使用主窗体的图标
                    singleWindows.Icon =
                        Utility.Windows.BitmapImageHelper.GetBitmapImage(viewInfo.Icon)
                        ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico")
                        ?? Application.Current.MainWindow.Icon;
                    singleWindows.Closed += Window_Closed;

                    singleWindows.Show();
                    singleWindows.Focus();
                    Application.Current.MainWindow.Cursor = Cursors.Arrow;
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //closePopView(true);
            ((sender as MetroWindow)?.Content as VmBase)?.Dispose();
        }

        private void LayoutDocument_Closed(object sender, EventArgs e)
        {
            ((sender as LayoutDocument)?.Content as UserControlBase).Dispose();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        private void closePopView(bool b)
        {
            if (popupViewStack.Count > 0)
            {
                MetroWindow popupWindow = popupViewStack.Pop() as MetroWindow;
                popupWindow.MouseDown -= PopupWindows_MouseDown;
                popupWindow.Close();
                popupWindow = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// 模式对话框的栈
        /// </summary>
        private Stack popupViewStack = new Stack();

        /// <summary>
        /// 鼠标按下拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupWindows_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var v = sender as MetroWindow;
                v.DragMove();
            }
            catch
            {

            }
        }

        #region 菜单数据相关
        /// <summary>
        /// 菜单数据，取决于用户权限
        /// </summary>
        public List<TreeViewItem> MenuItems { get; private set; }

        /// <summary>
        /// 初始化模块功能与界面的映射信息从配置文件加载
        /// </summary>
        /// <returns></returns>
        private void loadMenuFunctionViewInfoMaps()
        {
            try
            {
                GlobalData.MenuFunctionViewInfoMap = Utility.JsonHelper.FromJson<List<MenuFunctionViewInfoMap>>(System.IO.File.ReadAllText("MenuFunctionViewInfoMap.json"));
            }
            catch (Exception ex)
            {
                GlobalData.MenuFunctionViewInfoMap = new List<MenuFunctionViewInfoMap>();
                Application.Current.Resources["UiMessage"] = "MenuFunctionViewInfoMap.json配置文件初始化失败！" + ex.Message;
                Utility.LogHelper.Error(Application.Current.Resources["UiMessage"].ToString(), ex);

            }
        }

        /// <summary>
        /// 初始化菜单数据，debug模式使用自动生成的测试数据
        /// </summary>
        private void initMenuItems()
        {
            var tmpSolutionTreeViewItems = new List<TreeViewItem>();
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            tmpSolutionTreeViewItems = GetTreeViewItems(null, GlobalData.CurrentUserModule);
#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info(string.Format("生成树形菜单数据用时：{0}（毫秒）,菜单数量：", stopwatch.ElapsedMilliseconds, tmpSolutionTreeViewItems.Count()));
#endif
            MenuItems = tmpSolutionTreeViewItems;
        }

        /// <summary>
        /// 取得模块的树形数据源
        /// </summary>
        /// <param name="parentId">第一层默认null，数据库里面配置null</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<TreeViewItem> GetTreeViewItems(int? parentId, IEnumerable<MenuModule> nodes)
        {
            var mainNodes = nodes.Where(x => x.Parent_Id == parentId).ToList().OrderBy(a => a.OrderCode);
            List<TreeViewItem> mainTreeViewItems = new List<TreeViewItem>();
            foreach (var module in mainNodes)
            {
                TreeViewItem mainTreeViewItem = new TreeViewItem();
                mainTreeViewItem.IsExpanded = "1" == Utility.ConfigHelper.GetAppSetting("TreeMenuIsExpanded") ? true : false;
                mainTreeViewItem.Header = module.Name;
                mainTreeViewItem.ToolTip = module.Name;
                var viewInfo = GlobalData.MenuFunctionViewInfoMap?.FirstOrDefault(a => a.Controller == module.Functions.FirstOrDefault(b => b.Action == null)?.Controller)?.ViewInfo;
                //服务端没有设置Icon，那么使用本地的，如果本地没有配置，那么使用默认的
                if (string.IsNullOrWhiteSpace(module.Icon))
                {
                    if (Equals(viewInfo, null))
                    {
                        //使用本地默认
                        module.Icon = @"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(viewInfo.Icon))
                        {
                            //使用本地默认
                            module.Icon = @"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_16x16.ico";
                        }
                        else
                        {
                            module.Icon = viewInfo.Icon;
                        }
                    }
                }
                //设置显示的名称DisplayName优先使用服务器,如果服务器没有设置，则使用本地
                if (!Equals(viewInfo, null))
                {
                    if (string.IsNullOrWhiteSpace(module.Name))
                    {
                        module.Name = viewInfo.DisplayName;
                    }
                    else
                    {
                        if (!Equals(module.Name, viewInfo.DisplayName))
                        {
                            viewInfo.DisplayName = module.Name;
                        }
                    }
                }
                mainTreeViewItem.MouseUp += treeViewItem_MouseUp;
                mainTreeViewItem.MouseEnter += treeViewItem_MouseEnter;
                mainTreeViewItem.MouseLeave += treeViewItem_MouseLeave;

                mainTreeViewItem.Tag = module;
                if (mainTreeViewItem.Items.Count > 0)
                {
                    mainTreeViewItem.Focusable = false;
                }
                var st = new StackPanel() { Orientation = Orientation.Horizontal };
                st.Children.Add(Utility.Windows.BitmapImageHelper.GetImage(module.Icon));
                st.Children.Add(new TextBlock() { Text = module.Name, Margin = new Thickness(2, 0, 0, 0) });
                mainTreeViewItem.Header = st;
                mainTreeViewItems.Add(mainTreeViewItem);
            }
            List<MenuModule> otherNodes = nodes.Where(x => x.Parent_Id != parentId).OrderBy(a => a.OrderCode).ToList();
            foreach (var treeViewItem in mainTreeViewItems)
            {
                var items = GetTreeViewItems((treeViewItem.Tag as MenuModule).Id, otherNodes);
                foreach (var item in items)
                {
                    treeViewItem.Items.Add(item);
                }
                if (treeViewItem.HasItems)
                {
                    treeViewItem.Focusable = false;
                }
            }
            return mainTreeViewItems;
        }

        private void treeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var treeViewItem =
               Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Opacity = 0.7;
            }
        }
        private void treeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var treeViewItem =
               Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Opacity = 1;

            }
        }


        #endregion

        //        #region 顶部菜单控件,定义和初始化菜单

        //#if DEBUG

        //        /// <summary>
        //        /// top Content
        //        /// </summary>
        //        private Ribbon topContentControl;

        //        /// <summary>
        //        /// 顶端内容控件
        //        /// </summary>
        //        public Ribbon TopContentControl
        //        {
        //            get { return topContentControl; }
        //            set { Set(ref topContentControl, value); }
        //        }

        //        /// <summary>
        //        /// 初始化顶部菜单，内容多少由取决于<see cref="MenuItems"/>
        //        /// </summary>
        //        private void initTopContentControl()
        //        {
        //            TopContentControl = new Ribbon()
        //            {
        //                IsMinimized = true,
        //                AutomaticStateManagement = false,
        //                CanCustomizeQuickAccessToolBar = false,
        //            };
        //            foreach (var menuitem in this.MenuItems)
        //            {
        //                try
        //                {
        //                    //大tab
        //                    RibbonTabItem ribbonTabItem = new RibbonTabItem();
        //                    ribbonTabItem.Header = menuitem.Header;
        //                    TopContentControl.Tabs.Add(ribbonTabItem);

        //                    //每个tab
        //                    RibbonGroupBox ribbonGroupBox = new RibbonGroupBox();
        //                    ribbonTabItem.Groups.Add(ribbonGroupBox);
        //                    ribbonGroupBox.Header = menuitem.Header.ToString();

        //                    //tab里面的内容
        //                    foreach (var menuItemChild in menuitem.Items)
        //                    {
        //                        Fluent.Button button = new Fluent.Button();
        //                        TreeViewItem treeViewItem = menuItemChild as TreeViewItem;
        //                        var solutionTreeViewItemChild = treeViewItem.Tag as MenuModule;

        //                        button.Header = treeViewItem.Header;
        //                        button.ToolTip = treeViewItem.ToolTip;
        //                        MenuModule module = treeViewItem.Tag as MenuModule;
        //                        button.Command = this.MenuCommand;
        //                        button.CommandParameter = menuItemChild;
        //                        button.LargeIcon = module.Icon ?? @"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_32X32.ico";
        //                        ribbonGroupBox.Items.Add(button);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogHelper.Error("初始化顶部菜单时错误。", ex);
        //                }
        //            }
        //        }
        //#else

        //        /// <summary>
        //        /// top menu
        //        /// </summary>
        //        private Grid topContentControl = new Grid();

        //        /// <summary>
        //        /// 顶端菜单控件
        //        /// </summary>
        //        public Grid TopContentControl
        //        {
        //            get { return topContentControl; }
        //            set { Set(ref topContentControl, value); }
        //        }

        //        private void initTopContentControl()
        //        {
        //            TopContentControl.Height = 50;
        //            TopContentControl.Background = Utility.Windows.ResourceHelper.FindResource("HeaderColorBrush") as Brush;

        //            // TopContentControl.SetValue
        //            Menu menu = new Menu();
        //            menu.HorizontalAlignment = HorizontalAlignment.Right;
        //            menu.VerticalAlignment = VerticalAlignment.Bottom;

        //            menu.Items.Add(new System.Windows.Controls.MenuItem() { Header = "1234" });
        //            menu.Items.Add(new System.Windows.Controls.MenuItem() { Header = "234" });
        //            menu.Items.Add(new System.Windows.Controls.MenuItem() { Header = "234" });
        //            menu.Items.Add(new System.Windows.Controls.MenuItem() { Header = "234" });
        //            menu.Items.Add(new System.Windows.Controls.MenuItem() { Header = "234" });

        //            TopContentControl.Children.Add(menu);


        //        }
        //#endif
        //        #endregion

        /// <summary>
        /// 菜单点击命令
        /// </summary>
        private ICommand MenuCommand { get; set; }

        #region 主显示区域 包含树形菜单控件,定义和初始化菜单，菜单点击处理等

        #region  主显示区域控件
        private DockingManager mianContentControl;

        /// <summary>
        /// 主显示区域控件
        /// </summary>
        public DockingManager MianContentControl
        {
            get { return mianContentControl; }
            set { Set(ref mianContentControl, value); }
        }
        #endregion

        /// <summary>
        /// 初始化主要内容的容器控件
        /// </summary>
        private void initMianContentControl()
        {
            mianContentControl = new DockingManager();
            mianContentControl.BorderBrush = Utility.Windows.ResourceHelper.FindResource(@"AccentColorBrush") as System.Windows.Media.Brush;
            mianContentControl.Theme = new Xceed.Wpf.AvalonDock.Themes.MetroTheme();
            mianContentControl.DocumentClosed += MianContentControl_DocumentClosed;
            var v = Utility.Windows.ResourceHelper.FindResource(@"DockDocumentHeaderTemplate");
            DataTemplate dataTemplate = v as DataTemplate;

            mianContentControl.DocumentHeaderTemplate = dataTemplate;
            mianContentControl.Background = Utility.Windows.ResourceHelper.FindResource(@"DesktopBackground") as System.Windows.Media.ImageBrush;

            LayoutRoot layoutRoot = new LayoutRoot();
            mianContentControl.Layout = layoutRoot;

            LayoutAnchorSide layoutAnchorSide = new LayoutAnchorSide();

            layoutRoot.LeftSide = layoutAnchorSide;
            LayoutAnchorGroup layoutAnchorGroup = new LayoutAnchorGroup();

            layoutAnchorSide.Children.Add(layoutAnchorGroup);
            LayoutAnchorable layoutAnchorable = new LayoutAnchorable();
            layoutAnchorable.Title = Utility.Windows.ResourceHelper.FindResource(@"NavigationMenu").ToString();
            layoutAnchorable.IconSource = Utility.Windows.ResourceHelper.FindResource(@"Folder_32x32") as System.Windows.Media.ImageSource;

            layoutAnchorable.CanAutoHide = true;
            layoutAnchorable.CanClose = false;
            layoutAnchorable.CanFloat = false;
            layoutAnchorable.CanHide = false;
            layoutAnchorable.IsMaximized = false;
            layoutAnchorable.AutoHideMinWidth = (double)Utility.Windows.ResourceHelper.FindResource(@"LeftTreeAutoHideMinWidth");

            layoutAnchorGroup.Children.Add(layoutAnchorable);
            layoutAnchorable.ToggleAutoHide();
            TreeView treeView = new TreeView();
            layoutAnchorable.Content = treeView;
            treeView.ItemsSource = this.MenuItems;

            Style style = new Style(); ;
            style.TargetType = typeof(TreeViewItem);
            style.BasedOn = Utility.Windows.ResourceHelper.FindResource(@"MetroTreeViewItem") as Style;
            treeView.ItemContainerStyle = style;
            LayoutDocumentPaneGroup layoutDocumentPaneGroup = new LayoutDocumentPaneGroup();
            layoutRoot.RootPanel.Orientation = Orientation.Horizontal;
            layoutRoot.RootPanel.Children[1] = layoutDocumentPaneGroup;
            layoutDocumentPaneGroup.InsertChildAt(0, layoutDocumentPane);

        }

        /// <summary>
        /// tab页面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MianContentControl_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            Application.Current.Resources["UiMessage"] = e.Document.Title + " 已关闭！";
            Utility.LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        }

        /// <summary>
        /// 主文档tab页面容器,所有tab页面都由此对象管理
        /// </summary>
        private LayoutDocumentPane layoutDocumentPane = new LayoutDocumentPane();

        /// <summary>
        /// 菜单执行函数
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void OnExecuteMenuCommand(TreeViewItem treeViewItem)
        {
            Utility.LogHelper.Info("顶部选择菜单：" + treeViewItem.ToString());
            menuSelected(treeViewItem);
        }

        /// <summary>
        /// 菜单和树形点击处理函数
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void menuSelected(TreeViewItem treeViewItem)
        {
            //已经打开则激活
            if (layoutDocumentPane.Children.Any(a => a.Title == treeViewItem.Header.ToString()))
            {
                var tmp = layoutDocumentPane.Children.FirstOrDefault(a => a.Title == treeViewItem.Header.ToString());
                tmp.IsActive = true;
                Application.Current.Resources["UiMessage"] = $"{treeViewItem.Header.ToString()} 页面已经打开，设置激活成功！";
            }
            else
            {
                //没有打开则生成新的
                var menuModule = treeViewItem.Tag as MenuModule;
                if (Equals(menuModule, null))
                {
                    Application.Current.Resources["UiMessage"] = treeViewItem.Header + " 菜单模块没有对应的MenuModule！请确认配置正确。";
                    Utility.LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    return;
                }
                else
                {
                    var menuFunction = menuModule?.Functions.FirstOrDefault();
                    if (Equals(menuFunction, null))
                    {
                        Application.Current.Resources["UiMessage"] = menuModule?.Name + " 菜单功能没有对应的MenuFunction！请确认配置正确。";
                        Utility.LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                        return;
                    }
                    else
                    {
                        var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == menuFunction.Controller /*&& a.ViewInfo.DisplayName.Equals(treeViewItem.Header)*/);
                        if (!Equals(menuFunctionViewInfoMap, null) && !Equals(menuFunctionViewInfoMap.ViewInfo, null))
                        {
                            Application.Current.Resources["UiMessage"] = menuModule.Name + " 页面打开成功！";
                            menuFunctionViewInfoMap.ViewInfo.MenuModule = menuModule;
                            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap.ViewInfo, MessengerToken.Navigate);
                        }
                        else
                        {
                            Application.Current.Resources["UiMessage"] = menuModule?.Name + " 菜单功能没有对应的ViewInfo！请确认配置正确。";
                            Utility.LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 树形鼠标选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem =
              Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Focus();
                e.Handled = true;
                Utility.LogHelper.Info("树形选择菜单：" + treeViewItem.ToString());
                menuSelected(treeViewItem);
            }
        }

        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
            unRegisterMessenger();
        }

        protected override void Disposing()
        {
            base.Disposing();
            unRegisterMessenger();
            if (!Equals(CurrentLoginUser, null))
            {
                CurrentLoginUser = null;
            }
            if (!Equals(MenuItems, null))
            {
                MenuItems.Clear();
                MenuItems = null;
            }

            //if (!Equals(TopContentControl, null))
            //{
            //    TopContentControl = null;
            //}
            if (!Equals(layoutDocumentPane, null))
            {
                layoutDocumentPane.Children.ToList().ForEach((v) => { v.Close(); });
            }

            if (!Equals(MianContentControl, null))
            {

                MianContentControl = null;
            }
        }
    }
}
