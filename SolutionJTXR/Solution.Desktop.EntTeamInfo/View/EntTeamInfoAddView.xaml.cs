using Solution.Desktop.Core;

namespace Solution.Desktop.EntTeamInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EntTeamInfoAddView : UserControlBase
    {
        public EntTeamInfoAddView()
        {
            InitializeComponent();
            teamCode.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
