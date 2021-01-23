using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedstoneSidekick.Domain.MinecraftItems.CraftingTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.RSPFiles
{
    public class CraftingTreeItemConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ICraftingTreeItem);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var itemReader = jsonObject.CreateReader();
            ICraftingTreeItem item = new CraftingTreeSimpleItem();
            var hasIngredients = jsonObject.TryGetValue("Ingredients", out JToken ingredientsValue);

            if (hasIngredients)
            {
                item = new CraftingTreeCompoundItem();
            }

            serializer.Populate(itemReader, item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization");
        }
    }
}
