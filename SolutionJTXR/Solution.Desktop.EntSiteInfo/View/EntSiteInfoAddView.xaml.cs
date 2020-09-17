using Solution.Desktop.Core;

namespace Solution.Desktop.EntSiteInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EntSiteInfoAddView : UserControlBase
    {
        public EntSiteInfoAddView()
        {
            InitializeComponent();
            cmb_enterprise.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
