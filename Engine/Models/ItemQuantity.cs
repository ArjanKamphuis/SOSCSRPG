namespace Engine.Models
{
    public class ItemQuantity
    {
        public int ItemId { get; }
        public int Quantity { get; }

        public ItemQuantity(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
