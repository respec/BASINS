Module modFormInteract

    ''' <summary>
    ''' Validate that all textbox and combobox controls in form are non-blank (unless they are disabled or optional)
    ''' </summary>
    ''' <param name="fm">Form to validate</param>
    ''' <param name="OptionalControls">List of controls that are optional and for which checking is skipped</param>
    ''' <returns>True if all non-optional controls are non-blank</returns>
    ''' <remarks></remarks>
    Public Function BlankCheck(ByVal fm As System.Windows.Forms.Form, Optional ByVal OptionalControls As Generic.List(Of Control) = Nothing) As Boolean
        Dim frm As New FrmListBox
        Try
            frm.ListBoxErrors.Items.Clear()
            For Each pControl As Control In fm.Controls
                If OptionalControls Is Nothing OrElse Not OptionalControls.Contains(pControl) Then
                    If ((TypeOf pControl Is System.Windows.Forms.TextBox Or TypeOf pControl Is System.Windows.Forms.ComboBox) And pControl.Enabled) Then
                        If pControl.Text = "" Then frm.ListBoxErrors.Items.Add(pControl.Name)
                    End If
                End If
            Next pControl
            If frm.ListBoxErrors.Items.Count = 0 Then
                Return True
            Else
                frm.ShowDialog()
                Return False
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If frm IsNot Nothing Then frm.Dispose()
        End Try
    End Function

    Public Sub WritetoDict(ByRef fm As System.Windows.Forms.Form)
        Try
            If Not InputDataDictionary.ContainsKey(fm.Text) Then
                InputDataDictionary.Add(fm.Text, "**********")
            End If
            WritetoDict(CType(fm, Control))
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    Private Sub WritetoDict(ByVal cntl As Control)
        For Each pControl As Control In cntl.Controls
            If InputDataDictionary.ContainsKey(pControl.Name) Then
                If TypeOf pControl Is TextBox Or TypeOf pControl Is ComboBox Then
                    InputDataDictionary(pControl.Name) = pControl.Text
                ElseIf TypeOf pControl Is DateTimePicker And pControl.Enabled Then
                    InputDataDictionary(pControl.Name) = CType(pControl, DateTimePicker).Value
                ElseIf TypeOf pControl Is RadioButton Then
                    InputDataDictionary(pControl.Name) = CType(pControl, RadioButton).Checked
                ElseIf TypeOf pControl Is CheckBox Then
                    InputDataDictionary(pControl.Name) = CType(pControl, CheckBox).Checked
                End If
            Else
                If TypeOf pControl Is TextBox Or TypeOf pControl Is ComboBox Then
                    InputDataDictionary.Add(pControl.Name, pControl.Text)
                ElseIf TypeOf pControl Is DateTimePicker And pControl.Enabled Then
                    InputDataDictionary.Add(pControl.Name, CType(pControl, DateTimePicker).Value)
                ElseIf TypeOf pControl Is RadioButton Then
                    InputDataDictionary.Add(pControl.Name, CType(pControl, RadioButton).Checked)
                ElseIf TypeOf pControl Is CheckBox Then
                    InputDataDictionary.Add(pControl.Name, CType(pControl, CheckBox).Checked)
                End If
            End If
            WritetoDict(pControl) 'write children too
        Next pControl
    End Sub

    Public Sub LoadDict()
        Try
            InitializeInputDataDictionary() 'will load too
            Dim value As String = ""
            If InputDataDictionary.TryGetValue("cbxWASP", value) Then ComputeWASPFlag = CBool(value)
            If InputDataDictionary.TryGetValue("cbxWhAEM", value) Then ComputeWhAEMFlag = CBool(value)
            If InputDataDictionary.TryGetValue("cbxSediment", value) Then ComputeSedimentFlag = CBool(value)
            If InputDataDictionary.TryGetValue("cbxMercury", value) Then ComputeMercuryFlag = CBool(value)
            If InputDataDictionary.TryGetValue("cbxBalance", value) Then ComputeBalanceFlag = CBool(value)
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    Public Sub LoadForm(ByRef fm As System.Windows.Forms.Form)
        LoadForm(CType(fm, Control))
    End Sub

    Private Sub LoadForm(ByVal cntl As Control)
        For Each pControl As Control In cntl.Controls
            Dim value As String = ""
            If InputDataDictionary.TryGetValue(pControl.Name, value) AndAlso value <> "" Then
                If TypeOf pControl Is TextBox Or TypeOf pControl Is ComboBox Then
                    pControl.Text = value
                ElseIf TypeOf pControl Is CheckBox Then
                    CType(pControl, CheckBox).Checked = value
                ElseIf TypeOf pControl Is RadioButton Then
                    CType(pControl, RadioButton).Checked = value
                End If
            End If
            LoadForm(pControl)
        Next
    End Sub

    Public Sub SaveDict()
        Dim sw As New IO.StreamWriter(gMapInputFolder & "\InputData.txt")
        For Each kv As KeyValuePair(Of String, String) In InputDataDictionary
            sw.WriteLine(String.Format("{0},{1}", kv.Key, kv.Value))
        Next
        sw.Close()
        sw.Dispose()
    End Sub

    Public Sub CreateFile()
        If Not My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt") Then
            Dim sw As New IO.StreamWriter(gMapInputFolder & "\InputData.txt")
            sw.Close()
            sw.Dispose()
        End If
    End Sub

    Public Sub Filer()
        'Create a dictionary for input data layer values
        InputDataDictionary = New Generic.Dictionary(Of String, String)
        modFormInteract.LoadDict()
    End Sub

    Public Function IfInputDataFileExists() As Boolean
        If My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt") Then
            InitializeInputDataDictionary()
            Return InputDataDictionary.ContainsKey("Simulation Options")
        Else
            Return False
        End If
    End Function

    Public Function IfDatasetDefined() As Boolean
        If My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt") Then
            InitializeInputDataDictionary()
            If InputDataDictionary("cbxMercury") = 1 Then
                Return InputDataDictionary.ContainsKey("Mercury Input") And InputDataDictionary.ContainsKey("Sediment Input") And InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
            Else
                If InputDataDictionary("cbxSediment") = 1 Then
                    Return InputDataDictionary.ContainsKey("Sediment Input") And InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
                Else
                    Return InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
                End If
            End If
        End If
    End Function
End Module