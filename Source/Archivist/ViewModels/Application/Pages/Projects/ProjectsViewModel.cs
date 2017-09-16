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
    /// <summary>
    /// ViewModel for ProjectsPage
    /// </summary>
    public class ProjectsViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Private Fields

        /// <summary>
        /// Path to the project .sln file
        /// </summary>
        private string _SourcePath;

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
            get => _SourcePath;
            set
            {
                // If new value is the same skip
                if (_SourcePath == value)
                    return;

                _SourcePath = value;

                //TODO: Add default ArchivePath

            }
        }

        /// <summary>
        /// Path to the folder to store archives
        /// </summary>
        public string ArchivePath { get; set; }


        public ObservableCollection<ProjectItemControlViewModel> Projects { get; set; }

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

            BackupManager.CreateBackup();
            
            // Allocate projects
            Projects = new ObservableCollection<ProjectItemControlViewModel>();

            for (int i = 0; i < 12; i++)
            {
                Title = $"Test Title{i}";
                SourcePath = "SRC PATH";
                ArchivePath = "ArchivePath PATH";

                AddProject();
            }

            Title = "";
            SourcePath = "";
            ArchivePath = "";

            // Create Commands
            AddProjectCommand = new RelayCommand(AddProject);
            OpenSourceFileDialogCommand = new RelayCommand(OpenSourceFileDialog);
            SaveArchiveFileDialogCommand = new RelayCommand(OpenArchiveFileDialog);

        }

        #endregion

        #region Private Methods

        private void EditButtonClick(object sender, EventArgs e)
        {
            if(sender is ProjectItemControlViewModel project)
            {
                Title = project.Project.Title;
                SourcePath = project.Project.SourcePath;
                ArchivePath = project.Project.ArchivePath;
            }

        }

        /// <summary>
        /// Delete project from list from with request comes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            if (sender is ProjectItemControlViewModel project)
            {
                var result = Archivist.MessageBox.Show("Do you want to delete this project?", "Confirmation");

                if (result == MessageBoxResult.Yes)
                    Projects.Remove(project);
            }

        }

        #endregion

        #region Tasks

        /// <summary>
        /// Adds new project to user database
        /// </summary>
        /// <returns></returns>
        public void AddProject()
        {

            //TODO : add project to collection
            var project = new ProjectItemControlViewModel
            {
                Project = new Project
                {
                    Title = Title,
                    SourcePath = SourcePath,
                    ArchivePath = ArchivePath
                }
            };

            project.OnEditButtonClick += EditButtonClick;
            project.OnDeleteButtonClick += DeleteButtonClick;
            project.OnActiveProjectClick += DeleteButtonClick;

            Projects.Add(project);
        }

        /// <summary>
        /// Opens file dialog allowing user to pick ther solution file
        /// </summary>
        /// <returns></returns>
        public void OpenSourceFileDialog()
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
        }

        /// <summary>
        /// Opens file dialog that allows user to select directory where he wants to save his archives
        /// </summary>
        /// <returns></returns>
        public void OpenArchiveFileDialog()
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
                            // Check if title exist
                            int count = Projects.Count(p => p.Title == Title);
                            if (count > 0)
                            {
                                return "Project with that title already exists.";
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