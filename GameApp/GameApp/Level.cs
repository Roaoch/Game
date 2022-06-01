using Raylib_cs;
using System.Collections.Generic;
using static Raylib_cs.Raylib;

namespace SwordAndGun
{
    public class Level
    {
        public Player Player;
        public EnemyHiveMind EnemyHiveMind;
        public World World;

        public Level(Player player, List<Enemy> enemies, List<Rectangle> platforms, List<Rectangle> walls, string movemntsMap)
        {
            World = new World(platforms, walls);
            Map.SetMap(movemntsMap, player, enemies, World);
            Player = player;
            EnemyHiveMind = new EnemyHiveMind(enemies, player);
        }
    }
}