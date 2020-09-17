using System.Text;
namespace Solution.Desktop.Core
{
    /// <summary>
    /// 页面信息类
    /// </summary>
    public class ViewInfo
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public ViewInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewType">页面类型</param>
        /// <param name="viewAssemblyName">页面所在本地程序集名称，默认Solution.Desktop.View， 例如：Solution.Desktop.View</param>
        /// <param name="viewName">本地页面的名称，例如：Solution.Desktop.View.LoginView</param>
        /// <param name="viewModelAssemblyName">VM所在本地程序集的名称，例如Solution.Desktop.ViewModel</param>
        /// <param name="viewModelName">VM的名称，例如：Solution.Desktop.ViewModel.LoginViewModel</param>
        /// <param name="icon">图标</param>
        /// <param name="parameter">特殊参数</param>
        public ViewInfo(
            string displayName,
            ViewType viewType,
            string viewAssemblyName,
            string viewName,
            string viewModelAssemblyName,
            string viewModelName,
            string icon,
            object parameter = null,
            bool canClose = true
           )
        {
            DisplayName = displayName;
            ViewType = viewType;
            ViewAssemblyName = viewAssemblyName;
            ViewName = viewName;
            ViewModelAssemblyName = viewModelAssemblyName;
            ViewModelName = viewModelName;
            Icon = icon;
            Parameter = parameter;
            CanClose = canClose;
        }

        /// <summary>
        /// 页面是否可关闭,默认true可关闭，不配置默认可关闭
        /// </summary>
        public bool? CanClose { get; set; } = true;

        /// <summary>
        /// 本地页面的显示名称，例如：用户登陆，不定义则使用服务端的模块的Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public ViewType ViewType { get; set; } = ViewType.Unknown;

        /// <summary>
        /// 页面所在本地程序集名称，默认Solution.Desktop.View， 例如：Solution.Desktop.View
        /// </summary>
        public string ViewAssemblyName { get; set; } = "Solution.Desktop.View";

        /// <summary>
        /// 本地页面的名称，例如：Solution.Desktop.View.LoginView
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// VM所在本地程序集的名称，例如Solution.Desktop.ViewModel
        /// </summary>
        public string ViewModelAssemblyName { get; set; }

        /// <summary>
        /// VM的名称，例如：Solution.Desktop.ViewModel.LoginViewModel
        /// </summary>
        public string ViewModelName { get; set; }

        /// <summary>
        /// 特殊参数
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// 菜单模块
        /// </summary>
        public MenuModule MenuModule { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } = @"pack://application:,,,/Solution.Desktop.Resource;component/Images/logo_32X32.ico";

        /// <summary>
        /// 重写 Tostring方法，输出属性的值。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DisplayName=");
            stringBuilder.Append(DisplayName);
            stringBuilder.Append(System.Environment.NewLine);

            stringBuilder.Append("ViewType=");
            stringBuilder.Append(ViewType);
            stringBuilder.Append(System.Environment.NewLine);

            stringBuilder.Append("ViewAssemblyName=");
            stringBuilder.Append(ViewAssemblyName);
            stringBuilder.Append(System.Environment.NewLine);


            stringBuilder.Append("ViewName=");
            stringBuilder.Append(ViewName);
            stringBuilder.Append(System.Environment.NewLine);


            stringBuilder.Append("ViewModelAssemblyName=");
            stringBuilder.Append(ViewModelAssemblyName);
            stringBuilder.Append(System.Environment.NewLine);


            stringBuilder.Append("ViewModelName=");
            stringBuilder.Append(ViewModelName);
            stringBuilder.Append(System.Environment.NewLine);

            stringBuilder.Append("Parameter=");
            stringBuilder.Append(Parameter);
            stringBuilder.Append(System.Environment.NewLine);

            stringBuilder.Append("CanClose=");
            stringBuilder.Append(CanClose.ToString());
            stringBuilder.Append(System.Environment.NewLine);

            return stringBuilder.ToString();
        }


    }
}
