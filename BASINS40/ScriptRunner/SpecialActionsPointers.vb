Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Imports System.IO
Imports System.Text

Public Module SpecialActionsPointers
    Private pTestPath As String = "D:\lib3.0\MkCom\PERLND\v12.3"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pTestPath)
        Dim lName As String = pTestPath & "\newspecpointers2.txt"

        Dim lCurrentRecord As String
        Dim lStreamReader As New StreamReader(lName)
        Dim lCnt As Integer = 0
        Dim lOutputString As String = ""
        Do
            lCurrentRecord = lStreamReader.ReadLine
            lCnt += 1
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                If lCnt < 4 Then
                    lOutputString = lOutputString & " " & lCurrentRecord
                Else
                    lOutputString = lOutputString & " " & vbCrLf & lCurrentRecord
                    lCnt = 1
                End If

            End If
        Loop

        SaveFileString(pTestPath & "\newspecpointers3.txt", lOutputString)
    End Sub

End Module
