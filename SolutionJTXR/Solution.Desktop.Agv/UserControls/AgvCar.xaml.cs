using Solution.Desktop.Agv.Model;
using Solution.Desktop.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Solution.Desktop.Agv.UserControls
{
    /// <summary>
    /// Agv小车控件
    /// </summary>
    public partial class AgvCar : UserControlBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AgvCar()
        {
            init();
            InitializeComponent();
        }

        #region 属性

        /// <summary>
        /// agv实体
        /// </summary>
        public Agv.Model.AgvInfoModel AgvInfoModel;

        ///// <summary>
        ///// 小车Id
        ///// </summary>
        //public Guid CarId { get; set; }

        ///// <summary>
        ///// 小车名称
        ///// </summary>
        //public string CarName { get { return this.carName.Text; } set { this.carName.Text = value; } }


        private int carStatus = 0;

        /// <summary>
        /// 获取或设置 小车状态，状态改变后颜色改变。
        /// </summary>
        public int CarStatus
        {
            get { return carStatus; }
            set
            {
                carStatus = value;
                agvCar.Background = Utility.Windows.ResourceHelper.FindResource("AgvStatusBrush" + value) as Brush;
            }
        }

        #endregion

        MatrixTransform matrix = new MatrixTransform();
        TransformGroup groups = new TransformGroup();
        MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
        Storyboard story = new Storyboard();
        public Canvas canvas;

        private void init()
        {
            initAnimation();
        }

        private void initAnimation()
        {
            matrixAnimation.IsOffsetCumulative = false;// !matrixAnimation.AutoReverse;
            matrixAnimation.DoesRotateWithTangent = false;//旋转
            matrixAnimation.FillBehavior = FillBehavior.HoldEnd;
            //matrixAnimation.AccelerationRatio = 0.4;
            //matrixAnimation.DecelerationRatio = 0.4;
            story.Children.Add(matrixAnimation);
            Storyboard.SetTargetName(matrixAnimation, "matrix");
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="">运行路径需要的 时间间隔（秒）</param>
        public void Run(Canvas canvas, Path path, double timeSpan)
        {
            this.canvas = canvas;
            Canvas.SetLeft(this, -this.ActualWidth / 2);
            Canvas.SetTop(this, -this.ActualHeight / 2);
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = matrix;
            NameScope.SetNameScope(canvas, new NameScope());
            //this.RegisterName("matrix", matrix);
            canvas.RegisterName("matrix", matrix);
            matrixAnimation.PathGeometry = path.Data.GetFlattenedPathGeometry();
            matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(timeSpan));
            //matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //matrixAnimation.AutoReverse = true;
            //matrixAnimation.IsOffsetCumulative = false;// !matrixAnimation.AutoReverse;
            //matrixAnimation.DoesRotateWithTangent = true;//旋转
            //matrixAnimation.FillBehavior = FillBehavior.HoldEnd;
            //matrixAnimation.AccelerationRatio = 0.4;
            //matrixAnimation.DecelerationRatio = 0.4;
            //matrixAnimation.SpeedRatio = 0.5;
            //story.Children.Add(matrixAnimation);
            //Storyboard.SetTargetName(matrixAnimation, "matrix");
            //Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));
            story.Begin(canvas, true);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            story.Pause(canvas);
        }

        /// <summary>
        /// 暂停之后恢复,继续前进
        /// </summary>
        public void Resume()
        {
            story.Resume(canvas);
        }

        protected override void Disposing()
        {
            throw new NotImplementedException();
        }

        private void agvCar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            menu.IsOpen = true;
        }
    }



}
