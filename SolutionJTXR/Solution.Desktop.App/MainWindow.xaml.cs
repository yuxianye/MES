using MahApps.Metro.Controls;
using System.Windows;

namespace Solution.Desktop.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SetTitle();
        }
        private const string PART_WindowButtonCommands = "PART_WindowButtonCommands";

        public void SetTitle()
        {
            //Icon = "pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_64X64.ico"
            Title = Application.Current.TryFindResource(@"AppName")
                     + @" "
                     + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            //Application.Current.MainWindow.Title = Utility.Windows.ResourceHelper.LoadString(@"AppName")
            //    + " "
            //    + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major
            //    + "."
            //    + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;

            //Application.Current.MainWindow.Title = Utility.Windows.ResourceHelper.LoadString("AppName")
            //+ " "
            //+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major
            //+ "."
            //+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;

            WindowButtonCommands = GetTemplateChild(PART_WindowButtonCommands) as WindowButtonCommands;

            //if (WindowButtonCommands != null)
            //{
            //    WindowButtonCommands.ParentWindow = this;
            //}
            //titleBar = GetTemplateChild(PART_TitleBar) as UIElement;
            //titlebar
        }

        private void MetroWindow_Closed(object sender, System.EventArgs e)
        {
            mainView.Dispose();
            Application.Current.Resources.Clear();
            Application.Current.Resources = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.WindowButtonCommands.Margin = new Thickness(0, 4, 0, 0);
            this.WindowButtonCommands.MinHeight = 25;
            this.WindowButtonCommands.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
