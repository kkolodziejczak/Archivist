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
        public KeyboardShortcut BackupShortcut { get; private set; }

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

            // Get BackupShortcut from user's settings
            BackupShortcut = Storage.Settings.BackupShortcut;

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

            // Clear to display message to user know that he needs to press new key combination
            BackupShortcut = new KeyboardShortcut();

            //Record new shortcut and attach CreateBackup method
            Storage.Settings.BackupShortcut = await KeyboardShortcutManager.Instance.RecordKeyboardShortcut();

            // Set new Shortcut as BackupShortcut
            BackupShortcut = Storage.Settings.BackupShortcut;

            // Register new Shortcut
            KeyboardShortcutManager.Instance.RegisterKeyboardShortcut(BackupShortcut);

        }

        #endregion

    }
}
