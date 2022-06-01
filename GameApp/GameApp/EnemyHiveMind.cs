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

        private readonly Vector2 jumpVelocity = new Vector2(100, -100);
        private readonly Vector2 walkVelocity = new Vector2(4, 0);
        private readonly Vector2 climbUpVelocity = new Vector2(0, -10);
        private (int, int) tempPlayerCoordinate;

        public EnemyHiveMind(List<Enemy> enemies, Player player)
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
                if (tempPlayerCoordinate != player.MapCoordinate)
                    enemy.FindPathToPlayer(player, enemy.MapCoordinate);

                if (enemy.PathToPlayer.Count != 0)
                    GoToPlayer(enemy);

                enemy.Update(world);
                CheckPlayerHit(enemy, player);
            }
            tempPlayerCoordinate = player.MapCoordinate;
        }

        private void CheckPlayerHit(Enemy enemy, Player player)
        {
            if(CheckCollisionRecs(enemy.HitBox, player.AtackBox))
            {
                enemy.Hp -= player.AtackPower * GetFrameTime();
            }
        }

        private void GoToPlayer(Enemy enemy)
        {
            var nextPoint = enemy.PathToPlayer.First.Value;
            if (enemy.MapCoordinate == nextPoint)
            {
                enemy.PathToPlayer.RemoveFirst();
            }
            else
            {
                var displacment = GetDisplacment(nextPoint, enemy.MapCoordinate);
                if (displacment.Y != 0)
                {
                    if (displacment.Y < 0)
                    {
                        enemy.Velocity = Vector2.Zero;
                        enemy.Move(enemy.HitBox.x, enemy.HitBox.y - Map.TileSize);
                    }
                    if (displacment.Y > 0)
                    {
                        enemy.Velocity = Vector2.Zero;
                        enemy.Move(enemy.HitBox.x, enemy.HitBox.y + Map.TileSize);
                    }
                }
                else if (Map.MapLikeMovements[nextPoint.Item1, nextPoint.Item2] == MovementForLevel.Jump)
                {
                    enemy.Velocity += jumpVelocity * Vector2.UnitX * displacment.X;
                    enemy.Jump();
                }
                else
                {
                    enemy.Velocity += walkVelocity * displacment.X;
                }
            }
            //if (enemy.LocalTime == 0)
            //{
            //    enemy.Move(nextPoint.Item1 * Map.TileSize, nextPoint.Item2 * Map.TileSize);
            //    enemy.PathToPlayer.RemoveFirst();
            //}
        }

        private Vector2 GetDisplacment((int,int) first, (int,int) second)
        {
            return new Vector2(first.Item1 - second.Item1, first.Item2 - second.Item2);
        }
    }
}
