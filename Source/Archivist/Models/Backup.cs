using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Archivist
{
    public struct ProjectInfo
    {
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
    }

    public class BackupManager
    {

        public static void CreateBackup()
        {
            //doc.Load(@"C:\Users\Themo\Git\Archivist\Source\Archivist\Archivist.csproj");

            // TODO: get information About selected project

            // Get Solution path
            var projects = GetProjectInformation(@"C:\Users\Themo\Git\Archivist\Source\Archivist.sln");

            foreach(var project in projects)
            {
                BackupFiles(project);
            }

        }

        private static void BackupFiles(ProjectInfo project)
        {
            //TODO: get Archive path if any

            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            XDocument projDefinition = XDocument.Load(project.ProjectPath);

            var references = projDefinition.Element(msbuild +"Project")
                                           .Elements(msbuild + "ItemGroup");

            //throw new NotImplementedException();
        }

        private static List<ProjectInfo> GetProjectInformation(string solutionFilePath)
        {
            var OutputList = new List<ProjectInfo>();

            var SolutionFileContents = FileHelper.GetFileContents(solutionFilePath);

            string BaseSolutionPath = solutionFilePath.Remove(solutionFilePath.LastIndexOf('\\'));

            // Get Project Name and Project file path
            var output = Regex.Matches(SolutionFileContents, "(?:\\s\")(.*?)(?:\",\\s\")(.*?)(?:\")");

            foreach(Match match in output)
            {
                OutputList.Add(new ProjectInfo()
                {
                    ProjectName = match.Groups[1].Value,
                    ProjectPath = $"{BaseSolutionPath}\\{match.Groups[2].Value}",
                });
            }

            return OutputList;
        }
    }
}
