// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using Novacta.Documentation.ShfbTools.FileManagers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Provides methods to manage the highlighting of class names in
    /// SHFB documentation files.
    /// </summary>
    public static class HighlightingTools
    {
        #region COLOR SETTINGS

        /// <summary>
        /// Sets the color for highlighting class names.
        /// </summary>
        /// <param name="color">The name of the color to set for highlighting class names.</param>
        /// <returns>
        /// A value equal to <c>0</c> for successful operations; nonzero otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method adds a new CSS class representing the color you 
        /// want to be applied when representing a class name, as the following:
        /// <code language="none">
        /// .highlight-class-name { color: #2B91AF; }
        /// </code>
        /// </para>
        /// <para>
        /// The colors supported for class name highlighting are those
        /// having CSS names supported by all browsers as of date 3/21/2017.
        /// </para>
        /// <para>
        /// More thoroughly, supported colors are named as follows.
        /// <list type="table">
        /// <item>
        /// <description><c>"AliceBlue"</c></description>
        /// <description><c>"AntiqueWhite"</c></description>
        /// <description><c>"Aqua"</c></description>
        /// <description><c>"Aquamarine"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Azure"</c></description>
        /// <description><c>"Beige"</c></description>
        /// <description><c>"Bisque"</c></description>
        /// <description><c>"Black"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"BlanchedAlmond"</c></description>
        /// <description><c>"Blue"</c></description>
        /// <description><c>"BlueViolet"</c></description>
        /// <description><c>"Brown"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"BurlyWood"</c></description>
        /// <description><c>"CadetBlue"</c></description>
        /// <description><c>"Chartreuse"</c></description>
        /// <description><c>"Chocolate"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Coral"</c></description>
        /// <description><c>"CornflowerBlue"</c></description>
        /// <description><c>"Cornsilk"</c></description>
        /// <description><c>"Crimson"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Cyan"</c></description>
        /// <description><c>"DarkBlue"</c></description>
        /// <description><c>"DarkCyan"</c></description>
        /// <description><c>"DarkGoldenRod"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DarkGray"</c></description>
        /// <description><c>"DarkGrey"</c></description>
        /// <description><c>"DarkGreen"</c></description>
        /// <description><c>"DarkKhaki"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DarkMagenta"</c></description>
        /// <description><c>"DarkOliveGreen"</c></description>
        /// <description><c>"DarkOrange"</c></description>
        /// <description><c>"DarkOrchid"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DarkRed"</c></description>
        /// <description><c>"DarkSalmon"</c></description>
        /// <description><c>"DarkSeaGreen"</c></description>
        /// <description><c>"DarkSlateBlue"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DarkSlateGray"</c></description>
        /// <description><c>"DarkSlateGrey"</c></description>
        /// <description><c>"DarkTurquoise"</c></description>
        /// <description><c>"DarkViolet"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DeepPink"</c></description>
        /// <description><c>"DeepSkyBlue"</c></description>
        /// <description><c>"DimGray"</c></description>
        /// <description><c>"DimGrey"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"DodgerBlue"</c></description>
        /// <description><c>"FireBrick"</c></description>
        /// <description><c>"FloralWhite"</c></description>
        /// <description><c>"ForestGreen"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Fuchsia"</c></description>
        /// <description><c>"Gainsboro"</c></description>
        /// <description><c>"GhostWhite"</c></description>
        /// <description><c>"Gold"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"GoldenRod"</c></description>
        /// <description><c>"Gray"</c></description>
        /// <description><c>"Grey"</c></description>
        /// <description><c>"Green"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"GreenYellow"</c></description>
        /// <description><c>"HoneyDew"</c></description>
        /// <description><c>"HotPink"</c></description>
        /// <description><c>"IndianRed"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Indigo"</c></description>
        /// <description><c>"Ivory"</c></description>
        /// <description><c>"Khaki"</c></description>
        /// <description><c>"Lavender"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"LavenderBlush"</c></description>
        /// <description><c>"LawnGreen"</c></description>
        /// <description><c>"LemonChiffon"</c></description>
        /// <description><c>"LightBlue"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"LightCoral"</c></description>
        /// <description><c>"LightCyan"</c></description>
        /// <description><c>"LightGoldenRodYellow"</c></description>
        /// <description><c>"LightGray"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"LightGrey"</c></description>
        /// <description><c>"LightGreen"</c></description>
        /// <description><c>"LightPink"</c></description>
        /// <description><c>"LightSalmon"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"LightSeaGreen"</c></description>
        /// <description><c>"LightSkyBlue"</c></description>
        /// <description><c>"LightSlateGray"</c></description>
        /// <description><c>"LightSlateGrey"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"LightSteelBlue"</c></description>
        /// <description><c>"LightYellow"</c></description>
        /// <description><c>"Lime"</c></description>
        /// <description><c>"LimeGreen"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Linen"</c></description>
        /// <description><c>"Magenta"</c></description>
        /// <description><c>"Maroon"</c></description>
        /// <description><c>"MediumAquaMarine"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"MediumBlue"</c></description>
        /// <description><c>"MediumOrchid"</c></description>
        /// <description><c>"MediumPurple"</c></description>
        /// <description><c>"MediumSeaGreen"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"MediumSlateBlue"</c></description>
        /// <description><c>"MediumSpringGreen"</c></description>
        /// <description><c>"MediumTurquoise"</c></description>
        /// <description><c>"MediumVioletRed"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"MidnightBlue"</c></description>
        /// <description><c>"MintCream"</c></description>
        /// <description><c>"MistyRose"</c></description>
        /// <description><c>"Moccasin"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"NavajoWhite"</c></description>
        /// <description><c>"Navy"</c></description>
        /// <description><c>"OldLace"</c></description>
        /// <description><c>"Olive"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"OliveDrab"</c></description>
        /// <description><c>"Orange"</c></description>
        /// <description><c>"OrangeRed"</c></description>
        /// <description><c>"Orchid"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"PaleGoldenRod"</c></description>
        /// <description><c>"PaleGreen"</c></description>
        /// <description><c>"PaleTurquoise"</c></description>
        /// <description><c>"PaleVioletRed"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"PapayaWhip"</c></description>
        /// <description><c>"PeachPuff"</c></description>
        /// <description><c>"Peru"</c></description>
        /// <description><c>"Pink"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Plum"</c></description>
        /// <description><c>"PowderBlue"</c></description>
        /// <description><c>"Purple"</c></description>
        /// <description><c>"RebeccaPurple"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Red"</c></description>
        /// <description><c>"RosyBrown"</c></description>
        /// <description><c>"RoyalBlue"</c></description>
        /// <description><c>"SaddleBrown"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Salmon"</c></description>
        /// <description><c>"SandyBrown"</c></description>
        /// <description><c>"SeaGreen"</c></description>
        /// <description><c>"SeaShell"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Sienna"</c></description>
        /// <description><c>"Silver"</c></description>
        /// <description><c>"SkyBlue"</c></description>
        /// <description><c>"SlateBlue"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"SlateGray"</c></description>
        /// <description><c>"SlateGrey"</c></description>
        /// <description><c>"Snow"</c></description>
        /// <description><c>"SpringGreen"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"SteelBlue"</c></description>
        /// <description><c>"Tan"</c></description>
        /// <description><c>"Teal"</c></description>
        /// <description><c>"Thistle"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"Tomato"</c></description>
        /// <description><c>"Turquoise"</c></description>
        /// <description><c>"Violet"</c></description>
        /// <description><c>"Wheat"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"White"</c></description>
        /// <description><c>"WhiteSmoke"</c></description>
        /// <description><c>"Yellow"</c></description>
        /// <description><c>"YellowGreen"</c></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The environmental variable <c>SHFBROOT</c> cannot be found.<br/>
        /// -or-<br/>
        /// The environmental variable <c>SHFBROOT</c> exists but points to
        /// a SHFB installation which is corrupted or has a version different from the
        /// target one.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The value of <paramref name="color"/> is not supported.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="color"/> is <b>null</b>.
        /// </exception>
        /// <seealso href="https://www.w3schools.com/cssref/css_colors.asp"/>
        public static int SetClassNamesColor(string color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }
            if (Shfb.SHFBROOT is null)
            {
                throw new InvalidOperationException(
                    "The environmental variable SHFBROOT cannot be found. " +
                    String.Format(
                        "Please, install SHFB version {0} and try again.",
                        Shfb.TargetVersion));
            }
            return HighlightingTools.SetClassNamesColor(color, Shfb.SHFBROOT);
        }

        /// <summary>
        /// Sets the class name highlighting color for a SHFB installation in the specified path.
        /// </summary>
        /// <param name="color">The color to set for class name highlighting.</param>
        /// <param name="path">The path of the SHFB installation to update.</param>
        /// <returns>A value equal to <c>0</c> for successful settings; nonzero otherwise.</returns>
        internal static int SetClassNamesColor(string color, string path)
        {
            return Shfb.Update(HighlightingTools.ClassNameColorSetter, color, path);
        }

        /// <summary>
        /// Enumerates the file managers that encapsulates the
        /// updating logic required to set the specified highlighting color
        /// for the specified path.
        /// </summary>
        /// <param name="color">The color to set for class name highlighting.</param>
        /// <param name="path">
        /// The path of the SHFB installation to update.
        /// </param>
        /// <returns>
        /// The collection of file managers required for installation.
        /// </returns>
        static IEnumerable<FileManager> ClassNameColorSetter(string color, string path)
        {
            List<FileManager> managers = new List<FileManager>();

            #region HIGHLIGHT.CSS

            string colorHexCode = SupportedColors.GetColorHexCode(color);

            FileManager highlightCss = new HighlightCssEditor(
                Path.Combine(path,
                    "PresentationStyles",
                    "Colorizer",
                    "highlight.css"),
                colorHexCode);

            managers.Add(highlightCss);

            #endregion

            #region HIGHLIGHT.XSL

            FileManager highlightXsl = new HighlightXslEditor(
                Path.Combine(path,
                    "PresentationStyles",
                    "Colorizer",
                    "highlight.xsl"));

            managers.Add(highlightXsl);

            #endregion

            return managers;
        }

        /// <summary>
        /// Provides methods to manage the colors supported for 
        /// class name highlighting.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The colors supported for class name highlighting are those
        /// having CSS names supported by all browsers as of date 3/21/2017.
        /// </para>
        /// </remarks>
        /// <seealso href="https://www.w3schools.com/cssref/css_colors.asp"/>
        internal static class SupportedColors
        {
            #region SUPPORTED COLOR NAMES AND HEX CODES

            // Color Names Supported by All Browsers
            // Source: https://www.w3schools.com/cssref/css_colors.asp
            // Date of access: 3/21/2017
            static readonly string[] allBrowsersColors = new string[] {
            "AliceBlue,#F0F8FF",
            "AntiqueWhite,#FAEBD7",
            "Aqua,#00FFFF",
            "Aquamarine,#7FFFD4",  // Default
            "Azure,#F0FFFF",
            "Beige,#F5F5DC",
            "Bisque,#FFE4C4",
            "Black,#000000",
            "BlanchedAlmond,#FFEBCD",
            "Blue,#0000FF",
            "BlueViolet,#8A2BE2",
            "Brown,#A52A2A",
            "BurlyWood,#DEB887",
            "CadetBlue,#5F9EA0",
            "Chartreuse,#7FFF00",
            "Chocolate,#D2691E",
            "Coral,#FF7F50",
            "CornflowerBlue,#6495ED",
            "Cornsilk,#FFF8DC",
            "Crimson,#DC143C",
            "Cyan,#00FFFF",
            "DarkBlue,#00008B",
            "DarkCyan,#008B8B",
            "DarkGoldenRod,#B8860B",
            "DarkGray,#A9A9A9",
            "DarkGrey,#A9A9A9",
            "DarkGreen,#006400",
            "DarkKhaki,#BDB76B",
            "DarkMagenta,#8B008B",
            "DarkOliveGreen,#556B2F",
            "DarkOrange,#FF8C00",
            "DarkOrchid,#9932CC",
            "DarkRed,#8B0000",
            "DarkSalmon,#E9967A",
            "DarkSeaGreen,#8FBC8F",
            "DarkSlateBlue,#483D8B",
            "DarkSlateGray,#2F4F4F",
            "DarkSlateGrey,#2F4F4F",
            "DarkTurquoise,#00CED1",
            "DarkViolet,#9400D3",
            "DeepPink,#FF1493",
            "DeepSkyBlue,#00BFFF",
            "DimGray,#696969",
            "DimGrey,#696969",
            "DodgerBlue,#1E90FF",
            "FireBrick,#B22222",
            "FloralWhite,#FFFAF0",
            "ForestGreen,#228B22",
            "Fuchsia,#FF00FF",
            "Gainsboro,#DCDCDC",
            "GhostWhite,#F8F8FF",
            "Gold,#FFD700",
            "GoldenRod,#DAA520",
            "Gray,#808080",
            "Grey,#808080",
            "Green,#008000",
            "GreenYellow,#ADFF2F",
            "HoneyDew,#F0FFF",
            "HotPink,#FF69B",
            "IndianRed,#CD5C5",
            "Indigo,#4B0082",
            "Ivory,#FFFFF0",
            "Khaki,#F0E68C",
            "Lavender,#E6E6FA",
            "LavenderBlush,#FFF0F5",
            "LawnGreen,#7CFC00",
            "LemonChiffon,#FFFACD",
            "LightBlue,#ADD8E6",
            "LightCoral,#F08080",
            "LightCyan,#E0FFFF",
            "LightGoldenRodYellow,#FAFAD2",
            "LightGray,#D3D3D3",
            "LightGrey,#D3D3D3",
            "LightGreen,#90EE90",
            "LightPink,#FFB6C1",
            "LightSalmon,#FFA07A",
            "LightSeaGreen,#20B2AA",
            "LightSkyBlue,#87CEFA",
            "LightSlateGray,#778899",
            "LightSlateGrey,#778899",
            "LightSteelBlue,#B0C4DE",
            "LightYellow,#FFFFE0",
            "Lime,#00FF00",
            "LimeGreen,#32CD32",
            "Linen,#FAF0E6",
            "Magenta,#FF00FF",
            "Maroon,#800000",
            "MediumAquaMarine,#66CDAA",
            "MediumBlue,#0000CD",
            "MediumOrchid,#BA55D3",
            "MediumPurple,#9370DB",
            "MediumSeaGreen,#3CB371",
            "MediumSlateBlue,#7B68EE",
            "MediumSpringGreen,#00FA9A",
            "MediumTurquoise,#48D1CC",
            "MediumVioletRed,#C7158",
            "MidnightBlue,#191970",
            "MintCream,#F5FFFA",
            "MistyRose,#FFE4E1",
            "Moccasin,#FFE4B5",
            "NavajoWhite,#FFDEAD",
            "Navy,#000080",
            "OldLace,#FDF5E6",
            "Olive,#808000",
            "OliveDrab,#6B8E23",
            "Orange,#FFA500",
            "OrangeRed,#FF4500",
            "Orchid,#DA70D6",
            "PaleGoldenRod,#EEE8AA",
            "PaleGreen,#98FB98",
            "PaleTurquoise,#AFEEEE",
            "PaleVioletRed,#DB7093",
            "PapayaWhip,#FFEFD5",
            "PeachPuff,#FFDAB9",
            "Peru,#CD853F",
            "Pink,#FFC0CB",
            "Plum,#DDA0DD",
            "PowderBlue,#B0E0E6",
            "Purple,#800080",
            "RebeccaPurple,#663399",
            "Red,#FF0000",
            "RosyBrown,#BC8F8F",
            "RoyalBlue,#4169E1",
            "SaddleBrown,#8B4513",
            "Salmon,#FA8072",
            "SandyBrown,#F4A460",
            "SeaGreen,#2E8B57",
            "SeaShell,#FFF5EE",
            "Sienna,#A0522D",
            "Silver,#C0C0C0	",
            "SkyBlue,#87CEEB",
            "SlateBlue,#6A5ACD",
            "SlateGray,#708090",
            "SlateGrey,#708090",
            "Snow,#FFFAFA",
            "SpringGreen,#00FF7F",
            "SteelBlue,#4682B4",
            "Tan,#D2B48C",
            "Teal,#008080",
            "Thistle,#D8BFD8",
            "Tomato,#FF6347",
            "Turquoise,#40E0D0",
            "Violet,#EE82EE",
            "Wheat,#F5DEB3",
            "White,#FFFFFF",
            "WhiteSmoke,#F5F5F5",
            "Yellow,#FFFF00",
            "YellowGreen,#9ACD32"
        };

            static readonly Dictionary<string, string> supportedColors =
                new Dictionary<string, string>();


            static SupportedColors()
            {
                for (int i = 0; i < allBrowsersColors.Length; i++)
                {
                    var tokens = allBrowsersColors[i].Split(',');
                    supportedColors.Add(tokens[0], tokens[1]);
                }
            }

            #endregion

            public static string GetColorHexCode(string colorName)
            {
                if (supportedColors.ContainsKey(colorName))
                {
                    return supportedColors[colorName];
                }
                else
                {
                    throw new ArgumentException(
                        "Not supported color name. " +
                        String.Format("Value {0} is not supported. ", colorName) +
                        "See the documentation for a list of supported values.");
                }
            }
        }

        #endregion

        #region ADDING CLASS NAMES

        /// <summary>
        /// Adds a family of class names to the specified languages.
        /// </summary>
        /// <param name="family">The family identifier.</param>
        /// <param name="names">The names included in the family.</param>
        /// <param name="languages">The languages which
        /// the family must be associated with.</param>
        /// <returns>
        /// A value equal to <c>0</c> for successful operations; nonzero otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If a family already exists having the given identifier
        /// for the specified languages,
        /// its current class names will be deleted before starting 
        /// the addition of the new
        /// ones.
        /// </para>
        /// <para>
        /// Identifiers of supported languages are as follows.
        /// <list type="table">
        /// <item>
        /// <description><c>"c"</c></description>
        /// <description><c>"cs"</c></description>
        /// <description><c>"cpp"</c></description>
        /// <description><c>"fs"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"jsharp"</c></description>
        /// <description><c>"javascript"</c></description>
        /// <description><c>"jscriptnet"</c></description>
        /// <description><c>"vbnet"</c></description>
        /// </item>
        /// <item>
        /// <description><c>"vbscript"</c></description>
        /// <description><c>"sql"</c></description>
        /// <description><c>"pshell"</c></description>
        /// <description><c>"python"</c></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="family"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="names"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="languages"/> is <b>null</b>.<br/>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="family"/> is an empty string, 
        /// or a string consisting only of white-space characters.<br/>
        /// -or-<br/>
        /// <paramref name="names"/> contain empty strings, 
        /// or strings consisting only of white-space characters.<br/>
        /// -or-<br/>
        /// <paramref name="languages"/> contain strings 
        /// that are not supported language identifiers.<br/>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The environmental variable <c>SHFBROOT</c> cannot be found.<br />
        /// -or-<br />
        /// The environmental variable <c>SHFBROOT</c> exists but points to
        /// a SHFB installation which is corrupted or has a version different from the
        /// target one.</exception>
        public static int AddClassNamesFamily(
            string family,
            IEnumerable<string> names,
            IEnumerable<string> languages)
        {
            #region INPUT VALIDATION

            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }

            if (String.IsNullOrWhiteSpace(family))
            {
                throw new ArgumentException(
                    "The parameter cannot be an empty string, " +
                    "or a string consisting only of white-space characters.",
                    nameof(family));
            }

            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            foreach (var name in names)
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(
                        "The parameter cannot contain empty strings, " +
                        "or strings consisting only of white-space characters.",
                        nameof(names));

                }
            }

            if (languages is null)
            {
                throw new ArgumentNullException(nameof(languages));
            }

            SupportedLanguages.Validate(languages);

            #endregion

            if (Shfb.SHFBROOT is null)
            {
                throw new InvalidOperationException(
                    "The environmental variable SHFBROOT cannot be found. " +
                    String.Format(
                        "Please, install SHFB version {0} and try again.",
                        Shfb.TargetVersion));
            }

            return AddClassNamesFamily(
                    family,
                    names.Distinct(),
                    languages,
                    Shfb.SHFBROOT);
        }

        class FamilyInfo
        {
            internal string Family { get; set; }

            internal IEnumerable<string> Names { get; set; }

            internal IEnumerable<string> Languages { get; set; }
        }

        /// <summary>
        /// Adds a family of class names to be highlighted in a SHFB 
        /// installation having the specified path.
        /// </summary>
        /// <param name="family">A string used to identify
        /// the class names.</param>
        /// <param name="names">The class names to highlight.</param>
        /// <param name="languages">The languages for which 
        /// the class names need to be highlighted.</param>
        /// <param name="path">
        /// The path of the SHFB installation to update.
        /// </param>
        /// <returns>A value equal to <c>0</c> for successful installations; nonzero otherwise.</returns>
        internal static int AddClassNamesFamily(
            string family,
            IEnumerable<string> names,
            IEnumerable<string> languages,
            string path)
        {
            FamilyInfo updateInfo = new FamilyInfo()
            {
                Family = family,
                Names = names,
                Languages = languages
            };

            return Shfb.Update(HighlightingTools.ClassNamesFamilyAdder, updateInfo, path);
        }

        /// <summary>
        /// Enumerates the file managers that encapsulates the
        /// updating logic required to add the specified family of class names
        /// for the specified path.
        /// </summary>
        /// <param name="info">The information about the family of class names.</param>
        /// <param name="path">The path of the SHFB installation to update.</param>
        /// <returns>The collection of file managers required for installation.</returns>
        static IEnumerable<FileManager> ClassNamesFamilyAdder(
            FamilyInfo info,
            string path)
        {
            List<FileManager> managers = new List<FileManager>();

            FileManager highlightXml = new HighlightXmlFamilyAdder(
                Path.Combine(path,
                    "PresentationStyles",
                    "Colorizer",
                    "highlight.xml"),
                info.Family,
                info.Names,
                info.Languages);

            managers.Add(highlightXml);

            return managers;
        }

        /// <summary>
        /// Provides information about the languages for which
        /// lists of class names can be highlighted.
        /// </summary>
        internal static class SupportedLanguages
        {
            static readonly string[] supportedLanguages = new string[]
                {
                "c",
                "cs",
                "cpp",
                "fs",
                "jsharp",
                "javascript",
                "jscriptnet",
                "vbnet",
                "vbscript",
                "sql",
                "pshell",
                "python"
                };

            /// <summary>
            /// Validates the specified languages.
            /// </summary>
            /// <param name="languages">The languages to validate.</param>
            /// <exception cref="ArgumentException">
            /// One of the specified <paramref name="languages"/> is 
            /// not supported.
            /// </exception>
            public static void Validate(IEnumerable<string> languages)
            {
                foreach (var language in languages)
                {
                    if (!supportedLanguages.Contains(language))
                    {
                        throw new ArgumentException(
                            "Not supported language identifier. " +
                            String.Format("Value {0} is not supported. ", language) +
                            "See the documentation for a list of supported values.");

                    }
                }
            }

            /// <summary>
            /// Enumerates the supported languages.
            /// </summary>
            internal static IEnumerable<string> Get()
            {
                for (int i = 0; i < supportedLanguages.Length; i++)
                {
                    yield return supportedLanguages[i];
                }
            }
        }

        #endregion

        #region REMOVING CLASS NAMES

        /// <summary>
        /// Removes a family of class names from all supported languages.
        /// </summary>
        /// <param name="family">The family identifier.</param>
        /// <returns>
        /// A value equal to <c>0</c> for successful removals; nonzero otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="family"/> is <b>null</b>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The environmental variable <c>SHFBROOT</c> cannot be found.<br />
        /// -or-<br />
        /// The environmental variable <c>SHFBROOT</c> exists but points to
        /// a SHFB installation which is corrupted or has a version different from the
        /// target one.</exception>
        public static int RemoveClassNamesFamily(
            string family)
        {
            #region INPUT VALIDATION

            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }


            #endregion

            if (Shfb.SHFBROOT is null)
            {
                throw new InvalidOperationException(
                    "The environmental variable SHFBROOT cannot be found. " +
                    String.Format(
                        "Please, install SHFB version {0} and try again.",
                        Shfb.TargetVersion));
            }

            return RemoveClassNamesFamily(
                    family,
                    Shfb.SHFBROOT);
        }

        /// <summary>
        /// Removes a family of class names to be highlighted in a SHFB 
        /// installation having the specified path.
        /// </summary>
        /// <param name="family">A string used to identify
        /// the class names.</param>
        /// <param name="path">
        /// The path of the SHFB installation to update.
        /// </param>
        /// <returns>A value equal to <c>0</c> for successful removals; nonzero otherwise.</returns>
        internal static int RemoveClassNamesFamily(
            string family,
            string path)
        {
            return Shfb.Update(HighlightingTools.ClassNamesFamilyRemover, family, path);
        }

        /// <summary>
        /// Enumerates the file managers that encapsulates the
        /// updating logic required to remove the specified family of class names
        /// for the specified path.
        /// </summary>
        /// <param name="family">A string used to identify
        /// the class names.</param>
        /// <param name="path">The path of the SHFB installation to update.</param>
        /// <returns>The collection of file managers required for installation.</returns>
        static IEnumerable<FileManager> ClassNamesFamilyRemover(
            string family,
            string path)
        {
            List<FileManager> managers = new List<FileManager>();

            FileManager highlightXmlfamilyRemover = new HighlightXmlFamilyRemover(
                Path.Combine(path,
                    "PresentationStyles",
                    "Colorizer",
                    "highlight.xml"),
                family);

            managers.Add(highlightXmlfamilyRemover);

            return managers;
        }

        #endregion
    }
}
