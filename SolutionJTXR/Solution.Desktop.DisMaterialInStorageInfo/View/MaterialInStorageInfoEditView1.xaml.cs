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
    }
}
