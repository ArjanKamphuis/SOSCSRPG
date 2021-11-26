using Engine.Actions;
using Engine.Factories;
using Engine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestEngine.Actions
{
    [TestClass]
    public class TestAttackWithWeapon
    {
        [TestMethod]
        public void Test_Constructor_GoodParameters()
        {
            GameItem pointyStick = ItemFactory.CreateGameItem(1001);
            AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, "1d5");

            Assert.IsNotNull(attackWithWeapon);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Constructor_ItemIsNotAWeapon()
        {
            GameItem granolaBar = ItemFactory.CreateGameItem(2001);
            _ = new AttackWithWeapon(granolaBar, "1d5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Constructor_DamgeDiceStringEmpty()
        {
            GameItem pointyStick = ItemFactory.CreateGameItem(1001);
            _ = new AttackWithWeapon(pointyStick, string.Empty);
        }
    }
}
