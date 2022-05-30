using System.Collections.Generic;
using System.Linq;
using System;

namespace SwordAndGun
{
	public static class Map
    {
        public static MovementForLevel[,] MapLikeMovements { get; private set; }

        private static float tileSize;

        public static void SetMap(string movementsMap, Player player, List<Enemy> enemies, World world)
        {
            MapLikeMovements = StringToMovementForLevelMap(movementsMap);
            tileSize = world.WorldPlatforms[0].width / (MapLikeMovements.GetLength(0) - 2);
        }

        public static Tuple<int, int> GetCoordinate(IMoveable entity)
        {
            return Tuple.Create(
                (int)(entity.GetHitBox().x / tileSize) + 1,
                MapLikeMovements.GetLength(1) - ((int)(entity.GetHitBox().y / tileSize) + 2)
                );
        }

        private static MovementForLevel[,] StringToMovementForLevelMap(string movementsMap)
        {
            var splitedMovements = movementsMap.Split("\r\n");
            var result = new MovementForLevel[splitedMovements[0].Length, splitedMovements.Length];
            for (var i = 0; i < splitedMovements[0].Length; i++)
                for (var j = 0; j < splitedMovements.Length; j++)
                {
                    switch (splitedMovements[j][i])
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
