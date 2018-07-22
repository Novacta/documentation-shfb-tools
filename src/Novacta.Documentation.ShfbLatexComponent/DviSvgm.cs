// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System.IO;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Represents a DviSvgm process.
    /// </summary>
    /// <remarks>
    /// <see cref="DviSvgm"/> instances convert DVI files 
    /// into SVG files.
    /// </remarks>
    public class DviSvgm : FileProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DviSvgm"/> class.
        /// </summary>
        /// <param name="dvisvgmBinPath">The DviSvgm bin path.</param>
        /// <param name="workingPath">The working path.</param>
        /// <param name="defaultZoomFactor">The default zoom factor.</param>
        public DviSvgm(string dvisvgmBinPath, string workingPath, string defaultZoomFactor)
        {
            this.exe = dvisvgmBinPath + Path.DirectorySeparatorChar + "dvisvgm.exe";
            this.workingPath = workingPath;
            this.defaultZoomFactor = defaultZoomFactor;
        }

        private readonly string workingPath;
        private readonly string exe;
        private readonly string defaultZoomFactor;

        /// <inheritdoc />
        public override string WorkingDirectory { get { return this.workingPath; } }

        /// <inheritdoc />
        public override string Executable { get { return this.exe; } }

        /// <inheritdoc />
        public override string Arguments(string fileName, string additionalInfo)
        {
            string zoomFactor = this.defaultZoomFactor; 
            if (!object.ReferenceEquals(null, additionalInfo)) {
                zoomFactor = additionalInfo;
            }

            var arguments = @" --no-fonts --exact --zoom=" + zoomFactor + " \"" +
                fileName + "\"";

            return arguments;
        }
    }
}
