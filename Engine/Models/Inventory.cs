using Engine.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Inventory
    {
        #region Backing variables
        private readonly List<GameItem> _backingInventory = new List<GameItem>();
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();
        #endregion

        #region Properties
        public IReadOnlyList<GameItem> Items => _backingInventory.AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GroupedInventoryItem> GroupedInventory => _backingGroupedInventoryItems.AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GameItem> Weapons => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Weapon).AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GameItem> Consumables => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Consumable).AsReadOnly();
        [JsonIgnore]
        public bool HasConsumable => Consumables.Any();
        #endregion

        #region Constructors
        public Inventory(IEnumerable<GameItem> items = null)
        {
            if (items != null)
            {
                foreach (GameItem item in items)
                {
                    _backingInventory.Add(item);
                    AddItemToGroupedInventory(item);
                }
            }
        }
        #endregion

        #region Public functions
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => Items.Count(i => i.ItemTypeId == item.ItemId) >= item.Quantity);
        }
        #endregion

        #region Private functions
        private void AddItemToGroupedInventory(GameItem item)
        {
            if (item.IsUnique)
            {
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (_backingGroupedInventoryItems.All(gi => gi.Item.ItemTypeId != item.ItemTypeId))
                {
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));
                }
                _backingGroupedInventoryItems.Single(gi => gi.Item.ItemTypeId == item.ItemTypeId).Quantity++;
            }
        }
        #endregion
    }
}
