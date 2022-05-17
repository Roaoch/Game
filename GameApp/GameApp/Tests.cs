using NUnit.Framework;
using Raylib_cs;
using System.Numerics;
using SwordAndGun;

namespace Tests
{
    [TestFixture]
    static class Tester
    {
        [Test]
        public static void PlayerBasicMovementTest()
        {
            var player = new Player();
            Controller.OnKeyDowned(KeyboardKey.KEY_D, player);
            Assert.AreEqual(new Vector2(4, 0), player.Velocity);
        }
    }
}