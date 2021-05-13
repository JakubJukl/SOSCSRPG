using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;
using Engine.Actions;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;

        public World CurrentWorld { get; }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    _currentPlayer.OnRename -= OnCurrentPlayerRenamed;
                    _currentPlayer.OnChangeClass -= OnCurrentPlayerChangedClass;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                    _currentPlayer.OnRename += OnCurrentPlayerRenamed;
                    _currentPlayer.OnChangeClass += OnCurrentPlayerChangedClass;
                }
            }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(IsHome));

                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }

        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed -= OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if (_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed += OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"Objevil se divoký {CurrentMonster.Name}!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public bool HasAttunementToNorth()
        {
            if (HasLocationToNorth)
            {
                return ShortLocation("north").Attunement != null;
            }
            return false;
        }
        public bool HasAttunementToEast()
        {
            if (HasLocationToEast)
            {
                return ShortLocation("east").Attunement != null;
            }
            return false;
        }
        public bool HasAttunementToWest()
        {
            if (HasLocationToWest)
            {
                return ShortLocation("west").Attunement != null;
            }
            return false;
        }
        public bool HasAttunementToSouth()
        {
            if (HasLocationToSouth)
            {
                return ShortLocation("south").Attunement != null;
            }
            return false;
        }
        /*public bool HasAttunementToNorth =>
            ShortLocation("north").Attunement != null;

        public bool HasAttunementToEast =>
            ShortLocation("east").Attunement != null;

        public bool HasAttunementToWest =>
            ShortLocation("west").Attunement != null;

        public bool HasAttunementToSouth =>
            ShortLocation("south").Attunement != null;*/

        public bool HasLocationToNorth =>
           ShortLocation("north") != null;

        public bool HasLocationToEast =>
            ShortLocation("east") != null;

        public bool HasLocationToSouth =>
            ShortLocation("south") != null;

        public bool HasLocationToWest =>
            ShortLocation("west") != null;

        public bool HasMonster => CurrentMonster != null;

        public bool HasTrader => CurrentTrader != null;

        public bool IsHome => CurrentLocation.Name == "Domov";

        public GameSession()
        {
            CurrentPlayer = new Player("Kuba", ClassFactory.GetClassByName("Válečník"), 0, 10, 10, 0, 100, 100);

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
                CurrentPlayer.AddItemToInventory(SpellFactory.CreateSpell(1001));
            }
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));
            CurrentPlayer.SetBaseStrenght(2);

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }
        
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                if (HasAttunementToNorth())
                {
                    if (AttunementCompleted(ShortLocation("north")))
                    {
                        CurrentLocation = ShortLocation("north") ; 
                    }
                    else
                    {
                        AttunementNotCompleted(ShortLocation("north"));
                    }
                }
                else
                {
                    CurrentLocation = ShortLocation("north");
                }
            }
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                if (HasAttunementToEast())
                {
                    if (AttunementCompleted(ShortLocation("east")))
                    {
                        CurrentLocation = ShortLocation("east");
                    }
                    else
                    {
                        AttunementNotCompleted(ShortLocation("east"));
                    }
                }
                else
                {
                    CurrentLocation = ShortLocation("east");
                }
            }
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                if (HasAttunementToSouth())
                {
                    if (AttunementCompleted(ShortLocation("south")))
                    {
                        CurrentLocation = ShortLocation("south");
                    }
                    else
                    {
                        AttunementNotCompleted(ShortLocation("south"));
                    }
                }
                else
                {
                    CurrentLocation = ShortLocation("south");
                }
            }
        }

        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                if (HasAttunementToWest())
                {
                    if (AttunementCompleted(ShortLocation("west")))
                    {
                        CurrentLocation = ShortLocation("west");
                    }
                    else
                    {
                        AttunementNotCompleted(ShortLocation("west"));
                    }
                }
                else
                {
                    CurrentLocation = ShortLocation("west");
                }
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID &&
                                                             !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);

                        RaiseMessage("");
                        RaiseMessage($"Dokončil si '{quest.Name}' úkol");

                        // Give the player the quest rewards
                        RaiseMessage($"Dostal si {quest.RewardExperiencePoints} XP");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"Dostal si {quest.RewardGold} zlatých");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            RaiseMessage($"Dostal si {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        // Mark the Quest as completed
                        questToComplete.IsCompleted = true;

                        AttunementFactory.attunements.ForEach(delegate (Attunement att)
                            {
                                if (att.AttunementQuest == quest)
                                {
                                    att.CompleteAttunement(att);
                                }                            
                            });
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"Máš nový úkol: '{quest.Name}' ");
                    RaiseMessage(quest.Description);

                    RaiseMessage("Vrať se až budeš mít:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }

                    RaiseMessage("A na oplátku dostaneš:");
                    RaiseMessage($"   {quest.RewardExperiencePoints} XP");
                    RaiseMessage($"   {quest.RewardGold} zlatých");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }
        private Location ShortLocation (string s)
        {
            switch (s)
            {
                case "north":
                    return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);                 
                case "south":
                    return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
                case "east":
                    return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
                case "west":
                    return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
                default:
                    return CurrentLocation;
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void CastSpell()
        {
            if (CurrentMonster == null)
            {
                return;
            }

            if (CurrentPlayer.CurrentSpell == null)
            {
                RaiseMessage("Musíš si zvolit kouzlo, abys mohl útočit!");
                return;
            }

            CurrentPlayer.UseCurrentSpellOn(CurrentMonster);

            if (CurrentMonster.IsDead)
            {
                // Get another monster to fight
                GetMonsterAtLocation();
            }
            else
            {
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
        }

        public void AttackCurrentMonster()
        {
            if (CurrentMonster == null)
            {
                return;
            }

            if (CurrentPlayer.CurrentWeapon == null)
            {
                RaiseMessage("Musíš si zvolit zbraň, abys mohl útočit!");
                return;
            }

            CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);

            if (CurrentMonster.IsDead)
            {
                // Get another monster to fight
                GetMonsterAtLocation();
            }
            else
            {
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
        }

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                CurrentPlayer.UseCurrentConsumable();
            }
        }

        public void Rest()
        {
            if (IsHome)
            {
                RaiseMessage($"Odpočinkem sis doplnil všechnu manu a HP.");
                CurrentPlayer.CompletelyHeal();
                CurrentPlayer.CompletelyRegainMana();
            }
        }

        private void OnCurrentPlayerPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }

        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        RaiseMessage($"Vytvořil si 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                RaiseMessage("Nemáš dostatek materiálu:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ItemID)}");
                }
            }
        }

        private void OnCurrentMonsterPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage("Zabili tě.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
            CurrentPlayer.CompletelyRegainMana();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"Zabil si {CurrentMonster.Name}!");

            RaiseMessage($"Dostal si {CurrentMonster.RewardExperiencePoints} XP.");
            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);

            RaiseMessage($"Dostal si {CurrentMonster.Gold} zlatých.");
            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"Dostal si {gameItem.Name}.");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"Dosáhl si levelu {CurrentPlayer.Level}!");
            CurrentPlayer.CompletelyHeal();
            CurrentPlayer.CompletelyRegainMana();
        }

        private void OnCurrentPlayerRenamed(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage("Tvé nové jméno je: " + CurrentPlayer.Name);
        }

        private void OnCurrentPlayerChangedClass(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage("Tvé nové povolání je: " + CurrentPlayer.CharacterClass.Name);
        }

        private void AttunementNotCompleted(Location loc)
        {
            RaiseMessage("Musíš nejdříve splnit úkol "+loc.Attunement.AttunementQuest.Name+", aby si mohl vstoupit do této oblasti.");            
        }
        private bool AttunementCompleted(Location l)
        {
            if (l.Attunement.Completed)
            {
                return true;
            }
            return false;
        }

        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

    }
}
