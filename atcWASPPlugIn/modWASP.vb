Imports System.Windows.Forms

Module modWASP

    Public Const REGAPPNAME As String = "WASPPLUGIN"

#Region "Get and Save Form Window Positions and Sizes and control values"

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub GetWindowPos(ByRef RegAppName As String, ByVal FormName As String, ByRef x As Int16, ByRef y As Int16, ByRef w As Int16, ByRef h As Int16, ByRef WindowState As FormWindowState)

        Dim Index, UpperBound As Int16
        Dim maxw, maxh As Int16

        'Gets an array of all the screens connected to the system.

        Dim Screens() As System.Windows.Forms.Screen = System.Windows.Forms.Screen.AllScreens
        UpperBound = Screens.GetUpperBound(0)

        For Index = 0 To UpperBound
            With Screens(Index).WorkingArea
                maxw = Math.Max(maxw, .Right)
                maxh = Math.Max(maxh, .Bottom)
            End With
        Next

        If FormName = "" Then FormName = "PrintPreview"
        w = CInt(GetSetting(RegAppName, FormName, "W", w))
        h = CInt(GetSetting(RegAppName, FormName, "H", h))
        x = CInt(GetSetting(RegAppName, FormName, "X", x))
        y = CInt(GetSetting(RegAppName, FormName, "Y", y))
        If x + w > maxw Then x = maxw - w
        x = Math.Max(0, x)
        If y + h > maxh Then y = maxh - h
        y = Math.Max(0, y)
        If CInt(GetSetting(RegAppName, FormName, "Maximized", CStr(0))) = 1 Then WindowState = FormWindowState.Maximized
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Friend Sub GetWindowPos(ByRef RegAppName As String, ByRef f As System.Windows.Forms.Form)
        Dim ws As Windows.Forms.FormWindowState
        Dim dummy As Integer
        With f
            If .IsMdiChild Then 'only set size, not position
                GetWindowPos(RegAppName, .Name, dummy, dummy, .Width, .Height, ws)
            Else
                If f.FormBorderStyle = FormBorderStyle.Sizable Or f.FormBorderStyle = FormBorderStyle.SizableToolWindow Then
                    GetWindowPos(RegAppName, .Name, .Left, .Top, .Width, .Height, ws)
                Else
                    GetWindowPos(RegAppName, .Name, .Left, .Top, dummy, dummy, ws)
                End If
            End If
        End With
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub SaveWindowPos(ByRef RegAppName As String, ByVal FormName As String, _
           ByVal x As Int16, ByVal y As Int16, ByVal w As Int16, ByVal h As Int16, _
           ByRef WindowState As FormWindowState)

        If FormName = "" Then FormName = "PrintPreview"

        If WindowState = vbNormal Then
            SaveSetting(RegAppName, FormName, "W", w)
            SaveSetting(RegAppName, FormName, "H", h)
            SaveSetting(RegAppName, FormName, "X", x)
            SaveSetting(RegAppName, FormName, "Y", y)
        End If
        SaveSetting(RegAppName, FormName, "Maximized", IIf(WindowState = FormWindowState.Maximized, 1, 0))
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Friend Sub SaveWindowPos(ByRef RegAppName As String, ByRef f As System.Windows.Forms.Form)
        With f
            SaveWindowPos(RegAppName, .Name, .Left, .Top, .Width, .Height, .WindowState)
        End With
    End Sub

    ''' <summary>
    ''' Get last (or default) value for specified control that was saved in the registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name="Cntl">Control to retrieve value for</param>
    ''' <param name="DefaultValue">If not already in registry, will set to this value (text, checked, or selected index)</param>
    ''' <remarks></remarks>
    Public Sub GetControlValue(ByVal RegAppName As String, ByRef Cntl As Control, ByVal DefaultValue As String)
        If Not Cntl.Enabled Then Exit Sub
        Dim Value As String = GetSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name, DefaultValue)
        If TypeOf Cntl Is TextBox Then
            CType(Cntl, TextBox).Text = Value
        ElseIf TypeOf Cntl Is CheckBox Then
            If Value = "" Then Value = "False"
            CType(Cntl, CheckBox).Checked = CBool(Value)
        ElseIf TypeOf Cntl Is RadioButton Then
            If Value = "" Then Value = "False"
            If CBool(Value) Then CType(Cntl, RadioButton).Checked = True 'only set if true, to avoid triggering check-changed
        ElseIf TypeOf Cntl Is ComboBox Then
            'first, retrieve list of items
            Dim ListItems As String = GetSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name & "_Items", "")
            With CType(Cntl, ComboBox)
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    .Text = Value
                Else
                    If Value = "" Then Value = "0"
                    Dim idx As Integer = Val(Value)
                    If .Items.Count = 0 Then
                        For Each s As String In ListItems.Split(";")
                            .Items.Add(s)
                        Next
                    End If
                    If idx > .Items.Count - 1 Then .SelectedIndex = -1 Else .SelectedIndex = idx
                End If
                If .Text = "" And .Items.Count > 0 Then .SelectedIndex = 0
            End With
        ElseIf TypeOf Cntl Is ListBox Then
            If Value = "" Then Value = "-1"
            Dim idx As Integer = Val(Value)
            With CType(Cntl, ListBox)
                If idx > .Items.Count - 1 Then .SelectedIndex = -1 Else .SelectedIndex = idx
            End With
        ElseIf TypeOf Cntl Is DateTimePicker Then
            With CType(Cntl, DateTimePicker)
                If IsDate(Value) Then
                    .Value = Value
                Else
                    .Value = DateTime.Now
                End If
            End With
        Else
            'not all control types are supported; if invalid control type is passed, will ignore
            'Debug.Print("Invalid control in GetControlValue: " & Cntl.GetType.ToString)
            'Debug.Assert(False)
        End If
    End Sub

    ''' <summary>
    ''' Get last values for all controls on a form that was saved in the registry (default values cannot be set explictly, will use defaults from designer)
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name= "Container">Form or control containing controls to set values for</param>
    ''' <remarks>Want to get and set in order of tag index, as there may be cascading events</remarks>
    Public Sub GetControlValues(ByVal RegAppName As String, ByRef Container As Control)
        For Indx As Integer = 0 To Container.Controls.Count - 1
            For Each Cntl As Control In Container.Controls
                If Cntl.TabIndex = Indx And Cntl.Visible Then
                    GetControlValue(RegAppName, Cntl, "")
                    GetControlValues(RegAppName, Cntl)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Save value for specified control to registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name="Cntl">Control to set value for</param>
    ''' <remarks></remarks>
    Public Sub SaveControlValue(ByVal RegAppName As String, ByRef Cntl As Control)
        Dim Value As String
        If TypeOf Cntl Is TextBox Then
            Value = CType(Cntl, TextBox).Text
        ElseIf TypeOf Cntl Is CheckBox Then
            Value = CType(Cntl, CheckBox).Checked.ToString
        ElseIf TypeOf Cntl Is RadioButton Then
            Value = CType(Cntl, RadioButton).Checked.ToString
        ElseIf TypeOf Cntl Is ComboBox Then
            With CType(Cntl, ComboBox)
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    Value = .Text
                Else
                    Value = .SelectedIndex.ToString
                End If
                Dim ListItems As String = ""
                For Each s As String In .Items
                    ListItems &= IIf(ListItems = "", "", ";") & s
                Next
                SaveSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name & "_Items", ListItems)
            End With
        ElseIf TypeOf Cntl Is ListBox Then
            Value = CType(Cntl, ListBox).SelectedIndex.ToString
        ElseIf TypeOf Cntl Is DateTimePicker Then
            Value = CType(Cntl, DateTimePicker).Value.ToString
        Else
            'Debug.Assert(False)
            Exit Sub
        End If
        SaveSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name, Value)
    End Sub

    ''' <summary>
    ''' Save values for all controls on a form to registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name= "Container">Form or control containing controls to set values for</param>
    ''' <remarks>Want to get and set in order of tag index, as there may be cascading events</remarks>
    Public Sub SaveControlValues(ByVal RegAppName As String, ByRef Container As Control)
        For Indx As Integer = 0 To Container.Controls.Count - 1
            For Each Cntl As Control In Container.Controls
                If Cntl.TabIndex = Indx Then
                    SaveControlValue(RegAppName, Cntl)
                    SaveControlValues(RegAppName, Cntl) 'recursively look at controls that may be contained within this control
                End If
            Next
        Next
    End Sub

#End Region

    ''' <summary>
    ''' Display warning message
    ''' </summary>
    ''' <param name="WarningText">Warning text to display</param>
    Friend Function WarningMsg(ByVal WarningText As String) As DialogResult
        Return MapWinUtility.Logger.Message(WarningText, "Warning", IIf(WarningText.Contains("?"), MessageBoxButtons.OKCancel, MessageBoxButtons.OK), MessageBoxIcon.Warning, DialogResult.OK)
    End Function

    ''' <summary>
    ''' Display warning message
    ''' </summary>
    ''' <param name="WarningTextformat">Warning text to display as string format</param>
    ''' <param name="Args">Arguments for string format</param>
    Friend Sub WarningMsg(ByVal WarningTextFormat As String, ByVal ParamArray Args() As Object)
        WarningMsg(StringFormat(WarningTextFormat, Args))
    End Sub

    ''' <summary>
    ''' Display error message
    ''' </summary>
    ''' <param name="ErrorText">Error text to display</param>
    ''' <param name="ex">Exception (will display traceback info)</param>
    Friend Sub ErrorMsg(Optional ByVal ErrorText As String = "", Optional ByVal ex As Exception = Nothing)
        If ErrorText = "" Then ErrorText = "An unhandled error has occurred in the BASINS GBMM tool."
        If ex IsNot Nothing Then ErrorText &= vbCr & vbCr & "The detailed error message was:" & vbCr & vbCr & ex.ToString
        MapWinUtility.Logger.Message(ErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK)
    End Sub

    ''' <summary>
    ''' Format string using standard String.Format, except substitute \t and \n with tab and newline characters
    ''' </summary>
    Friend Function StringFormat(ByVal Format As String, ByVal ParamArray Args() As Object) As String
        Return String.Format(Format, Args).Replace("\t", vbTab).Replace("\n", vbNewLine)
    End Function

    ''' <summary>
    ''' Test object; if dbnull, nothing or empty string, return default value
    ''' </summary>
    Friend Function TestNull(ByVal Value As Object, ByVal DefaultValue As Object) As Object
        If IsDBNull(Value) OrElse IsNothing(Value) OrElse (TypeOf Value Is String AndAlso Value = "") Then Return DefaultValue Else Return Value
    End Function

    Friend Function OpenFile(ByVal InitialDir As String) As String
        With New OpenFileDialog
            .AddExtension = True
            .CheckFileExists = True
            .CheckPathExists = True
            .DefaultExt = ".WaspBuilder"
            .Filter = "WASP Builder input files (*.WaspBuilder)|*.WaspBuilder"
            .FilterIndex = 0
            .InitialDirectory = InitialDir
            If Not String.IsNullOrEmpty(InitialDir) AndAlso Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Open WASP Builder File"
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Return .FileName
            Else
                Return ""
            End If
            .Dispose()
        End With
    End Function

End Module
