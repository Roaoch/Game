using System.Collections.Generic;
using System.Linq;
using System;

namespace SwordAndGun
{
	public static class Map
    {
        public static MovementForLevel[,] MapLikeMovements { get; private set; }
        public static int LenghtX { get; private set; }
        public static int LenghtY { get; private set; }

        private static float tileSize;

        public static void SetMap(string movementsMap, Player player, List<Enemy> enemies, World world)
        {
            MapLikeMovements = StringToMovementForLevelMap(movementsMap);
            LenghtX = MapLikeMovements.GetLength(0);
            LenghtY = MapLikeMovements.GetLength(1);

            tileSize = world.WorldPlatforms[0].width / (LenghtX - 2);
        }

        public static (int, int) GetCoordinate(IMoveable entity)
        {
            return (
                (int)(entity.GetHitBox().x / tileSize) + 1,
                LenghtY - ((int)(entity.GetHitBox().y / tileSize) + 2)
                );
        }

        public static IEnumerable<(int, int)> GetPossiableDirection((int, int) coordinate)
        {
            var offsets = new List<(int, int)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };

            return offsets
                .Select(e => (e.Item1 + coordinate.Item1, e.Item2 + coordinate.Item2))
                .Where(e => e.Item1 >= 0 && e.Item2 >= 0)
                .Where(e => e.Item1 < LenghtX && e.Item2 < LenghtY)
                .Where(e => MapLikeMovements[e.Item1, e.Item2] != MovementForLevel.NoMovement);
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
