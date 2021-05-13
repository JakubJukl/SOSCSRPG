using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Attunement
    {
        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            private set { _completed = value; }
        }
        private int _ID;
        public int ID
        {
            get { return _ID; }
            private set { _ID = value; }
        }
        private Quest _attunementQuest;
        public Quest AttunementQuest
        {
            get { return _attunementQuest; }
            private set { _attunementQuest = value; }
        }

        public Attunement(int id, Quest quest, bool completed = false)
        {
            Completed = completed;
            AttunementQuest = quest;
            ID = id;
        }

        public void CompleteAttunement(Attunement att)
        {
            att.Completed = true;
        }
    }
}
