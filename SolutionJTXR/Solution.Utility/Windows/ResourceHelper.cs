using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solution.Utility.Windows
{

    public class ResourceHelper
    {
        public static string LoadString(string key)
        {
            return Application.Current.TryFindResource(key) as string;
        }

        public static object FindResource(string key)
        {
            return Application.Current.TryFindResource(key);
        }
    }
}
