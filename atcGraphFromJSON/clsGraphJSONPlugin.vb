Imports atcData.atcDataManager
Imports MapWinUtility

Public Class clsGraphJSONPlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Graph From JSON"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Create a Graph from a JSON file."
        End Get
    End Property

    <CLSCompliant(False)>
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_GraphFromJSON", AnalysisMenuName, "Graph From JSON")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(AnalysisMenuName & "_GraphFromJSON")
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = AnalysisMenuName & "_GraphFromJSON" Then
            Dim lOpenDialog As New System.Windows.Forms.OpenFileDialog
            With lOpenDialog
                .Title = "Open JSON file(s) containing graph specs"
                .DefaultExt = ".json"
                .Filter = "JSON files|*.json"
                .Multiselect = True

                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    For Each lFileName As String In .FileNames
                        Dim lOutputFileName As String = FilenameNoExt(lFileName) & ".png"
                        atcGraph.GraphJsonToFile(lFileName, lOutputFileName)
                    Next
                End If
            End With
            Handled = True
        End If
    End Sub
End Class
