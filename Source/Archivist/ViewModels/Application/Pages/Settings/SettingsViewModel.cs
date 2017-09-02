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


        public ICommand ShortCutButtonCommand { get; private set; } 

        public SettingsViewModel()
        {
            // create commands
            ShortCutButtonCommand = new RelayCommand(ShortcutButtonClick);
        }

        public void ShortcutButtonClick()
        {

        }

    }
}
