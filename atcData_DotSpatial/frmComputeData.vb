Public Class frmComputeData
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
  Friend WithEvents lstComputation As System.Windows.Forms.ListBox
  Friend WithEvents grpComputation As System.Windows.Forms.GroupBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.lstComputation = New System.Windows.Forms.ListBox
    Me.grpComputation = New System.Windows.Forms.GroupBox
    Me.SuspendLayout()
    '
    'lstComputation
    '
    Me.lstComputation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.lstComputation.IntegralHeight = False
    Me.lstComputation.Location = New System.Drawing.Point(0, 0)
    Me.lstComputation.Name = "lstComputation"
    Me.lstComputation.Size = New System.Drawing.Size(72, 342)
    Me.lstComputation.TabIndex = 0
    '
    'grpComputation
    '
    Me.grpComputation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpComputation.Location = New System.Drawing.Point(72, 0)
    Me.grpComputation.Name = "grpComputation"
    Me.grpComputation.Size = New System.Drawing.Size(212, 336)
    Me.grpComputation.TabIndex = 1
    Me.grpComputation.TabStop = False
    '
    'frmComputeTimeseries
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(288, 341)
    Me.Controls.Add(Me.grpComputation)
    Me.Controls.Add(Me.lstComputation)
    Me.Name = "frmComputeTimeseries"
    Me.Text = "Compute Timeseries"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Dim pDataManager As atcDataManager
  Dim pTS As atcDataGroup
  Dim pComputePlugin As atcTimeseriesCompute
  Dim pComputeOperationName As String

  Public Function AskUser(ByVal aDataManager As atcDataManager, Optional ByVal aGroup As atcDataGroup = Nothing) As atcTimeseries
    Dim pSaveGroup As atcDataGroup = Nothing
    pDataManager = aDataManager
    If aGroup Is Nothing Then
      pTS = New atcDataGroup
    Else
      pTS = aGroup
    End If

    Dim ComputePlugins As ICollection = pDataManager.GetPlugins(GetType(atcTimeseriesCompute))
    For Each cp As atcTimeseriesCompute In ComputePlugins
      For Each computationEntry As DictionaryEntry In cp.AvailableOperations
        lstComputation.Items.Add(computationEntry.Value.Name)
      Next
    Next

    Me.ShowDialog()

    If Not pComputePlugin Is Nothing AndAlso Not pComputeOperationName Is Nothing AndAlso pComputeOperationName.Length > 0 Then
      Return pComputePlugin.ComputeTimeseries(pComputeOperationName, pComputePlugin.ExtractArgs())
    End If
  End Function

  Private Sub lstComputation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstComputation.SelectedIndexChanged
    pComputeOperationName = lstComputation.SelectedItem
    Dim ComputePlugins As ICollection = pDataManager.GetPlugins(GetType(atcTimeseriesCompute))
    For Each cp As atcTimeseriesCompute In ComputePlugins
      For Each computationEntry As DictionaryEntry In cp.AvailableOperations
        If computationEntry.Value.Name.Equals(pComputeOperationName) Then
          pComputePlugin = cp
          grpComputation.Text = pComputeOperationName
          pComputePlugin.PopulateInterface(pComputeOperationName, grpComputation.Controls, pDataManager)
        End If
      Next
    Next
  End Sub

  Private Sub grpComputation_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpComputation.Enter

  End Sub
End Class
