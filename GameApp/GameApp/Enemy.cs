using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System.Collections.Generic;
using System;

namespace SwordAndGun
{
    public class Enemy : IMoveable
    {
        private Vector2 velocity;
        private float hp = 100;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle HitBox;
        public Rectangle AtackBox;
        public (int, int) MapCoordinate { get; private set; }
        public float Hp { get => hp; set => hp = Math.Clamp(value, 0, 100); }
        public float AtackPower { get; set; } = 50;

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }
        public bool IsMoving { get => velocity != Vector2.Zero; }

        public bool HaveNoClip { get; set; } = false;

        public Enemy(float x, float y)
        {
            Velocity = new Vector2(0, 0);
            HitBox = new Rectangle(x, y, 256, 256);
        }

        public void Move(Vector2 vector)
        {
            velocity += vector;
        }
        public void Move(float x, float y)
        {
            HitBox.x = x;
            HitBox.y = y;
        }
        public void Update(World world)
        {
            world.AlongPhysics(this);

            HitBox.x += Velocity.X * GetFrameTime() * 60;
            HitBox.y += Velocity.Y * GetFrameTime() * 60;

            MapCoordinate = Map.GetCoordinate(this);

            //if (Program.level1.enemyAtack.Currentframe == 2)
            //    AtackBox = new Rectangle(HitBox.x + HitBox.width, HitBox.y, 60, 200);
            //else
            //    AtackBox = default(Rectangle);
        }

        public Rectangle GetHitBox()
        {
            return HitBox;
        }

        public void Atack()
        {
            Velocity = Vector2.Zero;
            IsAtacking = true;
        }

        public void Jump()
        {
            CanBeMoved = false;
            Move(HitBox.x, HitBox.y - 2);
        }

        public void ToggleNoClip()
        {
            HaveNoClip = !HaveNoClip;
        }

        public LinkedList<(int, int)> PathToPlayer(Player player)
        {
            var searhIn = new Queue<(int, int)>();
            var tracks = new Dictionary<(int, int), LinkedList<(int, int)>>();
            var visited = new HashSet<(int, int)>();

            searhIn.Enqueue(MapCoordinate);
            visited.Add(MapCoordinate);
            tracks.Add(MapCoordinate, new LinkedList<(int, int)>());

            tracks[MapCoordinate].AddLast(MapCoordinate);

            while (searhIn.Count != 0)
            {
                var point = searhIn.Dequeue();

                foreach (var neighbour in Map.GetPossiableDirection(point))
                {
                    if (visited.Contains(neighbour))
                        continue;

                    visited.Add(neighbour);
                    searhIn.Enqueue(neighbour);
                    tracks.Add(neighbour, new LinkedList<(int, int)>(tracks[point]));
                    tracks[neighbour].AddFirst(neighbour);
                }
                if (tracks.ContainsKey(player.MapCoordinate))
                    break;
            }

            return tracks[player.MapCoordinate];
        }
    }
}
