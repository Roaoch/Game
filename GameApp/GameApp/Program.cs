﻿using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace SwordAndGun
{
    static class Program
    {
        public static Level level1 = new Level(
            new Player(700, 1844),
            new List<Enemy> 
            {
                new Enemy(1400, 1844),
                new Enemy(3150, 1844),
                new Enemy(2800, 1144),
                new Enemy(0, 794),
                new Enemy(3500, 444)
            },
            new List<Rectangle> 
            { 
                new Rectangle(0, 2100, 3850, 500), 
                new Rectangle(0, 1750, 1400, 20),
                new Rectangle(2800, 1750, 1050, 20),
                new Rectangle(700, 1400, 1050, 20),
                new Rectangle(2450, 1400, 1400, 20),
                new Rectangle(0, 1050, 1050, 20),
                new Rectangle(2800, 1050, 1050, 20),
                new Rectangle(350, 700, 2100, 20),
                new Rectangle(3500, 700, 350, 20),
                new Rectangle(0, 350, 1050, 20),
                new Rectangle(1750, 350, 700, 20)
            },
            @"NNNNNNNNNNNNN
NWCCJJCCWNNNN
NNCCWWCCNNNCN
NWCCNNNNNCCCN
NNNCWWJJWCCCN
NCCCCNNNNNCCN
NCCCCWWWNNCCN
NNNNNNNNNNNNN");

        public static void Main()
        {
            Drawer.Initialize(level1);
        }
    }
}