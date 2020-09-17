using Solution.Desktop.Core;

namespace Solution.Desktop.ProductInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductInfoEditView : UserControlBase
    {
        public ProductInfoEditView()
        {
            InitializeComponent();
            productCode.Focus();
        }
    }
}
