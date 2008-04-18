Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Text

Public Class PlugIn
    Inherits atcData.atcDataDisplay

    Private WithEvents pDataGroup As atcDataGroup   'group of atcData potentially used for analysis

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Biotic Ligand Model"
        End Get
    End Property
    Public Overrides ReadOnly Property Author() As String
        Get
            Return "HydroQual, Inc - Model and AQUA TERRA Consultants - BASINS interface"
        End Get
    End Property
    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        pMapWin = aMapWin
    End Sub
    Public Overrides Function Show(ByVal aDataGroup As atcDataGroup) _
                 As Object 'System.Windows.Forms.Form
        'creating an instance of the form asks user to specify some Data if none has been passed in
        Dim lfrmBLM As New frmBLM(Me, pDataGroup)

        If Not (pDataGroup Is Nothing) AndAlso pDataGroup.Count > 0 Then
            lfrmBLM.Show()
            Return lfrmBLM
        Else 'No data to run model on, don't show or return the form
            lfrmBLM.Dispose()
            Return Nothing
        End If
    End Function
 
    Public Sub RunBLM(ByVal aLocation As String, ByVal aStationName As String)
        Dim lString As String

        Dim lLocationData As atcDataGroup = pDataGroup.FindData("Location", aLocation)
        Logger.Dbg("RunBLM for " & aLocation & " DatasetCount " & lLocationData.Count)

        'TODO: get actual data for the location
        'lSB.Append(GetEmbeddedFileAsString("sampleData.blm"))
        Dim lConstituents As atcCollection = ConstituentsNeeded()
        Dim lConstituentData As New atcDataGroup
        Dim lConstituentMatchRequestedCount As Integer = 0
        For Each lDataSet As atcTimeseries In lLocationData
            If lConstituents.IndexFromKey(lDataSet.Attributes.GetDefinedValue("Constituent").Value) > -1 Then
                lConstituentMatchRequestedCount += 1
                If lDataSet.numValues > 10 Then 'need enouth data to be worthwhile
                    lConstituentData.Add(lDataSet)
                End If
            End If
        Next
        Logger.Dbg("ConstituentDataCount:" & lConstituentData.Count & ":" & lConstituentMatchRequestedCount)

        Dim lOutputDataRecords As New atcCollection
        Dim lAttributes As New ArrayList
        lAttributes.Add("Constituent")
        lAttributes.Add("Units")
        Dim lTimeseriesGridSource As New atcTimeseriesGridSource(lConstituentData, lAttributes, True)
        With lTimeseriesGridSource
            For lRowIndex As Integer = 2 To .Rows ' skip name and units
                Dim lDataCount As Integer = 0
                Dim lDataColumn(12) As Integer 'a fresh copy
                For lColumnIndex As Integer = 1 To .Columns
                    Dim lCellValue As String = .CellValue(lRowIndex, lColumnIndex)
                    If lCellValue.Length > 0 AndAlso IsNumeric(lCellValue) Then
                        Dim lDataIndex As Integer = lConstituents.IndexFromKey(.CellValue(0, lColumnIndex))
                        If lDataIndex > -1 AndAlso lDataColumn(lDataIndex) = 0 Then
                            lDataColumn(lDataIndex) = lColumnIndex
                            lDataCount += 1
                        End If
                    End If
                Next
                If lDataCount > 8 Then
                    lString = Chr(34) & "T" & Chr(34) & "," & _
                              Chr(34) & aLocation & Chr(34) & "," & _
                              Chr(34) & .CellValue(lRowIndex, 0).PadRight(20) & Chr(34) & ","
                    For lDataIndex As Integer = 0 To 12
                        If lDataColumn(lDataIndex) > 0 Then
                            lString &= .CellValue(lRowIndex, lDataColumn(lDataIndex)).PadLeft(14)
                        Else
                            lString &= Space(14)
                        End If
                    Next
                    lOutputDataRecords.Add(lString)
                End If
            Next
        End With
        Logger.Dbg("OutputDataRecordCount:" & lOutputDataRecords.Count)

        Dim lSB As New StringBuilder
        'generic headers - TODO: need to be able to edit
        lSB.Append(GetEmbeddedFileAsString("default.blm"))
        'data headers
        lSB.AppendLine(lOutputDataRecords.Count)
        lSB.AppendLine(aStationName & " (USGS Station Number " & aLocation & ")")
        lString = "Site Label             Sample Label  "
        For Each lConstituent As String In lConstituents
            lString &= lConstituent.PadLeft(14)
        Next
        lSB.AppendLine(lString)

        'TODO: don't hard code!!
        lString = Space(35) & "C".PadLeft(14) & "%".PadLeft(14) & Space(14)
        For lIndex As Integer = 0 To 8
            lString &= "mg/L".PadLeft(14)
        Next
        lSB.AppendLine(lString)
        'data
        For Each lString In lOutputDataRecords
            lSB.AppendLine(lString)
        Next

        'other stuff at end
        lSB.AppendLine("/*")
        lSB.AppendLine("3,5,-999")
        Dim lDataPath As String = PathNameOnly(pMapWin.Project.FileName) & "\BLM\"
        SaveFileString(lDataPath & aLocation & ".blm", lSB.ToString)
        SaveFileString(lDataPath & aLocation & ".grid", lTimeseriesGridSource.ToString)

        'BLM must be installed and have the file extension associated with the exe for this to work!
        OpenFile(lDataPath & aLocation & ".blm", True)
        'TODO: add BLM model results to data group
        'TODO: display results
    End Sub

    Private Function ConstituentsNeeded() As atcCollection
        Dim lConstituents As New atcCollection
        lConstituents.Add("Temperature", "Temp.")
        lConstituents.Add("Humic Acid Fraction", "HA")
        lConstituents.Add("pH", "pH")
        lConstituents.Add("Copper", "Cu")
        lConstituents.Add("Organic carbon", "DOC")
        lConstituents.Add("Calcium", "Ca")
        lConstituents.Add("Magnesium", "Mg")
        lConstituents.Add("Sodium", "Na")
        lConstituents.Add("Potassium", "K")
        lConstituents.Add("Sulfate", "SO4")
        lConstituents.Add("Cloride", "Cl")
        lConstituents.Add("Inorganic carbon", "DIC")
        lConstituents.Add("Sulfer", "S")
        Return lConstituents
    End Function
End Class
