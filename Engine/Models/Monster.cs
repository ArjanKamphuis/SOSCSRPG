using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hitPoints;

        public string Name { get; }
        public string ImageName { get; }
        public int MaximumHitPoints { get; }
        public int HitPoints
        {
            get => _hitPoints;
            set
            {
                _hitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MinimumDage { get; }
        public int MaximumDamage { get; }
        public int RewardExperiencePoints { get; }
        public int RewardGold { get; }

        public ObservableCollection<ItemQuantity> Inventory { get; } = new ObservableCollection<ItemQuantity>();

        public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int minimumDamage, int maximumDamage, int rewardExperiencePoints, int rewardGold)
        {
            Name = name;
            ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            MaximumHitPoints = maximumHitPoints;
            HitPoints = hitPoints;
            MinimumDage = minimumDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
        }
    }
}
