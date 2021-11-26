using Engine.Factories;
using Engine.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Location
    {
        private readonly List<Quest> _questsAvailableHere = new List<Quest>();
        private readonly List<MonsterEncounter> _monstersHere = new List<MonsterEncounter>();

        public int XCoordinate { get; }
        public int YCoordinate { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string ImageName { get; }
        [JsonIgnore]
        public IEnumerable<Quest> QuestsAvailableHere => _questsAvailableHere;
        [JsonIgnore]
        public IEnumerable<MonsterEncounter> MonstersHere => _monstersHere;
        [JsonIgnore]
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
            if (!QuestsAvailableHere.Any(q => q.Id == questId))
            {
                _questsAvailableHere.Add(QuestFactory.GetQuestById(questId));
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
            int randomNumber = DiceService.Instance.Roll(totalChances).Value;
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
