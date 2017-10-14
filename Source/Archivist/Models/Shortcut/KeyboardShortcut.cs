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

        /// <summary>
        /// Event that is fired when shortcut criteria are meet
        /// </summary>
        public event EventHandler OnShortcutActivated;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KeyboardShortcut()
        {
            Clear();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public KeyboardShortcut(KeyboardShortcut source)
        {
            this.Key = source.Key;
            this.Alt = source.Alt;
            this.Ctrl = source.Ctrl;
            this.Shift = source.Shift;
            this.OnShortcutActivated = source.OnShortcutActivated;
        }

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

        /// <summary>
        /// Sets shortcut values to default
        /// </summary>
        public void Clear()
        {
            this.Key = Key.None;
            this.Alt = false;
            this.Ctrl = false;
            this.Shift = false;
        }

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

        
    }
}
