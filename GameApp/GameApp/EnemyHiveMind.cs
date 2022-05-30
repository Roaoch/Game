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
        private readonly MovementForLevel[,] MapLikeMovements;

        public EnemyHiveMind(List<Enemy> enemies, string movementsMap)
        {
            AllEnemies = enemies;
            MapLikeMovements = StringToMovementForLevelMap(movementsMap);
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

        private MovementForLevel[,] StringToMovementForLevelMap(string movements)
        {
            var splitedMovements = movements.Split("\r\n");
            var result = new MovementForLevel[splitedMovements[0].Length, splitedMovements.Length];
            for(var i = 0; i < splitedMovements[0].Length; i++)
                for(var j = 0; j < splitedMovements.Length; j++)
                {
                    switch(splitedMovements[j][i])
                    {
                        case 'N':
                            result[i, j] = MovementForLevel.NoMovement;
                            break;
                        case 'C':
                            result[i, j] = MovementForLevel.Climb;
                            break;
                        case 'W':
                            result[i, j] = MovementForLevel.Walk;
                            break;
                        case 'J':
                            result[i, j] = MovementForLevel.Jump;
                            break;
                    }
                }

            return result;
        }
    }
}
