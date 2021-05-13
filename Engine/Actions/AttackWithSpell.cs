using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.ViewModels;

namespace Engine.Actions
{
    public class AttackWithSpell : BaseAction, IAction
    {
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;
        private readonly int _manaCost;
        private readonly GameItem _spellInUse;

        public AttackWithSpell(GameItem itemInUse, int minimumDamage, int maximumDamage, int manaCost)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Spell)
            {
                throw new ArgumentException($"{itemInUse.Name} není kouzlo");
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
            _manaCost = manaCost;
            _spellInUse = itemInUse;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage) * actor.Intelect;

            string actorName = (actor is Player) ? "Hráč" : $"{actor.Name.ToLower()}";
            string targetName = (target is Player) ? "tebe" : $"{target.Name.ToLower()}";

            if (damage == 0)
            {
                ReportResult($"{actorName} se netrefil do {targetName}.");
            }
            else
            {
                if (actor.CurrentManaPoints >= _manaCost)
                {
                    ReportResult($"{actorName} zasáhl {targetName} za {damage} dmg{(damage > 1 ? "" : "")}.");

                    target.TakeDamage(damage);
                    actor.DecreaseMana(_manaCost);
                }
                else
                {
                    ReportResult($"Snažil ses použít kouzlo {_spellInUse.Name}, ale ve zmatení si zjistil, že nemáš dost many.");
                }
            }
        }
    }
}
