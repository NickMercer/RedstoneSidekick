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

        public ItemDataUpdateHandler()
        {
            _csvVersions = GetVersionInfo();
        }

        public void RefreshAllData()
        {
            RefreshMinecraftItemData();
            RefreshCraftingData();
        }

        #region Item Data

        public void RefreshMinecraftItemData()
        {
            var itemFile = "ItemData.csv";
            var itemVersion = _csvVersions.Where(x => x.FileName == itemFile).FirstOrDefault();

            bool itemsRequireUpdate = CheckForMinecraftItemUpdate(itemFile, itemVersion);
            if (!itemsRequireUpdate) return;


            var itemData = _minecraftItemCSVReader.LoadItemData();
            var itemsInserted = _minecraftItemRepository.InsertMinecraftItems(itemData);

            if (itemsInserted > 0)
            {
                _csvFileVersionsRepository.UpdateCSVFileVersion(itemVersion);
            }
        }


        private bool CheckForMinecraftItemUpdate(string fileName, CSVFileVersion itemVersion)
        {
            var dbVersion = _csvFileVersionsRepository.GetCSVFileVersion(fileName);

            return itemVersion.Version != dbVersion.Version;
        }


        #endregion

        #region Crafting Data

        public void RefreshCraftingData()
        {
            var craftingFile = "ItemCraftingRecipes.csv";
            var craftingVersion = _csvVersions.Where(x => x.FileName == craftingFile).FirstOrDefault();

            bool craftingRequiresUpdate = CheckForCraftingRecipeUpdate(craftingFile, craftingVersion);
            if (!craftingRequiresUpdate) return;

            var craftingRecipes = _craftingRecipeCSVReader.LoadCraftingRecipes();
            var recipesInserted = _craftingRecipeRepository.InsertCraftingRecipes(craftingRecipes);

            if (recipesInserted > 0)
            {
                _csvFileVersionsRepository.UpdateCSVFileVersion(craftingVersion);
            }
        }

        private bool CheckForCraftingRecipeUpdate(string craftingFile, CSVFileVersion craftingVersion)
        {
            var dbVersion = _csvFileVersionsRepository.GetCSVFileVersion(craftingFile);

            return craftingVersion.Version != dbVersion.Version;
        }

        #endregion

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
