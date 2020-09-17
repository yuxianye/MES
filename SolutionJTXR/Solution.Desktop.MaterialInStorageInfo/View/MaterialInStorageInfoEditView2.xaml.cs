using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;

namespace Solution.Desktop.MaterialInStorageInfo.View
{
    /// <summary>
    /// MaterialInStorageInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInStorageInfoEditView2 : UserControlBase
    {
        public MaterialInStorageInfoEditView2()
        {
            InitializeComponent();
            InStorageBillCode.Focus();
        }

        private void InStorageType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //空托盘入库
            if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType - 1)
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
            //原料手动入库
            else if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType - 1)
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
