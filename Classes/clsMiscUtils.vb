'********************************************************************************************************
'File Name: clsMiscUtils.vb
'Description: Miscellaneous utilities which have no appropriate home in another class.
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
'
'*******************************************************************************************************

Imports System.Drawing
Imports System.Xml

Public Class MiscUtils
    Private Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As POINTAPI) As Integer

    Public Const HWND_TOPMOST As Integer = -1
    Public Const SWP_NOMOVE As Integer = &H2
    Public Const SWP_NOSIZE As Integer = &H1
    Public Const GWL_HWNDPARENT As Integer = (-8)

    Public Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    Public Shared Function GetCursorLocation() As Point
        Dim pnt As POINTAPI
        GetCursorPos(pnt)
        Return New Point(pnt.x, pnt.y)
    End Function

    Public Shared Function CheckInternetConnection(ByVal CheckAgainstURL As String, Optional ByVal TimeoutMilliseconds As Integer = 2000) As Boolean
        'CDM Aug 2 2006 Moved to NET class -- keeping here for backward compatibility with
        'things which are presently using the function in this class
        Return Net.checkinternetconnection(CheckAgainstURL, TimeoutMilliseconds)
    End Function

    Public Shared Function GetBaseName(ByVal Filename As String) As String
        Dim i As Integer
        Dim tStr As String = ""

        Filename.Replace("/", "\")

        'handle ESRI Grids correctly
        If Filename.Substring(Filename.LastIndexOf("\") + 1).Trim.ToLower = "sta.adf" Then
            tStr = Filename.Substring(0, Filename.LastIndexOf("\"))
            tStr = tStr.Substring(tStr.LastIndexOf("\") + 1)
            Return tStr
        End If

        For i = Len(Filename) To 1 Step -1
            If Mid(Filename, i, 1) = "\" Then
                tStr = Mid(Filename, i + 1, Len(Filename) - 1)
                Exit For
            End If
        Next
        If Len(tStr) = 0 Then
            tStr = Filename
        End If
        For i = Len(tStr) To 1 Step -1
            If Mid(tStr, i, 1) = "." Then
                tStr = Mid(tStr, 1, i - 1)
                Return tStr
            End If
        Next
        Return tStr
    End Function

    Public Shared Function GetExtensionName(ByVal Filename As String) As String
        Dim i As Integer
        Dim tStr As String
        tStr = Filename
        For i = Len(tStr) To 1 Step -1
            If Mid(tStr, i, 1) = "." Then
                tStr = Mid(tStr, i + 1, Len(tStr))
                Exit For
            End If
        Next
        Return tStr
    End Function

    'Verbose out put of process information, CPU info, memory info,
    'environment variables, etc.
    Public Shared Function GetDebugInfo() As String
        Dim retStringBuf As New System.Text.StringBuilder

        Try
            retStringBuf.AppendLine("MapWinUtility (debug reporter) Assembly Version: " + Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString() + "." + Environment.Version.Revision.ToString() + "." + Environment.Version.Build.ToString())
            retStringBuf.AppendLine("Operating System: " + Environment.OSVersion.Platform.ToString())
            retStringBuf.AppendLine("Service Pack: " + Environment.OSVersion.ServicePack())
            retStringBuf.AppendLine("Major Version:	" + Environment.OSVersion.Version.Major.ToString())
            retStringBuf.AppendLine("Minor Version:	" + Environment.OSVersion.Version.Minor.ToString())
            retStringBuf.AppendLine("Revision:		" + Environment.OSVersion.Version.MajorRevision.ToString())
            retStringBuf.AppendLine("Build:		" + Environment.OSVersion.Version.Build.ToString())
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("-------------------------------------------------")
            retStringBuf.Append("Logical Drives: ")
            For Each s As String In Environment.GetLogicalDrives()
                retStringBuf.Append(s & " ")
            Next
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("System Directory: " + Environment.SystemDirectory)
            retStringBuf.AppendLine("Current Directory: " + Environment.CurrentDirectory)
            retStringBuf.AppendLine("Command Line: " + Environment.CommandLine)
            retStringBuf.Append("Command Line Args: ")
            For Each s As String In Environment.GetCommandLineArgs
                retStringBuf.Append(s & " ")
            Next
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("------------Environment Variables-----------------")
            Dim iEnum As IDictionaryEnumerator = Environment.GetEnvironmentVariables.GetEnumerator()
            While iEnum.MoveNext()
                Dim entry As DictionaryEntry = iEnum.Entry
                retStringBuf.AppendLine(entry.Key & " == " & entry.Value)
            End While
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("------------Performance Info (Bytes)--------------")

            Dim currentProc As Process = Process.GetCurrentProcess()
            currentProc.Refresh()

            retStringBuf.Append("Private Memory:  ").AppendLine(currentProc.PrivateMemorySize64.ToString())
            retStringBuf.Append("Virtual Memory:  ").AppendLine(currentProc.VirtualMemorySize64.ToString())
            retStringBuf.Append("Total CPU time: ").AppendLine(currentProc.TotalProcessorTime.ToString())
            retStringBuf.Append("Total User Mode CPU time: ").AppendLine(currentProc.UserProcessorTime.ToString())
            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("------------Module Info:--------------------------")

            Dim myProcessModuleCollection As ProcessModuleCollection = currentProc.Modules
            Dim myProcessModule As ProcessModule
            For Each myProcessModule In myProcessModuleCollection
                Try
                    retStringBuf.Append("----Module Name:  ").AppendLine(myProcessModule.ModuleName)
                    retStringBuf.Append("    Path:  ").AppendLine(myProcessModule.FileName)
                    If myProcessModule.FileVersionInfo.FileVersion IsNot Nothing Then
                        retStringBuf.Append("    Version: ").AppendLine(myProcessModule.FileVersionInfo.FileVersion.ToString())
                    End If
                Catch
                End Try
            Next myProcessModule

            retStringBuf.Append(Environment.NewLine)
            retStringBuf.AppendLine("------------End Debug Info------------------------")
        Catch
        End Try

        Return retStringBuf.ToString()
    End Function

End Class
