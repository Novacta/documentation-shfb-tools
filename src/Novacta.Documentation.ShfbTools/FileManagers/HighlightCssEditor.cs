// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System;
using System.IO;
using System.Text;

namespace Novacta.Documentation.ShfbTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that adds a style class
    /// to file highlight.css  
    /// when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="HighlightCssEditor"/> 
    /// is expected to manage file highlight.css in the 
    /// Colorizer folder of a SHFB installation
    /// to enable class name highlights.
    /// </para>
    /// <para>
    /// This manager adds a new CSS class representing the color you 
    /// want to be applied when representing a class name, as the following:
    /// <code>
    /// .highlight-class-name { color: #2B91AF; }
    /// </code>
    /// </para>
    /// </remarks>
    class HighlightCssEditor : EditFileManager
    {
        readonly string colorHexCode;

        /// <summary>
        /// Gets the hex code of the color to apply
        /// when colorizing class names.
        /// </summary>
        /// <value>
        /// The hexadecimal code
        /// of the color to apply when highlighting class names.
        /// </value>
        public string ClassNameColorHexCode
        {
            get { return this.colorHexCode; }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="HighlightCssEditor"/> class.
        /// </summary>
        /// <param name="path">The path of the file to edit.</param>
        /// <param name="colorHexCode">The hexadecimal code
        /// of the color to apply when highlighting class names.</param>
        public HighlightCssEditor(
          string path,
          string colorHexCode) : base(path)
        {
            this.colorHexCode = colorHexCode;
        }

        /// <inheritdoc/>
        protected override void OnCommit()
        {
            byte[] initialContent;
            using (BinaryReader binaryReader = new BinaryReader(this.ManagedFileStream, UTF8Encoding.UTF8, true))
            {
                initialContent = binaryReader.ReadBytes(Convert.ToInt32(this.ManagedFileStream.Length));
            }
            StringBuilder builder = new StringBuilder();
            StringReader reader = new StringReader(
                UTF8Encoding.UTF8.GetString(initialContent));
            string cssClass = ".highlight-class-name";
            string cssClassDefinition =
                cssClass + " { color: " + this.colorHexCode + "; }";
            string cssLine;
            bool cssClassExists = false;

            while ((cssLine = reader.ReadLine()) != null)
            {
                if (cssLine.StartsWith(cssClass))
                {
                    cssClassExists = true;
                    builder.AppendLine(cssClassDefinition);
                }
                else
                {
                    builder.AppendLine(cssLine);
                }
            }
            if (!cssClassExists)
            {
                builder.AppendLine(cssClassDefinition);
            }

            this.ManagedFileStream.SetLength(0);

            using (StreamWriter writer = new StreamWriter(this.ManagedFileStream))
            {
                writer.Write(builder.ToString());
            }
        }
    }
}
