using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace SwordAndGun
{
    static class Program
    {
        public static Level level1 = new Level(
            new Player(),
            new List<Enemy> { new Enemy(600, 200) },
            new List<Rectangle> { new Rectangle(0, 600, 1920, 40), new Rectangle(0, 200, 200, 20) });
        public static void Main()
        {
            Drawer.Initialize(level1);
        }
    }
}
