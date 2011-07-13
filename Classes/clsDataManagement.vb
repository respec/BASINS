Imports System.Xml
Imports System.Drawing

'********************************************************************************************************
'File Name: clsDataManagement.vb
'Description: "Data Management" related functions such as checking for metadata, moving and deleting shapefiles, etc.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Public Class DataManagement
    'TODO: DeleteShapeFile() {delete all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'TODO: MoveShapeFile() {move all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'TODO: RenameShapeFile() {rename all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'NOTE: These are all implemented in GeoProc already

    Public Shared Function MetaDataExists(ByVal FileName As String) As Boolean
        If GetMetaDataFiles(FileName) Is Nothing Then Return False
        Return True
    End Function

    Public Shared Function GetMetaDataFiles(ByVal FileName As String) As String()
        Try
            Dim tStr As String
            Dim list As New ArrayList

            'if this is a directory (as in the case  of ESRI Grids)
            'chop off the '\' character
            If FileName.Chars(FileName.Length - 1) = "\"c Then
                FileName = FileName.Substring(0, FileName.Length - 1)
            End If

            ' try just adding the extension to test 
            tStr = FileName & ".htm"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            tStr = FileName & ".html"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            tStr = FileName & ".xml"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            'now try chopping off the original extension, and then adding the one we want to test
            Dim ext As String = System.IO.Path.GetExtension(FileName)
            FileName = FileName.Substring(0, FileName.Length - ext.Length)

            tStr = FileName & ".htm"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            tStr = FileName & ".html"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            tStr = FileName & ".xml"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            If list.Count = 0 Then
                Return Nothing
            Else
                Return CType(list.ToArray(GetType(String)), String())
            End If
        Catch
            Return Nothing
        End Try
    End Function
    '''<summary>
    '''Chris Michaelis July 2 2005
    '''Used to compare write times between the first and second file.
    '''Returns true if File2 is newer than File1
    ''' </summary>
    Public Shared Function CheckFile2Newest(ByVal File1 As String, ByVal File2 As String, Optional ByVal SameIfWithinXMinutes As Single = 2) As Boolean
        If System.IO.File.Exists(File1) And System.IO.File.Exists(File2) Then
            ' Allow a variance of up to 3 minutes. Slower systems will have the image
            ' write date a bit after the grid write date. If it takes longer than 3 minutes
            ' it's either a very large grid or a very slow computer - either way, the additional
            ' delay is noncritical.
            Dim f1_d As Date = System.IO.File.GetLastWriteTime(File1)
            Dim f2_d As Date = System.IO.File.GetLastWriteTime(File2)

            If f1_d.Equals(f2_d) Then Return True
            If Math.Abs(f1_d.Subtract(f2_d).TotalMinutes) < SameIfWithinXMinutes Then Return True

            'If the image was made *after* the grid this is OK as well.
            If (f1_d.Subtract(f2_d).TotalMinutes < 0) Then Return True

            Return False
        Else
            ' They don't match if they don't exist...
            Return False
        End If
    End Function

#Region "Color schemes"
    ''' <summary>
    ''' Imports grid color scheme from specified file name
    ''' </summary>
    Friend Function ImportScheme(ByVal Filename As String) As Object
        Dim doc As New XmlDocument
        Dim root As XmlElement = Nothing

        Try
            Dim sch As New MapWinGIS.GridColorScheme
            If root.Attributes("SchemeType").InnerText = "Grid" Then
                If ImportScheme(sch, root.Item("GridColoringScheme")) Then
                    Return sch
                End If
            Else
                MapWinUtility.Logger.Msg("File contains invalid coloring scheme type.")
                Return Nothing
            End If

        Catch ex As Exception
            MapWinUtility.Logger.Msg("Failed to import color scheme")
            Return Nothing
        End Try
        Return Nothing


    End Function

    ''' <summary>
    ''' Imports grid color scheme from specifed XML node
    ''' </summary>
    Private Function ImportScheme(ByRef sch As MapWinGIS.GridColorScheme, ByVal e As XmlElement) As Boolean
        Dim i As Integer
        Dim brk As MapWinGIS.GridColorBreak
        Dim t As String
        Dim azimuth, elevation As Double
        Dim n As XmlNode

        If e Is Nothing Then Return False

        sch.Key = e.Attributes("Key").InnerText
        t = e.Attributes("AmbientIntensity").InnerText
        sch.AmbientIntensity = IIf(IsNumeric(t), CDbl(t), 0.7)
        t = e.Attributes("LightSourceAzimuth").InnerText
        azimuth = IIf(IsNumeric(t), CDbl(t), 90)
        t = e.Attributes("LightSourceElevation").InnerText
        elevation = IIf(IsNumeric(t), CDbl(t), 45)
        sch.SetLightSource(azimuth, elevation)
        t = e.Attributes("LightSourceIntensity").InnerText
        sch.LightSourceIntensity = IIf(IsNumeric(t), CDbl(t), 0.7)

        For i = 0 To e.ChildNodes.Count - 1
            n = e.ChildNodes(i)
            brk = New MapWinGIS.GridColorBreak
            brk.Caption = n.Attributes("Caption").InnerText
            brk.LowColor = MapWinUtility.Colors.ColorToUInteger(Color.FromArgb(n.Attributes("LowColor").InnerText))
            brk.HighColor = MapWinUtility.Colors.ColorToUInteger(Color.FromArgb(n.Attributes("HighColor").InnerText))
            brk.LowValue = n.Attributes("LowValue").InnerText
            brk.HighValue = n.Attributes("HighValue").InnerText
            brk.ColoringType = n.Attributes("ColoringType").InnerText
            brk.GradientModel = n.Attributes("GradientModel").InnerText
            sch.InsertBreak(brk)
        Next
        Return True
    End Function

    ''' <summary>
    ''' Exports grid color scheme to the file with sprcified name
    ''' </summary>
    Friend Function ExportScheme(ByVal Path As String, ByVal Scheme As MapWinGIS.GridColorScheme) As Boolean
        Dim doc As New XmlDocument
        Dim mainScheme, root As XmlElement
        Dim schemeType As XmlAttribute
        root = doc.CreateElement("ColoringScheme")

        Dim AmbientIntensity, Key, LightSourceAzimuth As XmlAttribute
        Dim LightSourceElevation, LightSourceIntensity, NoDataColor As XmlAttribute

        If Scheme Is Nothing OrElse Scheme.NumBreaks = 0 Then Return False
        schemeType = doc.CreateAttribute("SchemeType")
        schemeType.InnerText = "Grid"
        root.Attributes.Append(schemeType)
        AmbientIntensity = doc.CreateAttribute("AmbientIntensity")
        Key = doc.CreateAttribute("Key")
        LightSourceAzimuth = doc.CreateAttribute("LightSourceAzimuth")
        LightSourceElevation = doc.CreateAttribute("LightSourceElevation")
        LightSourceIntensity = doc.CreateAttribute("LightSourceIntensity")
        NoDataColor = doc.CreateAttribute("NoDataColor")
        AmbientIntensity.InnerText = Scheme.AmbientIntensity
        Key.InnerText = Scheme.Key
        LightSourceAzimuth.InnerText = Scheme.LightSourceAzimuth
        LightSourceElevation.InnerText = Scheme.LightSourceElevation
        LightSourceIntensity.InnerText = Scheme.LightSourceIntensity
        NoDataColor.InnerText = MapWinUtility.Colors.IntegerToColor(Scheme.NoDataColor).ToArgb

        mainScheme = doc.CreateElement("GridColoringScheme")
        mainScheme.Attributes.Append(AmbientIntensity)
        mainScheme.Attributes.Append(Key)
        mainScheme.Attributes.Append(LightSourceAzimuth)
        mainScheme.Attributes.Append(LightSourceElevation)
        mainScheme.Attributes.Append(LightSourceIntensity)
        mainScheme.Attributes.Append(NoDataColor)
        root.AppendChild(mainScheme)
        doc.AppendChild(root)
        If ExportScheme(Scheme, doc, mainScheme) Then
            doc.Save(Path)
            Return True
        Else
            MapWinUtility.Logger.Msg("Failed to export coloring scheme.", MsgBoxStyle.Exclamation, "Error")
            Return False
        End If
    End Function

    ''' <summary>
    ''' Exports grid color scheme to specified XML node
    ''' </summary>
    Public Function ExportScheme(ByVal Scheme As MapWinGIS.GridColorScheme, ByVal RootDoc As XmlDocument, ByVal Parent As XmlElement) As Boolean
        Dim i As Integer
        Dim brk As XmlElement
        Dim caption As XmlAttribute
        Dim sValue As XmlAttribute
        Dim eValue As XmlAttribute
        Dim sColor As XmlAttribute
        Dim eColor As XmlAttribute
        Dim coloringType As XmlAttribute
        Dim gradientModel As XmlAttribute
        Dim curBrk As MapWinGIS.GridColorBreak

        If Scheme Is Nothing OrElse Scheme.NumBreaks = 0 Then Return False

        For i = 0 To Scheme.NumBreaks - 1
            curBrk = Scheme.Break(i)
            brk = RootDoc.CreateElement("Break")
            caption = RootDoc.CreateAttribute("Caption")
            sValue = RootDoc.CreateAttribute("LowValue")
            eValue = RootDoc.CreateAttribute("HighValue")
            sColor = RootDoc.CreateAttribute("LowColor")
            eColor = RootDoc.CreateAttribute("HighColor")
            coloringType = RootDoc.CreateAttribute("ColoringType")
            gradientModel = RootDoc.CreateAttribute("GradientModel")
            caption.InnerText = curBrk.Caption
            sValue.InnerText = curBrk.LowValue
            eValue.InnerText = curBrk.HighValue
            sColor.InnerText = MapWinUtility.Colors.IntegerToColor(curBrk.LowColor).ToArgb
            eColor.InnerText = MapWinUtility.Colors.IntegerToColor(curBrk.HighColor).ToArgb
            coloringType.InnerText = curBrk.ColoringType
            gradientModel.InnerText = curBrk.GradientModel
            brk.Attributes.Append(caption)
            brk.Attributes.Append(sValue)
            brk.Attributes.Append(eValue)
            brk.Attributes.Append(sColor)
            brk.Attributes.Append(eColor)
            brk.Attributes.Append(coloringType)
            brk.Attributes.Append(gradientModel)
            Parent.AppendChild(brk)
            curBrk = Nothing
        Next
        Return True
    End Function
#End Region
End Class
