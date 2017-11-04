using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// ViewModel representing <see cref="Archivist.Project"/>.
    /// </summary>
    public class ProjectItemControlViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// Information about project
        /// </summary>
        private Project _Project;

        #endregion

        #region Public Properties

        /// <summary>
        /// Project's title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Information about project
        /// </summary>
        public Project Project
        {
            get => _Project;
            set
            {
                if (Project == value)
                    return;

                _Project = value;

                Title = value.Title;
            }
        }

        #endregion
       
        #region Events

        /// <summary>
        /// Delegate that is representing event after buttons are clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ButtonHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that will be fired when pressed edit button
        /// </summary>
        public event ButtonHandler OnEditButtonClick;

        /// <summary>
        /// Event that will be fired when pressed delete button
        /// </summary>
        public event ButtonHandler OnDeleteButtonClick;

        /// <summary>
        /// Event that will be fired when pressed active button
        /// </summary>
        public event ButtonHandler OnActiveProjectClick;

        #endregion

        #region Commands

        /// <summary>
        /// Command that is called when edit button is pressed
        /// </summary>
        public ICommand EditButtonCommand { get; private set; }

        /// <summary>
        /// Command that is called when delete button is pressed
        /// </summary>
        public ICommand ArchiveButtonCommand { get; private set; }

        /// <summary>
        /// Command that is called when delete button is pressed
        /// </summary>
        public ICommand DeleteButtonCommand { get; private set; }

        /// <summary>
        /// Command that is called when active button is pressed
        /// </summary>
        public ICommand ActiveButtonCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProjectItemControlViewModel()
        {
            // Creating commands
            EditButtonCommand = new RelayCommand(EditButton);
            DeleteButtonCommand = new RelayCommand(DeleteButton);
            ActiveButtonCommand = new RelayCommand(ActiveButton);
            ArchiveButtonCommand = new RelayCommand(OpenArchiveFolder);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Method called when edit button is pressed
        /// </summary>
        /// <returns></returns>
        private void EditButton()
        {
            // Get event handler
            ButtonHandler handler = OnEditButtonClick;

            // If there is no method attached don't do anything
            if (handler != null)
            {
                OnEditButtonClick(this, new EventArgs());
            }
        }

        /// <summary>
        /// Method called when delete button is pressed
        /// </summary>
        /// <returns></returns>
        private void DeleteButton()
        {
            // Get event handler
            ButtonHandler handler = OnDeleteButtonClick;

            // If there is no method attached don't do anything
            if (handler != null)
            {
                OnDeleteButtonClick(this, new EventArgs());
            }

        }

        /// <summary>
        /// Method called when active button is pressed
        /// </summary>
        /// <returns></returns>
        private void ActiveButton()
        {
            // Get event handler
            ButtonHandler handler = OnActiveProjectClick;

            // If there is no method attached don't do anything
            if (handler != null)
            {
                OnActiveProjectClick(this, new EventArgs());
            }
        }

        /// <summary>
        /// Opens projects archive folder
        /// </summary>
        private void OpenArchiveFolder()
        {
            Process.Start(_Project.ArchivePath);
        }

        #endregion

    }
}
