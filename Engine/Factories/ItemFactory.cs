using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    internal static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        static ItemFactory()
        {
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 1, 3);
            BuildMiscellaneousItem(9001, "Snake fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat tail", 1);
            BuildMiscellaneousItem(9004, "Rat fur", 2);
            BuildMiscellaneousItem(9005, "Spider fang", 1);
            BuildMiscellaneousItem(9006, "Spiders silk", 2);
        }

        internal static GameItem CreateGameItem(int itemTypeId)
        {
            return _standardGameItems.SingleOrDefault(item => item.ItemTypeId == itemTypeId)?.Clone();
        }

        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));
        }
        private static void BuildWeapon(int id, string name, int price, int minimumDamage, int maximumDamage)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true, minimumDamage, maximumDamage));
        }
    }
}
