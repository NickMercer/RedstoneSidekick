using Microsoft.Win32;
using NatickCommon.WPF;
using RedstoneSidekick;
using RedstoneSidekick.Data;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.Projects;
using RedstoneSidekick.Logic.ProjectStrings;
using RedstoneSidekick.Logic.RSPFiles;
using RedstoneSidekick.Logic.StartupDataProcess;
using RedstoneSidekick.Logic.StructureFiles;
using RedstoneSidekickWPF.Commands;
using RedstoneSidekickWPF.ProjectWindow.PopUps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RedstoneSidekickWPF.ProjectWindow
{
    public class ProjectWindowVM : ViewModelBase
    {

        private RedstoneSidekickProject _project = new RedstoneSidekickProject();
        public RedstoneSidekickProject Project
        {
            get { return _project; }
            set
            {
                SetProperty(ref _project, value);
            }
        }




        #region Commands

        public NewProjectCommand NewProjectCommand { get; set; }
        public NewProjectFromStructureCommand NewProjectFromStructureCommand { get; set; }
        public AddStructureToProjectCommand AddStructureToProjectCommand { get; set; }
        public LoadProjectCommand LoadProjectCommand { get; set; }
        public SaveProjectCommand SaveProjectCommand { get; set; }
        public LoadProjectCodeCommand LoadProjectCodeCommand { get; set; }
        public CreateProjectCodeCommand CreateProjectCodeCommand { get; set; }
        #endregion

        public ProjectWindowVM()
        {
            new ItemDataUpdateHandler().RefreshAllData();

            RegisterCommands();

            Project.CraftingTree = CreateTestData();
        }

        private void RegisterCommands()
        {
            NewProjectCommand = new NewProjectCommand(this);
            NewProjectFromStructureCommand = new NewProjectFromStructureCommand(this);
            AddStructureToProjectCommand = new AddStructureToProjectCommand(this);
            LoadProjectCommand = new LoadProjectCommand(this);
            SaveProjectCommand = new SaveProjectCommand(this);
            LoadProjectCodeCommand = new LoadProjectCodeCommand(this);
            CreateProjectCodeCommand = new CreateProjectCodeCommand(this);
        }


        #region Menu Command Implementations

        internal void NewProject()
        {
            Project = new RedstoneSidekickProject();
        }

        internal void NewProjectFromStructure()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Structure Files(*.nbt)|*.nbt",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft\\saves")
            };
            var dialogResult = dialog.ShowDialog();

            Mouse.OverrideCursor = Cursors.Wait;
            if (dialogResult == true)
            {
                var filePath = dialog.FileName;
                var project = MinecraftStructureProcessor.CreateProjectFromStructureFile(filePath, dialog.SafeFileName);

                if (project != null)
                {
                    Project = project;
                }
            }
            Mouse.OverrideCursor = null;
        }

        internal void AddStructureToProject()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Structure Files(*.nbt)|*.nbt",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft\\saves")
            };
            var dialogResult = dialog.ShowDialog();

            Mouse.OverrideCursor = Cursors.Wait;
            if (dialogResult == true)
            {
                var filePath = dialog.FileName;
                Project = MinecraftStructureProcessor.AddStructureToProject(Project, filePath, dialog.SafeFileName);
            }
            Mouse.OverrideCursor = null;
        }

        internal void LoadProject()
        {
            RedstoneSidekickProject project = null;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Redstone Sidekick Projects (*.rsp)|*.rsp",
                InitialDirectory = $"{GlobalDataVars.AppDirectory}"
            };
            var success = dialog.ShowDialog();


            if (success == true)
            {
                string filePath = dialog.SafeFileName;
                string fileName = dialog.FileName;

                project = RSPFileProcessor.LoadProjectFromFile(filePath, fileName);
                if (project != null)
                {
                    Project = project;
                }
            }
            else
            {
                //TODO: Log Error.
                MessageBox.Show("File was not a valid .rsp file. Please try again", "File Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal void SaveProject()
        {
            var fileName = RSPFileProcessor.SaveProjectToFile(Project);

            if(fileName != "Error")
            {
                int extPos = fileName.LastIndexOf(".");
                Project.ProjectName = fileName.Substring(0, extPos);
            }
            
        }

        internal void LoadProjectCode()
        {
            var projectCodeDialog = new ProjectCodeInputWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            projectCodeDialog.Closing += ProjectCodeDialog_Closing;
            projectCodeDialog.ShowDialog();
        }
        private void ProjectCodeDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var codeDialog = (ProjectCodeInputWindow)sender;

            if (codeDialog.ProjectCode == null) return;

            var project = ProjectStringDecoder.Decode(codeDialog.ProjectCode);
            
            if(project != null)
            {
                Project = project;
            }
        }

        internal void CreateProjectCode()
        {
            var projectCode = ProjectStringEncoder.Encode(Project, ProjectStringEncoder.CraftingTreeOptions.Light);
            Clipboard.Clear();
            Clipboard.SetText(projectCode);

            MessageBox.Show("Redstone Sidekick Project Code copied to clipboard!", "Created Project Code", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion


        private static ProjectCraftingTree CreateTestData()
        {
            var itemDictionary = new Dictionary<int, int>
            {
                { 10, 5 },
                { 157, 1000 },
                { 183, 40 },
                { 214, 1 },
                { 521, 12 },
                { 325, 1 },
                { 568, 14 }
            };

            var craftingTree = new ProjectCraftingTree(itemDictionary);
            return craftingTree;
        }
    }
}