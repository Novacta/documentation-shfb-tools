// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System.Xml;

namespace Novacta.Documentation.ShfbTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that adds a template
    /// to file highlight.xsl 
    /// when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="HighlightCssEditor"/> 
    /// is expected to manage file highlight.xsl in the 
    /// Colorizer folder of a SHFB installation
    /// to enable class name highlights.
    /// </para>
    /// <para>
    /// This manager adds a new template to match the class rule
    /// set in the highlight.css file, as follows:
    /// <code language="xml">
    /// <![CDATA[
    /// <xsl:template match="class-name">
    ///    <span class="highlight-class-name">
    ///        <xsl:value-of select = "text()"
    ///                      disable-output-escaping="yes" />
    ///    </span>
    /// </xsl:template>
    /// ]]>
    /// </code>
    /// </para>
    /// </remarks>
    class HighlightXslEditor : EditFileManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightXslEditor"/> class.
        /// </summary>
        /// <param name="path">The path of the file to edit.</param>
        public HighlightXslEditor(
            string path) : base(path)
        {
        }

        /// <inheritdoc/>
        protected override void OnCommit()
        {
            var document = new XmlDocument();

            document.Load(this.ManagedFileStream);
            XmlNode root = document.DocumentElement;

            // Add the Transform name space.
            string xslNamespace = "http://www.w3.org/1999/XSL/Transform";
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("xsl", xslNamespace);

            string suffix = "class-name";
            XmlNode targetTemplate =
                root.SelectSingleNode("//xsl:template[@match='" + suffix + "']", nsmgr);

            if (targetTemplate is null)
            {
                // targetTemplate
                targetTemplate = document.CreateElement("xsl", "template", xslNamespace);
                XmlAttribute match = document.CreateAttribute("match");
                match.Value = suffix;
                targetTemplate.Attributes.Append(match);

                // span
                XmlNode span = document.CreateElement("span");
                XmlAttribute spanClass = document.CreateAttribute("class");
                spanClass.Value = "highlight-" + suffix;
                span.Attributes.Append(spanClass);
                targetTemplate.AppendChild(span);

                // value-of
                XmlNode valueOf = document.CreateElement("xsl", "value-of", xslNamespace);
                XmlAttribute select = document.CreateAttribute("select");
                select.Value = "text()";
                valueOf.Attributes.Append(select);
                XmlAttribute disableOutputEscaping = document.CreateAttribute("disable-output-escaping");
                disableOutputEscaping.Value = "yes";
                valueOf.Attributes.Append(disableOutputEscaping);
                span.AppendChild(valueOf);

                XmlNode outputNode = root.SelectSingleNode("//xsl:output", nsmgr);
                root.InsertAfter(targetTemplate, outputNode);

                this.ManagedFileStream.SetLength(0);
                document.Save(this.ManagedFileStream);
            }
        }
    }
}
