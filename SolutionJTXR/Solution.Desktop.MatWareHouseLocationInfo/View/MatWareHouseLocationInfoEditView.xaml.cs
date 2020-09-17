using Solution.Desktop.Core;

namespace Solution.Desktop.MatWareHouseLocationInfo.View
{
    /// <summary>
    /// WareHouseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MatWareHouseLocationInfoEditView : UserControlBase
    {
        public MatWareHouseLocationInfoEditView()
        {
            InitializeComponent();
            MatWareHouse_Id.Focus();
        }

        private void IsUse_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!PalletCode.Text.Equals("") &&
                IsUse.IsChecked.Value)
            {
                IsUse.IsEnabled = false;
            }
        }

        private void PalletCode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!PalletCode.Text.Equals("") &&
               IsUse.IsChecked.Value)
            {
                IsUse.IsEnabled = false;
            }
        }
    }
}
