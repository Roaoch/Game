using Raylib_cs;
using static Raylib_cs.Raylib;

namespace SwordAndGun
{
    public class Animation
    {
        public Texture2D Texture;
        public Rectangle TextureBox;
        public int Currentframe = 0;
        public int MaxFrameCount;
        public float TimePerFrame;
        public float LocalTimer;

        public Animation(string path, int frameCount, float timePerFrame)
        {
            Texture = LoadTexture(path);
            TextureBox = new Rectangle(0, 0, Texture.width / frameCount, Texture.height);
            MaxFrameCount = frameCount;
            TimePerFrame = timePerFrame;
        }
    }
}