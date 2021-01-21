using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.StructureFiles
{
    public interface IStructureProcessor
    {
        public RedstoneSidekickProject CreateProjectFromFile(string filePath, string fileName);

        public RedstoneSidekickProject AddStructureToProject(RedstoneSidekickProject project, string filePath, string fileName);


    }
}
