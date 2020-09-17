using Solution.Desktop.Core;

namespace Solution.Desktop.EntWorkAttendanceRecordInfo.View
{
    /// <summary>
    /// EntEmployeeInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EntWorkAttendanceRecordInfoAddView : UserControlBase
    {
        public EntWorkAttendanceRecordInfoAddView()
        {
            InitializeComponent();
            //cmb_enterprise.Focus();
        }

        private void EntWorkAttendanceRecordInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
