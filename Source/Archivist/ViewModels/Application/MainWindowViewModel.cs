using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ProjectsPage Page { get; set; } = new ProjectsPage();
    }
}
