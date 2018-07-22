﻿// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System.IO;

namespace Novacta.Documentation.ShfbTools
{
    /// <summary>
    /// Represents a LaTeX process.
    /// </summary>
    /// <remarks>
    /// <see cref="Latex"/> instances convert TEX files 
    /// into DVI files.
    /// </remarks>
    public class Latex : FileProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Latex"/> class.
        /// </summary>
        /// <param name="latexBinFolder">The LaTeX bin folder.</param>
        /// <param name="workingFolder">The working folder.</param>
        public Latex(string latexBinFolder, string workingFolder)
        {
            this.exe = latexBinFolder + Path.DirectorySeparatorChar + "latex.exe";
            this.workingFolder = workingFolder;
        }

        private readonly string workingFolder;
        private readonly string exe;

        /// <inheritdoc />
        public override string WorkingDirectory { get { return this.workingFolder; } }

        /// <inheritdoc />
        public override string Executable { get { return this.exe; } }

        /// <inheritdoc />
        public override string Arguments(string fileName, string additionalInfo)
        {
            var arguments = "-quiet -disable-installer -interaction=batchmode -output-directory=" + "\"" +
                this.workingFolder + "\"" + " " + "\"" + this.workingFolder +
                Path.DirectorySeparatorChar + fileName + "\""; ;

            return arguments;
        }
    }
}
