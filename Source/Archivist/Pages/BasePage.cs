using System;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Archivist
{
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

        #region Private members
        
        /// <summary>
        /// View Model for this page
        /// </summary>
        private VM mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// View Model for this page
        /// </summary>
        public VM ViewModel
        {
            get { return mViewModel; }

            set
            {
                // If ViewModel did not change return
                if (mViewModel == value)
                    return;

                // set ViewModel
                mViewModel = value;

                // set DataContext to ViewModel
                DataContext = mViewModel;
            }
        } 
        #endregion

        public BasePage() : base()
        {
            // Set View Model
            ViewModel = new VM();
        }
    }
}
