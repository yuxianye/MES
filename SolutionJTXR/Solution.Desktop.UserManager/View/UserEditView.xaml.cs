using Solution.Desktop.Core;
using System.Windows.Controls;

namespace Solution.Desktop.UserManager.View
{
    /// <summary>
    /// CommSocketServerEditView.xaml 的交互逻辑
    /// </summary>
    public partial class UserEditView : UserControlBase
    {
        public UserEditView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
    }
}
