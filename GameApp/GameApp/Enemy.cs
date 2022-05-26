using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
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
        public float Hp { get => hp; set => /*Math.Clamp(value, 0, 100)*/hp = value; }
        public float AtackPower { get; set; } = 50;

        public bool CanBeMoved { get; set; }
        public bool IsAtacking { get; set; }
        public bool IsMoving { get => velocity != Vector2.Zero; }
        public bool HaveNoClip { get; set; } = false;

        public Enemy(float x, float y)
        {
            Velocity = new Vector2(0, 0);
            HitBox = new Rectangle(x, y, 256, 256);
            AtackBox = default(Rectangle);
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
    }
}
