using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Actions
{
    public class Drink : BaseAction, IAction
    {
        private readonly int _manaToRegain;

        public Drink(GameItem itemInUse, int manaToRegain)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} není konzumovatelná");
            }

            _manaToRegain = manaToRegain;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "Ty" : $" {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "sobě" : $" {target.Name.ToLower()}";

            ReportResult($"{actorName} doplnil {targetName} {_manaToRegain} many{(_manaToRegain > 1 ? "" : "")}.");
            target.Drink(_manaToRegain);
        }
    }
}
