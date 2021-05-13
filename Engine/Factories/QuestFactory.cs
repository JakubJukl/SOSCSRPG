using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // Declare the items need to complete the quest, and its reward items
            List<ItemQuantity> q1ItemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> q1RewardItems = new List<ItemQuantity>();

            q1ItemsToComplete.Add(new ItemQuantity(9001, 5));
            q1RewardItems.Add(new ItemQuantity(1002, 1));

            // Create the quest
            _quests.Add(new Quest(1,
                                  "Vyčisti Herbalistovu zahradu",
                                  "Zabij hady v Herbalistově zahradě",
                                  q1ItemsToComplete,
                                  25, 10,
                                  q1RewardItems));

            List<ItemQuantity> q2ItemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> q2RewardItems = new List<ItemQuantity>();

            q2ItemsToComplete.Add(new ItemQuantity(3001, 5));
            q2RewardItems.Add(new ItemQuantity(2001, 1));

            _quests.Add(new Quest(2,
                                  "Zbav Farmářovo pole krys",
                                  "Zabij krysy na Farmářově poli dřív, než přijde hygiena",
                                  q2ItemsToComplete,
                                  100, 25,
                                  q2RewardItems));

            List<ItemQuantity> q3ItemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> q3RewardItems = new List<ItemQuantity>();

            q3ItemsToComplete.Add(new ItemQuantity(9006, 1));
            q3RewardItems.Add(new ItemQuantity(3003, 5));
            q3RewardItems.Add(new ItemQuantity(3001, 5));
            q3RewardItems.Add(new ItemQuantity(1003, 1));
            q3RewardItems.Add(new ItemQuantity(4001, 1));

            // Create the quest
            _quests.Add(new Quest(3,
                                  "Zabij lesní monstrum",
                                  "Za branami města něco číhá, ale nikdo doposud setkání s tím nepřežil",
                                  q3ItemsToComplete,
                                  25, 10,
                                  q3RewardItems));
        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
