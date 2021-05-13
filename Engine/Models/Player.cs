using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Engine.Factories;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private Class _characterClass;
        private int _experiencePoints;  

        public Class CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();
                SetLevelAndMaximumHitPoints();
            }
        }

        public event EventHandler OnLeveledUp;

        public ObservableCollection<QuestStatus> Quests { get; }

        public ObservableCollection<Recipe> Recipes { get; }

        public Player(string name, Class characterClass, int experiencePoints,
                      int maximumHitPoints, int currentHitPoints, int gold, int maximumManaPoints, int currentManaPoints) :
            base(name, maximumHitPoints, currentHitPoints, gold, maximumManaPoints, currentManaPoints)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;

            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }
       
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            if (originalLevel * 100 <= ExperiencePoints)
            {
                Level++;
                ExperiencePoints -= originalLevel * 100;
            }
            //Level = (ExperiencePoints / 100) + 1;

            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;

                if (CharacterClass == ClassFactory.GetClassByName("Warlock"))
                {
                    MaximumManaPoints = 2 * Level * 100;
                    Strenght = Level;
                }
                else if (CharacterClass == ClassFactory.GetClassByName("Válečník"))
                {
                    MaximumManaPoints = Level * 100;
                    Strenght = Level * 2;
                }

                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        public event EventHandler OnChangeClass;

        public void ChangeClass(Class newClass)
        {
            if (CharacterClass != newClass)
            {
                _characterClass = newClass;
                if (newClass == ClassFactory.GetClassByName("Warlock"))
                {
                    DoubleMana();
                    HalveStrenght();
                }
                else if (newClass == ClassFactory.GetClassByName("Válečník"))
                {
                    HalveMana();
                    DoubleStrenght();
                }
                OnChangeClass?.Invoke(this, new System.EventArgs());
                OnPropertyChanged(nameof(CharacterClass));
            }
        }

    }
}
