using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Agv.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using Solution.Utility.Extensions;
using Solution.Utility.Socket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Solution.Desktop.Agv.ViewModel
{
    /// <summary>
    /// 地标点相关
    /// </summary>
    public partial class Agv3DAnimationViewModel
    {
        #region 地标点3D控件集合
        private ObservableCollection<UserControls.MarkPoint3D> markPoint3Ds = new ObservableCollection<UserControls.MarkPoint3D>();

        /// <summary>
        /// 地标点可视控件集合
        /// </summary>
        public ObservableCollection<UserControls.MarkPoint3D> MarkPoint3Ds
        {
            get { return markPoint3Ds; }
            set { Set(ref markPoint3Ds, value); }
        }
        #endregion


        /// <summary>
        /// 初始化地标点3D可视对象
        /// </summary>
        private void initMarkPoint3Ds()
        {
            var markPointInfoes = getMarkPointPageData();

            foreach (var v in markPointInfoes)
            {
                MarkPoint3Ds.Add(new UserControls.MarkPoint3D() { MarkPointInfoModel = v });
            }
        }

        #region 查询agv信息,生成可视控件
        /// <summary>
        /// 取得分页数据
        /// </summary>
        private MarkPointInfoModel[] getMarkPointPageData()
        {
            PageRepuestParams pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 100;
            pageRepuestParams.FilterGroup = null;
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MarkPointInfoModel>>>(GlobalData.ServerRootUri + "MarkPointInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));
            string message;
            try
            {


                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    return result.Data.Data;
                }
                else
                {
                    message = result?.Message ?? "查询地标点信息失败，请联系管理员！";
                    LogHelper.Error(message, null);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //操作失败，显示错误信息
                message = result?.Message ?? "查询地标点信息失败，请联系管理员！";
                Application.Current.Resources["UiMessage"] = message;
                LogHelper.Error(message, ex);
                return null;
            }
        }

        #endregion

    }
}
