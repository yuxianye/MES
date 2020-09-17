using Solution.Desktop.Core;
using Solution.Desktop.EquipmentBrandInfo.Model;
namespace Solution.Desktop.EquipmentBrandInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EquipmentBrandInfoAddView : UserControlBase
    {
        public EquipmentBrandInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
        private void EquipmentBrandInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
         
        }
    }
}
