using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using System.Collections.Generic;

namespace SwordAndGun
{
    public static class Controller
    {
        static Dictionary<KeyboardKey, Vector2> PlayerMovements = new Dictionary<KeyboardKey, Vector2>() 
        {
            { KeyboardKey.KEY_D, new Vector2(4, 0) },
            { KeyboardKey.KEY_A, new Vector2(-4, 0) },
            { KeyboardKey.KEY_SPACE, new Vector2(0, -60) }
        };
        public static void OnKeyDowned(KeyboardKey key, Player player)
        {
            player.Velocity += PlayerMovements[key];
        }

        public static void CheckInputs(Player player)
        {
            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                player.Atack();
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                OnKeyDowned(KeyboardKey.KEY_D, player);
            }
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                OnKeyDowned(KeyboardKey.KEY_A, player);
            }
            if (IsKeyDown(KeyboardKey.KEY_SPACE) && player.CanBeMoved)
            {
                player.CanBeMoved = false;
                OnKeyDowned(KeyboardKey.KEY_SPACE, player);
            }
        }
    }
}
