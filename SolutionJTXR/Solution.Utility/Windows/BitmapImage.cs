using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Solution.Utility.Windows
{

    /// <summary>
    /// 图像助手
    /// </summary>
    public class BitmapImageHelper
    {
        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="uri">图像的路径。例如Images\\List_32x32.png或者pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png</param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImage(string uri)
        {
            if (Equals(uri, null) || uri.Length < 1)
            {
                return null;
            }
            BitmapImage bitmapImage = new BitmapImage();
            try
            {

                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                return bitmapImage;

            }
            catch (Exception ex)
            {
                Utility.LogHelper.Error("bitmapImage.UriSource记载资源错误。", ex);
                return null;
            }
        }

        /// <summary>
        /// 取得图像内容
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Image GetImage(string uri)
        {
            try
            {
                return new Image() { Source = GetBitmapImage(uri), Width = 16, Height = 16 };
            }
            catch (Exception ex)
            {
                Utility.LogHelper.Error("GetImage错误。", ex);
                return null;
            }
        }
    }
}
