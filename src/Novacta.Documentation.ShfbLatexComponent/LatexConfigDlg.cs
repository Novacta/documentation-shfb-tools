// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.Threading;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Represents a configuration dialog box for the Novacta
    /// ShfbLatexComponent.
    /// </summary>
    public partial class LatexConfigDlg : Form
    {
        /// <summary>
        /// Gets or sets the image file format.
        /// </summary>
        /// <value>The image file format.</value>
        public string ImageFileFormat { get; set; }

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
        /// Gets or sets a value indicating whether file processors
        /// standard output must be redirected to the 
        /// Sandcastle Help File Builder Log.
        /// </summary>
        /// <value><c>true</c> if file processors must be redirected;
        /// otherwise, <c>false</c>.</value>
        public bool RedirectFileProcessors { get; set; }


        /// <summary>
        /// Gets or sets the Latex bin folder.
        /// </summary>
        /// <value>The MiXTeX bin folder.</value>
        public string LatexBinFolder { get; set; }

        /// <summary>
        /// Gets or sets the DviSvgm bin folder.
        /// </summary>
        /// <value>The DviSvgm bin folder.</value>
        public string DviSvgmBinFolder { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LatexConfigDlg"/> class.
        /// </summary>
        public LatexConfigDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexConfigDlg"/> 
        /// class by parsing the configuration XML.
        /// </summary>
        /// <param name="configXml">The configuration XML.</param>
        /// <remarks>
        /// <para>
        /// An XML default configuration is expected as follows:
        /// </para>
        /// <para>
        /// <code language="XML" title="Default Configuration">
        /// <![CDATA[
        /// <?xml version="1.0" encoding="utf-8"?>
        /// <component id="Novacta.Shfb.LatexComponent">
        ///    <documentClass value="article" />
        ///    <imageFileFormat value="PNG" />
        ///    <redirectFileProcessors value="true" />
        ///    <imageDepthCorrection value="17" />
        ///    <imageScalePercentage value="100" />
        ///    <latexBinPath value="C:\Program Files\MiKTeX 2.9\miktex\bin\x64" />
        ///    <dvisvgmBinPath value="C:\Program Files\MiKTeX 2.9\miktex\bin\x64" />
        ///    <helpType value="{@HelpFileFormat}" />
        ///    <basePath value="{@WorkingFolder}" />
        ///    <languagefilter value="true" />
        /// </component>
        /// ]]>
        /// </code>
        /// </para>
        /// </remarks>
        public LatexConfigDlg(string configXml)
        {
            this.InitializeComponent();
            var config = XElement.Parse(configXml);

            // Image File Format
            string format;
            RadioButton radioButton;

            format = config.Element("imageFileFormat")
                 .Attribute("value").Value;

            radioButton = this.c_groupBoxFileFormat.Controls.OfType<RadioButton>()
              .First(r => 0 == string.CompareOrdinal(r.Text, format));

            radioButton.Checked = true;

            // Image Depth Correction
            this.c_imageDepthCorrection.Value = decimal.Parse(
                config.Element("imageDepthCorrection")
                      .Attribute("value").Value);

            // Image Scale Factor
            this.c_imageScalePercentage.Value = decimal.Parse(
                config.Element("imageScalePercentage")
                      .Attribute("value").Value);

            // Redirect File Processors
            this.c_redirectFileProcessors.Checked = bool.Parse(
                config.Element("redirectFileProcessors")
                      .Attribute("value").Value);

            // MiKTeX Bin Folder
            this.c_latexBinFolder.Text = config
                .Element("latexBinPath")
                .Attribute("value").Value;

            // DviSvgm Bin Folder
            this.c_dvisvgmBinFolder.Text = config
                .Element("dvisvgmBinPath")
                .Attribute("value").Value;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Image File Format
            RadioButton radioButton;

            radioButton = this.c_groupBoxFileFormat.Controls
                .OfType<RadioButton>()
                .First(r => r.Checked);
            this.ImageFileFormat = radioButton.Text;

            // Image Depth Correction
            this.ImageDepthCorrection = Convert.ToInt32(this.c_imageDepthCorrection.Value);

            // Image Scale Percentage
            this.ImageScalePercentage = Convert.ToDouble(this.c_imageScalePercentage.Value);

            // Redirect File Processors
            this.RedirectFileProcessors = this.c_redirectFileProcessors.Checked;

            // MiKTeX Bin Folder
            this.LatexBinFolder = this.c_latexBinFolder.Text;

            // DviSvgm Bin Folder
            this.DviSvgmBinFolder = this.c_dvisvgmBinFolder.Text;

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void latexBrowseButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(this.SelectLatexFolder);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        private void SelectLatexFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetLatexText(dialog.SelectedPath);
            }
        }

        // This method implements a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);

        private void SetLatexText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.c_latexBinFolder.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetLatexText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.c_latexBinFolder.Text = text;
            }
        }

        private void dvisvgmBrowseButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(this.SelectDviSvgmFolder);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void SetDviSvgmText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.c_dvisvgmBinFolder.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetDviSvgmText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.c_dvisvgmBinFolder.Text = text;
            }
        }

        private void SelectDviSvgmFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetDviSvgmText(dialog.SelectedPath);
            }
        }
    }
}
