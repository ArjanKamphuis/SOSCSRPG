using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hitPoints;

        public string Name { get; private set; }
        public string ImageName { get; private set; }
        public int MaximumHitPoints { get; private set; }
        public int HitPoints
        {
            get => _hitPoints;
            private set
            {
                _hitPoints = value;
                OnPropertyChanged();
            }
        }
        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }

        public ObservableCollection<ItemQuantity> Inventory { get; private set; } = new ObservableCollection<ItemQuantity>();

        public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int rewardExperiencePoints, int rewardGold)
        {
            Name = name;
            ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            MaximumHitPoints = maximumHitPoints;
            HitPoints = hitPoints;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
        }
    }
}
