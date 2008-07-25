''' <summary>
''' Interface for controls used on frmEdit
''' </summary>
''' <remarks></remarks>
Public Interface ctlEdit
    Event Change(ByVal aChange As Boolean)
    ReadOnly Property Caption() As String
    Property Changed() As Boolean
    Property Data() As Object
    Sub Add()
    Sub Help()
    Sub Remove()
    Sub Save()
End Interface
