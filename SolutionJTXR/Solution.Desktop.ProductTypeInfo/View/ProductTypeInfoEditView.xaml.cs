using Solution.Desktop.Core;

namespace Solution.Desktop.ProductTypeInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductTypeInfoEditView : UserControlBase
    {
        public ProductTypeInfoEditView()
        {
            InitializeComponent();
            prouctTypeCode.Focus();
        }
    }
}
