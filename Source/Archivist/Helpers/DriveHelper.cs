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

        public static string[] GetAllDirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly);
        }

        public static string[] GetAllFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Clears copies
        /// </summary>
        /// <returns></returns>
        public static ClearResult ClearDriveSpace()
        {
            var result = ClearResult.Fail;
            
            // -Get into projcet copies 
            foreach (var project in Storage.Settings.Projects)
            {
                var Projects = GetAllDirectories(project.ArchivePath);
               
                foreach(var projectPath in Projects)
                {
                    var Days = GetAllDirectories(projectPath);

                    foreach(var day in Days)
                    {
                        var ProjectArchiveDirectories = GetAllDirectories(day);

                        foreach(var archiveDirectory in ProjectArchiveDirectories)
                        {
                            var ProjectCopies = GetAllFiles(archiveDirectory);

                            // If there is only 1 copie leave it
                            if (ProjectCopies.Length > 1)
                            {
                                // Else clear all but lastest copie assuming that they are sorted
                                for (int i = 0; i < ProjectCopies.Length - 1; i++)
                                {
                                    if (File.Exists(ProjectCopies[i]))
                                    {
                                        File.Delete(ProjectCopies[i]);
                                        result = ClearResult.Success;
                                    }
                                }
                            }
                        }
                    }
                }

                
            }

            if (result == ClearResult.Fail)
            {
                MessageBox.Show("Hard Drive is full.\nMake more space!", "No space left!", MessageBoxType.Ok);
            }

            return result;
        }

    }
}
