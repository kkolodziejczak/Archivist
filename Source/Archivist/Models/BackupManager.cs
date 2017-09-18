using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO.Compression;


namespace Archivist
{
    public struct ProjectInfo
    {
        public string ProjectName { get; set; }
        public string ProjectConfigPath { get; set; }
        public string BasePath { get; set; }
    }

    public class BackupManager
    {

        public static void CreateBackup()
        {
            // TODO: get information About selected project
           

            // Get Solution path
            var projects = GetProjectInformation(@"C:\Users\Themo\Git\Archivist\Source\Archivist.sln");

            foreach(var project in projects)
            {
                BackupFiles(project);
            }

        }

        /// <summary>
        /// Returns information about projects found in .sln file
        /// </summary>
        /// <param name="solutionFilePath"></param>
        /// <returns></returns>
        private static List<ProjectInfo> GetProjectInformation(string solutionFilePath)
        {
            var ProjectList = new List<ProjectInfo>();

            var SolutionFileContents = IOHelper.GetFileContents(solutionFilePath);

            string BaseSolutionPath = Path.GetDirectoryName(solutionFilePath);

            // Get Project Name and Project file path
            var output = Regex.Matches(SolutionFileContents, "(?:\\s\")(.*?)(?:\",\\s\")(.*?)(?:\")");

            foreach (Match match in output)
            {
                ProjectList.Add(new ProjectInfo()
                {
                    ProjectName = match.Groups[1].Value,
                    ProjectConfigPath = Path.GetFileName(match.Groups[2].Value),
                    BasePath = $"{BaseSolutionPath}\\{match.Groups[1].Value}",
                });
            }

            return ProjectList;
        }

        /// <summary>
        /// Create backup of all files found in <paramref name="project"/>
        /// </summary>
        /// <param name="project"><see cref="ProjectInfo"/> that represents project data that you want to make backup of</param>
        private static void BackupFiles(ProjectInfo project)
        {

            List<string> SourceFiles = GetSourceFiles(project);
           
            //TODO: Get Diskname from:
            // Archive path.
            // Get ProjectName
            // Get Project Path

            // Data structure ?
            var ArchivePath = "C:\\TestArchivePath";
            var TemproraryDirectoryPath = "C:\\Temp";


            var AvailableTemporaryDiskSpace = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(TemproraryDirectoryPath));

            if (AvailableTemporaryDiskSpace < 10 * Size.Megabyte)
            {
                if (DriveHelper.ClearDriveSpace() == ClearResult.Fail)
                {
                    return;
                }
            }


            foreach (var sourceFile in SourceFiles)
            {
                CopyFileIntoDirectory(sourceFile, project.BasePath, TemproraryDirectoryPath);
            }


            var AvailableArchiveDiskSpace = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(ArchivePath));

            if (AvailableArchiveDiskSpace < DriveHelper.GetDirectorySize(TemproraryDirectoryPath))
            {
                if (DriveHelper.ClearDriveSpace() == ClearResult.Fail)
                {
                    return;
                }
            }

            CreateZipFile(TemproraryDirectoryPath, ArchivePath, project);

            // ~~Cleanup~~
            Directory.Delete(TemproraryDirectoryPath, true);

        }

        /// <summary>
        /// Returns List of source files found in <paramref name="project"/>
        /// </summary>
        /// <param name="project"><see cref="ProjectInfo"/> with project data to get files from</param>
        /// <returns></returns>
        private static List<string> GetSourceFiles(ProjectInfo project)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            XDocument proj = XDocument.Load($"{project.BasePath}\\{project.ProjectConfigPath}");

            var SourceFiles = proj.Element(msbuild + "Project")
                                  .Elements(msbuild + "ItemGroup")
                                  .Elements()
                                  .Where(s => s.Name.LocalName != "Reference"
                                           && s.Name.LocalName != "Resource"
                                           && s.Name.LocalName != "Import");

            List<string> SourceFileNames = new List<string>
            {
                // Add Projectfile for backup
                Path.GetFileName(project.ProjectConfigPath)
            };

            foreach (var data in SourceFiles)
            {
                // For all compile files that 
                var attribute = data.Attribute("Include");
                if (attribute.Value != String.Empty)
                {
                    SourceFileNames.Add(attribute.Value);
                }
                // If value contains '.' that mean it is an file
                if (data.Value.Contains('.'))
                {
                    SourceFileNames.Add(data.Value);
                }
            }

            return SourceFileNames;
        }

        /// <summary>
        /// Copy <paramref name="fileName"/> from <paramref name="fileDirectory"/> into <paramref name="outputDirectory"/>
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileDirectory">Directory path where file is</param>
        /// <param name="outputDirectory">Directory where to put copied file</param>
        private static void CopyFileIntoDirectory(string fileName, string fileDirectory, string outputDirectory)
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            
            string SourceFilePath = $"{fileDirectory}\\{fileName}";
            string TemporaryDirectoryPath = $"{outputDirectory}\\{Path.GetDirectoryName(fileName)}";

            if (File.Exists(SourceFilePath))
            {
                // If Directory that file is inside does not exist and file is not in root directory
                if (!Directory.Exists(TemporaryDirectoryPath) && Path.GetDirectoryName(fileName) != String.Empty)
                {
                    Directory.CreateDirectory(TemporaryDirectoryPath);
                }

                File.Copy(SourceFilePath, $"{outputDirectory}\\{fileName}", true);
            }
        }

        /// <summary>
        /// Creates Zip file from temporary directory 
        /// </summary>
        /// <param name="temporaryDirectoryPath">Path to folder with files to create zip file from</param>
        /// <param name="archivePath">Path to where save created zip file</param>
        /// <param name="project"><see cref="ProjectInfo"/> with information about project</param>
        private static void CreateZipFile(string temporaryDirectoryPath, string archivePath, ProjectInfo project)
        {

            var TodaysDateAsString = $"{ DateTime.Now.ToShortDateString()}".Replace('-', '_');
            var TimeAsString = $"{DateTime.Now.ToLongTimeString()}".Replace(':', '_');

            string NewFilePath = $"{archivePath}\\Archivist\\{TodaysDateAsString}\\{project.ProjectName}";

            if (!Directory.Exists(NewFilePath))
            {
                Directory.CreateDirectory(NewFilePath);
            }

            ZipFile.CreateFromDirectory(temporaryDirectoryPath, $"{NewFilePath}\\Archivist_{TimeAsString}.zip");
        }

    }
}
