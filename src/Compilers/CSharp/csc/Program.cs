// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.BuildTasks;
using static Microsoft.CodeAnalysis.CompilerServer.BuildProtocolConstants;

namespace Microsoft.CodeAnalysis.CSharp.CommandLine
{
    public class Program
    {
        public static int Main(string[] args)
            => BuildClient.RunWithConsoleOutput(
                args,
#if (!ON_PROJECTN)
                clientDir: AppDomain.CurrentDomain.BaseDirectory,
#else
                clientDir: null,
#endif
                workingDir: Directory.GetCurrentDirectory(),
#if (!ON_PROJECTN)
                sdkDir: RuntimeEnvironment.GetRuntimeDirectory(),
#else
#if (ON_PROJECTN32)
                sdkDir: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",
#else
                sdkDir: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
#endif
#endif
                analyzerLoader: new SimpleAnalyzerAssemblyLoader(),
                language: RequestLanguage.CSharpCompile,
                fallbackCompiler: Csc.Run);
    }
}
