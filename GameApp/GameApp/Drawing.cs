using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace SwordAndGun
{
    public static class Drawer
    {
        private static Level Level;

        private static Animation playerWalk = new Animation(@"../../../Texture/Walk.png", 3, 0.2f);
        public static Animation playerAtack = new Animation(@"../../../Texture/Atack.png", 4, 0.5f);
        private static Animation enemyWalk = new Animation(@"../../../Texture/EnemyWalk.png", 3, 0.2f);
        public static Animation enemyAtack = new Animation(@"../../../Texture/EnemyAtack.png", 4, 0.8f);

        private static bool inMainMenu = true;
        private static bool playerWin = false;

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
            camera.zoom = 1f;

            while (!WindowShouldClose())
            {
                BeginDrawing();
                {
                    if (inMainMenu)
                    {
                        ClearBackground(Color.BLACK);

                        DrawText("Kill All Enemies and Find the Document", GetScreenWidth() / 2 - 320, GetScreenHeight() / 2 - 100, 35, Color.WHITE);
                        DrawText("press ( Space )", GetScreenWidth() / 2 - 100, GetScreenHeight() / 2, 25, Color.GRAY);

                        if (Controller.IsButtonForSkipMenuPressed())
                            inMainMenu = false;
                    }
                    else if (player.IsDead)
                    {
                        ClearBackground(Color.BLACK);

                        DrawText("Mission Failed, we'll get em next time", GetScreenWidth() / 2 - 320, GetScreenHeight() / 2 - 100, 35, Color.WHITE);
                        DrawText("press ( Space )", GetScreenWidth() / 2 - 100, GetScreenHeight() / 2, 25, Color.GRAY);

                        if (Controller.IsButtonForSkipMenuPressed())
                        {
                            player.IsDead = false;
                            player.Hp = 100;
                        }
                    }
                    else if (playerWin)
                    {
                        ClearBackground(Color.BLACK);

                        DrawText("Mission Accomplished!", GetScreenWidth() / 2 - 200, GetScreenHeight() / 2 - 50, 50, Color.WHITE);
                    }
                    else
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

                            DrawEntity(player, player.ForwardBackward, playerWalk, playerAtack);

                            foreach (var enemy in enemyHiveMind.AllEnemies)
                            {
                                DrawEntity(enemy, enemy.ForwardBackward, enemyWalk, enemyAtack);
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
                        DrawText("HP = " + player.Hp.ToString() + "%", 10, 30, 20, Color.LIME);
                        DrawText(enemyAtack.Currentframe.ToString(), 10, 50, 20, Color.LIME);
                    }
                    EndDrawing();
                }
            }

            CloseWindow();
        }

        private static void UpAllTimers()
        {
            var frameTime = GetFrameTime();
            playerWalk.LocalTimer += frameTime;
            playerAtack.LocalTimer += frameTime;
            enemyWalk.LocalTimer += frameTime;
            enemyAtack.LocalTimer += frameTime;
        }

        private static void DrawAnimation(IMoveable entity, Animation animation, int forwardBeckward)
        {
            if (animation.LocalTimer >= animation.TimePerFrame)
            {
                animation.LocalTimer = 0;
                animation.Currentframe++;
            }

            animation.Currentframe = animation.Currentframe % animation.MaxFrameCount;

            animation.TextureBox.x = animation.TextureBox.width * animation.Currentframe;

            var c = entity.GetHitBox().height / animation.TextureBox.height;

            DrawTexturePro(animation.Texture,
                new Rectangle(animation.TextureBox.x, animation.TextureBox.y, forwardBeckward * animation.TextureBox.width, animation.TextureBox.height),
                new Rectangle(entity.GetHitBox().x, entity.GetHitBox().y, animation.TextureBox.width * c, animation.TextureBox.height * c),
                Vector2.Zero,
                0,
                Color.RAYWHITE);
        }

        private static void DrawEntity(ICanAtack entity, int forwardBackward, Animation walk, Animation atack)
        {
            if (entity.IsAtacking)
            {
                DrawAnimation(entity, atack, forwardBackward);
                if (atack.Currentframe == atack.MaxFrameCount - 1)
                {
                    atack.Currentframe = 0;
                    entity.IsAtacking = false;
                }
            }
            else if (entity.IsMoving && entity.CanBeMoved)
            {
                DrawAnimation(entity, walk, forwardBackward);
            }
            else
            {
                DrawTexturePro(walk.Texture,
                    new Rectangle(0, 0, forwardBackward * walk.TextureBox.width, walk.TextureBox.height),
                    entity.GetHitBox(),
                    Vector2.Zero,
                    0,
                    Color.RAYWHITE);
            }
        }

        private static Vector2 CameraToEntity(Camera2D camera, IMoveable player)
        {
            var x = player.GetHitBox().x - GetScreenWidth() / 4;
            var y = player.GetHitBox().y - GetScreenHeight() / 2;
            return (new Vector2(x, y) + camera.target) / 2;
        }
    }
}
