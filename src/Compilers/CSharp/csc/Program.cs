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

#if (ON_PROJECTN)
            //Can't determine the client directory in ProjectN, so default to the sdkDir
            #if (ON_PROJECTN32)
                clientDir: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",
                sdkDir: @"C:\Windows\Microsoft.NET\Framework\v4.0.30319",
            #else
                clientDir: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
                sdkDir: @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
            #endif
#else
                sdkDir: RuntimeEnvironment.GetRuntimeDirectory(),
                clientDir: AppDomain.CurrentDomain.BaseDirectory,
#endif
                workingDir: Directory.GetCurrentDirectory(),

                analyzerLoader: new SimpleAnalyzerAssemblyLoader(),
                language: RequestLanguage.CSharpCompile,
                fallbackCompiler: Csc.Run);
    }
}
