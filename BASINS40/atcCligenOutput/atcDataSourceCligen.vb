Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceCligen
  Inherits atcDataSource
  '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

  Private Shared pFileFilter As String = "Cligen Output Files (*.dat)|*.dat"
  Private pErrorDescription As String
  Private pColDefs As Hashtable
  'Private pReadAll As Boolean = False

  Public Overrides ReadOnly Property Description() As String
    Get
      Return "Cligen Output"
    End Get
  End Property

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Timeseries::Cligen Output"
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
        Dim lTable As New atcTableFixed
        Dim lSCol() As Integer = {0, 2, 5, 8, 12, 18, 24, 29, 36, 42, 48, 53, 58, 64}
        Dim lFLen() As Integer = {0, 2, 2, 4, 6, 6, 5, 7, 6, 6, 5, 5, 6, 6}
        Dim lFldNames() As String = {"", "da", "mo", "year", "prcp", "dur", "tp", "ip", _
                                     "tmax", "tmin", "rad", "w-vl", "w-dir", "tdew"}
        Dim lTSKey As String
        Dim lTSIndex As Integer
        Dim lLocation As String = FilenameOnly(aFileName)
        Dim lConstituentCode As Integer = -1
        Dim lJDate As Double
        Dim lDate() As Integer = {0, 0, 0, 24, 0, 0}
        Dim i As Integer
        Dim s As String
        With lTable
          .NumFields = 13
          .NumHeaderRows = 15
          For i = 1 To .NumFields
            .FieldName(i) = lFldNames(i)
            .FieldLength(i) = lFLen(i)
            .FieldStart(i) = lSCol(i)
          Next
          If lTable.OpenFile(aFileName) Then
            While Not lTable.atEOF
              For i = 4 To .NumFields
                lTSKey = .FieldName(i)
                lData = DataSets.ItemByKey(lTSKey)
                If lData Is Nothing Then
                  lData = New atcTimeseries(Me)
                  lData.Dates = New atcTimeseries(Me)
                  lData.numValues = .NumRecords
                  lData.Value(0) = Double.NaN
                  lData.Dates.Value(0) = Double.NaN
                  lData.Attributes.SetValue("Count", 0)
                  lData.Attributes.SetValue("Scenario", "CLIGEN")
                  lData.Attributes.SetValue("Location", lLocation)
                  lData.Attributes.SetValue("Constituent", .FieldName(i))
                  lData.Attributes.SetValue("point", False)
                  DataSets.Add(lTSKey, lData)
                End If
                lDate(0) = .Value(3)
                lDate(1) = .Value(2)
                lDate(2) = .Value(1)
                lJDate = Date2J(lDate)
                If lJDate <> 0 Then
                  lTSIndex = lData.Attributes.GetValue("Count") + 1
                  lData.Value(lTSIndex) = .Value(i)
                  lData.Dates.Value(lTSIndex) = lJDate
                  lData.Attributes.SetValue("Count", lTSIndex)
                  If lTSIndex = 1 Then 'put start date in 0th position of date array
                    lData.Dates.Value(0) = lJDate - 1
                  End If
                End If
              Next i
              .MoveNext()
            End While
            For Each lData In DataSets
              lData.numValues = lData.Attributes.GetValue("Count")
            Next
            Open = True
          Else
            Open = False
            Logger.Msg("Unable to process Cligen file " & aFileName, "Cligen Open")
          End If
        End With
      Catch endEx As EndOfStreamException
        Open = False
      End Try
    End If
  End Function

  Private Function parseWQObsDate(ByVal aDate As String, ByVal aTime As String) As Double
    'assume point values at specified time
    Dim d(5) As Integer 'date array
    Dim l As Integer 'Length of year (2 or 4 digit year)
    Dim i As Integer 'Year offset (1900 for 2-digit year)

    If Not IsNumeric(aTime) Then aTime = "1200" 'assume noon for missing obstime
    If IsNumeric(aDate) Then
      If Len(aDate) = 8 Then ' 4 dig yr
        l = 4
        i = 0
      Else
        l = 2
        i = 1900
      End If
      d(0) = Left(aDate, l) + i
      d(1) = Mid(aDate, l + 1, 2)
      d(2) = Right(aDate, 2)
      If IsNumeric(aTime) Then
        d(3) = Left(aTime, 2)
        d(4) = Right(aTime, 2)
      End If
      Return Date2J(d)
    Else
      Return 0
    End If
  End Function

End Class