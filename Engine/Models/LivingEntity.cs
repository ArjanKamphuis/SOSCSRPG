using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class LivingEntity : BaseNotificationClass
    {
        #region Properties

        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;

        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        private readonly ObservableCollection<GameItem> _inventory = new ObservableCollection<GameItem>();
        private readonly ObservableCollection<GroupedInventoryItem> _groupedInventory = new ObservableCollection<GroupedInventoryItem>();

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get => _currentHitPoints;
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get => _maximumHitPoints;
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Gold
        {
            get => _gold;
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public int Level
        {
            get => _level;
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        public GameItem CurrentWeapon
        {
            get => _currentWeapon;
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

        public bool IsDead => CurrentHitPoints <= 0;

        public ReadOnlyObservableCollection<GameItem> Inventory { get; }
        public ReadOnlyObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public IEnumerable<GameItem> Weapons => Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon);
        public IEnumerable<GameItem> Consumables => Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable);
        public bool HasConsumbale => Consumables.Any();

        #endregion

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;

            Inventory = new ReadOnlyObservableCollection<GameItem>(_inventory);
            GroupedInventory = new ReadOnlyObservableCollection<GroupedInventoryItem>(_groupedInventory);
        }

        public void AddItemToInventory(GameItem item)
        {
            _inventory.Add(item);
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumbale));

            if (item.IsUnique)
            {
                _groupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                GroupedInventoryItem groupedInventoryItem = GroupedInventory.SingleOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);
                if (groupedInventoryItem == null)
                {
                    _groupedInventory.Add(new GroupedInventoryItem(item, 1));
                }
                else
                {
                    groupedInventoryItem.Quantity++;
                }
            }
        }
        public void RemoveItemFromInventory(GameItem item)
        {
            _ = _inventory.Remove(item);
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumbale));

            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique
                ? GroupedInventory.SingleOrDefault(gi => gi.Item == item)
                : GroupedInventory.SingleOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);

            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    _ = _groupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }
        }

        public void RemoveItemsFromInventory(IEnumerable<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeId == itemQuantity.ItemId));
                }
            }
        }

        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeId == item.ItemId) < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
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
            CurrentHitPoints = Math.Min(CurrentHitPoints + hitPointsToHeal, MaximumHitPoints);
        }
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }
        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }
            Gold -= amountOfGold;
        }

        #region Private function

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }

        #endregion
    }
}
