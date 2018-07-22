using Novacta.Documentation.ShfbTools;
using System;

namespace LatexToolsInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            // It is assumed that the version of SHFB
            // targeted by the SHFB Tools
            // is installed on the host machine.

            // To install the Latex Tools.
            LatexTools.Install();

            Console.ReadKey();
        }
    }
}
