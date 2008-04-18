Imports atcData
Imports MapWinUtility

Public Class atcListPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::List"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataGroup As atcData.atcDataGroup) As Object
        Dim lForm As New atcListForm
        lForm.Initialize(aDataGroup)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aDataGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            Dim lForm As New atcListForm
            lForm.Initialize(aDataGroup)
            For Each lOption As String In aOption
                Select Case lOption.ToLower
                    Case "filternodata"
                        lForm.mnuFilterNoData.Checked = True
                    Case "viewnovalues"
                        lForm.mnuViewValues.Checked = False
                    Case "attributerows"
                        lForm.mnuAttributeRows.Checked = True
                    Case "attributecolumns"
                        lForm.mnuAttributeColumns.Checked = True
                    Case Else
                        Logger.Dbg("UnknownParameter:" & lOption)
                End Select
            Next
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub

End Class
