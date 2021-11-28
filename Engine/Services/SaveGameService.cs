using Engine.Factories;
using Engine.Models;
using Engine.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
                return null;
            }

            try
            {
                JObject data = JObject.Parse(File.ReadAllText(fileName));

                Player player = CreatePlayer(data);

                int x = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.XCoordinate)];
                int y = (int)data[nameof(GameSession.CurrentLocation)][nameof(Location.YCoordinate)];

                return new GameSession(player, x, y);
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
                return null;
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            //string fileVersion = FileVersion(data);

            //Player player = fileVersion switch
            //{
            //    "0.1.000" => new Player((string)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Name)],
            //                            (string)data[nameof(GameSession.CurrentPlayer)][nameof(Player.CharacterClass)],
            //                            (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.ExperiencePoints)],
            //                            (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.MaximumHitPoints)],
            //                            (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.CurrentHitPoints)],
            //                            (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Dexterity)],
            //                            (int)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Gold)]),
            //    _ => throw new InvalidDataException($"File version '{fileVersion}' not recognized"),
            //};
            //PopulatePlayerInventory(data, player);
            //PopulatePlayerQuests(data, player);
            //PopulatePlayerRecipes(data, player);
            //PopulateCurrentItems(data, player);

            //return player;
            return null;
        }

        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);
            switch (fileVersion)
            {
                case "0.1.000":
                    foreach (JToken itemToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Inventory)][nameof(Inventory.Items)])
                    {
                        int itemId = (int)itemToken[nameof(GameItem.ItemTypeId)];
                        player.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version '{fileVersion}' not recognized");
            }
        }

        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);
            switch (fileVersion)
            {
                case "0.1.000":
                    foreach (JToken questToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Quests)])
                    {
                        Quest quest = QuestFactory.GetQuestById((int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.Id)]);
                        _ = player.GiveQuest(new QuestStatus(quest) { IsCompleted = (bool)questToken[nameof(QuestStatus.IsCompleted)] });
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version '{fileVersion}' not recognized");
            }
        }

        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);
            switch (fileVersion)
            {
                case "0.1.000":
                    foreach (JToken recipeToken in (JArray)data[nameof(GameSession.CurrentPlayer)][nameof(Player.Recipes)])
                    {
                        int recipeId = (int)recipeToken[nameof(Recipe.Id)];
                        player.LearnRecipe(RecipeFactory.GetRecipeById(recipeId));
                    }
                    break;
                default:
                    throw new InvalidDataException($"File version '{fileVersion}' not recognized");
            }
        }

        private static void PopulateCurrentItems(JObject data, Player player)
        {
            string fileVersion = FileVersion(data);
            switch (fileVersion)
            {
                case "0.1.000":
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
                    break;
                default:
                    throw new InvalidDataException($"File version '{fileVersion}' not recognized");
            }
        }

        private static string FileVersion(JObject data)
        {
            return (string)data[nameof(GameSession.GameDetails.Version)];
        }
    }
}
