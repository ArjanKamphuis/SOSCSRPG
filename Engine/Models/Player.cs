using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        public event EventHandler OnLeveledUp;

        #region Properties

        private string _characterClass;
        private int _experiencePoints;

        private readonly ObservableCollection<QuestStatus> _quests = new();
        private readonly ObservableCollection<Recipe> _recipes = new();

        public string CharacterClass
        {
            get => _characterClass;
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        public int ExperiencePoints
        {
            get => _experiencePoints;
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();
                SetLevelAndMaximumHitPoints();
            }
        }

        public ReadOnlyObservableCollection<QuestStatus> Quests { get; }
        public ReadOnlyObservableCollection<Recipe> Recipes { get; }

        #endregion

        public Player(string name, string characterClass, int experiencePoints, int maximumHitPoints, int currentHitPoints, int dexterity, int gold)
            : base(name, maximumHitPoints, currentHitPoints, dexterity, gold)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;

            Quests = new ReadOnlyObservableCollection<QuestStatus>(_quests);
            Recipes = new ReadOnlyObservableCollection<Recipe>(_recipes);
        }

        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }

        /// <summary>
        /// Returns true when accepted
        /// </summary>
        /// <param name="quest">Quest to accept</param>
        public bool GiveQuest(Quest quest)
        {
            if (!Quests.Any(q => q.PlayerQuest.Id == quest.Id))
            {
                _quests.Add(new QuestStatus(quest));
                return true;
            }
            return false;
        }
        public bool GiveQuest(QuestStatus quest)
        {
            if (!Quests.Any(q => q.PlayerQuest.Id == quest.PlayerQuest.Id))
            {
                _quests.Add(quest);
                return true;
            }
            return false;
        }

        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.Id == recipe.Id))
            {
                _recipes.Add(recipe);
            }
        }

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (ExperiencePoints / 100) + 1;
            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
