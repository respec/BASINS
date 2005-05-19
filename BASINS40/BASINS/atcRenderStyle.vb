Option Strict Off
Option Explicit On
Public Class atcRenderStyle

  Private pName As String

  Private pOperator As Integer '-2:<  -1:<=  0:=  1:>=  2: >

  Private pValue As String
  Private pValueNum As Double
  Private pIsNumeric As Boolean

  Private pFillColor As System.Drawing.Color
  Private pFillStyle As Integer

  Private pLabelColor As System.Drawing.Color
  Private pLabelsOn As Boolean
  'Private pLabelFont As System.Drawing.Font
  Private pLabelFontName As String
  Private pLabelBold As Boolean
  Private pLabelItalic As Boolean
  Private pLabelStrikeOut As Boolean
  Private pLabelUnderline As Boolean
  Private pLabelSize As Single 'In Points
  Private pLabelWeight As Single

  Private pLineColor As System.Drawing.Color

  Private pLineStyle As Integer
  Private pLineWidth As Integer

  Private pMarkColor As System.Drawing.Color
  Private pMarkSize As Single
  Private pMarkStyle As Integer
  Private pMarkBitsW As Integer
  Private pMarkBitsH As Integer
  Private pMarkBits(,) As Boolean

  Public Property Name() As String
    Get
      Name = pName
    End Get
    Set(ByVal Value As String)
      pName = Value
    End Set
  End Property

  Public Property Value() As String
    Get
      Value = pValue
    End Get
    Set(ByVal Value As String)
      pValue = Value
      If IsNumeric(pValue) Then
        pIsNumeric = True
        pValueNum = CDbl(pValue)
      Else
        pIsNumeric = False
      End If
    End Set
  End Property

  Public Property operator() As String
    Get
      Select Case pOperator
        Case -2 : operator = "<"
        Case -1 : operator = "<="
        Case 0 : operator = "="
        Case 1 : operator = ">="
        Case 2 : operator = ">"
      End Select
    End Get
    Set(ByVal Value As String)
      Select Case Value
        Case "<" : pOperator = -2
        Case "<=" : pOperator = -1
        Case "=" : pOperator = 0
        Case ">=" : pOperator = 1
        Case ">" : pOperator = 2
      End Select
    End Set
  End Property

  Public Property FillColor() As System.Drawing.Color
    Get
      Return pFillColor
    End Get
    Set(ByVal Value As System.Drawing.Color)
      pFillColor = Value
    End Set
  End Property

  'None
  'Solid
  'Horizontal      '  -----
  'Vertical        '  |||||
  'Down            '  \\\\\
  'Up              '  /////
  'Cross           '  +++++
  'DiagCross       '  xxxxx
  Public Property FillStyle() As String
    Get
      Select Case pFillStyle
        Case -1 : FillStyle = "None"
        Case 0 : FillStyle = "Solid"
        Case 100 : FillStyle = "Horizontal" '  -----
        Case 101 : FillStyle = "Vertical" '  |||||"
        Case 102 : FillStyle = "Down" '  \\\\\"
        Case 103 : FillStyle = "Up" '  /////"
        Case 104 : FillStyle = "Cross" '  +++++"
        Case 105 : FillStyle = "DiagCross" '  xxxxx"
        Case Else : FillStyle = "Unknown"
      End Select
    End Get
    Set(ByVal Value As String)
      If IsNumeric(Value) Then
        pFillStyle = CInt(Value)
      Else
        Select Case LCase(Value)
          Case "none", "" : pFillStyle = -1
          Case "solid" : pFillStyle = 0
          Case "horizontal" : pFillStyle = 100 '  -----
          Case "vertical" : pFillStyle = 101 '  |||||
          Case "down" : pFillStyle = 102 '  \\\\\
          Case "up" : pFillStyle = 103 '  /////
          Case "cross" : pFillStyle = 104 '  +++++
          Case "diagcross" : pFillStyle = 105 '  xxxxx
          Case Else : pFillStyle = -1 'Default to no fill
        End Select
      End If
    End Set
  End Property
  Public ReadOnly Property FillStyleNumeric() As Integer
    Get
      FillStyleNumeric = pFillStyle
    End Get
  End Property

  Public Property LabelColor() As System.Drawing.Color
    Get
      LabelColor = pLabelColor
    End Get
    Set(ByVal Value As System.Drawing.Color)
      pLabelColor = Value
    End Set
  End Property

  Public Property LabelsOn() As Boolean
    Get
      LabelsOn = pLabelsOn
    End Get
    Set(ByVal Value As Boolean)
      pLabelsOn = Value
    End Set
  End Property

  Public Property LabelFontName() As String
    Get
      LabelFontName = pLabelFontName
    End Get
    Set(ByVal Value As String)
      pLabelFontName = Value
    End Set
  End Property

  Public Property LabelBold() As Boolean
    Get
      LabelBold = pLabelBold
    End Get
    Set(ByVal Value As Boolean)
      pLabelBold = Value
    End Set
  End Property

  Public Property LabelItalic() As Boolean
    Get
      LabelItalic = pLabelItalic
    End Get
    Set(ByVal Value As Boolean)
      pLabelItalic = Value
    End Set
  End Property

  Public Property LabelSize() As Single
    Get
      LabelSize = pLabelSize
    End Get
    Set(ByVal Value As Single)
      pLabelSize = Value
    End Set
  End Property

  Public Property LabelStrikeOut() As Boolean
    Get
      LabelStrikeOut = pLabelStrikeOut
    End Get
    Set(ByVal Value As Boolean)
      pLabelStrikeOut = Value
    End Set
  End Property

  Public Property LabelUnderline() As Boolean
    Get
      LabelUnderline = pLabelUnderline
    End Get
    Set(ByVal Value As Boolean)
      pLabelUnderline = Value
    End Set
  End Property

  Public Property LabelWeight() As Single
    Get
      LabelWeight = pLabelWeight
    End Get
    Set(ByVal Value As Single)
      pLabelWeight = Value
    End Set
  End Property

  Public Property LineColor() As System.Drawing.Color
    Get
      LineColor = pLineColor
    End Get
    Set(ByVal Value As System.Drawing.Color)
      pLineColor = Value
    End Set
  End Property

  'SOLID = 0
  'DASH = 1        '  -------
  'DOT = 2         '  .......
  'DASHDOT = 3     '  _._._._
  'DASHDOTDOT = 4  '  _.._.._

  Public Property LineStyle() As String
    Get
      Select Case pLineStyle
        Case 0 : LineStyle = "Solid"
        Case 1 : LineStyle = "Dash"
        Case 2 : LineStyle = "Dot"
        Case 3 : LineStyle = "DashDot"
        Case 4 : LineStyle = "DashDotDot"
        Case 8 : LineStyle = "Alternate"
        Case Else : LineStyle = CStr(pLineStyle)
      End Select
    End Get
    Set(ByVal Value As String)
      If IsNumeric(Value) Then
        pLineStyle = CInt(Value)
      Else
        Select Case LCase(Value)
          Case "solid" : pLineStyle = 0
          Case "dash" : pLineStyle = 1
          Case "dot" : pLineStyle = 2
          Case "dashdot" : pLineStyle = 3
          Case "dashdotdot" : pLineStyle = 4
          Case "alternate" : pLineStyle = 8
          Case Else : pLineStyle = 0 : System.Diagnostics.Debug.WriteLine("Unknown Line Style: " & Value & ", defaulting to Solid")
        End Select
      End If
    End Set
  End Property
  Public ReadOnly Property LineStyleNumeric() As Integer
    Get
      LineStyleNumeric = pLineStyle
    End Get
  End Property

  Public Property LineWidth() As Integer
    Get
      LineWidth = pLineWidth
    End Get
    Set(ByVal Value As Integer)
      pLineWidth = Value
    End Set
  End Property

  Public ReadOnly Property MarkBitsHeight() As Integer
    Get
      MarkBitsHeight = pMarkBitsH
    End Get
  End Property

  Public ReadOnly Property MarkBitsWidth() As Integer
    Get
      MarkBitsWidth = pMarkBitsW
    End Get
  End Property

  Public Property MarkColor() As System.Drawing.Color

    Get
      MarkColor = pMarkColor
    End Get
    Set(ByVal Value As System.Drawing.Color)
      pMarkColor = Value
    End Set
  End Property

  Public Property MarkSize() As Integer
    Get
      MarkSize = pMarkSize
    End Get
    Set(ByVal Value As Integer)
      pMarkSize = Value
    End Set
  End Property

  Public ReadOnly Property MarkStyleNumeric() As Integer
    Get
      MarkStyleNumeric = pMarkStyle
    End Get
  End Property
  Public Property MarkStyle() As String
    Get
      Select Case pMarkStyle
        Case 0 : MarkStyle = "Circle"
        Case 1 : MarkStyle = "Square"
        Case 2 : MarkStyle = "Cross"
        Case 3 : MarkStyle = "X"
        Case 4 : MarkStyle = "Diamond"
        Case 100 : MarkStyle = "Bitmap"
        Case Else : MarkStyle = ""
      End Select
    End Get
    Set(ByVal Value As String)
      If IsNumeric(Value) Then
        pMarkStyle = CInt(Value)
      Else
        Select Case LCase(Value)
          Case "Circle" : pMarkStyle = 0
          Case "Square" : pMarkStyle = 1
          Case "Cross" : pMarkStyle = 2
          Case "X" : pMarkStyle = 3
          Case "Diamond" : pMarkStyle = 4
          Case "Bitmap" : pMarkStyle = 100
          Case Else : pMarkStyle = -1
        End Select
      End If
    End Set
  End Property

  Public ReadOnly Property MarkBit(ByVal x As Integer, ByVal y As Integer) As Boolean
    Get
      On Error Resume Next 'bits outside bitmap are false
      MarkBit = pMarkBits(x, y)
    End Get
  End Property

  Private Function ColorFromName(ByVal aName As String) As System.Drawing.Color
    Dim tmpColor As System.Drawing.Color
    Dim grayLevel As Integer
    'TODO: make sure system colors work well here
    If IsNumeric(aName) Then
      tmpColor = System.Drawing.Color.FromArgb(CInt(aName))
    ElseIf aName.StartsWith("gray") And IsNumeric(Mid(aName, 5)) Then
      grayLevel = CInt(Mid(aName, 5))
      tmpColor = System.Drawing.Color.FromArgb(grayLevel, grayLevel, grayLevel)
    Else
      tmpColor = System.Drawing.Color.FromName(aName)
      'TODO: What should we do if color name is unknown? Defaulting to black now.
      'If tmpColor.Equals(System.Drawing.Color.Black) And _
      '    LCase(aName) <> "black" Then

      'End If
    End If
    Return tmpColor
  End Function

  Private Function ColorName(ByVal aColor As System.Drawing.Color) As String
    'TODO: make sure system colors work well here
    If aColor.IsNamedColor Then
      Return aColor.Name
    Else
      If aColor.R = aColor.G And aColor.R = aColor.B Then
        Return "gray" & aColor.R
      Else
        Return CStr(aColor.ToArgb())
      End If
    End If
  End Function

  Public Property xml() As Chilkat.Xml
    Get
      xml = New Chilkat.Xml
      With xml
        .Tag = "RenderStyle"
        .Content = pName

        If Len(Value) > 0 Then .AddAttribute("Value", Value)
        If operator <> "=" Then .AddAttribute("Operator", operator)

        If Not FillColor.IsEmpty Then .AddAttribute("FillColor", FillColor.Name)
        If Len(FillStyle) > 0 Then .AddAttribute("FillStyle", FillStyle)

        If Not LabelColor.IsEmpty Then
          .AddAttribute("LabelColor", LabelColor.Name)
          If Not LabelsOn Then .AddAttribute("LabelsOn", "false")
        End If
        If pLabelFontName.Length > 0 Then
          .AddAttribute("LabelFont", pLabelFontName)
          If pLabelBold Then .AddAttribute("LabelBold", "true")
          If pLabelItalic Then .AddAttribute("LabelItalic", "true")
          If pLabelStrikeOut Then .AddAttribute("LabelStrikethrough", "true")
          If pLabelUnderline Then .AddAttribute("LabelUnderline", "true")
          If pLabelSize > 0 Then .AddAttribute("LabelSize", CStr(pLabelSize))
        End If

        If Not pLineColor.IsEmpty And _
           Not pLineColor.Equals(System.Drawing.Color.Black) Then
          .AddAttribute("LineColor", pLineColor.Name)
        End If
        If Len(LineStyle) > 0 Then .AddAttribute("LineStyle", LineStyle)
        If LineWidth <> 1 Then .AddAttribute("LineWidth", CStr(LineWidth))

        If Not MarkColor.IsEmpty Then .AddAttribute("MarkColor", MarkColor.Name)
        If MarkSize > 0 Then .AddAttribute("MarkSize", CStr(MarkSize))
        If pMarkStyle >= 0 Then .AddAttribute("MarkStyle", MarkStyle)
        If MarkBitsWidth > 0 And MarkBitsHeight > 0 Then
          .AddAttribute("MarkBitsWidth", CStr(MarkBitsWidth))
          .AddAttribute("MarkBitsHeight", CStr(MarkBitsHeight))
          .AddAttribute("MarkBits", MarkBitsAsHex)
        End If
      End With
    End Get
    Set(ByVal Value As Chilkat.Xml)
      Dim iAttribute As Integer
      Dim attValue As String
      Dim lmarkBitsWidth As Integer
      Dim lmarkBitsHeight As Integer
      Dim labelsOnFound As Boolean
      Dim fillStyleFound As Boolean
      Dim fillColorFound As Boolean
      'Clear 'Not clearing so we can inherit default properties. Call Clear before setting xml if desired
      Name = Value.Content
      For iAttribute = 0 To Value.NumAttributes - 1
        attValue = Value.GetAttributeValue(iAttribute)
        Select Case LCase(Value.GetAttributeName(iAttribute))
          Case "value" : Me.Value = attValue
          Case "operator" : operator = attValue

          Case "fillcolor" : FillColor = ColorFromName(attValue) : fillColorFound = True
          Case "fillstyle" : FillStyle = attValue : fillStyleFound = True

          Case "labelcolor" : LabelColor = ColorFromName(attValue)
          Case "labelbold" : pLabelBold = CBool(attValue)
          Case "labelfont" : pLabelFontName = attValue
          Case "labelitalic" : pLabelItalic = CBool(attValue)
          Case "labelstrikethrough" : pLabelStrikeOut = CBool(attValue)
          Case "labelunderline" : pLabelUnderline = CBool(attValue)
          Case "labelsize" : pLabelSize = CSng(attValue)
          Case "labelson"
            labelsOnFound = True
            Select Case LCase(attValue)
              Case "off", "no", "0", "", "false" : LabelsOn = False
              Case Else : LabelsOn = True
            End Select
          Case "linecolor" : LineColor = ColorFromName(attValue)
          Case "linestyle" : LineStyle = attValue
          Case "linewidth" : LineWidth = CInt(attValue)

          Case "markcolor" : MarkColor = ColorFromName(attValue)
          Case "marksize" : MarkSize = CInt(attValue)
          Case "markstyle" : MarkStyle = attValue
          Case "markbitswidth" : If IsNumeric(attValue) Then lmarkBitsWidth = CInt(attValue)
          Case "markbitsheight" : If IsNumeric(attValue) Then lmarkBitsHeight = CInt(attValue)
            'MarkBitsWidth and MarkBitsHeight must be specified before MarkBits or a square will be assumed
          Case "markbits" : SetMarkBits(attValue, lmarkBitsWidth, lmarkBitsHeight)

          Case Else : System.Diagnostics.Debug.WriteLine("Warning: Unknown attribute '" & Value.GetAttributeName(iAttribute) & "' in " & Value.GetXml)
        End Select
      Next
      If Not labelsOnFound And Not LabelColor.IsEmpty Then LabelsOn = True
      If Not fillStyleFound And fillColorFound And pFillStyle = -1 Then pFillStyle = 0
    End Set
  End Property

  Public Sub SetMarkBits(ByVal aBits As String, Optional ByRef aWidth As Integer = 0, Optional ByRef aHeight As Integer = 0)
    Dim bitsToUse As String
    Dim iChar As Integer
    Dim hexit As String
    Dim byteValue As Integer
    Dim curBit As Integer
    Dim x As Integer
    Dim y As Integer

    pMarkStyle = 100 'Bitmap style

    'Strip out spaces and other non-hex characters
    For iChar = 1 To Len(aBits)
      hexit = UCase(Mid(aBits, iChar, 1))
      Select Case Asc(hexit)
        '48 to 57 = digits, 65 to 70 = ABCDEF
      Case 48 To 57, 65 To 70 : bitsToUse = bitsToUse & hexit
      End Select
    Next

    If aWidth = 0 Then
      If aHeight > 0 Then
        aWidth = (Len(bitsToUse) * 4) / aHeight
      Else
        aWidth = System.Math.Sqrt(Len(bitsToUse) * 4)
        aHeight = aWidth
      End If
    End If
    If aHeight = 0 Then
      aHeight = (Len(bitsToUse) * 4) / aWidth
    End If
    pMarkBitsW = aWidth
    pMarkBitsH = aHeight
    ReDim pMarkBits(pMarkBitsW - 1, pMarkBitsH - 1)

    x = 0
    y = 0
    For iChar = 1 To Len(bitsToUse)
      hexit = Mid(bitsToUse, iChar, 1)
      If IsNumeric(hexit) Then
        byteValue = CInt(hexit)
      Else
        byteValue = 10 + Asc(hexit) - Asc("A")
      End If
      curBit = 8
      While curBit > 0
        If (curBit And byteValue) Then
          pMarkBits(x, y) = True
          '        Debug.Print "X";
          '      Else
          '        Debug.Print " ";
        End If
        x = x + 1
        If x = aWidth Then
          x = 0
          y = y + 1
          'Debug.Print
        End If
        curBit = curBit / 2
      End While
    Next
  End Sub

  Public Function Matches(ByRef testValue As Object) As Boolean
    If pIsNumeric Then
      Select Case pOperator
        Case -2 : If testValue < pValueNum Then Matches = True
        Case -1 : If testValue <= pValueNum Then Matches = True
        Case 0 : If testValue = pValueNum Then Matches = True
        Case 1 : If testValue >= pValueNum Then Matches = True
        Case 2 : If testValue > pValueNum Then Matches = True
      End Select
    ElseIf Len(pValue) = 0 Then
      Matches = True
    Else
      Select Case pOperator
        Case -2 : If testValue < pValue Then Matches = True
        Case -1 : If testValue <= pValue Then Matches = True
        Case 0 : If testValue = pValue Then Matches = True
        Case 1 : If testValue >= pValue Then Matches = True
        Case 2 : If testValue > pValue Then Matches = True
      End Select
    End If
  End Function

  Public Sub Clear()

    pValue = ""
    pOperator = 0

    If Not pFillColor.IsEmpty Then pFillColor = System.Drawing.Color.Gray
    FillStyle = "None"

    If Not pLineColor.IsEmpty Then pLineColor = System.Drawing.Color.Black
    pLineStyle = 0
    pLineWidth = 1

    pMarkBitsW = 0
    pMarkBitsH = 0

    If Not pMarkColor.IsEmpty Then pMarkColor = System.Drawing.Color.Black
    pMarkSize = 0
    pMarkStyle = -1

    If Not pLabelColor.IsEmpty Then pLabelColor = System.Drawing.Color.Black
    'pLabelFont = System.Windows.Forms.Control.DefaultFont.Clone()
    pLabelFontName = ""
    pLabelBold = False
    pLabelItalic = False
    pLabelStrikeOut = False
    pLabelUnderline = False
    pLabelSize = 0
    pLabelsOn = False
  End Sub

  Public Function copy() As atcRenderStyle
    copy = New atcRenderStyle

    copy.Value = pValue
    copy.operator = operator

    copy.FillColor = pFillColor
    copy.FillStyle = CStr(pFillStyle)

    copy.LabelColor = pLabelColor
    copy.LabelBold = pLabelBold
    copy.LabelItalic = pLabelItalic
    copy.LabelFontName = pLabelFontName
    copy.LabelSize = pLabelSize
    copy.LabelStrikeOut = pLabelStrikeOut
    copy.LabelUnderline = pLabelUnderline
    copy.LabelWeight = pLabelWeight
    copy.LabelsOn = pLabelsOn

    copy.LineColor = pLineColor
    copy.LineWidth = pLineWidth
    copy.LineStyle = CStr(pLineStyle)

    copy.MarkColor = pMarkColor
    copy.MarkSize = pMarkSize
    copy.MarkStyle = CStr(pMarkStyle)

    If pMarkBitsH > 0 And pMarkBitsW > 0 Then
      copy.SetMarkBits(MarkBitsAsHex, pMarkBitsW, pMarkBitsH)
    End If

  End Function

  Public Function MarkBitsAsHex() As String
    Dim x, y As Integer
    Dim curByte As Integer
    MarkBitsAsHex = ""
    For y = 0 To pMarkBitsH - 1
      For x = 0 To pMarkBitsW - 1 Step 4
        curByte = 0
        If pMarkBits(x, y) Then curByte = curByte + 8
        If pMarkBits(x + 1, y) Then curByte = curByte + 4
        If pMarkBits(x + 2, y) Then curByte = curByte + 2
        If pMarkBits(x + 3, y) Then curByte = curByte + 1
        MarkBitsAsHex = MarkBitsAsHex & Hex(curByte)
      Next
      If y < pMarkBitsH - 1 Then MarkBitsAsHex = MarkBitsAsHex & " "
    Next
  End Function

  'Returns xml that leaves out the differences between this style and the given default
  Public Function xmlMinusDefault(ByRef aDefault As atcRenderStyle) As Object
    xmlMinusDefault = New Chilkat.Xml
    With xmlMinusDefault
      .Tag = "RenderStyle"
      .Content = pName
      If Value <> aDefault.Value Then .AddAttribute("Value", Value)
      If operator <> aDefault.operator Then .AddAttribute("Operator", operator)
      If Not FillColor.Equals(aDefault.FillColor) Then .AddAttribute("FillColor", ColorName(FillColor))
      If FillStyle <> aDefault.FillStyle Then .AddAttribute("FillStyle", FillStyle)
      If Not LabelColor.Equals(aDefault.LabelColor) Then .AddAttribute("LabelColor", ColorName(LabelColor))
      If LabelsOn <> aDefault.LabelsOn Then .AddAttribute("LabelsOn", CStr(LabelsOn))
      If pLabelFontName.Length > 0 And pLabelFontName <> aDefault.LabelFontName Then
        .AddAttribute("LabelFont", pLabelFontName)
      End If
      If pLabelBold <> aDefault.LabelBold Then .AddAttribute("LabelBold", CStr(pLabelBold))
      If pLabelItalic <> aDefault.LabelItalic Then .AddAttribute("LabelItalic", CStr(pLabelItalic))
      If pLabelStrikeOut <> aDefault.LabelStrikeOut Then .AddAttribute("LabelStrikethrough", CStr(pLabelStrikeOut))
      If pLabelUnderline <> aDefault.LabelUnderline Then .AddAttribute("LabelUnderline", CStr(pLabelUnderline))
      If pLabelSize > 0 And pLabelSize <> aDefault.LabelSize Then
        .AddAttribute("LabelSize", pLabelSize)
      End If
      If pLabelWeight > 0 And pLabelWeight <> aDefault.LabelWeight Then
        .AddAttribute("LabelWeight", pLabelWeight)
      End If
      If Not LineColor.Equals(aDefault.LineColor) Then .AddAttribute("LineColor", ColorName(pLineColor))
      If LineStyle <> aDefault.LineStyle Then .AddAttribute("LineStyle", LineStyle)
      If LineWidth <> aDefault.LineWidth Then .AddAttribute("LineWidth", LineWidth)
      If Not MarkColor.Equals(aDefault.MarkColor) Then .AddAttribute("MarkColor", ColorName(MarkColor))
      If MarkSize <> aDefault.MarkSize Then .AddAttribute("MarkSize", MarkSize)
      If MarkStyle <> aDefault.MarkStyle Then .AddAttribute("MarkStyle", MarkStyle)
      If MarkBitsWidth <> aDefault.MarkBitsWidth Then .AddAttribute("MarkBitsWidth", MarkBitsWidth)
      If MarkBitsHeight <> aDefault.MarkBitsWidth Then .AddAttribute("MarkBitsHeight", MarkBitsHeight)
      If MarkBitsAsHex() <> aDefault.MarkBitsAsHex Then .AddAttribute("MarkBits", MarkBitsAsHex)
    End With
  End Function

  Public Sub New()
    MyBase.New()
    Clear()
  End Sub
End Class