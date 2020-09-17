using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Windows.Controls;

namespace Solution.Desktop.MaterialOutStorageInfo.View
{
    /// <summary>
    /// MaterialOutStorageTaskInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialOutStorageTaskInfoEditView : UserControlBase
    {
        public MaterialOutStorageTaskInfoEditView()
        {
            InitializeComponent();
            OutStorageBillCode.Focus();
        }

        private void OutStorageType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OutStorageType.SelectedIndex == (int)OutStorageTypeEnumModel.OutStorageType.空托盘出库 - 1)
            {
                //MaterialID.IsEnabled = false;
                //MaterialID.SelectedItem = null;
                ////
                //Quantity.IsEnabled = false;
                //Quantity.Text = "";
                ////
                ////PalletQuantity.IsEnabled = false;
                ////PalletQuantity.Text = "";
                ////
                //PalletID.IsEnabled = true;
                //
                MaterialID.Visibility = System.Windows.Visibility.Collapsed;
                Quantity.Visibility = System.Windows.Visibility.Collapsed;
                PalletQuantity.Visibility = System.Windows.Visibility.Collapsed;
                //PalletID.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (OutStorageType.SelectedIndex == (int)OutStorageTypeEnumModel.OutStorageType.成品手动出库 - 1)
            {
                //MaterialID.Visibility = System.Windows.Visibility.Collapsed;
                //Quantity.Visibility = System.Windows.Visibility.Collapsed;
                //PalletQuantity.Visibility = System.Windows.Visibility.Collapsed;
                PalletID.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
