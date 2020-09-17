using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;
using System.Windows;

namespace Solution.Desktop.MaterialOutStorageInfo.View
{
    /// <summary>
    /// WareHouseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialOutStorageInfoEditView2 : UserControlBase
    {
        public MaterialOutStorageInfoEditView2()
        {
            InitializeComponent();
            OutStorageBillCode.Focus();
        }

        private void OutStorageType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OutStorageType.SelectedIndex == (int)OutStorageTypeEnumModel.OutStorageType.原料自动出库 - 1 ||
                 OutStorageType.SelectedIndex == (int)OutStorageTypeEnumModel.OutStorageType.原料自动出库演示 - 1)
            {
                //MessageBox.Show(string.Format("请选择\"成品手动出库\"或\"空托盘出库\"" + System.Environment.NewLine), "出库类型", MessageBoxButton.OK);
                //
                OutStorageType.SelectedIndex = (int)OutStorageTypeEnumModel.OutStorageType.空托盘出库 - 1;
            }
            //
            //
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
            }
            else if (OutStorageType.SelectedIndex == (int)OutStorageTypeEnumModel.OutStorageType.成品手动出库 - 1)
            {
                //MaterialID.IsEnabled = true;
                //Quantity.IsEnabled = true;
                ////PalletQuantity.IsEnabled = true;
                ////
                //PalletID.IsEnabled = false;
                //PalletID.SelectedItem = null;
            }
        }

        private void Quantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Quantity_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 500);
        }
    }
}
