Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData
'Imports atcDataTree
'Imports atcEvents

Public Module GenCloud
    Private Const pInputPath As String = "F:\BASINSMet\WDMFiltered\Ishshifted\Subset\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMFiltered\IshShifted\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenCloud:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lMissingDBF As New atcTableDBF
        Dim lISHWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lSODWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lBasinsWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lState As String = ""
        Dim lts As atcTimeseries = Nothing
        Dim lCurWDM As String = pOutputPath & "current.wdm"

        If lMissingDBF.OpenFile("F:\BASINSMet\WDMRaw\MissingSummary.dbf") Then
            Logger.Dbg("GenCloud: Opened Station Location file F:\BASINSMet\WDMRaw\MissingSummary.dbf")
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("GenCloud: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenCloud: Opening data file - " & lFile)
            FileCopy(lFile, lCurWDM)
            lISHWDMFile = New atcWDM.atcDataSourceWDM
            lISHWDMFile.Open(lCurWDM)
            For Each lDS As atcDataSet In lISHWDMFile.DataSets
                If lDS.Attributes.GetValue("Constituent") = "SKYCOND" Then
                    lts = lDS
                    Exit For
                End If
            Next
            If Not lts Is Nothing Then
                lStation = FilenameOnly(lFile)
                If lMissingDBF.FindFirst(3, lStation) Then 'found station ID on Missing Summary DBF
                    'now find %missing for SkyCond timeseries
                    While lMissingDBF.Value(3) = lStation
                        If lMissingDBF.Value(4) = "SKYCOND" Then Exit While
                        lMissingDBF.CurrentRecord += 1
                    End While
                    If lMissingDBF.Value(4) = "SKYCOND" AndAlso lMissingDBF.Value(19) < 10 Then
                        Dim ltsCloud As atcTimeseries = SkyCondOktas2CloudTenths(lts)
                        Dim lSJD As Double = ltsCloud.Attributes.GetValue("SJDAY")
                        If lSJD - Math.Truncate(lSJD) > Double.Epsilon Then
                            'not at start of a day, round up to first whole day (likely Jan. 1)
                            lSJD = Math.Truncate(lSJD) + 1
                        End If
                        Dim lEJD As Double = ltsCloud.Attributes.GetValue("EJDAY")
                        If lEJD - Math.Truncate(lEJD) > Double.Epsilon Then
                            'not at end of a day, round to end of last day
                            lEJD = Math.Truncate(lEJD) + 1
                        End If
                        Dim lts3 As atcTimeseries = SubsetByDate(ltsCloud, lSJD, lEJD, Nothing)
                        Dim lts2 As atcTimeseries = FillMissingByInterpolation(lts3, 1)
                        lts2.Attributes.SetValue("ID", 100)
                        lts2.Attributes.SetValue("Constituent", "CLOU")
                        If lISHWDMFile.AddDataSet(lts2) Then
                            Logger.Dbg("GenCloud: Wrote Hourly CLOU to DSN 100")
                            'now generate daily cloud cover for generation of Daily Solar
                            lts = Nothing
                            lts = Aggregate(lts2, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            lts.Attributes.SetValue("ID", 101)
                            lts.Attributes.SetValue("Constituent", "DCLO")
                            If lISHWDMFile.AddDataSet(lts) Then
                                Logger.Dbg("GenCloud: Wrote DCLO to DSN 101")
                            Else
                                Logger.Dbg("GenCloud: PROBLEM - Could not write DCLO to DSN 101")
                            End If
                        Else
                            Logger.Dbg("GenCloud: PROBLEM - Could not write Hourly CLOU to DSN 100")
                        End If
                        lts2 = Nothing
                    Else
                        Logger.Dbg("GenCloud: Percent Missing (" & lMissingDBF.Value(19) & ") too high for use")
                    End If
                Else
                    Logger.Dbg("GenCloud: PROBLEM - could not find Missing Summary for SKYCOND")
                End If
                lts = Nothing
            Else
                Logger.Dbg("GenCloud: Station does not contain SKYCOND dataset")
            End If
            lISHWDMFile.DataSets.Clear()
            FileCopy(lCurWDM, lFile)
            Kill(lCurWDM)
            lISHWDMFile = Nothing
        Next

        Logger.Dbg("GenCloud: Completed Cloud Cover generation")

        'Application.Exit()

    End Sub

    Private Sub CombineData(ByRef aTS1 As atcTimeseries, ByRef aDatasets As atcDataGroup, ByRef aWDMFile As atcWDM.atcDataSourceWDM, ByVal aCons As String)
        'Writes timeseries from two sources to one new and updated dataset on a WDM file
        'aTS1 - initial timeseries to write to new WDM file
        'aDatasets - datasets containing timeseries to append to aTS1
        'aWDMFile - WDM file to which timeseries are saved
        'aStation - station ID 
        'aCons - constituent name in aDatasets' timeseries for which data are to be appended

        Dim lts As atcTimeseries = Nothing
        Dim lID As String = Nothing
        Dim lDate As Double
        lts = aTS1.Clone
        If lts.Attributes.GetValue("Constituent") = "HPCP" Then 'special case for precip, always 1
            lID = 1
        Else 'use Basins 3.1 constituent index (1-8) for DSN
            lID = Right(lts.Attributes.GetValue("ID").ToString, 1)
        End If
        lts.Attributes.SetValue("ID", lID)
        If aWDMFile.AddDataSet(lts) Then
            lDate = lts.Attributes.GetValue("EJDay")
            Logger.Dbg("AppendHourly:CombineData:  Wrote initial TS for " & lts.Attributes.GetValue("Constituent") & _
                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lDate)
            lts = Nothing
            For Each lDS As atcDataSet In aDatasets
                If lDS.Attributes.GetValue("Constituent") = aCons Then
                    If lDS.Attributes.GetValue("SJDay") < lDate AndAlso _
                       lDS.Attributes.GetValue("EJDay") > lDate Then
                        lts = SubsetByDate(lDS, lDate, lDS.Attributes.GetValue("EJDay"), Nothing)
                        lts.Attributes.SetValue("ID", lID)
                        If aWDMFile.AddDataSet(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("AppendHourly:CombineData:  Appended TS for " & lts.Attributes.GetValue("Constituent") & _
                                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lts.Attributes.GetValue("EJDay"))
                        Else
                            Logger.Dbg("AppendHourly:CombineData:  PROBLEM Appending TS for " & lts.Attributes.GetValue("Constituent") & _
                                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lts.Attributes.GetValue("EJDay"))
                        End If
                        lts = Nothing
                    End If
                    Exit For
                End If
            Next
        Else
            Logger.Dbg("AppendHourly:CombineData:  PROBLEM writing initial TS for " & lts.Attributes.GetValue("Constituent") & _
                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lDate)
        End If

    End Sub

    Private Function SkyCondOktas2CloudTenths(ByRef aHrlySky As atcTimeseries) As atcTimeseries
        'convert ISH hourly Sky Condition timeseries (recorded in Oktas)
        'to Cloud Cover timeseries in tenths

        Dim lts As atcTimeseries = aHrlySky.Clone
        Dim lVal As Double = 0

        For i As Integer = 1 To lts.numValues
            lVal = lts.Value(i)
            If lVal >= 0 AndAlso lVal <= 10 Then
                Select Case lVal
                    Case 0, 1 'same as recorded okta value
                        lts.Value(i) = lVal
                    Case 2 '2/10 - 3/10
                        lts.Value(i) = 2.5
                    Case 3, 4, 5 '1/10 higher than recorded okta value
                        lts.Value(i) = lVal + 1
                    Case 6 '7/10 - 8/10
                        lts.Value(i) = 7.5
                    Case 7, 8 '2/10 higher than recorded okta value
                        lts.Value(i) = lVal + 2
                    Case 9, 10 'assume fully covered
                        lts.Value(i) = 10
                    Case Else
                        lts.Value(i) = lVal
                End Select
            End If
        Next
        Return lts
    End Function

End Module
