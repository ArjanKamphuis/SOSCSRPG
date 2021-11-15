using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Trader
    {
        public string Name { get; }
        public ObservableCollection<GameItem> Inventory { get; } = new ObservableCollection<GameItem>();

        public Trader(string name)
        {
            Name = name;
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);
        }
        public void RemoveItemFromIventory(GameItem item)
        {
            _ = Inventory.Remove(item);
        }
    }
}
