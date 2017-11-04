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

        #region Private Fields
        
        /// <summary>
        /// Open file filter
        /// </summary>
        private string _Filter { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Chosen path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Type of dialog
        /// </summary>
        public OpenDialogType Type { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command that opens dialog
        /// </summary>
        public ICommand DialogCommand { get; private set; }

        #endregion

        #region Events
        
        /// <summary>
        /// Event that fires whenever path is updated
        /// </summary>
        public event EventHandler OnPathUpdated;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">What type dialog to open</param>
        public OpenDialogViewModel(OpenDialogType type)
            : this(type, String.Empty)
        { }

        /// <summary>
        /// Constructor with specified  Filter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Filter"></param>
        public OpenDialogViewModel(OpenDialogType type, string filter)
            : this(type, filter, String.Empty)
        { }

        /// <summary>
        /// Constructor with specified Filter and starting path
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Filter"></param>
        public OpenDialogViewModel(OpenDialogType type, string filter, string path)
        {
            // Init properties
            this.Type = type;
            this.Path = path;
            this._Filter = filter;

            // Create command
            DialogCommand = new RelayCommand(OpenDialog);
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Opens Dialog
        /// </summary>
        public void OpenDialog()
        {
            bool PathChanged = false;

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
                        PathChanged = true;
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
                        PathChanged = true;
                    }
                    break;
                case OpenDialogType.OpenFolderDialog:

                    var OpenDialog = new System.Windows.Forms.FolderBrowserDialog();

                    if (OpenDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Path = OpenDialog.SelectedPath;
                        PathChanged = true;
                    }
                    break;
            }

            // Call Settings that path has changed
            if (OnPathUpdated != null && PathChanged == true)
                OnPathUpdated(this, new EventArgs());
        } 

        #endregion
    }
}
