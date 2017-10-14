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
        /// Shortcut that is pressed
        /// </summary>
        private KeyboardShortcut _PressedShortcut;

        /// <summary>
        /// Shortcut that was recorded
        /// </summary>
        private KeyboardShortcut _RecordedShortcut;

        /// <summary>
        /// ShortcutSelected by user
        /// </summary>
        public List<KeyboardShortcut> _RegisteredShortcuts { get; private set; }

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


        /// <summary>
        /// Default constructor
        /// </summary>
        private KeyboardShortcutManager()
        {
            // Init Properties
            _PressedShortcut = new KeyboardShortcut();

            // Prepare Keyboard listener
            _KeyboardListener = new LowLevelKeyboardListener();
            _KeyboardListener.OnKeyUp += OnKeyUp;
            _KeyboardListener.OnKeyDown += OnKeyDown;

            // Start to listen
            _KeyboardListener.HookKeyboard();

        }

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

        /// <summary>
        /// OnKeyDown Event Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyPressedArgs e)
        {
            SwitchKey(e.KeyPressed, true);

            foreach (var registeredShortcut in _RegisteredShortcuts)
            {
                if (AreShortcutsTheSame(registeredShortcut, _PressedShortcut))
                {
                    registeredShortcut.Execute();
                }
            }

        }
        
        /// <summary>
        /// Returns if KeyboardShortcuts contains the same shortcut key combination
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool AreShortcutsTheSame(KeyboardShortcut left, KeyboardShortcut right)
        {
            if (left is null || right is null)
                return false;

            return (left.Alt == right.Alt &&
                    left.Ctrl == right.Ctrl &&
                    left.Shift == right.Shift &&
                    left.Key == right.Key);
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
        /// <param name="value">Switch key On(true) or Off(false)</param>
        private void SwitchKey(Key key, bool value)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    _PressedShortcut.Ctrl = value;
                    break;

                case Key.LeftAlt:
                case Key.RightAlt:
                    _PressedShortcut.Alt = value;
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    _PressedShortcut.Shift = value;
                    break;

                default:
                    if(value == true)
                    {
                        _PressedShortcut.Key = key;
                    }
                    else
                    {

                        if(_IsRecording == true)
                        {
                            _RecordedShortcut = new KeyboardShortcut(_PressedShortcut);
                            _IsRecording = false;
                        }

                        _PressedShortcut.Key = Key.None;

                    }
                    break;
            }
        }

        /// <summary>
        /// Returns recorded Shortcut
        /// </summary>
        /// <returns></returns>
        public async Task<KeyboardShortcut> RecordKeyboardShortcut()
        {
            _IsRecording = true;

            // While recording wait for user to press key combination
            while (_IsRecording)
            {
                // Wait for user
                await Task.Delay(50);
            }

            // Return new Shortcut;
            return _RecordedShortcut;
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
        public void UnregisterKeyboardShortcut(KeyboardShortcut shortcut)
        {
            if (_RegisteredShortcuts == null)
                return;

            if (_RegisteredShortcuts.Contains(shortcut))
            {
                _RegisteredShortcuts.Remove(shortcut);
            }
        }

    }
}
