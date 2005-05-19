Public Class atcTimeseriesFile
  Inherits atcDataPlugin

  Private pFileName As String
  Private pTimeseries As ArrayList 'of atcTimeseries

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
      Return pFileName
    End Get
    Set(ByVal newValue As String)
      pFileName = newValue
    End Set
  End Property

  'The atcTimeseries objects in this file
  Public Overridable ReadOnly Property Timeseries() As ArrayList
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

  'Opens named file and reads enough to determine whether file is correct type
  'Returns True if file is successfully opened
  Public Overridable Function Open(ByVal newFileName As String) As Boolean
    Err.Raise(0, Me, "Open must be overridden to be used, atcTimeseriesFile does not implement.")
  End Function

  'Read all the data into an atcTimeseries (which must be from this file)
  'Called only from within atcTimeseries.EnsureValuesRead.
  Public Overridable Sub ReadData(ByVal aTimeseries As atcTimeseries)
    Err.Raise(0, Me, "ReadData must be overridden to be used, atcTimeseriesFile does not implement.")
  End Sub

  'Save all current data to a new file
  'Or save recently updated data to current file if SaveFileName = Me.FileName
  'Returns True if successful
  Public Overridable Function Save(ByVal SaveFileName As String) As Boolean
    Err.Raise(0, Me, "Save must be overridden to be used, atcTimeseriesFile does not implement.")
  End Function

  Public Sub New()
    pTimeseries = Nothing
    pTimeseries = New ArrayList
  End Sub
End Class
