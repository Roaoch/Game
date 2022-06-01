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
        public static float TileSize;

        public static void SetMap(string movementsMap, Player player, List<Enemy> enemies, World world)
        {
            MapLikeMovements = StringToMovementForLevelMap(movementsMap);
            LenghtX = MapLikeMovements.GetLength(0);
            LenghtY = MapLikeMovements.GetLength(1);

            TileSize = world.WorldPlatforms[0].width / (LenghtX - 2);

            player.MapCoordinate = GetCoordinate(player);
            foreach (var enemy in enemies)
            {
                enemy.MapCoordinate = GetCoordinate(enemy);
                enemy.FindPathToPlayer(player, enemy.MapCoordinate);
            }

        }

        public static (int, int) GetCoordinate(IMoveable entity)
        {
            return (
               (int)(entity.GetHitBox().x / TileSize) + 1,
                (int)(entity.GetHitBox().y / TileSize) + 1
                );
        }

        public static IEnumerable<(int, int)> GetPossiableDirection((int, int) coordinate)
        {
            var offsets = new List<(int, int)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach(var offset in offsets)
            {
                var newCoordinateX = coordinate.Item1 + offset.Item1;
                var newCoordinateY = coordinate.Item2 + offset.Item2;
                if (newCoordinateX >= 0 && newCoordinateY >= 0 &&
                    newCoordinateX < LenghtX && newCoordinateY < LenghtY &&
                    MapLikeMovements[newCoordinateX, newCoordinateY] != MovementForLevel.NoMovement &&
                    MapLikeMovements[newCoordinateX, newCoordinateY] != MovementForLevel.Jump)
                    yield return (newCoordinateX, newCoordinateY);
            }
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
/*
NNNNNNNNNNNNN
NWCCJJCCWNNNN
NNCCWWCCNNNCN
NWCCNNNNNCCCN
NNNCWWJJWCCCN
NCCCCNNNNNCCN
NCCCCWWWNNCCN
NNNNNNNNNNNNN
 */