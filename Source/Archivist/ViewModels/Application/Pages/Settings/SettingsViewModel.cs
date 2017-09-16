using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    public class SettingsViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Shortcut that fires create backup
        /// </summary>
        public KeyboardShortcut BackupShortcut { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command for recording backup <see cref="KeyboardShortcut"/>
        /// </summary>
        public ICommand RecordBackupCommand { get; private set; }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SettingsViewModel()
        {
            // create commands
            RecordBackupCommand = new RelayCommand(RecordBackupShortcut);

            // Generate Default Shortcut 
            // TODO: Load shortcut from user's settings
            BackupShortcut = new KeyboardShortcut()
            {
                Key = Key.S,
                Ctrl = true,
            };
            BackupShortcut.OnShortcutActivated += CreateBackup;


            // Start listening for Shortcut
            KeyboardShortcutManager.Instance.RegisterKeyboardShortcut(BackupShortcut);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method that records backup shortcut
        /// </summary>
        private async void RecordBackupShortcut()
        {
            // Clear old Shortcut from manager
            KeyboardShortcutManager.Instance.UnregisterKeyboardShortcut(BackupShortcut);

            //Record new shortcut and attach CreateBackup method
            BackupShortcut = await KeyboardShortcutManager.Instance.RecordKeyboardShortcut();
            BackupShortcut.OnShortcutActivated += CreateBackup;

            // Register new Shortcut
            // TODO: save new Shortcut into user settings
            KeyboardShortcutManager.Instance.RegisterKeyboardShortcut(BackupShortcut);

        }

        /// <summary>
        /// Temporary method that indicates creating backup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateBackup(object sender, EventArgs e)
        {
            MessageBox.Show("Backup created!", "Backup");
        }

        #endregion

    }
}
