using Raylib_cs;
using System.Linq;
using static Raylib_cs.Raylib;
using System.Collections.Generic;
using System.Numerics;

namespace SwordAndGun
{
    public class EnemyHiveMind
    {
        public List<Enemy> AllEnemies { get; private set; }

        public EnemyHiveMind(List<Enemy> enemies)
        {
            AllEnemies = enemies;
        }

        public void Update(World world, Player player)
        {
            AllEnemies = AllEnemies
                .Where(e => e.Hp != 0)
                .ToList();
            foreach(var enemy in AllEnemies)
            {
                enemy.Update(world);
                CheckPlayerHit(enemy, player);
            }
        }

        private void CheckPlayerHit(Enemy enemy, Player player)
        {
            if(CheckCollisionRecs(enemy.HitBox, player.AtackBox))
            {
                enemy.Hp -= player.AtackPower * GetFrameTime();
            }
        }
    }
}
