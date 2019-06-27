// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Novacta.Documentation.ShfbTools.Tests
{
    [DeploymentItem(@"testshfbroot\")]
    [TestClass]
    public class HighlightingToolsTests
    {
        static readonly string root = @".\";

        [TestMethod]
        public void TestSetClassNameColor()
        {
            string color = "MediumAquaMarine";

            int exitCode = HighlightingTools.SetClassNamesColor(
                color: color,
                path: root);

            Assert.AreEqual(expected: 0, actual: exitCode);
        }

        [TestMethod]
        public void TestAddClassNamesFamily()
        {
            string family = "testFamily";
            List<string> names = new List<string>() {
                "testName0",
                "testName1"
            };
            List<string> languages = new List<string>() {
                "cs",
                "vbnet"
            };

            int exitCode = HighlightingTools.AddClassNamesFamily(
                family: family,
                names: names,
                languages: languages,
                path: root);

            Assert.AreEqual(expected: 0, actual: exitCode);
        }

        [TestMethod]
        public void TestRemoveClassNamesFamily()
        {
            string family = "testFamily";
            List<string> names = new List<string>() {
                "testName0",
                "testName1"
            };
            List<string> languages = new List<string>() {
                "cs",
                "vbnet"
            };

            int exitCode = HighlightingTools.AddClassNamesFamily(
                family: family,
                names: names,
                languages: languages,
                path: root);

            Assert.AreEqual(expected: 0, actual: exitCode);

            exitCode = HighlightingTools.RemoveClassNamesFamily(
                family: family,
                path: root);

            Assert.AreEqual(expected: 0, actual: exitCode);
        }
    }
}
