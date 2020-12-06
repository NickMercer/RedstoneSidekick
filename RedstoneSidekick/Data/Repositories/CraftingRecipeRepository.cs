using Dapper;
using RedstoneSidekick.Domain;
using RedstoneSidekick.Domain.Recipes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RedstoneSidekick.Data.Repositories
{
    internal class CraftingRecipeRepository : IRepository
    {

        public IEnumerable<CraftingRecipe> GetCraftingRecipes()
        {
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT * FROM CraftingRecipes";
                return conn.Query<CraftingRecipe>(commandString, new DynamicParameters());
            }
        }

        public IEnumerable<CraftingRecipe> GetRecipesByMinecraftId(string minecraftId)
        {
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT * FROM CraftingRecipes WHERE ResultItemMinecraftId = @MinecraftId;";
                return conn.Query<CraftingRecipe>(commandString, new { MinecraftId = minecraftId });
            }
        }

        public IEnumerable<CraftingIngredient> GetIngredientsByRecipeId(int recipeId)
        {
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT i.RecipeId, i.IngredientMinecraftId, i.Count, m.Id FROM CraftingIngredients i LEFT JOIN MinecraftItems m ON i.IngredientMinecraftId = m.MinecraftId WHERE RecipeId = @RecipeId;";
                return conn.Query<CraftingIngredient>(commandString, new { RecipeId = recipeId });
            }
        }


        public int InsertCraftingRecipes(IEnumerable<CraftingRecipe> craftingRecipes)
        {
            var createdRows = 0;
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                foreach(var recipe in craftingRecipes)
                {
                    var commandString = "INSERT INTO CraftingRecipes (ResultItemMinecraftId, ResultCount) " +
                                        "VALUES (@ResultMinecraftId, @ResultCount);";

                    conn.Execute(commandString, new { ResultMinecraftId = recipe.ResultItemMinecraftId, ResultCount = recipe.ResultCount });

                    var idSelectString = "SELECT MAX(Id) FROM CraftingRecipes";
                    var idLong = conn.ExecuteScalar(idSelectString);
                    int.TryParse(idLong.ToString(), out int id);


                    if (id > 0)
                    {
                        recipe.Id = id;
                    }

                    createdRows++;

                    foreach (var ingredient in recipe.Ingredients) 
                    {
                        var commandString2 = "INSERT INTO CraftingIngredients (RecipeId, IngredientMinecraftId, Count) " +
                                             "VALUES (@RecipeId, @IngredientId, @Count) " +
                                             "ON CONFLICT(RecipeId, IngredientMinecraftId) DO UPDATE SET RecipeId = @RecipeId, IngredientMinecraftId = @IngredientId, Count = @Count;";

                        conn.Execute(commandString2, new { RecipeId = recipe.Id, IngredientId = ingredient.IngredientMinecraftId, Count = ingredient.Count });
                    }
                }
            }


            return createdRows;
        }
    }
}
