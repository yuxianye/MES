using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// vm����
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }
        /// <summary>
        /// ������vm
        /// </summary>
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        /// <summary>
        /// ��vm
        /// </summary>
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}