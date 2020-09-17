using Solution.Desktop.Core;
using System.Windows;

namespace Solution.Desktop.View
{
    /// <summary>
    ///主页面，尽量不要在codebehind里面写逻辑
    /// </summary>
    public partial class MainView : UserControlBase
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void UserControlBase_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Application.Current.MainWindow.DragMove();
            }
            catch
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}