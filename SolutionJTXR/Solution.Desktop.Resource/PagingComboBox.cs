using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Solution.Desktop.Resource
{
    /// <summary>
    /// 自定义带分页ComboBox
    /// </summary>
    [TemplatePart(Name = "FirstPageBtn", Type = typeof(Button))]
    [TemplatePart(Name = "LastPageBtn", Type = typeof(Button))]
    [TemplatePart(Name = "NextPageBtn", Type = typeof(Button))]
    [TemplatePart(Name = "FinalPageBtn", Type = typeof(Button))]
    [TemplatePart(Name = "SearchPageBtn", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "ContentTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PageCount", Type = typeof(TextBox))]
    [TemplatePart(Name = "PageSize", Type = typeof(ComboBox))]
    public partial class PagingComboBox : ComboBox
    {
        public PagingComboBox()
        {
            this.FirstPageClickButton.Click += this.OnFirstPageBtnClick;
            this.LastPageClickButton.Click += this.OnLastPageBtnClick;
            this.NextPageClickButton.Click += this.OnNextPageBtnClick;
            this.FinalPageClickButton.Click += this.OnFinalPageBtnClick;
            this.SearchPageClickButton.Click += this.OnSearchBtnClick;
            this.PageSizeComboBox.SelectionChanged += this.OnPageSizeSelectedChanged;
        }

        #region 初始化
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.FirstPageClickButton = this.EnforceInstance<Button>("FirstPageBtn");
            this.LastPageClickButton = this.EnforceInstance<Button>("LastPageBtn");
            this.NextPageClickButton = this.EnforceInstance<Button>("NextPageBtn");
            this.FinalPageClickButton = this.EnforceInstance<Button>("FinalPageBtn");
            this.SearchPageClickButton = this.EnforceInstance<ToggleButton>("SearchPageBtn");
            this.ContentTextBox = this.EnforceInstance<TextBox>("ContentTextBox");
            this.PageCountTextBox = Template.FindName("PageCount", this) as TextBox;
            this.PageCountTextBox.Text = PageCount.ToString();
            this.PageSizeComboBox = Template.FindName("PageSize", this) as ComboBox;
            List<int> list = PageSizeList as List<int>;
            if (list != null && list.Count > 0)
            {
                this.PageSizeComboBox.ItemsSource = list;
                this.PageSizeComboBox.SelectedItem = list[0];
            }
            this.InitializeVisualElementsContainer();
        }

        //Get element from name. If it exist then element instance return, if not, new will be created
        private T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
        {
            T element = this.GetTemplateChild(partName) as T ?? new T();
            return element;
        }

        private void InitializeVisualElementsContainer()
        {
            this.FirstPageClickButton.Click -= this.OnFirstPageBtnClick;
            this.FirstPageClickButton.Click += this.OnFirstPageBtnClick;
            this.LastPageClickButton.Click -= this.OnLastPageBtnClick;
            this.LastPageClickButton.Click += this.OnLastPageBtnClick;
            this.NextPageClickButton.Click -= this.OnNextPageBtnClick;
            this.NextPageClickButton.Click += this.OnNextPageBtnClick;
            this.FinalPageClickButton.Click -= this.OnFinalPageBtnClick;
            this.FinalPageClickButton.Click += this.OnFinalPageBtnClick;
            this.SearchPageClickButton.Click -= this.OnSearchBtnClick;
            this.SearchPageClickButton.Click += this.OnSearchBtnClick;
            this.PageSizeComboBox.SelectionChanged -= this.OnPageSizeSelectedChanged;
            this.PageSizeComboBox.SelectionChanged += this.OnPageSizeSelectedChanged;
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// 页码依赖属性
        /// </summary>
        public static DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount",
                typeof(int),
                typeof(PagingComboBox),
                new PropertyMetadata(0, OnPageCountChanged, CoerceValue));

        public static DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize",
                typeof(int),
                typeof(PagingComboBox),
                new PropertyMetadata(0, OnPageSizeChanged, CoerceValue));

        public static DependencyProperty TotalPageCountProperty = DependencyProperty.Register("TotalPageCount",
                typeof(int),
                typeof(PagingComboBox),
                new PropertyMetadata(0, OnTotalPageCountChanged, CoerceValue));

        public static DependencyProperty ContentProperty = DependencyProperty.Register("Content",
                typeof(object),
                typeof(PagingComboBox),
                new PropertyMetadata(0, OnSearchChanged, CoerceStrValue));
        public static DependencyProperty PageSizeListProperty = DependencyProperty.Register("PageSizeList",
                typeof(object),
                typeof(PagingComboBox),
                new PropertyMetadata(0, OnPageSizeListChanged, CoerceListValue));


        /// <summary>
        /// 页码
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get { return (int)GetValue(TotalPageCountProperty); }
            set { SetValue(TotalPageCountProperty, value); }
        }

        /// <summary>
        /// 搜索内容
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// 页面大小集合
        /// </summary>
        public object PageSizeList
        {
            get { return (object)GetValue(PageSizeListProperty); }
            set { SetValue(PageSizeListProperty, value); }
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            int newValue = (int)baseValue;
            return newValue;
        }

        private static object CoerceStrValue(DependencyObject d, object baseValue)
        {
            string newValue = Convert.ToString(baseValue);
            return newValue;
        }

        private static object CoerceListValue(DependencyObject d, object baseValue)
        {
            object newValue = baseValue;
            return newValue;
        }

        #endregion

        #region 路由事件
        public static readonly RoutedEvent PageCountChangedEvent =
                EventManager.RegisterRoutedEvent(nameof(PageCountChanged), RoutingStrategy.Direct,
                typeof(RoutedPropertyChangedEventHandler<int>), typeof(PagingComboBox));
        public static readonly RoutedEvent PageSizeChangedEvent =
                EventManager.RegisterRoutedEvent(nameof(PageSizeChanged), RoutingStrategy.Direct,
                typeof(RoutedPropertyChangedEventHandler<int>), typeof(PagingComboBox));
        public static readonly RoutedEvent TotalPageCountChangedEvent =
                EventManager.RegisterRoutedEvent(nameof(TotalPageCountChanged), RoutingStrategy.Direct,
                typeof(RoutedPropertyChangedEventHandler<int>), typeof(PagingComboBox));
        public static readonly RoutedEvent SearchChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SearchChanged), RoutingStrategy.Direct,
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(PagingComboBox));
        #endregion

        #region 私有属性
        // List分页公式 List.Skip((pagecount-1)*pagesize).Take(pagesize)
        private Button FirstPageClickButton = new Button();
        private Button LastPageClickButton = new Button();
        private Button NextPageClickButton = new Button();
        private Button FinalPageClickButton = new Button();
        private ToggleButton SearchPageClickButton = new ToggleButton();
        private TextBox ContentTextBox = new TextBox();
        private TextBox PageCountTextBox = new TextBox();
        private ComboBox PageSizeComboBox = new ComboBox();
        #endregion

        #region 事件方法
        /// <summary>
        /// 执行第一页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFirstPageBtnClick(object sender, RoutedEventArgs e)
        {
            PageCount = 1;
        }

        /// <summary>
        /// 执行上一页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLastPageBtnClick(object sender, RoutedEventArgs e)
        {
            if (PageCount > 1)
            {
                PageCount--;

            }
        }

        /// <summary>
        /// 执行下一页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNextPageBtnClick(object sender, RoutedEventArgs e)
        {
            if (PageCount < TotalPageCount)
            {
                PageCount++;

            }
        }

        /// <summary>
        /// 执行最后页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFinalPageBtnClick(object sender, RoutedEventArgs e)
        {
            PageCount = TotalPageCount;
        }

        private bool searchFlag = false;
        /// <summary>
        /// 执行最后页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSearchBtnClick(object sender, RoutedEventArgs e)
        {
            searchFlag = true;
            Content = ContentTextBox.Text;
        }

        /// <summary>
        /// 执行页面数据量变更操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPageSizeSelectedChanged(object sender, RoutedEventArgs e)
        {
            PageCount = 1;
            PageSize = Convert.ToInt32(this.PageSizeComboBox.SelectedValue.ToString());
        }
        #endregion

        #region 页码路由事件

        /// <summary>
        /// 页码改变事件处理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PagingComboBox control = (PagingComboBox)d;
            var e1 = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue,
                (int)e.NewValue, PageCountChangedEvent);
            control.OnPageCountChanged(e1);
        }

        /// <summary>
        /// 页码改变路由事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> PageCountChanged
        {
            add
            {
                AddHandler(PageCountChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PageCountChangedEvent, value);
            }
        }

        /// <summary>
        /// 发送页码改变路由事件参数
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPageCountChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            PageCountTextBox.Text = args.NewValue.ToString();
            RaiseEvent(args);
        }
        #endregion

        #region 页面大小路由事件

        /// <summary>
        /// 页码改变事件处理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PagingComboBox control = (PagingComboBox)d;
            var e1 = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue,
                (int)e.NewValue, PageSizeChangedEvent);
            control.OnPageSizeChanged(e1);
        }

        /// <summary>
        /// 页码改变路由事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> PageSizeChanged
        {
            add
            {
                AddHandler(PageSizeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PageSizeChangedEvent, value);
            }
        }

        /// <summary>
        /// 发送页码改变路由事件参数
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPageSizeChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            RaiseEvent(args);
        }
        #endregion

        #region 总页数路由事件

        /// <summary>
        /// 页码改变事件处理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTotalPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PagingComboBox control = (PagingComboBox)d;
            var e1 = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue,
                (int)e.NewValue, TotalPageCountChangedEvent);
            control.OnTotalPageCountChanged(e1);
        }

        /// <summary>
        /// 页码改变路由事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> TotalPageCountChanged
        {
            add
            {
                AddHandler(TotalPageCountChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TotalPageCountChangedEvent, value);
            }
        }

        /// <summary>
        /// 发送页码改变路由事件参数
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnTotalPageCountChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            RaiseEvent(args);
        }
        #endregion

        #region 搜索路由事件

        /// <summary>
        /// 页码改变事件处理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSearchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            PagingComboBox control = (PagingComboBox)d;
            var e1 = new RoutedPropertyChangedEventArgs<string>(Convert.ToString(e.OldValue),
                Convert.ToString(e.NewValue), SearchChangedEvent);
            control.OnSearchChanged(e1);
        }

        /// <summary>
        /// 页码改变路由事件
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> SearchChanged
        {
            add
            {
                AddHandler(SearchChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SearchChangedEvent, value);
            }
        }

        /// <summary>
        /// 发送页码改变路由事件参数
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnSearchChanged(RoutedPropertyChangedEventArgs<string> args)
        {
            if (searchFlag)
            {
                RaiseEvent(args);
                searchFlag = false;
            }
        }
        #endregion

        private static void OnPageSizeListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
