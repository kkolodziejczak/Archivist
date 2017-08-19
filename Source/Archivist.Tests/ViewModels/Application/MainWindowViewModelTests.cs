using Microsoft.VisualStudio.TestTools.UnitTesting;
using Archivist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void MainWindowViewModelTest()
        {
            MainWindowViewModel window = new MainWindowViewModel();

            Assert.IsTrue(window.Page == Pages.Projects);
        }
    }
}