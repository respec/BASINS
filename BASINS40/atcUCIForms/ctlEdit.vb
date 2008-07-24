''' <summary>
''' Interface for controls used on frmEdit
''' </summary>
''' <remarks></remarks>
Public Interface ctlEdit
    Event Change(ByVal aChange As Boolean)
    ReadOnly Property Caption() As String
    Property Changed() As Boolean
    Property Data() As Object
    ReadOnly Property Width() As Integer
    ReadOnly Property Height() As Integer
    Sub Add()
    Sub Help()
    Sub Remove()
    Sub Save()
End Interface
