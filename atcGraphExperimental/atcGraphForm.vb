Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports ZedGraph

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports System.Web.Script.Serialization
Imports System.Xml
Imports System.Xml.Linq
Imports System.Runtime.Serialization.Json
'Imports System.Runtime.InteropServices

Public Class atcGraphForm
    Inherits Form

    'Form object that contains graph(s)
    Private pMaster As ZedGraph.MasterPane
    Private pAuxEnabled As Boolean = False
    Public AuxFraction As Single = 0.2

    'Graph editing form
    Private WithEvents pEditor As frmGraphEditor ' ZedGraph.frmEdit 'frmGraphEdit

    Private WithEvents pZgc As ZedGraphControl

    Private pGrapher As clsGraphBase

    Private Shared SaveImageExtension As String = ".png"
    Friend WithEvents mnuViewVerticalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewHorizontalZoom As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopyMetafile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCoordinates As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewZoomAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSaveJson As MenuItem
    Friend WithEvents mnuApplyJson As MenuItem
    Friend WithEvents mnuCoordinatesOnMenuBar As System.Windows.Forms.MenuItem

    Public Property Grapher() As clsGraphBase
        Get
            Return pGrapher
        End Get
        Set(ByVal newValue As clsGraphBase)
            pGrapher = newValue
            RefreshGraph()
        End Set
    End Property

    Private Sub InitMasterPane()
        'InitMatchingColors(FindFile("", "GraphColors.txt")) 'Becky moved this here from atcGraph10/CreateZgc so that
        'the file is found ONCE instead of a gazillion times and the colors are initialized ONCE rather than a gazillion
        'times
        pZgc = CreateZgc()
        Me.Controls.Add(pZgc)
        With pZgc
            .Dock = DockStyle.Fill
            .IsEnableHZoom = mnuViewHorizontalZoom.Checked
            .IsEnableHPan = mnuViewHorizontalZoom.Checked
            .IsEnableVZoom = mnuViewVerticalZoom.Checked
            .IsEnableVPan = mnuViewVerticalZoom.Checked
            '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
            pMaster = .MasterPane
        End With

        RefreshGraph()
    End Sub

    Public Property AuxAxisEnabled() As Boolean
        Get
            Return pAuxEnabled
        End Get
        Set(ByVal aEnable As Boolean)
            If pAuxEnabled <> aEnable Then
                pAuxEnabled = aEnable
                EnableAuxAxis(pMaster, aEnable, AuxFraction)
                RefreshGraph()
            End If
        End Set
    End Property

#Region " Windows Form Designer generated code "

    <CLSCompliant(False)>
    Public Sub New()
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)

        InitMasterPane()

        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFilePrint As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditGraph As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem

    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(atcGraphForm))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem()
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem()
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem()
        Me.mnuSaveJson = New System.Windows.Forms.MenuItem()
        Me.mnuFileSave = New System.Windows.Forms.MenuItem()
        Me.mnuFilePrint = New System.Windows.Forms.MenuItem()
        Me.mnuEdit = New System.Windows.Forms.MenuItem()
        Me.mnuEditGraph = New System.Windows.Forms.MenuItem()
        Me.mnuEditSep1 = New System.Windows.Forms.MenuItem()
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem()
        Me.mnuEditCopyMetafile = New System.Windows.Forms.MenuItem()
        Me.mnuView = New System.Windows.Forms.MenuItem()
        Me.mnuViewVerticalZoom = New System.Windows.Forms.MenuItem()
        Me.mnuViewHorizontalZoom = New System.Windows.Forms.MenuItem()
        Me.mnuViewZoomAll = New System.Windows.Forms.MenuItem()
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem()
        Me.mnuCoordinates = New System.Windows.Forms.MenuItem()
        Me.mnuCoordinatesOnMenuBar = New System.Windows.Forms.MenuItem()
        Me.mnuHelp = New System.Windows.Forms.MenuItem()
        Me.mnuApplyJson = New System.Windows.Forms.MenuItem()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuCoordinates, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSep1, Me.mnuSaveJson, Me.mnuApplyJson, Me.mnuFileSave, Me.mnuFilePrint})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 1
        Me.mnuFileSep1.Text = "-"
        '
        'mnuSaveJson
        '
        Me.mnuSaveJson.Index = 2
        Me.mnuSaveJson.Text = "Save Specs"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 4
        Me.mnuFileSave.Text = "Save As..."
        '
        'mnuFilePrint
        '
        Me.mnuFilePrint.Index = 5
        Me.mnuFilePrint.Text = "Print"
        '
        'mnuEdit
        '
        Me.mnuEdit.Index = 1
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditGraph, Me.mnuEditSep1, Me.mnuEditCopy, Me.mnuEditCopyMetafile})
        Me.mnuEdit.Text = "Edit"
        '
        'mnuEditGraph
        '
        Me.mnuEditGraph.Index = 0
        Me.mnuEditGraph.Text = "Graph"
        '
        'mnuEditSep1
        '
        Me.mnuEditSep1.Index = 1
        Me.mnuEditSep1.Text = "-"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Index = 2
        Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.mnuEditCopy.Text = "Copy"
        '
        'mnuEditCopyMetafile
        '
        Me.mnuEditCopyMetafile.Index = 3
        Me.mnuEditCopyMetafile.Text = "Copy Metafile"
        Me.mnuEditCopyMetafile.Visible = False
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewVerticalZoom, Me.mnuViewHorizontalZoom, Me.mnuViewZoomAll})
        Me.mnuView.Text = "View"
        '
        'mnuViewVerticalZoom
        '
        Me.mnuViewVerticalZoom.Index = 0
        Me.mnuViewVerticalZoom.Text = "Vertical Zoom/Pan"
        '
        'mnuViewHorizontalZoom
        '
        Me.mnuViewHorizontalZoom.Checked = True
        Me.mnuViewHorizontalZoom.Index = 1
        Me.mnuViewHorizontalZoom.Text = "Horizontal Zoom/Pan"
        '
        'mnuViewZoomAll
        '
        Me.mnuViewZoomAll.Index = 2
        Me.mnuViewZoomAll.Text = "Zoom to All"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuCoordinates
        '
        Me.mnuCoordinates.Index = 4
        Me.mnuCoordinates.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCoordinatesOnMenuBar})
        Me.mnuCoordinates.Text = "Coordinates"
        '
        'mnuCoordinatesOnMenuBar
        '
        Me.mnuCoordinatesOnMenuBar.Checked = True
        Me.mnuCoordinatesOnMenuBar.Index = 0
        Me.mnuCoordinatesOnMenuBar.Text = "On Menu Bar"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 5
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'mnuApplyJson
        '
        Me.mnuApplyJson.Index = 3
        Me.mnuApplyJson.Text = "Apply Specs"
        '
        'atcGraphForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(543, 496)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "atcGraphForm"
        Me.Text = "Graph"
        Me.ResumeLayout(False)

    End Sub

#End Region

    <CLSCompliant(False)>
    Public ReadOnly Property ZedGraphCtrl() As ZedGraphControl
        Get
            Return pZgc
        End Get
    End Property

    <CLSCompliant(False)>
    Public ReadOnly Property PaneAux() As GraphPane
        Get
            If pMaster.PaneList.Count > 1 Then
                Return pMaster.PaneList(0)
            Else
                Return Nothing
            End If
        End Get
    End Property

    <CLSCompliant(False)>
    Public ReadOnly Property Pane() As GraphPane
        Get
            If pMaster.PaneList.Count > 1 Then
                Return pMaster.PaneList(1)
            Else
                Return pMaster.PaneList(0)
            End If
        End Get
    End Property

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData("", pGrapher.Datasets, Nothing, False, True, Me.Icon)
    End Sub

    Public Function SaveGraph(ByVal aFilename As String) As Boolean
        RefreshGraph()
        Dim lSaved As Boolean = False
        Try
            pZgc.SaveIn(aFilename)
            lSaved = True
        Catch ex As Exception
            lSaved = False
        End Try
        Return lSaved
    End Function

    Private Sub mnuFileSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        RefreshGraph()
        Dim lSavedAs As String
        lSavedAs = pZgc.SaveAs(SaveImageExtension)
        If lSavedAs.Length > 0 Then
            SaveImageExtension = System.IO.Path.GetExtension(lSavedAs)
        End If
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(pZgc.MasterPane.GetImage)
    End Sub

    Private Sub mnuEditCopyMetafile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopyMetafile.Click
        'MetafileHelper.PutEnhMetafileOnClipboard(Me.Handle, PaneAsMetafile(Pane))
    End Sub

    Private Sub mnuFilePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click
        Dim lPrintDialog As New PrintDialog
        Dim lPrintDocument As New Printing.PrintDocument
        AddHandler lPrintDocument.PrintPage, AddressOf Me.PrintPage
        AddHandler lPrintDocument.QueryPageSettings, AddressOf Me.PageSettings

        With lPrintDialog
            .Document = lPrintDocument
            .AllowSelection = False
            .ShowHelp = True
            .UseEXDialog = True
            Dim lDialogResult As Windows.Forms.DialogResult = .ShowDialog(Me)
            If (lDialogResult = Windows.Forms.DialogResult.OK) Then
                Dim lSaveRectangle As RectangleF = pMaster.Rect
                lPrintDocument.Print()
                ' Restore graph size to fit form's bounds. 
                pMaster.ReSize(Me.CreateGraphics, lSaveRectangle)
            End If
        End With
    End Sub

    Private Sub PageSettings(ByVal sender As System.Object, ByVal e As Printing.QueryPageSettingsEventArgs)
        If pMaster.Rect.Width > pMaster.Rect.Height Then
            e.PageSettings.Landscape = True
        Else
            e.PageSettings.Landscape = False
        End If
    End Sub

    '' <summary> Prints the displayed graph. </summary> 
    '' <param name="sender"> Object raising this event. </param> 
    '' <param name="e"> Event arguments passing graphics context to print to. </param> 
    Private Sub PrintPage(ByVal sender As System.Object, ByVal e As Printing.PrintPageEventArgs)
        ' Validate. 
        If (e Is Nothing) Then Return
        If (e.Graphics Is Nothing) Then Return

        ' Resize the graph to fit the printout. 
        With e.MarginBounds
            pMaster.ReSize(e.Graphics, New RectangleF(.X, .Y, .Width, .Height))
        End With

        ' Print the graph. 
        pMaster.Draw(e.Graphics)

        e.HasMorePages = False 'ends the print job
    End Sub

    Private Sub pEditor_Apply() Handles pEditor.Apply
        RefreshGraph()
    End Sub

    Public Sub RefreshGraph()
        pZgc.AxisChange()
        Invalidate()
        Refresh()
    End Sub

    Private Sub mnuEditGraph_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditGraph.Click
        If pEditor IsNot Nothing Then 'Try to re-use existing editor
            Try
                pEditor.Show()
                pEditor.BringToFront()
                Exit Sub
            Catch ex As Exception
                'Probably existing one was already disposed, fall through to creating a new one below
            End Try
        End If
        pEditor = New frmGraphEditor
        pEditor.Text = "Edit " & Me.Text
        pEditor.Icon = Me.Icon
        pEditor.Edit(pZgc)
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pGrapher.Datasets, Me.Icon)
    End Sub

    Private Sub ShowHelpForGraph()
        If System.Reflection.Assembly.GetEntryAssembly.Location.EndsWith("TimeseriesUtility.exe") Then
            ShowHelp("View\Graph.html")
        Else
            ShowHelp("BASINS Details\Analysis\Time Series Functions\Graph.html")
        End If
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelpForGraph()
    End Sub
    Private Sub atcGraphForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelpForGraph()
        End If
    End Sub

    Private Sub mnuCoordinatesOnMenuBar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCoordinatesOnMenuBar.Click
        mnuCoordinatesOnMenuBar.Checked = Not mnuCoordinatesOnMenuBar.Checked
        If Not mnuCoordinatesOnMenuBar.Checked Then mnuCoordinates.Text = "Coordinates"
    End Sub

    Private Sub mnuViewHorizontalZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewHorizontalZoom.Click
        mnuViewHorizontalZoom.Checked = Not mnuViewHorizontalZoom.Checked
        pZgc.IsEnableHZoom = mnuViewHorizontalZoom.Checked
        pZgc.IsEnableHPan = mnuViewHorizontalZoom.Checked
    End Sub

    Private Sub mnuViewVerticalZoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewVerticalZoom.Click
        mnuViewVerticalZoom.Checked = Not mnuViewVerticalZoom.Checked
        pZgc.IsEnableVZoom = mnuViewVerticalZoom.Checked
        pZgc.IsEnableVPan = mnuViewVerticalZoom.Checked
    End Sub

    'Private Sub mnuViewZoomMouse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewZoomMouse.Click
    '    mnuViewZoomMouse.Checked = Not mnuViewZoomMouse.Checked
    '    pZgc.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
    'End Sub

    Private Sub pZgc_ZoomEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal oldState As ZedGraph.ZoomState, ByVal newState As ZedGraph.ZoomState) Handles pZgc.ZoomEvent

    End Sub

    Private Function pZgc_MouseMoveEvent(ByVal sender As ZedGraph.ZedGraphControl, ByVal e As System.Windows.Forms.MouseEventArgs) As System.Boolean Handles pZgc.MouseMoveEvent
        If mnuCoordinatesOnMenuBar.Checked Then
            ' Save the mouse location
            Dim mousePt As New PointF(e.X, e.Y)
            Dim lPositionText As String = "Coordinates"
            ' Find the pane that contains the current mouse location
            Dim lPane As GraphPane = sender.MasterPane.FindChartRect(mousePt)
            ' If pane is non-null, we have a valid location.  Otherwise, the mouse is not within any chart rect.
            If Not lPane Is Nothing Then
                Try
                    Dim x, y As Double
                    ' Convert the mouse location to X, Y scale values
                    lPane.ReverseTransform(mousePt, x, y)
                    If Double.IsNaN(x) OrElse Double.IsNaN(y) Then
                        lPositionText = ""
                    Else
                        ' Format the status label text
                        Select Case lPane.XAxis.Type
                            Case AxisType.DateDual
                                Dim lDate As Date = Date.FromOADate(x)
                                If lPane.XAxis.Scale.Max - lPane.XAxis.Scale.Min > 10 Then
                                    lPositionText = lDate.ToString("yyyy MMM d")
                                Else
                                    lPositionText = lDate.ToString("yyyy MMM d HH:mm")
                                End If
                            Case AxisType.Probability
                                Dim lProbScale As ZedGraph.ProbabilityScale = lPane.XAxis.Scale
                                Select Case lProbScale.LabelStyle
                                    Case ProbabilityScale.ProbabilityLabelStyle.Percent
                                        lPositionText = DoubleToString(x * 100, 3, , , , 3) & "%"
                                    Case ProbabilityScale.ProbabilityLabelStyle.Fraction
                                        lPositionText = DoubleToString(x, 7, , , , 5)
                                    Case ProbabilityScale.ProbabilityLabelStyle.ReturnInterval
                                        If x > 0 Then
                                            lPositionText = DoubleToString(1 / x, , , , , 3) & "yr"
                                        Else
                                            lPositionText = ""
                                        End If
                                End Select
                            Case Else
                                lPositionText = DoubleToString(x)
                        End Select
                        lPositionText = "(" & lPositionText & ", " & DoubleToString(y) & ")"
                    End If
                Catch
                    'Ignore any error setting coordinate text, default label is fine
                End Try
            End If
            mnuCoordinates.Text = lPositionText
        End If
        ' Return false to indicate we have not processed the MouseMoveEvent
        ' ZedGraphControl should still go ahead and handle it
        Return False
    End Function

    Private Sub atcGraphForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Not pMaster Is Nothing AndAlso pMaster.PaneList.Count > 1 Then
            Dim lPaneAux As GraphPane = pMaster.PaneList(0)
            Dim lPaneMain As GraphPane = pMaster.PaneList(1)
            Dim lOrigAuxHeight As Single = lPaneAux.Rect.Height
            Dim lTotalPaneHeight As Single = lOrigAuxHeight + lPaneMain.Rect.Height
            lPaneAux.Rect = New System.Drawing.Rectangle(
                    lPaneAux.Rect.X, lPaneAux.Rect.Y,
                    lPaneAux.Rect.Width, lTotalPaneHeight * AuxFraction)
            lPaneMain.Rect = New System.Drawing.Rectangle(
                    lPaneMain.Rect.X, lPaneMain.Rect.Y - lOrigAuxHeight + lPaneAux.Rect.Height,
                    lPaneMain.Rect.Width, lTotalPaneHeight - lPaneAux.Rect.Height)
        End If
    End Sub

    Private Sub FreeResources()
        If Not pEditor Is Nothing Then
            pEditor.Dispose()
            pEditor = Nothing
        End If
        pMaster = Nothing
        pZgc = Nothing
        If Not pGrapher Is Nothing Then
            pGrapher.Dispose()
            pGrapher = Nothing
        End If
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        FreeResources()
    End Sub

    Private Sub atcGraphForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        FreeResources()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        FreeResources()
    End Sub

    Private Sub mnuViewZoomAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewZoomAll.Click
        pZgc.ZoomOutAll(Pane)
        'TODO: test when Aux pane active
    End Sub

    Private Sub mnuSaveJson_Click(sender As Object, e As EventArgs) Handles mnuSaveJson.Click

        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save graph specs to file"
            .DefaultExt = ".json"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".json"
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim lStr As String = ""
                Dim lSerial As String = ""
                Dim ser As JavaScriptSerializer = New JavaScriptSerializer()

                'lStr += "CURVES" & vbCrLf
                'For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                '    For Each lCurve As CurveItem In lPane.CurveList
                '        lStr += ser.Serialize(lCurve) & vbCrLf
                '    Next
                'Next

                'lStr += "LEGENDS" & vbCrLf
                'For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                '    lStr += ser.Serialize(lPane.Legend) & vbCrLf
                'Next

                'lStr += "OBJECTS" & vbCrLf
                'For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                '    For Each lObj As GraphObj In lPane.GraphObjList
                '        lStr += ser.Serialize(lObj) & vbCrLf
                '    Next
                'Next

                'lStr += "AXES" & vbCrLf
                'For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                '    For Each lAxis As YAxis In lPane.YAxisList
                '        lAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                '        lStr += ser.Serialize(lAxis) & vbCrLf
                '    Next
                '    For Each lAxis As Y2Axis In lPane.Y2AxisList
                '        lAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                '        lStr += ser.Serialize(lAxis) & vbCrLf
                '    Next
                '    lPane.XAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                '    lStr += ser.Serialize(lPane.XAxis) & vbCrLf
                'Next

                'fix a problem that will cause serialize to not work
                For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                    For Each lAxis As YAxis In lPane.YAxisList
                        lAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                    Next
                    For Each lAxis As Y2Axis In lPane.Y2AxisList
                        lAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                    Next
                    lPane.XAxis.Scale.FontSpec.Border.GradientFill.Brush = Nothing
                Next

                Try
                    lSerial += ser.Serialize(pZgc.MasterPane)
                Catch ex As Exception
                    lSerial += ex.ToString
                End Try

                SaveFileString(.FileName, lSerial)
            End If
        End With

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuApplyJson_Click(sender As Object, e As EventArgs) Handles mnuApplyJson.Click
        Dim lOpenDialog As New System.Windows.Forms.OpenFileDialog
        With lOpenDialog
            .Title = "Open file containing graph specs"
            .DefaultExt = ".json"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".json"
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim JsonString As String = File.ReadAllText(.FileName)
                'Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
                Try
                    'pZgc.MasterPane = ser.Deserialize(Of ZedGraph.MasterPane)(JsonString)   'does not work
                    Dim lBuffer() As Byte = File.ReadAllBytes(.FileName)
                    Dim lReader As XmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(lBuffer, New XmlDictionaryReaderQuotas())
                    Dim lXml As XElement = XElement.Load(lReader)
                    Dim lDoc As New XmlDocument
                    lDoc.LoadXml(lXml.ToString())

                    'set up collection of curves for easy reference
                    Dim lCurves As New atcCollection
                    Dim lCurveIndex As Integer = -1
                    For Each lPane As GraphPane In pZgc.MasterPane.PaneList
                        For Each lCurve As CurveItem In lPane.CurveList
                            lCurveIndex += 1
                            lCurves.Add(lCurveIndex, lCurve)
                        Next
                    Next

                    Dim lPaneList As XmlNodeList = lDoc.GetElementsByTagName("PaneList")
                    Dim lTag As String
                    lCurveIndex = -1
                    For Each lPane As XmlNode In lPaneList
                        For Each lItemNode As XmlNode In lPane.ChildNodes
                            For Each lCurveNode As XmlNode In lItemNode.ChildNodes
                                If lCurveNode.Name = "CurveList" Then
                                    lCurveIndex += 1
                                    For Each lChildItem As XmlNode In lCurveNode.ChildNodes
                                        For Each lChildNode As XmlNode In lChildItem.ChildNodes
                                            If lChildNode.Name = "Tag" Then
                                                lTag = lChildNode.InnerText
                                            ElseIf lChildNode.Name = "Label" Then
                                                lCurves(lCurveIndex).Label.Text = lChildNode.InnerText
                                            ElseIf lChildNode.Name = "Color" Then
                                                Dim lA As Integer = 0
                                                Dim lR As Integer = 0
                                                Dim lG As Integer = 0
                                                Dim lB As Integer = 0
                                                For Each lColorNode As XmlNode In lChildNode.ChildNodes
                                                    If lColorNode.Name = "R" Then
                                                        lR = lColorNode.InnerText
                                                    ElseIf lColorNode.Name = "G" Then
                                                        lG = lColorNode.InnerText
                                                    ElseIf lColorNode.Name = "B" Then
                                                        lB = lColorNode.InnerText
                                                    ElseIf lColorNode.Name = "A" Then
                                                        lA = lColorNode.InnerText
                                                    End If
                                                Next
                                                lCurves(lCurveIndex).color = Color.FromArgb(lA, lR, lG, lB)
                                            ElseIf lChildNode.Name = "Symbol" Then
                                                For Each lSymbolNode As XmlNode In lChildNode.ChildNodes
                                                    If lSymbolNode.Name = "Size" Then
                                                        lCurves(lCurveIndex).Symbol.Size = lSymbolNode.InnerText
                                                    End If
                                                Next
                                            End If
                                        Next
                                    Next
                                End If
                            Next
                        Next
                    Next
                    RefreshGraph()

                Catch ex As Exception
                    Dim l As Integer = 0
                End Try
            End If
        End With
    End Sub
End Class

