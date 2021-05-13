using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.ViewModels;

namespace Engine.Actions
{
    public class DrinkAndHeal : BaseAction, IAction
    {
        private readonly int _hitPointsToHeal;
        private readonly int _manaToRegain;

        public DrinkAndHeal(GameItem itemInUse, int hitPointsToHeal, int manaToRegain)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} není konzumovatelná");
            }

            _hitPointsToHeal = hitPointsToHeal;
            _manaToRegain = manaToRegain;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "Ty" : $" {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "sis" : $" {target.Name.ToLower()}";

            ReportResult($"{actorName} {targetName} doplnil {_hitPointsToHeal} hp a {_manaToRegain} many.");
            target.Heal(_hitPointsToHeal);
            target.Drink(_manaToRegain);
        }
    }
}
