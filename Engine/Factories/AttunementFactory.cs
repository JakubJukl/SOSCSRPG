using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.ViewModels;

namespace Engine.Factories
{
    internal static class AttunementFactory
    {
        private static readonly List<Attunement> _attunements = new List<Attunement>();

        public static readonly List<Attunement> attunements = _attunements;

        static AttunementFactory()
        {
            _attunements.Add(new Attunement(20000, QuestFactory.GetQuestByID(1)));
            _attunements.Add(new Attunement(20001, QuestFactory.GetQuestByID(3)));
        }

        internal static Attunement GetAttunementByID(int id)
        {
            return _attunements.FirstOrDefault(attunement => attunement.ID == id);
        }
    }
}
