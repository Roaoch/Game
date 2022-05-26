using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Collections.Generic;
using System.Numerics;

namespace SwordAndGun
{
    public class EnemyHiveMind
    {
        public List<Enemy> AllEnemies { get; }

        public EnemyHiveMind(List<Enemy> enemies)
        {
            AllEnemies = enemies;
        }

        public void Update(World world, Player player)
        {
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

        private void GetPathToPlayer()
        {

        }
    }
}
