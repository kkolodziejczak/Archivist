using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
 
    /// <summary>
    /// Class that represents keyboard shortcut
    /// </summary>
    public class KeyboardShortcut
    {

        #region Public Properties
        
        /// <summary>
        /// Shotcut Key
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Indicates if Alt key is pressed
        /// </summary>
        public bool Alt { get; set; }

        /// <summary>
        /// Indicates if Ctrl key is pressed
        /// </summary>
        public bool Ctrl { get; set; }

        /// <summary>
        /// Indicates if Shift key is pressed
        /// </summary>
        public bool Shift { get; set; }

        #endregion
        
        #region Event

        /// <summary>
        /// Event that is fired when shortcut criteria are meet
        /// </summary>
        public event EventHandler OnShortcutActivated;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KeyboardShortcut()
        {

        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public KeyboardShortcut(KeyboardShortcut shortcut)
        {
            this.Key = shortcut.Key;
            this.Alt = shortcut.Alt;
            this.Ctrl = shortcut.Ctrl;
            this.Shift = shortcut.Shift;
            this.OnShortcutActivated = shortcut.OnShortcutActivated;
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Fires shortcut methods that are connected to <see cref="OnShortcutActivated"/>.
        /// </summary>
        public void Execute()
        {
            if (OnShortcutActivated != null)
            {
                OnShortcutActivated(this, new EventArgs());
            }
        }

        #endregion

        #region Override

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Ctrl)
                sb.Append("Ctrl + ");
            if (Alt)
                sb.Append("Alt + ");
            if (Shift)
                sb.Append("Shift + ");

            return $"{sb.ToString()}{Key.ToString()}";
        }

        #endregion

    }
}
