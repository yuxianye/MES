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
    /// agv小车相关
    /// </summary>
    public partial class Agv3DAnimationViewModel
    {
        #region Agv 3D车辆控件集合
        private ObservableCollection<UserControls.Agv3D> agv3Ds = new ObservableCollection<UserControls.Agv3D>();

        /// <summary>
        /// agv可视控件集合
        /// </summary>
        public ObservableCollection<UserControls.Agv3D> Agv3Ds
        {
            get { return agv3Ds; }
            set { Set(ref agv3Ds, value); }
        }
        #endregion

        //#region Agv 3D车辆控件集合
        //private ObservableCollection<UserControls.AgvUIElement3D> agv3Ds = new ObservableCollection<UserControls.AgvUIElement3D>();

        ///// <summary>
        ///// agv可视控件集合
        ///// </summary>
        //public ObservableCollection<UserControls.AgvUIElement3D> Agv3Ds
        //{
        //    get { return agv3Ds; }
        //    set { Set(ref agv3Ds, value); }
        //}
        //#endregion
        /// <summary>
        /// 初始化Agv3D可视对象。默认在零点，带通讯成功后在设置位置等信息
        /// </summary>
        private void initAgv3Ds()
        {
            var agvinfo = getAgvInfoPageData();
            foreach (var v in agvinfo)
            {
                Agv3Ds.Add(new UserControls.Agv3D() { AgvInfoModel = v });
                //Agv3Ds.Add(new UserControls.Agv3D() { });
                //Agv3Ds.Add(new UserControls.AgvUIElement3D() { Width = 0.7, Length = 1.5, IsHitTestVisible = false, Stories = 1, FloorThickness = 0.5, RoofThickness = 0.2 });
            }
        }

        #region 查询agv信息,生成可视控件
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private AgvInfoModel[] getAgvInfoPageData()
        {
            PageRepuestParams pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 3;
            pageRepuestParams.FilterGroup = null;
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<AgvInfoModel>>>(GlobalData.ServerRootUri + "AgvInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));
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
                    message = result?.Message ?? "查询Agv信息失败，请联系管理员！";
                    LogHelper.Error(message, null);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //操作失败，显示错误信息
                message = result?.Message ?? "查询Agv信息失败，请联系管理员！";
                Application.Current.Resources["UiMessage"] = message;
                LogHelper.Error(message, ex);
                return null;
            }
        }

        #endregion

    }
}
