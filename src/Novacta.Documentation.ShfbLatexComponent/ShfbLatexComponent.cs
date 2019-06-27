// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Sandcastle.Core.BuildAssembler;
using Sandcastle.Core.BuildAssembler.BuildComponent;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Configuration;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Represents a build component able to add LaTeX formatted formulas within
    /// reference XML comments and conceptual content topics.
    /// </summary>
    public class ShfbLatexComponent : BuildComponentCore
    {
        /// <summary>
        /// Each LaTeX equation is represented as a LaTeX file.
        /// The following field is an equation identifier,
        /// and is used to form distinct names for the corresponding 
        /// LaTeX files.
        /// </summary>
        private static int EquationId = 0;

        private readonly XPathExpression referenceRoot =
            XPathExpression.Compile("document/comments");

        private bool isFileFormatPng;

        private bool isLatexDefaultModeInline;

        private string initialTexDocument;

        #region CONFIGURATION

        /// <summary>
        /// Gets or sets the additional preamble commands.
        /// </summary>
        /// <value>The additional preamble commands.</value>
        public string[] AdditionalPreambleCommands { get; set; }

        private string[] GetAdditionalPreambleCommands(
            XPathNavigator additionalPreambleCommands)
        {
            List<string> lines = new List<string>();

            if (additionalPreambleCommands.MoveToFirstChild())
            {
                lines.Add(additionalPreambleCommands.Value);
                while (additionalPreambleCommands.MoveToNext())
                {
                    lines.Add(additionalPreambleCommands.Value);
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Gets or sets the LaTeX default mode.
        /// </summary>
        /// <value>The LaTeX default mode.</value>
        public string LatexDefaultMode { get; set; }

        /// <summary>
        /// Gets the image file format.
        /// </summary>
        /// <value>The image file format.</value>
        public string ImageFileFormat { get; private set; }

        /// <summary>
        /// Gets or sets the image depth correction.
        /// </summary>
        /// <value>The image depth correction.</value>
        public int ImageDepthCorrection { get; set; }

        /// <summary>
        /// Gets or sets the image scale percentage.
        /// </summary>
        /// <value>The image scale percentage.</value>
        public double ImageScalePercentage { get; set; }

        /// <summary>
        /// Gets a value indicating whether file processors
        /// standard output must be redirected to the 
        /// Sandcastle Help File Builder Log.
        /// </summary>
        /// <value><c>true</c> if file processors must be redirected;
        /// otherwise, <c>false</c>.</value>
        public bool RedirectFileProcessors { get; private set; }

        /// <summary>
        /// Gets the LaTeX file processor.
        /// </summary>
        /// <value>The LaTeX file processor.</value>
        public FileProcessor Latex { get; private set; }

        /// <summary>
        /// Gets the DviPng file processor.
        /// </summary>
        /// <value>The DviPng file processor.</value>
        public FileProcessor DviPng { get; private set; }

        /// <summary>
        /// Gets the DviSvgm file processor.
        /// </summary>
        /// <value>The DviSvgm file processor.</value>
        public FileProcessor DviSvgm { get; private set; }

        /// <summary>
        /// Gets the output folders.
        /// </summary>
        /// <value>The output folders.</value>
        public string[] OutputFolders { get; private set; }

        /// <summary>
        /// Gets the working folder for SHFB.
        /// </summary>
        /// <remarks>
        /// It is equivalent to basePath.
        /// </remarks>
        /// <value>The working folder for SHFB.</value>
        public string BasePath { get; private set; }

        /// <summary>
        /// Gets the working folder for file processors.
        /// </summary>
        /// <remarks>
        /// It is equivalent to basePath + "\LaTeX\".
        /// </remarks>
        /// <value>The working folder for file processors.</value>
        public string WorkingFolder { get; private set; }

        #endregion

        #region Build component factory for MEF
        //=====================================================================

        /// <summary>
        /// This is used to create a new instance of the build component
        /// </summary>
        [BuildComponentExport(
            "LaTeX Component",
            IsVisible = true,
            IsConfigurable = true,
            Version = AssemblyInfo.Version,
            Copyright = AssemblyInfo.Copyright,
            Description = AssemblyInfo.Description)]
        public sealed class Factory : BuildComponentFactory
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public Factory()
            {
                // Build placement tells tools such as the Sandcastle 
                // Help File Builder how to insert the
                // component into build configurations in projects 
                // to which it is added.

                // Set placement for reference builds or remove 
                // if not used in reference builds
                base.ReferenceBuildPlacement = new ComponentPlacement(
                    PlacementAction.Before,
                    "XSL Transform Component");

                // Set placement for conceptual builds or remove 
                // if not used in conceptual builds
                base.ConceptualBuildPlacement = new ComponentPlacement(
                    PlacementAction.Before,
                    "XSL Transform Component");
            }

            /// <inheritdoc />
            public override BuildComponentCore Create()
            {
                return new ShfbLatexComponent(base.BuildAssembler);
            }

            /// <inheritdoc />
            public override string DefaultConfiguration
            {
                get
                {
                    return
                        @"<documentClass value=""article"" />" +
                        @"<imageFileFormat value=""SVG"" />" +
                        @"<additionalPreambleCommands>" +
                        @"<line>" +
                        @"% Paste here your additional preamble commands" +
                        @"</line>" +
                        @"</additionalPreambleCommands>" +
                        @"<latexDefaultMode value=""display""/>" +
                        @"<imageDepthCorrection value=""0"" />" +
                        @"<imageScalePercentage value=""100"" />" +
                        @"<redirectFileProcessors value=""false"" />" +
                        @"<dvisvgmBinPath value="""" />" +
                        @"<latexBinPath value="""" />" +
                        @"<helpType value=""{@HelpFileFormat}"" />" +
                        @"<basePath value=""{@WorkingFolder}"" />" +
                        @"<languagefilter value=""true"" />";
                }
            }

            /// <inheritdoc />
            public override string ConfigureComponent(
                string currentConfiguration,
                CompositionContainer container)
            {
                // Open the dialog to edit the configuration
                using (var dlg = new LatexConfigDlg(currentConfiguration))
                {
                    // Get the modified configuration if OK was clicked
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {

                        var config = XElement.Parse(currentConfiguration);

                        // additionalPreambleCommands
                        var additionalPreambleCommandsNode =
                            config.Element("additionalPreambleCommands");

                        additionalPreambleCommandsNode.RemoveNodes();
                        for (int i = 0; i < dlg.AdditionalPreambleCommands.Length; i++)
                        {
                            additionalPreambleCommandsNode.Add(
                                new XElement("line", dlg.AdditionalPreambleCommands[i]));
                        }

                        // latexDefaultMode
                        config.Element("latexDefaultMode").Attribute("value")
                            .SetValue(dlg.LatexDefaultMode);

                        // imageFileFormat
                        config.Element("imageFileFormat").Attribute("value")
                            .SetValue(dlg.ImageFileFormat);

                        // imageDepthCorrection
                        config.Element("imageDepthCorrection").Attribute("value")
                           .SetValue(dlg.ImageDepthCorrection);

                        // imageScaleFactor
                        config.Element("imageScalePercentage").Attribute("value")
                           .SetValue(dlg.ImageScalePercentage);

                        // redirectFileProcessors
                        config.Element("redirectFileProcessors").Attribute("value")
                            .SetValue(dlg.RedirectFileProcessors);

                        // latexBinPath
                        config.Element("latexBinPath").Attribute("value")
                             .SetValue(dlg.LatexBinFolder);

                        // dvisvgmBinPath
                        config.Element("dvisvgmBinPath").Attribute("value")
                            .SetValue(dlg.DviSvgmBinFolder);

                        currentConfiguration = config.ToString();
                    }
                }

                return currentConfiguration;
            }
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buildAssembler">A reference to the build assembler</param>
        protected ShfbLatexComponent(BuildAssemblerCore buildAssembler)
            : base(buildAssembler)
        {
        }

        #endregion

        #region APPLY HELPER METHODS

        #region LATEX MODE

        private static readonly string[] SupportedLaTeXModes =
            new string[2] { "inline", "display" };

        /// <summary>
        /// Determines whether the specified mode is a supported LaTeX mode.
        /// </summary>
        /// <param name="mode">The mode to be checked.</param>
        /// <returns><c>true</c> if the specified mode is supported; 
        /// otherwise, <c>false</c>.</returns>
        private static bool IsLaTeXModeSupported(string mode)
        {
            foreach (var supportedMode in SupportedLaTeXModes)
            {
                if (0 == string.CompareOrdinal(supportedMode, mode))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region LATEX SCALE

        private static readonly double BasePngResolution = 120.0;
        private static readonly double BaseSvgZoomFactor = 1.2;

        private static readonly string[] PredefinedScaleNames = new string[10] {
            "tiny",
            "scriptsize",
            "footnotesize",
            "small",
            "normalsize",
            "large",
            "Large",
            "LARGE",
            "huge",
            "Huge" };

        private static Dictionary<string, double> InitializePredefinedScaleFactors()
        {
            Dictionary<string, double> zoomFactors = new Dictionary<string, double>();

            double[] factors = new double[10] {
                0.5,
                0.7,
                0.8,
                0.9,
                1,
                1.2,
                1.44,
                1.728,
                2.074,
                2.488
            };

            for (int i = 0; i < 10; i++)
            {
                zoomFactors[PredefinedScaleNames[i]] = factors[i];
            }

            return zoomFactors;
        }

        private static readonly Dictionary<string, double> PredefinedScaleFactors =
            InitializePredefinedScaleFactors();

        /// <summary>
        /// Determines whether the specified scale is supported.
        /// </summary>
        /// <param name="scale">The scale to be checked.</param>
        /// <param name="scaleFactor">When this method returns, contains the factor associated 
        /// with the specified scale, if the scale is supported; otherwise, zero. 
        /// </param>
        /// <returns><c>true</c> if the specified scale is supported;
        /// otherwise, <c>false</c>.</returns>
        private static bool TryGetScaleFactor(string scale, out double scaleFactor)
        {
            bool isScalePredefined = PredefinedScaleFactors.TryGetValue(scale, out scaleFactor);
            if (isScalePredefined)
            {
                return true;
            }

            bool isParsable = Double.TryParse(scale, out double scalePercentage);
            scaleFactor = scalePercentage / 100.0;
            if (!isParsable)
            {
                return false;
            }
            if (scaleFactor <= 0.0)
            {
                scaleFactor = 0.0;
                return false;
            }

            return true;
        }

        #endregion

        #endregion

        #region Abstract method implementations
        //=====================================================================

        /// <summary>
        /// Initialize the build component.
        /// </summary>
        /// <param name="configuration">The component configuration.</param>
        public override void Initialize(XPathNavigator configuration)
        {
            this.WriteMessage(MessageLevel.Info,
                "[{0}, version {1}]\r\n   Novacta LaTeX Component.  {2}",
                AssemblyInfo.Title,
                AssemblyInfo.Version,
                AssemblyInfo.Copyright);

            #region ADDITIONAL PREAMBLE COMMANDS

            var additionalPreambleCommands =
                configuration.SelectSingleNode("//additionalPreambleCommands");

            this.AdditionalPreambleCommands =
                (additionalPreambleCommands is null) 
                ?
                    new string[1]
                        { "% Paste here your additional preamble commands" }
                :
                this.GetAdditionalPreambleCommands(
                    configuration.SelectSingleNode("//additionalPreambleCommands"));

            var texBuilder = new StringBuilder();
            texBuilder.Append("\\documentclass[10pt]{article}\r\n");
            texBuilder.Append("\\usepackage{amsmath}\r\n");
            texBuilder.Append("\\usepackage{amsfonts}\r\n");
            texBuilder.Append("\\usepackage[active,textmath,displaymath]{preview}\r\n");
            texBuilder.Append("\\pagestyle{empty}\r\n");

            for (int i = 0; i < this.AdditionalPreambleCommands.Length; i++)
            {
                texBuilder.AppendLine(this.AdditionalPreambleCommands[i]);
            }

            texBuilder.Append("\\begin{document}\r\n");

            this.initialTexDocument = texBuilder.ToString();

            #endregion

            #region LATEX DEFAULT MODE

            var latexDefaultMode = configuration.SelectSingleNode("//latexDefaultMode");
            this.LatexDefaultMode = 
                (latexDefaultMode is null)
                ?
                "display"
                :
                latexDefaultMode
                    .GetAttribute("value", String.Empty).ToLowerInvariant();

            this.isLatexDefaultModeInline =
                (0 == String.CompareOrdinal(this.LatexDefaultMode, "inline")) ? true : false;

            #endregion

            #region IMAGE FILE FORMAT

            var imageFormatNode = configuration.SelectSingleNode("//imageFileFormat");
            this.ImageFileFormat = imageFormatNode
                .GetAttribute("value", String.Empty).ToLowerInvariant();

            this.isFileFormatPng =
                (0 == String.CompareOrdinal(this.ImageFileFormat, "png")) ? true : false;

            #endregion

            #region IMAGE DEPTH CORRECTION

            var imageDepthCorrectionNode = configuration.SelectSingleNode("//imageDepthCorrection");

            this.ImageDepthCorrection = Convert.ToInt32(
                imageDepthCorrectionNode.GetAttribute("value", String.Empty));

            #endregion

            #region IMAGE SCALE FACTOR

            var imageScalePercentageNode = configuration.SelectSingleNode("//imageScalePercentage");

            this.ImageScalePercentage = Convert.ToDouble(
                imageScalePercentageNode.GetAttribute("value", String.Empty));

            #endregion

            #region REDIRECT FILE PROCESSORS

            // redirectFileProcessors

            var redirectFileProcessorsNode =
                configuration.SelectSingleNode("//redirectFileProcessors");

            this.RedirectFileProcessors = Convert.ToBoolean(
                redirectFileProcessorsNode.GetAttribute("value", String.Empty));

            #endregion

            #region LATEX

            #region WORKING FOLDER

            var basePathNode = configuration.SelectSingleNode("//basePath");

            var basePath = basePathNode.GetAttribute("value", String.Empty) +
                @"Help\Working\";

            this.BasePath = basePath;

            string workingFolder = basePath + "LaTeX";

            this.WorkingFolder = workingFolder;

            if (!Directory.Exists(workingFolder))
            {
                try
                {
                    Directory.CreateDirectory(workingFolder);
                }
                catch (Exception ex)
                {
                    this.WriteMessage(MessageLevel.Error, ex.Message);
                }
            }

            #endregion

            #region BIN FOLDER

            // latexBinPath

            var latexBinPathNode = configuration.SelectSingleNode("//latexBinPath");
            var latexBinFolder = latexBinPathNode.GetAttribute("value", String.Empty);

            #endregion

            this.Latex = new Latex(latexBinFolder, workingFolder);

            string defaultPngResolution = Convert.ToString(BasePngResolution);
            if (this.ImageScalePercentage != 100.0)
            {
                defaultPngResolution = Convert.ToString(
                    Math.Ceiling(BasePngResolution * this.ImageScalePercentage / 100.0));
            }
            this.DviPng = new DviPng(latexBinFolder, workingFolder, defaultPngResolution);

            #endregion

            #region DVISVGM

            // dvisvgmBinPath

            var dvisvgmBinPathNode = configuration.SelectSingleNode("//dvisvgmBinPath");

            var dvisvgmBinFolder = dvisvgmBinPathNode.GetAttribute("value", String.Empty);

            string defaultSvgZoomFactor = Convert.ToString(BaseSvgZoomFactor);
            if (this.ImageScalePercentage != 100.0)
            {
                defaultSvgZoomFactor = Convert.ToString(
                    BaseSvgZoomFactor * this.ImageScalePercentage / 100.0);
            }
            this.DviSvgm = new DviSvgm(
                dvisvgmBinFolder, 
                workingFolder, 
                defaultSvgZoomFactor,
                this.RedirectFileProcessors);

            #endregion

            #region DOCUMENTATION OUTPUT FOLDERS

            var helpTypeNode = configuration.SelectSingleNode("//helpType");

            var helpTypes = helpTypeNode.GetAttribute("value", String.Empty);
            var types = helpTypes.Split(',');

            var outputFolders = new string[types.Length];

            for (var i = 0; i < outputFolders.Length; i++)
            {
                var type = types[i].Trim();

                string path;
                if (type.Equals("HtmlHelp1", StringComparison.InvariantCultureIgnoreCase))
                {
                    path = @"Output\HtmlHelp1\media\";
                }
                else if (type.Equals("Website", StringComparison.InvariantCultureIgnoreCase))
                {
                    path = @"Output\Website\media\";
                }
                else if (type.Equals("MSHelpViewer", StringComparison.InvariantCultureIgnoreCase))
                {
                    path = @"Output\MSHelpViewer\media\";
                }
                else if (type.Equals("OpenXml", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!this.isFileFormatPng)
                    {
                        throw new ConfigurationErrorsException(
                            String.Format("The SGV file format is not supported by the " +
                            "help file format {0}.", type));
                    }
                    path = @"Output\OpenXml\media\";
                }
                else if (type.Equals("Markdown", StringComparison.InvariantCultureIgnoreCase))
                {
                    path = @"Output\Markdown\media\";
                }
                else
                {
                    throw new ConfigurationErrorsException(
                        String.Format("Help file format {0} is not supported " +
                        "by the Novacta Latex Component.", type));
                }
                outputFolders[i] = basePath + path;
            }

            this.OutputFolders = outputFolders;

            #endregion

            #region COMPONENT CONFIGURATION MESSAGES

            this.WriteMessage(MessageLevel.Info,
                String.Format(
                    CultureInfo.InvariantCulture,
                    "{0}, version {1} - {2}.",
                    AssemblyInfo.Title,
                    AssemblyInfo.Version,
                    AssemblyInfo.Copyright));

            this.WriteMessage(MessageLevel.Info, "Additional preamble commands:");
            for (int i = 0; i < this.AdditionalPreambleCommands.Length; i++)
            {
                this.WriteMessage(MessageLevel.Info,
                    this.AdditionalPreambleCommands[i]);
            }

            this.WriteMessage(MessageLevel.Info,
                string.Format("Documentation working folder: {0}", basePath));
            this.WriteMessage(MessageLevel.Info,
                string.Format("Component working folder: {0}", workingFolder));
            this.WriteMessage(MessageLevel.Info,
                "Default LaTex mode: " + this.LatexDefaultMode);
            this.WriteMessage(MessageLevel.Info,
                "Image File Format: " + this.ImageFileFormat);
            this.WriteMessage(MessageLevel.Info,
                string.Format("Image depth correction: {0}", this.ImageDepthCorrection));
            this.WriteMessage(MessageLevel.Info,
                string.Format("Image scale percentage: {0}", this.ImageScalePercentage));
            this.WriteMessage(MessageLevel.Info,
                "Redirect file processors: " + this.RedirectFileProcessors.ToString());
            this.WriteMessage(MessageLevel.Info,
                string.Format("LaTeX binary folder: {0}", latexBinFolder));
            this.WriteMessage(MessageLevel.Info,
                string.Format("DviSvgm binary folder: {0}", dvisvgmBinFolder));

            #endregion
        }

        /// <summary>
        /// This is implemented to perform the component tasks
        /// using different settings for conceptual and reference topics.
        /// </summary>
        /// <param name="document">The XML document under study.</param>
        /// <param name="key">The key (member name) of the item being documented.</param>
        /// <param name="namePrefix">The prefix for LaTeX equation names.</param>
        /// <param name="list">The list of latex nodes to be transformed.</param>
        /// <param name="isTopicConceptual">if set to <c>true</c> the topic is conceptual; otherwise, <c>false</c>.</param>
        private void Apply(XmlDocument document, string key,
            string namePrefix, XmlNodeList list, bool isTopicConceptual)
        {
            if (list is null)
            {
                return;
            }

            string defaultConceptualNamespace = isTopicConceptual ?
                "http://ddue.schemas.microsoft.com/authoring/2003/5" : null;

            string equationName = null;
            StringBuilder texBuilder = new StringBuilder();

            foreach (XmlNode node in list)
            {

                #region MODE ATTRIBUTE

                // LaTeX mode attribute defaults to a configuration option
                string latexMode = this.LatexDefaultMode;
                if (!(node.Attributes["mode"] is null))
                {
                    string mode = node.Attributes["mode"].InnerText;
                    if (IsLaTeXModeSupported(mode))
                    {
                        latexMode = mode;
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized LaTeX mode: \"" +
                            mode + "\". Using default mode: \"" + latexMode + "\".");
                    }
                }

                #endregion

                equationName = namePrefix + EquationId;

                #region LATEX GENERATION

                texBuilder.Clear();

                string texFileName = equationName + ".tex";
                string defaultLatexScale = "normalsize";
                string latexNodeInnerText = node.InnerText.Replace("\\\\", "\\\\\\\\").Trim();

                texBuilder.Append(this.initialTexDocument);

                switch (latexMode)
                {
                    case "inline":
                        texBuilder.AppendFormat("\\begin{{{0}}}${1}$\\end{{{2}}}\r\n",
                            defaultLatexScale, latexNodeInnerText, defaultLatexScale);
                        break;
                    case "display":
                        texBuilder.AppendFormat("\\begin{{{0}}}\\[{1}\\]\\end{{{2}}}\r\n",
                               defaultLatexScale, latexNodeInnerText, defaultLatexScale);
                        break;
                }

                texBuilder.Append("\\end{document}\r\n");

                using (TextWriter stringWriter = new StreamWriter(
                    this.WorkingFolder + Path.DirectorySeparatorChar + texFileName))
                {
                    stringWriter.Write(texBuilder.ToString());
                }

                #endregion

                #region SCALE ATTRIBUTE

                string pngResolution = null;
                string svgZoomFactor = null;
#pragma warning disable IDE0018 // Inline variable declaration
                double scaleFactor;
#pragma warning restore IDE0018 // Inline variable declaration

                if (!(node.Attributes["scale"] is null))
                {
                    string scale = node.Attributes["scale"].InnerText;
                    if (TryGetScaleFactor(scale, out scaleFactor))
                    {
                        pngResolution = Convert.ToString(
                           Math.Ceiling(scaleFactor * BasePngResolution));
                        svgZoomFactor = Convert.ToString(scaleFactor * BaseSvgZoomFactor);
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized scale: \"" +
                            scale + "\". Using default scale percentage: \"" + "100" + "\".");
                    }
                }

                #endregion

                #region DVI GENERATION

                string latexOutput = this.Latex.Run(texFileName);
                if (this.RedirectFileProcessors)
                {
                    this.WriteMessage(MessageLevel.Info, "Running LaTeX on " + texFileName + ".");
                    this.WriteMessage(MessageLevel.Info, latexOutput);
                }

                #endregion

                string outputFile, dviFileName = equationName + ".dvi";

                #region PNG GENERATION

                string dvipngOutput = this.DviPng.Run(equationName, pngResolution);
                if (this.RedirectFileProcessors)
                {
                    this.WriteMessage(MessageLevel.Info, "Running DviPng on " + dviFileName + ".");
                    this.WriteMessage(MessageLevel.Info, dvipngOutput);
                }

                string pngFileName = equationName + ".png";
                string pngFilePath = this.WorkingFolder +
                     Path.DirectorySeparatorChar + pngFileName;

                if (this.isFileFormatPng)
                {
                    foreach (var outputFolder in this.OutputFolders)
                    {
                        outputFile = outputFolder + pngFileName;
                        WriteMessage(MessageLevel.Info,
                            string.Format("Copying {0} to {1}", pngFilePath, outputFile));

                        File.Copy(pngFilePath, outputFile, true);
                    }

                }

                #endregion

                #region DEPTH ATTRIBUTE

                int imageDepth = 0;

                bool applyCorrectedDvipngDepth = true;

                if (!(node.Attributes["depth"] is null))
                {
                    string depth = node.Attributes["depth"].InnerText;
                    if (Int32.TryParse(depth, out imageDepth))
                    {
                        applyCorrectedDvipngDepth = false;
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized depth: \"" +
                            depth + "\". Using corrected DviPng depth.");
                    }
                }
                if (applyCorrectedDvipngDepth)
                {
                    // Determine the DviPng image depth
                    int firstDepthPosition = dvipngOutput.IndexOf("depth=") + 6;
                    int lastDepthPosition = dvipngOutput.IndexOf("]", firstDepthPosition) - 1;
                    int dvipngImageDepth = Convert.ToInt32(
                        dvipngOutput.Substring(firstDepthPosition, 1 + lastDepthPosition - firstDepthPosition));
                    imageDepth = -this.ImageDepthCorrection + dvipngImageDepth;
                }

                #endregion

                if (!this.isFileFormatPng)
                {
                    #region SVG GENERATION

                    string dvisvgmOutput = this.DviSvgm.Run(equationName, svgZoomFactor);
                    if (this.RedirectFileProcessors)
                    {
                        this.WriteMessage(MessageLevel.Info, "Running DviSvgm on " + dviFileName + ".");
                        this.WriteMessage(MessageLevel.Info, dvisvgmOutput);
                    }

                    string svgFileName = equationName + ".svg";

                    string svgFilePath = this.WorkingFolder +
                        Path.DirectorySeparatorChar + svgFileName;

                    foreach (var outputFolder in this.OutputFolders)
                    {
                        outputFile = outputFolder + svgFileName;
                        WriteMessage(MessageLevel.Info,
                            string.Format("Copying {0} to {1}", svgFilePath, outputFile));

                        File.Copy(svgFilePath, outputFile, true);
                    }

                    #endregion
                }

                #region LATEX NODE EMISSION

                bool isInlined = 0 == string.CompareOrdinal(latexMode, "inline");

                XmlNode latex = document.CreateElement("latexImg");

                XmlNode latexEquationName = document.CreateElement("name");
                latexEquationName.InnerText = equationName;
                latex.AppendChild(latexEquationName);

                XmlNode latexImageFileFormat = document.CreateElement("format");
                latexImageFileFormat.InnerText = this.ImageFileFormat;
                latex.AppendChild(latexImageFileFormat);

                XmlNode latexInlined = document.CreateElement("inline");
                latexInlined.InnerText = isInlined ? "1" : "0";
                latex.AppendChild(latexInlined);

                XmlNode latexImageDepth = document.CreateElement("depth");
                latexImageDepth.InnerText = Convert.ToString(imageDepth);
                latex.AppendChild(latexImageDepth);

                this.WriteMessage(MessageLevel.Info,
                    string.Format("Node for LaTeX image {0}: \n {1}",
                    equationName, latex.OuterXml));

                if (!isInlined)
                {
                    if (isTopicConceptual)
                    {
                        XmlNode beforeMarkup = document.CreateElement("markup",
                            defaultConceptualNamespace);
                        beforeMarkup.AppendChild(document.CreateElement("br"));
                        beforeMarkup.AppendChild(document.CreateElement("br"));

                        node.ParentNode.InsertBefore(beforeMarkup, node);

                        XmlNode afterMarkup = document.CreateElement("markup",
                            defaultConceptualNamespace);
                        afterMarkup.AppendChild(document.CreateElement("br"));
                        afterMarkup.AppendChild(document.CreateElement("br"));

                        node.ParentNode.InsertAfter(afterMarkup, node);
                    }
                    else
                    {
                        node.ParentNode.InsertBefore(document.CreateElement("br"), node);
                        node.ParentNode.InsertBefore(document.CreateElement("br"), node);
                        node.ParentNode.InsertAfter(document.CreateElement("br"), node);
                        node.ParentNode.InsertAfter(document.CreateElement("br"), node);
                    }
                }
                node.ParentNode.ReplaceChild(latex, node);

                #endregion

                EquationId++;
            }

        }

        /// <summary>
        /// This is implemented to perform the component tasks.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <param name="key">The key (member name) of the item being documented.</param>
        public override void Apply(XmlDocument document, string key)
        {
            XPathNavigator root, navDoc = document.CreateNavigator();
            root = navDoc.SelectSingleNode(this.referenceRoot);

            // If root is not null, it's a reference (API) build.  
            // If null, it's a conceptual build.
            if (root == null)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ltx", "http://www.novacta.net/2018/XSL/ShfbLatexTools");

                XmlNodeList list = document.SelectNodes("//ltx:latex", nsmgr);
                Apply(document, key, "clatex_", list, true);
            }
            else
            {
                XmlNodeList list = document.SelectNodes("//latex");
                Apply(document, key, "latex_", list, false);
            }
        }

        #endregion
    }
}
