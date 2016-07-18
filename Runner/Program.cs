using System;
using MultiPlayer;

namespace Runner
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
            var scene = new RunnerGame();
            using (var game = new EnigmaGame(scene))
            {
                game.Run();
            }
        }
    }
#endif
}
