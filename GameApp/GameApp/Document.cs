using System;
using Raylib_cs;

public class Document
{
	public bool OnTheGround { get; set; } = true;
	public Rectangle HitBox { get; set; }

	public Document(float x, float y)
    {
		HitBox = new Rectangle(x, y, 100, 40);
    }
}
