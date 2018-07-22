// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using Novacta.Documentation.ShfbTools.FileManagers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Provides a method to install the Novacta Latex Tools for
    /// SHFB.
    /// </summary>
    public static class LatexTools
    {
        /// <summary>
        /// Installs the Novacta SHFB Latex Tools.
        /// </summary>
        /// <returns>
        /// A value equal to <c>0</c> for successful installations; nonzero otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The environmental variable <c>SHFBROOT</c> cannot be found.<br/>
        /// -or-<br/>
        /// The environmental variable <c>SHFBROOT</c> exists but points to
        /// a SHFB installation which is corrupted or has a version different from the
        /// target one.
        /// </exception>
        public static int Install()
        {
            if (Shfb.SHFBROOT is null)
            {
                throw new InvalidOperationException(
                    "The environmental variable SHFBROOT cannot be found. " +
                    String.Format(
                        "Please, install SHFB version {0} and try again.",
                        Shfb.TargetVersion));

            }

            var ewSoftwareCommonApplicationDataPath = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData),
                "EWSoftware");

            return LatexTools.Install(ewSoftwareCommonApplicationDataPath, Shfb.SHFBROOT);
        }

        /// <summary>
        /// Installs the specified custom build path.
        /// </summary>
        /// <param name="ewSoftwareCommonApplicationDataPath">The path of the 
        /// SHFB common Application Data folder.</param>
        /// <param name="path">The path of the SHFB installation.</param>
        /// <returns>A value equal to <c>0</c> for successful installations; nonzero otherwise.</returns>
        internal static int Install(
            string ewSoftwareCommonApplicationDataPath,
            string path)
        {
            return Shfb.Update(
                LatexTools.Updater,
                ewSoftwareCommonApplicationDataPath,
                path);
        }

        /// <summary>
        /// Gets the transform for latexImg nodes.
        /// </summary>
        /// <returns>
        /// An XML document representing the transform
        /// for latexImg nodes.</returns>
        static XmlDocument GetLatexImgTransform()
        {
            var document = new XmlDocument();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<!--");
            builder.AppendLine("==============================================");
            builder.AppendLine("Transform for <latexImg> nodes");
            builder.AppendLine("==============================================");
            builder.AppendLine();
            builder.AppendLine("Provides support for the use of Latex formulas in SHFB documentation topics.");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("EXAMPLE");
            builder.AppendLine();
            builder.AppendLine("Let us assume that the following <latex> node is inserted in ");
            builder.AppendLine("a documentation topic:");
            builder.AppendLine();
            builder.AppendLine(@"<latex mode='inline' depth='25'>f_X\left(x\right)=x^2</latex>");
            builder.AppendLine();
            builder.AppendLine("and that the Novacta SHFB Latex Tools generated a file named 'latex_1.png'");
            builder.AppendLine("to represent the formula.");
            builder.AppendLine("Then, it will be transformed as follows:");
            builder.AppendLine();
            builder.AppendLine("<latexImg>");
            builder.AppendLine("  <name>latex_1</name>");
            builder.AppendLine("  <format>png</format>");
            builder.AppendLine("  <inline>1</inline>");
            builder.AppendLine("  <depth>25</depth>");
            builder.AppendLine("</latexImg>");
            builder.AppendLine();
            builder.AppendLine("Such <latexImg> node is transformed by this style sheet in different ways, ");
            builder.AppendLine("depending on the selected presentation style.");
            builder.AppendLine();
            builder.AppendLine("For style 'OpenXml', it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"../media/latex_1.png\" alt=\"LaTeX equation\" style=\"vertical-align: -25px\"></img>");
            builder.AppendLine();
            builder.AppendLine("For styles 'VS2010' and 'VS2013', for help outputs 'HtmlHelp1' and 'Website',");
            builder.AppendLine("it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"../media/latex_1.png\" alt =\"LaTeX equation\" style=\"vertical-align: -25px\"></img>");
            builder.AppendLine();
            builder.AppendLine("while for help output 'MsHelpViewer', it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"media/latex_1.png\" alt =\"LaTeX equation\" style=\"vertical-align: -25px\"></img>");
            builder.AppendLine();
            builder.AppendLine("The last transform also happen for style 'Markdown'.");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("REMARK");
            builder.AppendLine();
            builder.AppendLine("At the time of this writing, SVG files are not supported by ");
            builder.AppendLine("the OpenXml presentation style.");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("EXAMPLE OF INSTALLATION FOR PRESENTATION STYLE 'VS2013'");
            builder.AppendLine();
            builder.AppendLine("The following is the novacta_latex_tools.xsl transform.");
            builder.AppendLine();
            builder.AppendLine("It must be copied in the 'Transforms' folder of style 'VS2013',");
            builder.AppendLine("and imported in the main style sheets, accordingly.");
            builder.AppendLine("This implies that it must be imported in files");
            builder.AppendLine();
            builder.AppendLine(@"VS2013\Transforms\main_sandcastle.xsl");
            builder.AppendLine();
            builder.AppendLine("and");
            builder.AppendLine();
            builder.AppendLine(@"VS2013\Transforms\main_conceptual.xsl");
            builder.AppendLine();
            builder.AppendLine("by adding node");
            builder.AppendLine();
            builder.AppendLine("<xsl:import href=\"novacta_latex_tools.xsl\"/>");
            builder.AppendLine();
            builder.AppendLine("before node");
            builder.AppendLine();
            builder.AppendLine("<xsl:output method=\"xml\" omit-xml-declaration=\"yes\" indent=\"no\" encoding=\"utf-8\"/>");
            builder.AppendLine();
            builder.AppendLine("In addition, the following shared content items must be added as follows.");
            builder.AppendLine();
            builder.AppendLine("For help outputs 'HtmlHelp1' and 'Website',");
            builder.AppendLine();
            builder.AppendLine("<item id=\"novacta_latex_tools_path\">../media/{0}.{1}</item>");
            builder.AppendLine("<item id=\"novacta_latex_tools_depth\">vertical-align: -{0}px</item>");
            builder.AppendLine();
            builder.AppendLine(@"must be inserted in VS2013\Content\shared_content.xml");
            builder.AppendLine();
            builder.AppendLine("For help output 'MsHelpViewer',");
            builder.AppendLine();
            builder.AppendLine("<item id=\"novacta_latex_tools_path\">media/{0}.{1}</item>");
            builder.AppendLine();
            builder.AppendLine(@"must be inserted in VS2013\Content\shared_content_mshc.xml");
            builder.AppendLine("-->");

            string content = "<?xml version='1.0' encoding='utf-8'?>" +
            "<xsl:stylesheet xmlns:xsl = 'http://www.w3.org/1999/XSL/Transform' " +
                             "version = '2.0'>" +
                builder.ToString() +
                "<xsl:template match = 'latexImg'>" +
                    "<img alt='LaTeX equation'>" +
                        "<includeAttribute name = 'src' " +
                                          "item = 'novacta_latex_tools_path'>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'name'/>" +
                            "</parameter>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'format'/>" +
                            "</parameter>" +
                        "</includeAttribute>" +

                    "<xsl:if test=\"inline=1\">" +
                         "<includeAttribute name = 'style' " +
                                           "item = 'novacta_latex_tools_depth'>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'depth' />" +
                            "</parameter>" +
                         "</includeAttribute>" +
                    "</xsl:if>" +

                    "</img>" +
                 "</xsl:template>" +
             "</xsl:stylesheet>";
            document.LoadXml(content);

            return document;
        }

        /// <summary>
        /// Gets the latex component default configuration document.
        /// </summary>
        /// <returns>
        /// The latex component default configuration document.</returns>
        static XmlDocument GetLatexComponentDefaultConfiguration()
        {
            var document = new XmlDocument();
            string content =
                "<?xml version='1.0' encoding='utf-8'?>" +
                "<configuration>" +
                    "<dvisvgmBinPath />" +
                    "<latexBinPath />" +
                "</configuration>";

            document.LoadXml(content);

            return document;
        }

        /// <summary>
        /// Enumerates the file managers that encapsulates the
        /// updating logic required by the Novacta SHFB Latex Tools
        /// for the specified path.
        /// </summary>
        /// <param name="updateInfo">The update information.</param>
        /// <param name="path">The path of the SHFB installation to update.</param>
        /// <returns>The collection of file managers required for installation.</returns>
        static IEnumerable<FileManager> Updater(string updateInfo, string path)
        {
            #region STYLE SHEET IMPORTATION

            (string Href, XmlDocument Document) styleSheet;

            styleSheet.Href = "novacta_latex_tools.xsl";
            styleSheet.Document = GetLatexImgTransform();

            var styleSheets = new List<(string Href, XmlDocument Document)>
            {
                styleSheet
            };

            List<FileManager> managers = new List<FileManager>();
            foreach (var style in Shfb.PresentationStyles)
            {
                var styleManagers = Shfb.PrepareStyleSheetImportation(
                    path,
                    style,
                    Topics.All,
                    styleSheets);
                managers.AddRange(styleManagers);
            }

            #endregion

            #region SHARED CONTENT ITEMS 

            List<(string Id, string InnerText)> items;

            #region MARKDOWN

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "media/{0}.{1}"),
                ("novacta_latex_tools_depth", "vertical-align: -{0}px")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "Markdown",
                "SharedContent.xml",
                items));

            #endregion

            #region OPENXML

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "../media/{0}.{1}"),
                ("novacta_latex_tools_depth", "vertical-align: -{0}px")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "OpenXml",
                "SharedContent.xml",
                items));

            #endregion

            #region VS2010

            // Outputs: Website, HtmlHelp1

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "../media/{0}.{1}"),
                ("novacta_latex_tools_depth", "vertical-align: -{0}px")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2010",
                "shared_content.xml",
                items));

            // Output: MsHelpViewer

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "media/{0}.{1}")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2010",
                "shared_content_mshc.xml",
                items));

            #endregion

            #region VS2013

            // Outputs: Website, HtmlHelp1

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "../media/{0}.{1}"),
                ("novacta_latex_tools_depth", "vertical-align: -{0}px")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2013",
                "shared_content.xml",
                items));

            // Output: MsHelpViewer

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_latex_tools_path", "media/{0}.{1}")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2013",
                "shared_content_mshc.xml",
                items));

            #endregion

            #endregion

            #region CONFIGURATION

            managers.Add(new SvgCompatibilityConfigurator(
                Path.Combine(
                path,
                @"PresentationStyles",
                "VS2010",
                "Configuration",
                "BuildAssembler.config")));

            managers.Add(new SvgCompatibilityConfigurator(
                Path.Combine(
                path,
                @"PresentationStyles",
                "VS2013",
                "Configuration",
                "BuildAssembler.config")));

            #endregion

            #region NEW FILES

            string fileName, destinationFilePath, baseDestinationPath;

            baseDestinationPath = updateInfo;

            if (!Directory.Exists(baseDestinationPath))
            {
                Directory.CreateDirectory(baseDestinationPath);
            }

            baseDestinationPath = Path.Combine(
                baseDestinationPath,
                "Sandcastle Help File Builder");

            if (!Directory.Exists(baseDestinationPath))
            {
                Directory.CreateDirectory(baseDestinationPath);
            }

            baseDestinationPath = Path.Combine(
                baseDestinationPath,
                "Components and Plug-Ins");

            if (!Directory.Exists(baseDestinationPath))
            {
                Directory.CreateDirectory(baseDestinationPath);
            }

            // LatexPlugIn assembly file

            fileName = "Novacta.Documentation.ShfbLatexPlugIn.dll";

            destinationFilePath = Path.Combine(
                baseDestinationPath, fileName);

            managers.Add(
                new FromByteArrayFileCreator(
                    destinationFilePath,
                    Properties.Resources.Novacta_Documentation_ShfbLatexPlugIn));

            // LatexComponent assembly file

            fileName = "Novacta.Documentation.ShfbLatexComponent.dll";

            destinationFilePath = Path.Combine(
                baseDestinationPath, fileName);

            managers.Add(
                new FromByteArrayFileCreator(
                    destinationFilePath,
                    Properties.Resources.Novacta_Documentation_ShfbLatexComponent));

            // LatexComponent default configuration file

            fileName = "Novacta.Documentation.ShfbLatexComponent.config";

            managers.Add(new XmlFileCreator(
                Path.Combine(baseDestinationPath, fileName),
                GetLatexComponentDefaultConfiguration()));

            #endregion

            return managers;
        }
    }
}
