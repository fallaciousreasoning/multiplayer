﻿using System;
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
            using (var game = new RunnerGame())
            {
                game.Run();
            }
        }
    }
#endif
}