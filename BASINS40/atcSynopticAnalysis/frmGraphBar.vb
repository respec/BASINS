Imports atcUtility
Imports MapWinUtility

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms

Public Class frmGraphBar

    Private Shared SaveImageExtension As String = ".png"

    Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        SaveBitmapToFile()
    End Sub

    Public Sub SaveBitmapToFile(Optional ByVal aFileName As String = "")
        If aFileName.Length = 0 Then 'No file name specified - ask user
            Dim lSavedAs As String
            lSavedAs = zgc.SaveAs(SaveImageExtension)
            If lSavedAs.Length > 0 Then
                SaveImageExtension = System.IO.Path.GetExtension(lSavedAs)
            End If
        Else
            Dim lFormat As ImageFormat
            Select Case FileExt(aFileName).ToLower
                Case "bmp" : lFormat = ImageFormat.Bmp
                Case "png" : lFormat = ImageFormat.Png
                Case "gif" : lFormat = ImageFormat.Gif
                Case "jpg", _
                    "jpeg" : lFormat = ImageFormat.Jpeg
                Case "tif", _
                    "tiff" : lFormat = ImageFormat.Tiff
                Case Else : lFormat = ImageFormat.Png
            End Select

            MkDirPath(PathNameOnly(aFileName))
            Dim lStream As New StreamWriter(aFileName)
            zgc.MasterPane.GetImage.Save(lStream.BaseStream, lFormat)
            lStream.Close()
        End If
    End Sub

    Private Sub mnuFilePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click
        Dim printdlg As New PrintDialog
        Dim printdoc As New Printing.PrintDocument
        AddHandler printdoc.PrintPage, AddressOf Me.PrintPage

        printdlg.Document = printdoc
        printdlg.AllowSelection = False
        printdlg.ShowHelp = True

        ' If the result is OK then print the document.
        If (printdlg.ShowDialog = Windows.Forms.DialogResult.OK) Then
            Dim saveRect As RectangleF = Pane.Rect
            printdoc.Print()
            ' Restore graph size to fit form's bounds. 
            Pane.ReSize(Me.CreateGraphics, saveRect)
        End If
    End Sub

    '' <summary> Prints the displayed graph. </summary> 
    '' <param name="sender"> Object raising this event. </param> 
    '' <param name="e"> Event arguments passing graphics context to print to. </param> 
    Private Sub PrintPage(ByVal sender As System.Object, ByVal e As Printing.PrintPageEventArgs)
        ' Validate. 
        If (e Is Nothing) Then Return
        If (e.Graphics Is Nothing) Then Return

        ' Resize the graph to fit the printout. 
        With e.MarginBounds
            Pane.ReSize(e.Graphics, New RectangleF(.X, .Y, .Width, .Height))
        End With

        ' Print the graph. 
        Pane.Draw(e.Graphics)

        e.HasMorePages = False 'ends the print job
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(zgc.MasterPane.GetImage)
    End Sub

    <CLSCompliant(False)> _
    Public ReadOnly Property Pane() As ZedGraph.GraphPane
        Get
            Return zgc.MasterPane.PaneList.Item(0)
        End Get
    End Property

End Class