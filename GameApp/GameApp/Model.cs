using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace SwordAndGun
{
    public static class World
    {
        public static Vector2 GravityForce { get; } = new Vector2(0, 3);
        public static float Time { get; set; } = 0;
        private static List<Rectangle> platforms = new List<Rectangle>();
        private static float windageParametr = 0.1f;
        private static float frictinParametr = 0.2f;

        public static void AlongPhysics(IMoveable obj)
        {
            var collider = CheckWorldCollision(obj);
            if (collider == null)
                AlongGravity(obj);
            else
                Collise(obj, collider.Value);

            if (!obj.CanBeMoved)
                AlongWindage(obj);
            else
                AlongFriction(obj);

            ClampVelocityToZero(obj);
        }

        public static void CreatePlatform(Rectangle platform)
        {
            platforms.Add(platform);
        }

        public static void ClampVelocityToZero(IMoveable obj)
        {
            var epsilon = 1;
            if(Math.Abs(obj.Velocity.X) <= epsilon)
            {
                obj.Velocity = new Vector2(0, obj.Velocity.Y);
            }
            if(Math.Abs(obj.Velocity.Y) <= epsilon)
            {
                obj.Velocity = new Vector2(obj.Velocity.X, 0);
            }
        }
        private static void AlongGravity(IMoveable obj)
        {
            obj.Velocity += GravityForce;
        }

        private static Rectangle? CheckWorldCollision(IMoveable obj)
        {
            foreach(var platform in platforms)
            {
                if (CheckCollisionRecs(obj.GetHitBox(), platform))
                    return platform;
            }
            return null;
        }

        private static void Collise(IMoveable obj, Rectangle Collider)
        {
            obj.Velocity = new Vector2(obj.Velocity.X, 0);
            obj.Move(obj.GetHitBox().x, Collider.y - obj.GetHitBox().height + 1);
            obj.CanBeMoved = true;
        }

        private static void AlongWindage(IMoveable obj)
        {
            var windageForce = obj.Velocity * obj.Velocity * 0.05f * windageParametr;

            var parY = GetDirection(obj.Velocity.Y);
            obj.Velocity += windageForce * new Vector2(0, -parY);
        }
        private static void AlongFriction(IMoveable obj)
        {
            obj.Velocity -= new Vector2(obj.Velocity.X * frictinParametr, 0);
        }

        private static float GetDirection(float x)
        {
            return x == 0 ? 0 : x / Math.Abs(x);
        }
    }

    public class Player : IMoveable
    {
        private Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set 
            {
                velocity = value;
            }
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
            World.AlongPhysics(this);

            HitBox.x += Velocity.X * GetFrameTime() * 60;
            HitBox.y += Velocity.Y * GetFrameTime() * 60;
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
            Velocity = Vector2.Zero;
            IsAtacking = true;
        }

        public void Jump()
        {
            CanBeMoved = false;
            Move(HitBox.x, HitBox.y - 2);
        }
    }
}
