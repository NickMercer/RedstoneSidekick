using CsvHelper;
using RedstoneSidekick.Domain.MinecraftItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.CSVDataReaders
{
    public class ConversionDictionaryCSVReader
    {
        public const string ConversionDictionaryFilePath = "Data/CSVFiles/ConversionDictionary.csv";

        public IEnumerable<ConversionItem> LoadConversionDictionary()
        {
            try
            {
                return ReadConversionDictionaryCSV(ConversionDictionaryFilePath);
            }
            catch
            {
                throw new IOException($"Item conversion data loaded incorrectly while attempting to read from {ConversionDictionaryFilePath}. Make sure the file exists and is not corrupted.");
            }
        }

        private IEnumerable<ConversionItem> ReadConversionDictionaryCSV(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.RegisterClassMap<MinecraftItemMap>();
                return csvReader.GetRecords<ConversionItem>().ToList();
            }
        }
    }
}
