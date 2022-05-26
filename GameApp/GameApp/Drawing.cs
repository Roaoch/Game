using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace SwordAndGun
{
    public static class Drawer
    {
        private static Level Level;
        private static Animation PlayerWalk = new Animation(@"../../../Texture/Walk.png", 3, 0.2f);
        public static Animation PlayerAtack = new Animation(@"../../../Texture/Atack.png", 4, 0.6f);
        private static Animation EnemyWalk = new Animation(@"../../../Texture/EnemyWalk.png", 3, 0.2f);


        public static void Initialize(Level level)
        {
            InitWindow(1280, 720, "Hello World");
            SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            SetWindowState(ConfigFlags.FLAG_VSYNC_HINT);
            SetTargetFPS(60);

            Level = level;

            var enemyHiveMind = Level.EnemyHiveMind;
            var world = Level.World;
            var player = Level.Player;

            var enemy1 = enemyHiveMind.AllEnemies[0];
            var ground = world.WorldPlatforms[0];
            var platform1 = world.WorldPlatforms[1];

            while (!WindowShouldClose())
            {
                BeginDrawing();
                {
                    ClearBackground(Color.WHITE);
                    world.Time += GetFrameTime();

                    if (!player.IsAtacking)
                    {
                        Controller.CheckInputs(player);
                    }

                    player.Update(world);
                    enemyHiveMind.Update(world, player);

                    if (player.IsAtacking)
                    {
                        DrawPlayer(player, PlayerAtack);
                        if (PlayerAtack.Currentframe == PlayerAtack.MaxFrameCount - 1)
                            player.IsAtacking = false;
                    }
                    else if (player.IsMoving && player.CanBeMoved)
                    {
                        DrawPlayer(player, PlayerWalk);
                    }
                    else
                    {
                        DrawTexturePro(PlayerWalk.Texture,
                            new Rectangle(0, 0, PlayerWalk.TextureBox.width, PlayerWalk.TextureBox.height),
                            player.HitBox,
                            Vector2.Zero,
                            0,
                            Color.RAYWHITE);
                    }

                    if (enemy1.IsMoving && enemy1.CanBeMoved)
                    {
                        DrawEnemy(enemy1, EnemyWalk);
                    }
                    else
                    {
                        DrawTexturePro(EnemyWalk.Texture,
                            new Rectangle(0, 0, EnemyWalk.TextureBox.width, EnemyWalk.TextureBox.height),
                            enemy1.HitBox,
                            Vector2.Zero,
                            0,
                            Color.RAYWHITE);
                    }

                    DrawFPS(10, 10);
                    DrawText(player.Velocity.ToString(), 10, 30, 20, Color.LIME);
                    DrawText(player.HitBox.ToString(), 10, 50, 20, Color.LIME);
                    DrawText(enemy1.Hp.ToString(), GetScreenWidth() - 60, 10, 20, Color.RED);

                    DrawRectangleRec(player.HitBox, new Color(255, 0, 0, 50));
                    DrawRectangleRec(ground, Color.BLACK);
                    DrawRectangleRec(player.AtackBox, new Color(230, 0, 0, 50));
                    DrawRectangleRec(platform1, Color.BLACK);
                }
                EndDrawing();
            }

            CloseWindow();
        }

        private static void DrawPlayer(Player player, Animation animation)
        {
            if (Level.World.Time >= animation.TimePerFrame)
            {
                Level.World.Time = 0;
                animation.Currentframe++;
            }

            animation.Currentframe = animation.Currentframe % animation.MaxFrameCount;

            animation.TextureBox.x = animation.TextureBox.width * animation.Currentframe;

            var c = player.HitBox.height / animation.TextureBox.height;

            DrawTexturePro(animation.Texture,
                animation.TextureBox,
                new Rectangle(player.HitBox.x, player.HitBox.y, animation.TextureBox.width * c, animation.TextureBox.height * c),
                Vector2.Zero,
                0,
                Color.RAYWHITE);
        }

        private static void DrawEnemy(Enemy enemy, Animation animation)
        {
            if (Level.World.Time >= animation.TimePerFrame)
            {
                Level.World.Time = 0;
                animation.Currentframe++;
            }

            animation.Currentframe = animation.Currentframe % animation.MaxFrameCount;

            animation.TextureBox.x = animation.TextureBox.width * animation.Currentframe;

            var c = enemy.HitBox.height / animation.TextureBox.height;

            DrawTexturePro(animation.Texture,
                animation.TextureBox,
                new Rectangle(enemy.HitBox.x, enemy.HitBox.y, animation.TextureBox.width * c, animation.TextureBox.height * c),
                Vector2.Zero,
                0,
                Color.RAYWHITE);
        }
    }
}
