using Solution.Desktop.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.View.UserControls
{
    /// <summary>
    /// 数据分页控件
    /// </summary>
    public partial class DataPager : UserControlBase, INotifyPropertyChanged
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataPager()
        {
            inti();
            InitializeComponent();
        }

        /// <summary>
        /// 默认每页数量
        /// </summary>
        private static int defaultPageSize = 200;

        /// <summary>
        /// 初始化
        /// </summary>
        private void inti()
        {

#if  RELEASE  //调试时不执行
            int.TryParse(Utility.ConfigHelper.GetAppSetting("PageSize"), out defaultPageSize);
#endif
            FirstPageCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteFirstPageCommand, OnCanExecuteFirstPageCommand);
            PreviousPageCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePreviousPageCommand, OnCanExecutePreviousPageCommand);
            NextPageCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteNextPageCommand, OnCanExecuteNextPageCommand);
            LastPageCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLastPageCommand, OnCanExecuteLastPageCommand);
            LastPageCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLastPageCommand, OnCanExecuteLastPageCommand);
        }

        #region 总计记录数量
        /// <summary>
        /// 总计记录数量
        /// </summary>
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        /// <summary>
        /// 总计记录数量
        /// </summary>
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(int), typeof(DataPager), new UIPropertyMetadata(0, (sender, e) =>
            {
                var dp = sender as DataPager;
                if (dp == null) return;
                if (Equals(e.NewValue, e.OldValue)) return;
                dp.InitData();
            }));

        #endregion

        #region 每页数量列表数据源
        /// <summary>
        /// 每页数量列表数据源
        /// </summary>
        public ObservableCollection<int> PageSizeItems
        {
            get { return (ObservableCollection<int>)GetValue(PageSizeItemsProperty); }
            private set { SetValue(PageSizeItemsProperty, value); }
        }

        /// <summary>
        /// 页大小项的列表数据源
        /// </summary>
        public static readonly DependencyProperty PageSizeItemsProperty =
            DependencyProperty.Register("PageSizeItems", typeof(ObservableCollection<int>), typeof(DataPager),
                new UIPropertyMetadata(new ObservableCollection<int>() { 20, 50, 100, 200, 500, 1000, 5000, 10000 }, (sender, e) =>
                    {
                        var dp = sender as DataPager;
                        if (dp == null) return;
                        if (Equals(e.NewValue, e.OldValue)) return;
                    }));
        #endregion

        #region 选择的每页数量

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new UIPropertyMetadata(defaultPageSize, (sender, e) =>
            {
                var dp = sender as DataPager;
                if (dp == null) return;
                if (Equals(e.NewValue, e.OldValue)) return;
                dp.InitData();
                dp.OnPageChanging(dp.PageIndex);
            }));

        #endregion

        #region 页列表数据源
        /// <summary>
        /// 页列表数据源
        /// </summary>
        public ObservableCollection<int> PageIndexItems
        {
            get { return (ObservableCollection<int>)GetValue(PageIndexItemsProperty); }
            private set { SetValue(PageIndexItemsProperty, value); }
        }

        /// <summary>
        /// 页的列表数据源
        /// </summary>
        public static readonly DependencyProperty PageIndexItemsProperty =
            DependencyProperty.Register("PageIndexItems", typeof(ObservableCollection<int>), typeof(DataPager),
                new UIPropertyMetadata(new ObservableCollection<int>()));

        #endregion

        #region 当前页
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            private set { SetValue(PageIndexProperty, value); }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new UIPropertyMetadata(1, (sender, e) =>
            {
                var dp = sender as DataPager;
                if (dp == null) return;
                if (Equals(e.NewValue, e.OldValue)) return;
                dp.OnPageChanging((int)e.NewValue);
            }));
        #endregion

        #region 总页数

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get { return (int)GetValue(TotalPageCountProperty); }
            private set { SetValue(TotalPageCountProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public static readonly DependencyProperty TotalPageCountProperty =
            DependencyProperty.Register("TotalPageCount", typeof(int), typeof(DataPager), new UIPropertyMetadata(1));
        #endregion

        #region 首页命令
        /// <summary>
        /// 首页命令
        /// </summary>
        public ICommand FirstPageCommand { get; set; }

        /// <summary>
        /// 首页命令执行函数
        /// </summary>
        private void OnExecuteFirstPageCommand()
        {
            OnPageChanging(1);
        }

        /// <summary>
        /// 首页命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteFirstPageCommand()
        {
            if (this.PageIndex == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 上一页命令
        /// <summary>
        /// 上一页命令
        /// </summary>
        public ICommand PreviousPageCommand { get; set; }

        /// <summary>
        /// 上一页命令执行函数
        /// </summary>
        private void OnExecutePreviousPageCommand()
        {
            OnPageChanging(this.PageIndex - 1);
        }

        /// <summary>
        /// 上一页命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePreviousPageCommand()
        {
            if (this.PageIndex == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 下一页命令
        /// <summary>
        /// 下一页命令
        /// </summary>
        public ICommand NextPageCommand { get; set; }

        /// <summary>
        /// 下一页命令执行函数
        /// </summary>
        private void OnExecuteNextPageCommand()
        {
            OnPageChanging(this.PageIndex + 1);
        }

        /// <summary>
        /// 下一页命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteNextPageCommand()
        {
            if (this.PageIndex == TotalPageCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 末页命令
        /// <summary>
        /// 下一页命令
        /// </summary>
        public ICommand LastPageCommand { get; set; }

        /// <summary>
        /// 末页命令执行函数
        /// </summary>
        private void OnExecuteLastPageCommand()
        {
            OnPageChanging(this.TotalPageCount);
        }

        /// <summary>
        /// 末页命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteLastPageCommand()
        {
            if (this.PageIndex == TotalPageCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        /// <summary>
        /// 页码更改
        /// </summary>
        /// <param name="pageIndex"></param>
        internal void OnPageChanging(int pageIndex)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > this.TotalPageCount) pageIndex = this.TotalPageCount;

            this.PageIndex = pageIndex;
            if (PageChangedEventArgs.PageIndex == pageIndex && PageChangedEventArgs.PageSize == PageSize)
            {
                return;
            }
            PageChangedEventArgs = new PageChangedEventArgs() { PageSize = this.PageSize, PageIndex = pageIndex };
            PageChanged?.Invoke(this, PageChangedEventArgs);
        }

        #region 页变更事件参数

        /// <summary>
        /// 页变更事件参数
        /// </summary>
        private PageChangedEventArgs pageChangedEventArgs = new PageChangedEventArgs();

        /// <summary>
        /// 页变更事件参数
        /// </summary>
        public PageChangedEventArgs PageChangedEventArgs
        {
            get
            {
                return pageChangedEventArgs;
            }
            set
            {
                pageChangedEventArgs = value;
                NotifyPropertyChanged("PageChangedEventArgs");
            }
        }
        //{
        //    get { return (PageChangedEventArgs)GetValue(PageChangedEventArgsProperty); }
        //    set { SetValue(PageChangedEventArgsProperty, value); }
        //}

        ///// <summary>
        ///// 页变更事件参数
        ///// </summary>
        //public static readonly DependencyProperty PageChangedEventArgsProperty =
        //    DependencyProperty.Register("PageChangedEventArgs", typeof(PageChangedEventArgs), typeof(DataPager), new UIPropertyMetadata(new PageChangedEventArgs()));

        #endregion

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            if (this.TotalCount == 0)
            {
                this.TotalPageCount = 1;
            }
            else
            {
                this.TotalPageCount = this.TotalCount % this.PageSize > 0 ? (this.TotalCount / this.PageSize) + 1 : this.TotalCount / this.PageSize;
            }
            //设置页list
            if (!Equals(this.PageIndexItems.Count, TotalPageCount))
            {
                if (this.PageIndexItems.Count < TotalPageCount)
                {//增加
                    for (int i = 1; i <= TotalPageCount; i++)
                    {
                        if (!this.PageIndexItems.Contains(i))
                        {
                            this.PageIndexItems.Insert(i - 1, i);
                        }
                    }
                }
                else
                {//移除
                    //this.PageIndexItems.Remove ()
                    for (int i = PageIndexItems.Count; i > TotalPageCount; i--)
                    {
                        if (this.PageIndexItems.Contains(i))
                        {
                            this.PageIndexItems.Remove(i);
                        }
                    }
                }
            }

            if (this.PageIndex < 1)
            {
                this.PageIndex = 1;
            }
            if (this.PageIndex > this.TotalPageCount)
            {
                this.PageIndex = this.TotalPageCount;
            }
            if (this.PageSize < 1)
            {
                this.PageSize = 50;
            }
            //PageIndex = PageIndex;
        }

        /// <summary>
        /// 分页后处理的事件
        /// </summary>
        public event EventHandler<PageChangedEventArgs> PageChanged;

        #region INotifyPropertyChanged成员
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected override void Disposing()
        {
            throw new NotImplementedException();
        }
        #endregion


    }



}
