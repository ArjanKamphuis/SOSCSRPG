using Engine.Models;
using Engine.Services;
using System;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction
    {
        private readonly int _minimumDamage;
        private readonly int _maximumDamage;

        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }
            if (minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }
            if (maximumDamage < minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }

            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        public override void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = actor is Player ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = target is Player ? "you" : $"the {target.Name.ToLower()}";

            if (CombatService.AttackSucceeded(actor, target))
            {
                int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
                ReportResult($"{actorName} hit {targetName} for {damage} points.");
                target.TakeDamage(damage);
            }
            else
            {
                ReportResult($"{actorName} missed {targetName}.");
            }
        }
    }
}
