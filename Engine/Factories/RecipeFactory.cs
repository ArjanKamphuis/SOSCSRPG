using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    internal class RecipeFactory
    {
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory()
        {
            Recipe granolaBar = new Recipe(1, "Granola bar");
            granolaBar.AddIngredient(3001, 1);
            granolaBar.AddIngredient(3002, 1);
            granolaBar.AddIngredient(3003, 1);
            granolaBar.AddOutputItem(2001, 1);
            _recipes.Add(granolaBar);
        }

        internal static Recipe RecipeById(int id)
        {
            return _recipes.SingleOrDefault(x => x.Id == id);
        }
    }
}
