' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.Diagnostics

Namespace Microsoft.CodeAnalysis.Editor.UnitTests.Diagnostics.GenerateEvent
    Public Class GenerateEventCrossLanguageTests
        Inherits AbstractCrossLanguageUserDiagnosticTest

        Friend Overrides Function CreateDiagnosticProviderAndFixer(workspace As Workspace, language As String) As Tuple(Of DiagnosticAnalyzer, CodeFixProvider)
            Return Tuple.Create(Of DiagnosticAnalyzer, CodeFixProvider)(
                Nothing,
                New Microsoft.CodeAnalysis.VisualBasic.CodeFixes.GenerateEvent.GenerateEventCodeFixProvider())
        End Function

        <Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateEvent)>
        Public Sub TestGenerateEventInCSharpFileFromImplementsWithParameterList()
            Dim input =
        <Workspace>
            <Project Language="Visual Basic" AssemblyName="VBAssembly1" CommonReferences="true">
                <ProjectReference>CSAssembly1</ProjectReference>
                <Document>
Class c
    Implements i
    Event a(x As Integer) Implements $$i.foo
End Class
                </Document>
            </Project>
            <Project Language="C#" AssemblyName="CSAssembly1" CommonReferences="true">
                <Document FilePath=<%= DestinationDocument %>>
public interface i
{
}
                </Document>
            </Project>
        </Workspace>

            Dim expected =
                <text>public interface i
{
    event fooEventHandler foo;
}

public delegate void fooEventHandler(int x);
                </text>.Value.Trim()

            Test(input, expected)
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateEvent)>
        Public Sub TestGenerateEventInCSharpFileFromImplementsWithType()
            Dim input =
        <Workspace>
            <Project Language="Visual Basic" AssemblyName="VBAssembly1" CommonReferences="true">
                <ProjectReference>CSAssembly1</ProjectReference>
                <Document>
Class c
    Implements i
    Event a as EventHandler Implements $$i.foo
End Class
                </Document>
            </Project>
            <Project Language="C#" AssemblyName="CSAssembly1" CommonReferences="true">
                <Document FilePath=<%= DestinationDocument %>>
public interface i
{
}
                </Document>
            </Project>
        </Workspace>

            Dim expected =
                <text>using System;

public interface i
{
    event EventHandler foo;
}</text>.Value.Trim()

            Test(input, expected)
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateEvent)>
        <WorkItem(737021)>
        Public Sub TestGenerateEventInCSharpFileFromHandles()
            Dim input =
        <Workspace>
            <Project Language="Visual Basic" AssemblyName="VBAssembly1" CommonReferences="true">
                <ProjectReference>CSAssembly1</ProjectReference>
                <Document>
Class c
    WithEvents field as Program
    Sub Foo(x as Integer) Handles field.Ev$$
    End Sub
End Class
                </Document>
            </Project>
            <Project Language="C#" AssemblyName="CSAssembly1" CommonReferences="true">
                <Document FilePath=<%= DestinationDocument %>>
public class Program
{
}
                </Document>
            </Project>
        </Workspace>

            Dim expected =
                <text>public class Program
{
    public event EvHandler Ev;
}

public delegate void EvHandler(int x);
                </text>.Value.Trim()

            Test(input, expected)
        End Sub

        <WorkItem(144843, "Generate method stub generates into *.Designer.cs")>
        <Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateEvent)>
        Public Sub PreferNormalFileOverAutoGeneratedFile_Basic()
            Dim input =
        <Workspace>
            <Project Language="Visual Basic" AssemblyName="VBAssembly1" CommonReferences="true">
                <Document>
Class c
    WithEvents field as UserControl1
    Sub Foo(x as Integer) Handles field.Ev$$
    End Sub
End Class
                </Document>
                <Document FilePath="UserControl1.Designer.vb">
' This file is auto-generated
Partial Class UserControl1
End Class
                </Document>
                <Document FilePath="UserControl1.vb">
Partial Public Class UserControl1
End Class
                </Document>
            </Project>
        </Workspace>

            Dim expectedFileWithText =
                 New Dictionary(Of String, String) From {
                    {"UserControl1.vb",
<Text>
Partial Public Class UserControl1
    Public Event Ev(x As Integer)
End Class
</Text>.Value.Trim()},
                    {"UserControl1.Designer.vb",
<Text>
' This file is auto-generated
Partial Class UserControl1
End Class
</Text>.Value.Trim()}
                }

            Test(input, fileNameToExpected:=expectedFileWithText)
        End Sub
    End Class
End Namespace
