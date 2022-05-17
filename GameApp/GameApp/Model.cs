using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace SwordAndGun
{
    public static class World
    {
        public static Vector2 GravityForce { get; set; } = new Vector2(0, 3);
        private static List<Rectangle> platforms = new List<Rectangle>();
        private static float windage = 2;

        public static void MoveAlongPhysics(IMoveable obj)
        {
            foreach (var platform in platforms)
            {
                if (!CheckCollisionRecs(obj.GetHitBox(), platform))
                    obj.Velocity += GravityForce;
            }
        }

        public static void Collise(IMoveable obj)
        {
            var hitbox = obj.GetHitBox();
            foreach (var platform in platforms)
            {
                if (CheckCollisionRecs(hitbox, platform))
                {
                    obj.Velocity = new Vector2(obj.Velocity.X, 0);
                    obj.Move(hitbox.x, platform.y - hitbox.height);
                    obj.CanBeMoved = true;
                }
            }
        }

        public static void AlongWindage(IMoveable obj)
        {
            var velocity = obj.Velocity;
            if (velocity != Vector2.Zero)
            {
                if (velocity.X > 0)
                    obj.Velocity -= Vector2.UnitX * windage;
                if (velocity.X < 0)
                    obj.Velocity += Vector2.UnitX * windage;
                if (velocity.Y > 0)
                    obj.Velocity -= Vector2.UnitY * windage;
                if (velocity.Y < 0)
                    obj.Velocity += Vector2.UnitY * windage;
            }
        }

        public static void CreatePlatform(Rectangle platform)
        {
            platforms.Add(platform);
        }
    }
    public class Player : IMoveable
    {
        private Vector2 velocity;
        private int maxVelocity = 15;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { if (Math.Abs(value.X) < maxVelocity) velocity = value; }
        }
        public Rectangle HitBox;

        public Player()
        {
            Velocity = new Vector2(0, 0);
            HitBox = new Rectangle(200, 200, 256, 256);
        }

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }

        public void Move()
        {
            HitBox.x += Velocity.X * GetFrameTime() * 60;
            HitBox.y += Velocity.Y * GetFrameTime() * 60;

            World.AlongWindage(this);

            World.Collise(this);
        }

        public Rectangle GetHitBox()
        {
            return HitBox;
        }

        public void Move(float x, float y)
        {
            HitBox.x = x;
            HitBox.y = y;
        }

        public void Atack()
        {
            IsAtacking = true;
        }
    }
}
