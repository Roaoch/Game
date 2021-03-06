using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System;
using System.Threading.Tasks;

namespace SwordAndGun
{
    public class Player : ICanAtack, IMoveable
    {
        private Vector2 velocity;
        private float hp = 100;
        private float localTime = 0;

        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle HitBox;
        public Rectangle AtackBox;
        public float Hp { get => hp; set => hp = Math.Clamp(value, 0, 100); }
        public float AtackPower { get; set; } = 100;
        public (int, int) MapCoordinate { get; set; }
        public int ForwardBackward { get; private set; } = 1;

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }
        public bool IsMoving { get => velocity != Vector2.Zero; }
        public bool HaveNoClip { get; set; }
        public bool IsDead { get; set; }

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
            if (Hp == 0)
                IsDead = true;

            world.AlongPhysics(this);

            HitBox.x += Velocity.X * GetFrameTime() * 60;
            HitBox.y += Velocity.Y * GetFrameTime() * 60;

            if(velocity.X != 0)
                ForwardBackward = (int)(velocity.X / Math.Abs(velocity.X));

            MapCoordinate = Map.GetCoordinate(this);

            localTime += GetFrameTime();

            if (Drawer.playerAtack.Currentframe == 2)
                AtackBox = World.GenerateAtackBox(this, ForwardBackward);
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
