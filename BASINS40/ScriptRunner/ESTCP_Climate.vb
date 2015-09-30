Imports MapWindow.Interfaces   'MapWinInterfaces.dll resides in the BASINS41\bin folder
Imports MapWinUtility          'MapWinUtility.dll resides in the BASINS41\bin folder

Imports atcUtility             'atcUtility.dll resides in the BASINS41\bin\Plugins\BASINS folder
Imports atcData                'atcData.dll resides in the BASINS41\bin\Plugins\BASINS folder

Imports atcHspfBinOut          'atcHspfBinOut.dll resides in the BASINS41\bin\Plugins\BASINS folder
Imports atcTimeseriesSWAT      'atcTimeseriesSWAT.dll resides in the BASINS41\bin\Plugins\BASINS folder

Imports System.IO
Imports System.Text

Public Module ESTCP_Climate

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'open a wdm file:
        Dim lDataSource As atcWDM.atcDataSourceWDM = Nothing
        Dim lWDMFileName As String = "D:\ESTCP\ACF\ClimateScenario\ACF_PMETRev_met.WDM"
        If FileExists(lWDMFileName) Then
            'check to see if this file is already open in this project
            lDataSource = atcDataManager.DataSourceBySpecification(lWDMFileName)

            If lDataSource Is Nothing Then 'need to open it here
                lDataSource = New atcWDM.atcDataSourceWDM
                If Not lDataSource.Open(lWDMFileName) Then
                    lDataSource = Nothing
                End If
            End If
        End If

        'create new WDM file for storing results
        Dim lNewWDM As String = "D:\ESTCP\ACF\ClimateScenario\climate.WDM"
        Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
        lNewWDMfile.Open(lNewWDM)

        'at this point one can loop through the timeseries of the file
        For Each lDataSet As atcTimeseries In lDataSource.DataSets
            Dim lConstituent As String = lDataSet.Attributes.GetValue("Constituent")
            Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
            Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
            Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
            Dim lFileName As String = lDataSet.Attributes.GetValue("Data Source")

            'data set numbers of interest:
            'columbus precip 250
            '         air temp 243
            '         pmet 249
            'talbotton precip 481
            '          air temp 483
            '          pmet 489

            Dim lIsPrecip As Boolean = False
            Dim lIsAirTemp As Boolean = False
            Dim lThresh(12) As Double
            Dim lLower(12) As Double
            Dim lUpper(12) As Double
            Dim lAdd(12) As Double

            If lDsn = 250 Then
                'columbus precip
                lIsPrecip = True
                Dim lT1() As Double = {0.08, 0.1, 0.1173, 0.12, 0.1098, 0.11, 0.101, 0.11, 0.0998, 0.1, 0.1, 0.1}
                lThresh = lT1
                Dim lL1() As Double = {0.893504584940514, 0.828267674241468, 0.970722234194858, 0.891950243062045, 0.970506469322334, 0.551567211283874, 0.67801078320664, 0.818947966820425, 0.879370857035774, 1.25478163237953, 0.988475753550421, 1.25805472748197}
                lLower = lL1
                Dim lU1() As Double = {0.826286441072365, 1.21864270301569, 1.07371092138687, 0.726635267195552, 0.754843065844099, 0.859877760904651, 0.874591861128565, 0.691691596046413, 0.574252711757115, 0.940860733790508, 1.04684082928219, 0.93246526447717}
                lUpper = lU1
            ElseIf lDsn = 481 Then
                'talbotton precip
                lIsPrecip = True
                Dim lT2() As Double = {0.1171, 0.144, 0.189, 0.2, 0.146, 0.1808, 0.21, 0.2026, 0.16, 0.137, 0.1353, 0.1392}
                lThresh = lT2
                Dim lL2() As Double = {0.920459150450074, 0.840029628716995, 1.01984671405549, 0.941849420994994, 0.939821284235125, 0.530263329029638, 0.618420706019668, 0.783403088975432, 0.935042204355617, 1.33187953140681, 0.896392426985803, 1.24755208471955}
                lLower = lL2
                Dim lU2() As Double = {0.865610064209037, 1.17936686002546, 1.07001644283037, 0.689807782516879, 0.758522284215922, 0.775414256019918, 1.07886837778602, 0.669995812469504, 0.851073227301518, 0.605796750484189, 1.13625184099827, 0.808775246640861}
                lUpper = lU2
            ElseIf lDsn = 243 Then
                'columbus air temp
                lIsAirTemp = True
                Dim lA1() As Double = {2.10158, 1.95728, 1.82882, 1.69035, 2.27918, 3.44878, 3.54368, 3.0908, 2.71039, 2.91283, 2.24485, 3.32161}
                lAdd = lA1
            ElseIf lDsn = 483 Then
                'talbotton air temp
                lIsAirTemp = True
                Dim lA2() As Double = {2.13885, 1.9827, 1.80692, 1.67707, 2.2769, 3.43597, 3.55932, 2.96772, 2.6209, 2.86338, 2.25528, 3.36241}
                lAdd = lA2
            End If

            If lIsPrecip Then
                Dim lDateA(6) As Integer
                For lIndex As Integer = 0 To lDataSet.numValues
                    'convert julian date into date array so we know what month this is
                    Dim lDate As Double = lDataSet.Dates.Values(lIndex) - 0.0001
                    J2Date(lDate, lDateA)

                    Dim lValue As Double = lDataSet.Values(lIndex)
                    If lValue > 0 Then
                        If lValue > lThresh(lDateA(1) - 1) Then
                            lValue = lValue * lUpper(lDateA(1) - 1)
                        Else
                            lValue = lValue * lLower(lDateA(1) - 1)
                        End If
                        lDataSet.Values(lIndex) = lValue
                    End If
                Next
                lNewWDMfile.AddDataset(lDataSet)
            ElseIf lIsAirTemp Then
                Dim lDateA(6) As Integer
                For lIndex As Integer = 0 To lDataSet.numValues
                    'convert julian date into date array so we know what month this is
                    Dim lDate As Double = lDataSet.Dates.Values(lIndex) - 0.0001
                    J2Date(lDate, lDateA)

                    Dim lValue As Double = lDataSet.Values(lIndex)
                    lValue = lValue + (lAdd(lDateA(1) - 1) * 9.0 / 5.0)
                    lDataSet.Values(lIndex) = lValue
                Next
                lNewWDMfile.AddDataset(lDataSet)
            End If

        Next
        lDataSource.Save("D:\ESTCP\ACF\ClimateScenario\climate.WDM")
    End Sub

End Module
