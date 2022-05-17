using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace SwordAndGun
{
    public class Animation
    {
        public Texture2D Texture;
        public Rectangle TextureBox;
        public static float Time = 0;
        public int Currentframe = 0;
        public int MaxFrameCount;
        public float TimePerFrame;

        public Animation(string path, int frameCount, float timePerFrame)
        {
            Texture = LoadTexture(path);
            TextureBox = new Rectangle(0, 0, Texture.width / frameCount, Texture.height);
            MaxFrameCount = frameCount;
            TimePerFrame = timePerFrame;
        }
    }

    public static class Drawer
    {
        //private static Texture2D playerTexture = LoadTexture(@".\Texture\Walk.png");
        //private static Rectangle playerTextureBox = new Rectangle(0, 0, 300, 512);
        //private static float timer = 0;
        //private static float frame = 0;

        public static void Initialize()
        {
            InitWindow(1280, 720, "Hello World");
            SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            SetWindowState(ConfigFlags.FLAG_VSYNC_HINT);

            var playerWalk = new Animation(@".\Texture\Walk.png", 3, 0.1f);
            var playerAtack = new Animation(@".\Texture\Atack.png", 4, 0.5f);

            var player = new Player();
            var ground = new Rectangle(0, 600, GetScreenWidth(), 40);
            World.CreatePlatform(ground);

            while (!WindowShouldClose())
            {
                BeginDrawing();
                {
                    ClearBackground(Color.WHITE);
                    Animation.Time += GetFrameTime();

                    if (!player.IsAtacking)
                    {
                        Controller.CheckInputs(player);
                    }

                    World.MoveAlongPhysics(player);
                    player.Move();

                    if (player.IsAtacking)
                    {
                        DrawPlayer(player, playerAtack);
                        if (playerAtack.Currentframe == playerAtack.MaxFrameCount - 1)
                            player.IsAtacking = false;
                    }
                    else if (player.Velocity.X != 0)
                    {
                        DrawPlayer(player, playerWalk);
                    }
                    else
                    {
                        DrawTexturePro(playerWalk.Texture,
                            new Rectangle(0, 0, playerWalk.TextureBox.width, playerWalk.TextureBox.height),
                            player.HitBox,
                            Vector2.Zero,
                            0,
                            Color.RAYWHITE);
                    }

                    DrawFPS(10, 10);
                    DrawRectangleRec(player.HitBox, new Color(255, 0, 0, 50));
                    DrawRectangleRec(ground, Color.BLACK);
                }
                EndDrawing();
            }

            CloseWindow();
        }

        private static void DrawPlayer(Player player, Animation animation)
        {
            if (Animation.Time >= 0.35)
            {
                Animation.Time = 0;
                animation.Currentframe++;
            }

            animation.Currentframe = animation.Currentframe % animation.MaxFrameCount;

            animation.TextureBox.x = animation.TextureBox.width * animation.Currentframe;

            DrawTexturePro(animation.Texture, 
                animation.TextureBox, 
                player.HitBox, 
                Vector2.Zero, 
                0, 
                Color.RAYWHITE);
        }
    }
}
