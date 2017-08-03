using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Archivist
{
    public class ProjectsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Title of the project to add
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Path to the project
        /// </summary>
        public string SourcePath { get; set; }

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

        public ProjectsViewModel()
        {
            //TODO: Load Projects from "Database"

            // Allocate projects
            Projects = new ObservableCollection<ProjectViewModel>();

            for (int i =0;i< 12; i++)
                Projects.Add(new ProjectViewModel());

            // Create Commands
            AddProjectCommand = new RelayCommand(async () => await AddProject());
            OpenSourceFileDialogCommand = new RelayCommand(async () => await OpenSourceFileDialog());
            SaveArchiveFileDialogCommand = new RelayCommand(async () => await OpenArchiveFileDialog());
        }

        public async Task AddProject()
        {
            //TODO : add project to collection

            // Await because of async
            await Task.Delay(1);
        }

        public async Task OpenSourceFileDialog()
        {
            // Get Path
            OpenFileDialog dialog = new OpenFileDialog()
            {
                // Set Filter to solution files
                Filter = "Solution files (*.sln)|*.sln|All files (*.*)|*.*"
            };

            // Open file dialog
            // If Success save file path
            if (dialog.ShowDialog() == true)
                SourcePath = dialog.FileName;

            // Await because of async
            await Task.Delay(1);
        }

        public async Task OpenArchiveFileDialog()
        {
            // Create new save dialog
            SaveFileDialog dialog = new SaveFileDialog()
            {
                // Set Filter to archive files
                Filter = "Archive file (*.rar)|*.rar|All files (*.*)|*.*"
            };

            // Open file dialog
            // If Success save file path
            if (dialog.ShowDialog() == true)
                ArchivePath = dialog.FileName;

            // Await because of async
            await Task.Delay(1);
        }

    }
}