using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Engine.Actions;
using Engine.Factories;
using Engine.ViewModels;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        private int backupMana;
        private int _maximumManaPoints;
        private int _currentManaPoints;
        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;
        private int _intelect;
        private int _strenght;
        private GameItem _currentWeapon;
        private GameItem _currentSpell;
        private GameItem _currentConsumable;

        public event EventHandler OnRename;

        public int Strenght
        {
            get { return _strenght; }
            protected set
            {
                _strenght = value;
                OnPropertyChanged();
            }
        }

        public int Intelect
        {
            get { return _intelect; }
            private set
            {
                if (_intelect != backupMana / 100)
                {
                    int intdiff = _intelect - backupMana / 100;
                    _intelect = MaximumManaPoints / 100 + intdiff + value;
                }
                else
                {
                    _intelect = MaximumManaPoints / 100 + value;
                }
                backupMana = MaximumManaPoints;
                OnPropertyChanged();
            }
        }

        public int CurrentManaPoints
        {
            get { return _currentManaPoints; }
            private set
            {
                _currentManaPoints = value;
                OnPropertyChanged();
            }
        }

        public int MaximumManaPoints
        {
            get { return _maximumManaPoints; }
            protected set
            {
                _maximumManaPoints = value;
                Intelect = 0;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public GameItem CurrentSpell
        {
            get { return _currentSpell; }
            set
            {
                if (_currentSpell != null)
                {
                    _currentSpell.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentSpell = value;

                if (_currentSpell != null)
                {
                    _currentSpell.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameItem> Inventory { get; }

        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public ObservableCollection<GroupedInventoryItem> SpellInventory { get; set; }

        public ObservableCollection<GroupedInventoryItem> MixedInventory { get; set; }

        public List<GameItem> Spells =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Spell).ToList();

        public List<GameItem> Weapons =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public bool HasConsumable => Consumables.Any();

        public bool IsDead => CurrentHitPoints <= 0;

        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold,
                               int maximumManaPoints = 0, int currentManaPoints = 0, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;
            MaximumManaPoints = maximumManaPoints;
            CurrentManaPoints = currentManaPoints;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
            SpellInventory = new ObservableCollection<GroupedInventoryItem>();
            MixedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        public void UseCurrentSpellOn(LivingEntity target)
        {
            CurrentSpell.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void Drink(int manaToRegain)
        {           
            CurrentManaPoints += manaToRegain;

            if (CurrentManaPoints > MaximumManaPoints)
            {
                CurrentManaPoints = MaximumManaPoints;
            }
        }

        public void CompletelyRegainMana()
        {
            CurrentManaPoints = MaximumManaPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} má jen {Gold} zlatých, a nemůže utratit {amountOfGold} zlatých");
            }

            Gold -= amountOfGold;
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }
                GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
            SpellInventory.Clear();
            MixedInventory.Clear();
            foreach (GroupedInventoryItem a in GroupedInventory)
            {
                if (a.Item.Category == GameItem.ItemCategory.Spell)
                {
                    SpellInventory.Add(a);
                }
                else
                {
                    MixedInventory.Add(a);
                }
            }
            OnPropertyChanged(nameof(Spells));
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
              GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
              GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);

            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }
            SpellInventory.Clear();
            MixedInventory.Clear();
            foreach (GroupedInventoryItem a in GroupedInventory)
            {
                if (a.Item.Category == GameItem.ItemCategory.Spell)
                {
                    SpellInventory.Add(a);
                }
                else
                {
                    MixedInventory.Add(a);
                }
            }
            OnPropertyChanged(nameof(Spells));
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }
        }

        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetBaseStrenght(int strenght)
        {
            Strenght = strenght;
        }

        public void DoubleStrenght()
        {
            Strenght = Strenght * 2;
        }

        public void HalveStrenght()
        {
            Strenght = Strenght / 2;
        }

        public void DecreaseMana(int mana)
        {
            CurrentManaPoints -= mana;
        }

        public void DoubleMana()
        {
            MaximumManaPoints = MaximumManaPoints * 2;
            CurrentManaPoints = CurrentManaPoints * 2;
        }

        public void HalveMana()
        {
            MaximumManaPoints = MaximumManaPoints / 2;
            CurrentManaPoints = CurrentManaPoints / 2;
        }

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        public void Rename(string newName)
        {
            _name = newName;
            OnRename?.Invoke(this, new System.EventArgs());
            OnPropertyChanged(nameof(Name));
        }
    }
}
