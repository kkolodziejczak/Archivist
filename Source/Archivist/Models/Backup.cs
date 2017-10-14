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

    public class Backup
    {

        /// <summary>
        /// Queue with all CopyTasks
        /// </summary>
        private static Queue<List<Project>> Tasks { get; set; } = new Queue<List<Project>>();

        /// <summary>
        /// <see cref="DateTime"/> when last copy was made
        /// </summary>
        private static DateTime LastTask { get; set; } = DateTime.Now.AddSeconds(10);

        /// <summary>
        /// Creates Backup of current project
        /// </summary>
        public static void Create(object sender, EventArgs e)
        {
            if (Storage.Settings.SelectedProject == null)
            {
                return;
            }

            // Where >= 0 mean that last copy was made at least 10 sec ago
            if (DateTime.Now.AddSeconds(-10).CompareTo(LastTask) >= 0)
            {
                LastTask = DateTime.Now;

                var projects = GetProjectInformation(Storage.Settings.SelectedProject.SourcePath, Storage.Settings.SelectedProject);

                Tasks.Enqueue(projects);
            }

        }

        /// <summary>
        /// infinite loop that process all tasks
        /// </summary>
        public static async Task ProcessTask()
        {
            while (true)
            {
                if (Tasks.Count > 0)
                {
                    var TaskToProcess = Tasks.Dequeue();

                    foreach (var project in TaskToProcess)
                    {
                        BackupFiles(project);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), MainWindowViewModel.wtoken.Token);
            }
        }

        /// <summary>
        /// Returns information about projects found in .sln file
        /// </summary>
        /// <param name="solutionFilePath"></param>
        /// <returns></returns>
        private static List<Project> GetProjectInformation(string solutionFilePath, Project ProjectInfo)
        {
            var ProjectList = new List<Project>();

            var SolutionFileContents = IOHelper.GetFileContents(solutionFilePath);

            string BaseSolutionPath = Path.GetDirectoryName(solutionFilePath);

            // Get Project Name and Project file path
            var output = Regex.Matches(SolutionFileContents, "(?:\\s\")(.*?)(?:\",\\s\")(.*?)(?:\")");

            foreach (Match match in output)
            {
                ProjectList.Add(new Project()
                {
                    ProjectName = match.Groups[1].Value,
                    ProjectConfigPath = Path.GetFileName(match.Groups[2].Value),
                    BasePath = $"{BaseSolutionPath}\\{match.Groups[1].Value}",
                    Title = ProjectInfo.Title,
                    ArchivePath = ProjectInfo.ArchivePath,
                    SourcePath = ProjectInfo.SourcePath,
                    DateLastCopy = $"{DateTime.Now.ToShortDateString()}".Replace('-', '_'),
                    TimeLastCopy = $"{DateTime.Now.ToLongTimeString()}".Replace(':', '_'),
                });
            }

            return ProjectList;
        }

        /// <summary>
        /// Create backup of all files found in <paramref name="project"/>
        /// </summary>
        /// <param name="project"><see cref="Project"/> that represents project data that you want to make backup of</param>
        private static void BackupFiles(Project project)
        {
            List<string> SourceFiles = GetSourceFiles(project);

            var ArchivePath = project.ArchivePath;
            var TemporaryDirectoryPath = Storage.TemporaryDirectoryPath;

            var AvailableTemporaryDiskSpace = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(TemporaryDirectoryPath));

            if (AvailableTemporaryDiskSpace < 10 * Size.Megabyte)
            {
                if (DriveHelper.ClearDriveSpace() == ClearResult.Fail)
                {
                    return;
                }
                else
                {
                    var AvailableSpaceAfterCleaning = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(TemporaryDirectoryPath));
                    if (AvailableSpaceAfterCleaning < 10 * Size.Megabyte)
                    {
                        return;
                    }
                }
            }


            foreach (var sourceFile in SourceFiles)
            {
                CopyFileIntoDirectory(sourceFile, project.BasePath, TemporaryDirectoryPath);
            }


            var AvailableArchiveDiskSpace = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(ArchivePath));

            if (AvailableArchiveDiskSpace < DriveHelper.GetDirectorySize(TemporaryDirectoryPath))
            {
                if (DriveHelper.ClearDriveSpace() == ClearResult.Fail)
                {
                    CleanUp(TemporaryDirectoryPath);
                    return;
                }
                else
                {
                    var AvailableSpaceAfterCleaning = DriveHelper.GetTotalFreeSpace(Path.GetPathRoot(ArchivePath));

                    if (AvailableSpaceAfterCleaning < 10 * Size.Megabyte)
                    {
                        CleanUp(TemporaryDirectoryPath);
                        return;
                    }
                }
            }

            CreateZipFile(TemporaryDirectoryPath, ArchivePath, project);

            CleanUp(TemporaryDirectoryPath);
        }

        /// <summary>
        /// Returns List of source files found in <paramref name="project"/>
        /// </summary>
        /// <param name="project"><see cref="Project"/> with project data to get files from</param>
        /// <returns></returns>
        private static List<string> GetSourceFiles(Project project)
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
                // If value contains '.' that mean it is a file
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
        /// <param name="project"><see cref="Project"/> with information about project</param>
        private static void CreateZipFile(string temporaryDirectoryPath, string archivePath, Project project)
        {
            string NewFilePath = $"{archivePath}\\Archivist\\{project.DateLastCopy}\\{project.ProjectName}";

            if (!Directory.Exists(NewFilePath))
            {
                Directory.CreateDirectory(NewFilePath);
            }

            ZipFile.CreateFromDirectory(temporaryDirectoryPath, $"{NewFilePath}\\Archivist_{project.TimeLastCopy}.zip");
        }

        /// <summary>
        /// Clear all temporary files
        /// </summary>
        /// <param name="directoryPath"></param>
        private static void CleanUp(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        } 

    }
}
