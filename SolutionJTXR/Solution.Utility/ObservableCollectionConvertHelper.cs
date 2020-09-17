using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility
{
    public static class ObservableCollectionConvertHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> from)
        {
            ObservableCollection<T> to = new ObservableCollection<T>();
            foreach (var f in from)
            {
                to.Add(f);
            }
            return to;
        }
    }
}
