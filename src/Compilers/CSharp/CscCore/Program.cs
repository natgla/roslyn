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
#if (ON_PROJECTN)
#if (ON_PROJECTN32)
                       clientDirectory: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
                       sdkDirectory: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",
#else
                       clientDirectory: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
                       sdkDirectory: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
#endif
#else
                       clientDirectory: AppContext.BaseDirectory,
                       sdkDirectory: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",
#endif
                       analyzerLoader: new NoOpAnalyzerAssemblyLoader());
    }
}
