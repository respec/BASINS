Imports MapWindow.Interfaces
Imports System.Windows.Forms.DialogResult
Imports System
Imports atcUtility
Imports System.IO
Imports System.Collections.Specialized
Imports MapWinUtility


Module WriteEXSFile

    Public Sub WriteEXSFileMain(ByRef aMapWin As IMapWin)

        Dim StartUpForm As New HSPEXP.StartUp
        'Dim EXSFileForm As New HSPEXP.MakeEXSFile
        Dim EXSFileName As String = MakeEXSFile.txtEXSFileName.Text
        Dim ucifilename As String = MakeEXSFile.lblUCIFileName.Text
        Dim CurrentDirectory As String = IO.Path.GetDirectoryName(MakeEXSFile.lblUCIFileName.Text)
        Dim EXSFileContents As String = ""

        ucifilename = IO.Path.GetFileNameWithoutExtension(ucifilename)
        If Len(ucifilename) < 8 Then
            EXSFileContents = ucifilename
        Else
            MsgBox("The ucifilename must be less than 8 Ccharacters to make EXS file successfully!")
            End
        End If
        EXSFileContents = ucifilename.PadLeft(8) & MakeEXSFile.cmbNumberOfSites.Text.PadLeft(5) & "     1" & _
            MakeEXSFile.txtLat1.Text.PadLeft(7) & MakeEXSFile.txtLat2.Text.PadLeft(8) & MakeEXSFile.txtLong1.Text.PadLeft(8) & _
            MakeEXSFile.txtLong2.Text.PadLeft(8)
        If MakeEXSFile.chkAnalysisPeriod.Checked = True Then

            EXSFileContents &= "  " & MakeEXSFile.DTStartDate.Value.ToString("yyyyMMdd") & "  " & _
            MakeEXSFile.DTEndDate.Value.ToString("yyyyMMdd") & vbCrLf

        End If
        EXSFileContents &= MakeEXSFile.txtSIMQ.Text.PadLeft(4) & _
                            MakeEXSFile.txtObsFlow.Text.PadLeft(4) & _
                            MakeEXSFile.txtSURO.Text.PadLeft(4) & _
                            MakeEXSFile.txtIFWO.Text.PadLeft(4) & _
                            MakeEXSFile.txtAGWO.Text.PadLeft(4) & _
                            MakeEXSFile.txtSUPY.Text.PadLeft(4) & _
                            MakeEXSFile.txtPET.Text.PadLeft(4) & _
                            MakeEXSFile.txtSAET.Text.PadLeft(4) & _
                            MakeEXSFile.txtLZSX.Text.PadLeft(4) & _
                            MakeEXSFile.txtUZSX.Text.PadLeft(4) & _
                            "  1" & "  RCH" & MakeEXSFile.txtReachNumber.Text & vbCrLf
        EXSFileContents &= MakeEXSFile.txtNumberOfStorms.Text.PadLeft(4) & vbCrLf
        EXSFileContents &= " " & MakeEXSFile.DTStormStart1.Value.ToString("yyyy MM dd") & _
                           "  0  0  0 " & _
                           MakeEXSFile.DTStormEnd1.Value.ToString("yyyy MM dd") & _
                            "  0  0  0 " & vbCrLf
        EXSFileContents &= MakeEXSFile.txtArea1.Text.PadLeft(8) & vbCrLf
        EXSFileContents &= "   10.00    0.03   10.00   15.00   15.00    2.50   20.00   15.00    1.50   30.00"


        SaveFileString(CurrentDirectory & "\" & EXSFileName & ".exs", EXSFileContents)




    End Sub


End Module
