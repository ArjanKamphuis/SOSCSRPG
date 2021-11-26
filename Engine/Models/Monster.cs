using Engine.Factories;
using Engine.Services;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new();

        public int Id { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }

        public Monster(int id, string name, string imageName, int maximumHitPoints, int dexterity, GameItem currentWeapon, int rewardExperiencePoints, int gold)
            : base(name, maximumHitPoints, maximumHitPoints, dexterity, gold)
        {
            Id = id;
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }

        public void AddItemToLootTable(int id, int percentage)
        {
            _ = _lootTable.RemoveAll(ip => ip.Id == id);
            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public Monster GetNewInstance()
        {
            Monster newMonster = new(Id, Name, ImageName, MaximumHitPoints, Dexterity, CurrentWeapon, RewardExperiencePoints, Gold);
            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                newMonster.AddItemToLootTable(itemPercentage.Id, itemPercentage.Percentage);
                if (DiceService.Instance.Roll(100).Value <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.Id));
                }
            }
            return newMonster;
        }
    }
}
