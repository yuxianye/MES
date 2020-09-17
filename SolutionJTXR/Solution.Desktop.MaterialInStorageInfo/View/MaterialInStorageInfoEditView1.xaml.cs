using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;

namespace Solution.Desktop.MaterialInStorageInfo.View
{
    /// <summary>
    /// MaterialInStorageInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInStorageInfoEditView1 : UserControlBase
    {
        public MaterialInStorageInfoEditView1()
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
    }
}
