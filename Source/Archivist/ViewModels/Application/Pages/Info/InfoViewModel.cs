using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// ViewModel for InfoPage
    /// </summary>
    public class InfoViewModel :BaseViewModel
    {

        #region Private Fields

        /// <summary>
        /// Blog URL
        /// </summary>
        private readonly string _siteURL = "http://kkolodziejczak.net/";

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Current version of the application
        /// </summary>
        public string ApplicationVersion { get; set; }

        #endregion
        
        #region Commands

        /// <summary>
        /// Command that opens site
        /// </summary>
        public ICommand OpenSiteCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InfoViewModel()
        {
            // Create command
            OpenSiteCommand = new RelayCommand(OpenSite);

            // Get application version
            var version = Assembly.GetEntryAssembly().GetName().Version;
            ApplicationVersion = $"Version: {version.Major}.{version.MajorRevision}.{version.Minor}.{version.MinorRevision}";

        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens site from Url
        /// </summary>
        private void OpenSite()
        {
            Process.Start(_siteURL);
        }

        #endregion

    }
}
