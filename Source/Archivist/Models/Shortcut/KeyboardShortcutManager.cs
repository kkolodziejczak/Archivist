using DesktopWPFAppLowLevelKeyboardHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// Class that manages shortcuts.
    /// Manager is listening to keyboard strokes 
    /// when registered key combination appear will execute shortcuts action.
    /// </summary>
    public class KeyboardShortcutManager
    {
        #region Private Fields

        /// <summary>
        /// Listener that allows to perform sertent action on Key Presses.
        /// </summary>
        private LowLevelKeyboardListener _KeyboardListener;

        /// <summary>
        /// Singleton
        /// </summary>
        private static KeyboardShortcutManager _Instance;

        /// <summary>
        /// Indicate if <see cref="KeyboardShortcutManager"/> is recording new <see cref="KeyboardShortcut"/>.
        /// </summary>
        private bool _IsRecording;

        /// <summary>
        /// Shortcut type that will be replaced after recording
        /// </summary>
        private KeyboardShortcut _ShortcutToRecord;

        /// <summary>
        /// ShortcutSelected by user
        /// </summary>
        public List<KeyboardShortcut> _RegisteredShortcuts { get; private set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if Alt key is holded (Left and Right are the same)
        /// </summary>
        public bool IsAltPressed { get; private set; }

        /// <summary>
        /// Indicates if Ctrl key is holded (Left and Right are the same)
        /// </summary>
        public bool IsCtrlPressed { get; private set; }

        /// <summary>
        /// Indicates if Shift key is holded (Left and Right are the same)
        /// </summary>
        public bool IsShiftPressed { get; private set; }

        /// <summary>
        /// Key that is holded
        /// </summary>
        public Key KeyPressed { get; private set; }

        /// <summary>
        /// ShortcutManager Singleton instance
        /// </summary>
        public static KeyboardShortcutManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new KeyboardShortcutManager();

                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        private KeyboardShortcutManager()
        {
            
            // Prepare Keyboard listener
            _KeyboardListener = new LowLevelKeyboardListener();
            _KeyboardListener.OnKeyUp += OnKeyUp;
            _KeyboardListener.OnKeyDown += OnKeyDown;

            // Start to listen
            _KeyboardListener.HookKeyboard();

        }

        #endregion

        #region Destructor

        /// <summary>
        /// Default destructor
        /// </summary>
        ~KeyboardShortcutManager()
        {
            // Cleanup
            _KeyboardListener.OnKeyUp -= OnKeyUp;
            _KeyboardListener.OnKeyDown -= OnKeyDown;
            _KeyboardListener.UnHookKeyboard();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// OnKeyDown Event Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyPressedArgs e)
        {
            SwitchKey(e.KeyPressed, true);

            if (_IsRecording == true)
                return;

            foreach (var shortcut in _RegisteredShortcuts)
            {
                if (IsShortcutPressed(shortcut))
                {
                    shortcut.Execute();
                }
            }

        }

        /// <summary>
        /// Checks if shortcut is pressed 
        /// </summary>
        /// <param name="shortcut"></param>
        /// <returns></returns>
        private bool IsShortcutPressed(KeyboardShortcut shortcut)
        {
            return (IsAltPressed == shortcut.Alt &&
                    IsCtrlPressed == shortcut.Ctrl &&
                    IsShiftPressed == shortcut.Shift &&
                    KeyPressed == shortcut.Key);
        }

        /// <summary>
        /// OnKeyUp Event Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, KeyPressedArgs e)
        {
            SwitchKey(e.KeyPressed, false);
        }

        /// <summary>
        /// Changes value of keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SwitchKey(Key key, bool value)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    IsCtrlPressed = value;
                    break;
                case Key.LeftAlt:
                case Key.RightAlt:
                    IsAltPressed = value;
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    IsShiftPressed = value;
                    break;
                default:
                    if(value == false)
                    {
                        if (_IsRecording == true)
                        {
                            //TODO: Save Selected Shortcut in user Settings
                            UnRegisterKeyboardShortcut(_ShortcutToRecord);

                            var NewShortcut = new KeyboardShortcut(_ShortcutToRecord)
                            {
                                Key = key,
                                Alt = IsAltPressed,
                                Ctrl = IsCtrlPressed,
                                Shift = IsShiftPressed,
                            };

                            _RegisteredShortcuts.Add(NewShortcut);

                            _IsRecording = false;
                        }

                        KeyPressed = Key.None;
                    }
                    else
                    {
                        KeyPressed = key;
                    }
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets flag that indicates shortcut recording, the next combination of keys pressed by the user will be set as a shortcut
        /// </summary>
        /// <param name="shortcut">Shortcut with setted <see cref="OnShortcutActivated"/>.</param>
        public void RecordKeyboardShortcut(KeyboardShortcut shortcut)
        {
            _ShortcutToRecord = shortcut;
            _IsRecording = true;
        }

        /// <summary>
        /// Register shortcut to listen to.
        /// </summary>
        /// <param name="shortcut"></param>
        public void RegisterKeyboardShortcut(KeyboardShortcut shortcut)
        {
            if (_RegisteredShortcuts == null)
                _RegisteredShortcuts = new List<KeyboardShortcut>();

            _RegisteredShortcuts.Add(shortcut);
        }

        /// <summary>
        /// Removes shortcut from shortcut list
        /// </summary>
        /// <param name="shortcut"></param>
        public void UnRegisterKeyboardShortcut(KeyboardShortcut shortcut)
        {
            if (_RegisteredShortcuts == null)
                return;

            if (_RegisteredShortcuts.Contains(shortcut))
            {
                _RegisteredShortcuts.Remove(shortcut);
            }
        }

        #endregion
    }
}
