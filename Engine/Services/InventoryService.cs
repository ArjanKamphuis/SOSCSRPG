using Engine.Factories;
using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Services
{
    public static class InventoryService
    {
        public static Inventory AddItem(this Inventory inventory, GameItem item)
        {
            return inventory.AddItems(new List<GameItem> { item });
        }
        public static Inventory AddItemFromFactory(this Inventory inventory, int itemTypeId)
        {
            return inventory.AddItems(new List<GameItem> { ItemFactory.CreateGameItem(itemTypeId) });
        }

        public static Inventory AddItems(this Inventory inventory, IEnumerable<GameItem> items)
        {
            return new Inventory(inventory.Items.Concat(items));
        }
        public static Inventory AddItems(this Inventory inventory, IEnumerable<ItemQuantity> itemQuantities)
        {
            List<GameItem> itemsToAdd = new List<GameItem>();
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    itemsToAdd.Add(ItemFactory.CreateGameItem(itemQuantity.ItemId));
                }
            }
            return inventory.AddItems(itemsToAdd);
        }

        public static Inventory RemoveItem(this Inventory inventory, GameItem item)
        {
            return inventory.RemoveItems(new List<GameItem> { item });
        }

        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<GameItem> items)
        {
            List<GameItem> workingInventory = inventory.Items.ToList();
            foreach (GameItem item in items)
            {
                _ = workingInventory.Remove(item);
            }
            return new Inventory(workingInventory);
        }
        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<ItemQuantity> itemQuantities)
        {
            Inventory workingInventory = inventory;
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    workingInventory = workingInventory.RemoveItem(workingInventory.Items.First(item => item.ItemTypeId == itemQuantity.ItemId));
                }
            }
            return workingInventory;
        }

        public static List<GameItem> ItemsThatAre(this IEnumerable<GameItem> inventory, GameItem.ItemCategory category)
        {
            return inventory.Where(i => i.Category == category).ToList();
        }
    }
}
