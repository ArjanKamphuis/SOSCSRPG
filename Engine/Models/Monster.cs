namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; }
        public int MinimumDage { get; }
        public int MaximumDamage { get; }
        public int RewardExperiencePoints { get; }

        public Monster(string name, string imageName, int maximumHitPoints, int currentHitPoints, int minimumDamage, int maximumDamage, int rewardExperiencePoints, int gold)
            : base(name, maximumHitPoints, currentHitPoints, gold)
        {
            ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            MinimumDage = minimumDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}
