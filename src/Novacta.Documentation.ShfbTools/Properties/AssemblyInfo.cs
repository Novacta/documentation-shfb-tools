﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
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

[assembly: InternalsVisibleTo("Novacta.Documentation.ShfbTools.Tests")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("79bee57c-2170-4564-a25c-acd8647a2f90")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.Version)]

internal static class AssemblyInfo
{
    // Company
    public const string Company = "Giovanni Lafratta";

    // Product Name
    public const string Title = "Novacta.Documentation.ShfbTools";

    // Product version
    public const string Version = "2021.4.9.0";

    // Product description
    public const string Description =
        "Enhances Sandcastle Help File Builder " +
        "by providing support for Latex formulas, " +
        "image management in reference topics, " +
        "and class name highlighting in code examples.";
    
    // Assembly copyright information
    public const string Copyright = "Copyright \xA9 2018, Giovanni Lafratta, All Rights Reserved.";
}

