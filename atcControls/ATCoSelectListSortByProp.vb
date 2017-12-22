
''' <summary>
''' This control is a dual-paned selection tool with a list box in each pane.
''' The items in each list box are sorted by ascending alphanumeric order of a specified property.
''' </summary>
''' <remarks></remarks>
Public Class ATCoSelectListSortByProp
    Inherits System.Windows.Forms.UserControl
    'Copyright 2000-2013 by AQUA TERRA Consultants
    '##MODULE_AUTHOR Mark Gray and Robert Dusenbury of AQUA TERRA CONSULTANTS
    '##MODULE_SUMMARY <P>
    '##MODULE_REMARKS This control was made from a combination of standard VB tools.&nbsp;The _
    'left pane is titled <EM>Available</EM> and&nbsp;contains a list _
    'of items that may be, but are not as of yet, selected. The right pane is titled _
    '<EM>Selected</EM> and&nbsp;contains a list of such items.</P> _
    '<P>There are 4 tools located between the left and right panes that allow the _
    'user to transfer items between list boxes: a single right arrow labled _
    '<EM>Add</EM> and a single left arrow labled&nbsp;<EM>Remove</EM>&nbsp;that _
    'transfer only selected items to or from the <EM>Selected</EM> list box, and _
    'double right arrows labled <EM>Add All</EM>&nbsp;and double left _
    'arrows&nbsp;labled <EM>Remove All</EM>&nbsp;that tranfer all items to or from _
    'the <EM>Selected</EM> list box.</P> _
    '<P>Additionally, there is an up and a down arrow on the right side of the _
    'control that allows the user to move <EM>Selected</EM> items&nbsp;up or down in _
    'the list rank.&nbsp;</P>

    Public Event Change(ByVal Sender As System.Object)
    Public Event LeftListSelectedIndexChange(ByVal Sender As System.Object, ByVal e As System.EventArgs)

    Public Property DisplayMember As String
        Get
            Return lstLeft.DisplayMember
        End Get
        Set(ByVal value As String)
            lstLeft.DisplayMember = value
            lstRight.DisplayMember = value
        End Set
    End Property

    Private pSortMember As String = Nothing
    Public Property SortMember As String
        Get
            Return pSortMember
        End Get
        Set(ByVal value As String)
            pSortMember = value
        End Set
    End Property
    Public Property ButtonVisible(ByVal aButtonName As String) As Boolean
        Get
            Select Case aButtonName.ToLower
                Case "1", "add" : ButtonVisible = cmdMoveRight.Visible
                Case "2", "remove" : ButtonVisible = cmdMoveLeft.Visible
                Case "3", "add all" : ButtonVisible = cmdMoveAllRight.Visible
                Case "4", "remove all" : ButtonVisible = cmdMoveAllLeft.Visible
                Case "5", "move up" : ButtonVisible = cmdMoveUp.Visible
                Case "6", "move down" : ButtonVisible = cmdMoveDown.Visible
            End Select
        End Get
        Set(ByVal Value As Boolean)
            '##SUMMARY Boolean determining whether specified button is visible or not.
            '##PARAM butt (I) Index or Name identifying one of 6 buttons on control.
            '##PARAM NewValue (I) Boolean setting assigned to Visible property of button.
            Select Case aButtonName.ToLower
                Case "1", "add", "move right" : cmdMoveRight.Visible = Value
                Case "2", "remove", "move left" : cmdMoveLeft.Visible = Value
                Case "3", "add all", "move all right" : cmdMoveAllRight.Visible = Value
                Case "4", "remove all", "move all left" : cmdMoveAllLeft.Visible = Value
                Case "5", "move up" : cmdMoveUp.Visible = Value
                Case "6", "move down" : cmdMoveDown.Visible = Value
            End Select
        End Set
    End Property

    Public Property RightItem(ByVal i As Integer) As Object
        Get
            If i >= 0 And i < lstRight.Items.Count Then
                Return lstRight.Items(i)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Object)
            '##SUMMARY Name of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in right <EM>Selected</EM> ListBox.
            '##PARAM NewValue (I) Name assigned to <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            If Not InRightList(Value) Then
                If i = lstRight.Items.Count Then lstRight.Items.Add(Value)
                If i >= 0 And i < lstRight.Items.Count Then lstRight.Items(i) = Value
            End If
        End Set
    End Property

    Public Function RightItems() As Generic.List(Of String)
        Dim lItems As New Generic.List(Of String)
        For Each lItem As Object In lstRight.Items
            lItems.Add(lItem.ToString())
        Next
        Return lItems
    End Function

    Public Property LeftItem(ByVal i As Integer) As Object
        Get
            If i >= 0 And i < lstLeft.Items.Count Then
                Return lstLeft.Items(i)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Object)
            '##SUMMARY Name of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in left <EM>Available</EM> ListBox.
            '##PARAM NewValue (I) Name assigned to <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            If Not InLeftList(Value) Then
                If i = lstLeft.Items.Count Then lstLeft.Items.Add(Value)
                If i >= 0 And i < lstLeft.Items.Count Then lstLeft.Items(i) = Value
            End If
        End Set
    End Property

    'Public Property RightItemData(ByVal i As Integer) As Integer
    '    Get
    '        RightItemData = VB6.GetItemData(lstRight, i)
    '    End Get
    '    Set(ByVal Value As Integer)
    '##SUMMARY Integer set as property of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
    '##PARAM i (I) Index identifying sequential member in right <EM>Selected</EM> ListBox.
    '##PARAM NewValue (I) Integer assigned to ItemData property of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
    '        VB6.SetItemData(lstRight, i, Value)
    '    End Set
    'End Property

    'Public Property LeftItemData(ByVal i As Integer) As Integer
    '    Get
    '        LeftItemData = VB6.GetItemData(lstLeft, i)
    '    End Get
    '    Set(ByVal Value As Integer)
    '##SUMMARY Integer set as property of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
    '##PARAM i (I) Index identifying sequential member in left <EM>Available</EM> ListBox.
    '##PARAM NewValue (I) Integer assigned to ItemData property of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
    '        VB6.SetItemData(lstLeft, i, Value)
    '    End Set
    'End Property

    Public ReadOnly Property RightCount() As Integer
        Get
            '##SUMMARY Number of items in right <EM>Selected</EM> ListBox.
            Return lstRight.Items.Count
        End Get
    End Property

    Public ReadOnly Property LeftCount() As Integer
        Get
            '##SUMMARY Number of items in left <EM>Available</EM> ListBox.
            Return lstLeft.Items.Count
        End Get
    End Property

    Public Property RightLabel() As String
        Get
            Return lblRight.Text
        End Get
        Set(ByVal Value As String)
            '##SUMMARY Name of right ListBox; <EM>Selected</EM> is default.
            '##PARAM NewValue (I) Name assigned to Caption property of right ListBox.
            lblRight.Text = Value
        End Set
    End Property

    Public Property LeftLabel() As String
        Get
            Return lblLeft.Text
        End Get
        Set(ByVal Value As String)
            '##SUMMARY Name of left ListBox; <EM>Available</EM> is default.
            '##PARAM NewValue (I) Name assigned to Caption property of left ListBox.
            lblLeft.Text = Value
        End Set
    End Property

    Public Property MoveUpTip() As String
        Get
            MoveUpTip = ToolTip1.GetToolTip(cmdMoveUp)
        End Get
        Set(ByVal Value As String)
            'Attribute MoveUpTip.VB_Description = "Pop-up advice when cursor held over 'up arrow' on right side of control; "Move Item Up In List" is default."
            'UPGRADE_ISSUE: The preceding line couldn't be parsed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="82EBB1AE-1FCB-4FEF-9E6C-8736A316F8A7"'
            '##SUMMARY Pop-up advice when cursor held over 'up arrow' on right side of _
            'control; "Move Item Up In List" is default.
            '##PARAM NewValue (I) Text assigned to ToolTipText property of 'up arrow' button.
            ToolTip1.SetToolTip(cmdMoveUp, Value)
        End Set
    End Property

    Public Property MoveDownTip() As String
        Get
            MoveDownTip = ToolTip1.GetToolTip(cmdMoveDown)
        End Get
        Set(ByVal Value As String)
            'Attribute MoveDownTip.VB_Description = "Pop-up advice when cursor held over 'down arrow' on right side of control; "Move Item Down In List" is default."
            'UPGRADE_ISSUE: The preceding line couldn't be parsed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="82EBB1AE-1FCB-4FEF-9E6C-8736A316F8A7"'
            '##SUMMARY Pop-up advice when cursor held over 'down arrow' on right side of _
            'control; "Move Item Down In List" is default.
            '##PARAM NewValue (I) Text assigned to ToolTipText property of 'down arrow' button.
            ToolTip1.SetToolTip(cmdMoveDown, Value)
        End Set
    End Property

    Public Shadows Property Enabled() As Boolean
        Get
            Enabled = lstRight.Enabled
        End Get
        Set(ByVal Value As Boolean)
            '##SUMMARY Boolean determining whether control is enabled or not.
            '##PARAM NewValue (I) Boolean used to set Enabled property of all _
            'components within main object.
            lstRight.Enabled = Value
            lstLeft.Enabled = Value
            cmdMoveRight.Enabled = Value
            cmdMoveLeft.Enabled = Value
            cmdMoveAllRight.Enabled = Value
            cmdMoveAllLeft.Enabled = Value
            cmdMoveUp.Enabled = Value
            cmdMoveDown.Enabled = Value
        End Set
    End Property

    ''' <summary>
    ''' Alternative to setting LeftItem(index)=NewValue. Adds to end of list without checking for duplicates.
    ''' </summary>
    ''' <param name="NewValue">New value to add to left list</param>
    ''' <remarks>Faster than setting LeftItem(index)=NewValue</remarks>
    Public Sub LeftItemFastAdd(ByVal NewValue As Object)
        lstLeft.Items.Add(NewValue)
    End Sub

    ''' <summary>
    ''' Move an item from the left list to the right list
    ''' </summary>
    ''' <param name="aLeftIndex">zero-based index of item in left list</param>
    ''' <remarks>Inserts into right list in sorted order if SortMember was set, adds to end of right list if no SortMember</remarks>
    Public Sub MoveRight(ByVal aLeftIndex As Integer)
        MoveItem(aLeftIndex, lstLeft, lstRight)
    End Sub

    Public Sub MoveLeft(ByVal aRightIndex As Integer)
        MoveItem(aRightIndex, lstRight, lstLeft)
    End Sub

    Public Sub MoveAllRight()
        '##SUMMARY Moves all items from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        While lstLeft.Items.Count > 0
            MoveRight(0)
        End While
    End Sub

    Public Sub MoveAllLeft()
        '##SUMMARY Moves all items from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        While lstRight.Items.Count > 0
            MoveLeft(0)
        End While
    End Sub

    Private Sub MoveItem(ByVal aFromIndex As Integer, ByVal aFromList As System.Windows.Forms.ListBox, ByVal aToList As System.Windows.Forms.ListBox)
        If aFromIndex >= 0 AndAlso aFromIndex < aFromList.Items.Count Then
            ' find proper place in list for new item
            Dim lInsertHere As Integer = -1
            If Not String.IsNullOrEmpty(pSortMember) Then
                Try
                    Dim lSortingValue As Object = atcUtility.GetSomething(aFromList.Items(aFromIndex), pSortMember)
                    Dim lRightLstCount As Integer = aToList.Items.Count

                    For lCheckIndex As Integer = 0 To lRightLstCount - 1
                        If lSortingValue < atcUtility.GetSomething(aToList.Items(lCheckIndex), pSortMember) Then
                            lInsertHere = lCheckIndex
                            Exit For
                        End If
                    Next
                Catch
                End Try
            End If
            If lInsertHere >= 0 Then
                aToList.Items.Insert(lInsertHere, aFromList.Items(aFromIndex))
            Else
                aToList.Items.Add(aFromList.Items(aFromIndex))
            End If
            aFromList.Items.RemoveAt(aFromIndex)
        End If
        RaiseEvent Change(Me)
    End Sub

    Public Sub ClearRight()
        '##SUMMARY Removes all items from right <EM>Selected</EM> ListBox.
        lstRight.Items.Clear()
        RaiseEvent Change(Me)
    End Sub

    Public Sub ClearLeft()
        '##SUMMARY Removes all items from left <EM>Available</EM> ListBox.
        lstLeft.Items.Clear()
        RaiseEvent Change(Me)
    End Sub

    Public Function InRightList(ByVal aSearch As Object) As Boolean
        '##SUMMARY Boolean check whether specified item is in right <EM>Selected</EM> ListBox.
        '##PARAM search (I) Name of item to search for in right <EM>Selected</EM> ListBox.
        '##RETURNS True if incoming argument appears as item in right ListBox.
        Return lstRight.Items.Contains(aSearch)
    End Function

    Public Function InLeftList(ByVal aSearch As Object) As Boolean
        '##SUMMARY Boolean check whether specified item is in left <EM>Available</EM> ListBox.
        '##PARAM aSearch (I) Item to search for in left <EM>Available</EM> ListBox.
        '##RETURNS True if incoming argument appears as item in left ListBox.
        Return lstLeft.Items.Contains(aSearch)
    End Function

    Private Sub cmdMoveUp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveUp.Click
        Dim lMoved As Boolean = False
        Dim tmp As Object
        For lItemIndex As Integer = 1 To lstRight.Items.Count - 1
            If lstRight.GetSelected(lItemIndex) AndAlso Not lstRight.GetSelected(lItemIndex - 1) Then
                lMoved = True
                tmp = lstRight.Items(lItemIndex)
                lstRight.Items.RemoveAt(lItemIndex)
                lstRight.Items.Insert(lItemIndex - 1, tmp)

                lstRight.SetSelected(lItemIndex, False)
                lstRight.SetSelected(lItemIndex - 1, True)
            End If
        Next
        If lMoved Then RaiseEvent Change(Me)
    End Sub

    Private Sub cmdMoveDown_Click() Handles cmdMoveDown.Click
        Dim lMoved As Boolean = False
        Dim tmp As String
        For lItemIndex As Integer = lstRight.Items.Count - 2 To 0 Step -1
            If lstRight.GetSelected(lItemIndex) AndAlso Not lstRight.GetSelected(lItemIndex + 1) Then
                lMoved = True
                tmp = lstRight.Items(lItemIndex)
                lstRight.Items.RemoveAt(lItemIndex)
                lstRight.Items.Insert(lItemIndex + 1, tmp)

                lstRight.SetSelected(lItemIndex, False)
                lstRight.SetSelected(lItemIndex + 1, True)
            End If
        Next
        If lMoved Then RaiseEvent Change(Me)
    End Sub

    Private Sub cmdMoveAllLeft_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveAllLeft.Click
        '##SUMMARY Moves all items from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        MoveAllLeft()
    End Sub

    Private Sub cmdMoveAllRight_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveAllRight.Click
        '##SUMMARY Moves all items from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        MoveAllRight()
    End Sub

    Private Sub cmdMoveLeft_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveLeft.Click
        '##SUMMARY Moves selected item from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        'Index used to loop thru items in right <EM>Selected</EM> ListBox.
        Dim i As Integer = 0
        While i < lstRight.Items.Count
            If lstRight.GetSelected(i) Then MoveLeft(i) Else i += 1
        End While
    End Sub

    Private Sub cmdMoveRight_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveRight.Click
        '##SUMMARY Moves selected item from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        'Index used to loop thru items in left <EM>Available</EM> ListBox.
        Dim i As Integer = 0
        While i < lstLeft.Items.Count
            If lstLeft.GetSelected(i) Then MoveRight(i) Else i += 1
        End While
    End Sub

    Private Sub lstRight_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstRight.DoubleClick
        '##SUMMARY Moves double-clicked item from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        MoveLeft(lstRight.SelectedIndex)
    End Sub

    Private Sub lstLeft_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstLeft.DoubleClick
        '##SUMMARY Moves double-clicked item from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        MoveRight(lstLeft.SelectedIndex)
    End Sub

    Private Sub lstLeft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstLeft.SelectedIndexChanged
        RaiseEvent LeftListSelectedIndexChange(sender, e)
    End Sub

    Private Sub ATCoSelectListSortByProp_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width > 400 Then
            Dim lMargin As Integer = 6
            Dim lstWidth As Integer = (Width - 140) / 2
            lstLeft.Width = lstWidth
            lstRight.Width = lstWidth
            Dim lButtonLeft As Integer = lstWidth + lMargin
            cmdMoveRight.Left = lButtonLeft
            cmdMoveLeft.Left = lButtonLeft
            cmdMoveAllRight.Left = lButtonLeft
            cmdMoveAllLeft.Left = lButtonLeft
            lstRight.Left = lButtonLeft + cmdMoveRight.Width + lMargin
            lblRight.Left = lstRight.Left
            lblRight.Width = lstRight.Width
            lblLeft.Width = lstLeft.Width
        End If
    End Sub
End Class