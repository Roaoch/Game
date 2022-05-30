using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace SwordAndGun
{
    public class World
    {
        public Vector2 GravityForce { get; } = new Vector2(0, 3);
        public float Time { get; set; } = 0;
        public List<Rectangle> WorldPlatforms { get; }
        public List<Rectangle> WorldWall { get; }

        private float windageParametr = 0.1f;
        private float frictinParametr = 0.2f;

        public World(List<Rectangle> platforms)
        {
            WorldPlatforms = platforms;
        }

        public void AlongPhysics(IMoveable obj)
        {
            var collider = CheckWorldCollision(obj);
            if (obj.HaveNoClip || collider == null)
                AlongGravity(obj);
            else
                Collise(obj, collider.Value);

            if (!obj.CanBeMoved)
                AlongWindage(obj);
            else
                AlongFriction(obj);

            ClampVelocityToZero(obj);
        }

        public void ClampVelocityToZero(IMoveable obj)
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
        private void AlongGravity(IMoveable obj)
        {
            obj.Velocity += GravityForce;
        }

        private Rectangle? CheckWorldCollision(IMoveable obj)
        {
            var sourceHitbox = obj.GetHitBox();
            var hitBox = new Rectangle(sourceHitbox.x, sourceHitbox.y + (sourceHitbox.height / 4) * 3, sourceHitbox.width, sourceHitbox.height / 4);
            foreach (var platform in WorldPlatforms)
            {
                if (CheckCollisionRecs(hitBox, platform))
                    return platform;
            }
            return null;
        }

        private void Collise(IMoveable obj, Rectangle Collider)
        {
            obj.Velocity = new Vector2(obj.Velocity.X, 0);
            obj.Move(obj.GetHitBox().x, Collider.y - obj.GetHitBox().height + 1);
            obj.CanBeMoved = true;
        }

        private void AlongWindage(IMoveable obj)
        {
            var windageForce = obj.Velocity * obj.Velocity * 0.05f * windageParametr;

            var parY = GetDirection(obj.Velocity.Y);
            var parX = GetDirection(obj.Velocity.X);
            obj.Velocity += windageForce * new Vector2(-parX * 1.7f, -parY * 0.6f);
        }
        private void AlongFriction(IMoveable obj)
        {
            obj.Velocity -= new Vector2(obj.Velocity.X * frictinParametr, 0);
        }

        private float GetDirection(float x)
        {
            return x == 0 ? 0 : x / Math.Abs(x);
        }
    }
}
