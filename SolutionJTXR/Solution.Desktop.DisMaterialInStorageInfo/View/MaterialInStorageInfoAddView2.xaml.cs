using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;
using System.Windows;

namespace Solution.Desktop.MaterialInStorageInfo.View
{
    /// <summary>
    /// MaterialInStorageInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInStorageInfoAddView2 : UserControlBase
    {
        public MaterialInStorageInfoAddView2()
        {
            InitializeComponent();
            InStorageBillCode.Focus();
            //
            //InStorageType.SelectedIndex = (int)InStorageTypeEnumModel.InStorageType.原料手动入库 - 1;
        }

        private void InStorageType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ( InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.原料自动入库 - 1 ||
                 InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.成品自动入库 - 1 )
            {
                //MessageBox.Show(string.Format("请选择\"原料手动入库\"或\"空托盘入库\"" + System.Environment.NewLine), "入库类型", MessageBoxButton.OK);
                //
                InStorageType.SelectedIndex = (int)InStorageTypeEnumModel.InStorageType.空托盘入库 - 1 ;
            }
            //
            //
            if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.空托盘入库 - 1)
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
            else if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.原料手动入库 - 1)
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
