using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    public class ProjectViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Project title
        /// </summary>
        public string Title { get; set; } = "Title";

        /// <summary>
        /// Project Path
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Project archive path
        /// </summary>
        public string ArchivePath { get; set; }

        #endregion


        public override string ToString()
        {
            return Title;
        }
    }
}
