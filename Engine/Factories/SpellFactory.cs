using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Actions;

namespace Engine.Factories
{
    public static class SpellFactory
    {
        private static readonly List<GameItem> _spells = new List<GameItem>();

        static SpellFactory()
        {
            BuildSpell(1001, "Ohnivá koule", 1, 1, 2, 10, "DMG: 1 - 2, MANA: 10");
            BuildSpell(1002, "Záhuba", 5, 4, 5, 20, "DMG: 4 - 5, MANA: 20");
            BuildSpell(1003, "Utrpení", 10, 9, 14, 30, "DMG: 9 - 14, MANA: 30");
        }

        public static GameItem CreateSpell(int spellID)
        {
            return _spells.FirstOrDefault(item => item.ItemTypeID == spellID)?.Clone();
        }

        private static void BuildSpell(int id, string name, int price,
                                        int minimumDamage, int maximumDamage, int manaCost, string description)
        {
            GameItem spell = new GameItem(GameItem.ItemCategory.Spell, id, name, price, description, true);

            spell.Action = new AttackWithSpell(spell, minimumDamage, maximumDamage, manaCost);

            _spells.Add(spell);
        }

        public static string SpellName(int spellID)
        {
            return _spells.FirstOrDefault(i => i.ItemTypeID == spellID)?.Name ?? "";
        }
    }
}
