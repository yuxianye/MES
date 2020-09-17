using Solution.Desktop.Core;

namespace Solution.Desktop.EntEmployeeInfo.View
{
    /// <summary>
    /// EntEmployeeInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EntEmployeeInfoAddView : UserControlBase
    {
        public EntEmployeeInfoAddView()
        {
            InitializeComponent();
            //cmb_enterprise.Focus();
        }

        private void EntEmployeeInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
