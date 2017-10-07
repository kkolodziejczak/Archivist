using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Archivist
{
    public static class Storage
    {

        #region Private Fields

        private static string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Archivist";

        private static string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        private static UserSettings _Settings;

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads <see cref="Settings"/> from file
        /// </summary>
        public static void Load()
        {

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));
            try
            {
                using (var sr = new StreamReader(SettingsPath))
                {
                    Settings = (UserSettings)xs.Deserialize(sr);
                }
            }
            catch (InvalidOperationException e)
            {
                // Some error with settings file, create Default one.
                Settings = new UserSettings();
                Save();
            }
            catch (IOException e)
            {
                //TODO: find out why this error occurs, that something is reading file already,
                // even if it gets good data.
                Console.WriteLine(e.Message);
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

        #endregion

    }
}
