using Solution.Desktop.Core;

namespace Solution.Desktop.ProductTypeInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductTypeInfoAddView : UserControlBase
    {
        public ProductTypeInfoAddView()
        {
            InitializeComponent();
            prouctTypeCode.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
