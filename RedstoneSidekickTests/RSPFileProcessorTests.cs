using NUnit.Framework;
using RedstoneSidekick.Logic.RSPFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekickTests
{
    public class RSPFileProcessorTests
    {
        private const string _filePath = @"C:\Users\dekor\source\repos\RedstoneSidekick.NET5\RedstoneSidekick\RedstoneSidekick.WPF\bin\x64\Debug\net5.0-windows\diablo3logo.rsp";

 
        [Test]
        public void LoadProjectFromFile_ValidFile_CreatesProject()
        {
            var project = RSPFileProcessor.LoadProjectFromFile(_filePath, "diablo3logo.rsp");

            Assert.IsNotNull(project);
        }
    }
}
