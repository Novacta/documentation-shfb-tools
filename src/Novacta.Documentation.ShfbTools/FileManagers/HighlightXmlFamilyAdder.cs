// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Novacta.Transactions.IO;
using System.Collections.Generic;
using System.Xml;

namespace Novacta.Documentation.ShfbTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that adds a family of
    /// keywords to file highlight.xml 
    /// when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="HighlightXmlFamilyAdder"/> 
    /// is expected to manage file highlight.xml in the 
    /// Colorizer folder of a SHFB installation
    /// to enable class name highlights.
    /// </para>
    /// <para>
    /// The aim of this manager is twofold.
    /// <list type="bullet">
    /// <item>
    /// It adds a new keyword list, identified by
    /// <see cref="Family"/>, whose items are those
    /// enumerated by <see cref="Names"/>.
    /// For example, if <see cref="Family"/> evaluates
    /// to <c>"my-family"</c>, and <see cref="Names"/>
    /// enumerates <c>"Class0"</c>, <c>"Class1"</c>, and 
    /// <c>"Class2"</c>, then the following code is inserted
    /// under node <c>keywordlists</c>:
    /// <code language="xml">
    /// <![CDATA[
    ///    <keywordlist id = "my-family">
    ///        <kw>Class0</kw>
    ///        <kw>Class1</kw>
    ///        <kw>Class2</kw>
    ///    </keywordlist>
    /// ]]>
    /// </code>
    /// </item>
    /// <item>
    /// It adds a keyword rule for each language in <see cref="Languages"/>.
    /// For example, if <see cref="Languages"/> contains <c>"cs"</c>, then 
    /// under node <c>languages</c> the editor looks for a <c>language</c>
    /// node having attribute <c>id</c> set to <c>"cs"</c>, and updates its
    /// <c>context</c> child as follows.
    /// <code language="xml">
    /// <![CDATA[
    ///	<language id = "cs" tabSize="4" name="C#">
    ///	  <!-- Code contexts: default (most common) is code. -->
    ///		<contexts default="code">
    ///		  <!-- basic source code context -->
    ///		  <context id = "code" attribute="code">
    ///         <!-- BEGIN ADDED RULE -->
    ///			<!-- class names C# -->
    ///			<keyword attribute="class-name"
    ///                  context="code"
    ///                  family="my-family" />
    ///         <!--END ADDED RULE -->
    ///     </context>
    ///   </contexts>
    ///	</language>
    /// ]]>
    /// </code>
    /// Note that, inside the <c>keyword</c> node, 
    /// the <c>family</c> attribute has a value equal 
    /// to the <c>id</c> attribute of the <c>keywordlist</c> node
    /// previously added, while attribute <c>attribute</c> 
    /// has a value equal to the postfix of the CSS class name 
    /// added to file highlight.ccs by <see cref="HighlightCssEditor"/>.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    class HighlightXmlFamilyAdder : EditFileManager
    {
        /// <summary>
        /// Gets the family used to identify
        /// the class names.
        /// </summary>
        /// <value>The family.</value>
        public string Family { get; private set; }

        /// <summary>
        /// Gets the class names to highlight.
        /// </summary>
        /// <value>The class names.</value>
        public IEnumerable<string> Names { get; private set; }

        /// <summary>
        /// Gets the languages for which 
        /// the class names need to be highlighted.
        /// </summary>
        /// <value>The languages.</value>
        public IEnumerable<string> Languages { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightXmlFamilyAdder"/> class.
        /// </summary>
        /// <param name="path">The path of the file to edit.</param>
        /// <param name="family">The family used to identify
        /// the class names.</param>
        /// <param name="names">The class names to highlight.</param>
        /// <param name="languages">The languages for which 
        /// the class names need to be highlighted.
        /// </param>
        public HighlightXmlFamilyAdder(
            string path,
            string family,
            IEnumerable<string> names,
            IEnumerable<string> languages) : base(path)
        {
            this.Family = family;
            this.Names = names;
            this.Languages = languages;
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
            }
            else
            {
                targetKeywordListNode = document.CreateElement("keywordlist");
                keywordListsNode.AppendChild(targetKeywordListNode);
            }
            XmlAttribute idAttribute = document.CreateAttribute("id");
            idAttribute.Value = this.Family;
            targetKeywordListNode.Attributes.Append(idAttribute);

            XmlNode kwNode;
            foreach (var keyword in this.Names)
            {
                kwNode = document.CreateElement("kw");
                kwNode.InnerText = keyword;
                targetKeywordListNode.AppendChild(kwNode);
            }

            #endregion

            #region LANGUAGES

            XmlNode languagesNode =
            highlightNode.SelectSingleNode("//languages");

            foreach (var language in this.Languages)
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
                }
                else
                {
                    keywordNode = document.CreateElement("keyword");
                    contextNode.AppendChild(keywordNode);
                }

                XmlAttribute attributeAttribute =
                    document.CreateAttribute("attribute");
                attributeAttribute.Value = "class-name";
                keywordNode.Attributes.Append(attributeAttribute);

                XmlAttribute contextAttribute =
                    document.CreateAttribute("context");
                contextAttribute.Value = "code";
                keywordNode.Attributes.Append(contextAttribute);

                XmlAttribute familyAttribute =
                    document.CreateAttribute("family");
                familyAttribute.Value = this.Family;
                keywordNode.Attributes.Append(familyAttribute);
            }

            #endregion

            this.ManagedFileStream.SetLength(0);
            document.Save(this.ManagedFileStream);
        }
    }
}
