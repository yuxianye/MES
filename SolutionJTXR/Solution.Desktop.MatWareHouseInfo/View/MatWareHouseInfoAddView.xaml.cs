using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using System;
using System.Windows;

namespace Solution.Desktop.MatWareHouseInfo.View
{
    /// <summary>
    /// WareHouseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatWareHouseInfoAddView : UserControlBase
    {
        public MatWareHouseInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //ViewInfo viewInfo = new ViewInfo("修改密码",
                //                                 ViewType.Popup,
                //                                 "Solution.Desktop.View",
                //                                 "Solution.Desktop.View.ModifyPasswordView",
                //                                 "Solution.Desktop.ViewModel",
                //                                 "Solution.Desktop.ViewModel.ModifyPasswordViewModel",
                //                                 "pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png");
                //Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);

                //viewInfo.ViewAssemblyName = Solution.Desktop.MatWareHouseInfo
                //viewInfo.ViewName = Solution.Desktop.MatWareHouseInfo.View.MatWareHouseInfoView
                //viewInfo.ViewModelAssemblyName = Solution.Desktop.MatWareHouseInfo
                //viewInfo.ViewModelName = Solution.Desktop.MatWareHouseInfo.ViewModel.MatWareHouseInfoViewModel

                ViewInfo viewInfo2 = new ViewInfo("测试",
                                                    ViewType.Popup,
                                                    "Solution.Desktop.MatWareHouseInfo",
                                                    "Solution.Desktop.MatWareHouseInfo.UserControls.MatWareHouseInfoViewNew",
                                                    "Solution.Desktop.MatWareHouseInfo",
                                                    "Solution.Desktop.MatWareHouseInfo.UserControls.MatWareHouseInfoViewModelNew",
                                                    "pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png",
                                                    "aaaaa");
                Messenger.Default.Send<ViewInfo>(viewInfo2, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "打开设置页面失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);
            }          
        }
    }
}

