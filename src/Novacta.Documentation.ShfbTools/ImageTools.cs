// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Provides a method to install the Novacta Image Tools for
    /// SHFB.
    /// </summary>
    public static class ImageTools
    {
        /// <summary>
        /// Installs the Novacta SHFB Image Tools.
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
            return ImageTools.Install(Shfb.SHFBROOT);
        }

        /// <summary>
        /// Installs the Novacta SHFB Image Tools in the specified path.
        /// </summary>
        /// <param name="path">The path of the SHFB installation to update.
        /// </param>
        /// <returns>
        /// A value equal to <c>0</c> for successful installations; nonzero otherwise.
        /// </returns>
        internal static int Install(string path)
        {
            return Shfb.Update(ImageTools.Updater, (string)(null), path);
        }

        /// <summary>
        /// Gets the transform for image nodes.
        /// </summary>
        /// <returns>
        /// A XML document representing the transform
        /// for image nodes.</returns>
        static XmlDocument GetImageToolsTransform()
        {
            var document = new XmlDocument();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<!--");
            builder.AppendLine("==============================================");
            builder.AppendLine("Transform for <image> nodes in Sandcastle topics");
            builder.AppendLine("==============================================");
            builder.AppendLine();
            builder.AppendLine("Provides support for the use of <img> nodes in Sandcastle reference topics.");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("EXAMPLE");
            builder.AppendLine();
            builder.AppendLine("Let us assume that a file named myImage.png should be used in ");
            builder.AppendLine("a SHFB project, both in conceptual and in Sandcastle reference files.");
            builder.AppendLine("To use it in a conceptual topic, the file must be added to folder 'media',");
            builder.AppendLine("and its property 'Build Action' must be set to 'Image'.");
            builder.AppendLine("To use it in a Sandcastle reference file,");
            builder.AppendLine("set property 'Copy To Media' to 'True',");
            builder.AppendLine("and use the following node in your XML comments:");
            builder.AppendLine();
            builder.AppendLine("<image>");
            builder.AppendLine("  <src>myImage.png</src>");
            builder.AppendLine("  <alt>My image</alt>");
            builder.AppendLine("  <width>100%</width>");
            builder.AppendLine("</image>");
            builder.AppendLine();
            builder.AppendLine("This node is transformed in different ways, depending on the");
            builder.AppendLine("presentation style you selected.");
            builder.AppendLine();
            builder.AppendLine("For style 'OpenXml', it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"../media/myImage.png\" alt=\"My image\" width=\"100%\"></img>");
            builder.AppendLine();
            builder.AppendLine("For styles 'VS2010' and 'VS2013', for help outputs 'HtmlHelp1' and 'Website',");
            builder.AppendLine("it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"../media/myImage.png\" alt =\"My image\" width=\"100%\"></img>");
            builder.AppendLine();
            builder.AppendLine("while for help output 'MsHelpViewer', it is transformed into");
            builder.AppendLine();
            builder.AppendLine("<img src=\"media/myImage.png\" alt =\"My image\" width=\"100%\"></img>");
            builder.AppendLine();
            builder.AppendLine("The last transform also happens for style 'Markdown'.");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("EXAMPLE OF INSTALLATION FOR PRESENTATION STYLE 'VS2013'");
            builder.AppendLine();
            builder.AppendLine("This is the novacta_image_tools.xsl transform.");
            builder.AppendLine();
            builder.AppendLine("It must be copied in the 'Transforms' folder of style 'VS2013',");
            builder.AppendLine("and imported in the main style sheets, accordingly.");
            builder.AppendLine("This implies that it must be imported in file");
            builder.AppendLine();
            builder.AppendLine(@"VS2013\Transforms\main_sandcastle.xsl");
            builder.AppendLine();
            builder.AppendLine("by adding node");
            builder.AppendLine();
            builder.AppendLine("<xsl:import href=\"novacta_image_tools.xsl\"/>");
            builder.AppendLine();
            builder.AppendLine("before node");
            builder.AppendLine();
            builder.AppendLine("<xsl:output method=\"xml\" omit-xml-declaration=\"yes\" indent=\"no\" encoding=\"utf-8\"/>");
            builder.AppendLine();
            builder.AppendLine("In addition, the following shared content items must be added as follows.");
            builder.AppendLine();
            builder.AppendLine("For help outputs 'HtmlHelp1' and 'Website',");
            builder.AppendLine();
            builder.AppendLine("<item id=\"novacta_image_tools_path\">../media/{0}</item>");
            builder.AppendLine("<item id=\"novacta_image_tools_alt\">{0}</item>");
            builder.AppendLine("<item id=\"novacta_image_tools_width\">{0}</item>");
            builder.AppendLine();
            builder.AppendLine(@"must be inserted in VS2013\Content\shared_content.xml");
            builder.AppendLine();
            builder.AppendLine("For help output 'MsHelpViewer',");
            builder.AppendLine();
            builder.AppendLine("<item id=\"novacta_image_tools_path\">media/{0}</item>");
            builder.AppendLine();
            builder.AppendLine(@"must be inserted in VS2013\Content\shared_content_mshc.xml");
            builder.AppendLine("-->");

            string content = "<?xml version='1.0' encoding='utf-8'?>" +
            "<xsl:stylesheet xmlns:xsl = 'http://www.w3.org/1999/XSL/Transform' " +
                             "version = '2.0'>" +
                builder.ToString() +
                "<xsl:template match = 'image'>" +
                    "<img>" +
                        "<includeAttribute name = 'src' " +
                                          "item = 'novacta_image_tools_path'>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'src'/>" +
                            "</parameter>" +
                        "</includeAttribute>" +

                    "<xsl:if test = 'width'>" +
                         "<includeAttribute name = 'width' " +
                                           "item = 'novacta_image_tools_width'>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'width' />" +
                            "</parameter>" +
                         "</includeAttribute>" +
                    "</xsl:if>" +

                    "<xsl:if test = 'alt'>" +
                        "<includeAttribute name = 'alt' " +
                                          "item = 'novacta_image_tools_alt'>" +
                            "<parameter>" +
                                "<xsl:value-of select = 'alt'/>" +
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
        /// Enumerates the file managers that encapsulates the
        /// updating logic required by the Novacta SHFB Image Tools
        /// for the specified path.
        /// </summary>
        /// <param name="updateInfo">The update information.</param>
        /// <param name="path">The path of the SHFB installation to update.</param>
        /// <returns>The collection of file managers required for installation.</returns>
        static IEnumerable<FileManager> Updater(string updateInfo, string path)
        {
            #region STYLE SHEET IMPORTATION

            (string Href, XmlDocument Document) styleSheet;

            styleSheet.Href = "novacta_image_tools.xsl";
            styleSheet.Document = GetImageToolsTransform();

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
                    Topics.Sandcastle,
                    styleSheets);
                managers.AddRange(styleManagers);
            }

            #endregion

            #region SHARED CONTENT ITEMS 

            List<(string Id, string InnerText)> items;

            #region MARKDOWN

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_image_tools_path", "media/{0}"),
                ("novacta_image_tools_alt", "{0}"),
                ("novacta_image_tools_width", "{0}")
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
                ("novacta_image_tools_path", "../media/{0}"),
                ("novacta_image_tools_alt", "{0}"),
                ("novacta_image_tools_width", "{0}")
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
                ("novacta_image_tools_path", "../media/{0}"),
                ("novacta_image_tools_alt", "{0}"),
                ("novacta_image_tools_width", "{0}")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2010",
                "shared_content.xml",
                items));

            // Output: MsHelpViewer

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_image_tools_path", "media/{0}")
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
                ("novacta_image_tools_path", "../media/{0}"),
                ("novacta_image_tools_alt", "{0}"),
                ("novacta_image_tools_width", "{0}")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2013",
                "shared_content.xml",
                items));

            // Output: MsHelpViewer

            items = new List<(string Id, string InnerText)>
            {
                ("novacta_image_tools_path", "media/{0}")
            };

            managers.Add(Shfb.PrepareSharedContentItemsModification(
                path,
                "VS2013",
                "shared_content_mshc.xml",
                items));

            #endregion

            #endregion

            return managers;
        }
    }
}
