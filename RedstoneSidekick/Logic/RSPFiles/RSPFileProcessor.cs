using Microsoft.Win32;
using RedstoneSidekick.Data;
using RedstoneSidekick.Domain.Projects;
using RedstoneSidekick.Logic.ProjectStrings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace RedstoneSidekick.Logic.RSPFiles
{
    public static class RSPFileProcessor
    {

        public static string SaveProjectToFile(RedstoneSidekickProject project)
        {
            string projectAsBase64String = ProjectStringEncoder.Encode(project, ProjectStringEncoder.CraftingTreeOptions.Full);
            string fileName = $"{project.ProjectName}.rsp";

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Redstone Sidekick Projects (*.rsp)|*.rsp",
                InitialDirectory = $"{GlobalDataVars.AppDirectory}",
                FileName = fileName,
                ValidateNames = true,
                DefaultExt = ".rsp",
                AddExtension = true
            };

            if (dialog.ShowDialog() == true)
            {
                fileName = dialog.FileName;

                File.WriteAllText(fileName, projectAsBase64String);

                return dialog.SafeFileName;
            }

            return "Error";
        }

        public static RedstoneSidekickProject LoadProjectFromFile(string filePath, string fileName)
        {
            RedstoneSidekickProject project = null;

            string fileString = File.ReadAllText(filePath);

            project = ProjectStringDecoder.Decode(fileString);

            if(project == null)
            {
                return project;
            }

            if(project.ProjectName == "Untitled Project")
            {
                string projectName = fileName;
                int extPos = projectName.LastIndexOf(".");
                project.ProjectName = projectName.Substring(0, extPos);
            }
            
            return project;
        }
    }
}
