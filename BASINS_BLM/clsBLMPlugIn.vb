Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Text

Public Class PlugIn
    Inherits atcData.atcDataDisplay

    Friend pMapWin As MapWindow.Interfaces.IMapWin
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
    Public Overrides Function Show(Optional ByVal aDataGroup As atcDataGroup = Nothing) _
                 As Object 'System.Windows.Forms.Form
        Dim lDataGroup As atcDataGroup = aDataGroup

        'creating an instance of the form asks user to specify some Data if none has been passed in
        Dim lfrmBLM As New frmBLM(Me, lDataGroup)

        If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
            lfrmBLM.Show()
            Return lfrmBLM
        Else 'No data to display, don't show or return the form
            lfrmBLM.Dispose()
            Return Nothing
        End If
    End Function
 
    Public Sub RunBLM(ByVal aLocation As String)
        Dim lLocationData As atcDataGroup = atcDataManager.DataSets.FindData("Location", aLocation)
        Logger.Dbg("RunBLM for " & aLocation & " DatasetCount " & lLocationData.Count)

        Dim lSB As New StringBuilder
        'header - need to be able to edit
        lSB.Append(GetEmbeddedFileAsString("default.blm"))

        'TODO: get actual data for the location
        'lSB.Append(GetEmbeddedFileAsString("sampleData.blm"))
        Dim lConstituents As atcCollection = ConstituentsNeeded()
        Dim lConstituentData As New atcDataGroup
        For Each lDataSet As atcDataSet In lLocationData
            If lConstituents.IndexFromKey(lDataSet.Attributes.GetDefinedValue("Constituent").Value) > -1 Then
                lConstituentData.Add(lDataSet)
            End If
        Next
        Logger.Dbg("ConstituentDataCount:" & lConstituentData.Count)

        lSB.AppendLine("/*")
        lSB.AppendLine("3,5,-999")
        Dim lDataPath As String = PathNameOnly(pMapWin.Project.FileName) & "\BLM\"
        SaveFileString(lDataPath & aLocation & ".blm", lSB.ToString)

        'BLM must be installed and have the file extension associated with the exe for this to work!
        OpenFile(lDataPath & aLocation & ".blm", True)
        'TODO: add BLM model results to data group
        'TODO: display results
    End Sub

    Private Function ConstituentsNeeded() As atcCollection
        Dim lConstituents As New atcCollection
        lConstituents.Add("Temperature", "Temperature")
        lConstituents.Add("Humic Acid Fraction", "HA:10")
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
