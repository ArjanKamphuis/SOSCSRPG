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

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get => _currentHitPoints;
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get => _maximumHitPoints;
            set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Gold
        {
            get => _gold;
            set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameItem> Inventory { get; } = new ObservableCollection<GameItem>();
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; } = new ObservableCollection<GroupedInventoryItem>();

        public List<GameItem> Weapons => Inventory.Where(i => i is Weapon).ToList();

        #endregion

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);
            OnPropertyChanged(nameof(Weapons));

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                GroupedInventoryItem groupedInventoryItem = GroupedInventory.SingleOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);
                if (groupedInventoryItem == null)
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 1));
                }
                else
                {
                    groupedInventoryItem.Quantity++;
                }
            }
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            _ = Inventory.Remove(item);
            OnPropertyChanged(nameof(Weapons));

            GroupedInventoryItem groupedInventoryItemToRemove = GroupedInventory.SingleOrDefault(gi => gi.Item == item);
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
        }
    }
}
