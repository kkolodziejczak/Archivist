using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// ViewModel representing <see cref="ProjectModel"/>.
    /// </summary>
    public class ProjectItemControlViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Information about project
        /// </summary>
        private ProjectModel m_Project;

        #endregion

        #region Public Properties

        /// <summary>
        /// Project's title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Information about project
        /// </summary>
        public ProjectModel Project
        {
            get => m_Project;
            set
            {
                if (Project == value)
                    return;

                m_Project = value;

                Title = value.Title;
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate that is representing event after buttons are clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ButtonHandler(object sender, EventArgs e);

        #endregion

        #region Events

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
        public ICommand DeleteButtonCommand { get; private set; }

        /// <summary>
        /// Command that is called when active button is pressed
        /// </summary>
        public ICommand ActiveButtonCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProjectItemControlViewModel()
        {
            // Creating commands
            EditButtonCommand = new RelayCommand(async () => await EditButton());
            DeleteButtonCommand = new RelayCommand(async () => await DeleteButton());
            ActiveButtonCommand = new RelayCommand(async() => await ActiveButton());
        }

        #endregion

        #region Tasks

        /// <summary>
        /// Method called when edit button is pressed
        /// </summary>
        /// <returns></returns>
        private async Task EditButton()
        {
            // Get event handler
            ButtonHandler handler = OnEditButtonClick;

            // If there is no method attached don't do anything
            if (handler != null)
            {
                OnEditButtonClick(this, new EventArgs());
            }

            //Because of async
            await Task.Delay(1);
        }

        /// <summary>
        /// Method called when delete button is pressed
        /// </summary>
        /// <returns></returns>
        private async Task DeleteButton()
        {
            // Get event handler
            ButtonHandler handler = OnDeleteButtonClick;

            // If there is no method attached don't do anything
            if(handler != null)
            {
                OnDeleteButtonClick(this, new EventArgs());
            }

            // Because of async
            await Task.Delay(1);
        }

        /// <summary>
        /// Method called when active button is pressed
        /// </summary>
        /// <returns></returns>
        private async Task ActiveButton()
        {
            // Get event handler
            ButtonHandler handler = OnActiveProjectClick;

            // If there is no method attached don't do anything
            if (handler != null)
            {
                OnActiveProjectClick(this, new EventArgs());
            }

            // Because of async
            await Task.Delay(1);
        }

        #endregion

    }
}
