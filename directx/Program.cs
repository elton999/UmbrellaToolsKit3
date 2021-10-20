using System;

namespace directx
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameProject.Game1())
                game.Run();
        }
    }
}
