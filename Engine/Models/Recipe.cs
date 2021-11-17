using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Recipe
    {
        private readonly List<ItemQuantity> _ingredients = new List<ItemQuantity>();
        private readonly List<ItemQuantity> _outputItems = new List<ItemQuantity>();

        public int Id { get; }
        public string Name { get; }

        public IEnumerable<ItemQuantity> Ingredients => _ingredients;
        public IEnumerable<ItemQuantity> OutputItems => _outputItems;

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
