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

        private readonly Vector2 jumpVelocity = new Vector2(10, 100);
        private readonly Vector2 walkVelocity = new Vector2(10, 0);
        private readonly Vector2 climbUpVelocity = new Vector2(0, 100);

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
                GoToPlayer(enemy, enemy.PathToPlayer(player), player);
            }
            AllEnemies[0].PathToPlayer(player);
        }

        private void CheckPlayerHit(Enemy enemy, Player player)
        {
            if(CheckCollisionRecs(enemy.HitBox, player.AtackBox))
            {
                enemy.Hp -= player.AtackPower * GetFrameTime();
            }
        }

        private void GoToPlayer(Enemy enemy, LinkedList<(int, int)> path, Player player)
        {
            var previouse = path.First.Value;
            var previouseMovement = Map.MapLikeMovements[previouse.Item1, previouse.Item2];
            foreach(var point in path.Skip(1))
            {
                
            }
        }
    }
}
