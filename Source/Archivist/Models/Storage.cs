using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Archivist
{

    /// <summary>
    /// Contains new window Title
    /// </summary>
    public class WindowNameArg : EventArgs
    {
        public string NewWindowName { get; set; }

        public WindowNameArg(string windowName):base()
        {
            this.NewWindowName = windowName;
        }
    }

    public static class Storage
    {
        #region Private Fields

        /// <summary>
        /// Path to Archivist data folder
        /// </summary>
        private static string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Archivist";

        /// <summary>
        /// Path where user settings are stored
        /// </summary>
        private static string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        /// <summary>
        /// Path where archives will be created by default
        /// </summary>
        public static string DefaultArchivePath = $@"{SettingsFolderPath}\Archives";

        /// <summary>
        /// User Settings
        /// </summary>
        private static UserSettings _Settings;

        #endregion

        #region Public Properties

        /// <summary>
        /// Path to directory what will be created for temprorary use
        /// </summary>
        public static string TemporaryDirectoryPath { get; } = $@"{SettingsFolderPath}\Temp";

        /// <summary>
        /// Users settings
        /// </summary>
        public static UserSettings Settings
        {
            get
            {
                if (_Settings == null)
                    Load();

                return _Settings;
            }
            private set
            {
                if (value == _Settings)
                    return;

                _Settings = value;

            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Delegate that allows to create event with new WindowTitle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void WindowsTitleUpdateEventHandler(object sender, WindowNameArg e);

        /// <summary>
        /// event that will be fired when selected project changes
        /// </summary>
        public static event WindowsTitleUpdateEventHandler OnWindowTitleUpdate;

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Fires <see cref="OnWindowTitleUpdate"/> event
        /// </summary>
        /// <param name="title"></param>
        public static void UpdateWindowTitle()
        {
            if (OnWindowTitleUpdate != null)
            {
                OnWindowTitleUpdate(new object(), new WindowNameArg(Settings.SelectedProject.Title));
            }
        }

        /// <summary>
        /// Loads <see cref="Settings"/> from file
        /// </summary>
        public static void Load()
        {

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));
            try
            {
                using (var sr = new StreamReader(new FileStream(SettingsFilePath,
                                                                FileMode.Open,
                                                                FileAccess.Read,
                                                                FileShare.ReadWrite)))
                {
                    Settings = xs.Deserialize(sr) as UserSettings;
                }
            }
            catch (Exception e)
            {
                // Some error with settings file, create new one from scratch.
                Settings = new UserSettings();
                Save();
            }
        }

        /// <summary>
        /// Sets default <see cref="UserSettings"/>
        /// </summary>
        public static void SetDefaultSettings()
        {
            Settings = new UserSettings();
        }

        /// <summary>
        /// Saves <see cref="Settings"/> into file
        /// </summary>
        public static void Save()
        {
            if (Settings == null)
                return;

            if (!Directory.Exists(Path.GetDirectoryName(SettingsFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            }

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));
            using (TextWriter tw = new StreamWriter(SettingsFilePath))
            {
                xs.Serialize(tw, Settings);
            }

        }

        #endregion
    }
}
