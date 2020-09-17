using Solution.Desktop.Core;

namespace Solution.Desktop.EntAreaInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class EntAreaInfoEditView : UserControlBase
    {
        public EntAreaInfoEditView()
        {
            InitializeComponent();
            cmb_enterprise.Focus();
        }
    }
}
