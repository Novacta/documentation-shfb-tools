namespace Novacta.Documentation.ShfbTools
{
    partial class LatexConfigDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.c_redirectFileProcessors = new System.Windows.Forms.CheckBox();
            this.c_imageScalePercentage = new System.Windows.Forms.NumericUpDown();
            this.imageScaleFactorLabel = new System.Windows.Forms.Label();
            this.latexBinFolderLabel = new System.Windows.Forms.Label();
            this.c_latexBinFolder = new System.Windows.Forms.TextBox();
            this.latexBrowseButton = new System.Windows.Forms.Button();
            this.dvisvgmBinFolderLabel = new System.Windows.Forms.Label();
            this.c_dvisvgmBinFolder = new System.Windows.Forms.TextBox();
            this.dvisvgmBrowseButton = new System.Windows.Forms.Button();
            this.graphicRepresentationlabel = new System.Windows.Forms.Label();
            this.c_radioFileFormatPng = new System.Windows.Forms.RadioButton();
            this.c_radioFileFormatSvg = new System.Windows.Forms.RadioButton();
            this.c_groupBoxFileFormat = new System.Windows.Forms.GroupBox();
            this.imageDepthCorrectionLabel = new System.Windows.Forms.Label();
            this.c_imageDepthCorrection = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageScalePercentage)).BeginInit();
            this.c_groupBoxFileFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageDepthCorrection)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(259, 553);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(84, 29);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(350, 553);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(84, 29);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // c_redirectFileProcessors
            // 
            this.c_redirectFileProcessors.AutoSize = true;
            this.c_redirectFileProcessors.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.c_redirectFileProcessors.Location = new System.Drawing.Point(13, 318);
            this.c_redirectFileProcessors.Name = "c_redirectFileProcessors";
            this.c_redirectFileProcessors.Size = new System.Drawing.Size(237, 24);
            this.c_redirectFileProcessors.TabIndex = 5;
            this.c_redirectFileProcessors.Text = "Redirect file processors:        ";
            this.c_redirectFileProcessors.UseVisualStyleBackColor = true;
            // 
            // c_imageScalePercentage
            // 
            this.c_imageScalePercentage.Location = new System.Drawing.Point(228, 244);
            this.c_imageScalePercentage.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.c_imageScalePercentage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.c_imageScalePercentage.Name = "c_imageScalePercentage";
            this.c_imageScalePercentage.Size = new System.Drawing.Size(120, 26);
            this.c_imageScalePercentage.TabIndex = 6;
            this.c_imageScalePercentage.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // imageScaleFactorLabel
            // 
            this.imageScaleFactorLabel.AutoSize = true;
            this.imageScaleFactorLabel.Location = new System.Drawing.Point(15, 246);
            this.imageScaleFactorLabel.Name = "imageScaleFactorLabel";
            this.imageScaleFactorLabel.Size = new System.Drawing.Size(188, 20);
            this.imageScaleFactorLabel.TabIndex = 7;
            this.imageScaleFactorLabel.Text = "Image scale percentage: ";
            // 
            // latexBinFolderLabel
            // 
            this.latexBinFolderLabel.AutoSize = true;
            this.latexBinFolderLabel.Location = new System.Drawing.Point(15, 392);
            this.latexBinFolderLabel.Name = "latexBinFolderLabel";
            this.latexBinFolderLabel.Size = new System.Drawing.Size(129, 20);
            this.latexBinFolderLabel.TabIndex = 8;
            this.latexBinFolderLabel.Text = "LaTeX bin folder:";
            // 
            // c_latexBinFolder
            // 
            this.c_latexBinFolder.Location = new System.Drawing.Point(228, 389);
            this.c_latexBinFolder.Name = "c_latexBinFolder";
            this.c_latexBinFolder.ReadOnly = true;
            this.c_latexBinFolder.Size = new System.Drawing.Size(287, 26);
            this.c_latexBinFolder.TabIndex = 9;
            // 
            // latexBrowseButton
            // 
            this.latexBrowseButton.Location = new System.Drawing.Point(538, 385);
            this.latexBrowseButton.Name = "latexBrowseButton";
            this.latexBrowseButton.Size = new System.Drawing.Size(120, 34);
            this.latexBrowseButton.TabIndex = 10;
            this.latexBrowseButton.Text = "Browse...";
            this.latexBrowseButton.UseVisualStyleBackColor = true;
            this.latexBrowseButton.Click += new System.EventHandler(this.latexBrowseButton_Click);
            // 
            // dvisvgmBinFolderLabel
            // 
            this.dvisvgmBinFolderLabel.AutoSize = true;
            this.dvisvgmBinFolderLabel.Location = new System.Drawing.Point(15, 458);
            this.dvisvgmBinFolderLabel.Name = "dvisvgmBinFolderLabel";
            this.dvisvgmBinFolderLabel.Size = new System.Drawing.Size(144, 20);
            this.dvisvgmBinFolderLabel.TabIndex = 11;
            this.dvisvgmBinFolderLabel.Text = "DviSvgm bin folder:";
            // 
            // c_dvisvgmBinFolder
            // 
            this.c_dvisvgmBinFolder.Location = new System.Drawing.Point(228, 455);
            this.c_dvisvgmBinFolder.Name = "c_dvisvgmBinFolder";
            this.c_dvisvgmBinFolder.ReadOnly = true;
            this.c_dvisvgmBinFolder.Size = new System.Drawing.Size(287, 26);
            this.c_dvisvgmBinFolder.TabIndex = 12;
            // 
            // dvisvgmBrowseButton
            // 
            this.dvisvgmBrowseButton.Location = new System.Drawing.Point(538, 451);
            this.dvisvgmBrowseButton.Name = "dvisvgmBrowseButton";
            this.dvisvgmBrowseButton.Size = new System.Drawing.Size(120, 34);
            this.dvisvgmBrowseButton.TabIndex = 13;
            this.dvisvgmBrowseButton.Text = "Browse...";
            this.dvisvgmBrowseButton.UseVisualStyleBackColor = true;
            this.dvisvgmBrowseButton.Click += new System.EventHandler(this.dvisvgmBrowseButton_Click);
            // 
            // graphicRepresentationlabel
            // 
            this.graphicRepresentationlabel.AutoSize = true;
            this.graphicRepresentationlabel.Location = new System.Drawing.Point(15, 85);
            this.graphicRepresentationlabel.Name = "graphicRepresentationlabel";
            this.graphicRepresentationlabel.Size = new System.Drawing.Size(132, 20);
            this.graphicRepresentationlabel.TabIndex = 15;
            this.graphicRepresentationlabel.Text = "Image file format:";
            // 
            // c_radioFileFormatPng
            // 
            this.c_radioFileFormatPng.AutoSize = true;
            this.c_radioFileFormatPng.Location = new System.Drawing.Point(10, 43);
            this.c_radioFileFormatPng.Name = "c_radioFileFormatPng";
            this.c_radioFileFormatPng.Size = new System.Drawing.Size(68, 24);
            this.c_radioFileFormatPng.TabIndex = 3;
            this.c_radioFileFormatPng.Text = "PNG";
            this.c_radioFileFormatPng.UseVisualStyleBackColor = true;
            // 
            // c_radioFileFormatSvg
            // 
            this.c_radioFileFormatSvg.AutoSize = true;
            this.c_radioFileFormatSvg.Checked = true;
            this.c_radioFileFormatSvg.Location = new System.Drawing.Point(107, 43);
            this.c_radioFileFormatSvg.Name = "c_radioFileFormatSvg";
            this.c_radioFileFormatSvg.Size = new System.Drawing.Size(69, 24);
            this.c_radioFileFormatSvg.TabIndex = 4;
            this.c_radioFileFormatSvg.TabStop = true;
            this.c_radioFileFormatSvg.Text = "SVG";
            this.c_radioFileFormatSvg.UseVisualStyleBackColor = true;
            // 
            // c_groupBoxFileFormat
            // 
            this.c_groupBoxFileFormat.Controls.Add(this.c_radioFileFormatSvg);
            this.c_groupBoxFileFormat.Controls.Add(this.c_radioFileFormatPng);
            this.c_groupBoxFileFormat.Location = new System.Drawing.Point(228, 38);
            this.c_groupBoxFileFormat.Name = "c_groupBoxFileFormat";
            this.c_groupBoxFileFormat.Size = new System.Drawing.Size(187, 100);
            this.c_groupBoxFileFormat.TabIndex = 5;
            this.c_groupBoxFileFormat.TabStop = false;
            this.c_groupBoxFileFormat.Text = " Supported formats ";
            // 
            // imageDepthCorrectionLabel
            // 
            this.imageDepthCorrectionLabel.AutoSize = true;
            this.imageDepthCorrectionLabel.Location = new System.Drawing.Point(15, 172);
            this.imageDepthCorrectionLabel.Name = "imageDepthCorrectionLabel";
            this.imageDepthCorrectionLabel.Size = new System.Drawing.Size(181, 20);
            this.imageDepthCorrectionLabel.TabIndex = 19;
            this.imageDepthCorrectionLabel.Text = "Image depth correction: ";
            // 
            // c_imageDepthCorrection
            // 
            this.c_imageDepthCorrection.Location = new System.Drawing.Point(228, 172);
            this.c_imageDepthCorrection.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.c_imageDepthCorrection.Name = "c_imageDepthCorrection";
            this.c_imageDepthCorrection.Size = new System.Drawing.Size(120, 26);
            this.c_imageDepthCorrection.TabIndex = 18;
            // 
            // LaTeXConfigDlg
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(677, 604);
            this.Controls.Add(this.imageDepthCorrectionLabel);
            this.Controls.Add(this.c_imageDepthCorrection);
            this.Controls.Add(this.c_groupBoxFileFormat);
            this.Controls.Add(this.graphicRepresentationlabel);
            this.Controls.Add(this.dvisvgmBrowseButton);
            this.Controls.Add(this.c_dvisvgmBinFolder);
            this.Controls.Add(this.dvisvgmBinFolderLabel);
            this.Controls.Add(this.latexBrowseButton);
            this.Controls.Add(this.c_latexBinFolder);
            this.Controls.Add(this.latexBinFolderLabel);
            this.Controls.Add(this.imageScaleFactorLabel);
            this.Controls.Add(this.c_imageScalePercentage);
            this.Controls.Add(this.c_redirectFileProcessors);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LaTeXConfigDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LaTeX Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.c_imageScalePercentage)).EndInit();
            this.c_groupBoxFileFormat.ResumeLayout(false);
            this.c_groupBoxFileFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageDepthCorrection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox c_redirectFileProcessors;
        private System.Windows.Forms.NumericUpDown c_imageScalePercentage;
        private System.Windows.Forms.Label imageScaleFactorLabel;
        private System.Windows.Forms.Label latexBinFolderLabel;
        private System.Windows.Forms.TextBox c_latexBinFolder;
        private System.Windows.Forms.Button latexBrowseButton;
        private System.Windows.Forms.Label dvisvgmBinFolderLabel;
        private System.Windows.Forms.TextBox c_dvisvgmBinFolder;
        private System.Windows.Forms.Button dvisvgmBrowseButton;
        private System.Windows.Forms.Label graphicRepresentationlabel;
        private System.Windows.Forms.RadioButton c_radioFileFormatPng;
        private System.Windows.Forms.RadioButton c_radioFileFormatSvg;
        private System.Windows.Forms.GroupBox c_groupBoxFileFormat;
        private System.Windows.Forms.Label imageDepthCorrectionLabel;
        private System.Windows.Forms.NumericUpDown c_imageDepthCorrection;
    }
}