using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;

namespace Engine.Services
{
    public interface IDiceService
    {
        public enum RollerType
        {
            Random,
            Crypto,
            MathNet,
            Constant
        }

        IDice Dice { get; }
        IDiceConfiguration Configuration { get; }
        IDieRollTracker RollTracker { get; }

        void Configure(RollerType rollerType, bool enableTracker = false, int constantValue = 1);
        DiceResult Roll(string diceNotation);
        DiceResult Roll(int sides, int numDice = 1, int modifier = 0);
    }
}
