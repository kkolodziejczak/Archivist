using System;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Archivist
{
    /// <summary>
    /// Types of pages that can be created
    /// </summary>
    public enum Pages
    {
        /// <summary>
        /// Projects page
        /// </summary>
        Projects,

        /// <summary>
        /// Settings page
        /// </summary>
        Settings,

        /// <summary>
        /// Info page
        /// </summary>
        Info
    }

    /// <summary>
    /// Base page class for all pages
    /// </summary>
    public class BasePage : UserControl
    {
    }

    /// <summary>
    /// Generic base page class with viewmodel
    /// </summary>
    /// <typeparam name="VM">ViewModel based on <see cref="BaseViewModel"/></typeparam>
    public class BasePage<VM> : BasePage
        where VM: BaseViewModel, new()
    {

        #region Private Fields
        
        /// <summary>
        /// View Model for this page
        /// </summary>
        private VM _ViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// View Model for this page
        /// </summary>
        public VM ViewModel
        {
            get { return _ViewModel; }

            set
            {
                // If ViewModel did not change return
                if (_ViewModel == value)
                    return;

                // set ViewModel
                _ViewModel = value;

                // set DataContext to ViewModel
                DataContext = _ViewModel;
            }
        }
        #endregion
        
        #region Constructor

        public BasePage() : base()
        {
            // Set View Model
            ViewModel = new VM();
        } 
        #endregion
    }
}
