using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public class ClassFactory
    {
        private static readonly List<Class> _classes = new List<Class>();
        static ClassFactory()
        {
            Class Válečník = new Class("Válečník", 12000, "Může kouzlit, ale jeho hlavní síla spočívá ve využívání těžkých zbraní k boji na blízko.");
            Class Warlock = new Class("Warlock", 12001, "Má k dispozici dvakrát tolik many, co Válečník a ze všech zdrojů taky dostavá 2x více many. Jeho silnou stránkou jsou ničivá kouzla.");

            AddClassToList(Válečník);
            AddClassToList(Warlock);

        }

        public static Class GetClassByName(string name)
        {
            return _classes.FirstOrDefault(t => t.Name == name);
        }

        private static void AddClassToList(Class Class)
        {
            if (_classes.Any(t => t.Name == Class.Name))
            {
                throw new ArgumentException($"Tady už je povolání se jménem '{Class.Name}'");
            }

            _classes.Add(Class);
        }
        public static string ClassName(int classID)
        {
            return _classes.FirstOrDefault(i => i.ClassID == classID)?.Name ?? "";
        }

        public static readonly List<Class> classes = _classes;

        /*static ClassFactory()
        {
            BuildClass("Válečník", 12000);
            BuildClass("Warlock", 12001);

            AddClassToList(12000);
            AddClassToList(12001);
        }

        public static Class CreateClass(int classID)
        {
            return _Classes.FirstOrDefault(item => item.ClassID == classID);
        }

        private static void BuildClass(string name, int id)
        {
            _Classes.Add(new Class(name, id));
        }

        public static string ClassName(int classID)
        {
            return _Classes.FirstOrDefault(i => i.ClassID == classID)?.Name ?? "";
        }

            */
    }
}

