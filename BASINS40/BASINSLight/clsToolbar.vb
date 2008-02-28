'********************************************************************************************************
'File Name: clsToolbar.vb
'Description: Public class on the plugin interface that exposes toolbar button and combo box functions.
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
'1/31/2005 - Total overhaul to remove DotNetBar component. (dpa)
'********************************************************************************************************


Public Class Toolbar
    Implements Interfaces.Toolbar

    Friend tbars As Hashtable
    Friend m_CustomToolbars As New Hashtable
    Friend m_Buttons As New Hashtable

    Public Function NumToolbarButtons(ByVal ToolbarName As String) As Integer Implements MapWindow.Interfaces.Toolbar.NumToolbarButtons
        Try
            Return CType(m_CustomToolbars(ToolbarName), ToolStrip).Items.Count
        Catch
            Return 0
        End Try
    End Function

    Public Function PressToolbarButton(ByVal Name As String) As Boolean Implements MapWindow.Interfaces.Toolbar.PressToolbarButton
        Try
            For Each c As ToolStripItem In frmMain.tlbMain.Items
                If c.Name.ToLower() = Name.Trim().ToLower() And c.Enabled Then
                    Dim e As New System.Windows.Forms.ToolStripItemClickedEventArgs(c)
                    frmMain.tlbMain_ButtonClick(c, e)
                    Return True
                End If
            Next
        Catch
        End Try

        Return False
    End Function

    Friend Function Contains(ByVal key As Object) As Boolean
        If (tbars.Contains(key) Or m_Buttons.Contains(key)) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub New()
        tbars = New Hashtable(107)
        m_Buttons = New Hashtable(107)
    End Sub

    Protected Overrides Sub Finalize()
        m_Buttons = Nothing
        tbars = Nothing
        MyBase.Finalize()
    End Sub

    Public Function AddToolbar(ByVal [Name] As String) As Boolean Implements Interfaces.Toolbar.AddToolbar
        'Adds a new custom toolbar.  
        'Updated 1/30/2005 (dpa)
        Dim f As New ToolStrip
        Try
            If m_CustomToolbars.ContainsKey([Name]) = True Then
                g_error = "Invalid Item Name: Cannot have items with duplicate names"
                Return False
            Else
                f.Name = [Name]
                f.ImageList = frmMain.ilsToolbar
                AddHandler f.ItemClicked, AddressOf frmMain.tlbMain_ButtonClick

                'Chris Michaelis, June 13, 2006:
                'AddHandler f.EndDrag, AddressOf frmMain.UndockableToolstrip_EndDrag

                'frmMain.StripDocker.TopToolStripPanel.Join(f, frmMain.StripDocker.TopToolStripPanel.Rows.Length - 1)
                frmMain.StripDocker.TopToolStripPanel.Join(f, New Point(frmMain.tlbMain.Location.X + frmMain.tlbMain.Width, frmMain.tlbMain.Location.Y))

                m_CustomToolbars.Add([Name], f)
                Return True
            End If
        Catch ex As System.Exception
            g_error = ex.Message
            ShowError(ex)
            Return False
        End Try
    End Function

    Public Function RemoveToolbar(ByVal [Name] As String) As Boolean Implements Interfaces.Toolbar.RemoveToolbar
        'Removes any floating toolbars
        'updated 1/31/2005 (dpa)
        Dim f As ToolStrip
        Try
            f = CType(m_CustomToolbars([Name]), ToolStrip)
            frmMain.StripDocker.TopToolStripPanel.Controls.Remove(f)
            m_CustomToolbars.Remove([Name])
        Catch ex As Exception
            g_error = ex.Message
            ShowError(ex)
            Return False
        End Try
    End Function

    Public Function RemoveButton(ByVal [Name] As String) As Boolean Implements Interfaces.Toolbar.RemoveButton
        'Removes a toolbar button from the toolbar
        'Updated 1/15/2005 - dpa
        Dim b As Windows.Forms.ToolStripItem
        Dim bName As String
        Dim Found As Boolean = False
        Dim f As ToolStrip
        Try
            'Try to remove the button from the main toolbar
            For Each b In frmMain.tlbMain.Items
                bName = CType(b.Tag, String)
                If bName = [Name] Then
                    If TypeOf b Is ToolStripDropDownButton Then
                        RemoveHandler CType(b, ToolStripDropDownButton).DropDownItemClicked, AddressOf modMain.frmMain.tlbMain_ButtonClick
                    End If
                    frmMain.tlbMain.Items.Remove(b)
                    m_Buttons.Remove([Name])
                    Found = True
                    Exit For
                End If

                If TypeOf b Is ToolStripDropDownButton Then
                    For i As Integer = 0 To CType(b, ToolStripDropDownButton).DropDownItems.Count - 1
                        If CType(b, ToolStripDropDownButton).DropDownItems(i).Name = [Name] Then
                            CType(b, ToolStripDropDownButton).DropDownItems.RemoveAt(i)
                            m_Buttons.Remove([Name])
                            Found = True
                            Exit For
                        End If
                    Next
                End If
            Next

            'Now try to remove it from any floating toolbars
            'Search through all existing toolbars
            Dim en As IDictionaryEnumerator = m_CustomToolbars.GetEnumerator()
            While en.MoveNext()
                f = CType(m_CustomToolbars(en.Key()), ToolStrip)
                For Each b In f.Items
                    bName = CType(b.Tag, String)
                    If bName = [Name] Then
                        f.Items.Remove(b)
                        m_Buttons.Remove([Name])
                        Found = True
                        Exit For
                    End If

                    If TypeOf b Is ToolStripDropDownButton Then
                        For i As Integer = 0 To CType(b, ToolStripDropDownButton).DropDownItems.Count - 1
                            If CType(b, ToolStripDropDownButton).DropDownItems(i).Name = [Name] Then
                                CType(b, ToolStripDropDownButton).DropDownItems.RemoveAt(i)
                                m_Buttons.Remove([Name])
                                Found = True
                                Exit For
                            End If
                        Next
                    End If
                Next
            End While

            If Found = True Then
                Return True
            Else
                g_error = "Could not find button to remove"
                Return False
            End If
        Catch ex As Exception
            g_error = ex.Message
            ShowError(ex)
            Return False
        End Try
    End Function

    <CLSCompliant(False)> _
    Public Function AddComboBox(ByVal [Name] As String, ByVal Toolbar As String, ByVal After As String) As MapWindow.Interfaces.ComboBoxItem Implements MapWindow.Interfaces.Toolbar.AddComboBox
        ''Adds a combobox to the main toolbar or to a floating toolbar.
        ''Updated 1/30/2005 (dpa)
        Dim f As ToolStrip
        Dim cb As New Windows.Forms.ToolStripComboBox

        Try
            If [Name] = "" Then
                g_error = "Name of ComboBox is required in order to add to toolbar"
                Return Nothing
            End If

            If frmMain.m_ComboBoxes.ContainsKey([Name]) = True Then
                g_error = "Invalid Item Name: Cannot have items with duplicate names"
                Return New ComboBoxItem((CType(frmMain.m_ComboBoxes([Name]), ToolStripItem)))
            End If

            'If no Toolbar name is specified, then add it to the main toolbar.
            If Toolbar = "" Then
                cb.Width = 100
                cb.Height = 16
                cb.Name = [Name]
                frmMain.tlbMain.Items.Add(cb)
                AddHandler cb.SelectedIndexChanged, AddressOf frmMain.CustomCombo_SelectedIndexChanged
                cb.Visible = True
                frmMain.m_ComboBoxes.Add([Name], cb)
                Return New ComboBoxItem(cb)
            Else
                'Check if the toolbar they specified already exitst
                f = CType(m_CustomToolbars(Toolbar), ToolStrip)
                If Not f Is Nothing Then
                    cb.Width = 100
                    cb.Height = 16
                    f.Items.Add(cb)
                    AddHandler cb.SelectedIndexChanged, AddressOf frmMain.CustomCombo_SelectedIndexChanged
                    cb.Visible = True
                    Return New ComboBoxItem(cb)
                Else
                    g_error = "No toolbar with the specified name exists."
                    Return Nothing
                End If
            End If
        Catch ex As System.Exception
            g_error = ex.Message
            ShowError(ex)
            Return Nothing
        End Try
    End Function

#Region "Add Button Functions"

    <CLSCompliant(False)> _
    Public Function AddButton(ByVal [Name] As String) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.AddButton
        'AddButton Type 1
        'This is the simplest button adding call. It just addes a blank button with a text label to the main toolbar.  
        'Reccomend not using this call because it adds a text label which vertically grows the whole toolbar.
        'Note that we store the name of the button in the tag property. 
        'Updated 1/29/2005 (dpa)
        Dim NewButton As Windows.Forms.ToolStripButton

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function."
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try
            If Not [Name] = "-" Then
                NewButton = New Windows.Forms.ToolStripButton
                NewButton.Tag = [Name]
                NewButton.Text = [Name]
                NewButton.Name = [Name]

                frmMain.tlbMain.Items.Add(NewButton)
                m_Buttons.Add([Name], NewButton)
                Return New ToolbarButton(NewButton)
            Else
                frmMain.MenuStrip1.Items.Add(New ToolStripSeparator())
            End If
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try

        Return Nothing
    End Function

    <CLSCompliant(False)> _
  Public Function AddButton(ByVal [Name] As String, ByVal IsDropDown As Boolean) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.AddButton
        Dim NewButton As Windows.Forms.ToolStripDropDownButton

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function."
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try
            If Not [Name] = "-" Then
                NewButton = New Windows.Forms.ToolStripDropDownButton
                NewButton.Tag = [Name]
                NewButton.Text = [Name]
                NewButton.Name = [Name]

                frmMain.tlbMain.Items.Add(NewButton)
                m_Buttons.Add([Name], NewButton)
                Return New ToolbarButton(NewButton)
            Else
                frmMain.MenuStrip1.Items.Add(New ToolStripSeparator())
            End If
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try

        Return Nothing
    End Function

    <CLSCompliant(False)> _
Public Function AddButton(ByVal [Name] As String, ByVal Toolbar As String, ByVal IsDropDown As Boolean) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.AddButton
        Dim NewButton As Windows.Forms.ToolStripDropDownButton

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function."
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try
            If Not [Name] = "-" Then
                NewButton = New Windows.Forms.ToolStripDropDownButton
                NewButton.Tag = [Name]
                NewButton.Text = [Name]
                NewButton.Name = [Name]

                'Check if the toolbar they specified already exits
                Dim f As ToolStrip = Nothing
                If m_CustomToolbars.Contains(Toolbar) Then f = CType(m_CustomToolbars(Toolbar), ToolStrip)
                If Not f Is Nothing Then
                    f.Items.Add(NewButton)
                Else
                    frmMain.tlbMain.Items.Add(NewButton)
                End If

                AddHandler NewButton.DropDownItemClicked, AddressOf modMain.frmMain.tlbMain_ButtonClick
                m_Buttons.Add([Name], NewButton)
                Return New ToolbarButton(NewButton)
            Else
                frmMain.MenuStrip1.Items.Add(New ToolStripSeparator())
            End If
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try

        Return Nothing
    End Function

    <CLSCompliant(False)> _
    Public Function AddButton(ByVal [Name] As String, ByVal Picture As Object) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.AddButton
        'AddButton Type 2
        'This is the most common AddButton function.  It adds a button by name and picture to the main toolbar.
        'Just show the picture, but not the text label
        'updated 1/25/05 dpa
        Dim NewButton As Windows.Forms.ToolStripButton
        Dim NewImageIndex As Integer

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function"
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try
            frmMain.ilsToolbar.Images.Add(CType(Picture, System.Drawing.Icon))
            NewImageIndex = frmMain.ilsToolbar.Images.Count - 1
            NewButton = New Windows.Forms.ToolStripButton
            NewButton.Tag = [Name]
            NewButton.ImageIndex = NewImageIndex
            frmMain.tlbMain.Items.Add(NewButton)
            m_Buttons.Add([Name], NewButton)
            Return New ToolbarButton(NewButton)
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try
    End Function

    <CLSCompliant(False)> _
    Public Function AddButton(ByVal [Name] As String, ByVal Picture As Object, ByVal [Text] As String) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.AddButton
        'AddButton Type 3
        'Allows you to specify the name, text and picture.  This is the only version that uses a text label 
        'by default.  
        'Updated 1/29/2005 (dpa)
        Dim NewButton As Windows.Forms.ToolStripButton
        Dim NewImageIndex As Integer

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function"
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try
            frmMain.ilsToolbar.Images.Add(CType(Picture, System.Drawing.Icon))
            NewImageIndex = frmMain.ilsToolbar.Images.Count - 1
            NewButton = New Windows.Forms.ToolStripButton
            'NewButton.Text = [Text] ' lets don't show the text label because it screws up the size of the toolbar. 
            NewButton.Tag = [Name]
            NewButton.ImageIndex = NewImageIndex
            frmMain.tlbMain.Items.Add(NewButton)
            m_Buttons.Add([Name], NewButton)
            Return New ToolbarButton(NewButton)

        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try
    End Function

    Private Function AddButton(ByVal [Name] As String, ByVal Toolbar As String, ByVal ParentButton As String, ByVal After As String) As MapWindow.Interfaces.ToolbarButton Implements MapWindow.Interfaces.Toolbar.AddButton
        'AddButton Type 4 
        'Allows you to specify the toolbar, the parent button and the button that it goes after.
        'Updated 1/29/2005 - dpa
        Dim NewButton As Windows.Forms.ToolStripItem
        Dim f As ToolStrip

        If [Name] = "" Then
            g_error = "Name not specified in AddButton function"
            Return Nothing
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return New ToolbarButton((CType(m_Buttons([Name]), ToolStripItem)))
        End If

        Try

            'If no Toolbar name is specified, then add it to the main toolbar.
            If Toolbar = "" Then
                ' If it's part of a dropdown item, create it as a menu item
                ' so that its buttons will appear correctly.
                If ParentButton Is Nothing OrElse ParentButton.Trim() = "" Then
                    NewButton = New Windows.Forms.ToolStripButton
                    NewButton.Tag = [Name]
                    NewButton.Name = [Name]
                    frmMain.tlbMain.Items.Add(NewButton)
                Else
                    NewButton = New Windows.Forms.ToolStripMenuItem
                    NewButton.Tag = [Name]
                    NewButton.Name = [Name]
                    Dim added As Boolean = False
                    For i As Integer = 0 To frmMain.tlbMain.Items.Count - 1
                        If frmMain.tlbMain.Items(i).Name.ToLower() = ParentButton.ToLower() Then
                            added = True
                            If TypeOf frmMain.tlbMain.Items(i) Is ToolStripDropDownButton Then
                                CType(frmMain.tlbMain.Items(i), ToolStripDropDownButton).DropDownItems.Add(NewButton)
                            End If
                        End If
                    Next

                    If Not added Then frmMain.tlbMain.Items.Add(NewButton)
                End If
                m_Buttons.Add([Name], NewButton)
                Return New ToolbarButton(NewButton)
            Else
                'Check if the toolbar they specified already exits
                f = CType(m_CustomToolbars(Toolbar), ToolStrip)
                If Not f Is Nothing Then
                    NewButton = New Windows.Forms.ToolStripButton
                    NewButton.Tag = [Name]
                    NewButton.Name = [Name]
                    If ParentButton Is Nothing OrElse ParentButton.Trim() = "" Then
                        f.Items.Add(NewButton)
                    Else
                        Dim added As Boolean = False
                        For i As Integer = 0 To f.Items.Count - 1
                            If f.Items(i).Name.ToLower() = ParentButton.ToLower() Then
                                added = True
                                If TypeOf f.Items(i) Is ToolStripDropDownButton Then
                                    CType(f.Items(i), ToolStripDropDownButton).DropDownItems.Add(NewButton)
                                End If
                            End If
                        Next

                        If Not added Then f.Items.Add(NewButton)
                    End If
                    m_Buttons.Add([Name], NewButton)
                    Return New ToolbarButton(NewButton)
                Else
                    g_error = "No toolbar with the specified name exists."
                    Return Nothing
                End If
            End If
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return Nothing
        End Try
    End Function

    Private Sub AddButtonDropDownSeparator(ByVal [Name] As String, ByVal Toolbar As String, ByVal ParentButton As String) Implements MapWindow.Interfaces.Toolbar.AddButtonDropDownSeparator
        Dim NewButton As New Windows.Forms.ToolStripSeparator
        Dim f As ToolStrip

        If [Name] = "" Then
            g_error = "Name not specified in AddButtonDropDownSeparator function"
            Return
        End If

        If m_Buttons.ContainsKey([Name]) = True Then
            g_error = "Invalid Item Name: Cannot have items with duplicate names"
            Return
        End If

        Try

            'If no Toolbar name is specified, then add it to the main toolbar.
            If Toolbar = "" Then
                NewButton.Tag = [Name]
                NewButton.Name = [Name]
                If ParentButton Is Nothing OrElse ParentButton.Trim() = "" Then
                    frmMain.tlbMain.Items.Add(NewButton)
                Else
                    Dim added As Boolean = False
                    For i As Integer = 0 To frmMain.tlbMain.Items.Count - 1
                        If frmMain.tlbMain.Items(i).Name.ToLower() = ParentButton.ToLower() Then
                            added = True
                            If TypeOf frmMain.tlbMain.Items(i) Is ToolStripDropDownButton Then
                                CType(frmMain.tlbMain.Items(i), ToolStripDropDownButton).DropDownItems.Add(NewButton)
                            End If
                        End If
                    Next

                    If Not added Then frmMain.tlbMain.Items.Add(NewButton)
                End If
                m_Buttons.Add([Name], NewButton)
                Return
            Else
                'Check if the toolbar they specified already exits
                f = CType(m_CustomToolbars(Toolbar), ToolStrip)
                If Not f Is Nothing Then
                    NewButton.Tag = [Name]
                    If ParentButton Is Nothing OrElse ParentButton.Trim() = "" Then
                        f.Items.Add(NewButton)
                    Else
                        Dim added As Boolean = False
                        For i As Integer = 0 To f.Items.Count - 1
                            If f.Items(i).Name.ToLower() = ParentButton.ToLower() Then
                                added = True
                                If TypeOf f.Items(i) Is ToolStripDropDownButton Then
                                    CType(f.Items(i), ToolStripDropDownButton).DropDownItems.Add(NewButton)
                                End If
                            End If
                        Next

                        If Not added Then f.Items.Add(NewButton)
                    End If
                    m_Buttons.Add([Name], NewButton)
                    Return
                Else
                    g_error = "No toolbar with the specified name exists."
                    Return
                End If
            End If
        Catch ex As Exception
            g_error = ex.Message()
            ShowError(ex)
            Return
        End Try
    End Sub
#End Region

    <CLSCompliant(False)> _
    Public Function ButtonItem(ByVal [Name] As String) As Interfaces.ToolbarButton Implements Interfaces.Toolbar.ButtonItem
        'Returns the button item given a name.
        'Updated 1/30/2005 (dpa)
        Dim button As Windows.Forms.ToolStripButton = Nothing

        Try
            button = CType(frmMain.m_Toolbar.m_Buttons([Name]), Windows.Forms.ToolStripButton)
        Catch
        End Try

        If button Is Nothing Then
            Return Nothing
        Else
            Return New ToolbarButton(button)
        End If
    End Function

    <CLSCompliant(False)> _
    Public Function ComboBoxItem(ByVal [Name] As String) As Interfaces.ComboBoxItem Implements Interfaces.Toolbar.ComboBoxItem
        'Returns the comboboxitem given its name
        'June 1 2006 Chris M
        Try
            Dim Item As Windows.Forms.ToolStripComboBox = Nothing
            Dim Strip As Windows.Forms.ToolStrip

            For i As Integer = 0 To frmMain.StripDocker.TopToolStripPanel.Controls.Count - 1
                If TypeOf (frmMain.StripDocker.TopToolStripPanel.Controls(i)) Is MenuStrip Then
                    Strip = frmMain.StripDocker.TopToolStripPanel.Controls(i)
                    For j As Integer = 0 To Strip.Items.Count - 1
                        If Strip.Items(j).Name = [Name] Then Return New ComboBoxItem(Item)
                    Next
                End If
            Next

            Return Nothing

        Catch ex As Exception
            ShowError(ex)
            Return Nothing
        End Try
    End Function

    Public Function RemoveComboBox(ByVal [Name] As String) As Boolean Implements MapWindow.Interfaces.Toolbar.RemoveComboBox
        'Removes a combo box from the main toolbar or floating toolbars.
        'June 1 2006 Chris M
        If [Name] = "" Then Return False

        Try
            Dim Strip As Windows.Forms.ToolStrip

            For i As Integer = 0 To frmMain.StripDocker.TopToolStripPanel.Controls.Count - 1
                If TypeOf (frmMain.StripDocker.TopToolStripPanel.Controls(i)) Is ToolStrip Then
                    Strip = frmMain.StripDocker.TopToolStripPanel.Controls(i)
                    For j As Integer = 0 To Strip.Items.Count - 1
                        If Strip.Items(j).Name = [Name] Then
                            Strip.Items.RemoveAt(j)
                            frmMain.m_ComboBoxes.Remove([Name])
                            Return True
                        End If
                    Next
                End If
            Next

            g_error = "Could not find combo box"
            Return False
        Catch ex As Exception
            ShowError(ex)
            Return Nothing
        End Try
    End Function
End Class