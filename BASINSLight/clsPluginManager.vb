Imports System.IO
Imports Microsoft.Win32
Imports MapWindow

Public Class Plugins_IPlugin
    Implements Interfaces.Plugins

    Public m_PluginList As Hashtable
    Public m_LoadedPlugins As Collection
    Public m_ApplicationPlugins As Hashtable
    Private m_PluginFolder As String
    Private m_dlg As PluginsForm
    Private m_Locked As Boolean
    Private m_ProjectedCoordToolbar As Windows.Forms.StatusBarPanel = Nothing
    Private m_AlternateCoordToolbar As Windows.Forms.StatusBarPanel = Nothing

    Public Sub New()
        MyBase.New()
        m_PluginList = New Hashtable()
        m_PluginList.Clear()
        m_LoadedPlugins = New Collection()
        m_ApplicationPlugins = New Hashtable()
        m_dlg = New PluginsForm()
    End Sub

    Protected Overrides Sub Finalize()
        m_PluginList.Clear()
        m_PluginList = Nothing
        m_LoadedPlugins = Nothing
        MyBase.Finalize()
    End Sub

    Friend Function Contains(ByVal Key As String) As Boolean
        Return m_PluginList.ContainsKey(Key)
    End Function

    Public Sub Clear() Implements Interfaces.Plugins.Clear
        m_PluginList.Clear()
    End Sub

    Friend Function LoadPlugins() As Boolean
        If m_Locked Then Exit Function

        PluginFolder = App.Path & "\Plugins"
        Dim PotentialPlugins As ArrayList = MapWinUtility.PluginManagementTools.FindPluginDLLs(PluginFolder)
        For Each PluginName As String In PotentialPlugins
            Dim info As New PluginInfo()

            Try
                If PluginName.IndexOf("RemoveMe-Script") > 0 Then
                    Kill(PluginName)
                ElseIf info.Init(PluginName, GetType(Interfaces.IPlugin).GUID) = True Then
                    If m_PluginList.ContainsKey(info.Key) = False Then
                        m_PluginList.Add(info.Key, info)
                    Else
                        'Chris Michaelis, May 18 2006, for BugZilla 171
                        Dim dupfile As String = CType(m_PluginList(info.Key), PluginInfo).FileName
                        If Not dupfile.ToLower() = PluginName.ToLower() Then
                            MapWinUtility.Logger.Msg("Warning: A duplicate plug-in has been detected." + vbCrLf + vbCrLf + _
                            "The loaded plug-in is: " + dupfile + vbCrLf + _
                            "The duplicate that was skipped: " + PluginName, MsgBoxStyle.Information, "Duplicate Plug-in Detected")
                        End If
                    End If
                End If
            Catch ex As Exception
                ' Keep it quiet because the MapWindow is loading
                MapWinUtility.Logger.Dbg(ex.ToString())
            End Try
        Next
    End Function

    Friend Sub LoadApplicationPlugins(ByVal ApplicationPluginPath As String)
        Try
            If ApplicationPluginPath = "" Then Return
            If System.IO.Directory.Exists(ApplicationPluginPath) = False Then Return

            Dim lSkipPlugins() As String = {"mwIdentifier", "mwLabeler", "TableEditor"} 'just work with MapWindow
            Dim PotentialPlugins As ArrayList = MapWinUtility.PluginManagementTools.FindPluginDLLs(ApplicationPluginPath)
            Dim info As PluginInfo
            Dim plugin As Interfaces.IPlugin
            For Each pluginName As String In PotentialPlugins
                If Array.IndexOf(lSkipPlugins, IO.Path.GetFileNameWithoutExtension(pluginName)) = -1 Then
                    Try
                        info = New PluginInfo
                        If info.Init(pluginName, GetType(Interfaces.IPlugin).GUID) Then
                            If Not info.Key Is Nothing AndAlso Not m_ApplicationPlugins.ContainsKey(info.Key) Then
                                plugin = MapWinUtility.PluginManagementTools.CreatePluginObject(pluginName, info.CreateString)
                                If Not plugin Is Nothing Then
                                    m_ApplicationPlugins.Add(info.Key, plugin)
                                    plugin.Initialize(frmMain, frmMain.Handle.ToInt32)
                                End If
                            End If
                        End If
                    Catch e As Exception
                        ' There was an error in this one plugin.  Keep going on.
                        MapWinUtility.Logger.Dbg(e.ToString())
                    End Try
                End If
            Next
        Catch e As Exception
            MapWinUtility.Logger.Dbg(e.ToString())
        End Try
    End Sub


    Public Function AddFromFile(ByVal Path As String) As Boolean Implements Interfaces.Plugins.AddFromFile
        If m_Locked Then Exit Function
        Dim info As New PluginInfo()
        Dim retval As Boolean
        Try
            retval = info.Init(Path, GetType(Interfaces.IPlugin).GUID)
            If retval = True Then
                If m_PluginList.ContainsKey(info.Key) = False Then
                    m_PluginList.Add(info.Key, info)
                Else
                    'Chris Michaelis, May 18 2006, for BugZilla 171
                    Dim dupfile As String = CType(m_PluginList(info.Key), PluginInfo).FileName
                    If Not dupfile.ToLower() = Path.ToLower() Then
                        MsgBox("Warning: A duplicate plug-in has been detected." + vbCrLf + vbCrLf + _
                        "The loaded plug-in is: " + dupfile + vbCrLf + _
                        "The duplicate that was skipped: " + Path, MsgBoxStyle.Information, "Duplicate Plug-in Detected")
                    End If
                End If
            End If
            Return retval
        Catch ex As Exception
            ShowError(ex)
            Return False
        End Try
    End Function

    Public Function AddFromDir(ByVal Path As String) As Boolean Implements Interfaces.Plugins.AddFromDir
        If m_Locked Then Exit Function
        Dim ar As ArrayList = MapWinUtility.PluginManagementTools.FindPluginDLLs(Path)
        For Each s As String In ar
            AddFromFile(s)
        Next
    End Function

    Public Function StartPlugin(ByVal Key As String) As Boolean Implements Interfaces.Plugins.StartPlugin
        Dim Plugin As Interfaces.IPlugin
        Dim info As PluginInfo

        If m_Locked Then Exit Function
        If ContainsKey(m_LoadedPlugins, Key) Then Return True
        If m_PluginList.ContainsKey(Key) Then
            Try
                info = CType(m_PluginList(Key), PluginInfo)
                Plugin = MapWinUtility.PluginManagementTools.CreatePluginObject(info.FileName, info.CreateString)
                If Plugin Is Nothing Then
                    Return False
                Else
                    m_LoadedPlugins.Add(Plugin, info.Key)
                    'TODO: do any plugins need the IMapWin?
                    Plugin.Initialize(frmMain, frmMain.Handle.ToInt32)
                    Return True
                End If
            Catch ex As Exception
                MapWinUtility.Logger.Dbg(ex.Message)
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Public Sub StopPlugin(ByVal Key As String) Implements Interfaces.Plugins.StopPlugin
        If m_Locked Then Exit Sub
        Try
            If ContainsKey(m_LoadedPlugins, Key) Then
                Dim Plugin As Interfaces.IPlugin
                If Not m_LoadedPlugins(Key) Is Nothing Then
                    Plugin = CType(m_LoadedPlugins(Key), Interfaces.IPlugin)
                    If Not Plugin Is Nothing Then
                        Try
                            Plugin.Terminate()
                        Catch ex As Exception
                            MsgBox("Warning: The plugin '" + Key + "' raised an error in its Terminate() function." + vbCrLf + vbCrLf + "The full details appear in the application log (if enabled).", MsgBoxStyle.Exclamation, "Plug-in Error Warning")
                            MapWinUtility.Logger.Dbg("Plugin Exception in " + Key + ": " + ex.ToString())
                            'Proceed
                        End Try
                        Plugin = Nothing
                    End If
                End If
                m_LoadedPlugins.Remove(Key)
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public ReadOnly Property Count() As Integer Implements Interfaces.Plugins.Count
        Get
            Return m_PluginList.Count
        End Get
    End Property

    <CLSCompliant(False)> _
    Default Public ReadOnly Property Item(ByVal Index As Integer) As Interfaces.IPlugin Implements Interfaces.Plugins.Item
        Get
            If Index <= m_LoadedPlugins.Count AndAlso Index > 0 Then  'collections are 1-based
                Return CType(m_LoadedPlugins(Index), Interfaces.IPlugin)
            End If

            Return Nothing
        End Get
    End Property

    Public Sub Remove(ByVal IndexOrKey As Object) Implements Interfaces.Plugins.Remove
        If m_Locked Then Exit Sub
        Try
            Select Case LCase(TypeName(IndexOrKey))
                Case "string", "long", "integer", "short"
                    If ContainsKey(m_LoadedPlugins, IndexOrKey) Then StopPlugin(CStr(IndexOrKey))
                    m_PluginList.Remove(IndexOrKey)
            End Select
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Friend ReadOnly Property LoadedPlugins() As Collection
        Get
            Return m_LoadedPlugins
        End Get
    End Property

    Friend ReadOnly Property PluginsList() As Hashtable
        Get
            Return m_PluginList
        End Get
    End Property

    Public Property PluginFolder() As String Implements Interfaces.Plugins.PluginFolder
        Get
            Return m_PluginFolder
        End Get
        Set(ByVal Value As String)
            m_PluginFolder = Value
        End Set
    End Property

    Public Function PluginIsLoaded(ByVal Key As String) As Boolean Implements Interfaces.Plugins.PluginIsLoaded
        Return ContainsKey(m_LoadedPlugins, Key)
    End Function

    Public Sub ShowPluginDialog() Implements Interfaces.Plugins.ShowPluginDialog
        If Not m_Locked Then m_dlg.ShowDialog()
    End Sub

    Friend Sub UnloadAll()
        Try
            For i As Integer = m_LoadedPlugins.Count To 1 Step -1
                Dim Plugin As Interfaces.PluginInfo = CType(PluginsList(MapWinUtility.PluginManagementTools.GenerateKey(m_LoadedPlugins(i).GetType())), Interfaces.PluginInfo)
                StopPlugin(Plugin.Key)
            Next
            m_LoadedPlugins = New Collection
            GC.Collect()
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Friend Sub UnloadApplicationPlugins()
        Dim plugin As Interfaces.IPlugin

        Try
            Dim item As DictionaryEntry
            For Each item In m_ApplicationPlugins
                plugin = CType(item.Value, Interfaces.IPlugin)
                plugin.Terminate()
                plugin = Nothing
            Next

            m_ApplicationPlugins.Clear()
            m_ApplicationPlugins = New Hashtable
        Catch ex As Exception
            g_error = ex.Message
            ShowError(ex)
        End Try
    End Sub

    Friend Function ItemClicked(ByVal ItemName As String) As Boolean
        'Called by the MapWindow GUI when a user clicks a toolbar button that was added by a plugin.
        Dim tPlugin As Interfaces.IPlugin
        Dim handled As Boolean = False

        Dim item As DictionaryEntry
        For Each item In m_ApplicationPlugins
            Try
                tPlugin = CType(item.Value, Interfaces.IPlugin)
                tPlugin.ItemClicked(ItemName, handled)
                If handled Then Return True
            Catch ex As Exception
                g_error = ex.Message
                ShowError(ex)
            End Try
        Next

        For Each tPlugin In m_LoadedPlugins
            Try
                ' tPlugin = CType(Item.Value, Interfaces.IPlugin)
                tPlugin.ItemClicked(ItemName, handled)
                'if one of the plugins returns "handled=true" then we stop sending the click to other plugins. 
                If handled Then Return True
            Catch ex As Exception
                g_error = ex.Message
                ShowError(ex)
            End Try
        Next
    End Function

    Friend Function Message(ByVal msg As String) As Boolean
        Dim tPlugin As Interfaces.IPlugin
        Dim handled As Boolean = False

        For Each tPlugin In m_LoadedPlugins
            Try
                tPlugin.Message(msg, handled)
                If handled Then Return True
            Catch ex As Exception
                ShowError(ex)
            End Try
        Next
    End Function

    Friend Sub ProjectLoading(ByVal Key As String, ByVal ProjectFile As String, ByVal SettingsString As String)
        Dim tPlugin As Interfaces.IPlugin = Nothing

        Try
            If m_ApplicationPlugins.Contains(Key) = False Then Exit Try
            tPlugin = CType(m_ApplicationPlugins(Key), Interfaces.IPlugin)
            tPlugin.ProjectLoading(ProjectFile, SettingsString)
            tPlugin = Nothing
        Catch ex As Exception
            g_error = ex.Message
            If Not tPlugin Is Nothing Then
                Dim new_ex As New System.Exception("An error occured in the plugin named: " & tPlugin.Name, ex)
                ShowError(new_ex)
            Else
                ShowError(ex)
            End If
        End Try

        Try
            If ContainsKey(m_LoadedPlugins, Key) = False Then Exit Sub
            tPlugin = CType(m_LoadedPlugins(Key), Interfaces.IPlugin)
            tPlugin.ProjectLoading(ProjectFile, SettingsString)
            tPlugin = Nothing
        Catch ex As Exception
            g_error = ex.Message
            If Not tPlugin Is Nothing Then
                Dim new_ex As New System.Exception("An error occured in the plugin named: " & tPlugin.Name, ex)
                ShowError(new_ex)
            Else
                ShowError(ex)
            End If
        End Try
    End Sub

    Friend Sub ProjectSaving(ByVal Key As String, ByVal ProjectFile As String, ByVal SettingsString As String)
        Dim tPlugin As Interfaces.IPlugin

        Try
            If m_ApplicationPlugins.Contains(Key) = False Then Exit Try
            tPlugin = CType(m_ApplicationPlugins(Key), Interfaces.IPlugin)
            tPlugin.ProjectSaving(ProjectFile, SettingsString)
            tPlugin = Nothing
        Catch ex As Exception
            g_error = ex.Message
            ShowError(ex)
        End Try

        Try
            If ContainsKey(m_LoadedPlugins, Key) = False Then Exit Sub
            tPlugin = CType(m_LoadedPlugins(Key), Interfaces.IPlugin)
            tPlugin.ProjectSaving(ProjectFile, SettingsString)
            tPlugin = Nothing
        Catch ex As Exception
            g_error = ex.Message
            ShowError(ex)
        End Try
    End Sub

    <CLSCompliant(False)> _
    Public Function LoadFromObject(ByVal Plugin As Interfaces.IPlugin, ByVal PluginKey As String) As Boolean Implements Interfaces.Plugins.LoadFromObject
        Try
            If Plugin Is Nothing Then
                Return False
            Else
                If ContainsKey(m_LoadedPlugins, PluginKey) Then
                    Return False
                Else
                    'TODO: does anyone need IMapWin?
                    Plugin.Initialize(Nothing, 0)
                    m_LoadedPlugins.Add(Plugin, PluginKey)
                    Return True
                End If
            End If
        Catch ex As Exception
            ShowError(ex)
            Return False
        End Try
    End Function

    <CLSCompliant(False)> _
    Public Function LoadFromObject(ByVal Plugin As Interfaces.IPlugin, ByVal PluginKey As String, ByVal SettingsString As String) As Boolean Implements Interfaces.Plugins.LoadFromObject
        Try
            If Plugin Is Nothing Then
                Return False
            Else
                If ContainsKey(m_LoadedPlugins, PluginKey) Then
                    Return False
                Else
                    'TODO: do any need IMapWin?
                    Plugin.Initialize(Nothing, 0)
                    m_LoadedPlugins.Add(Plugin, PluginKey)
                    Return True
                End If
            End If
        Catch ex As Exception
            ShowError(ex)
            Return False
        End Try
    End Function

    Friend Function ContainsKey(ByVal c As Collection, ByVal key As Object) As Boolean
        Dim o As Object = Nothing
        Try
            o = c(key)
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Sub BroadcastMessage(ByVal Message As String) Implements Interfaces.Plugins.BroadcastMessage
        'CDM 4/24/2006 - This is a simplified version of Message, it just doesn't have a
        '"handled" return value consideration. Call the one function, to reduce duplication of code.
        Me.Message(Message)
    End Sub

    Public Function GetPluginKey(ByVal PluginName As String) As String Implements Interfaces.Plugins.GetPluginKey
        'Returns the plugin key associated with a plugin name.  The plugin name is the name that is displayed in the 
        'Plugins menu.
        'dpa 1/25/2005
        Dim info As PluginInfo
        Dim obj As DictionaryEntry

        For Each obj In m_PluginList
            info = CType(obj.Value, PluginInfo)
            If info.Name = PluginName Then
                Return info.Key
            End If
        Next
        Return ""
    End Function

End Class