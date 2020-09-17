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
    /// 路段相关
    /// </summary>
    public partial class Agv3DAnimationViewModel//2 : VmBase
    {
        #region 路段3D控件集合
        private ObservableCollection<UserControls.Road3D> road3Ds = new ObservableCollection<UserControls.Road3D>();

        /// <summary>
        /// 路段可视控件集合
        /// </summary>
        public ObservableCollection<UserControls.Road3D> Road3Ds
        {
            get { return road3Ds; }
            set { Set(ref road3Ds, value); }
        }
        #endregion


        /// <summary>
        /// 初始化路段3D可视对象
        /// </summary>
        private void initRoad3Ds()
        {
            var markPointInfoes = getRoadInfoPageData();

            foreach (var v in markPointInfoes)
            {
                var point1 = MarkPoint3Ds.FirstOrDefault(a => a.MarkPointInfoModel.Id == v.StartMarkPointInfo_Id);
                var point2 = MarkPoint3Ds.FirstOrDefault(a => a.MarkPointInfoModel.Id == v.EndMarkPointInfo_Id);

                Road3Ds.Add(new UserControls.Road3D()
                {
                    RoadInfoModel = v,
                    //Point1 = new Point3D(point1.MarkPointInfoModel.X, point1.MarkPointInfoModel.Y, 0),
                    //Point2 = new Point3D(point2.MarkPointInfoModel.X, point2.MarkPointInfoModel.Y, 0)


                    Points = new Point3DCollection() { new Point3D(point1.MarkPointInfoModel.X, point1.MarkPointInfoModel.Y, 0),
                   new Point3D(point2.MarkPointInfoModel.X, point2.MarkPointInfoModel.Y, 0)},

                });

            }
        }

        #region 查询路段信息,生成可视控件
        /// <summary>
        /// 取得分页数据
        /// </summary>
        private RoadInfoModel[] getRoadInfoPageData()
        {
            PageRepuestParams pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 100;
            pageRepuestParams.FilterGroup = null;
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RoadInfoModel>>>(GlobalData.ServerRootUri + "RoadInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));
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
                    message = result?.Message ?? "查询路段信息失败，请联系管理员！";
                    LogHelper.Error(message, null);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //操作失败，显示错误信息
                message = result?.Message ?? "查询路段信息失败，请联系管理员！";
                Application.Current.Resources["UiMessage"] = message;
                LogHelper.Error(message, ex);
                return null;
            }
        }

        #endregion

    }
}
