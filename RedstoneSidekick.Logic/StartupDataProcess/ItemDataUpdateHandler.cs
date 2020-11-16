using Newtonsoft.Json;
using RedstoneSidekick.Data.CSVDataReaders;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedstoneSidekick.Logic.StartupDataProcess
{
    public class ItemDataUpdateHandler
    {
        private CSVFileVersion _itemDataVersion;

        private readonly MinecraftItemRepository _minecraftItemRepository = new MinecraftItemRepository();

        private readonly CSVFileVersionsRepository _csvFileVersionsRepository = new CSVFileVersionsRepository();

        private readonly MinecraftItemCSVReader _minecraftItemCSVReader = new MinecraftItemCSVReader();

        public void RefreshMinecraftItemData()
        {
            bool itemsRequireUpdate = CheckForMinecraftItemUpdate();
            if (!itemsRequireUpdate) return;


            var itemData = _minecraftItemCSVReader.LoadItemData();
            var itemsInserted = _minecraftItemRepository.InsertMinecraftItems(itemData);

            if (itemsInserted > 0)
            {
                _csvFileVersionsRepository.UpdateCSVFileVersion(_itemDataVersion);
            }
        }


        private bool CheckForMinecraftItemUpdate()
        {
            _itemDataVersion = GetVersionInfo();
            var dbVersion = _csvFileVersionsRepository.GetCSVFileVersion("ItemData.csv");

            return _itemDataVersion.Version != dbVersion.Version;
        }


        private CSVFileVersion GetVersionInfo()
        {
            var versionFilePath = "CSVFiles/versions.json";
            CSVFileVersion version;

            using (StreamReader reader = new StreamReader(versionFilePath))
            {
                string json = reader.ReadToEnd();
                version = JsonConvert.DeserializeObject<CSVFileVersion>(json);
            }

            return version;
        }
    }
}
