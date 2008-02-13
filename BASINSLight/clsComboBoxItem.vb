'********************************************************************************************************
'File Name: clsComboBoxItem.vb
'Description: Class that defines a combo box toolbar item as exposed to the plugin interface.
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
'1/29/2005 This class has been totally re-written to operate with out the DotNetBar component.
'
'********************************************************************************************************
Public Class ComboBoxItem

    Implements Interfaces.ComboBoxItem

    Private m_Box As Windows.Forms.ToolStripComboBox
    Private m_Description As String
    Private m_Tooltip As String

    Public Sub New(ByVal ComboBox As Windows.Forms.ToolStripComboBox)
        m_Box = ComboBox
    End Sub

    Public Property Cursor() As System.Windows.Forms.Cursor Implements MapWindow.Interfaces.ComboBoxItem.Cursor
        Get
            'Unsupported now
            Return Cursors.Default
        End Get
        Set(ByVal Value As System.Windows.Forms.Cursor)
            'Unsupported
        End Set
    End Property

    Public Property Description() As String Implements MapWindow.Interfaces.ComboBoxItem.Description
        Get
            Return m_Description
        End Get
        Set(ByVal Value As String)
            m_Description = Value
        End Set
    End Property

    Public Property DropDownStyle() As System.Windows.Forms.ComboBoxStyle Implements MapWindow.Interfaces.ComboBoxItem.DropDownStyle
        Get
            Return m_Box.DropDownStyle
        End Get
        Set(ByVal Value As System.Windows.Forms.ComboBoxStyle)
            m_Box.DropDownStyle = Value
        End Set
    End Property

    Public Property Enabled() As Boolean Implements MapWindow.Interfaces.ComboBoxItem.Enabled
        Get
            Return m_Box.Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Box.Enabled = Value
        End Set
    End Property

    Public Function Items() As System.Windows.Forms.ComboBox.ObjectCollection Implements MapWindow.Interfaces.ComboBoxItem.Items
        Return m_Box.Items
    End Function

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.ComboBoxItem.Name
        Get
            Return CType(m_Box.Tag, String)
        End Get
    End Property

    Public Property SelectedIndex() As Integer Implements MapWindow.Interfaces.ComboBoxItem.SelectedIndex
        Get
            Return m_Box.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            m_Box.SelectedIndex = Value
        End Set
    End Property

    Public Property SelectedItem() As Object Implements MapWindow.Interfaces.ComboBoxItem.SelectedItem
        Get
            Return m_Box.SelectedItem()
        End Get
        Set(ByVal Value As Object)
            m_Box.SelectedItem = Value
        End Set
    End Property

    Public Property SelectedText() As String Implements MapWindow.Interfaces.ComboBoxItem.SelectedText
        Get
            Return m_Box.SelectedText
        End Get
        Set(ByVal Value As String)
            m_Box.SelectedText = Value
        End Set
    End Property

    Public Property SelectionLength() As Integer Implements MapWindow.Interfaces.ComboBoxItem.SelectionLength
        Get
            Return m_Box.SelectionLength
        End Get
        Set(ByVal Value As Integer)
            m_Box.SelectionLength = Value
        End Set
    End Property

    Public Property SelectionStart() As Integer Implements MapWindow.Interfaces.ComboBoxItem.SelectionStart
        Get
            Return m_Box.SelectionStart
        End Get
        Set(ByVal Value As Integer)
            m_Box.SelectionStart = Value
        End Set
    End Property

    Public Property Text() As String Implements MapWindow.Interfaces.ComboBoxItem.Text
        Get
            Return m_Box.Text
        End Get
        Set(ByVal Value As String)
            m_Box.Text = Value
        End Set
    End Property

    Public Property Tooltip() As String Implements MapWindow.Interfaces.ComboBoxItem.Tooltip
        Get
            Return m_Tooltip
        End Get
        Set(ByVal Value As String)
            m_Tooltip = Value
        End Set
    End Property

    Public Property Width() As Integer Implements MapWindow.Interfaces.ComboBoxItem.Width
        Get
            Return m_Box.Width
        End Get
        Set(ByVal Value As Integer)
            m_Box.Width = Value
        End Set
    End Property
End Class
