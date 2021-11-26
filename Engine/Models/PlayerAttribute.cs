using Engine.Services;

namespace Engine.Models
{
    public class PlayerAttribute
    {
        public string Key { get; }
        public string DisplayName { get; }
        public string DiceNotation { get; }
        public int BaseValue { get; private set; }
        public int ModifiedValue { get; private set; }

        public PlayerAttribute(string key, string displayName, string diceNotation)
            : this(key, displayName, diceNotation, DiceService.Instance.Roll(diceNotation).Value)
        {
        }

        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue)
            : this(key, displayName, diceNotation, baseValue, baseValue)
        {
        }

        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue, int modifiedValue)
        {
            Key = key;
            DisplayName = displayName;
            DiceNotation = diceNotation;
            BaseValue = baseValue;
            ModifiedValue = modifiedValue;
        }

        public void ReRoll()
        {
            BaseValue = DiceService.Instance.Roll(DiceNotation).Value;
            ModifiedValue = BaseValue;
        }
    }
}
