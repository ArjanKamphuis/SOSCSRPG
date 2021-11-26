namespace Engine.Models
{
    public class PlayerAttributeModifier
    {
        public string AttributeKey { get; }
        public int Modifier { get; }

        public PlayerAttributeModifier(string attributeKey, int modifier)
        {
            AttributeKey = attributeKey;
            Modifier = modifier;
        }
    }
}
