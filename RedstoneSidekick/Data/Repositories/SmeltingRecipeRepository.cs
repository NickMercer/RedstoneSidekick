using Dapper;
using RedstoneSidekick.Domain.Recipes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace RedstoneSidekick.Data.Repositories
{
    public class SmeltingRecipeRepository : IRepository
    {

        public IEnumerable<SmeltingRecipe> GetSmeltingRecipes()
        {
            using(IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            {
                var commandString = "SELECT * FROM SmeltingRecipes";
                return conn.Query<SmeltingRecipe>(commandString, new DynamicParameters());
            }
        }

        

        public int InsertSmeltingRecipes(IEnumerable<SmeltingRecipe> smeltingRecipes)
        {
            var createdRows = 0;
            using (IDbConnection conn = new SQLiteConnection(GlobalDataVars.SQLiteConnectionString))
            { 
                foreach (var recipe in smeltingRecipes)
                {
                    var commandString = "INSERT INTO SmeltingRecipes (IngredientId, ResultId) " +
                                            "VALUES (@IngredientId, @ResultId) " +
                                            "ON CONFLICT(IngredientId) DO UPDATE SET IngredientId = @IngredientId, ResultId = @ResultId;";

                    createdRows += conn.Execute(commandString, new { IngredientId = recipe.IngredientId, ResultId = recipe.ResultId });
                }
            }

            return createdRows;
        }
    }
}
