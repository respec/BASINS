' sample code for defining a custom attribute
' don't use - see atcAttributeDefinition and atcDefinedValue instead
'
Public Sub Sample()
    Dim lParent As New Parent
    Dim lIsMetric As Boolean = False
    Dim lStr As String = Units.AsString(lParent, "Area", lIsMetric)
End Sub

Public Class Parent
    <UnitsAttribute("acres", "hectares")> Public Area As Double = 0.0
End Class

Public Class UnitsAttribute
    Inherits System.Attribute
    Public English As String = ""
    Public Metric As String = ""

    Public Sub New(ByVal aEnglish As String, Optional ByVal aMetric As String = "none")
        English = aEnglish
        If aMetric = "none" AndAlso aEnglish <> "none" Then
            Metric = English
        End If
        Metric = aMetric
    End Sub
    Public Function AsString(ByVal aMetric As Boolean) As String
        If aMetric Then Return Metric Else Return English
    End Function
    Public Shared Function AsString(ByRef aObject As Object, ByVal aFieldName As String, ByVal aMetric As Boolean) As String
        Dim lType As Type = aObject.GetType
        Dim lMemberInfo As Reflection.MemberInfo = lType.GetField(aFieldName)
        If lMemberInfo Is Nothing Then
            lMemberInfo = lType.GetProperty(aFieldName)
        End If
        If lMemberInfo Is Nothing Then
            Return "none"
        Else
            Dim lUnits As New UnitsAttribute("none")
            lUnits = Attribute.GetCustomAttribute(lMemberInfo, lUnits.GetType)
            If lUnits Is Nothing Then
                Return "none"
            Else
                Return lUnits.AsString(aMetric)
            End If
        End If
    End Function
End Class
