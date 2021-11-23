using Engine.Factories;
using Engine.Models;
using Engine.Services;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass, IDisposable
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        #region Properties

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;
        private Battle _currentBattle;

        public string Version { get; } = "0.1.000";

        [JsonIgnore]
        public World CurrentWorld { get; }
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            private set
            {
                UnsubscribePlayer();
                _currentPlayer = value;
                SubscribePlayer();
            }
        }

        public Location CurrentLocation
        {
            get => _currentLocation;
            private set
            {
                _currentLocation = value;
                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                CurrentMonster = CurrentLocation.GetMonster();

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CurrentTrader = _currentLocation.TraderHere;
            }
        }
        [JsonIgnore]
        public Monster CurrentMonster
        {
            get => _currentMonster;
            private set
            {
                UnSubscribeBattle();
                _currentBattle = null;
                _currentMonster = value;
                SubscribeBattle();

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }
        [JsonIgnore]
        public Trader CurrentTrader
        {
            get => _currentTrader;
            private set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        [JsonIgnore]
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        [JsonIgnore]
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        [JsonIgnore]
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        [JsonIgnore]
        public bool HasMonster => CurrentMonster != null;
        [JsonIgnore]
        public bool HasTrader => CurrentTrader != null;

        #endregion

        public GameSession()
        {
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = new Player("Scott", "Fighter", 0, 10, 10, RandomNumberGenerator.NumberBetween(3, 18), 100000);

            if (!CurrentPlayer.Inventory.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.GetRecipeById(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }
        public GameSession(Player player, int xCoordinate, int yCoordinate)
        {
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = player;
            CurrentLocation = CurrentWorld.LocationAt(xCoordinate, yCoordinate);
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        public void Dispose()
        {
            UnsubscribePlayer();
            UnSubscribeBattle();
        }

        #region Private Functions

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (CurrentPlayer.GiveQuest(quest))
                {
                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest");
                    RaiseMessage(quest.Description);

                    RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }

                    RaiseMessage("And you will receive:");
                    RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"   {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }
                }
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.SingleOrDefault(q => q.PlayerQuest.Id == quest.Id && !q.IsCompleted);
                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);

                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");

                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                            RaiseMessage($"You receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        public void AttackCurrentMonster()
        {
            _currentBattle?.AttackOpponent();
        }

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed += OnConsumableActionPerformed;
                }
                CurrentPlayer.UseCurrentConsumable();
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed -= OnConsumableActionPerformed;
                }
            }
        }

        public void CrafItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);
                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                        RaiseMessage($"You craft 1 {outputItem.Name}");
                        CurrentPlayer.AddItemToInventory(outputItem);
                    }
                }
            }
            else
            {
                RaiseMessage("You do not have the required ingredients:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ItemId)}");
                }
            }
        }

        private void SubscribePlayer()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.OnKilled += OnPlayerKilled;
                _currentPlayer.OnLeveledUp += OnPlayerLeveledUp;
            }
        }
        private void UnsubscribePlayer()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.OnKilled -= OnPlayerKilled;
                _currentPlayer.OnLeveledUp -= OnPlayerLeveledUp;
            }
        }
        private void SubscribeBattle()
        {
            if (_currentMonster != null)
            {
                _currentBattle = new Battle(CurrentPlayer, CurrentMonster);
                _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
            }
        }
        private void UnSubscribeBattle()
        {
            if (_currentBattle != null)
            {
                _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                _currentBattle.Dispose();
            }
        }

        private void OnPlayerKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("");
            RaiseMessage($"You have been killed.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }

        private void OnPlayerLeveledUp(object sender, System.EventArgs e)
        {
            RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs e)
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        private void OnConsumableActionPerformed(object sender, string message)
        {
            RaiseMessage(message);
        }

        private void RaiseMessage(string message)
        {
            _messageBroker.RaiseMessage(message);
        }

        #endregion
    }
}
