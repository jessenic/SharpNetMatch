using System;
using System.Collections.Generic;

namespace SharpNetMatch
{
    /// <summary>
    /// Simple SharpNetMatch application using Microsoft.Xna.Framework.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var program = new SharpNetMatch())
                program.Run();

        }
    }
}