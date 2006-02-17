Imports atcData
Imports MapWinUtility

Public Class Variation

  Public Name As String = "<untitled>"
  Public DataSets As atcDataGroup
  Public ComputationSource As atcDataSource
  Public Operation As String = ""
  Public Min As Double = Double.NaN
  Public Max As Double = Double.NaN
  Public Increment As Double = Double.NaN
  Public CurrentValue As Double = Double.NaN

  Public ColorAboveMax As System.Drawing.Color = System.Drawing.Color.OrangeRed
  Public ColorBelowMin As System.Drawing.Color = System.Drawing.Color.DeepSkyBlue
  Public ColorDefault As System.Drawing.Color = System.Drawing.Color.White

  Public Function Clone() As Variation
    Dim newVariation As New Variation
    With newVariation
      .Name = Name
      .DataSets = DataSets
      .ComputationSource = ComputationSource
      .Operation = Operation.Clone
      .Min = Min
      .Max = Max
      .Increment = Increment
      .CurrentValue = CurrentValue
      .ColorAboveMax = ColorAboveMax
      .ColorBelowMin = ColorBelowMin
      .ColorDefault = ColorDefault
    End With
    Return newVariation
  End Function

  Private Property DataSetsXML() As String
    Get
      If DataSets Is Nothing Then
        Return ""
      Else
        Dim lXML As String = "  <DataSets count='" & DataSets.Count & "'>" & vbCrLf
        For Each lDataSet As atcDataSet In DataSets
          lXML &= "    <DataSet"
          lXML &= " ID='" & lDataSet.Attributes.GetValue("ID") & "'"
          lXML &= " Location='" & lDataSet.Attributes.GetValue("Location") & "'"
          lXML &= " Constituent='" & lDataSet.Attributes.GetValue("Constituent") & "'"
          lXML &= ">" & vbCrLf
        Next
        Return lXML & "  </DataSets>" & vbCrLf
      End If

    End Get
    Set(ByVal Value As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(Value) Then
        DataSets = New atcDataGroup
        If lXML.FirstChild2() Then
          Do
            Dim lID As String = lXML.GetAttrValue("ID")
            If lID.Length > 0 Then
              Dim lDataGroup As atcDataGroup = g_DataManager.DataSets.FindData("ID", lID, 1)
              If lDataGroup.Count > 0 Then
                DataSets.Add(lDataGroup.ItemByIndex(0))
              Else
                Logger.Msg("No data loaded with ID " & lID, "Variation from XML")
              End If
            Else
              Logger.Msg("No data set ID found in XML", "Variation from XML")
            End If
          Loop While lXML.NextSibling2
        End If
      End If
    End Set
  End Property

  Public Property XML() As String
    Get
      Return "<Variation>" & vbCrLf _
           & "  <Name>" & Name & "</Name>" & vbCrLf _
           & "  <Min>" & Min & "</Min>" & vbCrLf _
           & "  <Max>" & Max & "</Max>" & vbCrLf _
           & "  <Increment>" & Increment & "</Increment>" & vbCrLf _
           & "  <Operation>" & Operation & "</operation>" & vbCrLf _
           & "  <ComputationSource>" & ComputationSource.Name & "</ComputationSource>" & vbCrLf _
           & DataSetsXML _
           & "</Variation>" & vbCrLf
    End Get
    Set(ByVal Value As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(Value) Then
        If lXML.FirstChild2() Then
          Do
            With lXML
              Select Case .Tag.ToLower
                Case "name" : Name = .Content
                Case "min" : Min = CDbl(.Content)
                Case "max" : Max = CDbl(.Content)
                Case "increment" : Increment = CDbl(.Content)
                Case "operation" : Operation = .Content
                Case "computationsource"
                  ComputationSource = g_DataManager.DataSourceByName(.Content)
                Case "datasets" : DataSetsXML = .GetXml
              End Select
            End With
          Loop While lXML.NextSibling2
        End If
      End If

    End Set
  End Property

  Public Overrides Function ToString() As String
    Dim retStr As String = Name & " " & Operation
    If Not Double.IsNaN(Min) Then retStr &= " from " & Format(Min, "0.0")
    If Not Double.IsNaN(Max) Then retStr &= " to " & Format(Max, "0.0")
    If Not Double.IsNaN(Increment) Then retStr &= " step " & Format(Increment, "0.0")
    Return retStr
  End Function
End Class
