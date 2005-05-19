Public Class atcGrid
  Inherits System.Windows.Forms.UserControl
  Private pRows As Integer
  Private pCols As Integer
  Private pRowLineColor As Color
  Private pColLineColor As Color
  Private pRowLineWidth As Single
  Private pColLineWidth As Single
  Private pRowHeight() As Single
  Private pColWidth() As Single

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    pRows = 3
    pCols = 3
    pRowLineColor = Color.Black
    pColLineColor = Color.Black
    pRowLineWidth = 1
    pColLineWidth = 1
    ReDim pRowHeight(pCols)
    pRowHeight(0) = 50
    ReDim pColWidth(pCols)
    pColWidth(0) = Me.Width

  End Sub

  'UserControl overrides dispose to clean up the component list.
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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    components = New System.ComponentModel.Container
  End Sub

#End Region

  Private Sub ATCgrid_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    Dim g As Graphics = e.Graphics
    Dim x As Single
    Dim y As Single
    Dim dx As Single
    Dim dy As Single
    Dim lPen As New Pen(pRowLineColor, pRowLineWidth)
    y = 0
    dy = pRowHeight(0)
    For iRow As Integer = 0 To pRows - 1
      If iRow <= pRowHeight.GetUpperBound(0) Then
        y += pRowHeight(0)
      Else
        y += dy
      End If
      g.DrawLine(lPen, g.ClipBounds.Left, y, g.ClipBounds.Right, y)
    Next
    lPen = New Pen(pRowLineColor, pRowLineWidth)
    x = 0
    dx = pColWidth(0)
    For iCol As Integer = 0 To pCols - 1
      g.DrawLine(lPen, x, g.ClipBounds.Top, x, g.ClipBounds.Bottom)
    Next
  End Sub

End Class
