using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System;
using System.Threading.Tasks;

namespace SwordAndGun
{
    public class Player : IMoveable
    {
        private Vector2 velocity;
        private float hp = 100;
        private float localTime = 0;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle HitBox;
        public Rectangle AtackBox;
        public float Hp { get => hp; set => Math.Clamp(value, 0, 100); }
        public float AtackPower { get; set; } = 100;
        public (int, int) MapCoordinate { get; set; }

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }
        public bool IsMoving { get => velocity != Vector2.Zero; }
        public bool HaveNoClip { get; set; } = false;

        public Player(float x, float y)
        {
            Velocity = new Vector2(0, 0);
            HitBox = new Rectangle(x, y, 256, 256);
            MapCoordinate = Map.GetCoordinate(this);
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

            localTime += GetFrameTime();

            if (Drawer.PlayerAtack.Currentframe == 2)
                AtackBox = new Rectangle(HitBox.x + HitBox.width, HitBox.y, 60, 200);
            else
                AtackBox = default(Rectangle);

            if(HaveNoClip && localTime >= 0.12)
            {
                HaveNoClip = false;
                localTime = 0;
            }
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
            HaveNoClip = true;
            localTime = 0;
        }
    }
}
