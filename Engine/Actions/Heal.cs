using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly int _hitPointsToHeal;

        public Heal(GameItem itemInUse, int hitPointsToHeal)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} není konzumovatelná");
            }

            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "Ty": $" {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "sebe" : $" {target.Name.ToLower()}";

            ReportResult($"{actorName} healnul {targetName} za {_hitPointsToHeal} hp{(_hitPointsToHeal > 1 ? "" : "")}.");
            target.Heal(_hitPointsToHeal);
        }
    }
}
