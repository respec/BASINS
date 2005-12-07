Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceSWATDBF
  Inherits atcDataSource
  '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

  Private Shared pFileFilter As String = "SWAT Output Files (*.dbf)|*.dbf"
  Private pErrorDescription As String
  Private pColDefs As Hashtable
  'Private pReadAll As Boolean = False

  Public Overrides ReadOnly Property Description() As String
    Get
      Return "SWAT Output DBF"
    End Get
  End Property

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Timeseries::SWATDBF"
    End Get
  End Property

  Public Overrides ReadOnly Property Category() As String
    Get
      Return "File"
    End Get
  End Property

  Public Overrides ReadOnly Property CanOpen() As Boolean
    Get
      Return True 'yes, this class can open files
    End Get
  End Property

  Public Overrides ReadOnly Property CanSave() As Boolean
    Get
      Return False 'no saving yet, but could implement if needed 
    End Get
  End Property

  Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
    Dim lData As atcTimeseries
    Dim lDates As atcTimeseries

    If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
      aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
    End If

    If Not FileExists(aFileName) Then
      pErrorDescription = "File '" & aFileName & "' not found"
    Else
      Me.Specification = aFileName

      Try
        Dim i As Integer
        Dim lDBF As IATCTable
        Dim lDateCol As Integer = -1
        Dim lLocnCol As Integer = -1
        Dim lLUCol As Integer = -1
        Dim lSoilCol As Integer = -1
        Dim lTSKey As String
        Dim lTSIndex As Integer
        Dim lNCons As Integer = 0
        Dim lConsName As String
        Dim s As String
        lDBF = New clsATCTableDBF
        lDBF.OpenFile(aFileName)
        Dim lConsFields(lDBF.NumFields) As Integer
        Dim lConsNames(lDBF.NumFields) As String
        For i = 1 To lDBF.NumFields
          Select Case UCase(lDBF.FieldName(i))
            Case "DATE" : lDateCol = i
            Case "HUC", "SUBBASIN" : lLocnCol = i
            Case "LANDUSE" : lLUCol = i
            Case "HRU" 'ignore for now
            Case "SOIL" : lSoilCol = i
            Case Else
              lNCons += 1
              lConsFields(lNCons) = i
              lConsNames(lNCons) = lDBF.FieldName(i)
          End Select
        Next
        If lDateCol > 0 AndAlso lLocnCol > 0 Then
          While Not lDBF.atEOF
            For i = 1 To lNCons
              lConsName = lConsNames(i)
              If lLUCol > -1 Then lConsName += ":" & lDBF.Value(lLUCol)
              If lSoilCol > -1 Then lConsName += ":" & lDBF.Value(lSoilCol)
              lTSKey = lDBF.Value(lLocnCol) & ":" & lConsName
              lData = DataSets.ItemByKey(lTSKey)
              If lData Is Nothing Then
                lData = New atcTimeseries(Me)
                lData.Dates = New atcTimeseries(Me)
                lData.numValues = lDBF.NumRecords
                lData.Dates.numValues = lData.numValues
                lData.Attributes.SetValue("Count", 0)
                lData.Attributes.SetValue("Scenario", "OBSERVED")
                lData.Attributes.SetValue("Location", lDBF.Value(lLocnCol))
                lData.Attributes.SetValue("Constituent", lConsName)
                lData.Attributes.SetValue("point", False)
                DataSets.Add(lTSKey, lData)
              End If
              lTSIndex = lData.Attributes.GetValue("Count") + 1
              lData.Value(lTSIndex) = lDBF.Value(lConsFields(i))
              lData.Attributes.SetValue("Count", lTSIndex)
              If lTSIndex = 1 Then 'put start date in 0th position of date array
                lData.Dates.Value(0) = parseTAMUDate(lDBF.Value(lDateCol), True)
              End If
              lData.Dates.Value(lTSIndex) = parseTAMUDate(lDBF.Value(lDateCol), False)
            Next i
            lDBF.MoveNext()
          End While
          For Each lData In DataSets
            lData.numValues = lData.Attributes.GetValue("Count")
          Next
          Open = True
        End If
      Catch endEx As EndOfStreamException
        Open = False
      End Try
    End If
  End Function

  Private Function parseTAMUDate(ByVal s As String, ByVal StartOfInterval As Boolean) As Double
    'assume mean values at end of interval
    Dim lDate As Double
    Dim lYr As Integer
    Dim lMn As Integer
    Dim lDy As Integer

    If IsNumeric(s) Then
      If Len(s) = 8 Then 'day
        lMn = Left(s, 2) 'Mid(s, 3, 2)
        lDy = Mid(s, 3, 2)
        lYr = Mid(s, 5, 4)
        If StartOfInterval Then
          lDate = Jday(lYr, lMn, lDy, 0, 0, 0)
        Else
          lDate = Jday(lYr, lMn, lDy, 24, 0, 0)
        End If
      ElseIf Len(s) = 6 Then 'month
        lYr = Mid(s, 3, 4)
        lMn = Left(s, 2)
        If StartOfInterval Then
          lDate = Jday(lYr, lMn, 1, 0, 0, 0)
        Else
          lDate = Jday(lYr, lMn, daymon(lYr, lMn), 24, 0, 0)
        End If
      Else 'year
        lYr = Left(s, 4)
        If StartOfInterval Then
          lDate = Jday(lYr, 1, 1, 0, 0, 0)
        Else
          lDate = Jday(lYr, 12, 31, 24, 0, 0)
        End If
      End If
    Else
      lDate = 0
    End If

    Return lDate

  End Function

End Class