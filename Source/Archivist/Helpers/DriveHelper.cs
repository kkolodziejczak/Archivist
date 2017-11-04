using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    /// <summary>
    /// Possible clearing results
    /// </summary>
    public enum ClearResult
    {
        Success,
        Fail
    }

    /// <summary>
    /// Helper class that allows to perform additional actions on drives
    /// </summary>
    public static class DriveHelper
    {
        #region Public Methods
        
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
        /// Returns all top level directories from <see cref="directoryPath"/>
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetAllDirectories(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return new string[0];
            }

            return Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Returns all top level files from <see cref="directoryPath"/>
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
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

                foreach (var projectPath in Projects)
                {
                    var Days = GetAllDirectories(projectPath);

                    foreach (var day in Days)
                    {
                        var ProjectArchiveDirectories = GetAllDirectories(day);

                        foreach (var archiveDirectory in ProjectArchiveDirectories)
                        {
                            var ProjectCopies = GetAllFiles(archiveDirectory);

                            // If there is only 1 copie leave it
                            if (ProjectCopies.Length > 1)
                            {
                                // Else clear all but lastest copie assuming that they are sorted
                                for (int i = 0; i < ProjectCopies.Length - 1; i++)
                                {

                                    if (File.Exists(ProjectCopies[i]) && Path.GetExtension(ProjectCopies[i]) == ".zip")
                                    {
                                        try
                                        {
                                            File.Delete(ProjectCopies[i]);
                                            result = ClearResult.Success;
                                        }
                                        catch
                                        {
                                            // some error ignore
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            return result;
        } 

        #endregion

    }
}
