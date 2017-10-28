using Microsoft.VisualStudio.TestTools.UnitTesting;
using Archivist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        [TestMethod()]
        public void MainWindowViewModelTest()
        {
            MainWindowViewModel window = new MainWindowViewModel();

            Assert.IsTrue(window.Page == Pages.Projects);
        }

        [TestMethod()]
        public void SwitchToSettingsPageTest()
        {
            MainWindowViewModel window = new MainWindowViewModel();

            window.SettingsCommand.Execute(null);

            Assert.IsTrue(window.Page == Pages.Settings);
        }

        [TestMethod()]
        public void SwitchToProjectsPageTest()
        {
            MainWindowViewModel window = new MainWindowViewModel();

            window.ProjectsCommand.Execute(null);

            Assert.IsTrue(window.Page == Pages.Projects);
        }

        [TestMethod()]
        public void SwitchToInfoPageTest()
        {
            MainWindowViewModel window = new MainWindowViewModel();

            window.InfoCommand.Execute(null);

            Assert.IsTrue(window.Page == Pages.Info);
        }


    }
}