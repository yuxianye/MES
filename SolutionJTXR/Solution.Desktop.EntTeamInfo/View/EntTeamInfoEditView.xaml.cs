using Solution.Desktop.Core;

namespace Solution.Desktop.EntTeamInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class EntTeamInfoEditView : UserControlBase
    {
        public EntTeamInfoEditView()
        {
            InitializeComponent();
            teamCode.Focus();
        }
    }
}
