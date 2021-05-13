using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;

namespace Engine.Models
{
    public class Class : BaseNotificationClass
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description {get; }

        public int ClassID { get; }

        public Class(string Name, int classID, string description)
        {
            _name = Name;
            ClassID = classID;
            Description = description;
        }
        
    }
}
