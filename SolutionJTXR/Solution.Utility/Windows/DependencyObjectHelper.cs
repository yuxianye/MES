using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Solution.Utility.Windows
{


    public class DependencyObjectHelper
    {
        public static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                if (source is Visual || source is System.Windows.Media.Media3D.Visual3D)
                    source = VisualTreeHelper.GetParent(source);
            }
            return source;

        }
    }
}
