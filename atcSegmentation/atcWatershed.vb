Public Class Watershed
    Public LandUses As LandUses
    Public Reaches As Reaches
    Public Channels As Channels
    Public PointLoads As PointLoads
    Public MetSegments As MetSegments
    Public Name As String = ""

    Public Function Open(ByVal aName As String) As Integer
        Name = aName
        Dim lReturnCode As Integer = 0
        If Not Reaches Is Nothing Then Reaches = Nothing
        Reaches = New Reaches
        lReturnCode += Me.Reaches.Open(Me)

        If Not LandUses Is Nothing Then LandUses = Nothing
        LandUses = New LandUses
        lReturnCode += Me.LandUses.Open(Me)

        If Not Channels Is Nothing Then Channels = Nothing
        Channels = New Channels
        lReturnCode += Me.Channels.Open(Me)

        If Not PointLoads Is Nothing Then PointLoads = Nothing
        PointLoads = New PointLoads
        lReturnCode += Me.PointLoads.Open(Me)

        If Not MetSegments Is Nothing Then MetSegments = Nothing
        MetSegments = New MetSegments
        If Me.MetSegments.Open(Me) <> 0 Then
            MetSegments = Nothing
        End If

        Return lReturnCode
    End Function
End Class
