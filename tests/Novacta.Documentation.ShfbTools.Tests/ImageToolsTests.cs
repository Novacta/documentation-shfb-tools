// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Novacta.Documentation.ShfbTools.Tests
{
    [DeploymentItem(@"testshfbroot\")]
    [TestClass]
    public class ImageToolsTests
    {
        static readonly string root = @".\";

        [TestMethod]
        public void TestInstall()
        {
            int exitCode = ImageTools.Install(root);

            Assert.AreEqual(expected: 0, actual: exitCode);

            Assert.IsTrue(
                File.Exists(
                    Path.Combine(
                        root, 
                        @"PresentationStyles\Markdown\Transforms\novacta_image_tools.xsl")));
            Assert.IsTrue(
               File.Exists(
                   Path.Combine(
                       root,
                       @"PresentationStyles\OpenXml\Transforms\novacta_image_tools.xsl")));
            Assert.IsTrue(
                File.Exists(
                    Path.Combine(
                        root,
                        @"PresentationStyles\VS2010\Transforms\novacta_image_tools.xsl")));
            Assert.IsTrue(
                File.Exists(
                   Path.Combine(
                       root,
                       @"PresentationStyles\VS2013\Transforms\novacta_image_tools.xsl")));
        }
    }
}
