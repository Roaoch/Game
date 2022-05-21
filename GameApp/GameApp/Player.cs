using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace SwordAndGun
{
    public class Player : IMoveable
    {
        private Vector2 velocity;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle HitBox;

        public Player()
        {
            Velocity = new Vector2(0, 0);
            HitBox = new Rectangle(200, 200, 256, 256);
        }

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }
        public bool IsMoving { get => velocity != Vector2.Zero; }

        public void Move(Vector2 vector)
        {
            velocity += vector;
        }
        public void Move(float x, float y)
        {
            HitBox.x = x;
            HitBox.y = y;
        }
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
