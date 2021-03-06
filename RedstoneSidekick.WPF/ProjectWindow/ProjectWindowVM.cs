﻿using Microsoft.Win32;
using Natick.Utilities.ViewModels;
using RedstoneSidekick;
using RedstoneSidekick.Data;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain.MinecraftItems;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using RedstoneSidekick.Domain.MinecraftItems.GatheringList;
using RedstoneSidekick.Domain.Projects;
using RedstoneSidekick.Logic.GatheringList;
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
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RedstoneSidekickWPF.ProjectWindow
{
    public class ProjectWindowVM : ViewModelBase
    {
        private ProjectWindow _window;

        private RedstoneSidekickProject _project = new RedstoneSidekickProject();
        public RedstoneSidekickProject Project
        {
            get { return _project; }
            set
            {
                SetProperty(ref _project, value);
            }
        }

        private ObservableCollection<IMinecraftItem> _minecraftItemList;
        public ObservableCollection<IMinecraftItem> MinecraftItemList
        {
            get { return _minecraftItemList; }
            set { SetProperty(ref _minecraftItemList, value); }
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

        public ProjectWindowVM(ProjectWindow window)
        {
            _window = window;

            new ItemDataUpdateHandler().RefreshAllData();

            var items = new MinecraftItemRepository().GetMinecraftItems();
            MinecraftItemList = new ObservableCollection<IMinecraftItem>(items);

            RegisterCommands();

            Project.CraftingTree = new ProjectCraftingTree();
            Project.GatheringList = new ProjectGatheringList(Project.CraftingTree.Items.ToList());
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
            _window.SwitchTabs(1);
            Project = new RedstoneSidekickProject();
        }

        internal void NewProjectFromStructure()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Structure/Schematic Files(*.nbt,*.schematic, *.schem)|*.nbt;*.schematic;*.schem",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft\\saves")
            };
            var dialogResult = dialog.ShowDialog();

            Mouse.OverrideCursor = Cursors.Wait;
            if (dialogResult == true)
            {
                _window.SwitchTabs(1);

                var filePath = dialog.FileName;

                StructureProcessor processor;
                try
                {
                    processor = StructureProcessorFactory.Build(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                try
                {
                    var project = processor.CreateProjectFromFile(filePath, dialog.SafeFileName);

                    if (project != null)
                    {
                        Project = project;
                    }
                    else
                    {
                        MessageBoxWindow window = new MessageBoxWindow()
                        {
                            Title = "File Does Not Exist",
                            MessageText = "File does not exist. Please try again",
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            ShowActivated = true
                        };
                        window.Show();
                    }
                }
                catch
                {
                    MessageBoxWindow window = new MessageBoxWindow()
                    {
                        Title = "File Format Not Supported",
                        MessageText = $"The schematic file that was loaded was either corrupt or not a supported schematic type. {Environment.NewLine}" +
                                        $"Redstone Sidekick currently supports Minecraft structure files and Sponge Specification .schem files (Like the ones used by the newest version of WorldEdit).",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ShowActivated = true
                    };
                    window.Show();
                }


            }
            Mouse.OverrideCursor = null;
        }

        internal void AddStructureToProject()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Structure/Schematic Files(*.nbt,*.schematic, *.schem)|*.nbt;*.schematic;*.schem",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft\\saves")
            };
            var dialogResult = dialog.ShowDialog();

            _window.SwitchTabs(1);

            Mouse.OverrideCursor = Cursors.Wait;
            if (dialogResult == true)
            {
                var filePath = dialog.FileName;

                StructureProcessor processor;
                try
                {
                    processor = StructureProcessorFactory.Build(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                try 
                { 
                    Project = processor.AddStructureToProject(Project, filePath, dialog.SafeFileName);
                }
                catch
                {
                    MessageBoxWindow window = new MessageBoxWindow()
                    {
                        Title = "File Format Not Supported",
                        MessageText = $"The schematic file that was loaded was either corrupt or not a supported schematic type. {Environment.NewLine}" +
                                        $"Redstone Sidekick currently supports Minecraft structure files and Sponge Specification .schem files (Like the ones used by the newest version of WorldEdit).",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ShowActivated = true
                    };
                    window.Show();
                }
            }
            Mouse.OverrideCursor = null;
        }

        internal void LoadProject()
        {
            RedstoneSidekickProject project;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Redstone Sidekick Projects (*.rsp)|*.rsp",
                InitialDirectory = $"{GlobalDataVars.AppDirectory}"
            };
            var success = dialog.ShowDialog();


            if (success == true)
            {
                _window.SwitchTabs(1);

                string filePath = dialog.FileName;
                string fileName = dialog.SafeFileName;

                project = RSPFileProcessor.LoadProjectFromFile(filePath, fileName);
                if (project != null)
                {
                    Project = project;
                }
            }
            else
            {
                //TODO: Log Error.
                MessageBoxWindow window = new MessageBoxWindow()
                {
                    Title = "File Invalid",
                    MessageText = "File was not a valid .rsp file. Please try again.",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowActivated = true
                };
                window.Show();
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

            _window.SwitchTabs(1);

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

            MessageBoxWindow window = new MessageBoxWindow()
            {
                Title = "Created Project Code",
                MessageText = "Redstone Sidekick Project Code copied to clipboard!",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowActivated = true
            };
            window.Show();

        }

        #endregion


        internal void RefreshCraftingTree()
        {
            if(Project.GatheringList.Items != null)
            {
                Project.CraftingTree.UpdateItemTree(Project.GatheringList.Items);
            }
        }

        internal void RefreshGatheringList()
        {
            if(Project.GatheringList == null)
            {
                Project.GatheringList = new ProjectGatheringList(Project.CraftingTree.Items);
            }
            else
            {
                Project.GatheringList.UpdateList(Project.CraftingTree.Items);
            }
        }

        private static ProjectCraftingTree CreateTestData()
        {
            var itemDictionary = new Dictionary<int, int>
            {
                { 10, 5 },
                { 2, 1 },
                { 157, 1000 },
                { 183, 40 },
                { 214, 1 },
                { 521, 12 },
                { 325, 1 },
                { 568, 14 },
                { 722, 1 },
                { 205, 7 }
            };

            var craftingTree = new ProjectCraftingTree(itemDictionary);
            return craftingTree;
        }
    }
}