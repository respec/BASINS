Public Class atcSWMMReport
    Implements IBlock

    Private pName As String
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        Name = "[REPORT]"
        pSWMMProject = aSWMMPRoject
    End Sub

    'Temporarily just save the text for now
    Private pText As String
    Public Property Text() As String
        Get
            Return pText
        End Get
        Set(ByVal value As String)
            pText = value
        End Set
    End Property
    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'TODO
        Text = aContents
    End Sub

    Public Overrides Function ToString() As String
        Return Text()
    End Function
End Class
