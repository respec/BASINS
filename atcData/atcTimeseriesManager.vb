'Manages a set of currently open data files
'Uses the set of plugins currently loaded to find ones that inherit atcTimeseriesFile

Imports System.Reflection

Public Class atcTimeseriesManager
  Private pMapWin As MapWindow.Interfaces.IMapWin
  Private pFiles As ArrayList 'of atcTimeseriesFile

  Public Sub New(ByVal aMapWin As MapWindow.Interfaces.IMapWin)
    pMapWin = aMapWin
    pFiles = New ArrayList
  End Sub

  'The set of atcTimeseriesFile objects representing currently open files
  Public ReadOnly Property Files() As ICollection
    Get
      Return pFiles
    End Get
  End Property

  'The currently loaded plugins that inherit atcTimeseriesFile
  'All are empty atcTimeseriesFile objects.
  Public Function TimeseriesFilePlugins() As ICollection
    Dim retval As New ArrayList
    Dim lastPlugIn As Integer = pMapWin.Plugins.Count() - 1
    For iPlugin As Integer = 0 To lastPlugIn
      Dim curPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
      If Not curPlugin Is Nothing Then
        Dim pluginName As String = curPlugin.Name
        If CType(curPlugin, Object).GetType().IsSubclassOf(GetType(atcTimeseriesFile)) Then
          retval.Add(curPlugin)
        End If
      End If
    Next
    Return retval
  End Function

  'The currently loaded plugins that inherit atcDataPlugin but not atcTimeseriesFile
  'All are empty atcDataPlugin objects.
  Public Function TimeseriesDataPlugins() As ICollection
    Dim retval As New ArrayList
    Dim lastPlugIn As Integer = pMapWin.Plugins.Count() - 1
    For iPlugin As Integer = 0 To lastPlugIn
      Dim curPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
      If Not curPlugin Is Nothing Then
        Dim pluginName As String = curPlugin.Name
        Dim pluginType As System.Type = CType(curPlugin, Object).GetType()
        If pluginType.IsSubclassOf(GetType(atcDataPlugin)) Then
          If Not pluginType.IsSubclassOf(GetType(atcTimeseriesFile)) Then
            retval.Add(curPlugin)
          End If
        End If
      End If
    Next
    Return retval
  End Function


  'Open a file and return the new atcTimeseriesFile object
  'aFileFilter: selected filter in Open dialog - used to determine which class can open file
  'if aFileFilter is omitted, Open tries searching for a class that supports the extension of aFileName
  Public Function Open(ByVal aFileName As String, Optional ByVal aFileFilter As String = "") As atcTimeseriesFile
    Dim newFile As atcTimeseriesFile

    If aFileFilter.Length = 0 Then aFileFilter = System.IO.Path.GetExtension(aFileName)
    aFileFilter = aFileFilter.ToLower

    For Each atf As atcTimeseriesFile In TimeseriesFilePlugins()
      'Might need a better test than this, or try more than one if multiple types open 
      'files with the same extension or if filter is not set for a atcTimeseriesFile type
      If atf.FileFilter.ToLower.IndexOf(aFileFilter) >= 0 Then
        Dim typ As System.Type = atf.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        newFile = asm.CreateInstance(typ.FullName)
      End If
    Next

    If newFile Is Nothing Then
      'TODO: how do we want to handle this?
      'Err.Raise(0, Me, "Could not find a loaded plugin that can open file '" & aFileName & "'")
    Else
      If newFile.Open(aFileName) Then
        pFiles.Add(newFile)
        Return newFile
      Else
        'TODO: handle false return from Open
        'Err.Raise(0, Me, "Could open file '" & aFileName & "' with '" & newFile.Name & "'")
      End If
    End If
    'Should not get here - either an error will be raised or newFile returned above
    Return Nothing 'Failed to find appropriate atcTimeseriesFile class or failed to open file
  End Function

  'Returns FileFilters of all loaded atcTimeseriesFile types, formatted for common dialog
  Public Function FileFilters() As String
    Dim retval As String = ""
    For Each atf As atcTimeseriesFile In TimeseriesFilePlugins()
      If retval.Length > 0 Then retval += "|" 'separate with |
      retval += atf.FileFilter
    Next
    Return retval
  End Function
End Class
