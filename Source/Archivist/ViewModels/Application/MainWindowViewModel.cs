﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// ViewModel for the MainWindow.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        
        /// <summary>
        /// Title of the window
        /// </summary>
        private string _WindowTitle;

        /// <summary>
        /// Current page
        /// </summary>
        public Pages Page { get; set; }

        /// <summary>
        /// Title of the window
        /// </summary>
        public string WindowTitle
        {
            get
            {
                if (_WindowTitle == String.Empty || _WindowTitle == null)
                    return "Archivist - None";

                return _WindowTitle;
            }
            set
            {
                if(value.Contains("Archivist -"))
                    _WindowTitle = value;

                _WindowTitle = $"Archivist - {value}";
            }
        }

        /// <summary>
        /// Token to stop Backup infinite loop
        /// </summary>
        public static CancellationTokenSource wtoken { get; set; }

        /// <summary>
        /// Command to switch to ProjectsPage
        /// </summary>
        public ICommand ProjectsCommand { get; set; }

        /// <summary>
        /// Command to switch to SettingsPage
        /// </summary>
        public ICommand SettingsCommand { get; set; }

        /// <summary>
        /// Command to switch to InfoPage
        /// </summary>
        public ICommand InfoCommand { get; set;  }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Load settings 
            Storage.Load();

            // Set Starting page
            Page = Pages.Projects;

            // Create commands
            ProjectsCommand = new RelayCommand(() => SwitchPage(Pages.Projects));
            SettingsCommand = new RelayCommand(() => SwitchPage(Pages.Settings));
            InfoCommand = new RelayCommand(() => SwitchPage(Pages.Info));

            // Set BackupButton Shortcut event
            Storage.Settings.BackupShortcut.OnShortcutActivated += Backup.Create;
            
            // Start listening for Shortcut
            KeyboardShortcutManager.Instance.RegisterKeyboardShortcut(Storage.Settings.BackupShortcut);

            // Create Tasks that will run in background
            wtoken = new CancellationTokenSource();
            Task.Run(Backup.ProcessTask, wtoken.Token);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindowViewModel()
        {
            using (wtoken)
            {
                wtoken.Cancel();
            }

            wtoken = null;
        }

        /// <summary>
        /// Switches page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public void SwitchPage(Pages page)
        {
            Page = page;
        }

        /// <summary>
        /// Updates Main Window Title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateWindowTitle(object sender, WindowNameArg e)
        {
            WindowTitle = e.NewWindowName;
        }

    }
}
