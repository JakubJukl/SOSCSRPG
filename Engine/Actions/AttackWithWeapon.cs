using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} není zbraň");
            }

            if (_minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage musí být 0 nebo větší");
            }

            if (_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("maximumDamage musí být větší než minDmg");
            }

            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage;
            if (actor is Player) 
            {
                damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage) * actor.Strenght;
            }
            else
            {
                damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
            }         

            string actorName = (actor is Player) ? "Hráč" : $"{actor.Name.ToLower()}";
            string targetName = (target is Player) ? "tebe" : $"{target.Name.ToLower()}";

            if (damage == 0)
            {
                ReportResult($"{actorName} se netrefil do {targetName}.");
            }
            else
            {
                ReportResult($"{actorName} zasáhl {targetName} za {damage} dmg{(damage > 1 ? "" : "")}.");

                target.TakeDamage(damage);              
            }
        }
    }
}
