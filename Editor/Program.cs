using System;
using MultiPlayer;

namespace Editor
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var scene = new EditorGame();
            using (var game = new EnigmaGame(scene))
                game.Run();
        }
    }
#endif
}
