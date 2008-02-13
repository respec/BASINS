'********************************************************************************************************
'File Name: clsPluginInfo.vb
'Description: Public class on the plugin interface for managing plugins.    
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'1/31/2005 - No change from the public domain version. 
'6/13/2005 - Removed old commented COM Plugin support.  
'********************************************************************************************************
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports MapWindow

Public Class PluginInfo
    Implements Interfaces.PluginInfo

    '************************************************************************
    ' Member variables
    '************************************************************************
    Private m_CoClassGUID As String
    Private m_CoClassName As String
    Private m_CreateString As String
    Private m_Filename As String
    'Private m_DllType As DllType
    Private m_Name As String
    Private m_Description As String
    Private m_Version As String
    Private m_Author As String
    Private m_BuildDate As String
    Private m_Key As String
    Private m_Initialized As Boolean
    Private IPluginGUID As String

    '************************************************************************
    ' Constructor and Destructor
    '************************************************************************

    Public Sub New()
        MyBase.New()
        m_Initialized = False
        IPluginGUID = UCase("{" & GetType(Interfaces.IPlugin).GUID.ToString() & "}")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    '************************************************************************
    ' Member Functions and Sub-Routines
    '************************************************************************

    Friend Function Init(ByVal Path As String, ByVal SearchingGUID As Guid) As Boolean
        Dim bRes As Boolean
        Try

            Path = Path.Replace("""", "")
            If Dir(Path) = "" Then Return False ' make sure the file exists
            bRes = AddFromFileDotNetAssembly(Path, SearchingGUID)
            m_Initialized = bRes
        Catch ex As System.Exception
            bRes = False
            ShowError(ex)
        End Try
        Return bRes
    End Function

    Private Function AddFromFileDotNetAssembly(ByVal PathToFile As String, ByVal SearchingGUID As Guid) As Boolean
        'try to look at the file as a .NET Assembly
        'creating a 
        Dim CanLoad As Boolean = False 'try to load classes as plugin until one loads
        Dim asm As System.Reflection.Assembly
        Dim CoClassList As Type() 'list of items within the assembly
        Dim CoClass As Type 'information about the current assembly item
        Dim Infc As Type 'current interface
        Dim InfcList As Type() 'list of interfaces implemented by the current assembly

        Try
            asm = System.Reflection.Assembly.LoadFrom(Trim(PathToFile))
            Try
                CoClassList = asm.GetTypes()
            Catch rtlex As ReflectionTypeLoadException
                Return False

            Catch ex As System.Exception
                ShowError(ex)
                Return False
            End Try

            For Each CoClass In CoClassList
                InfcList = CoClass.GetInterfaces()
                For Each Infc In InfcList
                    If System.Guid.op_Equality(Infc.GUID, SearchingGUID) Then
                        If Not CanLoad Then
                            CanLoad = InitAssembly(CoClass, PathToFile)
                            If CanLoad Then Return True
                        End If
                    End If
                Next Infc
            Next CoClass
        Catch ex2 As System.BadImageFormatException
            ' This is not a .net assembly!  don't load it
        Catch ex As System.Exception
            ShowError(ex)
        End Try
        CoClassList = Nothing
        asm = Nothing
        Return CanLoad
    End Function

    Private Function InitAssembly(ByRef AssemblyInfo As System.Type, ByVal PathToFile As String) As Boolean 'initialize a .NET assembly
        'initialize a VS.NET assembly plugin
        Dim Asm As System.Reflection.Assembly

        Try
            'm_DllType = DllType.DotNetAssembly
            m_CoClassGUID = UCase("{" + AssemblyInfo.GUID.ToString() + "}")
            m_CoClassName = AssemblyInfo.Name
            m_Filename = PathToFile
            m_CreateString = MapWinUtility.PluginManagementTools.GetCreateString(AssemblyInfo)
            m_Key = MapWinUtility.PluginManagementTools.GenerateKey(AssemblyInfo)

            Asm = System.Reflection.Assembly.GetAssembly(AssemblyInfo)

            Try
                Dim attribs As Integer = Asm.GetType(m_CreateString).Attributes
                If (CInt(TypeAttributes.Abstract) And attribs) = CInt(TypeAttributes.Abstract) Then Return False
                'Do not load abstract classes.
                'Note bitwise and, this is to account for other modifiers like "public" also.
            Catch
                'Try to load it up anyway.
            End Try

            Try
                Dim o As Object = Asm.CreateInstance(m_CreateString)
                If TypeOf o Is Interfaces.IPlugin Then
                    Dim plugin As Interfaces.IPlugin
                    plugin = CType(o, Interfaces.IPlugin)

                    m_Author = plugin.Author
                    m_BuildDate = plugin.BuildDate
                    m_Description = plugin.Description
                    m_Name = plugin.Name
                    If m_Name Is Nothing OrElse m_Name.Length = 0 Then Return False
                    m_Version = plugin.Version
                    plugin = Nothing
                    Asm = Nothing
                    o = Nothing
                Else
                    Try
                        'Attempt late binding -- we wouldn't
                        'have made it here if we didn't
                        'contain one of the coclasses we're
                        'looking for.
                        m_Author = o.Author
                        m_BuildDate = o.BuildDate
                        m_Description = o.Description
                        m_Name = o.Name
                        If m_Name Is Nothing OrElse m_Name.Length = 0 Then Return False
                        m_Version = o.Version
                        o = Nothing
                    Catch
                        Return False
                    End Try
                End If
            Catch ex As Exception
                MapWinUtility.Logger.Dbg("The plugin '" & PathToFile & "' failed to load" + vbCrLf + vbCrLf + "Details: " + ex.ToString())
                Return False
            End Try

        Catch ex As System.Exception
            MapWinUtility.Logger.Dbg(ex.ToString())
            Return False
        End Try

        Return True
    End Function

    Public ReadOnly Property Key() As String Implements Interfaces.PluginInfo.Key
        Get
            Key = m_Key
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements Interfaces.PluginInfo.Author
        Get
            Return m_Author
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements Interfaces.PluginInfo.BuildDate
        Get
            Return m_BuildDate
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.PluginInfo.Description
        Get
            Return m_Description
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements Interfaces.PluginInfo.Name
        Get
            Return m_Name
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements Interfaces.PluginInfo.Version
        Get
            Return m_Version
        End Get
    End Property

    Public ReadOnly Property GUID() As String Implements Interfaces.PluginInfo.GUID
        Get
            Return m_CoClassGUID
        End Get
    End Property

    Friend ReadOnly Property CoClassName() As String
        Get
            Return m_CoClassName
        End Get
    End Property

    Friend ReadOnly Property CreateString() As String
        Get
            Return m_CreateString
        End Get
    End Property

    Friend ReadOnly Property FileName() As String
        Get
            Return m_Filename
        End Get
    End Property
End Class
