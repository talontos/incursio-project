using System;

namespace Incursio
{
    static class IncursioProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Incursio game = new Incursio())
            {
                game.Run();
            }
        }
    }
}

