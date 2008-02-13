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
'1/31/2005 - Total overhaul to remove DotNetBar button (dpa).
'********************************************************************************************************

Public Class ToolbarButton
    Implements MapWindow.Interfaces.ToolbarButton
    Private m_Button As Windows.Forms.ToolStripItem
    Private m_Picture As Object

    '1/31/2005 (dpa)
    Private m_BeginsGroup As Boolean
    Private m_Cursor As Cursor
    Private m_Category As String
    Private m_Description As String
    Private m_Displayed As Boolean
    Private m_Name As String

    Public Sub New(ByVal Button As Windows.Forms.ToolStripButton)
        m_Button = Button
    End Sub

    Public Sub New(ByVal Button As Windows.Forms.ToolStripDropDownButton)
        m_Button = Button
    End Sub

    Public Sub New(ByVal Button As Windows.Forms.ToolStripItem)
        m_Button = Button
    End Sub

    Public Property BeginsGroup() As Boolean Implements MapWindow.Interfaces.ToolbarButton.BeginsGroup
        Get
            Return m_BeginsGroup
        End Get
        Set(ByVal Value As Boolean)
            m_BeginsGroup = Value
        End Set
    End Property

    Public Property Category() As String Implements MapWindow.Interfaces.ToolbarButton.Category
        Get
            Return m_Category
        End Get
        Set(ByVal Value As String)
            m_Category = Value
        End Set
    End Property

    Public Property Cursor() As Cursor Implements MapWindow.Interfaces.ToolbarButton.Cursor
        Get
            Return m_Cursor
        End Get
        Set(ByVal Value As Cursor)
            m_Cursor = Value
        End Set
    End Property

    Public Property Description() As String Implements MapWindow.Interfaces.ToolbarButton.Description
        Get
            Return m_Description
        End Get
        Set(ByVal Value As String)
            m_Description = Value
        End Set
    End Property

    Public Property Displayed() As Boolean Implements MapWindow.Interfaces.ToolbarButton.Displayed
        Get
            Return m_Displayed
        End Get
        Set(ByVal Value As Boolean)
            m_Displayed = Value
        End Set
    End Property

    Public Property Enabled() As Boolean Implements MapWindow.Interfaces.ToolbarButton.Enabled
        Get
            Return m_Button.Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Button.Enabled = Value
        End Set
    End Property

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.ToolbarButton.Name
        Get
            Return CType(m_Button.Tag, String)
        End Get
    End Property

    Public ReadOnly Property NumSubItems() As Integer Implements MapWindow.Interfaces.ToolbarButton.NumSubItems
        Get
            If TypeOf (m_Button) Is ToolStripDropDownButton Then
                Return CType(m_Button, ToolStripDropDownButton).DropDownItems.Count
            End If
        End Get
    End Property

    Public Property Picture() As Object Implements MapWindow.Interfaces.ToolbarButton.Picture
        'Updated 1/29/2005 (dpa)
        Get
            Return m_Picture
        End Get
        Set(ByVal Value As Object)
            m_Picture = Value

            Try
                If TypeOf Value Is System.Drawing.Icon Then
                    Dim img As System.Drawing.Image = CType(Value, Icon).ToBitmap()
                    m_Button.Image = img
                Else
                    m_Button.Image = Value
                End If

            Catch ex As Exception
                Debug.WriteLine(ex.ToString())
            End Try

            'Below no longer appropriate for toolstrip
            'Try
            '    'TODO: check if the image already exists in the image list before adding it.
            '    'Add the picture to the image list.
            '    If TypeOf Value Is Icon Then
            '        frmMain.ilsToolbar.Images.Add(CType(Value, Icon))
            '        m_Button.ImageIndex = frmMain.ilsToolbar.Images.Count - 1
            '    ElseIf TypeOf Value Is System.Drawing.Image Then
            '        frmMain.ilsToolbar.Images.Add(CType(Value, Image))
            '        m_Button.ImageIndex = frmMain.ilsToolbar.Images.Count - 1
            '    End If
            'Catch
            'End Try
        End Set
    End Property

    Public Property Pressed() As Boolean Implements MapWindow.Interfaces.ToolbarButton.Pressed
        Get
            If TypeOf (m_Button) Is ToolStripButton Then
                Return CType(m_Button, ToolStripButton).Checked
            End If
        End Get
        Set(ByVal Value As Boolean)
            If TypeOf (m_Button) Is ToolStripButton Then
                CType(m_Button, ToolStripButton).Checked = Value
            End If
        End Set
    End Property

    <CLSCompliant(False)> _
    Public Function SubItem(ByVal Index As Integer) As MapWindow.Interfaces.ToolbarButton Implements MapWindow.Interfaces.ToolbarButton.SubItem
        If TypeOf (m_Button) Is ToolStripDropDownButton Then
            Dim ddbutton As ToolStripDropDownButton = CType(m_Button, ToolStripDropDownButton)
            If ddbutton.DropDownItems.Count < Index AndAlso Index >= 0 Then
                Return New ToolbarButton(CType(ddbutton.DropDownItems(Index), ToolStripButton))
            Else
                Return Nothing
            End If
        End If
        Return Nothing
    End Function

    <CLSCompliant(False)> _
    Public Function SubItem(ByVal [Name] As String) As MapWindow.Interfaces.ToolbarButton Implements MapWindow.Interfaces.ToolbarButton.SubItem
        If TypeOf (m_Button) Is ToolStripDropDownButton Then
            Dim ddbutton As ToolStripDropDownButton = CType(m_Button, ToolStripDropDownButton)
            For i As Integer = 0 To ddbutton.DropDownItems.Count - 1
                If ddbutton.DropDownItems(i).Name = [Name] Then Return New ToolbarButton(CType(ddbutton.DropDownItems(i), ToolStripButton))
            Next
        End If
        Return Nothing
    End Function

    Public Property Text() As String Implements MapWindow.Interfaces.ToolbarButton.Text
        Get
            Return m_Button.Text
        End Get
        Set(ByVal Value As String)
            m_Button.Text = Value
        End Set
    End Property

    Public Property Tooltip() As String Implements MapWindow.Interfaces.ToolbarButton.Tooltip
        Get
            Return m_Button.ToolTipText
        End Get
        Set(ByVal Value As String)
            m_Button.ToolTipText = Value
        End Set
    End Property

    Public Property Visible() As Boolean Implements MapWindow.Interfaces.ToolbarButton.Visible
        Get
            Return m_Button.Visible
        End Get
        Set(ByVal Value As Boolean)
            m_Button.Visible = Value
        End Set
    End Property
End Class
