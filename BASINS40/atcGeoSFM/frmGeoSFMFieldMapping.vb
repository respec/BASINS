Imports atcMwGisUtility
Imports atcUtility
Imports MapWinUtility

Public Class frmGeoSFMFieldMapping

    Dim pStreamsLayerIndex As Integer
    Dim pSegmentFieldMap As New atcUtility.atcCollection
    Dim pfrmGeoSFM As frmGeoSFM

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'set field mapping as specified in field mapping control
        pSegmentFieldMap.Clear()
        For lIndex As Integer = 0 To AtcConnectFields.lstConnections.Items.Count - 1
            Dim lTxt As String = AtcConnectFields.lstConnections.Items(lIndex)
            Dim lBaseLen As Integer = 0
            Dim lBaseName As String = ""
            If Mid(lTxt, 1, 7) = "Segment" Then
                lBaseLen = 7
                lBaseName = "Segment"
            End If
            Dim lSpacePos As Integer = InStr(lTxt, " ")
            Dim lGTPos As Integer = InStr(lTxt, ">")
            Dim lSrc As String = Mid(lTxt, lBaseLen + 2, lSpacePos - lBaseLen - 2)

            If Mid(lTxt, lGTPos + 2, lBaseLen) = lBaseName Then
                Dim lTar As String = Mid(lTxt, lGTPos + lBaseLen + 3)
                If Mid(lTxt, 1, 7) = "Segment" Then
                    pSegmentFieldMap.Add(lSrc, lTar)
                End If
            End If
        Next

        'pfrmGeoSFM.pPlugIn.WASPProject.SegmentFieldMap = pSegmentFieldMap

        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Public Sub Init(ByVal aStreamsLayerIndex As Integer, ByVal aSegmentFieldMap As atcCollection, ByVal afrmGeoSFM As frmGeoSFM)
        pStreamsLayerIndex = aStreamsLayerIndex
        pSegmentFieldMap = aSegmentFieldMap
        pfrmGeoSFM = afrmGeoSFM

        Logger.Dbg("SetFieldMappingControl Begin")

        'add source fields from dbf
        AtcConnectFields.lstSource.Items.Clear()
        If aStreamsLayerIndex > -1 Then
            For lFieldIndex As Integer = 0 To GisUtil.NumFields(aStreamsLayerIndex) - 1
                AtcConnectFields.lstSource.Items.Add("Segment:" & GisUtil.FieldName(lFieldIndex, aStreamsLayerIndex))
            Next
        End If

        'add target properties from introspection on the swmm classes
        AtcConnectFields.lstTarget.Items.Clear()
        Dim lSegment As New atcWASP.atcWASPSegment
        For Each lField As Reflection.FieldInfo In lSegment.GetType.GetFields
            AtcConnectFields.lstTarget.Items.Add("Segment:" & lField.Name)
        Next

        'add existing connections from default field maps
        AtcConnectFields.lstConnections.Items.Clear()
        Dim lConn As String
        Dim lType As String = "Segment"
        For lIndex As Integer = 0 To pSegmentFieldMap.Count - 1
            lConn = lType & ":" & pSegmentFieldMap.Keys(lIndex) & " <-> " & lType & ":" & pSegmentFieldMap(lIndex)
            AtcConnectFields.AddConnection(lConn, True)
        Next
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

End Class