﻿using Engine.Actions;
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

            BuildWeapon(1501, "Snake fangs", 0, 0, 2);
            BuildWeapon(1502, "Rat claws", 0, 0, 2);
            BuildWeapon(1503, "Spider fangs", 0, 0, 4);

            BuildHealingItem(2001, "Granola bar", 5, 2);

            BuildMiscellaneousItem(3001, "Oats", 1);
            BuildMiscellaneousItem(3002, "Honey", 2);
            BuildMiscellaneousItem(3003, "Raisins", 2);

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

        internal static string ItemName(int itemTypeId)
        {
            return _standardGameItems.SingleOrDefault(i => i.ItemTypeId == itemTypeId)?.Name ?? "";
        }

        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));
        }
        private static void BuildWeapon(int id, string name, int price, int minimumDamage, int maximumDamage)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);
            weapon.Action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);
            _standardGameItems.Add(weapon);
        }
        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem consumable = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            consumable.Action = new Heal(consumable, hitPointsToHeal);
            _standardGameItems.Add(consumable);
        }
    }
}
