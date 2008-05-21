Imports System.Net
Imports atcUtility

Public Class frmDownload

    Public Const CancelString As String = "<Cancel>"
    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pOk As Boolean = False

    Public Function AskUser(ByVal aMapWin As MapWindow.Interfaces.IMapWin) As String
        pMapWin = aMapWin

        'The following line hot-wires the form to just do met data download
        'chkBASINS_Met.Checked = True : cboRegion.SelectedIndex = 0 ': Me.Height = 141 ': Return Me.XML

        btnBrowseWDMmet.Tag = ""
        If Not pMapWin.Project Is Nothing Then
            If Not pMapWin.Project.FileName Is Nothing AndAlso pMapWin.Project.FileName.Length > 0 Then
                btnBrowseWDMmet.Tag = IO.Path.Combine(IO.Path.GetDirectoryName(pMapWin.Project.FileName), "met\met.wdm")
                btnBrowseWDMdailydischarge.Tag = IO.Path.Combine(IO.Path.GetDirectoryName(pMapWin.Project.FileName), "flow.wdm")
            End If
        End If

        Dim lHucIndex As Integer = HUC8Index()
        If lHucIndex >= 0 Then
            Dim lHUC8s As ArrayList = HUC8s()
            If lHUC8s.Count = 1 Then
                cboRegion.Items.Add("Hydrologic Unit " & lHUC8s(0))
            ElseIf lHUC8s.Count > 1 Then
                cboRegion.Items.Add("Hydrologic Units")
            End If
        End If

        Dim lRegionIndex As Integer = GetSetting("DataDownload", "Defaults", "RegionType", cboRegion.Items.Count - 1)
        If lRegionIndex < 0 OrElse lRegionIndex >= cboRegion.Items.Count Then
            lRegionIndex = 0
        End If
        If lRegionIndex < cboRegion.Items.Count Then cboRegion.SelectedIndex = lRegionIndex

        If GetSetting("DataDownload", "Defaults", "Clip", "").ToLower.Equals("true") Then
            chkClip.Checked = True
        End If

        If GetSetting("DataDownload", "Defaults", "Merge", "").ToLower.Equals("true") Then
            chkMerge.Checked = True
        End If

        If StationsXMLfromMap.Length = 0 Then
            panelNWISnoStations.Visible = True
            panelNWISnoStations.BringToFront()
            chkNWIS_GetNWISDischarge.Visible = False
            chkNWIS_GetNWISMeasurements.Visible = False
            chkNWIS_GetNWISWQ.Visible = False
        End If

        Me.ShowDialog()
        If pOk Then
            Return Me.XML
        Else
            Return CancelString
        End If
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pOk = False
        Me.Close()
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        SaveSetting("DataDownload", "Defaults", "RegionType", cboRegion.SelectedIndex)
        SaveSetting("DataDownload", "Defaults", "Clip", chkClip.Checked.ToString)
        SaveSetting("DataDownload", "Defaults", "Merge", chkMerge.Checked.ToString)

        pOk = True
        Me.Close()
    End Sub

    Public Function SelectedRegion() As D4EMDataManager.Region
        Dim lRegion As D4EMDataManager.Region
        Dim lPreferredFormat As String = "box"
        Try
            Dim lExtents As MapWinGIS.Extents = Nothing
            Select Case cboRegion.SelectedIndex
                Case 0 : lExtents = pMapWin.View.Extents
                Case 1 : lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                Case 2 : lExtents = pMapWin.Layers(HUC8Index).Extents : lPreferredFormat = "huc8"
            End Select
            If Not lExtents Is Nothing Then
                lRegion = New D4EMDataManager.Region(lExtents.yMax, lExtents.yMin, lExtents.xMin, lExtents.xMax, pMapWin.Project.ProjectProjection)
                lRegion.HUC8s = HUC8s()
                lRegion.PreferredFormat = lPreferredFormat
                Return lRegion.GetProjected(D4EMDataManager.SpatialOperations.GeographicProjection)
            End If
        Catch ex As Exception
            Dim lRegionForm As New frmSpecifyRegion
            lRegion = lRegionForm.AskUser()
            If Not lRegion Is Nothing Then
                Return lRegion.GetProjected(D4EMDataManager.SpatialOperations.GeographicProjection)
            End If
        End Try
        Return Nothing
    End Function

    Private Function StationsXMLfromMap() As String
        Dim lStationsXML As String = ""
        If pMapWin.View IsNot Nothing Then
            Dim lSelected As MapWindow.Interfaces.SelectInfo = pMapWin.View.SelectedShapes
            If lSelected.NumSelected > 0 Then
                Dim lLayer As MapWindow.Interfaces.Layer = pMapWin.Layers.Item(pMapWin.Layers.CurrentLayer)
                Dim lLayerShapefile As MapWinGIS.Shapefile = lLayer.GetObject
                Dim lKeyField As Integer
                For lKeyField = 0 To lLayerShapefile.NumFields - 1
                    Select Case lLayerShapefile.Field(lKeyField).Name
                        Case "site_no", "LocId" : Exit For 'TODO: use list of ID fields for layers from layers.dbf
                    End Select
                Next
                If lKeyField <= lLayerShapefile.NumFields Then
                    For lShapeIndex As Integer = 0 To lSelected.NumSelected - 1
                        lStationsXML &= "<stationid>" & lLayerShapefile.CellValue(lKeyField, lSelected.Item(lShapeIndex).ShapeIndex) & "</stationid>" & vbCrLf
                    Next
                End If
            End If
        End If
        Return lStationsXML
    End Function

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lDesiredProjection As String = ""
            Dim lRegionXML As String = ""
            Dim lRegion As D4EMDataManager.Region = Me.SelectedRegion
            If Not lRegion Is Nothing Then lRegionXML = lRegion.XML
            Dim lStationsXML As String = StationsXMLfromMap()

            Dim lCacheFolder As String = GetSetting("DataDownload", "defaults", "Cache_dir")
            If lCacheFolder.Length = 0 Then
                lCacheFolder = IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)) & IO.Path.DirectorySeparatorChar
            End If
            lCacheFolder = "<CacheFolder>" & lCacheFolder & "</CacheFolder>" & vbCrLf

            Dim lSaveFolder As String = ""
            If Not pMapWin.Project Is Nothing Then
                If Not pMapWin.Project.ProjectProjection Is Nothing AndAlso pMapWin.Project.ProjectProjection.Length > 0 Then
                    lDesiredProjection = "<DesiredProjection>" & pMapWin.Project.ProjectProjection & "</DesiredProjection>" & vbCrLf
                End If
                If Not pMapWin.Project.FileName Is Nothing AndAlso pMapWin.Project.FileName.Length > 0 Then
                    lSaveFolder &= "<SaveIn>" & IO.Path.GetDirectoryName(pMapWin.Project.FileName) & "</SaveIn>" & vbCrLf
                End If
            End If

            For Each lControl As Windows.Forms.Control In Me.Controls
                If lControl.Name.StartsWith("grp") AndAlso lControl.HasChildren Then
                    Dim lCheckedChildren As String = ""
                    For Each lChild As Windows.Forms.Control In lControl.Controls
                        If lChild.GetType.Name = "CheckBox" AndAlso CType(lChild, Windows.Forms.CheckBox).Checked Then
                            Dim lChildName As String = lChild.Name.Substring(lControl.Name.Length + 1)
                            If lChildName.ToLower.StartsWith("get") Then 'this checkbox has its own function name

                                Dim lWDMfilename As String = ""
                                If lChild Is chkNWIS_GetNWISDischarge AndAlso btnBrowseWDMdailydischarge.Tag.Length > 0 Then
                                    lWDMfilename = "<SaveWDM>" & btnBrowseWDMdailydischarge.Tag & "</SaveWDM>" & vbCrLf
                                End If

                                lXML &= "<function name='" & lChildName & "'>" & vbCrLf _
                                     & "<arguments>" & vbCrLf _
                                     & lSaveFolder _
                                     & lCacheFolder _
                                     & lDesiredProjection _
                                     & lRegionXML _
                                     & lStationsXML _
                                     & lWDMfilename _
                                     & "<clip>" & chkClip.Checked & "</clip>" & vbCrLf _
                                     & "<merge>" & chkMerge.Checked & "</merge>" & vbCrLf _
                                     & "</arguments>" & vbCrLf _
                                     & "</function>" & vbCrLf
                            Else 'This checkbox adds a data type to the parent function
                                lCheckedChildren &= "<DataType>" & lChildName & "</DataType>" & vbCrLf
                            End If
                        End If
                    Next
                    If lCheckedChildren.Length > 0 Then

                        Dim lWDMfilename As String = ""
                        If lCheckedChildren.Contains("<DataType>Met</DataType>") AndAlso btnBrowseWDMmet.Tag.Length > 0 Then
                            lWDMfilename = "<SaveWDM>" & btnBrowseWDMmet.Tag & "</SaveWDM>" & vbCrLf
                        End If

                        lXML &= "<function name='Get" & lControl.Name.Substring(3) & "'>" & vbCrLf _
                             & "<arguments>" & vbCrLf _
                             & lCheckedChildren _
                             & lSaveFolder _
                             & lCacheFolder _
                             & lDesiredProjection _
                             & lRegionXML _
                             & lStationsXML _
                             & lWDMfilename _
                             & "<clip>" & chkClip.Checked & "</clip>" & vbCrLf _
                             & "<merge>" & chkMerge.Checked & "</merge>" & vbCrLf _
                             & "<joinattributes>true</joinattributes>" & vbCrLf _
                             & "</arguments>" & vbCrLf _
                             & "</function>" & vbCrLf
                    End If
                End If
            Next
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Function HUC8Index() As Integer
        If Not pMapWin Is Nothing AndAlso Not pMapWin.Layers Is Nothing Then
            Dim lIndex As Integer = 0
            While lIndex < pMapWin.Layers.NumLayers
                Try
                    If pMapWin.Layers(lIndex).FileName.ToLower.EndsWith(IO.Path.DirectorySeparatorChar & "cat.shp") Then
                        Return lIndex
                    End If
                Catch ex As Exception
                End Try
                lIndex += 1
            End While
        End If
        Return -1
    End Function

    Private Function HUC8s() As ArrayList
        'First check for a cat layer that contains the list of HUC-8s
        Dim lHUC8s As New ArrayList
        If pMapWin IsNot Nothing AndAlso pMapWin.Project IsNot Nothing AndAlso pMapWin.Project.FileName IsNot Nothing Then
            Dim lCatDbfName As String = IO.Path.Combine(IO.Path.GetDirectoryName(pMapWin.Project.FileName), "cat.dbf")
            If IO.File.Exists(lCatDbfName) Then
                Dim lCatDbf As New atcUtility.atcTableDBF
                lCatDbf.OpenFile(lCatDbfName)
                Dim lHucField As Integer = lCatDbf.FieldNumber("CU")
                If lHucField > 0 Then
                    For lRecord As Integer = 1 To lCatDbf.NumRecords
                        lCatDbf.CurrentRecord = lRecord
                        lHUC8s.Add(lCatDbf.Value(lHucField))
                    Next
                End If
            End If
        End If
        Return lHUC8s
    End Function

    '''' <summary>
    '''' Synchronous HTTP download
    '''' </summary>
    'Private Function URLtoString(ByVal Url As String, Optional ByVal aCookieContainer As CookieContainer = Nothing) As String
    '    Dim BytesProcessed As Integer = 0
    '    Debug.Assert(Url.StartsWith("http://"))
    '    Dim DownloadStartTime As Date = DateTime.Now

    '    ' create content stream from memory or file
    '    Dim ContentStream As New Text.StringBuilder

    '    ' Create the request object.
    '    Dim request As HttpWebRequest = HttpWebRequest.Create(Url)
    '    request.CookieContainer = aCookieContainer        
    '    Dim response As HttpWebResponse = request.GetResponse()
    '    ' only if server responds 200 OK
    '    If (response.StatusCode = HttpStatusCode.OK) Then
    '        Dim readBuffer As Byte()
    '        ReDim readBuffer(2048)
    '        Dim responseStream As IO.Stream = response.GetResponseStream()
    '        Dim iByte As Integer
    '        While (True)
    '            Dim bytesRead As Integer = responseStream.Read(readBuffer, 0, readBuffer.Length)
    '            If (bytesRead <= 0) Then Exit While
    '            iByte = 0
    '            While iByte < bytesRead
    '                ContentStream.Append(Chr(readBuffer(iByte)))
    '                iByte += 1
    '            End While
    '            BytesProcessed += bytesRead
    '        End While
    '    End If
    '    Return ContentStream.ToString
    'End Function

    'Private Function DownloadGeocache() As String
    '    Dim cookieContainer As New System.Net.CookieContainer()
    '    ' create the web request
    '    Dim req As HttpWebRequest = HttpWebRequest.Create("http://www.geocaching.com/login/default.aspx")
    '    req.CookieContainer = cookieContainer
    '    req.Method = "POST"
    '    ' create post data
    '    Dim bytePostData As Byte() = System.Text.Encoding.GetEncoding(1252).GetBytes("myUsername=&myPassword=&cookie=on&Button1=Login")
    '    ' create stream object
    '    Dim streamPostData As System.IO.Stream = req.GetRequestStream()
    '    ' write to the stream
    '    streamPostData.Write(bytePostData, 0, bytePostData.Length)
    '    ' close the stream
    '    streamPostData.Close()
    '    ' wait for the response
    '    Dim resp As HttpWebResponse = req.GetResponse()
    '    ' initialize header collection
    '    Dim coll As WebHeaderCollection = resp.Headers
    '    ' get site minder cookie data
    '    Dim sessionId As String = coll.Get("Set-Cookie")
    '    ' get current location
    '    Dim location As String = coll.Get("Location")
    '    ' close response stream
    '    resp.GetResponseStream().Close()
    '    ' create new cookie container object
    '    ' add cookie headers to cookie container
    '    'cookieContainer.SetCookies(New System.Uri("http://www.geocaching.com/"), sessionId)

    '    ' Print the properties of each cookie.
    '    Dim cook As Cookie
    '    For Each cook In resp.Cookies
    '        Console.WriteLine("Cookie:")
    '        Console.WriteLine("{0} = {1}", cook.Name, cook.Value)
    '        Console.WriteLine("Domain: {0}", cook.Domain)
    '        Console.WriteLine("Path: {0}", cook.Path)
    '        Console.WriteLine("Port: {0}", cook.Port)
    '        Console.WriteLine("Secure: {0}", cook.Secure)

    '        Console.WriteLine("When issued: {0}", cook.TimeStamp)
    '        Console.WriteLine("Expires: {0} (expired? {1})", cook.Expires, cook.Expired)
    '        Console.WriteLine("Don't save: {0}", cook.Discard)
    '        Console.WriteLine("Comment: {0}", cook.Comment)
    '        Console.WriteLine("Uri for comments: {0}", cook.CommentUri)
    '        Console.WriteLine("Version: RFC {0}", IIf(cook.Version = 1, "2109", "2965"))

    '        ' Show the string representation of the cookie.
    '        Console.WriteLine("String: {0}", cook.ToString())
    '    Next cook

    '    Return URLtoString("http://www.geocaching.com/seek/nearest.aspx?lat=33.775&lon=-84.295", cookieContainer)

    'End Function

    Private Sub btnBrowseWDM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseWDMdailydischarge.Click, _
                                                                                                       btnBrowseWDMmet.Click
        Dim lDialog As New Windows.Forms.SaveFileDialog
        With lDialog
            .Title = "Save in WDM file..."
            .FileName = sender.tag
            .Filter = "*.wdm|*.wdm|*.*|*.*"
            .DefaultExt = ".wdm"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                sender.tag = .FileName
            Else
                sender.tag = ""
            End If
        End With
    End Sub

    Private Sub btnOther_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOther.Click
        '    Me.Visible = False
        '    Dim lDirectory As String
        '    If pMapWin.Project Is Nothing Then
        '        lDirectory = CurDir()
        '    Else
        '        lDirectory = IO.Path.GetDirectoryName(pMapWin.Project.FileName) & "\"
        '    End If
        '    BASINS.DownloadNewData(lDirectory)
        '    Me.Close()
    End Sub

    Private Sub frmDownload_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Project Creation and Management\GIS and Time-Series Data\Download.html")
        End If
    End Sub
End Class