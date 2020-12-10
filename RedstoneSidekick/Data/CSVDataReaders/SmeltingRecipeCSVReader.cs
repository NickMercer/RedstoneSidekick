using CsvHelper;
using RedstoneSidekick.Data.Repositories;
using RedstoneSidekick.Domain.Recipes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace RedstoneSidekick.Data.CSVDataReaders
{
    public class SmeltingRecipeCSVReader
    {
        public const string SmeltingRecipeFilePath = "Data/CSVFiles/ItemSmeltingData.csv";

        private MinecraftItemRepository _minecraftItemRepository;
        private Dictionary<string, int> _minecraftIdtoIdDictionary;
        private Dictionary<string, string> _conversionDictionary;

        public IEnumerable<SmeltingRecipe> LoadSmeltingRecipes()
        {
            try
            {
                return ReadSmeltingDataCSV(SmeltingRecipeFilePath);
            }
            catch
            {
                throw new IOException($"Smelting data loaded incorrectly while attempting to read from {SmeltingRecipeFilePath}. Make sure the file exists and is not corrupted.");
            }
        }

        private IEnumerable<SmeltingRecipe> ReadSmeltingDataCSV(string filePath)
        {
            _minecraftItemRepository = new MinecraftItemRepository();
            _minecraftIdtoIdDictionary = _minecraftItemRepository.GetMinecraftIdToIdDictionary();
            _conversionDictionary = new ConversionDictionaryRepository().GetConversionDictionary();

            using (var streamReader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                var smeltingRecipes = new List<SmeltingRecipe>();

                csvReader.Read();
                csvReader.ReadHeader();
                while(csvReader.Read())
                {
                    smeltingRecipes.Add(ConvertLineToSmeltingRecipe(csvReader));
                }

                return smeltingRecipes;
            }
        }

        private SmeltingRecipe ConvertLineToSmeltingRecipe(CsvReader csvReader)
        {
            var resultMinecraftId = csvReader.GetField("ItemId");
            var ingredientMinecraftId = csvReader.GetField("IngredientId");
            var primaryMethod = csvReader.GetField("PrimaryMethod");

            var recipe = new SmeltingRecipe
            {
                ResultId = GetItemId(resultMinecraftId),
                IngredientId = GetItemId(ingredientMinecraftId)
            };

            return recipe;
        }

        private int GetItemId(string minecraftId)
        {
            var id = 0;

            if (_conversionDictionary.ContainsKey(minecraftId))
            {
                minecraftId = _conversionDictionary[minecraftId];
            }

            if (_minecraftIdtoIdDictionary.ContainsKey(minecraftId))
            {
                id = _minecraftIdtoIdDictionary[minecraftId];
            }

            return id;
        }
    }
}
