VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsShape"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pShapeType As Integer
Private pVertices() As Double
Private pCount As Long

'We use zero elements of 2-D Vertices array
'Vertices(0, 0) is first X value
'Vertices(0, 1) is first Y value
'Vertices(1, 0) is second X value
Public Property Get Vertices() As Variant
  Vertices = pVertices
End Property
Public Property Let Vertices(newVertices As Variant)
  pVertices = newVertices
  pCount = UBound(pVertices, 1) + 1
End Property

Public Property Get Count() As Long
  Count = pCount
End Property

'1=point
'3=line
'5=polygon
Public Property Get ShapeType() As Integer
  ShapeType = pShapeType
End Property
Public Property Let ShapeType(newValue As Integer)
  pShapeType = newValue
End Property

Public Property Get IsPoint() As Boolean
  If ShapeType = 1 Then IsPoint = True Else IsPoint = False
End Property
Public Property Let IsPoint(newValue As Boolean)
  If newValue Then ShapeType = 1 Else ShapeType = 0
End Property

Public Property Get IsLine() As Boolean
  If ShapeType = 3 Then IsLine = True Else IsLine = False
End Property
Public Property Let IsLine(newValue As Boolean)
  If newValue Then ShapeType = 3 Else ShapeType = 0
End Property

Public Property Get IsPolygon() As Boolean
  If ShapeType = 5 Then IsPolygon = True Else IsPolygon = False
End Property
Public Property Let IsPolygon(newValue As Boolean)
  If newValue Then ShapeType = 5 Else ShapeType = 0
End Property

'pct as PictureBox
Public Sub Draw(pct As Object, Optional radius As Double = 0)
  Dim b&, ob&, k&
  Dim lValue#
  Dim poly() As POINTAPI
  Dim vtx As Long
  If Me.IsPolygon Then 'fill it
    ReDim poly(Me.Count - 1)
    For vtx = 0 To Me.Count - 1
      ' -2 is a kludge adjustment
      poly(vtx).X = -2 + (((pVertices(vtx, 0) - pct.ScaleLeft) / pct.ScaleWidth) * pct.Width) / Screen.TwipsPerPixelX
      poly(vtx).Y = -2 + (((pVertices(vtx, 1) - pct.ScaleTop) / pct.ScaleHeight) * pct.Height) / Screen.TwipsPerPixelY
    Next
    b = CreateSolidBrush(pct.ForeColor)
    ob = SelectObject(pct.hDC, b)
    k = Polygon(pct.hDC, poly(0), Me.Count - 1)
    k = SelectObject(pct.hDC, ob)
    k = DeleteObject(b)
  ElseIf Me.IsPoint Then
    If radius = 0 Then radius = pct.ScaleWidth / 150
    pct.Circle (pVertices(0, 0), pVertices(0, 1)), radius
  Else 'line
    For vtx = 1 To Me.Count - 1
      pct.Line (pVertices(vtx - 1, 0), pVertices(vtx - 1, 1))-(pVertices(vtx, 0), pVertices(vtx, 1))
    Next
  End If

End Sub
