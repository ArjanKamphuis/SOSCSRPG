using Engine.Factories;
using Engine.Models;
using Engine.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.Services
{
    public static class SaveGameService
    {
        public static void Save(GameSession gameSession, string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(gameSession, Formatting.Indented));
        }

        public static GameSession LoadLastSaveOrCreateNew(string fileName)
        {
            if (!File.Exists(fileName))
            {
                FileNotFoundException ex = new($"Filename: {fileName}");
                LoggingService.Log(ex);
                throw ex;
            }

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(fileName));

                Player player = CreatePlayer(data);

                int x = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.XCoordinate)];
                int y = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.YCoordinate)];

                return new GameSession(player, x, y);
            }
            catch
            {
                FormatException ex = new($"Error readin: {fileName}");
                LoggingService.Log(ex);
                throw ex;
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            Player player = new((string)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Name)],
                                (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.ExperiencePoints)],
                                (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.MaximumHitPoints)],
                                (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.CurrentHitPoints)],
                                GetPlayerAttributes(data),
                                (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Gold)]);

            PopulatePlayerInventory(data, player);
            PopulatePlayerQuests(data, player);
            PopulatePlayerRecipes(data, player);
            PopulateCurrentItems(data, player);

            return player;
        }

        private static IEnumerable<PlayerAttribute> GetPlayerAttributes(JObject data)
        {
            List<PlayerAttribute> attributes = new();
            foreach (JToken itemToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Attributes)])
            {
                attributes.Add(new((string)itemToken[nameof(PlayerAttribute.Key)],
                    (string)itemToken[nameof(PlayerAttribute.DisplayName)],
                    (string)itemToken[nameof(PlayerAttribute.DiceNotation)],
                    (int)itemToken[nameof(PlayerAttribute.BaseValue)],
                    (int)itemToken[nameof(PlayerAttribute.ModifiedValue)]));
            }
            return attributes;
        }

        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            foreach (JToken itemToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Inventory)][nameof(Inventory.Items)])
            {
                int itemId = (int)itemToken[nameof(GameItem.ItemTypeId)];
                player.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }

        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            foreach (JToken questToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Quests)])
            {
                Quest quest = QuestFactory.GetQuestById((int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.Id)]);
                _ = player.GiveQuest(new QuestStatus(quest) { IsCompleted = (bool)questToken[nameof(QuestStatus.IsCompleted)] });
            }
        }

        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            foreach (JToken recipeToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Recipes)])
            {
                int recipeId = (int)recipeToken[nameof(Recipe.Id)];
                player.LearnRecipe(RecipeFactory.GetRecipeById(recipeId));
            }
        }

        private static void PopulateCurrentItems(JObject data, Player player)
        {
            JToken currentWeaponToken = data[nameof(GameSession.CurrentPlayer)][nameof(Player.CurrentWeapon)];
            if (currentWeaponToken.HasValues)
            {
                player.CurrentWeapon = player.Inventory.Weapons.FirstOrDefault(weapon => weapon.ItemTypeId == (int)currentWeaponToken[nameof(GameItem.ItemTypeId)]);
            }
            JToken currentConsumableToken = data[nameof(GameSession.CurrentPlayer)][nameof(Player.CurrentConsumable)];
            if (currentConsumableToken.HasValues)
            {
                player.CurrentConsumable = player.Inventory.Consumables.FirstOrDefault(consumable => consumable.ItemTypeId == (int)currentConsumableToken[nameof(GameItem.ItemTypeId)]);
            }
        }
    }
}
