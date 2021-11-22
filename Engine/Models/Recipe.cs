using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Recipe
    {
        private readonly List<ItemQuantity> _ingredients = new List<ItemQuantity>();
        private readonly List<ItemQuantity> _outputItems = new List<ItemQuantity>();

        public int Id { get; }
        [JsonIgnore]
        public string Name { get; }

        [JsonIgnore]
        public IEnumerable<ItemQuantity> Ingredients => _ingredients;
        [JsonIgnore]
        public IEnumerable<ItemQuantity> OutputItems => _outputItems;

        [JsonIgnore]
        public string ToolTipContents =>
            "Ingredients" + Environment.NewLine + "===========" + Environment.NewLine + string.Join(Environment.NewLine, Ingredients.Select(i => i.QuantityItemDescription)) + Environment.NewLine + Environment.NewLine +
            "Creates" + Environment.NewLine + "===========" + Environment.NewLine + string.Join(Environment.NewLine, OutputItems.Select(i => i.QuantityItemDescription));

        public Recipe(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddIngredient(int itemId, int quantity)
        {
            if (!_ingredients.Any(x => x.ItemId == itemId))
            {
                _ingredients.Add(new ItemQuantity(itemId, quantity));
            }
        }
        public void AddOutputItem(int itemId, int quantity)
        {
            if (!_outputItems.Any(x => x.ItemId == itemId))
            {
                _outputItems.Add(new ItemQuantity(itemId, quantity));
            }
        }
    }
}
