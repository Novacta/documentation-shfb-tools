// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System.Xml;

namespace Novacta.Documentation.ShfbTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that removes a family of
    /// keywords from file highlight.xml 
    /// when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="HighlightXmlFamilyRemover"/> 
    /// is expected to manage file highlight.xml in the 
    /// Colorizer folder of a SHFB installation
    /// to disable class name highlights.
    /// </para>
    /// <para>
    /// The aim of this manager is twofold.
    /// <list type="bullet">
    /// <item>
    /// It removes the keyword list identified by
    /// <see cref="Family"/>, if any can be found
    /// in the target installation.
    /// </item>
    /// <item>
    /// It removes every keyword rules having attribute <c>family</c>
    /// that evaluates to <cref name="Family" />.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    class HighlightXmlFamilyRemover : EditFileManager
    {
        /// <summary>
        /// Gets the family used to identify
        /// the class names.
        /// </summary>
        /// <value>The family.</value>
        public string Family { get; private set; }

        /// <summary>
        /// Initializes a new instance of 
        /// the <see cref="HighlightXmlFamilyAdder"/> class.
        /// </summary>
        /// <param name="path">The path of the file to edit.</param>
        /// <param name="family">The family used to identify
        /// the class names.</param>
        public HighlightXmlFamilyRemover(
            string path,
            string family) : base(path)
        {
            this.Family = family;
        }

        /// <inheritdoc/>
        protected override void OnCommit()
        {
            var document = new XmlDocument();

            document.Load(this.ManagedFileStream);
            XmlNode highlightNode = document.DocumentElement;

            #region KEYWORD LIST

            XmlNode keywordListsNode =
            highlightNode.SelectSingleNode("keywordlists");
            XmlNode targetKeywordListNode =
                keywordListsNode.SelectSingleNode(
                    "keywordlist[@id='" + this.Family + "']");
            if (targetKeywordListNode != null)
            {
                targetKeywordListNode.RemoveAll();
                keywordListsNode.RemoveChild(targetKeywordListNode);
            }

            #endregion

            #region LANGUAGES

            XmlNode languagesNode =
            highlightNode.SelectSingleNode("//languages");

            foreach (var language in HighlightingTools.SupportedLanguages.Get())
            {
                XmlNode targetLanguageNode =
                    languagesNode.SelectSingleNode(
                        "language[@id='" + language + "']");

                XmlNode contextsNode =
                    targetLanguageNode.SelectSingleNode("contexts");

                XmlNode contextNode =
                    contextsNode.SelectSingleNode("context[@id='code']");

                XmlNode keywordNode =
                contextNode.SelectSingleNode(
                    "keyword[@family='" + this.Family + "']");

                if (keywordNode != null)
                {
                    keywordNode.RemoveAll();
                    contextNode.RemoveChild(keywordNode);
                }
            }

            #endregion

            this.ManagedFileStream.SetLength(0);
            document.Save(this.ManagedFileStream);
        }
    }

}
