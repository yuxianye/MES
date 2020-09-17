using Solution.Desktop.Core;

namespace Solution.Desktop.EntAreaInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EntAreaInfoAddView : UserControlBase
    {
        public EntAreaInfoAddView()
        {
            InitializeComponent();
            cmb_enterprise.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
