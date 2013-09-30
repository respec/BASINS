Option Strict Off
Option Explicit On


Friend Class ATCoSelectListSortByProp
    Inherits System.Windows.Forms.UserControl
    'Copyright 2000 by AQUA TERRA Consultants

    '##MODULE_NAME AtcoSelectListSorted
    '##MODULE_DATE December 14, 2000
    '##MODULE_AUTHOR Mark Gray and Robert Dusenbury or AQUA TERRA CONSULTANTS
    '##MODULE_SUMMARY <P>This control is a dual-paned selection tool with a list box in each _
    'pane. The items in each list box are sorted by ascending alphanumeric order of a _
    'specified property.
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

    Public Event Change(ByVal Sender As System.Object, ByVal e As System.EventArgs)
    Public Event LeftListSelectedIndexChange(ByVal Sender As System.Object, ByVal e As System.EventArgs)

    Public Property ButtonVisible(ByVal butt As Object) As Boolean
        Get
            Dim label As String 'text identifying one of the 6 buttons on the control
            'UPGRADE_WARNING: Couldn't resolve default property of object butt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            label = LCase(butt)
            Select Case label
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
            Dim label As String 'text identifying one of the 6 buttons on the control
            'UPGRADE_WARNING: Couldn't resolve default property of object butt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            label = LCase(butt)
            Select Case label
                Case "1", "add", "move right" : cmdMoveRight.Visible = Value
                Case "2", "remove", "move left" : cmdMoveLeft.Visible = Value
                Case "3", "add all", "move all right" : cmdMoveAllRight.Visible = Value
                Case "4", "remove all", "move all left" : cmdMoveAllLeft.Visible = Value
                Case "5", "move up" : cmdMoveUp.Visible = Value
                Case "6", "move down" : cmdMoveDown.Visible = Value
            End Select
        End Set
    End Property

    Public Property RightItem(ByVal i As Integer) As String
        Get
            If i >= 0 And i < lstRight.Items.Count Then
                RightItem = lstRight.Items(i).ToString
            Else
                RightItem = ""
            End If
        End Get
        Set(ByVal Value As String)
            '##SUMMARY Name of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in right <EM>Selected</EM> ListBox.
            '##PARAM NewValue (I) Name assigned to <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            If Len(Value) > 0 And Not InRightList(Value) Then
                If i = lstRight.Items.Count Then lstRight.Items.Add(Value)
                If i >= 0 And i < lstRight.Items.Count Then VB6.SetItemString(lstRight, i, Value)
            End If
        End Set
    End Property

    Public Property LeftItem(ByVal i As Integer) As String
        Get
            If i >= 0 And i < lstLeft.Items.Count Then
                LeftItem = lstLeft.Items(i).ToString
            Else
                LeftItem = ""
            End If
        End Get
        Set(ByVal Value As String)
            '##SUMMARY Name of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in left <EM>Available</EM> ListBox.
            '##PARAM NewValue (I) Name assigned to <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            If Not InLeftList(Value) Then
                If i = lstLeft.Items.Count Then lstLeft.Items.Add(Value)
                If i >= 0 And i < lstLeft.Items.Count Then VB6.SetItemString(lstLeft, i, Value)
            End If
        End Set
    End Property

    Public Property RightItemData(ByVal i As Integer) As Integer
        Get
            RightItemData = VB6.GetItemData(lstRight, i)
        End Get
        Set(ByVal Value As Integer)
            '##SUMMARY Integer set as property of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in right <EM>Selected</EM> ListBox.
            '##PARAM NewValue (I) Integer assigned to ItemData property of <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
            VB6.SetItemData(lstRight, i, Value)
        End Set
    End Property

    Public Property LeftItemData(ByVal i As Integer) As Integer
        Get
            LeftItemData = VB6.GetItemData(lstLeft, i)
        End Get
        Set(ByVal Value As Integer)
            '##SUMMARY Integer set as property of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            '##PARAM i (I) Index identifying sequential member in left <EM>Available</EM> ListBox.
            '##PARAM NewValue (I) Integer assigned to ItemData property of <EM>i</EM>th item in left <EM>Available</EM> ListBox.
            VB6.SetItemData(lstLeft, i, Value)
        End Set
    End Property

    Public ReadOnly Property RightCount() As Integer
        Get
            '##SUMMARY Number of items in right <EM>Selected</EM> ListBox.
            RightCount = lstRight.Items.Count
        End Get
    End Property

    Public ReadOnly Property LeftCount() As Integer
        Get
            '##SUMMARY Number of items in left <EM>Available</EM> ListBox.
            LeftCount = lstLeft.Items.Count
        End Get
    End Property

    Public Property RightLabel() As String
        Get
            RightLabel = lblRight.Text
        End Get
        Set(ByVal Value As String)
            '##SUMMARY Name of right ListBox; <EM>Selected</EM> is default.
            '##PARAM NewValue (I) Name assigned to Caption property of right ListBox.
            lblRight.Text = Value
        End Set
    End Property

    Public Property LeftLabel() As String
        Get
            LeftLabel = lblLeft.Text
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

    Public Sub LeftItemFastAdd(ByVal NewValue As String)
        '##SUMMARY Used in place of Let LeftItem to speed addition of hundreds of items. _
        'If you are adding fewer than 100 items, use LeftItem property instead.
        '##PARAM NewValue (I) Name assigned to new item in left <EM>Available</EM> ListBox.
        lstLeft.Items.Add(NewValue)
    End Sub

    Public Sub MoveRight(ByVal i As Integer)
        ' ##SUMMARY Moves selected item from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        ' ##PARAM i (I) Integer identifying <EM>i</EM>th item in left <EM>Available</EM> ListBox.
        ' ##REMARKS After move, reorders right list box in ascending alphanumeric order of _
        'specified property.
        Dim j As Integer 'index used in 1st to loop thru items in right list box
        Dim k As Integer 'index used in 2nd to loop thru items in right list box
        Dim lstCnt As Object 'number of items in right list box
        Dim tmpData() As Integer = Nothing 'array of ItemData properties of items in right list box
        Dim tmp() As String = Nothing 'array of item names in right list box

        If i >= 0 And i < lstLeft.Items.Count Then
            ' find proper place in list for new item
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lstCnt = lstRight.Items.Count
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            While j < lstCnt
                If VB6.GetItemData(lstLeft, i) < VB6.GetItemData(lstRight, j) Then GoTo x
                j = j + 1
            End While
x:
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If j < lstCnt Then
                'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ReDim tmpData(lstCnt - j - 1)
                'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ReDim tmp(lstCnt - j - 1)
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For k = j + 1 To lstCnt
                tmpData(k - j - 1) = VB6.GetItemData(lstRight, j)
                tmp(k - j - 1) = VB6.GetItemString(lstRight, j)
                lstRight.Items.RemoveAt((j))
            Next k
            lstRight.Items.Add(VB6.GetItemString(lstLeft, i))
            VB6.SetItemData(lstRight, lstRight.Items.Count - 1, VB6.GetItemData(lstLeft, i))
            lstLeft.Items.RemoveAt(i)
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For k = j + 1 To lstCnt
                lstRight.Items.Add(tmp(k - j - 1))
                VB6.SetItemData(lstRight, k, tmpData(k - j - 1))
            Next k
        End If
        RaiseEvent Change(Me, Nothing)
    End Sub

    Public Sub MoveLeft(ByVal i As Integer)
        ' ##SUMMARY Moves selected item from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        ' ##PARAM i (I) Integer identifying <EM>i</EM>th item in right <EM>Selected</EM> ListBox.
        ' ##REMARKS After move, reorders left list box in ascending alphanumeric order of _
        'specified property.
        Dim j As Integer 'index used in 1st to loop thru items in left list box
        Dim k As Integer 'index used in 2nd to loop thru items in left list box
        Dim lstCnt As Object 'number of items in left list box
        Dim tmpData() As Integer = Nothing 'array of ItemData properties of items in left list box
        Dim tmp() As String = Nothing 'array of item names in left list box

        If i >= 0 And i < lstRight.Items.Count Then
            ' find proper place in list for new item
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lstCnt = lstLeft.Items.Count
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            While j < lstCnt
                If VB6.GetItemData(lstRight, i) < VB6.GetItemData(lstLeft, j) Then GoTo x
                j = j + 1
            End While
x:
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If j < lstCnt Then
                'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ReDim tmpData(lstCnt - j - 1)
                'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ReDim tmp(lstCnt - j - 1)
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For k = j + 1 To lstCnt
                tmpData(k - j - 1) = VB6.GetItemData(lstLeft, j)
                tmp(k - j - 1) = VB6.GetItemString(lstLeft, j)
                lstLeft.Items.RemoveAt((j))
            Next k
            lstLeft.Items.Add(VB6.GetItemString(lstRight, i))
            VB6.SetItemData(lstLeft, lstLeft.Items.Count - 1, VB6.GetItemData(lstRight, i))
            lstRight.Items.RemoveAt(i)
            'UPGRADE_WARNING: Couldn't resolve default property of object lstCnt. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For k = j + 1 To lstCnt
                lstLeft.Items.Add(tmp(k - j - 1))
                VB6.SetItemData(lstLeft, k, tmpData(k - j - 1))
            Next k
        End If
        RaiseEvent Change(Me, Nothing)
    End Sub

    Public Sub MoveAllRight()
        '##SUMMARY Moves all items from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        Dim i As Integer 'Index used to loop thru items in left ListBox
        For i = 0 To lstLeft.Items.Count - 1
            MoveRight(0)
        Next i
    End Sub

    Public Sub MoveAllLeft()
        '##SUMMARY Moves all items from right <EM>Selected</EM> to left <EM>Available</EM> ListBox.
        Dim i As Integer 'Index used to loop thru items in right ListBox
        For i = 0 To lstRight.Items.Count - 1
            MoveLeft(0)
        Next i
        lstRight.Items.Clear()
    End Sub

    Public Sub ClearRight()
        '##SUMMARY Removes all items from right <EM>Selected</EM> ListBox.
        lstRight.Items.Clear()
        RaiseEvent Change(Me, Nothing)
    End Sub

    Public Sub ClearLeft()
        '##SUMMARY Removes all items from left <EM>Available</EM> ListBox.
        lstLeft.Items.Clear()
        RaiseEvent Change(Me, Nothing)
    End Sub

    Public Function InRightList(ByVal search As String) As Boolean
        '##SUMMARY Boolean check whether specified item is in right <EM>Selected</EM> ListBox.
        '##PARAM search (I) Name of item to search for in right <EM>Selected</EM> ListBox.
        '##RETURNS True if incoming argument appears as item in right ListBox.
        InRightList = InList(search, lstRight)
    End Function

    Public Function InLeftList(ByVal search As String) As Boolean
        '##SUMMARY Boolean check whether specified item is in left <EM>Available</EM> ListBox.
        '##PARAM search (I) Name of item to search for in left <EM>Available</EM> ListBox.
        '##RETURNS True if incoming argument appears as item in left ListBox.
        InLeftList = InList(search, lstLeft)
    End Function

    Private Function InList(ByVal S As String, ByRef Lst As ListBox) As Boolean
        '##SUMMARY Boolean check whether specified string occurs as item in ListBox.
        '##PARAM s (I) String item for which Lst object will be searched.
        '##PARAM Lst (I) List object to be searched thru for specified string item.
        '##RETURNS True if 'S' appears as item in 'Lst'.

        'Dim i As Integer 'Index used to loop thru items in Lst object
        'Dim found As Boolean 'True if item is found in list

        'i = 0
        'found = False
        'Do While Not found
        '    'UPGRADE_WARNING: Couldn't resolve default property of object Lst.List. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '    If S = Lst.Items.Item(i).ToString Then
        '        found = True
        '        'UPGRADE_WARNING: Couldn't resolve default property of object Lst.ListCount. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '    ElseIf i < Lst.Items.Count - 1 Then
        '        i = i + 1
        '    Else
        '        Exit Do
        '    End If
        'Loop

        'InList = found

        Return Lst.Items.Contains(S)

    End Function

    Private Sub cmdMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveUp.Click
        Dim i As Integer 'Index used to loop thru items in right <EM>Selected</EM> ListBox.
        Dim tmpIndex As Integer 'Used to hold value of ItemData property of list members when reordering.
        For i = 1 To lstRight.Items.Count - 1
            If lstRight.GetSelected(i) And Not lstRight.GetSelected(i - 1) Then

                tmpIndex = lstRight.SelectedIndex - 1
                lstRight.Items.Insert(i - 1, lstRight.Items(i)) ' move the selected entry up in position by 1
                lstRight.Items.RemoveAt(i + 1) ' Remove the original one (now that it has been pushed down by one)
                lstRight.SetSelected(tmpIndex, True)
                lstRight.SetSelected(i, False)
                lstRight.Update() 'Invalidate UI force redraw

                'tmp = VB6.GetItemString(lstRight, i - 1)
                'tmpData = VB6.GetItemData(lstRight, i - 1)
                'VB6.SetItemString(lstRight, i - 1, VB6.GetItemString(lstRight, i))
                'VB6.SetItemData(lstRight, i - 1, VB6.GetItemData(lstRight, i))
                'VB6.SetItemString(lstRight, i, tmp)
                'VB6.SetItemData(lstRight, i, tmpData)
                'lstRight.SetSelected(i, False)
                'lstRight.SetSelected(i - 1, True)
                RaiseEvent Change(Me, Nothing)
            End If
        Next i
    End Sub

    Private Sub cmdMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveDown.Click
        Dim i As Integer 'Index used to loop thru items in right <EM>Selected</EM> ListBox.
        'Dim tmp As String 'Used to hold names of list members when reordering.
        Dim tmpIndex As Integer 'Used to hold value of ItemData property of list members when reordering.
        For i = lstRight.Items.Count - 2 To 0 Step -1
            If lstRight.GetSelected(i) And Not lstRight.GetSelected(i + 1) Then

                tmpIndex = lstRight.SelectedIndex + 1
                lstRight.Items.Insert(i + 2, lstRight.Items(i)) ' move the selected entry Down in position by 1
                lstRight.Items.RemoveAt(i) ' Remove the original one
                lstRight.SetSelected(tmpIndex, True)
                lstRight.SetSelected(i, False)
                lstRight.Update() 'Invalidate UI force redraw


                'tmp = VB6.GetItemString(lstRight, i + 1)
                'tmpData = VB6.GetItemData(lstRight, i + 1)

                'VB6.SetItemString(lstRight, i + 1, VB6.GetItemString(lstRight, i))
                'VB6.SetItemData(lstRight, i + 1, VB6.GetItemData(lstRight, i))

                'VB6.SetItemString(lstRight, i, tmp)
                'VB6.SetItemData(lstRight, i, tmpData)

                'lstRight.SetSelected(i, False)
                'lstRight.SetSelected(i + 1, True)
                RaiseEvent Change(Me, Nothing)
            End If
        Next i
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
        Dim i As Integer 'Index used to loop thru items in right <EM>Selected</EM> ListBox.
        i = 0
        While i < lstRight.Items.Count
            If lstRight.GetSelected(i) Then MoveLeft(i) Else i = i + 1
        End While
    End Sub

    Private Sub cmdMoveRight_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveRight.Click
        '##SUMMARY Moves selected item from left <EM>Available</EM> to right <EM>Selected</EM> ListBox.
        Dim i As Integer 'Index used to loop thru items in left <EM>Available</EM> ListBox.
        i = 0
        While i < lstLeft.Items.Count
            If lstLeft.GetSelected(i) Then MoveRight(i) Else i = i + 1
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

    Private Sub ATCoSelectListSortByProp_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        '##SUMMARY Resizes components depending on how user sizes control.
        Dim lSpaceListToButtons As Integer = 6 'Quantifies size of margin
        'Quantifies width of control being used by components
        Dim UsedWidth As Integer
        UsedWidth = cmdMoveRight.Width + cmdMoveUp.Width + lSpaceListToButtons * 3
        'Adjust Width of components
        If Width - UsedWidth > 100 Then
            lstLeft.Width = (Width - UsedWidth) / 2
            lblLeft.Width = lstLeft.Width - (lblLeft.Left - lstLeft.Left)
            lstRight.Width = lstLeft.Width
            cmdMoveRight.Left = lstLeft.Left + lstLeft.Width + lSpaceListToButtons
            cmdMoveLeft.Left = cmdMoveRight.Left
            cmdMoveAllRight.Left = cmdMoveRight.Left
            cmdMoveAllLeft.Left = cmdMoveRight.Left
            lstRight.Left = cmdMoveRight.Left + cmdMoveRight.Width + lSpaceListToButtons
            lblRight.Left = lstRight.Left + (lblLeft.Left - lstLeft.Left)
            lblRight.Width = lstRight.Width - (lblRight.Left - lstRight.Left)
        End If
    End Sub

    'UPGRADE_ISSUE: VBRUN.PropertyBag type was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
    'UPGRADE_WARNING: UserControl event ReadProperties is not supported. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="92F3B58C-F772-4151-BE90-09F4A232AEAD"'
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        '##SUMMARY Reads LeftLabel and RightLabel properties of left _
        'and right ListBoxes, repectively, and sets label caption _
        'of each ListBox to that property. Default for right _
        'ListBox is <EM>Selected:</EM>, and left is <EM>Available:</EM>.
        '##PARAM PropBag (I) Intrinsic VB object containing properties _
        'of controls within main control.
        'UPGRADE_ISSUE: PropertyBag method PropBag.ReadProperty was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'UPGRADE_WARNING: Couldn't resolve default property of object PropBag.ReadProperty(). Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Me.RightLabel = PropBag.ReadProperty("RightLabel", "Selected:")
        'UPGRADE_ISSUE: PropertyBag method PropBag.ReadProperty was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'UPGRADE_WARNING: Couldn't resolve default property of object PropBag.ReadProperty(). Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Me.LeftLabel = PropBag.ReadProperty("LeftLabel", "Available:")
    End Sub

    Private Sub lstLeft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstLeft.SelectedIndexChanged
        RaiseEvent LeftListSelectedIndexChange(sender, e)
    End Sub
End Class