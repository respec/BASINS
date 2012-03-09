Option Strict Off
Option Explicit On

Imports System.Data
Imports System.Collections.ObjectModel
Imports MapWinUtility

'Copyright 2012 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Class HSPFParmDB
    Public Name As String = ""
    <CLSCompliant(False)> Public Database As atcUCI.atcMDB

    Public Sub Open(ByVal aFilename As String)
        Name = aFilename
        If Name.Length = 0 Then
            Name = "HSPFParm2003.mdb"
        End If
        If Not IO.File.Exists(Name) Then
            Name = GetSetting("HSPF", "ParameterMDB", "Path")
            If Not IO.File.Exists(Name) Then
                Name = atcUtility.FindFile("Please locate 'HSPFParm2003.mdb' in a writable directory", "HSPFParm2003.mdb")
                SaveSetting("HSPF", "ParameterMDB", "Path", Name)
            End If
        End If

        'Database = New atcUCI.atcMDB(Name)

        'Dim lTable As DataTable = Database.GetTable("ParmData")
        'Debug.Print("R:" & lTable.Rows.Count & " C:" & lTable.Columns.Count)
        'lTable = Database.GetTable("WatershedData")
        'Debug.Print("R:" & lTable.Rows.Count & " C:" & lTable.Columns.Count)
        'For lRow As Integer = 0 To lTable.Rows.Count - 1
        '    Debug.Print("R:" & lRow & " " & lTable.Rows(lRow).Item(1).ToString)
        'Next
    End Sub

    Public Sub New(ByVal aFilename As String)
        Open(aFilename)
    End Sub
End Class
