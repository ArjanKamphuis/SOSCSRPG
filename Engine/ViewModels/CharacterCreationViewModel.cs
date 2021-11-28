using Engine.Factories;
using Engine.Models;
using Engine.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.ViewModels
{
    public class CharacterCreationViewModel : BaseNotificationClass
    {
        private Race _selectedRace;

        public GameDetails GameDetails { get; } = GameDetailsService.ReadGameDetails();
        public string Name { get; set; }
        public Race SelectedRace
        {
            get => _selectedRace;
            set
            {
                _selectedRace = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PlayerAttribute> PlayerAttributes { get; } = new();

        public bool HasRaces => GameDetails.Races.Any();
        public bool HasRaceAttributeModifiers => HasRaces && GameDetails.Races.Any(r => r.PlayerAttributeModifiers.Any());

        public CharacterCreationViewModel()
        {
            PlayerAttributes = new(PlayerAttributes);

            if (HasRaces)
            {
                SelectedRace = GameDetails.Races.First();
            }

            RollNewCharacter();
        }

        public void RollNewCharacter()
        {
            PlayerAttributes.Clear();
            foreach (PlayerAttribute playerAttribute in GameDetails.PlayerAttributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }
            ApplyAttributeModifiers();
        }

        public void ApplyAttributeModifiers()
        {
            foreach (PlayerAttribute playerAttribute in PlayerAttributes)
            {
                PlayerAttributeModifier attributeRaceModifier = SelectedRace.PlayerAttributeModifiers.FirstOrDefault(pam => pam.AttributeKey.Equals(playerAttribute.Key, StringComparison.Ordinal));
                playerAttribute.ModifiedValue = playerAttribute.BaseValue + (attributeRaceModifier?.Modifier ?? 0);
            }
        }

        public Player GetPlayer()
        {
            Player player = new(Name, 0, 10, 10, PlayerAttributes, 10);
            player.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3003));
            player.LearnRecipe(RecipeFactory.GetRecipeById(1));
            return player;
        }
    }
}
