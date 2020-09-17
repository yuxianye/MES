using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Windows;

namespace Solution.Desktop.MaterialInStorageInfo.View
{
    /// <summary>
    /// MaterialInStorageInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInStorageInfoAuditView2 : UserControlBase
    {
        public MaterialInStorageInfoAuditView2()
        {
            InitializeComponent();
            InStorageBillCode.Focus();
        }

        private void AuditStatus_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //空托盘入库
            if (AuditStatus.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType - 1)
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
            else if (AuditStatus.SelectedIndex == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType - 1)
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
