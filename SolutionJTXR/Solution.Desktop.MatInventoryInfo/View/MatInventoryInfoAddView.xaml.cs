using Solution.Desktop.Core;

namespace Solution.Desktop.MatInventoryInfo.View
{
    /// <summary>
    /// MatInventoryInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatInventoryInfoAddView : UserControlBase
    {
        public MatInventoryInfoAddView()
        {
            InitializeComponent();
            MatWareHouse_Id.Focus();
        }
    }
}
