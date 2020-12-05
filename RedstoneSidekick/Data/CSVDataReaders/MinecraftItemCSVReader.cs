using CsvHelper;
using RedstoneSidekick.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.CSVDataReaders
{
    public class MinecraftItemCSVReader
    {
        public const string MinecraftItemFilePath = "Data/CSVFiles/ItemData.csv";

        public IEnumerable<MinecraftItem> LoadItemData()
        {
            try
            {
                return ReadItemDataCSV(MinecraftItemFilePath);
            }
            catch
            {
                throw new IOException($"Item data loaded incorrectly while attempting to read from {MinecraftItemFilePath}. Make sure the file exists and is not corrupted.");
            }
        }

        private IEnumerable<MinecraftItem> ReadItemDataCSV(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<MinecraftItemMap>();
                return csvReader.GetRecords<MinecraftItem>().ToList();
            }
        }
    }
}
