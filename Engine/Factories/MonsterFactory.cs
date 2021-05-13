using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    Monster snake =
                        new Monster("Had", "Snake.png", 8, 8, 5, 1);

                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);

                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 65);
                    AddLootItem(snake, 3002, 10);

                    return snake;

                case 2:
                    Monster rat =
                        new Monster("Krysa", "Rat.png", 10, 10, 5, 1);

                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);

                    AddLootItem(rat, 3001, 10);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 65);

                    return rat;

                case 3:
                    Monster giantSpider =
                        new Monster("Obrovský Pavouk", "GiantSpider.png", 20, 20, 10, 3);

                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);

                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 65);
                    AddLootItem(giantSpider, 3003, 10);

                    return giantSpider;

                default:
                    throw new ArgumentException(string.Format("Nestvůra '{0}' neexistuje", monsterID));
            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
