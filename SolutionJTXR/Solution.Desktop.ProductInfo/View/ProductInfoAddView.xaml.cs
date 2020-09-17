using Solution.Desktop.Core;

namespace Solution.Desktop.ProductInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductInfoAddView : UserControlBase
    {
        public ProductInfoAddView()
        {
            InitializeComponent();
            productCode.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
