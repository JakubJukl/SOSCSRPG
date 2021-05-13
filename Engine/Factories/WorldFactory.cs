using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(-2, -1, "Farmářova Pole",
                "Jsou tu velké lány pole, s obrovskými krysami, které se v nich ukrývají.",
                "FarmFields.png");

            newWorld.LocationAt(-2, -1).AddMonster(2, 100);
            newWorld.LocationAt(-2, -1).AddMonster(1, 50);

            newWorld.AddLocation(-1, -1, "Farmářův Příbytek",
                "Tohle je dům tvého souseda, Farmáře Pepy.",
                "Farmhouse.png");
            newWorld.LocationAt(-1, -1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(2));

            newWorld.LocationAt(-1, -1).TraderHere =
                TraderFactory.GetTraderByName("Farmář Pepa");

            newWorld.AddLocation(0, -1, "Domov",
                "Tvůj sladký domov",
                "Home.png");

            newWorld.AddLocation(-1, 0, "Večerka",
                "Obchůdek Pavly, tvé žluté kamarádky.",
                "Trader.png");

            newWorld.LocationAt(-1, 0).TraderHere =
                TraderFactory.GetTraderByName("Pavla");

            newWorld.AddLocation(0, 0, "Náměstí",
                "Nic moc tu není, až na fontánu",
                "TownSquare.png");

            newWorld.AddLocation(1, 0, "Městská Brána",
                "Je tu brána, která brání město před obrovskými pavouky.",
                "TownGate.png");

            newWorld.LocationAt(1, 0).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(3));

            newWorld.AddLocation(2, 0, "Pavoučí les",
                "Les jako každý jiný, ale od čeho můžou být ty pavučiny?",
                "SpiderForest.png", AttunementFactory.GetAttunementByID(20000));

            newWorld.LocationAt(2, 0).AddMonster(3, 100);

            newWorld.AddLocation(3, 0, "Mýtina",
                "Ještě tu nic není",
                "", AttunementFactory.GetAttunementByID(20001));

            newWorld.LocationAt(3, 0).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(3));

            newWorld.AddLocation(0, 1, "Herbalistova Chatrč",
                "Vidíš malou chatrč se střechou porostlou bylinkami.",
                "HerbalistsHut.png");

            newWorld.LocationAt(0, 1).TraderHere =
                TraderFactory.GetTraderByName("Herbalista Péťa");

            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));

            newWorld.AddLocation(0, 2, "Herbalistova Zahrada",
                "Je tu pozoruhodné množství rostlin, ale ještě poziruhodnější množství hadů.",
                "HerbalistsGarden.png");

            newWorld.LocationAt(0, 2).AddMonster(1, 100);

            return newWorld;
        }
    }
}
