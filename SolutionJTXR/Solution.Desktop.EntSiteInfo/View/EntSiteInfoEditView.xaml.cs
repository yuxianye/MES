using Solution.Desktop.Core;

namespace Solution.Desktop.EntSiteInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class EntSiteInfoEditView : UserControlBase
    {
        public EntSiteInfoEditView()
        {
            InitializeComponent();
            cmb_enterprise.Focus();
        }
    }
}
