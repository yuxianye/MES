using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Linq;
namespace Solution.Desktop.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Mutex used to allow only one instance.
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// Name of mutex to use. Should be unique for all applications.
        /// </summary>
        public const string MutexName = "Solution.Desktop.App";

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hWnd">Window handle to bring to front.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// AppPath(包含末尾的斜线)，应用程序的exe路径
        /// </summary>
        public static readonly string AppPath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1);

        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            Uri uri = null;
#if DEBUG
            uri = new Uri(@"pack://application:,,,/Solution.Desktop.Resource;component/DefaultResources.xaml", UriKind.RelativeOrAbsolute);
#else
            //uri = new Uri(AppPath + @"DefaultResources.xaml", UriKind.RelativeOrAbsolute);
            uri = new Uri(@"pack://application:,,,/Solution.Desktop.Resource;component/DefaultResources.xaml", UriKind.RelativeOrAbsolute);

#endif
            ResourceDictionary res = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Add(res);

            // Try to grab mutex
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);


            if (!createdNew)
            {


                // Bring other instance to front and exit.
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Application.Current.Shutdown();
            }
            else
            {
                // Add Event handler to exit event.
                Exit += CloseMutexHandler;
            }
        }

        /// <summary>
        /// Handler that closes the mutex.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void CloseMutexHandler(object sender, EventArgs e)
        {
            _mutex?.Close();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (e.ApplicationExitCode == 1)
            {
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            }

        }

    }
}
