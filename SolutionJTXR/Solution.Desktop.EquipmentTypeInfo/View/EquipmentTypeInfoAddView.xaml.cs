using Solution.Desktop.Core;
using Solution.Desktop.EquipmentTypeInfo.Model;
namespace Solution.Desktop.EquipmentTypeInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EquipmentTypeInfoAddView : UserControlBase
    {
        public EquipmentTypeInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
        private void EquipmentTypeInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
         
        }
    }
}
