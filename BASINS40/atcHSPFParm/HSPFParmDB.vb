Option Strict Off
Option Explicit On

Imports System.Data
Imports System.Collections.ObjectModel
Imports MapWinUtility

'Copyright 2012 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Class HSPFParmDB
    Public Name As String = ""
    <CLSCompliant(False)> Public Database As atcUtility.atcMDB

    Public Sub Open(ByVal aFilename As String)
        Name = aFilename
        If Name.Length = 0 Then
            Name = "HSPFParmV2.mdb"
        End If
        If Not IO.File.Exists(Name) Then
            Name = GetSetting("HSPF", "ParameterMDB", "Path")
            If Not IO.File.Exists(Name) Then
                Name = atcUtility.FindFile("Please locate 'HSPFParmV2.mdb' in a writable directory", "HSPFParmV2.mdb")
                SaveSetting("HSPF", "ParameterMDB", "Path", Name)
            End If
        End If

    End Sub

    Public Sub New(ByVal aFilename As String)
        Open(aFilename)
    End Sub
End Class
