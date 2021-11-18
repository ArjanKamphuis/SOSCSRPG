using Engine.Factories;

namespace Engine.Models
{
    public class ItemQuantity
    {
        public int ItemId { get; }
        public int Quantity { get; }
        public string QuantityItemDescription => $"{Quantity} {ItemFactory.ItemName(ItemId)}";

        public ItemQuantity(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
