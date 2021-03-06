using Engine.Actions;
using Engine.Models;
using Engine.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        private static readonly List<GameItem> _standardGameItems = new();

        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        public static GameItem CreateGameItem(int itemTypeId)
        {
            return _standardGameItems.SingleOrDefault(item => item.ItemTypeId == itemTypeId)?.Clone();
        }

        public static string ItemName(int itemTypeId)
        {
            return _standardGameItems.SingleOrDefault(i => i.ItemTypeId == itemTypeId)?.Name ?? "";
        }

        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);
                GameItem gameItem = new(itemCategory, node.AttributeAsInt("Id"), node.AttributeAsString("Name"), node.AttributeAsInt("Price"), itemCategory == GameItem.ItemCategory.Weapon);

                if (itemCategory == GameItem.ItemCategory.Weapon)
                {
                    gameItem.Action = new AttackWithWeapon(gameItem, node.AttributeAsString("DamageDice"));
                }
                else if (itemCategory == GameItem.ItemCategory.Consumable)
                {
                    gameItem.Action = new Heal(gameItem, node.AttributeAsInt("HitPointsToHeal"));
                }

                _standardGameItems.Add(gameItem);
            }
        }

        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            return itemType switch
            {
                "Weapon" => GameItem.ItemCategory.Weapon,
                "HealingItem" => GameItem.ItemCategory.Consumable,
                _ => GameItem.ItemCategory.Miscellaneous,
            };
        }
    }
}
