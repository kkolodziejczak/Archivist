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

        public static object GetStaticFieldValue(string KeyValue)
        {
            return Application.Current.FindResource(KeyValue);
        }
    }
}
