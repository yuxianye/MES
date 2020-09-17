using Solution.Desktop.Core;
using Solution.Desktop.MatWareHousAreaLocationInfo.Model;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;

namespace Solution.Desktop.MatWareHousAreaLocationInfo.View
{
    /// <summary>
    /// WareHouseInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class MatWareHousAreaLocationInfoView : UserControlBase
    {
        bool datagriddetailLayoutUpdated = false;
        //
        SolidColorBrush solidcolorBrush1;
        SolidColorBrush solidcolorBrush2;
        SolidColorBrush solidcolorBrush3;
        SolidColorBrush solidcolorBrush4;
        SolidColorBrush solidcolorBrush5;

        public MatWareHousAreaLocationInfoView()
        {
            InitializeComponent();
            //空库位
            solidcolorBrush1 = new SolidColorBrush();
            solidcolorBrush1.Color = Colors.LightGray;
            //空托盘
            solidcolorBrush2 = new SolidColorBrush();
            solidcolorBrush2.Color = Colors.LightGreen;
            //部分库位
            solidcolorBrush4 = new SolidColorBrush();
            solidcolorBrush4.Color = Colors.SlateBlue;
            //满库位
            solidcolorBrush3 = new SolidColorBrush();
            solidcolorBrush3.Color = Colors.Blue;
            //异常库位
            solidcolorBrush5 = new SolidColorBrush();
            solidcolorBrush5.Color = Colors.Red;
        }

        private void DataGridDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            int ii = e.Row.GetIndex() + 1;
            if (ii == ((DataGrid)sender).Items.Count)
            {
                datagriddetailLayoutUpdated = true;
            }
        }

        private void DataGridDetail_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridShow();
        }

        private void DataGridDetail_LayoutUpdated(object sender, System.EventArgs e)
        {
            if (datagriddetailLayoutUpdated)
            {
                DataGridShow();
                datagriddetailLayoutUpdated = false;
            }
        }

        public void DataGridShow()
        {
            for (int i = 0; i < DataGridDetail.Items.Count; i++)
            {
                foreach (DataGridColumn datagridColumn in DataGridDetail.Columns)
                {
                    if (datagridColumn.Header.ToString().IndexOf("列") != -1)
                    {
                        TextBlock textBlock = datagridColumn.GetCellContent(DataGridDetail.Items[i]) as TextBlock;
                        if (textBlock != null)
                        {
                            if (datagridColumn.Header.ToString().IndexOf("列") != -1)
                            {
                                string sInfo = textBlock.Text;
                                //
                                if (!sInfo.Equals(""))
                                {
                                    string[] asInfo = sInfo.Split(' ');
                                    string sWareHouseLocationCode = "";
                                    string sPalletCode = "";
                                    decimal dStorageQuantity = 0;
                                    decimal dFullPalletQuantity = 0;
                                    string sIsUse = "";
                                    //
                                    if (asInfo.Length >= 5)
                                    {
                                        sWareHouseLocationCode = asInfo[0];
                                        sPalletCode = asInfo[1];
                                        //
                                        dStorageQuantity = 0;
                                        decimal.TryParse(asInfo[2], out dStorageQuantity);
                                        //
                                        dFullPalletQuantity = 0;
                                        decimal.TryParse(asInfo[3], out dFullPalletQuantity);
                                        //
                                        sIsUse = asInfo[4];
                                        //
                                        if (!sPalletCode.Equals("-") && dStorageQuantity > 0 && dStorageQuantity == dFullPalletQuantity)
                                        {
                                            textBlock.Background = solidcolorBrush3;
                                        }
                                        else if (!sPalletCode.Equals("-") && dStorageQuantity > 0)
                                        {
                                            textBlock.Background = solidcolorBrush4;
                                        }
                                        else if (!sPalletCode.Equals("-"))
                                        {
                                            textBlock.Background = solidcolorBrush2;
                                        }
                                        else if (!sIsUse.Equals("True"))
                                        {
                                            textBlock.Background = solidcolorBrush5;
                                        }
                                        else
                                        {
                                            textBlock.Background = solidcolorBrush1;
                                        }
                                        //
                                        //textBlock.ToolTip = i.ToString() + " " + datagridColumn.DisplayIndex.ToString() + " " + sInfo;
                                        textBlock.Tag = i.ToString() + " " + datagridColumn.DisplayIndex.ToString() + " " + sInfo;
                                    }
                                    //
                                    textBlock.Text = "";
                                }
                            }
                            else
                            {
                                //textBlock.ToolTip = i.ToString() + " " + datagridColumn.DisplayIndex.ToString();
                                textBlock.Tag = i.ToString() + " " + datagridColumn.DisplayIndex.ToString();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            //foreach (MatWareHousAreaLocationInfoModel items in DataGridDetail.Items)
            //{
            //    string sLayerNumber = items.LayerNumber;
            //    string sColumnNumber01 = items.ColumnNumber01;
            //}
        }


        private void DataGridDetail_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void DataGridDetail2_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridShow2();
        }

        public void DataGridShow2()
        {
            for (int i = 0; i < DataGridDetail2.Items.Count; i++)
            {
                foreach (DataGridColumn datagridColumn in DataGridDetail2.Columns)
                {
                    //
                    TextBlock textBlock = datagridColumn.GetCellContent(DataGridDetail2.Items[i]) as TextBlock;
                    if (textBlock != null)
                    {
                        if (datagridColumn.Header.ToString().IndexOf("空库位") != -1)
                        {
                            textBlock.Background = solidcolorBrush1;
                        }
                        else if (datagridColumn.Header.ToString().IndexOf("空托盘") != -1)
                        {
                            textBlock.Background = solidcolorBrush2;
                        }
                        else if (datagridColumn.Header.ToString().IndexOf("满库位") != -1)
                        {
                            textBlock.Background = solidcolorBrush3;
                        }
                        else if (datagridColumn.Header.ToString().IndexOf("部分库位") != -1)
                        {
                            textBlock.Background = solidcolorBrush4;
                        }
                        else if (datagridColumn.Header.ToString().IndexOf("异常库位") != -1)
                        {
                            textBlock.Background = solidcolorBrush5;
                        }
                    }
                }
            }
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (e.Key == System.Windows.Input.Key.Enter)
            //{
            //    //foreach (MatWareHousAreaLocationInfoModel items in DataGridDetail.Items)
            //    //{
            //    //    DataGridDetail.Items.Remove(items);
            //    //}
            //}
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            //DataGridDetail.ItemsSource = null;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T childContent = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                childContent = v as T;
                if (childContent == null)
                {
                    childContent = GetVisualChild<T>(v);
                }
                if (childContent != null)
                {
                    break;
                }
            }
            return childContent;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
