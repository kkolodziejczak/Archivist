using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Archivist
{
    /// <summary>
    /// ViewModel for Dialog
    /// </summary>
    public class DialogViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Title of dialog box
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message text to be displayed
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Text that is displayed on Accepting button
        /// </summary>
        public string ButtonTextOk { get; set; }

        /// <summary>
        /// Text that is displayed on Cancel button type
        /// </summary>
        public string ButtonTextNo { get; set; }

        /// <summary>
        /// Type of MessageBox
        /// </summary>
        public MessageBoxType messageBoxType {get; set;}

        #endregion

    }
}
