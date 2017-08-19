using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Archivist
{
    public class ProjectsViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Private Fields

        /// <summary>
        /// Path to the project .sln file
        /// </summary>
        private string m_SourcePath; 

        #endregion

        #region Public Properties

        /// <summary>
        /// Title of the project to add
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Path to the project .sln file
        /// </summary>
        public string SourcePath
        {
            get => m_SourcePath;
            set
            {
                // If new value is the same skip
                if (m_SourcePath == value)
                    return;

                m_SourcePath = value;

                //TODO: Add default ArchivePath

            }
        }

        /// <summary>
        /// Path to the folder to store archives
        /// </summary>
        public string ArchivePath { get; set; }


        public ObservableCollection<ProjectViewModel> Projects { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command that adds Project
        /// </summary>
        public ICommand AddProjectCommand { get; set; }

        /// <summary>
        /// Command that opens <see cref="OpenFileDialog"/> to store it's path
        /// </summary>
        public ICommand OpenSourceFileDialogCommand { get; set; }

        /// <summary>
        /// Command that opens <see cref="SaveFileDialog"/> to store it's path
        /// </summary>
        public ICommand SaveArchiveFileDialogCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProjectsViewModel()
        {
            //TODO: Load Projects from "Database"

            // Allocate projects
            Projects = new ObservableCollection<ProjectViewModel>();

            for (int i = 0; i < 12; i++)
                Projects.Add(new ProjectViewModel());

            // Create Commands
            AddProjectCommand = new RelayCommand(async () => await AddProject());
            OpenSourceFileDialogCommand = new RelayCommand(async () => await OpenSourceFileDialog());
            SaveArchiveFileDialogCommand = new RelayCommand(async () => await OpenArchiveFileDialog());
        }

        #endregion

        #region Tasks

        /// <summary>
        /// Adds new project to user database
        /// </summary>
        /// <returns></returns>
        public async Task AddProject()
        {
            //TODO : add project to collection

            // Await because of async
            await Task.Delay(1);
        }

        /// <summary>
        /// Opens file dialog allowing user to pick ther solution file
        /// </summary>
        /// <returns></returns>
        public async Task OpenSourceFileDialog()
        {
            // Get Path
            OpenFileDialog dialog = new OpenFileDialog()
            {
                // Set Filter to solution files
                Filter = "Solution files (*.sln)|*.sln|All files (*.*)|*.*"
            };

            //TODO add check for proper file format!
            // Open file dialog
            // If Success save file path
            if (dialog.ShowDialog() == true)
                SourcePath = dialog.FileName;

            // Await because of async
            await Task.Delay(1);
        }

        /// <summary>
        /// Opens file dialog that allows user to select directory where he wants to save his archives
        /// </summary>
        /// <returns></returns>
        public async Task OpenArchiveFileDialog()
        {
            // Create new save dialog
            SaveFileDialog dialog = new SaveFileDialog()
            {
                // Set Filter to archive files
                Filter = "Archive file (*.rar)|*.rar|All files (*.*)|*.*"
            };

            // Open file dialog, if success save file path
            if (dialog.ShowDialog() == true)
                ArchivePath = dialog.FileName;

            // Await because of async
            await Task.Delay(1);
        }

        #endregion

        #region IDataErrorInfo

        public string Error => string.Empty;

        /// <summary>
        /// Checks for any Errors that may occur.
        /// </summary>
        /// <param name="columnName">Property Name.</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Title":
                        // If if user didn't provide any Title display message
                        if (Title == null)
                        {
                            return "Please enter title of the project.";
                        }
                        else
                        {
                            // If Title is composed of white-space characters display error message.
                            if (Title.Trim() == string.Empty)
                            {
                                return "Project title is required.";
                            }
                        }

                        break;

                    case "SourcePath":
                        // If if user didn't provide any source path display message
                        if (SourcePath == null)
                        {
                            return "Please chose project path.";
                        }
                        else
                        {
                            // If source path is composed of white-space characters display error message.
                            if (SourcePath.Trim() == string.Empty)
                            {
                                return "Project source path is required.";
                            }
                            else
                            {
                                // If source path does not end with .sln display error message.
                                if (!SourcePath.EndsWith(".sln"))
                                {
                                    return "Project solution file (.sln) is required.";
                                }
                            }
                        }
                        break;
                }
                return null;
            }
        }

        #endregion
    }
}