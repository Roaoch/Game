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
        public List<Rectangle> WorldWalls { get; }
        public static Rectangle Exit { get; private set; }
        public static Document Goal { get; private set; }

        private float windageParametr = 0.1f;
        private float frictinParametr = 0.2f;

        public World(List<Rectangle> platforms, List<Rectangle> walls, Document goal, Rectangle exit)
        {
            WorldPlatforms = platforms;
            WorldWalls = walls;
            Goal = goal;
            Exit = exit;
        }

        public void AlongPhysics(IMoveable obj)
        {
            var platfomCollider = CheckPlatfomCollision(obj.GetHitBox());
            if (obj.HaveNoClip || platfomCollider == null)
                AlongGravity(obj);
            else
                CollisePlatfom(obj, platfomCollider.Value);

            var wallCollider = CheckWallCollision(obj.GetHitBox());
            if (wallCollider != null)
                ColliseWall(obj, wallCollider.Value);


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

        public static Rectangle GenerateAtackBox(IMoveable obj, int forwardBackward)
        {
            return new Rectangle(obj.GetHitBox().x + (-60 + forwardBackward * 60) / 2 + (obj.GetHitBox().width + forwardBackward * obj.GetHitBox().width) / 2,
                obj.GetHitBox().y, 60, 200);
        }

        public static bool IsGameOver(Player player, EnemyHiveMind enemyHiveMind)
        {
            if (!Goal.OnTheGround && enemyHiveMind.AllEnemiesAreDead)
                return CheckCollisionRecs(player.HitBox, Exit);
            if(Goal.OnTheGround)
                Goal.OnTheGround = !CheckCollisionRecs(player.HitBox, Goal.HitBox);
            return false;
        }

        private void AlongGravity(IMoveable obj)
        {
            obj.Velocity += GravityForce;
        }

        private Rectangle? CheckPlatfomCollision(Rectangle sourceHitbox)
        {
            var hitBox = new Rectangle(sourceHitbox.x, sourceHitbox.y + (sourceHitbox.height / 4) * 3, sourceHitbox.width, sourceHitbox.height / 4);
            foreach (var platform in WorldPlatforms)
            {
                if (CheckCollisionRecs(hitBox, platform))
                    return platform;
            }
            return null;
        }

        private Rectangle? CheckWallCollision(Rectangle sourceHitbox)
        {
            foreach(var wall in WorldWalls)
            {
                if (CheckCollisionRecs(sourceHitbox, wall))
                    return wall;
            }
            return null;
        }

        private void CollisePlatfom(IMoveable obj, Rectangle collider)
        {
            obj.Velocity = Vector2.UnitX * obj.Velocity;
            obj.Move(obj.GetHitBox().x, collider.y - obj.GetHitBox().height + 1);
            obj.CanBeMoved = true;
        }

        private void ColliseWall(IMoveable obj, Rectangle collider)
        {
            obj.Velocity = Vector2.UnitY * obj.Velocity;
            var rightEdge = collider.x + collider.width;
            var middle = collider.x + collider.width / 2;
            if (middle < obj.GetHitBox().x && rightEdge >= obj.GetHitBox().x)
                obj.Move(rightEdge, obj.GetHitBox().y);
            else
                obj.Move(collider.x - obj.GetHitBox().width, obj.GetHitBox().y);
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
