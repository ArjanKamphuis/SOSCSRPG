using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            IEnumerable<ItemQuantity> itemsToComplete = new List<ItemQuantity>() { new ItemQuantity(9001, 5) };
            IEnumerable<ItemQuantity> rewardItems = new List<ItemQuantity>() { new ItemQuantity(1002, 1) };
            _quests.Add(new Quest(1, "Clear the herb garden", "Defeat the snakes in the Herbalist's garden", itemsToComplete, 25, 10, rewardItems));
        }

        internal static Quest GetQuestById(int id)
        {
            return _quests.SingleOrDefault(quest => quest.Id == id);
        }
    }
}
