using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// vm容器
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }
        /// <summary>
        /// 主窗体vm
        /// </summary>
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        /// <summary>
        /// 主vm
        /// </summary>
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}