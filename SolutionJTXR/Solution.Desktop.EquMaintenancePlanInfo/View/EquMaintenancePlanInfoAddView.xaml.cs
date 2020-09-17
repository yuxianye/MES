using Solution.Desktop.Core;
namespace Solution.Desktop.EquMaintenancePlanInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EquMaintenancePlanInfoAddView : UserControlBase
    {
        public EquMaintenancePlanInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
        private void EquipmentTypeInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
         
        }
    }
}
