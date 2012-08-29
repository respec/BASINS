Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Xml

Public Class atcTimeseriesSTORET
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "STORET Output Files (*.STORETresults)|*.STORETresults"
    Private pNaN As Double = GetNaN()

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "STORET Water Quality"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::STORET"
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
        If MyBase.Open(aFileName, aAttributes) Then
            Dim lDocument As New XmlDocument
            Dim lXMLstr As String
            Try
                lXMLstr = IO.File.ReadAllText(Specification).Replace(Chr(182), Chr(9)) 'replace strange delimiter with tab
                lDocument.LoadXml(lXMLstr)
            Catch ex As Exception
                Logger.Msg(Specification & vbCrLf & ex.Message, "Could not open STORET file")
                Return False
            End Try
            Dim lResultsNode As XmlNode = lDocument.ChildNodes(1)
            Dim lNode As XmlNode

            Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
            Dim lTSBuilder As atcData.atcTimeseriesBuilder
            Dim lFieldStart As Integer = 1

            For Each lOrganizationNode As XmlNode In lResultsNode.ChildNodes
                Dim lOrganizationIdentifier As String = ""
                Dim lOrganizationFormalName As String = ""
                For Each lOrgChild As XmlNode In lOrganizationNode.ChildNodes
                    Select Case lOrgChild.Name
                        Case "OrganizationDescription"
                            For Each lNode In lOrgChild.ChildNodes
                                Select Case lNode.Name
                                    Case "OrganizationIdentifier" : lOrganizationIdentifier = lNode.InnerText.Trim
                                    Case "OrganizationFormalName" : lOrganizationFormalName = lNode.InnerText.Trim
                                End Select
                            Next
                        Case "Activity"
                            Dim lActivityAttributes As New atcDataAttributes
                            SetAttribute(lActivityAttributes, "OrganizationIdentifier", lOrganizationIdentifier)
                            SetAttribute(lActivityAttributes, "OrganizationFormalName", lOrganizationFormalName)
                            For Each lActivityNode As XmlNode In lOrgChild.ChildNodes
                                Select Case lActivityNode.Name
                                    Case "ActivityDescription", "MonitoringLocation"
                                        For Each lNode In lActivityNode.ChildNodes
                                            SetAttribute(lActivityAttributes, lNode.Name, lNode.InnerText.Trim)
                                        Next
                                    Case "Result"
                                        Dim lDateStr As String = "No Date"
                                        Dim lDate As Double = 0
                                        If lActivityAttributes.ContainsAttribute("ActivityStartDate") Then
                                            Try
                                                lDateStr = lActivityAttributes.GetValue("ActivityStartDate")
                                                lDate = Date.Parse(lDateStr).ToOADate
                                            Catch
                                                Logger.Dbg("Could not parse STORET date " & lActivityAttributes.GetValue("ActivityStartDate"))
                                            End Try
                                        End If
                                        Dim lValueStr As String = ""
                                        Dim lResultAttributes As New atcDataAttributes
                                        'Currently only placing one location in each file, but include it in key just in case
                                        Dim lKey As String = lActivityAttributes.GetValue("MonitoringLocationIdentifier", "") & ":"

                                        ProcessResultNodeChildren(lActivityNode, lActivityAttributes, lKey, lValueStr)

                                        lTSBuilder = lTSBuilders.Builder(lKey)
                                        If lTSBuilder.NumValues = 0 Then 'Set attributes of new builder
                                            lTSBuilder.Attributes.ChangeTo(lActivityAttributes)
                                            lTSBuilder.Attributes.SetValue("Key", lKey)
                                        End If
                                        If lValueStr.Length = 0 Then
                                            Logger.Dbg("No STORET value found. date " & lDateStr & " for " & lKey & " in " & Specification)
                                            lTSBuilder.AddValue(lDate, pNaN)
                                        Else
                                            Dim lValue As Double
                                            If Not Double.TryParse(lValueStr, lValue) Then
                                                Logger.Dbg("Could not parse STORET value '" & lValueStr & "' date " & lDateStr & " for " & lKey & " in " & Specification)
                                                lValue = pNaN
                                            End If
                                            lTSBuilder.AddValue(lDate, lValue)
                                        End If
                                        lTSBuilder.AddValueAttributes(lResultAttributes)
                                End Select
                            Next
                    End Select
                Next
            Next

            'Logger.Dbg("Created Builders")
            Logger.Progress("Creating Timeseries", 0, 0)
            lTSBuilders.CreateTimeseriesAddToGroup(Me.DataSets)
            For Each lDataSet As atcData.atcTimeseries In Me.DataSets
                With lDataSet.Attributes
                    Dim lKeyParts() As String = .GetValue("Key").Split(":")
                    .SetValue("Scenario", "Observed")
                    .SetValue("Location", lKeyParts(0))
                    Try
                        .SetValue("Constituent", lKeyParts(1))
                        .SetValue("Units", lKeyParts(2))
                    Catch
                    End Try
                End With
            Next
            Logger.Status("")
            Return True
        Else
            Logger.Dbg("atcDataSource could not open " & Specification)
            Return False
        End If
    End Function

    Private Sub ProcessResultNodeChildren(ByVal aNode As XmlNode, ByVal aResultAttributes As atcDataAttributes, ByRef aKey As String, ByRef aValueStr As String)
        For Each lNode As XmlNode In aNode.ChildNodes 'ResultDescription and ResultLabInformation
            If lNode.HasChildNodes AndAlso lNode.FirstChild.Name <> "#text" Then
                ProcessResultNodeChildren(lNode, aResultAttributes, aKey, aValueStr)
            Else
                SetAttribute(aResultAttributes, lNode.Name, lNode.InnerText.Trim)
                Select Case lNode.Name
                    Case "ResultMeasureValue", "MeasureValue"
                        If lNode.InnerText.Trim.Length > 0 Then
                            If aValueStr.Length > 0 Then
                                Logger.Dbg("Second value found. First was '" & aValueStr & "' second is '" & lNode.InnerText.Trim & "'")
                            End If
                            aValueStr = lNode.InnerText.Trim
                        End If
                    Case "CharacteristicName", "MeasureUnitCode"
                        aKey &= lNode.InnerText.Trim & ":"
                End Select
            End If
        Next
    End Sub

    Private Sub SetAttribute(ByVal aAttributes As atcDataAttributes, ByVal aAttributeName As String, ByVal aNewValue As String)
        If aNewValue IsNot Nothing AndAlso aNewValue.Length > 0 Then
            aAttributes.SetValue(aAttributeName, aNewValue)
        End If
    End Sub
End Class
