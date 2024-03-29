﻿using System;
using System.Diagnostics;
using MultiPlayer.Core.InputMethods;
using MultiPlayer.Core.Systems;

namespace MultiPlayer
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
            using (var game = new EnigmaGame(new Scene(new XnaMouse(), new XnaKeyboard())))
                game.Run();
        }
    }
#endif
}
