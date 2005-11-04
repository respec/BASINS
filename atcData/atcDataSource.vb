Public Class atcDataSource
  Inherits atcDataPlugin

  Private Shared pDataManager As atcDataManager

  Private pAttributes As atcDataAttributes
  Private pSpecification As String
  Private pData As atcDataGroup

  Public Enum EnumExistAction
    ExistNoAction = 0
    ExistReplace = 1
    ExistAppend = 2
    ExistRenumber = 4
    ExistAskUser = 8
  End Enum

  ''' <summary>Attributes associated with all the DataSets (location, constituent, etc.)</summary>
  Public ReadOnly Property Attributes() As atcDataAttributes
    Get
      Return pAttributes
    End Get
  End Property

  ''' <summary>atcAttributeDefinitions representing operations supported by ComputeTimeseries</summary
  Public Overridable ReadOnly Property AvailableOperations() As atcDataAttributes
    Get
      Return New atcDataAttributes 'default to an empty list with nothing available
    End Get
  End Property

  ''' <summary>The atcDataSet objects in this file</summary>
  Public Overridable ReadOnly Property DataSets() As atcDataGroup
    Get
      Return pData
    End Get
  End Property

  ''' <summary>True if Open is implemented - files can be read</summary>
  Public Overridable ReadOnly Property CanOpen() As Boolean
    Get
      Return False
    End Get
  End Property

  ''' <summary>True if Save is implemented - files can be saved</summary>
  Public Overridable ReadOnly Property CanSave() As Boolean
    Get
      Return False
    End Get
  End Property

  ''' <summary>Opens file or database and reads enough to determine whether it is correct type. Returns True if successfully opened.</summary>
  ''' aSpecification = file name, connect string, or other string needed to open
  ''' <param name="aAttributes">
  '''     <para>additional information which may be used for opening and will be saved as Attributes</para>
  ''' </param>   
  Public Overridable Function Open(ByVal aSpecification As String, Optional ByVal aAttributes As atcDataAttributes = Nothing) As Boolean
    Throw New Exception("Open must be overridden to be used, atcDataSource does not implement.")
  End Function

  'Read all the data into an atcDataSet (which must be from this file)
  'Called only from within atcData.EnsureValuesRead.
  Public Overridable Sub ReadData(ByVal aData As atcDataSet)
    Err.Raise(0, Me, "ReadData must be overridden to be used, atcDataSource does not implement.")
  End Sub

  'Save all current data to a new file
  'Or save recently updated data to current file if SaveFileName = Me.FileName
  'Returns True if successful
  Public Overridable Function Save(ByVal SaveFileName As String, _
         Optional ByRef ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
    Err.Raise(0, Me, "Save must be overridden to be used, atcDataSource does not implement.")
  End Function

  'Name of file we are reading and/or writing, including full path
  'Or connection string for database
  'Or which computation is being performed
  'Should only be set by inheriting class, only during Open or Save
  Public Overridable Property Specification() As String
    Get
      Return pSpecification
    End Get
    Set(ByVal newValue As String)
      pSpecification = newValue
    End Set
  End Property

  Public Overridable Function AddDataSet(ByRef t As atcDataSet, _
         Optional ByRef ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
    If pData.Contains(t) Then
      Return False 'can't add, already have it
    Else
      pData.Add(t)
      Return True
    End If
  End Function

  Public Property DataManager() As atcDataManager
    Get
      Return pDataManager
    End Get
    Set(ByVal newValue As atcDataManager)
      pDataManager = newValue
    End Set
  End Property

  Public Sub New()
    pData = New atcDataGroup
  End Sub

End Class
