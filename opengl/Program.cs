using System;

namespace opengl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Project.Game1())
                game.Run();
        }
    }
}
