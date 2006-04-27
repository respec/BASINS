Imports atcData
Imports atcTimeseriesNdayHighLow
Imports atcUtility

Public Class frmSpecifyFrequency
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents panelTop As System.Windows.Forms.Panel
  Friend WithEvents panelBottom As System.Windows.Forms.Panel
  Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents grpRecurrence As System.Windows.Forms.GroupBox
  Friend WithEvents btnRecurrenceNone As System.Windows.Forms.Button
  Friend WithEvents btnRecurrenceAll As System.Windows.Forms.Button
  Friend WithEvents lstRecurrence As System.Windows.Forms.ListBox
  Friend WithEvents grpNday As System.Windows.Forms.GroupBox
  Friend WithEvents btnNdayNone As System.Windows.Forms.Button
  Friend WithEvents btnNdayAll As System.Windows.Forms.Button
  Friend WithEvents lstNday As System.Windows.Forms.ListBox
  Friend WithEvents btnOkLow As System.Windows.Forms.Button
  Friend WithEvents btnOkHigh As System.Windows.Forms.Button
  Friend WithEvents txtNdayAdd As System.Windows.Forms.TextBox
  Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
  Friend WithEvents btnRecurrenceAdd As System.Windows.Forms.Button
  Friend WithEvents txtRecurrenceAdd As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSpecifyFrequency))
    Me.panelTop = New System.Windows.Forms.Panel
    Me.grpRecurrence = New System.Windows.Forms.GroupBox
    Me.lstRecurrence = New System.Windows.Forms.ListBox
    Me.btnRecurrenceAdd = New System.Windows.Forms.Button
    Me.txtRecurrenceAdd = New System.Windows.Forms.TextBox
    Me.btnRecurrenceNone = New System.Windows.Forms.Button
    Me.btnRecurrenceAll = New System.Windows.Forms.Button
    Me.Splitter1 = New System.Windows.Forms.Splitter
    Me.grpNday = New System.Windows.Forms.GroupBox
    Me.btnNdayAdd = New System.Windows.Forms.Button
    Me.txtNdayAdd = New System.Windows.Forms.TextBox
    Me.btnNdayNone = New System.Windows.Forms.Button
    Me.btnNdayAll = New System.Windows.Forms.Button
    Me.lstNday = New System.Windows.Forms.ListBox
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnOkHigh = New System.Windows.Forms.Button
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOkLow = New System.Windows.Forms.Button
    Me.panelTop.SuspendLayout()
    Me.grpRecurrence.SuspendLayout()
    Me.grpNday.SuspendLayout()
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'panelTop
    '
    Me.panelTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.panelTop.Controls.Add(Me.grpRecurrence)
    Me.panelTop.Controls.Add(Me.Splitter1)
    Me.panelTop.Controls.Add(Me.grpNday)
    Me.panelTop.Location = New System.Drawing.Point(0, 0)
    Me.panelTop.Name = "panelTop"
    Me.panelTop.Size = New System.Drawing.Size(490, 378)
    Me.panelTop.TabIndex = 14
    '
    'grpRecurrence
    '
    Me.grpRecurrence.Controls.Add(Me.lstRecurrence)
    Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAdd)
    Me.grpRecurrence.Controls.Add(Me.txtRecurrenceAdd)
    Me.grpRecurrence.Controls.Add(Me.btnRecurrenceNone)
    Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAll)
    Me.grpRecurrence.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpRecurrence.Location = New System.Drawing.Point(250, 0)
    Me.grpRecurrence.Name = "grpRecurrence"
    Me.grpRecurrence.Size = New System.Drawing.Size(240, 378)
    Me.grpRecurrence.TabIndex = 14
    Me.grpRecurrence.TabStop = False
    Me.grpRecurrence.Text = "Recurrence Interval"
    '
    'lstRecurrence
    '
    Me.lstRecurrence.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstRecurrence.IntegralHeight = False
    Me.lstRecurrence.ItemHeight = 16
    Me.lstRecurrence.Location = New System.Drawing.Point(10, 18)
    Me.lstRecurrence.Name = "lstRecurrence"
    Me.lstRecurrence.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lstRecurrence.Size = New System.Drawing.Size(220, 305)
    Me.lstRecurrence.TabIndex = 7
    '
    'btnRecurrenceAdd
    '
    Me.btnRecurrenceAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnRecurrenceAdd.Enabled = False
    Me.btnRecurrenceAdd.Location = New System.Drawing.Point(154, 295)
    Me.btnRecurrenceAdd.Name = "btnRecurrenceAdd"
    Me.btnRecurrenceAdd.Size = New System.Drawing.Size(76, 28)
    Me.btnRecurrenceAdd.TabIndex = 14
    Me.btnRecurrenceAdd.Text = "Add"
    '
    'txtRecurrenceAdd
    '
    Me.txtRecurrenceAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtRecurrenceAdd.Enabled = False
    Me.txtRecurrenceAdd.Location = New System.Drawing.Point(10, 295)
    Me.txtRecurrenceAdd.Name = "txtRecurrenceAdd"
    Me.txtRecurrenceAdd.Size = New System.Drawing.Size(134, 22)
    Me.txtRecurrenceAdd.TabIndex = 13
    Me.txtRecurrenceAdd.Text = ""
    '
    'btnRecurrenceNone
    '
    Me.btnRecurrenceNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnRecurrenceNone.Location = New System.Drawing.Point(154, 336)
    Me.btnRecurrenceNone.Name = "btnRecurrenceNone"
    Me.btnRecurrenceNone.Size = New System.Drawing.Size(76, 26)
    Me.btnRecurrenceNone.TabIndex = 12
    Me.btnRecurrenceNone.Text = "None"
    '
    'btnRecurrenceAll
    '
    Me.btnRecurrenceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnRecurrenceAll.Location = New System.Drawing.Point(10, 336)
    Me.btnRecurrenceAll.Name = "btnRecurrenceAll"
    Me.btnRecurrenceAll.Size = New System.Drawing.Size(76, 27)
    Me.btnRecurrenceAll.TabIndex = 11
    Me.btnRecurrenceAll.Text = "All"
    '
    'Splitter1
    '
    Me.Splitter1.Location = New System.Drawing.Point(240, 0)
    Me.Splitter1.Name = "Splitter1"
    Me.Splitter1.Size = New System.Drawing.Size(10, 378)
    Me.Splitter1.TabIndex = 13
    Me.Splitter1.TabStop = False
    '
    'grpNday
    '
    Me.grpNday.Controls.Add(Me.btnNdayAdd)
    Me.grpNday.Controls.Add(Me.txtNdayAdd)
    Me.grpNday.Controls.Add(Me.btnNdayNone)
    Me.grpNday.Controls.Add(Me.btnNdayAll)
    Me.grpNday.Controls.Add(Me.lstNday)
    Me.grpNday.Dock = System.Windows.Forms.DockStyle.Left
    Me.grpNday.Location = New System.Drawing.Point(0, 0)
    Me.grpNday.Name = "grpNday"
    Me.grpNday.Size = New System.Drawing.Size(240, 378)
    Me.grpNday.TabIndex = 12
    Me.grpNday.TabStop = False
    Me.grpNday.Text = "Number of Days"
    '
    'btnNdayAdd
    '
    Me.btnNdayAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnNdayAdd.Location = New System.Drawing.Point(154, 295)
    Me.btnNdayAdd.Name = "btnNdayAdd"
    Me.btnNdayAdd.Size = New System.Drawing.Size(76, 28)
    Me.btnNdayAdd.TabIndex = 12
    Me.btnNdayAdd.Text = "Add"
    '
    'txtNdayAdd
    '
    Me.txtNdayAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtNdayAdd.Location = New System.Drawing.Point(10, 295)
    Me.txtNdayAdd.Name = "txtNdayAdd"
    Me.txtNdayAdd.Size = New System.Drawing.Size(134, 22)
    Me.txtNdayAdd.TabIndex = 11
    Me.txtNdayAdd.Text = ""
    '
    'btnNdayNone
    '
    Me.btnNdayNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnNdayNone.Location = New System.Drawing.Point(154, 336)
    Me.btnNdayNone.Name = "btnNdayNone"
    Me.btnNdayNone.Size = New System.Drawing.Size(76, 26)
    Me.btnNdayNone.TabIndex = 10
    Me.btnNdayNone.Text = "None"
    '
    'btnNdayAll
    '
    Me.btnNdayAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnNdayAll.Location = New System.Drawing.Point(10, 336)
    Me.btnNdayAll.Name = "btnNdayAll"
    Me.btnNdayAll.Size = New System.Drawing.Size(76, 27)
    Me.btnNdayAll.TabIndex = 9
    Me.btnNdayAll.Text = "All"
    '
    'lstNday
    '
    Me.lstNday.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstNday.IntegralHeight = False
    Me.lstNday.ItemHeight = 16
    Me.lstNday.Location = New System.Drawing.Point(10, 18)
    Me.lstNday.Name = "lstNday"
    Me.lstNday.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lstNday.Size = New System.Drawing.Size(220, 268)
    Me.lstNday.TabIndex = 7
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnOkHigh)
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOkLow)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 393)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(489, 37)
    Me.panelBottom.TabIndex = 15
    '
    'btnOkHigh
    '
    Me.btnOkHigh.Location = New System.Drawing.Point(10, 0)
    Me.btnOkHigh.Name = "btnOkHigh"
    Me.btnOkHigh.Size = New System.Drawing.Size(115, 28)
    Me.btnOkHigh.TabIndex = 2
    Me.btnOkHigh.Text = "Compute High"
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(298, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(76, 28)
    Me.btnCancel.TabIndex = 1
    Me.btnCancel.Text = "Cancel"
    '
    'btnOkLow
    '
    Me.btnOkLow.Location = New System.Drawing.Point(154, 0)
    Me.btnOkLow.Name = "btnOkLow"
    Me.btnOkLow.Size = New System.Drawing.Size(115, 28)
    Me.btnOkLow.TabIndex = 0
    Me.btnOkLow.Text = "Compute Low"
    '
    'frmSpecifyFrequency
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(489, 430)
    Me.Controls.Add(Me.panelTop)
    Me.Controls.Add(Me.panelBottom)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.KeyPreview = True
    Me.Name = "frmSpecifyFrequency"
    Me.Text = "Select Numbers of Days and Recurrence Intervals"
    Me.panelTop.ResumeLayout(False)
    Me.grpRecurrence.ResumeLayout(False)
    Me.grpNday.ResumeLayout(False)
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private WithEvents pGroup As atcDataGroup
  Private pOk As Boolean
  Private pChoseHigh As Boolean

  Public Function AskUser(ByVal aGroup As atcDataGroup, ByRef aChoseHigh As Boolean) As Boolean
    pGroup = aGroup
    Clear()
    Me.ShowDialog()
    If pOk Then aChoseHigh = pChoseHigh
    pGroup = Nothing
    Return pOk
  End Function

  Private Sub Clear()
    pOk = False
    lstRecurrence.Items.Clear()
    lstNday.Items.Clear()
    Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
    Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")
    Dim lDefault As Object

    lDefault = lNDayHi.Arguments.GetDefinedValue("NDay").Definition.DefaultValue
    If Not lDefault Is Nothing AndAlso IsArray(lDefault) Then
      For Each lNday As Double In lDefault
        lstNday.Items.Add(lNday)
        'lstNday.SetSelected(lstNday.Items.Count - 1, True)
      Next
    End If

    lDefault = lNDayHi.Arguments.GetDefinedValue("Return Period").Definition.DefaultValue
    If Not lDefault Is Nothing AndAlso IsArray(lDefault) Then
      For Each lNyear As Double In lDefault
        lstRecurrence.Items.Add(Format(lNyear, "0.####"))
        'lstRecurrence.SetSelected(lstRecurrence.Items.Count - 1, True)
      Next
    End If

  End Sub

  Private Sub Calculate(ByVal aOperationName As String)
    Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
    Dim lArgs As New atcDataAttributes
    lArgs.SetValue("Timeseries", pGroup)
    lArgs.SetValue("NDay", ListToArray(lstNday))
    lArgs.SetValue("Return Period", ListToArray(lstRecurrence))
    lCalculator.Open(aOperationName, lArgs)
  End Sub

  'Return all selected items, or if none are selected then all items
  Private Function ListToArray(ByVal aList As System.Windows.Forms.ListBox) As Double()
    Dim lArray() As Double
    If aList.SelectedItems.Count > 0 Then
      ReDim lArray(aList.SelectedItems.Count - 1)
      For lIndex As Integer = 0 To aList.SelectedItems.Count - 1
        lArray(lIndex) = CDbl(aList.SelectedItems(lIndex))
      Next
    Else
      ReDim lArray(aList.Items.Count - 1)
      For lIndex As Integer = 0 To aList.Items.Count - 1
        lArray(lIndex) = CDbl(aList.Items(lIndex))
      Next
    End If
    Return lArray
  End Function

  Private Sub btnNdayAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAdd.Click
    lstNday.Items.Add(txtNdayAdd.Text)
    lstNday.SetSelected(lstNday.Items.Count - 1, True)
  End Sub

  Private Sub btnRecurrenceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAdd.Click
    lstRecurrence.Items.Add(txtRecurrenceAdd.Text)
    lstRecurrence.SetSelected(lstRecurrence.Items.Count - 1, True)
  End Sub

  Private Sub btnNdayAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAll.Click
    For index As Integer = 0 To lstNday.Items.Count - 1
      lstNday.SetSelected(index, True)
    Next
  End Sub

  Private Sub btnNdayNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayNone.Click
    For index As Integer = 0 To lstNday.Items.Count - 1
      lstNday.SetSelected(index, False)
    Next
  End Sub

  Private Sub btnRecurrenceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAll.Click
    For index As Integer = 0 To lstRecurrence.Items.Count - 1
      lstRecurrence.SetSelected(index, True)
    Next
  End Sub

  Private Sub btnRecurrenceNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceNone.Click
    For index As Integer = 0 To lstRecurrence.Items.Count - 1
      lstRecurrence.SetSelected(index, False)
    Next
  End Sub

  Private Sub btnOkLow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOkLow.Click
    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
    Calculate("n-day low value")
    pOk = True
    pChoseHigh = False
    Close()
  End Sub

  Private Sub btnOkHigh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkHigh.Click
    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
    Calculate("n-day high value")
    pOk = True
    pChoseHigh = True
    Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Close()
  End Sub

  Private Sub frmSpecifyFrequency_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    If e.KeyValue = Windows.Forms.Keys.F1 Then
      ShowHelp("BASINS Details\Analysis\Time Series Functions\Frequency Grid.html")
    End If
  End Sub
End Class
