using Solution.Desktop.Core;
using System.Windows.Input;
using Solution.Desktop.Model;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// 安全可控的vm基类
    /// </summary>
    public abstract class ControllableViewModelBase : VmBase
    {
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin
        {
            get { return GlobalData.CurrentLoginUser.IsAdmin; }
        }

        //private System.Windows.Visibility adminControlVisibility = System.Windows.Visibility.Collapsed;

        //public System.Windows.Visibility AdminControlVisibility
        //{

        //    get
        //    {
        //        if (GlobalData.CurrentLoginUser.IsAdmin)
        //        {
        //            adminControlVisibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            adminControlVisibility = System.Windows.Visibility.Collapsed;
        //        }
        //        return adminControlVisibility;

        //    }
        //    set
        //    {
        //        Set(ref adminControlVisibility, value);
        //    }


        //}












    }
}
