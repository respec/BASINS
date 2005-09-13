Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcTimeseriesTD3240
  Inherits atcDataSource
  '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

  Private Shared pFileFilter As String = "TD-3240 Files (*.ncd)|*.ncd"
  Private pErrorDescription As String
  Private pColDefs As Hashtable
  'Private pReadAll As Boolean = False

  Public Overrides ReadOnly Property Description() As String
    Get
      Return "Hourly Precip, Archive Format, TD-3240"
    End Get
  End Property

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "TD-3240"
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

  Private Function AddColEnd(ByVal aStart As Integer, ByVal aEnd As Integer, ByVal aName As String) As ColDef
    Return AddColumn(aStart, aEnd - aStart + 1, aName)
  End Function

  Private Function AddColumn(ByVal aStart As Integer, ByVal aWidth As Integer, ByVal aName As String) As ColDef
    AddColumn = New ColDef(aStart, aWidth, aName)
    pColDefs.Add(aName, AddColumn)
  End Function

  Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
    Dim lData As atcTimeseries
    Dim lDates As atcTimeseries
    If Not FileExists(aFileName) Then
      pErrorDescription = "File '" & aFileName & "' not found"
    Else
      Me.Specification = aFileName
      Dim inStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
      Dim inBuffer As New BufferedStream(inStream)
      Dim inReader As New BinaryReader(inBuffer)
      Dim curLine As String
      Dim repeat As Integer = 0
      Dim repeatOffset As Integer
      Dim repeatsThisLine As Integer
      Dim maxRepeat As Integer = 24

      Dim vals(100) As Double 'array of data values
      Dim dats(100) As Double 'array of julian dates
      Dim iVal As Integer = 0 'current index for populating vals and dats

      pColDefs = New Hashtable
      Dim ColRecType As ColDef = AddColEnd(1, 3, "RecType")
      Dim ColState As ColDef = AddColEnd(4, 5, "State")
      Dim ColLocation As ColDef = AddColEnd(4, 11, "Location")
      Dim ColElementType As ColDef = AddColEnd(12, 15, "ElementType")
      Dim ColUnits As ColDef = AddColEnd(16, 17, "Units")
      Dim ColYear As ColDef = AddColEnd(18, 21, "Year")
      Dim ColMonth As ColDef = AddColEnd(22, 23, "Month")
      Dim ColDay As ColDef = AddColEnd(26, 27, "Day")
      Dim ColRepeats As ColDef = AddColEnd(28, 30, "Repeats")

      'These columns can repeat multiple times per line
      Dim ColHour(0) As ColDef
      Dim ColMinute(0) As ColDef
      Dim ColValue(0) As ColDef
      Dim ColFlag1(0) As ColDef
      Dim ColFlag2(0) As ColDef

      RedimensionRepeating(maxRepeat, ColHour, ColMinute, ColValue, ColFlag1, ColFlag2)

      Try
        curLine = NextLine(inReader)
        PopulateColumns(curLine)
        repeatsThisLine = CInt(ColRepeats.Value)

        If (ColRecType.Value = "HPD" AndAlso _
           ColElementType.Value = "HPCP" AndAlso _
           (ColUnits.Value = "HI" OrElse ColUnits.Value = "HT") AndAlso _
           IsNumeric(ColValue(0).Value) AndAlso _
           IsNumeric(ColYear.Value) AndAlso _
           IsNumeric(ColMonth.Value) AndAlso _
           IsNumeric(ColDay.Value) AndAlso _
           IsNumeric(ColHour(0).Value) AndAlso _
           IsNumeric(ColMinute(0).Value) AndAlso _
           ColYear.Value > 1700 AndAlso _
           ColMonth.Value < 13 AndAlso _
           ColDay.Value > 0 AndAlso _
           ColDay.Value < 32 AndAlso _
           ColHour(0).Value < 26 AndAlso _
           ColMinute(0).Value <= 60) Then

          lDates = New atcTimeseries(Me)
          lData = New atcTimeseries(Me)
          lData.Dates = lDates
          lData.Attributes.SetValue("Scenario", "OBSERVED")
          lData.Attributes.SetValue("Location", ColLocation.Value)
          lData.Attributes.SetValue("Constituent", ColElementType.Value)
          lData.Attributes.SetValue("Description", "Hourly Precip in Inches")
          Me.AddDataSet(lData)
          'If pReadAll Then
          Dim MissingVal As Double = -9.99
          Dim MissingAcc As Double = -9.98
          'Dim nVals As Integer = ?
          Dim InMissing As Boolean = False
          Dim InAccum As Boolean = False
          'Dim sdat(6) As Integer 'starting date
          'Dim edat(6) As Integer 'ending (or current) date
          lData.Attributes.SetValue("MVal", MissingVal)
          lData.Attributes.SetValue("MAcc", MissingAcc)
          Do
            If repeatsThisLine > maxRepeat Then
              maxRepeat = repeatsThisLine
              RedimensionRepeating(maxRepeat, ColHour, ColMinute, ColValue, ColFlag1, ColFlag2)
              PopulateColumns(curLine)
            End If
            For repeat = 0 To repeatsThisLine - 1
              Select Case Trim(ColValue(repeat).Value)
                Case "999999", "99999", "" ' skip missing values
                Case Else
                  If ColFlag1(repeat).Value <> "I" Then
                    If iVal > vals.GetUpperBound(0) Then
                      ReDim Preserve vals(iVal * 2)
                      ReDim Preserve dats(iVal * 2)
                    End If
                    If InMissing Then
                      vals(iVal) = MissingVal
                    Else
                      vals(iVal) = CDbl(ColValue(repeat).Value) / 100
                    End If
                    dats(iVal) = Jday(ColYear.Value, _
                                       ColMonth.Value, _
                                       ColDay.Value, _
                                       ColHour(repeat).Value, _
                                       ColMinute(repeat).Value, 0)
                    Select Case ColFlag1(repeat).Value
                      Case " " 'Usually flag is blank

                      Case "[", "{"
                        vals(iVal) = MissingVal
                        InMissing = True
                      Case "]", "}"
                        vals(iVal) = MissingVal
                        InMissing = False
                      Case "M", "D" 'Same as above Case "]", "}" ?
                        vals(iVal) = MissingVal
                        InMissing = False
                      Case "a"
                        InAccum = True
                      Case "A"
                        Select Case ColValue(repeat).Value
                          Case "000000", "099999"
                            InAccum = True
                          Case Else
                            InAccum = False
                        End Select
                    End Select
                    If InAccum Then vals(iVal) = MissingAcc
                    iVal = iVal + 1
                  End If
              End Select
            Next
            curLine = NextLine(inReader)
            PopulateColumns(curLine)
            repeatsThisLine = CInt(ColRepeats.Value)
          Loop
          Open = True
        Else
          Open = False
        End If
      Catch endEx As EndOfStreamException
        ReDim Preserve vals(iVal - 1)
        ReDim Preserve dats(iVal - 1)
        lData.Values = vals
        lDates.Values = dats
        lData.ValuesNeedToBeRead = False
        lDates.ValuesNeedToBeRead = False
        Open = True
      End Try
      inReader.Close()
      inBuffer.Close()
      inStream.Close()
    End If
  End Function

  Private Sub RedimensionRepeating(ByVal aMaximum As Integer, _
                                   ByRef ColHour() As ColDef, _
                                   ByRef ColMinute() As ColDef, _
                                   ByRef ColValue() As ColDef, _
                                   ByRef ColFlag1() As ColDef, _
                                   ByRef ColFlag2() As ColDef)

    Dim oldMaximum As Integer = ColHour.GetUpperBound(0)
    Dim repeatOffset As Integer
    ReDim Preserve ColHour(aMaximum)
    ReDim Preserve ColMinute(aMaximum)
    ReDim Preserve ColValue(aMaximum)
    ReDim Preserve ColFlag1(aMaximum)
    ReDim Preserve ColFlag2(aMaximum)

    If oldMaximum = 0 Then oldMaximum = -1 'need to add first column

    For repeat As Integer = oldMaximum + 1 To aMaximum
      repeatOffset = repeat * 12 '12 characters from 31 to 42 repeat
      ColHour(repeat) = AddColumn(31 + repeatOffset, 2, "Hour." & repeat)
      ColMinute(repeat) = AddColumn(33 + repeatOffset, 2, "Minute." & repeat)
      ColValue(repeat) = AddColumn(35 + repeatOffset, 6, "Value." & repeat)
      ColFlag1(repeat) = AddColumn(41 + repeatOffset, 1, "Flag1." & repeat)
      ColFlag2(repeat) = AddColumn(42 + repeatOffset, 1, "Flag2." & repeat)
    Next

  End Sub

  'Reads the next line from a text file whose lines end with carriage return and/or linefeed
  'Advances the position of the stream to the beginning of the next line
  'Returns Nothing if already at end of file
  Private Function NextLine(ByVal aReader As BinaryReader) As String
    Dim ch As Char
    Try
      NextLine = Nothing
ReadCharacter:
      ch = aReader.ReadChar
      Select Case ch
        Case vbCr 'Found end of line, consume linefeed if it is next
          If CInt(aReader.PeekChar) = CInt(10) Then aReader.ReadChar()
        Case vbLf 'Unix-style line ends without carriage return
        Case Else 'Found a character that does not end the line
          If NextLine Is Nothing Then
            NextLine = ch
          Else
            NextLine &= ch
          End If
          GoTo ReadCharacter
      End Select
    Catch endEx As EndOfStreamException
      If NextLine Is Nothing Then 'We had nothing to read, already finished file last time
        Throw endEx
      Else
        'Reaching the end of file is fine, we have finished reading this file
      End If
    End Try

  End Function

  'Set values in pColDefs from current row/line of file
  Private Function PopulateColumns(ByVal aRow As String) As Boolean
    If pColDefs Is Nothing Then Return False
    Dim rowLength As Integer = aRow.Length
    For Each de As DictionaryEntry In pColDefs
      Dim cd As ColDef = de.Value
      If cd.StartCol > rowLength Then
        cd.Value = ""
      ElseIf cd.StartCol + cd.Length > rowLength Then
        cd.Value = Mid(aRow, cd.StartCol)
      Else
        cd.Value = Mid(aRow, cd.StartCol, cd.Length)
      End If
    Next
  End Function

  Public Overrides Sub ReadData(ByVal aReadMe As atcData.atcDataSet)
    If Not Me.DataSets.Contains(aReadMe) Then
      System.Diagnostics.Debug.WriteLine("Cannot read data: not from this file")
    Else

      'Should not ever get here since we are now reading all data in Open

      'pReadAll = True
      'Open(Me.FileName)
      'pReadAll = False
    End If
  End Sub

  Private Class ColDef
    Public Name As String
    Public StartCol As Long
    Public Length As Integer
    Public Value As String

    Public Sub New(ByVal aStart As Integer, ByVal aWidth As Integer, ByVal aName As String)
      Me.StartCol = aStart
      Me.Length = aWidth
      Me.Name = aName
    End Sub


  End Class

  Public Overrides ReadOnly Property AvailableOperations() As atcData.atcDataGroup
    Get

    End Get
  End Property

End Class