using NUnit.Framework;
using Raylib_cs;
using System.Numerics;
using System.Threading;
using SwordAndGun;

namespace Tests
{
    [TestFixture]
    class Tester
    {
        [Test]
        public void PlayerBasicMovementTest()
        {
            var player = new Player();
            Controller.OnKeyDowned(KeyboardKey.KEY_D, player);
            Thread.Sleep(100);
        }
    }
}