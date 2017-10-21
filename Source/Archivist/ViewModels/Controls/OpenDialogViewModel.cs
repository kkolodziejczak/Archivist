using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// Type of dialog
    /// </summary>
    public enum OpenDialogType
    {
        OpenFileDialog,
        SaveFileDialog,
        OpenFolderDialog,
    }

    /// <summary>
    /// Choose folder, save file, open file dialog
    /// </summary>
    public class OpenDialogViewModel: BaseViewModel
    {

        /// <summary>
        /// Chosen path
        /// </summary>
        public string Path { get; set; }

        private string _Filter { get; set; }

        /// <summary>
        /// Type of dialog
        /// </summary>
        public OpenDialogType Type { get; private set; }

        /// <summary>
        /// Command that opens dialog
        /// </summary>
        public ICommand DialogCommand { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">What type dialog to open</param>
        public OpenDialogViewModel(OpenDialogType type)
            :this(type, String.Empty)
        {}

        /// <summary>
        /// Constructor with specified  Filter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Filter"></param>
        public OpenDialogViewModel(OpenDialogType type, string filter)
        {
            // Init properties
            this.Type = type;
            this.Path = String.Empty;
            this._Filter = filter;

            // Create command
            DialogCommand = new RelayCommand(OpenDialog);
        }

        /// <summary>
        /// Opens Dialog
        /// </summary>
        public void OpenDialog()
        {
            switch (Type)
            {
                case OpenDialogType.OpenFileDialog:

                    var FileDialog = new OpenFileDialog()
                    {
                        Filter = _Filter
                    };

                    if (FileDialog.ShowDialog() == true)
                    {
                        Path = FileDialog.FileName;
                    }
                    break;
                case OpenDialogType.SaveFileDialog:

                    var SaveDialog = new SaveFileDialog()
                    {
                        Filter = _Filter
                    };

                    if (SaveDialog.ShowDialog() == true)
                    {
                        Path = SaveDialog.FileName;
                    }
                    break;
                case OpenDialogType.OpenFolderDialog:

                    var OpenDialog = new System.Windows.Forms.FolderBrowserDialog();

                    if (OpenDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Path = OpenDialog.SelectedPath;
                    }
                    break;
            }
        }
    }
}
