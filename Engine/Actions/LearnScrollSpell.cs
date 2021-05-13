using Engine.Models;
using System;
using Engine.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    class LearnScrollSpell : BaseAction, IAction
    {
        private readonly int _spellID;

        public  LearnScrollSpell(GameItem itemInUse, int spellID) 
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} není konzumovatelná");
            }

            _spellID = spellID;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "Ty" : $" {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "se" : $" {target.Name.ToLower()}";

            ReportResult($"{actorName} si {targetName} naučil nové kouzlo.");
            target.AddItemToInventory(SpellFactory.CreateSpell(_spellID));
        }
    }
}
