using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Windows.Controls;

namespace Solution.Desktop.MaterialInStorageInfo.View
{
    /// <summary>
    /// MaterialInStorageTaskInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInStorageTaskInfoEditView : UserControlBase
    {
        public MaterialInStorageTaskInfoEditView()
        {
            InitializeComponent();
            InStorageBillCode.Focus();
        }

        private void InStorageType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
            //空托盘入库
            if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType - 1)
            {
                MaterialID.Visibility = System.Windows.Visibility.Collapsed;
                Quantity.Visibility = System.Windows.Visibility.Collapsed;
                PalletQuantity.Visibility = System.Windows.Visibility.Collapsed;
                //PalletID.Visibility = System.Windows.Visibility.Collapsed;
            }
            //原料手动入库
            else if (InStorageType.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType - 1)
            {
                //MaterialID.Visibility = System.Windows.Visibility.Collapsed;
                //Quantity.Visibility = System.Windows.Visibility.Collapsed;
                //PalletQuantity.Visibility = System.Windows.Visibility.Collapsed;
                PalletID.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
