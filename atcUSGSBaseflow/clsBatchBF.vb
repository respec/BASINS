Imports atcTimeseriesBaseflow
Imports atcUtility
Imports atcBatchProcessing

''' <summary>
''' The overall organizing program
''' </summary>
''' <remarks></remarks>
Public Class clsBatchBF
    Private Specs As clsBatchBFSpec
    

    Public Sub New(ByVal aSpec As clsBatchBFSpec)
        Specs = aSpec
    End Sub

    Public Sub ProcessBatch()
        If Specs Is Nothing Then Return

        For Each lBFOpnID As Integer In Specs.ListBatchBaseflowOpns.Keys
            Dim lBFOpn As atcCollection = Specs.ListBatchBaseflowOpns.ItemByKey(lBFOpnID)
            For Each lStation As clsBatchUnitStation In lBFOpn

            Next
        Next

    End Sub
End Class
