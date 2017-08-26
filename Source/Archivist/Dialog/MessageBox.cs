using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    /// <summary>
    /// Results that <see cref="MessageBox"/> can return
    /// </summary>
    public enum MessageBoxResult
    {
        Yes,
        No
    }

    /// <summary>
    /// MessageBox allows to show DialogWindow.
    /// </summary>
    public static class MessageBox
    {
        /// <summary>
        /// Show dialog window with message and title
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(string message, string title)
        {
            //Create Proper ViewModel
            var vm = new DialogViewModel()
            {
                Title = title,
                Question = message,
                ButtonTextOk = "Yes",
                ButtonTextNo = "No"
            };

            // Create and show Dialog
            var DialogResult = new DialogWindow(vm).ShowDialog();

            // Return proper value
            if (DialogResult == true)
                return MessageBoxResult.Yes;
            else
                return MessageBoxResult.No;
        }
        
    }
}
