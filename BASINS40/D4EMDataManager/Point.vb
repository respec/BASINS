
'Imports MapWinUtility
'Imports System.Xml

'Public Class Point
'    'TODO: make this a base class, move code that depends on kind of Region (box, circle, polygon) into subclasses

'    Friend pX As Double = atcUtility.GetNaN
'    Friend pY As Double = atcUtility.GetNaN
'    Friend pProjection As String = ""


'    Public Sub New(ByVal aX As Double, _
'                   ByVal aY As Double, _
'                   ByVal aProjection As String)
'        pX = aX
'        pY = aY

'        pProjection = aProjection
'        Validate()
'    End Sub

'    Public Sub New(ByVal aArgs As Xml.XmlNode)
'        SetXML(aArgs)
'    End Sub

'    Public Property X() As Integer
'        Get
'            Return pX
'        End Get
'        Set(ByVal Value As Integer)
'            pX = Value
'        End Set
'    End Property

'    Public Property Y() As Integer
'        Get
'            Return pY
'        End Get
'        Set(ByVal Value As Integer)
'            pY = Value
'        End Set
'    End Property

'    Public Property Projection() As String
'        Get
'            Return pProjection
'        End Get
'        Set(ByVal Value As String)
'            pProjection = Value
'        End Set
'    End Property

'    Private Sub SetXML(ByVal node As Xml.XmlNode)

'        Dim xcoord As String
'        Dim ycoord As String

'        Dim tmpNode As XmlNode

'        tmpNode = node.SelectSingleNode("//point/x")
'        xcoord = tmpNode.InnerText
'        pX = Convert.ToDouble(xcoord)

'        tmpNode = node.SelectSingleNode("//point/y")
'        ycoord = tmpNode.InnerText
'        pY = Convert.ToDouble(ycoord)

'        pProjection = node.SelectSingleNode("//point/projection").InnerText

'        Validate()
'    End Sub

'    Public Overridable Function XML() As String

'        Dim ptXml As String
'        ptXml = "<point>" & vbCrLf _
'             & "<x>" & pX & "</x>" & vbCrLf _
'             & "<y>" & pY & "</y>" & vbCrLf _
'             & "<projection>" & pProjection & "</projection>" & vbCrLf _
'             & "</point>" & vbCrLf

'        Return ptXml

'    End Function

'    Public Overridable Sub Validate()

'        If Double.IsNaN(pX) Then
'            Throw New ApplicationException("X coordinate not specified")
'        ElseIf Double.IsNaN(pY) Then
'            Throw New ApplicationException("Y coordinate not specified")
'        ElseIf pProjection.Length = 0 Then
'            Throw New ApplicationException("Projection not specified")

'        End If
'    End Sub

'    Public Overrides Function ToString() As String
'        Return "Point X coordinate " & pX & " Y coordinate " & pY
'    End Function
'End Class
