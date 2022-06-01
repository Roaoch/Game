using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace SwordAndGun
{
    public static class Drawer
    {
        private static Level Level;

        private static Animation PlayerWalk = new Animation(@"../../../Texture/Walk.png", 3, 0.2f);
        public static Animation PlayerAtack = new Animation(@"../../../Texture/Atack.png", 4, 0.5f);
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

            var camera = new Camera2D();
            camera.zoom = 0.78f;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                {
                    BeginMode2D(camera);
                    {
                        camera.target = CameraToEntity(camera, player);

                        ClearBackground(Color.WHITE);
                        world.Time += GetFrameTime();
                        UpAllTimers();

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
                            {
                                PlayerAtack.Currentframe = 0;
                                player.IsAtacking = false;
                            }
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

                        foreach (var enemy in enemyHiveMind.AllEnemies)
                        {
                            if (enemy.IsMoving && enemy.CanBeMoved)
                            {
                                DrawEnemy(enemy, EnemyWalk);
                            }
                            else
                            {
                                DrawTexturePro(EnemyWalk.Texture,
                                    new Rectangle(0, 0, EnemyWalk.TextureBox.width, EnemyWalk.TextureBox.height),
                                    enemy.HitBox,
                                    Vector2.Zero,
                                    0,
                                    Color.RAYWHITE);
                            }
                        }

                        foreach (var platform in world.WorldPlatforms)
                        {
                            DrawRectangleRec(platform, Color.DARKGRAY);
                        }

                        foreach (var wall in world.WorldWalls)
                        {
                            DrawRectangleRec(wall, Color.BLACK);
                        }
                    }
                    EndMode2D();

                    DrawFPS(10, 10);
                    DrawText(player.Velocity.ToString(), 10, 30, 20, Color.LIME);
                    DrawText(player.HitBox.ToString(), 10, 50, 20, Color.LIME);
                }
                EndDrawing();
            }

            CloseWindow();
        }

        private static void UpAllTimers()
        {
            var frameTime = GetFrameTime();
            PlayerWalk.LocalTimer += frameTime;
            PlayerAtack.LocalTimer += frameTime;
            EnemyWalk.LocalTimer += frameTime;
            //EnemyAtack.LocalTimer += frameTime;
        }

        private static void DrawPlayer(Player player, Animation animation)
        {
            if (animation.LocalTimer >= animation.TimePerFrame)
            {
                animation.LocalTimer = 0;
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
            if (animation.LocalTimer >= animation.TimePerFrame)
            {
                animation.LocalTimer = 0;
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

        private static Vector2 CameraToEntity(Camera2D camera, IMoveable player)
        {
            var x = player.GetHitBox().x - GetScreenWidth() / 4;
            var y = player.GetHitBox().y - GetScreenHeight() / 2;
            return (new Vector2(x, y) + camera.target) / 2;
        }
    }
}
