using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using BrowsableAttribute = System.ComponentModel.BrowsableAttribute;
using System.Windows.Controls;

namespace Solution.Desktop.Agv.UserControls
{

    //    public class Agv3D : BoxVisual3D// MeshElement3D
    //    {
    //        public Agv3D()
    //        {
    //            //Fill = Brushes.Orange;
    //            SetValue(FillProperty, Utility.Windows.ResourceHelper.FindResource("AgvCarColorBrush"));
    //            SetValue(WidthProperty, Utility.Windows.ResourceHelper.FindResource("AgvWidth"));
    //            SetValue(HeightProperty, Utility.Windows.ResourceHelper.FindResource("AgvHeight"));
    //            SetValue(LengthProperty, Utility.Windows.ResourceHelper.FindResource("AgvLength"));
    //#if DEBUG
    //            Center = new Point3D(10, 2, Height / 2);
    //#endif

    //            //base.Visual3DModel=
    //            //var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - HTC3650-1.STL");
    //            //            //foreach (var v in model3d.Children)
    //            //            //{


    //            //            //}
    //            //            var result = model3d.Children[0] as GeometryModel3D;
    //            //            return result.Geometry as MeshGeometry3D;
    //        }

    //        protected override MeshGeometry3D Tessellate()
    //        {

    //            //ModelImporter modelImporter = new ModelImporter();
    //            ////var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - HTC3650-1.STL");
    //            //var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - 安全围栏-堆垛机-1 立柱总成-端部-2 连接角-4.STL");
    //            //var result = model3d.Children[0] as GeometryModel3D;
    //            //return result.Geometry as MeshGeometry3D;
    //            /////////////////////////////////////////////////////////////////////
    //            var b = new MeshBuilder(false, true);
    //            b.AddCubeFace(
    //                this.Center, new Vector3D(-1, 0, 0), new Vector3D(0, 0, 1), this.Length, this.Width, this.Height);
    //            b.AddCubeFace(
    //                this.Center, new Vector3D(1, 0, 0), new Vector3D(0, 0, -1), this.Length, this.Width, this.Height);
    //            b.AddCubeFace(
    //                this.Center, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), this.Width, this.Length, this.Height);
    //            b.AddCubeFace(
    //                this.Center, new Vector3D(0, 1, 0), new Vector3D(0, 0, -1), this.Width, this.Length, this.Height);
    //            if (this.TopFace)
    //            {
    //                b.AddCubeFace(
    //                    this.Center, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), this.Height, this.Length, this.Width);
    //            }

    //            if (this.BottomFace)
    //            {
    //                b.AddCubeFace(
    //                    this.Center, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), this.Height, this.Length, this.Width);
    //            }

    //            b.AddArrow(this.Center, new Point3D(Center.X + 1, Center.Y + 1, Center.Z + 1), 2);
    //            //b.ExtrudeText(
    //            //               "Helix Toolkit",
    //            //               "Arial",
    //            //               FontStyles.Normal,
    //            //               FontWeights.Bold,
    //            //               20,
    //            //               new Vector3D(1, 0, 0),
    //            //               new Point3D(0, 0, 0),
    //            //               new Point3D(0, 0, 1));
    //            return b.ToMesh();
    //        }
    //    }
    public class AgvUIElement3D : UIElement3D//ModelUIElement3D//
    {
        public static readonly DependencyProperty RoofAngleProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("RoofAngle", 15, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty RoofThicknessProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("RoofThickness", 0.2, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty FloorThicknessProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("FloorThickness", 0.2, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty StoryHeightProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("StoryHeight", 2.5, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty WidthProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("Width", 10, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty LengthProperty = DependencyPropertyEx.Register<double, AgvUIElement3D>("Length", 20, (s, e) => s.AppearanceChanged());
        public static readonly DependencyProperty StoriesProperty = DependencyPropertyEx.Register<int, AgvUIElement3D>("Stories", 1, (s, e) => s.AppearanceChanged());

        private readonly GeometryModel3D roof = new GeometryModel3D();
        private readonly GeometryModel3D walls = new GeometryModel3D();

        public AgvUIElement3D()
        {
            //ModelUIElement3D modelUIElement3D = new ModelUIElement3D();
            //modelUIElement3D.Model = new PointLight();

            //ModelVisual3D modelVisual3D = new ModelVisual3D();
            //modelVisual3D.Content = new PointLight();

            //Viewport3D viewport3D = new Viewport3D();
            //viewport3D.Camera = new PerspectiveCamera();
            //HelixToolkit.Wpf.HelixViewport3D.
            ////viewport3D.Children.Add 
            ModelImporter modelImporter = new ModelImporter();
            modelImporter.Load("");



            this.roof.Material = MaterialHelper.CreateMaterial(Brushes.Brown, ambient: 10);
            this.walls.Material = MaterialHelper.CreateMaterial(Brushes.White, ambient: 10);
            this.AppearanceChanged();
            var model = new Model3DGroup();
            model.Children.Add(this.roof);
            model.Children.Add(this.walls);
            this.Visual3DModel = model;
        }

        [Category("House properties")]
        //[Slidable(0, 60)]
        [Browsable(true)]
        public double Width
        {
            get { return (double)this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }

        //[Slidable(0, 60)]
        [Browsable(true)]
        public double Length
        {
            get { return (double)this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        //[Slidable(0, 4)]
        //[FormatString("0.00")]
        [Browsable(true)]
        public double StoryHeight
        {
            get { return (double)this.GetValue(StoryHeightProperty); }
            set { this.SetValue(StoryHeightProperty, value); }
        }

        //[Slidable(1, 8)]
        [Browsable(true)]
        public int Stories
        {
            get { return (int)this.GetValue(StoriesProperty); }
            set { this.SetValue(StoriesProperty, value); }
        }

        //[Slidable(0, 60)]
        [Browsable(true)]
        public double RoofAngle
        {
            get { return (double)this.GetValue(RoofAngleProperty); }
            set { this.SetValue(RoofAngleProperty, value); }
        }

        //[Slidable(0, 2)]
        //[FormatString("0.00")]
        [Browsable(true)]
        public double RoofThickness
        {
            get { return (double)this.GetValue(RoofThicknessProperty); }
            set { this.SetValue(RoofThicknessProperty, value); }
        }

        //[Slidable(0, 1)]
        //[FormatString("0.00")]
        [Browsable(true)]
        public double FloorThickness
        {
            get { return (double)this.GetValue(FloorThicknessProperty); }
            set { this.SetValue(FloorThicknessProperty, value); }
        }

        private void AppearanceChanged()
        {
            var y0 = 0d;
            var wallBuilder = new MeshBuilder(false, false);
            for (int i = 0; i < this.Stories; i++)
            {
                if (i > 0 && this.FloorThickness > 0)
                {
                    wallBuilder.AddBox(new Point3D(0, 0, y0 + this.FloorThickness / 2), this.Length + 0.2, this.Width + 0.2, this.FloorThickness);
                    y0 += this.FloorThickness;
                }

                wallBuilder.AddBox(new Point3D(0, 0, y0 + this.StoryHeight / 2), this.Length, this.Width, this.StoryHeight);
                y0 += this.StoryHeight;
            }

            var theta = Math.PI / 180 * this.RoofAngle;
            var roofBuilder = new MeshBuilder(false, false);
            var y1 = y0 + Math.Tan(theta) * this.Width / 2;
            var p0 = new Point(0, y1);
            var p1 = new Point(this.Width / 2 + 0.2 * Math.Cos(theta), y0 - 0.2 * Math.Sin(theta));
            var p2 = new Point(p1.X + this.RoofThickness * Math.Sin(theta), p1.Y + this.RoofThickness * Math.Cos(theta));
            var p3 = new Point(0, y1 + this.RoofThickness / Math.Cos(theta));
            var p4 = new Point(-p2.X, p2.Y);
            var p5 = new Point(-p1.X, p1.Y);
            var roofSection = new[] { p0, p1, p1, p2, p2, p3, p3, p4, p4, p5, p5, p0 };
            roofBuilder.AddExtrudedSegments(roofSection, new Vector3D(0, -1, 0), new Point3D(-this.Length / 2, 0, 0), new Point3D(this.Length / 2, 0, 0));
            var cap = new[] { p0, p1, p2, p3, p4, p5 };
            roofBuilder.AddPolygon(cap, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), new Point3D(-this.Length / 2, 0, 0));
            roofBuilder.AddPolygon(cap, new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), new Point3D(this.Length / 2, 0, 0));
            var p6 = new Point(this.Width / 2, y0);
            var p7 = new Point(-this.Width / 2, y0);
            wallBuilder.AddPolygon(new[] { p0, p6, p7 }, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), new Point3D(-this.Length / 2, 0, 0));
            wallBuilder.AddPolygon(new[] { p0, p6, p7 }, new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), new Point3D(this.Length / 2, 0, 0));
            this.walls.Geometry = wallBuilder.ToMesh(true);
            this.roof.Geometry = roofBuilder.ToMesh(true);




        }
    }

    /// <summary>
    /// AGV3D可视控件，车辆位置或者通讯连接后再设置为显示
    /// </summary>
    public class Agv3D : BoxVisual3D
    {
        public Agv3D()
        {
            SetValue(FillProperty, Utility.Windows.ResourceHelper.FindResource("AgvCarColorBrush"));
            SetValue(WidthProperty, Utility.Windows.ResourceHelper.FindResource("AgvWidth"));
            SetValue(HeightProperty, Utility.Windows.ResourceHelper.FindResource("AgvHeight"));
            SetValue(LengthProperty, Utility.Windows.ResourceHelper.FindResource("AgvLength"));
            Center = new Point3D(Center.X, Center.Y, Height / 2);
        }

        /// <summary>
        /// 名称文字
        /// </summary>
        private BillboardTextVisual3D billboardTextVisual3D = new BillboardTextVisual3D();

        /// <summary>
        /// 车轮直径
        /// </summary>
        public static readonly DependencyProperty WheelDiameterProperty = DependencyProperty.Register(
            "WheelDiameter", typeof(double), typeof(Agv3D), new UIPropertyMetadata(0.12, GeometryChanged));

        /// <summary>
        /// 获取或设置 车轮直径.
        /// </summary>
        /// <value>The diameter. The default value is <c>0.06</c>.</value>
        public double WhellDiameter
        {
            get
            {
                return (double)this.GetValue(WheelDiameterProperty);
            }

            set
            {
                this.SetValue(WheelDiameterProperty, value);
            }
        }

        /// <summary>
        /// AGV信息模型
        /// </summary>
        public Agv.Model.AgvInfoModel AgvInfoModel { get; set; }


        protected override MeshGeometry3D Tessellate()
        {

            var b = new MeshBuilder(false, false);
            var centerBox = new Point3D(this.Center.X, this.Center.Y, this.Center.Z + WhellDiameter / 2);
            b.AddCubeFace(
               centerBox, new Vector3D(-1, 0, 0), new Vector3D(0, 0, 1), this.Length, this.Width, this.Height);
            b.AddCubeFace(
               centerBox, new Vector3D(1, 0, 0), new Vector3D(0, 0, -1), this.Length, this.Width, this.Height);
            b.AddCubeFace(
               centerBox, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), this.Width, this.Length, this.Height);
            b.AddCubeFace(
               centerBox, new Vector3D(0, 1, 0), new Vector3D(0, 0, -1), this.Width, this.Length, this.Height);
            if (this.TopFace)
            {
                b.AddCubeFace(
                   centerBox, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), this.Height, this.Length, this.Width);
            }

            if (this.BottomFace)
            {
                b.AddCubeFace(
                  centerBox, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), this.Height, this.Length, this.Width);
            }
            double wheelZ = centerBox.Z - Height / 2 + 0.01;
            //b.AddArrow(new Point3D(Center.X, Center.Y, Center.Z + Height / 2), new Point3D(Center.X, Center.Y, Center.Z + Height), 2, 0.3);
            //突出车体0.005
            const double wheelBulge = 0.005;
            const double wheelThickness = 0.04;

            //四个轮子，轮子突出车体0.005，内嵌0.015
            //左前
            b.AddPipe(new Point3D(centerBox.X + Length / 4, centerBox.Y + Width / 2 - (wheelThickness - wheelBulge), wheelZ), new Point3D(centerBox.X + Length / 4, centerBox.Y + Width / 2 + wheelBulge, wheelZ), 0.01, WhellDiameter, 36);
            //右前
            b.AddPipe(new Point3D(centerBox.X + Length / 4, centerBox.Y - Width / 2 - wheelBulge, wheelZ), new Point3D(centerBox.X + Length / 4, centerBox.Y - Width / 2 + (wheelThickness - wheelBulge), wheelZ), 0.01, WhellDiameter, 36);

            //左后
            b.AddPipe(new Point3D(centerBox.X - Length / 4, centerBox.Y + Width / 2 - (wheelThickness - wheelBulge), wheelZ), new Point3D(centerBox.X - Length / 4, centerBox.Y + Width / 2 + wheelBulge, wheelZ), 0.01, WhellDiameter, 36);
            //右后
            b.AddPipe(new Point3D(centerBox.X - Length / 4, centerBox.Y - Width / 2 - wheelBulge, wheelZ), new Point3D(centerBox.X - Length / 4, centerBox.Y - Width / 2 + (wheelThickness - wheelBulge), wheelZ), 0.01, WhellDiameter, 36);
            //名称
            billboardTextVisual3D.Text = this.AgvInfoModel?.CarName;
            billboardTextVisual3D.Background = Brushes.Transparent;
            billboardTextVisual3D.Foreground = Brushes.Blue;
            //textVisual3D.FontSize = 20;
            billboardTextVisual3D.Position = new Point3D(Center.X + Length / 2, Center.Y, Center.Z + Height);
            //textVisual3D.Height = 5;

            if (!this.Children.Contains(billboardTextVisual3D))
            {
                this.Children.Add(billboardTextVisual3D);
            }



            return b.ToMesh();

        }
        //if (!this.Children.Contains(particleSystem))
        //{
        //    particleSystem.Position = new Point3D(Center.X - Length / 2, Center.Y + Width / 2, Center.Z);
        //    this.Children.Add(particleSystem);
        //}
        //var b = new MeshBuilder(false, true);
        //b.AddCubeFace(
        //    this.Center, new Vector3D(-1, 0, 0), new Vector3D(0, 0, 1), this.Length, this.Width, this.Height);
        //b.AddCubeFace(
        //    this.Center, new Vector3D(1, 0, 0), new Vector3D(0, 0, -1), this.Length, this.Width, this.Height);
        //b.AddCubeFace(
        //    this.Center, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), this.Width, this.Length, this.Height);
        //b.AddCubeFace(
        //    this.Center, new Vector3D(0, 1, 0), new Vector3D(0, 0, -1), this.Width, this.Length, this.Height);
        //if (this.TopFace)
        //{
        //    b.AddCubeFace(
        //        this.Center, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), this.Height, this.Length, this.Width);
        //}

        //if (this.BottomFace)
        //{
        //    b.AddCubeFace(
        //        this.Center, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), this.Height, this.Length, this.Width);
        //}


        //b.AddTriangle(this.Center, new Point3D(this.Center.X + 5, this.Center.Y + 5, this.Center.Z + 5), new Point3D(this.Center.X + 5, this.Center.Y + 5, this.Center.Z + 15));
        //b.Append.ExtrudeText(
        //               "Helix Toolkit",
        //               "Arial",
        //               FontStyles.Normal,
        //               FontWeights.Bold,
        //               20,
        //               new Vector3D(1, 0, 0),
        //               new Point3D(0, 0, 0),
        //               new Point3D(0, 0, 1));


        //////base.Tessellate();
        //HelixToolkit.Wpf.TextVisual3D text = new HelixToolkit.Wpf.TextVisual3D();
        //text.FontSize = 60;
        ////text.TextDirection =this .LengthDirection;
        ////text.Position = new Point3D(this.Center.X, this.Center.Y, this.Center.Z + this.Height / 2 + 30);
        //text.Position = Center;// new Point3D(0, 0, 0);
        //text.Foreground = Brushes.Green;
        //text.Background = Brushes.Black;
        //text.Text = this.DisplayName;
        ////AddVisual3DChild(text);
        //BillboardTextVisual3D billboardTextVisual3D = new BillboardTextVisual3D();
        //billboardTextVisual3D.Text = this.DisplayName;
        //////builder.ExtrudeText(
        //////    "Helix Toolkit",
        //////    "Arial",
        //////    FontStyles.Normal,
        //////    FontWeights.Bold,
        //////    20,
        //////    new Vector3D(1, 0, 0),
        //////    new Point3D(0, 0, 0),
        //////    new Point3D(0, 0, 1));
        /////
        ////return b.ToMesh();

        //GeometryModel3D gm3D = text.Content as GeometryModel3D;
        //MeshGeometry3D mesh = gm3D.Geometry as MeshGeometry3D;
        ////Point3DCollection point = new Point3DCollection();
        ////point = mesh.Positions;
        ////List<Point3D> list = new List<Point3D>();
        ////list.AddRange(point);
        //var tes = base.Tessellate();
        ////foreach (var v in mesh.Positions)
        ////{
        ////    tes.Positions.Add(v);
        ////}
        ////foreach (var v in mesh.TriangleIndices)
        ////{
        ////    //tes.Positions.Add(v);
        ////    tes.TriangleIndices.Add(v);
        ////}

        //HelixToolkit.Wpf.ModelImporter modelImporter = new ModelImporter();
        //var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - HTC3650-1.STL");
        ////foreach (var v in model3d.Children)
        ////{


        ////}
        //var result = model3d.Children[0] as GeometryModel3D;
        //return result.Geometry as MeshGeometry3D;

        ////return tes;

        ////return b.ToMesh();// mesh;
        ////b.Append(mesh);


        ////return b.ToMesh();



        //ModelImporter modelImporter = new ModelImporter();
        ////var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - HTC3650-1.STL");
        //var model3d = modelImporter.Load(@"C:\Users\Administrator\Desktop\stl\布局调整-总装 - 安全围栏-堆垛机-1 立柱总成-端部-2 连接角-4.STL");
        //var result = model3d.Children[0] as GeometryModel3D;
        //return result.Geometry as MeshGeometry3D;
        /////////////////////////////////////////////////////////////////////
        //ParticleSystem particleSystem = new ParticleSystem();
        //protected override void OnGeometryChanged()
        //{
        //    base.OnGeometryChanged();
        //    HelixToolkit.Wpf.TextVisual3D text = new HelixToolkit.Wpf.TextVisual3D();
        //    text.FontSize = 60;
        //    //text.TextDirection =this .LengthDirection;
        //    text.Position = new Point3D(this.Center.X, this.Center.Y, this.Center.Z + this.Height / 2 + 30);
        //    text.Foreground = Brushes.Green;
        //    text.Background = Brushes.Black;
        //    text.Text = "Agv";// this.DisplayName;
        //    AddVisual3DChild(text);
        //}
        ///// <summary>
        ///// 显示名车.
        ///// </summary>
        //public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        //    "DisplayName", typeof(string), typeof(Agv3D), new UIPropertyMetadata("Agv", GeometryChanged));

        //private static void OnDisplayNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    Agv3D agv3D = d as Agv3D;
        //    if (!Equals(e.NewValue, e.OldValue))
        //    {
        //        agv3D.ChangeDiaplayName(e.NewValue.ToString());
        //    }
        //}

        //private void ChangeDiaplayName(string sisplayName)
        //{

        //}

        ///// <summary>
        ///// 获取或设置名称
        ///// </summary>
        //public string DisplayName
        //{
        //    get
        //    {
        //        return (string)this.GetValue(DisplayNameProperty);
        //    }

        //    set
        //    {
        //        this.SetValue(DisplayNameProperty, value);
        //    }
        //}

    }
}
