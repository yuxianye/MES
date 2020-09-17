using Solution.Desktop.Core;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// 可分页的vm基类
    /// </summary>
    public abstract class PageableViewModelBase : VmBase /*ControllableViewModelBase*/
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageableViewModelBase()
        {
            PageChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<PageChangedEventArgs>(OnExecutePageChangedCommand);
        }

        /// <summary>
        /// 翻页命令,当前页数变化时执行此命令
        /// </summary>
        public ICommand PageChangedCommand { get; private set; }

        #region 当前浏览页面的页码

        private int totalCounts = 0;
        /// <summary>
        /// 所有记录的个数
        /// </summary>
        public int TotalCounts
        {
            get { return totalCounts; }
            set { Set(ref totalCounts, value); }
        }
        #endregion

        /// <summary>
        /// 分页改变时重写此方法，然后根据分页参数从服务端取得数据
        /// </summary>
        public abstract void OnExecutePageChangedCommand(PageChangedEventArgs e);

        #region 数据列表的右键快捷菜单

        public List<MenuItem> menuItems;

        /// <summary>
        /// 右键菜单
        /// </summary>
        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                Set(ref menuItems, value);
            }
        }
        #endregion

    }
}
