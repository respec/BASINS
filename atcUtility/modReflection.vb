Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports System.Reflection
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Module modReflection

    Private pDontNeedTest As ArrayList

    'Private Function GetEmbeddedFileAsStream(ByVal fileName As String, ByVal aAssembly As Assembly) As IO.Stream
    '    Return aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName)
    'End Function

    ''' <summary>
    ''' Extracts an embedded file out of a given assembly as a string
    ''' </summary>
    ''' <param name="fileName">Name of the file to extract.</param>
    ''' <returns>A string containing the file data.</returns>
    Public Function GetEmbeddedFileAsString(ByVal fileName As String, ByVal aFullResourceName As String) As String
        Dim lAssembly As Assembly = Assembly.GetCallingAssembly
        Dim lReader As New IO.StreamReader(lAssembly.GetManifestResourceStream(aFullResourceName))
        Return lReader.ReadToEnd()
    End Function

    ''' <summary>
    ''' Extracts an embedded file out of a given assembly as a string
    ''' </summary>
    ''' <param name="aAssembly">Assembly file is embedded in.</param>
    ''' <param name="fileName">Name of the file to extract.</param>
    ''' <returns>A string containing the file data.</returns>
    Public Function GetEmbeddedFileAsString(ByVal fileName As String, Optional ByVal aAssembly As Assembly = Nothing) As String
        If aAssembly Is Nothing Then aAssembly = Assembly.GetCallingAssembly
        Dim lReader As New IO.StreamReader(aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName))
        Return lReader.ReadToEnd()
    End Function

    Public Function GetEmbeddedFileAsBinaryReader(ByVal aFileName As String, Optional ByVal aAssembly As Assembly = Nothing, Optional ByVal aPrefixName As String = "") As IO.BinaryReader
        Try
            If aAssembly Is Nothing Then aAssembly = Assembly.GetCallingAssembly
            Dim lPrefixName As String = aPrefixName
            If lPrefixName.Length = 0 Then
                lPrefixName = aAssembly.GetName().Name
            End If
            Logger.Dbg("Assembly " & aAssembly.GetName().Name & " PrefixName " & lPrefixName & " FileName " & aFileName)
            Dim lStream As IO.Stream = aAssembly.GetManifestResourceStream(lPrefixName + "." + aFileName)
            If lStream Is Nothing Then
                Logger.Dbg("FailedToGetStream")
                Return Nothing
            End If
            Logger.Dbg("StreamLength " & lStream.Length)
            Dim lBinaryReader As New IO.BinaryReader(lStream)
            Logger.Dbg("BinaryReader " & lBinaryReader.PeekChar)
            Return lBinaryReader
        Catch lEx As Exception
            Logger.Dbg(lEx.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetEmbeddedFileAsBitmap(ByVal fileName As String, Optional ByVal aAssembly As Assembly = Nothing) As Drawing.Bitmap
        If aAssembly Is Nothing Then aAssembly = Assembly.GetCallingAssembly
        Dim lStream As IO.Stream = aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName)
        Dim lBitmap As New Drawing.Bitmap(lStream)
        lStream.Close()
        Return lBitmap
    End Function

    Public Function BuildMissingTests(ByVal aSavePath As String) As String
        Dim s As String, t As String
        Dim lEntryModule As String, lEntryFunction As String
        Dim lTestModule As String, lTestFunction As String
        Dim lAddModule As String, lAddFunction As String, oAddModule As String
        Dim found As Boolean
        Dim sEntry As New Collection
        Dim sTest As New Collection
        Dim sAdd As New Collection
        Dim sRec As String

        Dim a As [Assembly] = Reflection.Assembly.GetCallingAssembly 'GetExecutingAssembly

        aSavePath = System.IO.Path.Combine(aSavePath, a.GetName.Name) 'byval does not change orig
        System.IO.Directory.CreateDirectory(aSavePath)

        s = ReflectAssemblyAsString(a)
        SaveFileString(aSavePath & g_PathChar & "Reflection.txt", s)

        While Len(s) > 0
            sRec = StrSplit(s, vbCrLf, "")
            If (InStr(sRec, "<none>") = 0 And InStr(sRec, "Public") > 0) Then
                If Left(sRec, 5).ToLower = "test_" Then
                    sTest.Add(sRec)
                Else
                    sEntry.Add(sRec)
                End If
            End If
        End While

        s = ""
        For Each lEntry As String In sEntry
            lEntryModule = StrSplit(lEntry, ":", "")
            t = StrSplit(lEntry, ":", "")
            lEntryFunction = lEntry
            found = False
            s = s & lEntryModule & ":" & lEntryFunction
            For Each lTest As String In sTest
                lTestModule = StrSplit(lTest, ":", "")
                If "Test_" & lEntryModule = lTestModule Then
                    t = StrSplit(lTest, ":", "")
                    lTestFunction = Mid(lTest, 5)
                    If lTestFunction = lEntryFunction Then 'already have it
                        found = True
                        s = s & ":match" & vbCrLf
                        Exit For
                    End If
                End If
            Next
            If Not found Then
                If lEntryModule = "modReflection" Then
                    s = s & ":skip" & vbCrLf
                Else
                    s = s & ":add" & vbCrLf
                    sAdd.Add(lEntryModule & ":" & lEntryFunction)
                End If
            End If
        Next
        s = s & "CountTests:" & sTest.Count & vbCrLf
        s = s & "CountEntry:" & sEntry.Count & vbCrLf
        s = s & "CountAdd:" & sAdd.Count & vbCrLf
        SaveFileString(aSavePath & g_PathChar & "TestsToAdd.txt", s)

        s = "Option Strict Off" & vbCrLf & _
            "Option Explicit On" & vbCrLf & _
            "'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license" & vbCrLf & _
            vbCrLf & _
            "Imports NUnit.Framework" & vbCrLf & _
            "Imports ATCUtility.modReflection" & vbCrLf & _
            vbCrLf & _
            "<TestFixture()> Public Class Test_Builder" & vbCrLf & _
            "  Public Sub TestsAllPresent()" & vbCrLf & _
            "    Dim lTestBuildStatus As String = BuildMissingTests(""c:\test\"")" & vbCrLf & _
            "    Assert.AreEqual(""All tests present."", lTestBuildStatus, lTestBuildStatus)" & vbCrLf & _
            "  End Sub" & vbCrLf & _
            "End Class" & vbCrLf & vbCrLf

        oAddModule = ""
        For Each lAdd As String In sAdd
            ' s = s & "process:" & lAdd & vbCrLf
            lAddModule = StrSplit(lAdd, ":", "")
            If lAddModule <> oAddModule Then
                If Len(oAddModule) > 0 Then
                    s &= "End Class" & vbCrLf & vbCrLf
                End If
                s &= "<TestFixture()> Public Class Test_" & lAddModule & vbCrLf & vbCrLf
                oAddModule = lAddModule
            End If
            lAddFunction = StrSplit(lAdd, ":", "")
            ' how do we get some arguments?" 
            s &= "  Public Sub Test" & lAddFunction & "()" & vbCrLf & _
                 "    '" & lAddFunction & "()" & vbCrLf & _
                 "    Assert.Ignore(" & Chr(34) & "Test not yet written" & Chr(34) & ")" & vbCrLf & _
                 "  End Sub" & vbCrLf & vbCrLf
        Next
        s = s & "End Class" & vbCrLf
        aSavePath = aSavePath & g_PathChar & "Test_" & a.GetName.Name & ".vb"
        SaveFileString(aSavePath, s)

        If sAdd.Count = 0 Then
            s = "All tests present."
        Else
            s = sAdd.Count & " tests needed. See '" & aSavePath & "' for test stub code."
        End If

        Return s

    End Function

    Private Function ReflectAssemblyAsString(ByVal mA As [Assembly]) As String

        Dim lTypes As Type() = mA.GetTypes
        Dim lReturnStr As String = ""

        For Each lType As Type In lTypes
            lReturnStr &= MethodDetails(lType, "Public,Static", BindingFlags.Public Or BindingFlags.Static)
            lReturnStr &= MethodDetails(lType, "Public,Instance", BindingFlags.Public Or BindingFlags.Instance)
            's &= MethodDetails(lType, "NonPub,Static", BindingFlags.NonPublic Or BindingFlags.Static)
            's &= MethodDetails(lType, "NonPub,Instance", BindingFlags.NonPublic Or BindingFlags.Instance)
        Next
        Return lReturnStr
    End Function

    Private Function MethodDetails(ByRef aT As Type, ByVal aMethodType As String, ByVal aFlag As Integer) As String
        Dim lReturnStr As String = ""
        Dim lThisMethodStr As String

        Dim lMethods As MethodInfo() = aT.GetMethods(aFlag)
        If lMethods.Length = 0 Then
            'lReturnStr &= aT.Name & ":" & aMethodType & ":" & "<none>" & vbCrLf
        Else
            For Each m As MethodInfo In lMethods
                If NeedToTest(m.Name) Then
                    lThisMethodStr = aT.Name & ":" & aMethodType & ":" & m.Name & vbCrLf
                    If InStr(lReturnStr, lThisMethodStr) = 0 Then 'only add if not already added
                        lReturnStr &= lThisMethodStr
                    End If
                End If
            Next
        End If
        Return lReturnStr
    End Function

    Public Function MethodAvailable(ByRef aT As Type, ByVal aMethodName As String) As Boolean
        Dim lMethodInfo As System.Reflection.MethodInfo = aT.GetMethod(aMethodName)
        Return lMethodInfo IsNot Nothing
    End Function

    Private Function NeedToTest(ByVal aMethodName As String) As Boolean
        If pDontNeedTest Is Nothing Then
            pDontNeedTest = New ArrayList
            Dim lDontNeedTest() As String = { _
            "<none>", _
            "GetHashCode", _
            "GetType", _
            "set_AutoScaleBaseSize", _
            "get_AutoScaleBaseSize", _
            "set_ActiveControl", _
            "get_ActiveControl", _
            "set_AutoScroll", _
            "get_AutoScroll", _
            "get_Handle", _
            "Invoke", _
            "EndInvoke", _
            "BeginInvoke", _
            "get_InvokeRequired", _
            "ResetText", _
            "Refresh", _
            "ResetRightToLeft", _
            "ResetForeColor", _
            "ResetFont", _
            "ResetCursor", _
            "ResetBackColor", _
            "PreProcessMessage", _
            "set_Text", _
            "get_Text", _
            "set_RightToLeft", _
            "get_RightToLeft", _
            "set_ForeColor", _
            "get_ForeColor", _
            "set_Font", _
            "get_Font", _
            "get_Focused", _
            "set_Dock", _
            "get_Dock", _
            "get_DisplayRectangle", _
            "set_Cursor", _
            "get_Cursor", _
            "set_ContextMenu", _
            "get_ContextMenu", _
            "set_BindingContext", _
            "get_BindingContext", _
            "set_BackgroundImage", _
            "get_BackgroundImage", _
            "set_BackColor", _
            "get_BackColor", _
            "set_Anchor", _
            "get_Anchor", _
            "set_AllowDrop", _
            "get_AllowDrop", _
            "Dispose", _
            "remove_Disposed", _
            "add_Disposed", _
            "set_Site", _
            "get_Site", _
            "CreateObjRef", _
            "InitializeLifetimeService", _
            "GetLifetimeService", _
            "get_AcceptButton", _
            "set_AcceptButton", _
            "get_ActiveMdiChild", _
            "get_AllowTransparency", _
            "set_AllowTransparency", _
            "get_AutoScale", _
            "set_AutoScale", _
            "get_FormBorderStyle", _
            "set_FormBorderStyle", _
            "get_CancelButton", _
            "set_CancelButton", _
            "get_ClientSize", _
            "set_ClientSize", _
            "get_ControlBox", _
            "set_ControlBox", _
            "get_DesktopBounds", _
            "set_DesktopBounds", _
            "get_DesktopLocation", _
            "set_DesktopLocation", _
            "get_DialogResult", _
            "set_DialogResult", _
            "get_HelpButton", _
            "set_HelpButton", _
            "get_Icon", _
            "set_Icon", _
            "get_IsMdiChild", _
            "get_IsMdiContainer", _
            "set_IsMdiContainer", _
            "get_KeyPreview", _
            "set_KeyPreview", _
            "add_MaximizedBoundsChanged", _
            "remove_MaximizedBoundsChanged", _
            "get_MaximumSize", _
            "set_MaximumSize", _
            "add_MaximumSizeChanged", _
            "remove_MaximumSizeChanged", _
            "get_Menu", _
            "set_Menu", _
            "get_MinimumSize", _
            "set_MinimumSize", _
            "add_MinimumSizeChanged", _
            "remove_MinimumSizeChanged", _
            "get_MaximizeBox", _
            "set_MaximizeBox", _
            "get_MdiChildren", _
            "get_MdiParent", _
            "set_MdiParent", _
            "get_MergedMenu", _
            "get_MinimizeBox", _
            "set_MinimizeBox", _
            "get_Modal", _
            "get_Opacity", _
            "set_Opacity", _
            "get_OwnedForms", _
            "get_Owner", _
            "set_Owner", _
            "get_ShowInTaskbar", _
            "set_ShowInTaskbar", _
            "get_Size", _
            "set_Size", _
            "get_SizeGripStyle", _
            "set_SizeGripStyle", _
            "get_StartPosition", _
            "set_StartPosition", _
            "get_TabIndex", _
            "set_TabIndex", _
            "add_TabIndexChanged", _
            "remove_TabIndexChanged", _
            "get_TopLevel", _
            "set_TopLevel", _
            "get_TopMost", _
            "set_TopMost", _
            "get_TransparencyKey", _
            "set_TransparencyKey", _
            "get_IsRestrictedWindow", _
            "get_WindowState", _
            "set_WindowState", _
            "Activate", _
            "add_Activated", _
            "remove_Activated", _
            "add_Closing", _
            "remove_Closing", _
            "add_Closed", _
            "remove_Closed", _
            "add_Deactivate", _
            "remove_Deactivate", _
            "add_Load", _
            "remove_Load", _
            "add_MdiChildActivate", _
            "remove_MdiChildActivate", _
            "add_MenuComplete", _
            "remove_MenuComplete", _
            "add_MenuStart", _
            "remove_MenuStart", _
            "add_InputLanguageChanged", _
            "remove_InputLanguageChanged", _
            "add_InputLanguageChanging", _
            "remove_InputLanguageChanging", _
            "AddOwnedForm", _
            "Close", _
            "LayoutMdi", _
            "RemoveOwnedForm", _
            "SetDesktopBounds", _
            "SetDesktopLocation", _
            "ShowDialog", _
            "get_ParentForm", _
            "Validate", _
            "get_AutoScrollMargin", _
            "set_AutoScrollMargin", _
            "get_AutoScrollPosition", _
            "set_AutoScrollPosition", _
            "get_AutoScrollMinSize", _
            "set_AutoScrollMinSize", _
            "get_DockPadding", _
            "ScrollControlIntoView", _
            "SetAutoScrollMargin", _
            "get_AccessibilityObject", _
            "get_AccessibleDefaultActionDescription", _
            "set_AccessibleDefaultActionDescription", _
            "get_AccessibleDescription", _
            "set_AccessibleDescription", _
            "get_AccessibleName", _
            "set_AccessibleName", _
            "get_AccessibleRole", _
            "set_AccessibleRole", _
            "add_BackColorChanged", _
            "remove_BackColorChanged", _
            "add_BackgroundImageChanged", _
            "remove_BackgroundImageChanged", _
            "get_DataBindings", _
            "ResetBindings", _
            "add_BindingContextChanged", _
            "remove_BindingContextChanged", _
            "get_Bottom", _
            "get_Bounds", _
            "set_Bounds", _
            "get_CanFocus", _
            "get_CanSelect", _
            "get_Capture", _
            "set_Capture", _
            "get_CausesValidation", _
            "set_CausesValidation", _
            "add_CausesValidationChanged", _
            "remove_CausesValidationChanged", _
            "get_ClientRectangle", _
            "get_CompanyName", _
            "get_ContainsFocus", _
            "add_ContextMenuChanged", _
            "remove_ContextMenuChanged", _
            "get_Controls", _
            "get_Created", _
            "add_CursorChanged", _
            "remove_CursorChanged", _
            "get_IsDisposed", _
            "get_Disposing", _
            "add_DockChanged", _
            "remove_DockChanged", _
            "get_Enabled", _
            "set_Enabled", _
            "add_EnabledChanged", _
            "remove_EnabledChanged", _
            "add_FontChanged", _
            "remove_FontChanged", _
            "add_ForeColorChanged", _
            "remove_ForeColorChanged", _
            "get_HasChildren", _
            "get_Height", _
            "set_Height", _
            "get_IsHandleCreated", _
            "get_ImeMode", _
            "set_ImeMode", _
            "get_IsAccessible", _
            "set_IsAccessible", _
            "get_Left", _
            "set_Left", _
            "get_Location", _
            "set_Location", _
            "add_LocationChanged", _
            "remove_LocationChanged", _
            "get_Name", _
            "set_Name", _
            "get_Parent", _
            "set_Parent", _
            "get_ProductName", _
            "get_ProductVersion", _
            "get_RecreatingHandle", _
            "get_Region", _
            "set_Region", _
            "get_Right", _
            "add_RightToLeftChanged", _
            "remove_RightToLeftChanged", _
            "add_SizeChanged", _
            "remove_SizeChanged", _
            "get_TabStop", _
            "set_TabStop", _
            "add_TabStopChanged", _
            "remove_TabStopChanged", _
            "get_Tag", _
            "set_Tag", _
            "add_TextChanged", _
            "remove_TextChanged", _
            "get_Top", _
            "set_Top", _
            "get_TopLevelControl", _
            "get_Visible", _
            "set_Visible", _
            "add_VisibleChanged", _
            "remove_VisibleChanged", _
            "get_Width", _
            "set_Width", _
            "get_WindowTarget", _
            "set_WindowTarget", _
            "add_Click", _
            "remove_Click", _
            "add_ControlAdded", _
            "remove_ControlAdded", _
            "add_ControlRemoved", _
            "remove_ControlRemoved", _
            "add_DragDrop", _
            "remove_DragDrop", _
            "add_DragEnter", _
            "remove_DragEnter", _
            "add_DragOver", _
            "remove_DragOver", _
            "add_DragLeave", _
            "remove_DragLeave", _
            "add_GiveFeedback", _
            "remove_GiveFeedback", _
            "add_HandleCreated", _
            "remove_HandleCreated", _
            "add_HandleDestroyed", _
            "remove_HandleDestroyed", _
            "add_HelpRequested", _
            "remove_HelpRequested", _
            "add_Invalidated", _
            "remove_Invalidated", _
            "add_Paint", _
            "remove_Paint", _
            "add_QueryContinueDrag", _
            "remove_QueryContinueDrag", _
            "add_QueryAccessibilityHelp", _
            "remove_QueryAccessibilityHelp", _
            "add_DoubleClick", _
            "remove_DoubleClick", _
            "add_Enter", _
            "remove_Enter", _
            "add_GotFocus", _
            "remove_GotFocus", _
            "add_ImeModeChanged", _
            "remove_ImeModeChanged", _
            "add_KeyDown", _
            "remove_KeyDown", _
            "add_KeyPress", _
            "remove_KeyPress", _
            "add_KeyUp", _
            "remove_KeyUp", _
            "add_Layout", _
            "remove_Layout", _
            "add_Leave", _
            "remove_Leave", _
            "add_LostFocus", _
            "remove_LostFocus", _
            "add_MouseDown", _
            "remove_MouseDown", _
            "add_MouseEnter", _
            "remove_MouseEnter", _
            "add_MouseLeave", _
            "remove_MouseLeave", _
            "add_MouseHover", _
            "remove_MouseHover", _
            "add_MouseMove", _
            "remove_MouseMove", _
            "add_MouseUp", _
            "remove_MouseUp", _
            "add_MouseWheel", _
            "remove_MouseWheel", _
            "add_Move", _
            "remove_Move", _
            "add_Resize", _
            "remove_Resize", _
            "add_ChangeUICues", _
            "remove_ChangeUICues", _
            "add_StyleChanged", _
            "remove_StyleChanged", _
            "add_SystemColorsChanged", _
            "remove_SystemColorsChanged", _
            "add_Validating", _
            "remove_Validating", _
            "add_Validated", _
            "remove_Validated", _
            "add_ParentChanged", _
            "remove_ParentChanged", _
            "BringToFront", _
            "Contains", _
            "CreateGraphics", _
            "CreateControl", _
            "DoDragDrop", _
            "FindForm", _
            "Focus", _
            "GetChildAtPoint", _
            "GetContainerControl", _
            "GetNextControl", _
            "Hide", _
            "Invalidate", _
            "PerformLayout", _
            "PointToClient", _
            "PointToScreen", _
            "ResetImeMode", _
            "RectangleToClient", _
            "RectangleToScreen", _
            "ResumeLayout", _
            "Scale", _
            "Select", _
            "SelectNextControl", _
            "SendToBack", _
            "SetBounds", _
            "Show", _
            "SuspendLayout", _
            "Update", _
            "get_Container"}
            pDontNeedTest.AddRange(lDontNeedTest)
        End If
        Return Not pDontNeedTest.Contains(aMethodName)
    End Function

    Public Function FieldNames(ByVal aType As Type, ByVal aDelimiter As String) As String
        Dim lNames As String = ""
        For Each lField As Reflection.FieldInfo In aType.GetFields()
            lNames &= lField.Name & aDelimiter
        Next
        If lNames.Length > aDelimiter.Length Then 'Trim trailing delimiter
            lNames = lNames.Substring(0, lNames.Length - aDelimiter.Length)
        End If
        Return lNames
    End Function

    Public Function FieldValues(ByVal aObject As Object, ByVal aDelimiter As String) As String
        Dim lValues As String = ""
        For Each lField As Reflection.FieldInfo In aObject.GetType.GetFields()
            lValues &= lField.GetValue(aObject) & aDelimiter
        Next
        If lValues.Length > aDelimiter.Length Then 'Trim trailing delimiter
            lValues = lValues.Substring(0, lValues.Length - aDelimiter.Length)
        End If
        Return lValues
    End Function

    Public Function NumberObjects(ByVal aObjects As ArrayList, ByVal aFieldToNumber As String, Optional ByVal aPrefix As String = "", Optional ByVal aStartIndex As Integer = 1) As ArrayList
        If aObjects IsNot Nothing AndAlso aObjects.Count > 0 Then
            Dim lType As Type = aObjects.Item(0).GetType
            Dim lField As FieldInfo = lType.GetField(aFieldToNumber)
            For Each lItem As Object In aObjects
                Select Case Type.GetTypeCode(lField.FieldType)
                    Case TypeCode.String
                        Dim lValue As String = lField.GetValue(lItem)
                        If lValue Is Nothing OrElse lValue.Length = 0 Then
                            lField.SetValue(lItem, aPrefix & aStartIndex)
                        End If
                    Case TypeCode.Int32 : lField.SetValue(lItem, aStartIndex)
                    Case TypeCode.Int64 : lField.SetValue(lItem, CLng(aStartIndex))
                End Select
                aStartIndex += 1
            Next
        End If
        Return aObjects
    End Function

    ''' <summary>
    ''' Set a property or field given its name and a new value
    ''' </summary>
    ''' <param name="aObject">Object whose property or field needs to be set</param>
    ''' <param name="aFieldName">Name of property or field to set</param>
    ''' <param name="aValue">New value to set</param>
    Public Function SetSomething(ByRef aObject As Object, ByVal aFieldName As String, ByVal aValue As Object) As Boolean
        Return (SetSomething(aObject, aFieldName, aValue, True).Length = 0)
    End Function

    ''' <summary>
    ''' Set a property or field given its name and a new value
    ''' </summary>
    ''' <param name="aObject">Object whose property or field needs to be set</param>
    ''' <param name="aFieldName">Name of property or field to set</param>
    ''' <param name="aValue">New value to set</param>
    ''' <param name="aLogProblems">Quiet flag</param>
    Public Function SetSomething(ByRef aObject As Object, ByVal aFieldName As String, ByVal aValue As Object, _
                                 ByVal aLogProblems As Boolean) As String
        Dim lSetSomething As String = ""
        Try
            Dim lType As Type = aObject.GetType
            Dim lProperty As Reflection.PropertyInfo = lType.GetProperty(aFieldName)
            If lProperty IsNot Nothing Then
                Select Case Type.GetTypeCode(lProperty.PropertyType)
                    Case TypeCode.Boolean : lProperty.SetValue(aObject, CBool(aValue), Nothing)
                    Case TypeCode.Byte : lProperty.SetValue(aObject, CByte(aValue), Nothing)
                    Case TypeCode.Char : lProperty.SetValue(aObject, CChar(aValue), Nothing)
                    Case TypeCode.DateTime : lProperty.SetValue(aObject, CDate(aValue), Nothing)
                    Case TypeCode.Decimal : lProperty.SetValue(aObject, CDec(aValue), Nothing)
                    Case TypeCode.Double : lProperty.SetValue(aObject, CDbl(aValue), Nothing)
                    Case TypeCode.Int16 : lProperty.SetValue(aObject, CShort(aValue), Nothing)
                    Case TypeCode.Int32 : lProperty.SetValue(aObject, CInt(aValue), Nothing)
                    Case TypeCode.Int64 : lProperty.SetValue(aObject, CLng(aValue), Nothing)
                    Case TypeCode.SByte : lProperty.SetValue(aObject, CSByte(aValue), Nothing)
                    Case TypeCode.Single : lProperty.SetValue(aObject, CSng(aValue), Nothing)
                    Case TypeCode.String : lProperty.SetValue(aObject, CStr(aValue), Nothing)
                    Case TypeCode.UInt16 : lProperty.SetValue(aObject, CUShort(aValue), Nothing)
                    Case TypeCode.UInt32 : lProperty.SetValue(aObject, CUInt(aValue), Nothing)
                    Case TypeCode.UInt64 : lProperty.SetValue(aObject, CULng(aValue), Nothing)
                    Case Else
                        lSetSomething = "Unable to set " & lType.Name & "." & aFieldName & ": unknown type " & lProperty.PropertyType.Name
                End Select
            Else
                Dim lField As Reflection.FieldInfo = lType.GetField(aFieldName)
                If lField IsNot Nothing Then
                    Select Case Type.GetTypeCode(lField.FieldType)
                        Case TypeCode.Boolean : lField.SetValue(aObject, CBool(aValue))
                        Case TypeCode.Byte : lField.SetValue(aObject, CByte(aValue))
                        Case TypeCode.Char : lField.SetValue(aObject, CChar(aValue))
                        Case TypeCode.DateTime : lField.SetValue(aObject, CDate(aValue))
                        Case TypeCode.Decimal : lField.SetValue(aObject, CDec(aValue))
                        Case TypeCode.Double : lField.SetValue(aObject, CDbl(aValue))
                        Case TypeCode.Int16 : lField.SetValue(aObject, CShort(aValue))
                        Case TypeCode.Int32 : lField.SetValue(aObject, CInt(aValue))
                        Case TypeCode.Int64 : lField.SetValue(aObject, CLng(aValue))
                        Case TypeCode.SByte : lField.SetValue(aObject, CSByte(aValue))
                        Case TypeCode.Single : lField.SetValue(aObject, CSng(aValue))
                        Case TypeCode.String : lField.SetValue(aObject, CStr(aValue))
                        Case TypeCode.UInt16 : lField.SetValue(aObject, CUShort(aValue))
                        Case TypeCode.UInt32 : lField.SetValue(aObject, CUInt(aValue))
                        Case TypeCode.UInt64 : lField.SetValue(aObject, CULng(aValue))
                        Case Else
                            lSetSomething = "Unable to set " & lType.Name & "." & aFieldName & ": unknown type " & lField.FieldType.Name
                    End Select
                Else
                    lSetSomething = "Unable to set " & lType.Name & "." & aFieldName & ": unknown field or property"
                End If
            End If
            If aLogProblems AndAlso lSetSomething.Length > 0 Then
                Logger.Dbg(lSetSomething)
            End If
        Catch e As Exception
            If aLogProblems Then
                Logger.Dbg("Exception setting field " & aFieldName & ": " & e.Message)
            End If
        End Try
        Return lSetSomething
    End Function

    Public Function GetSomething(ByRef aObject As Object, ByVal aFieldName As String) As Object
        Dim lType As Type = aObject.GetType
        Dim lProperty As Reflection.PropertyInfo = lType.GetProperty(aFieldName)
        If lProperty IsNot Nothing Then
            Return lProperty.GetValue(aObject, Nothing)
        Else
            Dim lField As Reflection.FieldInfo = lType.GetField(aFieldName)
            If lField IsNot Nothing Then
                Return lField.GetValue(aObject)
            End If
        End If
        Return Nothing
    End Function

    Public Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20) & _
                    " Local(MB):" & System.GC.GetTotalMemory(True) / (2 ^ 20)
    End Function

    ''' <summary>
    ''' Copy the Text or Checked state from one Windows.Forms.Control to another
    ''' Also copies state of child controls, if any
    ''' </summary>
    ''' <param name="aOriginalControl">Control to copy state from</param>
    ''' <param name="aCopyTo">Control to copy state to</param>
    ''' <remarks>Useful for copying the state of an Options form to support Cancel</remarks>
    Public Sub CopyControlState(ByVal aOriginalControl As Windows.Forms.Control, ByVal aCopyTo As Windows.Forms.Control)
        SetSomething(aCopyTo, "Text", GetSomething(aOriginalControl, "Text"))
        SetSomething(aCopyTo, "Checked", GetSomething(aOriginalControl, "Checked"))

        For lindex As Integer = 0 To aOriginalControl.Controls.Count - 1
            Dim lOriginalControl As Windows.Forms.Control = aOriginalControl.Controls.Item(lindex)
            Dim lCopyTo As Windows.Forms.Control = aCopyTo.Controls.Item(lindex)
            CopyControlState(lOriginalControl, lCopyTo)
        Next
    End Sub

    'Public Sub ConnectObjects(ByRef aListOfObjects As ArrayList, _
    '                          ByVal aIdFieldName As String, _
    '                          ByVal aLinkIdFieldName As String, _
    '                          ByVal aLinkFieldName As String)
    '    If Not aListOfObjects Is Nothing AndAlso aListOfObjects.Count > 0 Then
    '        Dim lType As Type = aListOfObjects(0).GetType
    '        Dim lIDField As FieldInfo = Nothing
    '        Dim lLinkIDField As FieldInfo = Nothing
    '        Dim lLinkField As FieldInfo = Nothing

    '        For Each lField As Reflection.FieldInfo In lType.GetFields()
    '            If lField.Name = aIdFieldName Then lIDField = lField
    '            If lField.Name = aLinkFieldName Then lLinkField = lField
    '            If lField.Name = aLinkIdFieldName Then lLinkField = lField
    '        Next

    '        If lIDField Is Nothing Then
    '            Logger.Dbg("ConnectObjects: ID Field not found: " & aIdFieldName)
    '        ElseIf lLinkField Is Nothing Then
    '            Logger.Dbg("ConnectObjects: Link Field not found: " & aLinkFieldName)
    '        ElseIf lLinkIDField Is Nothing Then
    '            Logger.Dbg("ConnectObjects: Link ID Field not found: " & aLinkIdFieldName)
    '        Else
    '            Dim lNumLinked As Integer = 0
    '            For Each lObject As Object In aListOfObjects
    '                Dim lLinkId As Object = lLinkIDField.GetValue(lObject)
    '                For lSearchListIndex As Integer = 0 To aListOfObjects.Count - 1
    '                    Dim lSearchObject As Object = aListOfObjects.Item(lSearchListIndex)
    '                    If lIDField.GetValue(lSearchObject) = lLinkId Then
    '                        lLinkField.SetValue(lObject, lSearchObject)
    '                        lNumLinked += 1
    '                        Exit For
    '                    End If
    '                Next
    '                'lField.SetValue(aObject, CBool(lValue))
    '            Next
    '            Logger.Dbg("ConnectObjects linked " & lNumLinked & " of " & aListOfObjects.Count)
    '        End If
    '    End If
    'End Sub

End Module
