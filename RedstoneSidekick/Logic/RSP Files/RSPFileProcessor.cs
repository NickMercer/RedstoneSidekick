using Microsoft.Win32;
using RedstoneSidekick.Data;
using RedstoneSidekick.Domain.Projects;
using RedstoneSidekick.Logic.ProjectStrings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace RedstoneSidekick.Logic.RSP_Files
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

        public static RedstoneSidekickProject LoadProjectFromFile()
        {
            RedstoneSidekickProject project = new RedstoneSidekickProject();

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Redstone Sidekick Projects (*.rsp)|*.rsp",
                InitialDirectory = $"{GlobalDataVars.AppDirectory}"
            };
            var success = dialog.ShowDialog();


            if(success == true)
            {
                string fileName = dialog.FileName;
                string fileString = File.ReadAllText(fileName);

                project = ProjectStringDecoder.Decode(fileString);

                if(project.ProjectName == "Untitled Project")
                {
                    string projectName = dialog.SafeFileName;
                    int extPos = projectName.LastIndexOf(".");
                    project.ProjectName = projectName.Substring(0, extPos);
                }
            }
            else
            {
                //TODO: Log Error.
                MessageBox.Show("File was not a valid .rsp file. Please try again", "File Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return project;
        }
    }
}
