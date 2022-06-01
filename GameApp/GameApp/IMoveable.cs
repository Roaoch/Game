using System;
using Raylib_cs;
using System.Numerics;

public interface IMoveable
{
    Vector2 Velocity { get; set; }
    bool IsMoving { get; }
    bool CanBeMoved { get; set; }
    bool HaveNoClip { get; set; }


    Rectangle GetHitBox();
    void Move(Vector2 vector);
    void Move(float x, float y);
    
}
