using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Actions;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        static ItemFactory()
        {
            BuildWeapon(1001, "Klacek", 1, 1, 2, "DMG: 1 - 2");
            BuildWeapon(1002, "Rezavý Meč", 5, 1, 3, "DMG: 1 - 3");
            BuildWeapon(1003, "Otcův Meč", 10, 4, 6, "DMG: 4 - 6");

            BuildWeapon(1501, "Hadí zub", 0, 0, 2, "");
            BuildWeapon(1502, "Krysí dráp", 0, 0, 2, "");
            BuildWeapon(1503, "Pavoučí tesák", 0, 0, 4, "");
            BuildWeapon(1999, "Ultimátní meč", 0, 998, 999, "DMG: 998 - 999");

            BuildHealingItem(2001, "Cereálie", 5, 4, 40, "Najez se a doplň si 4 HP a 40 Many");

            BuildMiscellaneousItem(3001, "Oves", 1, "Přísada do Cereálií");
            BuildMiscellaneousItem(3002, "Med", 2, "Přísada do Cereálií");
            BuildMiscellaneousItem(3003, "Rozinky", 2, "Přísada do Cereálií");

            BuildScroll(4001, "Svitek Utrpení", 10, 1003, "Naučí tě jak způsobovat Utrpení");

            BuildMiscellaneousItem(9001, "Hadí zub", 1, "Neměl si se stát zubařem?");
            BuildMiscellaneousItem(9002, "Hadí kůže", 2, "Jednou z nich budou dobré boty");
            BuildMiscellaneousItem(9003, "Krysí ocas", 1, "Svačina chudých");
            BuildMiscellaneousItem(9004, "Krysí kožíšek", 2, "Skoro jako myší, jen víc smrdí");
            BuildMiscellaneousItem(9005, "Pavoučí zub", 1, "Zachvíli budeš trhat zuby i vesničanům");
            BuildMiscellaneousItem(9006, "Pavoučí hedvábí", 2, "Asi žrali Bource Morušové");
        }

        public static GameItem CreateGameItem(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID)?.Clone();
        }

        private static void BuildMiscellaneousItem(int id, string name, int price, string description)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price, description));
        }

        private static void BuildWeapon(int id, string name, int price,
                                        int minimumDamage, int maximumDamage, string description)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, description, true);

            weapon.Action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);

            _standardGameItems.Add(weapon);
        }
        
        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal, int manaToRegain, string description)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price, description);
            item.Action = new DrinkAndHeal(item, hitPointsToHeal, manaToRegain);
            _standardGameItems.Add(item);
        }

        private static void BuildScroll(int id, string name, int price, int spellId, string description)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price, description);
            item.Action = new LearnScrollSpell(item, spellId);
            _standardGameItems.Add(item);
        }

        public static string ItemName(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Name ?? "";
        }
    }
}
