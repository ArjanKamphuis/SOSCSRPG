﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine + "Items to complete the quest" + Environment.NewLine + "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) + Environment.NewLine + Environment.NewLine + "Rewards" + Environment.NewLine +
            "===========================" + Environment.NewLine + $"{RewardExperiencePoints} experience points" + Environment.NewLine + $"{RewardGold} gold pieces" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));

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
