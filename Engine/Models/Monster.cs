using Engine.Factories;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();

        public int Id { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }

        public Monster(int id, string name, string imageName, int maximumHitPoints, GameItem currentWeapon, int rewardExperiencePoints, int gold)
            : base(name, maximumHitPoints, maximumHitPoints, gold)
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
            Monster newMonster = new Monster(Id, Name, ImageName, MaximumHitPoints, CurrentWeapon, RewardExperiencePoints, Gold);
            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                newMonster.AddItemToLootTable(itemPercentage.Id, itemPercentage.Percentage);
                if (RandomNumberGenerator.NumberBetween(1, 100) <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.Id));
                }
            }
            return newMonster;
        }
    }
}
