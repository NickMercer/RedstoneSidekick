using Newtonsoft.Json;
using RedstoneSidekick.Data.CSVDataReaders;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Logic.StartupDataProcess
{
    public class ItemDataUpdateHandler
    {
        private readonly List<CSVFileVersion> _csvVersions;
        private readonly CSVFileVersionsRepository _csvFileVersionsRepository = new CSVFileVersionsRepository();


        private readonly MinecraftItemCSVReader _minecraftItemCSVReader = new MinecraftItemCSVReader();
        private readonly MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();


        private readonly CraftingRecipeCSVReader _craftingRecipeCSVReader = new CraftingRecipeCSVReader();
        private readonly CraftingRecipeRepository _craftingRecipeRepository = new CraftingRecipeRepository();

        private readonly SmeltingRecipeCSVReader _smeltingRecipeCSVReader = new SmeltingRecipeCSVReader();
        private readonly SmeltingRecipeRepository _smeltingRecipeRepository = new SmeltingRecipeRepository();

        public ItemDataUpdateHandler()
        {
            _csvVersions = GetVersionInfo();
        }

        public void RefreshAllData()
        {
            RefreshMinecraftItemData("ItemData.csv");
            RefreshCraftingData("ItemCraftingRecipes.csv");
            RefreshSmeltingData("ItemSmeltingData.csv");
        }



        public void RefreshMinecraftItemData(string fileName)
        {
            if (FileRequiresUpdate(fileName) == false) return;

            var itemData = _minecraftItemCSVReader.LoadItemData();
            var itemsInserted = _minecraftItemRepository.InsertMinecraftItems(itemData);

            UpdateFileVersion(fileName, itemsInserted);
        }



        public void RefreshCraftingData(string fileName)
        {
            if (FileRequiresUpdate(fileName) == false) return;

            var craftingData = _craftingRecipeCSVReader.LoadCraftingRecipes();
            var recipesInserted = _craftingRecipeRepository.InsertCraftingRecipes(craftingData);

            UpdateFileVersion(fileName, recipesInserted);
        }



        private void RefreshSmeltingData(string fileName)
        {
            if (FileRequiresUpdate(fileName) == false) return;

            var smeltingData = _smeltingRecipeCSVReader.LoadSmeltingRecipes();
            var recipesInserted = _smeltingRecipeRepository.InsertSmeltingRecipes(smeltingData);

            UpdateFileVersion(fileName, recipesInserted);
        }

        private bool FileRequiresUpdate(string fileName)
        {
            var fileVersion = _csvVersions.Where(x => x.FileName == fileName).FirstOrDefault();

            bool fileRequiresUpdate = CheckForUpdate(fileName, fileVersion);
            return fileRequiresUpdate;
        }
        private bool CheckForUpdate(string file, CSVFileVersion fileVersion)
        {
            var dbVersion = _csvFileVersionsRepository.GetCSVFileVersion(file);

            return fileVersion.Version != dbVersion.Version;
        }
        private void UpdateFileVersion(string fileName, int rowsInserted)
        {
            if (rowsInserted <= 0) return;

            var fileVersion = _csvVersions.Where(x => x.FileName == fileName).FirstOrDefault();
            _csvFileVersionsRepository.UpdateCSVFileVersion(fileVersion);
        }

        private List<CSVFileVersion> GetVersionInfo()
        {
            var versionFilePath = "Data/CSVFiles/versions.json";
            List<CSVFileVersion> version;

            using (StreamReader reader = new StreamReader(versionFilePath))
            {
                string json = reader.ReadToEnd();

                version = JsonConvert.DeserializeObject<List<CSVFileVersion>>(json);
            }

            return version;
        }
    }
}
