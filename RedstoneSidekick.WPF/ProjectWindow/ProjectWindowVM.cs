using NatickCommon.WPF;
using RedstoneSidekick;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.Projects;
using RedstoneSidekick.Logic.ProjectStrings;
using RedstoneSidekick.Logic.RSP_Files;
using RedstoneSidekick.Logic.StartupDataProcess;
using RedstoneSidekickWPF.Commands;
using RedstoneSidekickWPF.ProjectWindow.PopUps;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RedstoneSidekickWPF.ProjectWindow
{
    public class ProjectWindowVM : ViewModelBase
    {

        private string _projectName = "New Project";
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        private ProjectCraftingTree _craftingTree;
        public ProjectCraftingTree CraftingTree
        {
            get { return _craftingTree; }
            set { SetProperty(ref _craftingTree, value); }
        }


        #region Commands

        public NewProjectCommand NewProjectCommand { get; set; }
        public LoadProjectCommand LoadProjectCommand { get; set; }
        public SaveProjectCommand SaveProjectCommand { get; set; }
        public LoadProjectCodeCommand LoadProjectCodeCommand { get; set; }
        public CreateProjectCodeCommand CreateProjectCodeCommand { get; set; }
        #endregion

        public ProjectWindowVM()
        {
            new ItemDataUpdateHandler().RefreshAllData();

            RegisterCommands();

            CraftingTree = CreateTestData();
        }

        private void RegisterCommands()
        {
            NewProjectCommand = new NewProjectCommand(this);
            LoadProjectCommand = new LoadProjectCommand(this);
            SaveProjectCommand = new SaveProjectCommand(this);
            LoadProjectCodeCommand = new LoadProjectCodeCommand(this);
            CreateProjectCodeCommand = new CreateProjectCodeCommand(this);
        }

        #region Menu Command Implementations

        internal void NewProject()
        {
            ProjectName = "New Project";
            CraftingTree = new ProjectCraftingTree();
        }

        internal void LoadProject()
        {
            var project = RSPFileProcessor.LoadProjectFromFile();
            ProjectName = project.ProjectName;
            CraftingTree = project.CraftingTree;
        }

        internal void SaveProject()
        {
            var project = new RedstoneSidekickProject
            {
                ProjectName = ProjectName,
                CraftingTree = CraftingTree
            };

            var fileName = RSPFileProcessor.SaveProjectToFile(project);

            if(fileName != "Error")
            {
                int extPos = fileName.LastIndexOf(".");
                ProjectName = fileName.Substring(0, extPos);
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
            ProjectName = project.ProjectName;
            CraftingTree = project.CraftingTree;
        }

        internal void CreateProjectCode()
        {
            var project = new RedstoneSidekickProject
            {
                ProjectName = ProjectName,
                CraftingTree = CraftingTree
            };

            var projectCode = ProjectStringEncoder.Encode(project, ProjectStringEncoder.CraftingTreeOptions.Light);
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