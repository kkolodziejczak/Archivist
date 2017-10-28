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

        /// <summary>
        /// Shortcut that fires create backup
        /// </summary>
        public KeyboardShortcut BackupShortcut { get; private set; }

        /// <summary>
        /// Default archive path
        /// </summary>
        public OpenDialogViewModel DefaultPathDialog { get; private set; }

        /// <summary>
        /// Indicates if something was changed
        /// </summary>
        public bool Change { get; set; }

        /// <summary>
        /// Command for recording backup <see cref="KeyboardShortcut"/>
        /// </summary>
        public ICommand RecordBackupCommand { get; private set; }

        /// <summary>
        /// Command that clears archives from all projects
        /// </summary>
        public ICommand ClearArchivesCommand { get; private set; }

        /// <summary>
        /// Command that save changes
        /// </summary>
        public ICommand SaveSettingsCommand { get; private set; }

        /// <summary>
        /// Command that discard changes
        /// </summary>
        public ICommand CancelChangeCommand { get; private set; }

        /// <summary>
        /// Command that load default settings
        /// </summary>
        public ICommand SetDefaultSettingsCommand{ get; private set; }

        /// <summary>
        /// Command that sets default archive path
        /// </summary>
        public ICommand SetDefaultArchivePathCommand { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SettingsViewModel()
        {
            // create commands
            RecordBackupCommand = new RelayCommand(RecordBackupShortcut);
            ClearArchivesCommand = new RelayCommand(ClearArchives);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            CancelChangeCommand = new RelayCommand(GetLastSettings);
            SetDefaultSettingsCommand = new RelayCommand(SetDefault);
            SetDefaultArchivePathCommand = new RelayCommand(OpenArchiveFileDialog);

            // Init Dialog to folder type
            DefaultPathDialog = new OpenDialogViewModel(OpenDialogType.OpenFolderDialog);
            
            // Get BackupShortcut from user's settings
            GetLastSettings();

        }

        /// <summary>
        /// Opens file dialog that allows user to select directory where he wants to save his archives by default
        /// </summary>
        /// <returns></returns>
        public void OpenArchiveFileDialog()
        {
            DefaultPathDialog.OpenDialog();
            Storage.Settings.DefaultPath = DefaultPathDialog.Path;

            Change = true;
        }

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

            Change = true;
        }

        /// <summary>
        /// Clears all archives
        /// </summary>
        private void ClearArchives()
        {
            string result = String.Empty;

            if (DriveHelper.ClearDriveSpace() == ClearResult.Success)
            {
                result = ResourceHelper.GetStaticFieldValue("ClearSuccessMessage") as string;
            }
            else
            {
                result = ResourceHelper.GetStaticFieldValue("ClearFailMessage") as string;
            }

            MessageBox.Show(result, "Clear Result",MessageBoxType.Ok);

        }

        /// <summary>
        /// Sets settings to default and reload view
        /// </summary>
        private void SetDefault()
        {
            Storage.SetDefaultSettings();

            // Refresh variables values to update view
            InitVariables();

            Change = true;
        }

        /// <summary>
        /// Save settings
        /// </summary>
        private void SaveSettings()
        {
            // Saves settings
            Storage.Save();

            Change = false;
        }

        /// <summary>
        /// Loads data from user settings and fills properties
        /// </summary>
        private void GetLastSettings()
        {
            Storage.Load();

            // Refresh variables values to update them
            InitVariables();

            Change = false;
        }

        /// <summary>
        /// Initialize all public properties with data from user settings
        /// </summary>
        private void InitVariables()
        {
            // Get BackupShortcut from user's settings
            BackupShortcut = Storage.Settings.BackupShortcut;
            DefaultPathDialog.Path = Storage.Settings.DefaultPath;
        }

    }
}
