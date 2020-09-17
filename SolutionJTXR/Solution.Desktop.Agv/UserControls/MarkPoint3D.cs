using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using HelixToolkit.Wpf;
namespace Solution.Desktop.Agv.UserControls
{
    /// <summary>
    /// 地标点可视模型
    /// </summary>
    public class MarkPoint3D : PieSliceVisual3D
    {

        public MarkPoint3D()
        {
            InnerRadius = 0;
            OuterRadius = 0.042;
            StartAngle = 0;
            EndAngle = 360;
            SetValue(FillProperty, Utility.Windows.ResourceHelper.FindResource("MarkPointBrush"));
        }



        private Model.MarkPointInfoModel markPointInfoModel;
        /// <summary>
        /// 地标点信息
        /// </summary>
        public Model.MarkPointInfoModel MarkPointInfoModel
        {
            get
            {
                return markPointInfoModel;
            }
            set
            {
                markPointInfoModel = value;
                this.Center = new System.Windows.Media.Media3D.Point3D(value.X, value.Y, 0.001);
            }
        }
    }
}
