using System.Collections.Generic;

namespace Engine.Models
{
    public class Quest
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public IEnumerable<ItemQuantity> ItemsToComplete { get; }

        public int RewardExperiencePoints { get; }
        public int RewardGold { get; }
        public IEnumerable<ItemQuantity> RewardItems { get; }

        public Quest(int id, string name, string description, IEnumerable<ItemQuantity> itemsToComplete, int rewardExperiencePoints, int rewardGold, IEnumerable<ItemQuantity> rewardItems)
        {
            Id = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
