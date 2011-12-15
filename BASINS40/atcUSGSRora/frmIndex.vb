Imports System.Text.RegularExpressions
Public Class frmIndex
    Public Event NewK(ByVal aK As Double)

    Public Sub New(ByVal aFileKname As String)
        InitializeComponent()

        If System.IO.File.Exists(aFileKname) Then
            lstIndices.Items.Clear()
            Dim lSR As New System.IO.StreamReader(aFileKname)
            Dim lOneLine As String = ""
            Dim lArr() As String = Nothing
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine().TrimEnd(vbCr).TrimEnd(vbLf)
                lArr = Regex.Split(lOneLine, "\s+")
                If lArr.Length = 2 AndAlso Not IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                    lstIndices.Items.Add(loneLine)
                End If
            End While
            lSR.Close()
            lSR = Nothing
        End If
    End Sub

    Private Sub lstIndices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstIndices.SelectedIndexChanged
        Dim lKey As String = lstIndices.SelectedItem
        Dim lArr() As String = Regex.Split(lKey, "\s+")
        If lArr.Length = 2 AndAlso Not IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
            RaiseEvent NewK(Double.Parse(lArr(1)))
        End If
    End Sub
End Class