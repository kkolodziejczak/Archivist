using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    public class Project
    {

        /// <summary>
        /// Project title provided by user
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Project name from config file
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Path where stored is .sln file
        /// </summary>
        public string SourcePath {get; set;}

        /// <summary>
        /// Project path to config file from .sln file
        /// </summary>
        public string ProjectConfigPath { get; set; }

        /// <summary>
        /// Base path to project files
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Path to where store archives
        /// </summary>
        public string ArchivePath { get; set; }

        /// <summary>
        /// Time when last copie was created
        /// </summary>
        public string TimeLastCopy { get; set; }

        /// <summary>
        /// Date when last copie was created
        /// </summary>
        public string DateLastCopy { get; set; }


    }
}
