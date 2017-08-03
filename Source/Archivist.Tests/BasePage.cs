using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Archivist
{
    public class BasePage<VM> : UserControl
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

        public BasePage()
        {
            // Set View Model
            ViewModel = new VM();
        }
    }
}
