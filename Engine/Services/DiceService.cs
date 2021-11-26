using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
using System;

namespace Engine.Services
{
    public class DiceService : IDiceService
    {
        public static IDiceService Instance { get; } = new DiceService();

        public IDice Dice => new Dice();
        public IDieRoller DieRoller { get; private set; } = new RandomDieRoller();
        public IDiceConfiguration Configuration => Dice.Configuration;
        public IDieRollTracker RollTracker { get; private set; }

        public void Configure(IDiceService.RollerType rollerType, bool enableTracker = false, int constantValue = 1)
        {
            RollTracker = enableTracker ? new DieRollTracker() : null;

            DieRoller = rollerType switch
            {
                IDiceService.RollerType.Random => new RandomDieRoller(RollTracker),
                IDiceService.RollerType.Crypto => new CryptoDieRoller(RollTracker),
                IDiceService.RollerType.MathNet => new MathNetDieRoller(RollTracker),
                IDiceService.RollerType.Constant => new ConstantDieRoller(constantValue),
                _ => throw new ArgumentOutOfRangeException(nameof(rollerType)),
            };
        }

        public DiceResult Roll(string diceNotation)
        {
            return Dice.Roll(diceNotation, DieRoller);
        }

        public DiceResult Roll(int sides, int numDice = 1, int modifier = 0)
        {
            DiceResult result = Dice.Dice(sides, numDice).Constant(modifier).Roll(DieRoller);
            Dice.Clear();
            return result;
        }

        private DiceService() { }
    }
}
