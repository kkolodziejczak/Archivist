using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        
        /// <summary>
        /// Path to the project .sln file
        /// </summary>
        private string _SourcePath;

        /// <summary>
        /// Title of the project to add
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Path to the project .sln file
        /// </summary>
        public string SourcePath
        {
            get
            {
                if (_SourcePath == null || _SourcePath == String.Empty)
                    return "";

                return _SourcePath;
            }
            set
            {
                // If new value is the same skip
                if (_SourcePath == value)
                    return;

                _SourcePath = value;
            }
        }

        /// <summary>
        /// Path to the folder to store archives
        /// </summary>
        public string ArchivePath { get; set; }

        /// <summary>
        /// Indicator that show if user is editing project information
        /// </summary>
        public bool Editing { get; private set; }

        /// <summary>
        /// Project that is edited
        /// </summary>
        public ProjectItemControlViewModel EditedProject { get; private set; }

        /// <summary>
        /// Projects added by user
        /// </summary>
        public ObservableCollection<ProjectItemControlViewModel> Projects { get; set; }
        
        /// <summary>
        /// Command that adds Project
        /// </summary>
        public ICommand AddProjectCommand { get; set; }

        /// <summary>
        /// Command that opens <see cref="OpenFileDialog"/> to store it's path
        /// </summary>
        public ICommand OpenSourceFileDialogCommand { get; set; }

        /// <summary>
        /// Command that opens <see cref="System.Windows.Forms.FolderBrowserDialog"/> to store it's path
        /// </summary>
        public ICommand SaveArchiveFileDialogCommand { get; set; }

        /// <summary>
        /// Command that cancels editing
        /// </summary>
        public ICommand CancelButtonCommand { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProjectsViewModel()
        {
            // Create Commands
            AddProjectCommand = new RelayCommand(AddProject);
            OpenSourceFileDialogCommand = new RelayCommand(OpenSourceFileDialog);
            SaveArchiveFileDialogCommand = new RelayCommand(OpenArchiveFileDialog);
            CancelButtonCommand = new RelayCommand(CancelEditing);

            if (Storage.Settings.Projects == null)
                return;

            // Allocate projects
            Projects = new ObservableCollection<ProjectItemControlViewModel>();

            foreach (var project in Storage.Settings.Projects)
            {
                var newProject = new ProjectItemControlViewModel
                {
                    Project = project,
                };

                newProject.OnEditButtonClick += EditButtonClick;
                newProject.OnDeleteButtonClick += DeleteButtonClick;
                newProject.OnActiveProjectClick += SelectProjectClick;

                Projects.Add(newProject);
            }

        }

        /// <summary>
        /// Method that fills fields for edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButtonClick(object sender, EventArgs e)
        {
            Editing = true;

            if (sender is ProjectItemControlViewModel project)
            {
                EditedProject = project;
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
                var result = MessageBox.Show("Do you want to delete this project?", "Confirmation", MessageBoxType.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Projects.Remove(project);
                    Storage.Settings.Projects.Remove(project.Project);
                    Storage.Save();
                }
            }

        }

        /// <summary>
        /// Adds new project to user database
        /// </summary>
        /// <returns></returns>
        public void AddProject()
        {
            var newProjectItem = new Project()
            {
                Title = Title,
                SourcePath = SourcePath,
            };

            if (ArchivePath == null || ArchivePath == String.Empty)
            {
                newProjectItem.ArchivePath = Storage.Settings.DefaultPath;
            }
            else
            {
                newProjectItem.ArchivePath = ArchivePath;
            }

            // In case of editing current project
            if (Editing == true)
            {
                SwapEditedProjectInformation(newProjectItem);
                return;
            }

            Storage.Settings.Projects.Add(newProjectItem);
            Storage.Save();

            var project = new ProjectItemControlViewModel
            {
                Project = newProjectItem,
            };

            project.OnEditButtonClick += EditButtonClick;
            project.OnDeleteButtonClick += DeleteButtonClick;
            project.OnActiveProjectClick += SelectProjectClick;

            Projects.Add(project);

            ClearInputFields();
        }

        /// <summary>
        /// Set new information for <see cref="EditedProject"/> information
        /// </summary>
        private void SwapEditedProjectInformation(Project newProjectItem)
        {
            var projIndex = Projects.IndexOf(EditedProject);
            var oldProjIndex = Storage.Settings.Projects.IndexOf(Projects[projIndex].Project);

            Projects[projIndex].Title = Title;
            Projects[projIndex].Project = newProjectItem;

            Storage.Settings.Projects[oldProjIndex] = newProjectItem;
            Storage.Save();

            ClearInputFields();
            Editing = false;
        }

        /// <summary>
        /// Cancels editing mode
        /// </summary>
        public void CancelEditing()
        {
            Editing = false;
            ClearInputFields();
            EditedProject = null;
        }

        /// <summary>
        /// Selects project that user will work on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectProjectClick(object sender, EventArgs e)
        {
            if (sender is ProjectItemControlViewModel project)
            {
                Storage.Settings.SelectedProject = project.Project;
            }
        }

        /// <summary>
        /// Clears input fields
        /// </summary>
        private void ClearInputFields()
        {
            Title = String.Empty;
            SourcePath = String.Empty;
            ArchivePath = String.Empty;
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
            // Create new File dialog
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            // Open file dialog, if success save file path
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ArchivePath = dialog.SelectedPath;
        }

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
                                // While in editing mode don't display error messages about edited project title
                                if (Editing == true)
                                {
                                    if (Title != EditedProject.Title)
                                    {
                                        return "Project with that title already exists.";
                                    }
                                }
                                else
                                {
                                    return "Project with that title already exists.";
                                }
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

    }
}