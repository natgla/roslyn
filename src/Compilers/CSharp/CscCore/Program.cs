// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.CodeAnalysis.CSharp.CommandLine
{
    public class Program
    {
        public static int Main(string[] args)
            => Csc.Run(args: args,
#if (!ON_PROJECTN)
                       clientDirectory: AppContext.BaseDirectory,
#else
                       clientDirectory: null,
#endif
                       sdkDirectory: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",   // TODO: do we need Framework64?
                       analyzerLoader: new NoOpAnalyzerAssemblyLoader());
    }
}
