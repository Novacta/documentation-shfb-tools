using Novacta.Documentation.ShfbTools;
using System;

namespace ImageToolsInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            // It is assumed that the version of SHFB
            // targeted by the SHFB Tools
            // is installed on the host machine.

            // To install the Image Tools.
            ImageTools.Install();

            Console.ReadKey();
        }
    }
}
