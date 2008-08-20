Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Collections.ObjectModel

'***************IMPORTANT NOTES:***************
'Check definition of pCtl 
'Check starter path - hardcoded for now!!!
'**********************************************


Public Class frmAgPrac

    Dim pUci As HspfUci
    'in line below, may need to change to ctlEditSpecialAction
    Dim pCtl As ctlEditSpecialAction

    Dim cPracIDs As New Collection
    Dim cPracRecs As New Collection

    Private Sub Form_Load()

        Dim vOper As Object
        Dim lOper As HspfOperation
        Dim i, pcount As Integer
        Dim ctmp, tname, lstr, ilen, tstr As String
        Dim cend As Boolean

        GroupLayers.Visible = True
        GroupPar.Visible = True
        GroupProps.Visible = True
        Height = 630

        comboRep.Items.Clear()
        comboRep.Items.Add("None")
        comboRep.Items.Add("YR")
        comboRep.Items.Add("MO")
        comboRep.Items.Add("DY")
        comboRep.Items.Add("HR")
        comboRep.Items.Add("MI")
        'read database
        'i = FreeFile()
        ''On Error GoTo ErrHandler
        'tname = "C:\Basins\models\HSPF\bin\starter" & "\" & "agpractice.txt"
        'FileOpen(i, tname, OpenMode.Input)
        'pcount = 0

        'Do Until EOF(i)
        '    lstr = LineInput(i)
        '    ilen = Len(lstr)
        '    If ilen > 6 Then
        '        If Microsoft.VisualBasic.Left(lstr, 7) = "PRACTIC" Then
        '            'found start of a practice
        '            ctmp = StrRetRem(lstr)
        '            lstPrac.Items.Add(lstr)
        '            pcount = pcount + 1
        '            cend = False
        '            Do While Not cend
        '                lstr = LineInput(i)
        '                tstr = Trim(lstr)
        '                ilen = Len(tstr)
        '                If Microsoft.VisualBasic.Left(tstr, ilen) = "END PRACTICE" Then
        '                    'found end of practice
        '                    cend = True
        '                Else
        '                    cPracIDs.Add(pcount)
        '                    cPracRecs.Add(lstr)
        '                End If
        '            Loop
        '        End If
        '    End If
        'Loop
        'Close()
        'formerly goto FillLists
        'For Each vOper In pUci.OpnSeqBlock.Opns
        '    lOper = vOper
        '    If lOper.Name = "PERLND" Then
        '        lstSeg.Items.Add(lOper.Name & " " & lOper.Id & " (" & lOper.Description & ")")
        '    End If
        'Next

        'atxStart(0).HardMax = pUci.GlobalBlock.EDate(0)
        'atxStart(0).HardMin = pUci.GlobalBlock.SDate(0)
        'ErrHandler:
        '        If Err.Number = 53 Then
        '            MsgBox("File " & tname & " not found.", vbOKOnly, "Read Ag Practices Problem")
        '        Else
        '            MsgBox(Err.Description & vbCrLf & vbCrLf & lstr, _
        '              vbOKOnly, "Read Ag Practices Problem")
        'End If

    End Sub

    Private Sub frmAgPrac_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form_Load()
    End Sub

    Friend Sub Init(ByVal aUci As HspfUci, ByVal aCtl As ctlEditSpecialAction)
        pUci = aUci
        pCtl = aCtl
        Me.Icon = aCtl.ParentForm.Icon
    End Sub

End Class