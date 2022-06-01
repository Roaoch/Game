using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System.Collections.Generic;
using System;

namespace SwordAndGun
{
    public class Enemy : ICanAtack, IMoveable
    {
        private Vector2 velocity;
        private float hp = 100;
        public float LocalTime = 0;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle HitBox;
        public Rectangle AtackBox;
        public (int, int) MapCoordinate { get; set; }
        public LinkedList<(int, int)> PathToPlayer { get; private set; } = new LinkedList<(int, int)>();
        public float Hp { get => hp; set => hp = Math.Clamp(value, 0, 100); }
        public float AtackPower { get; set; } = 30;
        public int ForwardBackward { get; private set; } = 1;

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
            LocalTime += GetFrameTime();
            world.AlongPhysics(this);

            HitBox.x += Velocity.X * GetFrameTime() * 60;
            HitBox.y += Velocity.Y * GetFrameTime() * 60;

            if (velocity.X != 0)
                ForwardBackward = -(int)(velocity.X / Math.Abs(velocity.X));

            MapCoordinate = Map.GetCoordinate(this);

            if (LocalTime >= 0.8)
            {
                LocalTime = 0;
            }

            if (Drawer.enemyAtack.Currentframe == 2)
                AtackBox = World.GenerateAtackBox(this, -ForwardBackward);
            else
                AtackBox = default(Rectangle);
        }

        public Rectangle GetHitBox()
        {
            return HitBox;
        }

        public void SetDisplacment(int displacmentToPlayer)
        {
            ForwardBackward = displacmentToPlayer;
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
            HaveNoClip = true;
            LocalTime = 0;
        }

        public void FindPathToPlayer(Player player, (int, int) position)
        {
            var searhIn = new Queue<(int, int)>();
            var tracks = new Dictionary<(int, int), LinkedList<(int, int)>>();
            var visited = new HashSet<(int, int)>();

            searhIn.Enqueue(position);
            visited.Add(position);
            tracks.Add(position, new LinkedList<(int, int)>());

            tracks[position].AddLast(position);

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

            if (tracks.ContainsKey(player.MapCoordinate))
                foreach (var point in tracks[player.MapCoordinate])
                    PathToPlayer.AddFirst(point);
            else
                PathToPlayer = new LinkedList<(int, int)>();
        }
    }
}
