using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    /// <summary>
    /// Helper class that allows to perform additional actions on files
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// Returns contents of file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileContents(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return String.Empty;
            }

        }

    }
}
