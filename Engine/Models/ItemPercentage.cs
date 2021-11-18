namespace Engine.Models
{
    public class ItemPercentage
    {
        public int Id { get; }
        public int Percentage { get; }

        public ItemPercentage(int id, int percentage)
        {
            Id = id;
            Percentage = percentage;
        }
    }
}
