using Solution.Desktop.Core;
using System.Windows.Controls;

namespace Solution.Desktop.ModuleManager.View
{
    /// <summary>
    /// CommSocketServerEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ModuleManagerEditView : UserControlBase
    {
        public ModuleManagerEditView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
    }
}
