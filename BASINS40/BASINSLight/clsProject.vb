'************************************************************
'Class:         Project
'Context:       Public - part of the plug-in interface
'Filename:      clsProject.vb
'Last Update:   1/16/2005, dpa
'Description:
'This class is the public class object that plugins see.  
'It used to keep it's own copy of the XMLProjectFile, but 
'as of the OSS version 4.0, this object now uses the handle to
'the global friend ProjInfo instance of the xmlProjectFile. 
'New error checking added for version 4.0.
'
'Modifications 
'05/15/2007 Tom Shanley (tws) added  setting to control save of shape-level formatting
'************************************************************
Public Class Project
    Implements MapWindow.Interfaces.Project

    Public Sub OpenIntoCurrent(ByVal filename As String) Implements Interfaces.Project.LoadIntoCurrentProject
        'modMain.frmMain.DoOpenIntoCurrent(filename)
    End Sub

    Public Function Load(ByVal FileName As String) As Boolean Implements MapWindow.Interfaces.Project.Load
        'Implements the IMapWin.Interfaces.Project.Load function that 
        'allows a plugin to load a project file. 
        If System.IO.File.Exists(fileName) Then
            ProjInfo.ProjectFileName = fileName
            Return ProjInfo.LoadProject(FileName)
        Else
            Return False
        End If
    End Function

    Public Function Save(ByVal FileName As String) As Boolean Implements MapWindow.Interfaces.Project.Save
        'Implements the IMapWin.Interfaces.Project.Save function that 
        'allows a plugin to save a project file. 
        If System.IO.Path.GetExtension(FileName) <> ".mwprj" Then
            'Add the .mwprj extension if needed
            FileName += ".mwprj"
        End If
        ProjInfo.ProjectFileName = FileName
        Dim retval = ProjInfo.SaveProject()
        frmMain.SetModified(False)
        Return retval
    End Function

    Public Function SaveConfig(ByVal FileName As String) As Boolean Implements MapWindow.Interfaces.Project.SaveConfig
        'Implements the IMapWin.Interfaces.Project.SaveConfig function that 
        'allows a plugin to save a config file. 
        If System.IO.Path.GetExtension(FileName) <> ".mwcfg" Then
            FileName += ".mwcfg"
        End If
        ProjInfo.ConfigFileName = FileName
        Return ProjInfo.SaveConfig()
    End Function

    Public ReadOnly Property ConfigFileName() As String Implements MapWindow.Interfaces.Project.ConfigFileName
        'Implements the IMapWin.Interfaces.Project.ConfigFileName property. 
        'Update in version 4, pulls the config filename from the projinfo object
        'instead of from the frmMain object.
        Get
            Return ProjInfo.ConfigFileName
        End Get
    End Property

    Public ReadOnly Property FileName() As String Implements MapWindow.Interfaces.Project.FileName
        'Implements the IMapWin.Interfaces.Project.FileName property.
        'Update in version 4, pulls the filename from the projinfo object
        'instead of from the frmMain object.
        Get
            Return ProjInfo.ProjectFileName
        End Get
    End Property

    Public Property Modified() As Boolean Implements MapWindow.Interfaces.Project.Modified
        'Implements the IMapWin.Interfaces.Project.Modified property.
        'Update in version 4, pulls the modified status from the projinfo object
        'instead of from the frmMain object.
        Get
            Return ProjInfo.Modified
        End Get
        Set(ByVal Value As Boolean)
            frmMain.SetModified(Value)
        End Set
    End Property

    Public Property ProjectProjection() As String Implements Interfaces.Project.ProjectProjection
        Get
            If modMain.ProjInfo.ProjectProjection Is Nothing Or modMain.ProjInfo.ProjectProjection = "" Then
                Return ""
            End If

            Return modMain.ProjInfo.ProjectProjection.Trim()
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then Value = ""

            modMain.ProjInfo.ProjectProjection = Value
        End Set
    End Property

    Public Property MapUnits() As String Implements Interfaces.Project.MapUnits
        Get
            If modMain.ProjInfo.m_MapUnits Is Nothing Or modMain.ProjInfo.m_MapUnits = "" Then
                modMain.ProjInfo.m_MapUnits = "" 'In case it was 'nothing'

                'Try to detect the map unit from the proj4 string;
                'this will only work for meters however.
                If Not ProjectProjection() = "" Then
                    If Not InStr(ProjectProjection().ToLower, "+units=") = 0 Then
                        Dim components() As String = ProjectProjection().ToLower.Substring(ProjectProjection().ToLower.IndexOf("+units=")).Split(Convert.ToChar(32))
                        Dim unitpart() As String = components(0).Split(Convert.ToChar(61))
                        If (unitpart.Length > 1 And unitpart(0) = "+units") Then
                            If (unitpart(1) = "m") Then
                                modMain.ProjInfo.m_MapUnits = "Meters"
                            End If
                        End If
                    Else
                        'Is it latlong?
                        If Not InStr(ProjectProjection().ToLower, "+longlat=") = 0 Or InStr(ProjectProjection().ToLower, "+latlong=") = 0 Then
                            modMain.ProjInfo.m_MapUnits = "Lat/Long"
                        End If
                    End If
                End If

            End If

            Return modMain.ProjInfo.m_MapUnits.Trim()
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then Value = ""

            modMain.ProjInfo.m_MapUnits = Value.Trim()
        End Set
    End Property

    Public Property MapUnitsAlternate() As String Implements Interfaces.Project.MapUnitsAlternate
        Get
            If modMain.ProjInfo.ShowStatusBarCoords_Alternate Is Nothing Or modMain.ProjInfo.ShowStatusBarCoords_Alternate = "" Then
                Return ""
            Else
                Return modMain.ProjInfo.ShowStatusBarCoords_Alternate.Trim()
            End If
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then Value = ""

            modMain.ProjInfo.ShowStatusBarCoords_Alternate = Value.Trim()
        End Set
    End Property

    Public ReadOnly Property ConfigLoaded() As Boolean Implements Interfaces.Project.ConfigLoaded
        Get
            Return modMain.ProjInfo.ConfigLoaded
        End Get
    End Property

    Public ReadOnly Property RecentProjects() As System.Collections.ArrayList Implements Interfaces.Project.RecentProjects
        Get
            Return modMain.ProjInfo.RecentProjects
        End Get
    End Property

    ' tws 05/15/07
    Public Property SaveShapeSettings() As Boolean Implements Interfaces.Project.SaveShapeSettings
        Get
            Return modMain.ProjInfo.SaveShapeSettings
        End Get
        Set(ByVal value As Boolean)
            modMain.ProjInfo.SaveShapeSettings = value
        End Set
    End Property
End Class
