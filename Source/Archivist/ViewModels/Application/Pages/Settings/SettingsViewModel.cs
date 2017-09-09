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
        /// Text to display at shortcut button
        /// </summary>
        public string ShortCutButtonText { get; set; } = "ctrl + s";

        public KeyboardShortcut BackupShortcut { get; set; }


        public ICommand ShortCutButtonCommand { get; private set; } 

        public SettingsViewModel()
        {
            // create commands
            ShortCutButtonCommand = new RelayCommand(ShortcutButtonClick);

            BackupShortcut = new KeyboardShortcut()
            {
                Key = Key.S,
                Ctrl = true,
            };

            BackupShortcut.OnShortcutActivated += CreateBackup;

            KeyboardShortcutManager.Instance.RegisterKeyboardShortcut(BackupShortcut);


        }

        public void ShortcutButtonClick()
        {
            KeyboardShortcutManager.Instance.RecordKeyboardShortcut(BackupShortcut);
        }

        public void CreateBackup(object sender, EventArgs e)
        {
            MessageBox.Show("Backup created!", "Backup");
        }

    }
}
