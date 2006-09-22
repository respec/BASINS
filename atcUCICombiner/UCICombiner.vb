Imports atcUtility
Imports System.Collections.Specialized

Public Class UCICombiner
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        CombineUCIs()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Me.Text = "Form1"
    End Sub

#End Region

    Private Function CombineUCIs() As Boolean

        Dim i As Integer
        Dim lUciCnt As Integer
        Dim lOper As atcUCI.HspfOperation
        Dim lTable As atcUCI.HspfTable
        Dim lConn As atcUCI.HspfConnection

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Name = "hspfmsg.mdb"

        'get names of all ucis in dir
        Dim lPerlndUcis As New Collection
        Dim lImplndUcis As New Collection
        Dim lRchresUcis As New Collection
        Dim lUciFullNames As New NameValueCollection
        Dim lUcis As New Collection
        Dim lString As String
        Dim lUciName As String
        AddFilesInDir(lUciFullNames, "C:\cbp_working\output\", False, "*.uci")

        'we could open each uci and look to see what operation it contains,
        'but we know based on the naming convention
        For Each lString In lUciFullNames
            lUciName = FilenameNoPath(lString)
            If Mid(lUciName, 1, 3) = "afo" Or _
               Mid(lUciName, 1, 3) = "imh" Or _
               Mid(lUciName, 1, 3) = "iml" Then
                lImplndUcis.Add(lUciName)
            ElseIf Mid(lUciName, 1, 3) = "riv" Then
                lRchresUcis.Add(lUciName)
            Else
                lPerlndUcis.Add(lUciName)
            End If
        Next lString
        'put them in order
        For i = 1 To lPerlndUcis.Count
            lUcis.Add(lPerlndUcis(i))
        Next i
        For i = 1 To lImplndUcis.Count
            lUcis.Add(lImplndUcis(i))
        Next i
        For i = 1 To lRchresUcis.Count
            lUcis.Add(lRchresUcis(i))
        Next i

        'create a new uci to be the combined uci
        Dim lCombinedUci As New atcUCI.HspfUci

        ChDir("C:\cbp_working\output")
        'read first uci
        lCombinedUci.FastReadUciForStarter(lMsg, lUcis(1))
        lCombinedUci.MetSeg2Source()
        lCombinedUci.Point2Source()

        'make this the combined uci
        lCombinedUci.Name = "combined.uci"
        lCombinedUci.GlobalBlock.RunInf.Value = "Combined UCI for Monocacy"

        Dim lMetSegCounter As Integer = 100
        Dim lLandUseCounter As Integer = 1
        Dim lOrigId As Integer
        Dim lTotalMetSegCount As Integer = 7

        'change this operation number
        lOper = lCombinedUci.OpnBlks("PERLND").ids(1)
        lOrigId = lOper.Id
        lOper.Id = 101
        'remove all the targets from this perlnd
        For Each lConn In lOper.Targets
            lOper.Targets.Remove(1)
        Next lConn
        'renumber data sets to reflect met seg number
        For Each lConn In lOper.Sources
            lConn.Source.VolId = lConn.Source.VolId + lMetSegCounter
        Next lConn
        'change operation number in all special actions
        RenumberOperationInSpecialActions(lCombinedUci, "PERLND", lOrigId, lOper.Id)

        'now start looping through the rest of the ucis
        For lUciCnt = 2 To lUcis.Count
            'read each uci 
            Dim lUci As New atcUCI.HspfUci
            lUci.FastReadUciForStarter(lMsg, lUcis(lUciCnt))
            lUci.MetSeg2Source()
            lUci.Point2Source()

            'add operations of second uci into first uci
            Dim lNewOperId As Integer
            For Each lOper In lUci.OpnSeqBlock.Opns
                If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                    lMetSegCounter = lMetSegCounter + 100
                    If lMetSegCounter > lTotalMetSegCount * 100 Then
                        'this will be a new land use
                        lLandUseCounter = lLandUseCounter + 1
                        lMetSegCounter = 100
                    End If
                    lNewOperId = lMetSegCounter + lLandUseCounter
                Else
                    lNewOperId = 1
                End If
                'make sure this is a unique number
                Do While Not lCombinedUci.OpnBlks(lOper.Name).operfromid(lNewOperId) Is Nothing
                    lNewOperId = lNewOperId + 1
                Loop

                'add this operation
                Dim lOpn As New atcUCI.HspfOperation
                lOpn = lOper
                lOpn.Name = lOper.Name
                lOrigId = lOper.Id
                lOpn.Id = lNewOperId
                lOpn.Uci = lCombinedUci
                lCombinedUci.OpnBlks(lOper.Name).Ids.Add(lOpn, "K" & lOpn.Id)
                lOpn.OpnBlk = lCombinedUci.OpnBlks(lOper.Name)
                lCombinedUci.OpnSeqBlock.Add(lOper)

                'remove the comments so we don't get repeated headers
                For Each lTable In lOpn.Tables
                    lTable.Comment = ""
                Next lTable

                'update ftable number
                If lOper.Name = "RCHRES" Then
                    lOper.FTable.Id = lNewOperId
                    lOper.Tables("HYDR-PARM2").parmvalue("FTBUCI") = lNewOperId
                End If

                If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                    'remove all the targets from perlnds and implnds
                    '(we can pass internally without writing to wdm)
                    For Each lConn In lOpn.Targets
                        lOpn.Targets.Remove(1)
                    Next lConn
                    'renumber data sets to reflect met seg number
                    For Each lConn In lOpn.Sources
                        If lConn.Target.VolName = lOper.Name Then
                            lConn.Source.VolId = lConn.Source.VolId + lMetSegCounter
                        End If
                    Next lConn
                ElseIf lOper.Name = "RCHRES" Then
                    'remove all the sources coming from upstream through wdms
                    '(we can pass internally without reading from wdm)
                    i = 1
                    For Each lConn In lOper.Sources
                        If lConn.Source.VolName = "WDM4" Then
                            lOper.Sources.Remove(i)
                        Else
                            i = i + 1
                        End If
                    Next lConn
                    'reset the connection operation numbers
                    For Each lConn In lOper.Targets
                        If lConn.Source.VolName = "RCHRES" Then
                            lConn.Source.Opn.Id = lNewOperId
                            lConn.Source.VolId = lNewOperId
                        End If
                    Next lConn
                    For Each lConn In lOper.Sources
                        If lConn.Target.VolName = "RCHRES" Then
                            If Not lConn.Target.Opn Is Nothing Then
                                lConn.Target.Opn.Id = lNewOperId
                            End If
                            lConn.Target.VolId = lNewOperId
                        End If
                    Next lConn
                End If

                'RenumberOperationInSpecialActions(lUci, lOper.Name, lOrigId, lOper.Id)
                'now add the special actions records to the uci
                'For Each lRecord In lUci.SpecialActionBlk.Records
                '    lCombinedUci.SpecialActionBlk.Records.Add(lRecord)
                'Next

            Next lOper

            lUci = Nothing
        Next lUciCnt

        lCombinedUci.Source2MetSeg()
        ChDir("C:\cbp_working\output\combined")
        lCombinedUci.Save()

    End Function

    Private Function RenumberOperationInSpecialActions(ByVal aUci As atcUCI.HspfUci, ByVal aOperName As String, ByVal aOrigId As Integer, ByVal aNewId As Integer) As Boolean
        Dim lRecord As atcUCI.HspfSpecialRecord
        Dim i As Integer

        For Each lRecord In aUci.SpecialActionBlk.Records
            i = InStr(lRecord.Text, aOperName & "  " & aOrigId)
            If i > 0 Then
                Mid(lRecord.Text, i) = aOperName & aNewId
            End If
            i = InStr(lRecord.Text, aOperName & "   " & aOrigId)
            If i > 0 Then
                Mid(lRecord.Text, i) = aOperName & " " & aNewId
            End If
        Next
    End Function
End Class
