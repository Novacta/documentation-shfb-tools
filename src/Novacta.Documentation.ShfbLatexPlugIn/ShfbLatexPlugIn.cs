// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using System.Xml.XPath;

using Sandcastle.Core;
using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildComponent;
using SandcastleBuilder.Utils.BuildEngine;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Provides support for representing LaTeX formatted formulas using SVG files in 
    /// reference XML comments and conceptual content topics for MS Help Viewer files created with 
    /// Sandcastle Help File Builder.
    /// </summary>
    /// <remarks>The <c>HelpFileBuilderPlugInExportAttribute</c> is used to export your plug-in so that the help
    /// file builder finds it and can make use of it.  The example below shows the basic usage for a common
    /// plug-in.  Set the additional attribute values as needed:
    ///
    /// <list type="bullet">
    ///     <item>
    ///         <term>IsConfigurable</term>
    ///         <description>Set this to true if your plug-in contains configurable settings.  The
    /// <c>ConfigurePlugIn</c> method will be called to let the user change the settings.</description>
    ///     </item>
    ///     <item>
    ///         <term>RunsInPartialBuild</term>
    ///         <description>Set this to true if your plug-in should run in partial builds used to generate
    /// reflection data for the API Filter editor dialog or namespace comments used for the Namespace Comments
    /// editor dialog.  Typically, this is left set to false.</description>
    ///     </item>
    /// </list>
    /// 
    /// Plug-ins are singletons in nature.  The composition container will create instances as needed and will
    /// dispose of them when the container is disposed of.</remarks>
    [HelpFileBuilderPlugInExport(
        "LaTeX Support in MS Help Viewer files - SVG Fix",
        IsConfigurable = false,
        IsHidden = false,
        Version = AssemblyInfo.Version,
        Copyright = AssemblyInfo.Copyright,
        Description = AssemblyInfo.Description)]
    public sealed class ShfbLatexPlugIn : IPlugIn
    {
        #region Private data members
        //=====================================================================

        private List<ExecutionPoint> executionPoints;

        private BuildProcess builder;
        #endregion

        #region IPlugIn implementation
        //=====================================================================

        /// <summary>
        /// This read-only property returns a collection of execution points that define when the plug-in should
        /// be invoked during the build process.
        /// </summary>
        public IEnumerable<ExecutionPoint> ExecutionPoints
        {
            get
            {
                if (this.executionPoints == null)
                    this.executionPoints = new List<ExecutionPoint>
                    {
                        new ExecutionPoint(BuildStep.CompilingHelpFile, ExecutionBehaviors.Before),
                    };

                return this.executionPoints;
            }
        }

        /// <summary>
        /// This method is used by the Sandcastle Help File Builder to let the plug-in perform its own
        /// configuration.
        /// </summary>
        /// <param name="project">A reference to the active project</param>
        /// <param name="currentConfig">The current configuration XML fragment</param>
        /// <returns>A string containing the new configuration XML fragment</returns>
        /// <remarks>The configuration data will be stored in the help file builder project</remarks>
        public string ConfigurePlugIn(SandcastleProject project, string currentConfig)
        {
            // TODO: Add and invoke a configuration dialog if you need one.  You will also need to set the
            // IsConfigurable property to true on the class's export attribute.
            MessageBox.Show("This plug-in has no configurable settings", "LaTeX Plug-In",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            return currentConfig;
        }

        /// <summary>
        /// This method is used to initialize the plug-in at the start of the build process
        /// </summary>
        /// <param name="buildProcess">A reference to the current build process</param>
        /// <param name="configuration">The configuration data that the plug-in should use to initialize itself</param>
        public void Initialize(BuildProcess buildProcess, XPathNavigator configuration)
        {
            this.builder = buildProcess;

            var metadata = (HelpFileBuilderPlugInExportAttribute)this.GetType()
                .GetCustomAttributes(
                typeof(HelpFileBuilderPlugInExportAttribute), false).First();

            this.builder.ReportProgress("{0} Version {1}\r\n{2}", metadata.Id, metadata.Version,
                metadata.Copyright);
        }

        /// <summary>
        /// This method is used to execute the plug-in during the build process
        /// </summary>
        /// <param name="context">The current execution context</param>
        public void Execute(ExecutionContext context)
        {
            if (this.builder.CurrentFormat == HelpFileFormats.MSHelpViewer)
            {
                this.builder.ReportProgress("Transforming MS Help Viewer files to support LaTeX content");
                this.TransformLaTeXEmbedTags();
            }
        }

        /// <summary>
        /// Transforms LaTeX img tags into embed tags.
        /// </summary>
        private void TransformLaTeXEmbedTags()
        {
            string basePath = this.builder.WorkingFolder + @"\Output\MSHelpViewer\html\";
            bool isFirstFile = true;

            foreach (string sourceFile in Directory.EnumerateFiles(basePath))
            {

                XmlDocument document = new XmlDocument();
                document.Load(sourceFile);
                XmlNode root = document.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ns", "http://www.w3.org/1999/xhtml");

                XmlNodeList list = root.SelectNodes("//ns:img[@alt='LaTeX equation']", nsmgr);

                if (list.Count > 0)
                {
                    foreach (XmlNode img in list)
                    {
                        XmlNode embed = document.CreateElement("embed");

                        XmlAttribute alt = document.CreateAttribute("alt");
                        alt.Value = "LaTeX equation";
                        embed.Attributes.Append(alt);

                        XmlAttribute type = document.CreateAttribute("type");
                        type.Value = "image/svg+xml";
                        embed.Attributes.Append(type);

                        XmlAttribute src = document.CreateAttribute("src");
                        string imgSource = img.Attributes.GetNamedItem("src").Value;
                        string fileName, fileExtension;
                        int slashPosition = imgSource.IndexOf("/");
                        int dotPosition = imgSource.LastIndexOf('.');
                        fileName = imgSource.Substring(slashPosition + 1, dotPosition - slashPosition - 1);
                        fileExtension = imgSource.Substring(dotPosition + 1, imgSource.Length - dotPosition - 1);

                        XmlNode imgStyle = img.Attributes.GetNamedItem("style");
                        if (!(imgStyle is null))
                        {
                            XmlAttribute style = document.CreateAttribute("style");
                            style.Value = imgStyle.Value;
                            embed.Attributes.Append(style);
                        }

                        if (isFirstFile)
                        {
                            if (string.CompareOrdinal(fileExtension, "svg") != 0)
                            {
                                // LaTeX equations are represented using a graphic format 
                                // other than SVG
                                return;
                            }
                            isFirstFile = false;
                        }

                        src.Value = string.Format(@"ms-xhelp:///?method=asset&id=media\{0}.{1}&package={2}.mshc&topiclocale={3}",
                            fileName, "svg", this.builder.ResolvedHtmlHelpName, this.builder.CurrentProject.Language.Name);

                        embed.Attributes.Append(src);

                        img.ParentNode.ReplaceChild(embed, img);
                        this.builder.ReportProgress("<embed> tag created for SVG file {0}.svg:", fileName);
                        this.builder.ReportProgress(embed.OuterXml);
                    }

                    document.Save(sourceFile);
                }
            }
        }

        #endregion

        #region IDisposable implementation
        //=====================================================================

        // TODO: If the plug-in hasn't got any disposable resources, this finalizer can be removed
        /// <summary>
        /// This handles garbage collection to ensure proper disposal of the plug-in if not done explicitly
        /// with <see cref="Dispose()"/>.
        /// </summary>
        ~ShfbLatexPlugIn()
        {
            this.Dispose();
        }

        /// <summary>
        /// This implements the Dispose() interface to properly dispose of the plug-in object
        /// </summary>
        public void Dispose()
        {
            // TODO: Dispose of any resources here if necessary
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
