Public Class atcDataSource
  Inherits atcDataPlugin

  Private pFileNameOrConnectString As String
  Private pTimeseries As atcTimeseriesGroup

  Public Enum EnumExistAction
    ExistNoAction = 0
    ExistReplace = 1
    ExistAppend = 2
    ExistRenumber = 4
    ExistAskUser = 8
  End Enum

  'Filter for files that this class can read, formatted for common dialog.
  'Returns empty string if there is no particular filter that would work.
  Public Overridable ReadOnly Property FileFilter() As String
    Get
      Return "" '"All Files (*.*)|*.*"
    End Get
  End Property

  'Name of file we are reading and/or writing, including full path
  'Should only be set by inheriting class, only during Open or Save
  Public Overridable Property FileName() As String
    Get
      Return pFileNameOrConnectString
    End Get
    Set(ByVal newValue As String)
      pFileNameOrConnectString = newValue
    End Set
  End Property

  'The atcTimeseries objects in this file
  Public Overridable ReadOnly Property Timeseries() As atcTimeseriesGroup
    Get
      Return pTimeseries
    End Get
  End Property

  'True if OpenFile is implemented - files can be read
  Public Overridable ReadOnly Property CanOpen() As Boolean
    Get
      Return False
    End Get
  End Property

  'True if SaveAs is implemented - files can be saved
  Public Overridable ReadOnly Property CanSave() As Boolean
    Get
      Return False
    End Get
  End Property

  'Opens file or database and reads enough to determine whether it is correct type
  'Returns True if successfully opened
  Public Overridable Function Open(ByVal aFileNameOrConnectString As String) As Boolean
    Err.Raise(0, Me, "Open must be overridden to be used, atcDataSource does not implement.")
  End Function

  'Read all the data into an atcTimeseries (which must be from this file)
  'Called only from within atcTimeseries.EnsureValuesRead.
  Public Overridable Sub ReadData(ByVal aTimeseries As atcTimeseries)
    Err.Raise(0, Me, "ReadData must be overridden to be used, atcDataSource does not implement.")
  End Sub

  'Save all current data to a new file
  'Or save recently updated data to current file if SaveFileName = Me.FileName
  'Returns True if successful
  Public Overridable Function Save(ByVal SaveFileName As String, _
                          Optional ByRef ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
    Err.Raise(0, Me, "Save must be overridden to be used, atcDataSource does not implement.")
  End Function

  Public Overridable Function AddTimeseries(ByRef t As atcTimeseries, _
                       Optional ByRef ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
    If pTimeseries.Contains(t) Then
      Return False 'can't add, already have it
    Else
      pTimeseries.Add(t)
      Return True
    End If
  End Function

  Public Sub New()
    pTimeseries = New atcTimeseriesGroup
  End Sub
End Class
