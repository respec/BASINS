Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDurationHydrographControl
    Private pDataGroup As atcTimeseriesGroup
    Private pResultForm As System.Windows.Forms.Form

    Private pDefaultNumClasses As Integer = 35
    Private pDefaultClasses As Double() = {0.0, 0.1, 0.2, 0.3, 0.5, 0.7, 0.8, 0.9, 1.0}
    Private pHelpLocation As String = "BASINS Details\Analysis\USGS Surface Water Statistics\Duration Hydrograph.html"
    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click
        ShowHelp(pHelpLocation)
    End Sub

    Private Sub frmDurationHydrographControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pHelpLocation)
        End If
    End Sub


    Public Property DataGroup() As atcTimeseriesGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal value As atcTimeseriesGroup)
            pDataGroup = value
        End Set
    End Property

    Public Property ResultForm() As frmResult
        Get
            Return pResultForm
        End Get
        Set(ByVal value As frmResult)
            pResultForm = value
        End Set
    End Property

    Public Property DefaultClasses() As Double()
        Get
            Return pDefaultClasses
        End Get
        Set(ByVal value As Double())
            pDefaultClasses = value
        End Set
    End Property

    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '
        DataGroup = aDataGroup
    End Sub

    Private Sub frmDurationHydrographControl_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
        End If

        pDataGroup = Nothing
    End Sub

    Private Sub frmDurationHydrographControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'lblAnalysisInfo.Text = ""
        'lblAnalysisInfo.Text = "Step1. Select timeseries data" & vbNewLine & _
        '                       "Step2. Examine the class limits (use default or create anew)" & vbNewLine & _
        '                       "Step3. Choose an anlysis" & vbNewLine & _
        '                       "Step4. Click either Graph or Report to carry out analysis and see results"
        lstPctExceed.CurrentValues = DefaultClasses
        ResultForm = CreateForm()
    End Sub

    Private Sub DisplayDefault() Handles lstPctExceed.DefaultRequested
        lstPctExceed.CurrentValues = DefaultClasses
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If ResultForm Is Nothing Then
            Logger.Dbg("Duration Hydrograph analysis failed to initialize result form")
            Exit Sub
        ElseIf ResultForm.IsDisposed Then
            ResultForm = Nothing
            ResultForm = CreateForm()
        End If

        Dim lListPct As List(Of Double) = ListOK()

        If DataGroup.Count = 0 Then
            mnuSelectData_Click(Nothing, Nothing)
        End If
        ResultForm.Initialize("DurationHydrograph", DataGroup, lListPct.ToArray(), "report")
        ResultForm.Show()
        ResultForm.txtReport.SelectionLength = 0
    End Sub

    Private Sub btnGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraph.Click
        If ResultForm Is Nothing Then
            Logger.Dbg("Duration/Compare analysis failed to initialize proper form")
            Exit Sub
        ElseIf ResultForm.IsDisposed Then
            ResultForm = Nothing
            ResultForm = CreateForm()
        End If

        Dim lListPct As List(Of Double) = ListOK()

        If DataGroup.Count = 0 Then
            mnuSelectData_Click(Nothing, Nothing)
        End If

        ResultForm.Initialize("DurationHydrograph", DataGroup, lListPct.ToArray(), "graph")
    End Sub

    Private Sub mnuSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Data For Analysis", _
                                                   pDataGroup, Nothing, True, True, Me.Icon)
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
            pDataGroup = Nothing
        End If
        Me.Close()
    End Sub

#Region "Util"
    Private Function CreateForm() As frmResult
        Dim lDisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        Dim lFrmResult As New frmResult
        lFrmResult.Icon = Me.Icon
        For Each lDataDisplay As atcDataDisplay In lDisplayPlugins
            Dim lMenuText As String = lDataDisplay.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            If TypeOf (lFrmResult) Is frmResult Then
                CType(lFrmResult, frmResult).mnuAnalysis.DropDownItems().Add(lMenuText, Nothing, New EventHandler(AddressOf lFrmResult.mnuAnalysis_Click))
            Else
                lFrmResult.mnuAnalysis.DropDownItems().Add(lMenuText, Nothing, New EventHandler(AddressOf lFrmResult.mnuAnalysis_Click))
            End If
        Next
        Return lFrmResult
    End Function

    Private Function ListOK() As List(Of Double)
        Dim lDict As New Dictionary(Of Double, Integer)
        Dim lList As New List(Of Double)
        For Each lPct As Double In lstPctExceed.CurrentValues
            If lPct >= 0.0 AndAlso lPct <= 1.0 Then
                If Not lList.Contains(lPct) Then
                    lList.Add(lPct)
                End If
            End If
        Next
        If Not lList.Contains(0) Then lList.Add(0)
        If Not lList.Contains(1) Then lList.Add(1)
        lList.Sort()
        Return lList
    End Function
#End Region

End Class