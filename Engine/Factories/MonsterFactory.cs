using Engine.Models;
using Engine.Services;
using Engine.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Factories
{
    internal static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";

        private static readonly GameDetails s_gameDetails = GameDetailsService.ReadGameDetails();
        private static readonly List<Monster> _baseMonsters = new();

        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath = data.SelectSingleNode("/Monsters").AttributeAsString("RootImagePath");
                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                List<PlayerAttribute> attributes = s_gameDetails.PlayerAttributes;
                attributes.Single(a => a.Key.Equals("DEX", StringComparison.Ordinal)).BaseValue
                    = attributes.Single(a => a.Key.Equals("DEX", StringComparison.Ordinal)).ModifiedValue
                    = Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText, null);

                Monster monster = new(node.AttributeAsInt("Id"), node.AttributeAsString("Name"), $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                    node.AttributeAsInt("MaximumHitPoints"), attributes, ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponId")), node.AttributeAsInt("RewardXP"), node.AttributeAsInt("Gold"));

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("Id"), lootItemNode.AttributeAsInt("Percentage"));
                    }
                }

                _baseMonsters.Add(monster);
            }
        }

        internal static Monster GetMonster(int monsterId)
        {
            return _baseMonsters.SingleOrDefault(m => m.Id == monsterId)?.GetNewInstance();
        }
    }
}
