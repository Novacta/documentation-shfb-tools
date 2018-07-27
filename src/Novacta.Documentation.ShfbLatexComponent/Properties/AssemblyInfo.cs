using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General assembly information
[assembly: AssemblyTitle(AssemblyInfo.Title)]
[assembly: AssemblyDescription(AssemblyInfo.Description)]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany(AssemblyInfo.Company)]
[assembly: AssemblyProduct(AssemblyInfo.Title)]
[assembly: AssemblyCopyright(AssemblyInfo.Copyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// Resources contained within the assembly are English
[assembly: NeutralResourcesLanguageAttribute("en")]

[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.Version)]

// This defines constants that can be used here and in the custom presentation style export attribute
internal static class AssemblyInfo
{
    // Company
    public const string Company = "Novacta";

    // Product Name
    public const string Title = "Novacta.Documentation.ShfbLatexComponent";

    // Product version
    public const string Version = "2018.7.8.0";

    // Product description
    public const string Description =
        "Provides support for adding LaTeX formatted formulas in " +
        "reference XML comments and conceptual content topics created with " +
        "Sandcastle Help File Builder.";

    // Assembly copyright information
    public const string Copyright = "Copyright \xA9 2018, Giovanni Lafratta, All Rights Reserved.";
}
