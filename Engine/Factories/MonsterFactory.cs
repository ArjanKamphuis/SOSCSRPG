﻿using Engine.Models;
using System;

namespace Engine.Factories
{
    internal static class MonsterFactory
    {
        internal static Monster GetMonster(int monsterId)
        {
            switch (monsterId)
            {
                case 1:
                    Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1) { CurrentWeapon = ItemFactory.CreateGameItem(1501) };
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);
                    return snake;
                case 2:
                    Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1) { CurrentWeapon = ItemFactory.CreateGameItem(1502) };
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
                    return rat;
                case 3:
                    Monster giantSpider = new Monster("Giant Spider", "GiantSpider.png", 10, 10, 10, 3) { CurrentWeapon = ItemFactory.CreateGameItem(1503) };
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);
                    return giantSpider;
                default:
                    throw new ArgumentException($"MonsterType '{monsterId}' does not exist");
            }
        }

        private static void AddLootItem(Monster monster, int itemId, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }
    }
}
