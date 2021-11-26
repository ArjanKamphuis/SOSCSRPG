using System.Collections.Generic;

namespace Engine.Models
{
    public class Race
    {
        public string Key { get; }
        public string DisplayName { get; }
        public List<PlayerAttributeModifier> PlayerAttributeModifiers { get; } = new();

        public Race(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
