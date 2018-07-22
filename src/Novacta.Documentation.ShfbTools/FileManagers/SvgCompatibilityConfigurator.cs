// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System.Xml;

namespace Novacta.Documentation.ShfbTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that edits a SHFB build
    /// configuration file to enable SVG compatibility
    /// when a transaction is successfully committed.
    /// </summary>
    class SvgCompatibilityConfigurator : EditFileManager
    {
        public SvgCompatibilityConfigurator(string path) : base(path)
        {
        }

        /// <summary>
        /// Sets the meta content attribute of the specified SHFB help output.
        /// </summary>
        /// <param name="helpOutputNode">The help output node.</param>
        /// <param name="content">The content to set.</param>
        private static void EditMetaContentAttribute(XmlNode helpOutputNode, string content)
        {
            XmlNode additionalHeaderResourcesNode =
                helpOutputNode.SelectSingleNode("component[@id='Additional Header Resources Component']");

            XmlNode metaNode = additionalHeaderResourcesNode.SelectSingleNode("meta");

            XmlAttribute contentAttribute = metaNode.Attributes["content"];
            contentAttribute.Value = content;
        }

        ///<inherithdoc/>
        protected override void OnCommit()
        {
            var document = new XmlDocument();

            document.Load(this.ManagedFileStream);
            XmlNode root = document.DocumentElement;

            string metaContent = "IE=edge";

            #region MAML

            XmlNode mamlCaseNode = root.SelectSingleNode(
                "//case[@value='MAML']");

            XmlNode helpOutputNode =
                mamlCaseNode.SelectSingleNode(".//helpOutput[@format='MSHelpViewer']");

            EditMetaContentAttribute(helpOutputNode, metaContent);

            helpOutputNode =
                mamlCaseNode.SelectSingleNode(".//helpOutput[@format='HtmlHelp1']");

            EditMetaContentAttribute(helpOutputNode, metaContent);

            #endregion

            #region API

            XmlNode apiCaseNode = root.SelectSingleNode(
                "//case[@value='API']");

            helpOutputNode =
                apiCaseNode.SelectSingleNode(".//helpOutput[@format='MSHelpViewer']");

            EditMetaContentAttribute(helpOutputNode, metaContent);

            helpOutputNode =
                apiCaseNode.SelectSingleNode(".//helpOutput[@format='HtmlHelp1']");

            EditMetaContentAttribute(helpOutputNode, metaContent);

            #endregion

            this.ManagedFileStream.SetLength(0);
            document.Save(this.ManagedFileStream);
        }
    }
}
