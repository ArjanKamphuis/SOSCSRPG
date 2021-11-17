using Engine.Factories;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Location
    {
        private readonly List<Quest> _quests = new List<Quest>();
        private readonly List<MonsterEncounter> _monstersHere = new List<MonsterEncounter>();

        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }
        public IEnumerable<Quest> QuestsAvailableHere => _quests;
        public IEnumerable<MonsterEncounter> MonstersHere => _monstersHere;
        public Trader TraderHere { get; set; }

        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        public void AddQuest(int questId)
        {
            if (!_quests.Any(q => q.Id == questId))
            {
                _quests.Add(QuestFactory.GetQuestById(questId));
            }
        }

        public void AddMonster(int monsterId, int chanceOfEncountering)
        {
            MonsterEncounter monsterEncounter = MonstersHere.SingleOrDefault(m => m.MonsterId == monsterId);
            if (monsterEncounter != null)
            {
                monsterEncounter.ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                _monstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
            }
        }

        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);
            int runningTotal = 0;

            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;
                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
                }
            }

            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
        }
    }
}
