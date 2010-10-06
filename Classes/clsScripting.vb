'********************************************************************************************************
'File Name: clsScripting.vb
'Description: A class providing scripting capability to MapWindow and plugins.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'09/02/2010 - KW - Modified class to work with MW4 and MW6/DotSpatial
'********************************************************************************************************

Imports System.CodeDom.Compiler
Imports System.Reflection

Public Class Scripting

    ''' <summary>
    ''' Unicode prefix sometimes gets included in a string, we have to remove it before interpreting as a filename or script
    ''' </summary>
    Private Shared ByteOrderMarker As String = Chr(239) & Chr(187) & Chr(191)
    Private Shared Sub RemoveByteOrderMarker(ByRef aString As String)
        If aString IsNot Nothing AndAlso aString.StartsWith(ByteOrderMarker) Then
            aString = aString.Substring(ByteOrderMarker.Length)
        End If
    End Sub


    'Broke out the run method into PrepareScript and Run
    'No longer have to pass in a reference to the Mainform
    'PrepareScript now returns an assembly.  Assembly can be
    'loaded by the appropriate Plugin manager from the calling code
    Public Shared Function PrepareScript(ByVal aLanguage As String, _
                        ByVal aDLLfilename As String, _
                        ByVal aCode As String, _
                        ByRef aErrors As String, _
                        ByVal aPluginFolder As String) As Assembly


        aErrors = "" 'No errors yet
        aLanguage = GetLanguageFromFilename(aLanguage)
        Dim assy As System.Reflection.Assembly



        If aDLLfilename Is Nothing OrElse aDLLfilename.Length = 0 Then
            aDLLfilename = MakeScriptName(aPluginFolder)
        End If

        RemoveByteOrderMarker(aCode)

        If aLanguage = "dll" Then
            assy = System.Reflection.Assembly.LoadFrom(aCode)
        Else 'compile the code into an assembly
            Try
                If System.IO.File.Exists(aCode) Then
                    aCode = Strings.WholeFileString(aCode)
                Else
                    Dim lCodeFile As String = PathNameOnly(aPluginFolder) & "\scripts\" & aCode
                    If System.IO.File.Exists(lCodeFile) Then
                        aCode = Strings.WholeFileString(lCodeFile)
                    End If
                End If
            Catch ex As Exception
                'Treat as code, not as file name
            End Try

            RemoveByteOrderMarker(aCode)
            assy = Compile(aLanguage, aCode, aErrors, aDLLfilename)

        End If

        Return assy


    End Function


    Public Shared Function Run(ByVal aAssembly As Assembly, _
                    ByRef aErrors As String, _
                    ByVal ParamArray aArgs() As Object) As Object

        Dim assy As Assembly
        assy = aAssembly

        Dim MethodName As String = "ScriptMain" 'Can't be MAIN or the C# compiler will have a heart attack.

        Dim assyTypes As Type() 'list of items within the assembly

        If aErrors Is Nothing OrElse aErrors.Length = 0 Then

            assyTypes = assy.GetTypes()
            For Each typ As Type In assyTypes
                Dim scriptMethodInfo As MethodInfo = typ.GetMethod(MethodName)
                If Not scriptMethodInfo Is Nothing Then
                    Logger.Dbg("Invoke:" & scriptMethodInfo.Name)
                    If aArgs Is Nothing Then
                        Logger.Dbg("No Args")
                    Else
                        For Each lArg As Object In aArgs
                            If Not (lArg Is Nothing) Then
                                Logger.Dbg("Arg:" & lArg.GetType.ToString & ":<" & lArg.ToString & ">")
                            End If
                        Next
                    End If
                    Try
                        Return scriptMethodInfo.Invoke(Nothing, aArgs) 'assy.CreateInstance(typ.Name)
                    Catch ex As Exception
                        Logger.Dbg("Exception:" & ex.ToString)
                        Return False
                    End Try
                    If Not scriptMethodInfo Is Nothing Then
                        Return scriptMethodInfo.Invoke(Nothing, aArgs) 'assy.CreateInstance(typ.Name)
                    End If
                End If
            Next
            aErrors = "Scripting.Run: '" & MethodName & "' not found"

        End If
        Return Nothing
    End Function

    Public Shared Function MakeScriptName(ByVal aPluginFolder As String) As String
        Dim lTryName As String
        Dim lTryCount As Integer = 1
        Dim lScriptName As String = "RemoveMe-Script-"

        Do
            lTryName = aPluginFolder & "\" & _
                      lScriptName & lTryCount & ".dll"
            lTryCount += 1
        Loop While System.IO.File.Exists(lTryName)
        Return lTryName
    End Function

    'language = "vb" or "cs" or "js"
    Public Shared Function Compile(ByVal aLanguage As String, _
                                   ByVal aCode As String, _
                                   ByRef aErrors As String, _
                          Optional ByVal aOutputFilename As String = "") As System.Reflection.Assembly
        Dim params As CompilerParameters
        Dim results As CompilerResults
        Dim provider As CodeDomProvider
        Dim needSupportCode As Boolean = False
        Dim lSupportCode As String = ""

        aLanguage = GetLanguageFromFilename(aLanguage)

        Select Case aLanguage
            Case "cs"
                'provider = New Microsoft.CSharp.CSharpCodeProvider
                'Paul Meems - 2010/09/13: Use FrameWork v3.5 so we can use the C#3.0 options:
                Dim provOptions As New System.Collections.Generic.Dictionary(Of String, String)()
                provOptions.Add("CompilerVersion", "v3.5")
                provider = New Microsoft.CSharp.CSharpCodeProvider(provOptions)

                If aCode.IndexOf("using ") < 0 Then needSupportCode = True
                'Case "js" : provider = Activator.CreateInstance("Microsoft.JScript", "Microsoft.JScript.JScriptCodeProvider").Unwrap()
            Case "vb"
                provider = New Microsoft.VisualBasic.VBCodeProvider
                If aCode.IndexOf("Public ") < 0 Then needSupportCode = True
            Case Else : provider = New Microsoft.VisualBasic.VBCodeProvider
        End Select

        params = New System.CodeDom.Compiler.CompilerParameters

        'jlk&mg - 2010/09 - explicitly load assemblies that scripts are likely to need
        Dim lAssemblyNames() As String = {"MapWinGeoProc", "MapWinUtility", "MapWinInterfaces", "Zedgraph", _
                                            "DotSpatial.Common", "DotSpatial.Data", "DotSpatial.Desktop", _
                                            "DotSpatial.Projections", "DotSpatial.Topology"}

        For Each lAssemblyName As String In lAssemblyNames
            Dim lAssemblyFileName As String = IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), lAssemblyName & ".dll")
            If IO.File.Exists(lAssemblyFileName) Then
                params.ReferencedAssemblies.Add(lAssemblyFileName)
            End If
        Next

        If aOutputFilename.Length = 0 Then
            params.GenerateInMemory = True      'Assembly is created in memory
            params.GenerateExecutable = False
        Else
            params.OutputAssembly = aOutputFilename
        End If
        params.TreatWarningsAsErrors = False
        params.WarningLevel = 4
        'params.ReferencedAssemblies.AddRange(refs)

        For Each refAssy As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies()

            Dim lAssyName As String = Strings.StrSplit(refAssy.FullName, ",", "")
            Select Case lAssyName
                Case "mscorlib", _
                     "mwIdentifier", _
                     "TableEditor.mw", _
                     "MapWindow.resources", _
                     "MapWinGeoProc", _
                     "MapWinUtility", _
                     "ZedGraph", _
                     "MapWinInterfaces", _
                     "log4net", _
                     "System.Windows.Forms.Ribbon35"

                Case Else
                    'Chris M July 2006 -- Don't add 'RemoveMe' or resources with "" as the location.
                    If Not lAssyName.Contains("MapWindow") And Not lAssyName.Contains("RemoveMe") Then
                        'Chris M Jan 1 2006 -- don't add localized satellite assemblies
                        If Not lAssyName.EndsWith(".resources") Then
                            If lAssyName.StartsWith("RemoveMe") Then
                                'Don't add temporary assemblies
                            ElseIf Not refAssy.Location.Trim() = "" Then
                                If needSupportCode Then
                                    Select Case aLanguage
                                        Case "vb" : lSupportCode &= "Imports " & lAssyName & vbCrLf
                                        Case "cs" : lSupportCode &= "using " & lAssyName & ";" & vbCrLf
                                    End Select
                                End If
                                params.ReferencedAssemblies.Add(refAssy.Location)
                            End If
                        End If
                    End If
            End Select
        Next

        If aLanguage.ToLower = "vb" And needSupportCode Then
            aCode = lSupportCode & vbCrLf _
                                 & "Public Module ScriptModule" & vbCrLf _
                                 & "  Public Sub ScriptMain(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)" & vbCrLf _
                                 & aCode & vbCrLf _
                                 & "  End Sub" & vbCrLf _
                                 & "End Module"
        End If

        'MsgBox(aCode)

        Try
            results = provider.CompileAssemblyFromSource(params, aCode)
            If results.Errors.Count = 0 Then        'No compile errors or warnings
                Return results.CompiledAssembly
            Else
                For Each err As CompilerError In results.Errors
                    aErrors &= (String.Format( _
                        "Line {0}, Col {1}: Error {2} - {3}", _
                        err.Line, err.Column, err.ErrorNumber, err.ErrorText)) & vbCrLf
                Next
            End If
        Catch ex As Exception
            'Compile errors don't throw exceptions. This is a deeper problem
            aErrors = ex.Message
        End Try

        If Not aErrors Is Nothing AndAlso aErrors.Length > 0 Then
            Logger.Dbg(aErrors)
        End If

        Return Nothing

    End Function

    Public Shared Function GetLanguageFromFilename(ByVal aFilename As String) As String
        'Paul Meems 3 aug 2009: Added, because somehow .cs wasn't changed in cs:
        aFilename = aFilename.Trim(".")

        If aFilename.StartsWith(".") Then
            aFilename.Remove(0, 1)
        End If
        Return aFilename.ToLower()
    End Function
End Class
