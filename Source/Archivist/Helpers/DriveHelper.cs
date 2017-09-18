using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    public enum ClearResult
    {
        Success,
        Fail
    }

    public static class DriveHelper
    {
        /// <summary>
        /// Returns TotalFreeSpace for provided Drive name
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public static long GetTotalFreeSpace(string driveName)
        {
            return DriveInfo.GetDrives()
                            .Where(d => d.Name == driveName)
                            .Select(d => d.TotalFreeSpace)
                            .ToList()
                            .FirstOrDefault();
        }

        /// <summary>
        /// Returns directory size
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static long GetDirectorySize(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

            long size = 0;
            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name);
                size += info.Length;
            }

            return size;
        }

        /// <summary>
        /// Clears copies from Archives.
        /// </summary>
        /// <returns></returns>
        public static ClearResult ClearDriveSpace()
        {
            // Maybe check for "space that may be celared" 1st 
            // If it is less than 10 MB return fail ??

            /////////////////////////
            // Clearing algorithm: //
            /////////////////////////
            // -Get into projcet copies. 
            // -Get all days
            // -If there is only 1 copie leave it
            // -Else clear all but lastest copie
            // -If there are only one copie or 
            // -and sapce can not be cleared return fail.
            MessageBox.Show("Hard Drive is full.\nMake more space!", "No space left!", MessageBoxType.Ok);

            return ClearResult.Fail;
        }

    }
}
