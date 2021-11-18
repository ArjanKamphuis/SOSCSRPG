namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public int Id { get; }

        public Trader(int id, string name)
            : base(name, 9999, 9999, 9999)
        {
            Id = id;
        }
    }
}
