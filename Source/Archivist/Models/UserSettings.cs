using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Archivist
{

    /// <summary>
    /// Class that stores all settings
    /// </summary>
    public class UserSettings
    {

        #region Private Fields
        
        /// <summary>
        /// Shortcut that fires create backup
        /// </summary>
        private KeyboardShortcut _BackupShortcut { get; set; }

        /// <summary>
        /// Default path to save archives
        /// </summary>
        private string _DefaultPath { get; set; }

        /// <summary>
        /// All projects that user has added
        /// </summary>
        private List<Project> _Projects { get; set; }

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Selected project
        /// </summary>
        public Project _SelectedProject { get; set; }

        /// <summary>
        /// Shortcut that fires create backup
        /// </summary>
        public KeyboardShortcut BackupShortcut
        {
            get
            {
                if (_BackupShortcut == null)
                {
                    _BackupShortcut = new KeyboardShortcut()
                    {
                        Key = Key.S,
                        Ctrl = true,
                    };
                    _BackupShortcut.OnShortcutActivated += Backup.Create;
                }

                return _BackupShortcut;
            }
            set
            {
                if (value == _BackupShortcut)
                    return;

                _BackupShortcut = value;
                _BackupShortcut.OnShortcutActivated += Backup.Create;
            }
        }

        /// <summary>
        /// Default path to save archives
        /// </summary>
        public string DefaultPath
        {
            get
            {
                if (_DefaultPath == String.Empty || _DefaultPath == null)
                    return Storage.DefaultArchivePath;

                return _DefaultPath;
            }
            set
            {
                _DefaultPath = value;
            }
        }

        /// <summary>
        /// All projects that user has added
        /// </summary>
        public List<Project> Projects
        {
            get
            {
                if (_Projects == null)
                    _Projects = new List<Project>();

                return _Projects;
            }
            set
            {
                if (_Projects == value)
                    return;

                _Projects = value;
            }
        }

        /// <summary>
        /// Selected project
        /// </summary>
        [XmlIgnore]
        public Project SelectedProject
        {
            get
            {
                return _SelectedProject;
            }
            set
            {
                if (_SelectedProject == value)
                    return;

                _SelectedProject = value;

                // Update window title
                Storage.UpdateWindowTitle();
            }
        } 

        #endregion

    }
}
