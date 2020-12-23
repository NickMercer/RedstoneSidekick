using CsvHelper;
using RedstoneSidekick.Domain.Recipes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.CSVDataReaders
{
    public class CraftingRecipeCSVReader
    {
        public const string CraftingRecipeFilePath = "Data/CSVFiles/ItemCraftingRecipes.csv";

        public IEnumerable<CraftingRecipe> LoadCraftingRecipes()
        {
            try
            {
                return ReadCraftingCSVData(CraftingRecipeFilePath);
            }
            catch
            {
                throw new IOException($"Crafting Recipe data loaded incorrectly while attempting to read from {CraftingRecipeFilePath}. Make sure the file exists and is not corrupted.");
            }

        }

        private IEnumerable<CraftingRecipe> ReadCraftingCSVData(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                var craftingList = new List<CraftingRecipe>();

                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    craftingList.Add(ConvertLineToCraftingRecipe(csvReader));
                }

                return craftingList;
            }
        }

        private CraftingRecipe ConvertLineToCraftingRecipe(CsvReader csvReader)
        {
            var resultItem = csvReader.GetField("ItemId");
            int.TryParse(csvReader.GetField("ResultCount"), out int resultCount);

            var ingredientIds = csvReader.GetField("IngredientId").Split('|');
            var ingredientCounts = csvReader.GetField("IngredientCounts").Split('|');

            var ingredients = new List<CraftingIngredient>();
            
            for(var i = 0; i < ingredientIds.Length; i++)
            {
                var id = ingredientIds[i];
                int.TryParse(ingredientCounts[i], out int count);
                ingredients.Add(new CraftingIngredient { IngredientMinecraftId = id, Count = count });
            }


            var recipe = new CraftingRecipe
            {
                ResultMinecraftId = resultItem,
                ResultCount = resultCount,
                Ingredients = ingredients,
            };

            return recipe;
        }
    }
}
