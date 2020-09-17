using Solution.Desktop.Core;

namespace Solution.Desktop.MatWareHouseLocationInfo.View
{
    /// <summary>
    /// WareHouseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatWareHouseLocationInfoAddView : UserControlBase
    {
        public MatWareHouseLocationInfoAddView()
        {
            InitializeComponent();
            MatWareHouse_Id.Focus();
        }
    }
}
