using Raylib_cs;
using System.Collections.Generic;
using static Raylib_cs.Raylib;

namespace SwordAndGun
{
    public class Level
    {
        public Player Player = new Player();
        public EnemyHiveMind EnemyHiveMind;
        public World World;

        public Level(Player player, List<Enemy> enemies, List<Rectangle> platforms)
        {
            Player = player;
            EnemyHiveMind = new EnemyHiveMind(enemies);
            World = new World(platforms);
        }
    }
}