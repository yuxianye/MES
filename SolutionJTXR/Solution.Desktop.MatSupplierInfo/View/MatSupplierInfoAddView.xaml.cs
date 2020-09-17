using Solution.Desktop.Core;

namespace Solution.Desktop.MatSupplierInfo.View
{
    /// <summary>
    /// WareHouseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatSupplierInfoAddView : UserControlBase
    {
        public MatSupplierInfoAddView()
        {
            InitializeComponent();
            SupplierCode.Focus();
        }
    }
}
