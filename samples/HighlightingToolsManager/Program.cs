using Novacta.Documentation.ShfbTools;
using System;
using System.Collections.Generic;

namespace HighlightingToolsManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // It is assumed that the version of SHFB
            // targeted by the SHFB Tools
            // is installed on the host machine.

            // To highlight specific class names,
            // execute the following steps, A to B.

            // A. Set a color to be
            // applied when highlighting.
            // See the documentation for a list of 
            // supported colors.
            HighlightingTools.SetClassNamesColor("MediumAquaMarine");

            // B. Add the class names to be highlighted.

            // B.1 In SHFB, class names are grouped by
            // an identifier referred to as a "family".
            string family = "sampleFamily";

            // B.2 List the class names.
            List<string> names = new List<string>() {
                "IntegerOperation",
                "IntegerArrayOperation"
            };

            // B.3 In SHFB, keyword families are linked to
            // languages.
            // List the languages for which you want
            // the class names highlighted.
            // See the documentation for a list of 
            // supported languages.
            List<string> languages = new List<string>() {
                "cs",
                "vbnet"
            };

            // B.4 Update your SHFB installation so that
            // your class names are recognized as a family
            // of keywords for the specified languages.
            HighlightingTools.AddClassNamesFamily(
                family,
                names,
                languages);

            // To remove a family of class names,
            // so that it will no longer be highlighted,
            // uncomment the following line.
            //HighlightingTools.RemoveClassNamesFamily(family);

            Console.ReadKey();
        }
    }
}
