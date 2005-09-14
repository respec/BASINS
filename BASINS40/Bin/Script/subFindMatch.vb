Imports atcData
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptFindMatch
  Public Function Main(ByVal aDataGroup As atcDataGroup, ByVal aName As String, ByVal aValue As String, ByVal aLimit As Integer) As atcDataGroup
    Dim lMatch As New atcDataGroup
    Dim lValue As String
    Try
      For Each lDataset As atcDataSet In aDataGroup
        lValue = lDataset.Attributes.GetValue(aName)
        If lValue.ToLower = aValue.ToLower Then
          lMatch.Add(lDataset)
          If lMatch.Count = aLimit Then 'note default limit is 0, never match
            Exit For
          End If
        End If
      Next
    Catch
    End Try

    Return lMatch
  End Function
End Module
