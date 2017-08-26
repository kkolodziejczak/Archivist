using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Archivist
{
    public static class ResourceHelper
    {
        public static object GetStaticFieldValue(string valueKey)
        {
            return Application.Current.FindResource(valueKey);
        }

        public static object GetColor(string color)
        {
            return Application.Current.FindResource(color);
        }
    }
}
