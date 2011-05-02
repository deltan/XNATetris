using System;

namespace deltan.XNATetris
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (XNATetris game = new XNATetris())
            {
                game.Run();
            }
        }
    }
}

