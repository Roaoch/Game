using Raylib_cs;

namespace SwordAndGun
{
    public static class Drawer
    {
        public static void Initialize()
        {
            Raylib.InitWindow(1280, 720, "Hello World");
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                    Raylib.ClearBackground(Color.WHITE);


                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
