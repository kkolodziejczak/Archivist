using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Archivist
{
    
    public class MainWindowViewModel : BaseViewModel
    {

        #region Public Properties
        
        /// <summary>
        /// Current page
        /// </summary>
        public Pages Page { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to switch to ProjectsPage
        /// </summary>
        public ICommand ProjectsCommand { get; set; }

        /// <summary>
        /// Command to switch to SettingsPage
        /// </summary>
        public ICommand SettingsCommand { get; set; }

        /// <summary>
        /// Command to switch to ReportPage
        /// </summary>
        public ICommand ReportCommand { get; set; }

        /// <summary>
        /// Command to switch to InfoPage
        /// </summary>
        public ICommand InfoCommand { get; set;  }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Set Starting page
            Page = Pages.Projects;

            // Create commands
            ProjectsCommand = new RelayCommand(async () => await SwitchPage(Pages.Projects));
            SettingsCommand = new RelayCommand(async () => await SwitchPage(Pages.Settings));
            ReportCommand = new RelayCommand(async () => await SwitchPage(Pages.Report));
            InfoCommand = new RelayCommand(async () => await SwitchPage(Pages.Info));
        }

        #endregion

        #region Tasks

        /// <summary>
        /// Switches page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task SwitchPage(Pages page)
        {
            Page = page;
            await Task.Delay(1);
        } 

        #endregion
    }
}
