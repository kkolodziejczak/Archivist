using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Archivist
{

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

        private static string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Archivist";

        private static string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        private static UserSettings _Settings;


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

        /// <summary>
        /// Path to directory what will be created for temprorary use
        /// </summary>
        public static string TemporaryDirectoryPath { get; } = @"C:\\Temp";

        /// <summary>
        /// Settings file path
        /// </summary>
        public static string SettingsPath { get; } = @"C:\Users\Themo\Desktop\Settings.ini";
        
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

                Save();
            }
        }

        /// <summary>
        /// Fires <see cref="OnWindowTitleUpdate"/> event
        /// </summary>
        /// <param name="title"></param>
        public static void UpdateWindowTitle()
        {
            if(OnWindowTitleUpdate != null)
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
                using (var sr = new StreamReader(new FileStream(SettingsPath,
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
        /// Saves <see cref="Settings"/> into file
        /// </summary>
        public static void Save()
        {
            if (Settings == null)
                return;

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));

            using (TextWriter tw = new StreamWriter(SettingsPath))
            {
                xs.Serialize(tw, Settings);
            }

        }

    }
}
