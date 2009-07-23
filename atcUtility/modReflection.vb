Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports System.Reflection
Imports MapWinUtility

Public Module modReflection

    Private pDontNeedTest As ArrayList

    'Private Function GetEmbeddedFileAsStream(ByVal fileName As String, ByVal aAssembly As Assembly) As IO.Stream
    '    Return aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName)
    'End Function

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

    Public Function GetEmbeddedFileAsBitmap(ByVal fileName As String, Optional ByVal aAssembly As Assembly = Nothing) As Drawing.Bitmap
        If aAssembly Is Nothing Then aAssembly = Assembly.GetCallingAssembly
        Dim s As IO.Stream = aAssembly.GetManifestResourceStream(aAssembly.GetName().Name + "." + fileName)
        Dim lBitmap As New Drawing.Bitmap(s)
        s.Close()
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
            pDontNeedTest.Add("<none>")
            pDontNeedTest.Add("GetHashCode")
            pDontNeedTest.Add("GetType")
            pDontNeedTest.Add("set_AutoScaleBaseSize")
            pDontNeedTest.Add("get_AutoScaleBaseSize")
            pDontNeedTest.Add("set_ActiveControl")
            pDontNeedTest.Add("get_ActiveControl")
            pDontNeedTest.Add("set_AutoScroll")
            pDontNeedTest.Add("get_AutoScroll")
            pDontNeedTest.Add("get_Handle")
            pDontNeedTest.Add("Invoke")
            pDontNeedTest.Add("EndInvoke")
            pDontNeedTest.Add("BeginInvoke")
            pDontNeedTest.Add("get_InvokeRequired")
            pDontNeedTest.Add("ResetText")
            pDontNeedTest.Add("Refresh")
            pDontNeedTest.Add("ResetRightToLeft")
            pDontNeedTest.Add("ResetForeColor")
            pDontNeedTest.Add("ResetFont")
            pDontNeedTest.Add("ResetCursor")
            pDontNeedTest.Add("ResetBackColor")
            pDontNeedTest.Add("PreProcessMessage")
            pDontNeedTest.Add("set_Text")
            pDontNeedTest.Add("get_Text")
            pDontNeedTest.Add("set_RightToLeft")
            pDontNeedTest.Add("get_RightToLeft")
            pDontNeedTest.Add("set_ForeColor")
            pDontNeedTest.Add("get_ForeColor")
            pDontNeedTest.Add("set_Font")
            pDontNeedTest.Add("get_Font")
            pDontNeedTest.Add("get_Focused")
            pDontNeedTest.Add("set_Dock")
            pDontNeedTest.Add("get_Dock")
            pDontNeedTest.Add("get_DisplayRectangle")
            pDontNeedTest.Add("set_Cursor")
            pDontNeedTest.Add("get_Cursor")
            pDontNeedTest.Add("set_ContextMenu")
            pDontNeedTest.Add("get_ContextMenu")
            pDontNeedTest.Add("set_BindingContext")
            pDontNeedTest.Add("get_BindingContext")
            pDontNeedTest.Add("set_BackgroundImage")
            pDontNeedTest.Add("get_BackgroundImage")
            pDontNeedTest.Add("set_BackColor")
            pDontNeedTest.Add("get_BackColor")
            pDontNeedTest.Add("set_Anchor")
            pDontNeedTest.Add("get_Anchor")
            pDontNeedTest.Add("set_AllowDrop")
            pDontNeedTest.Add("get_AllowDrop")
            pDontNeedTest.Add("Dispose")
            pDontNeedTest.Add("remove_Disposed")
            pDontNeedTest.Add("add_Disposed")
            pDontNeedTest.Add("set_Site")
            pDontNeedTest.Add("get_Site")
            pDontNeedTest.Add("CreateObjRef")
            pDontNeedTest.Add("InitializeLifetimeService")
            pDontNeedTest.Add("GetLifetimeService")
            pDontNeedTest.Add("get_AcceptButton")
            pDontNeedTest.Add("set_AcceptButton")
            pDontNeedTest.Add("get_ActiveMdiChild")
            pDontNeedTest.Add("get_AllowTransparency")
            pDontNeedTest.Add("set_AllowTransparency")
            pDontNeedTest.Add("get_AutoScale")
            pDontNeedTest.Add("set_AutoScale")
            pDontNeedTest.Add("get_FormBorderStyle")
            pDontNeedTest.Add("set_FormBorderStyle")
            pDontNeedTest.Add("get_CancelButton")
            pDontNeedTest.Add("set_CancelButton")
            pDontNeedTest.Add("get_ClientSize")
            pDontNeedTest.Add("set_ClientSize")
            pDontNeedTest.Add("get_ControlBox")
            pDontNeedTest.Add("set_ControlBox")
            pDontNeedTest.Add("get_DesktopBounds")
            pDontNeedTest.Add("set_DesktopBounds")
            pDontNeedTest.Add("get_DesktopLocation")
            pDontNeedTest.Add("set_DesktopLocation")
            pDontNeedTest.Add("get_DialogResult")
            pDontNeedTest.Add("set_DialogResult")
            pDontNeedTest.Add("get_HelpButton")
            pDontNeedTest.Add("set_HelpButton")
            pDontNeedTest.Add("get_Icon")
            pDontNeedTest.Add("set_Icon")
            pDontNeedTest.Add("get_IsMdiChild")
            pDontNeedTest.Add("get_IsMdiContainer")
            pDontNeedTest.Add("set_IsMdiContainer")
            pDontNeedTest.Add("get_KeyPreview")
            pDontNeedTest.Add("set_KeyPreview")
            pDontNeedTest.Add("add_MaximizedBoundsChanged")
            pDontNeedTest.Add("remove_MaximizedBoundsChanged")
            pDontNeedTest.Add("get_MaximumSize")
            pDontNeedTest.Add("set_MaximumSize")
            pDontNeedTest.Add("add_MaximumSizeChanged")
            pDontNeedTest.Add("remove_MaximumSizeChanged")
            pDontNeedTest.Add("get_Menu")
            pDontNeedTest.Add("set_Menu")
            pDontNeedTest.Add("get_MinimumSize")
            pDontNeedTest.Add("set_MinimumSize")
            pDontNeedTest.Add("add_MinimumSizeChanged")
            pDontNeedTest.Add("remove_MinimumSizeChanged")
            pDontNeedTest.Add("get_MaximizeBox")
            pDontNeedTest.Add("set_MaximizeBox")
            pDontNeedTest.Add("get_MdiChildren")
            pDontNeedTest.Add("get_MdiParent")
            pDontNeedTest.Add("set_MdiParent")
            pDontNeedTest.Add("get_MergedMenu")
            pDontNeedTest.Add("get_MinimizeBox")
            pDontNeedTest.Add("set_MinimizeBox")
            pDontNeedTest.Add("get_Modal")
            pDontNeedTest.Add("get_Opacity")
            pDontNeedTest.Add("set_Opacity")
            pDontNeedTest.Add("get_OwnedForms")
            pDontNeedTest.Add("get_Owner")
            pDontNeedTest.Add("set_Owner")
            pDontNeedTest.Add("get_ShowInTaskbar")
            pDontNeedTest.Add("set_ShowInTaskbar")
            pDontNeedTest.Add("get_Size")
            pDontNeedTest.Add("set_Size")
            pDontNeedTest.Add("get_SizeGripStyle")
            pDontNeedTest.Add("set_SizeGripStyle")
            pDontNeedTest.Add("get_StartPosition")
            pDontNeedTest.Add("set_StartPosition")
            pDontNeedTest.Add("get_TabIndex")
            pDontNeedTest.Add("set_TabIndex")
            pDontNeedTest.Add("add_TabIndexChanged")
            pDontNeedTest.Add("remove_TabIndexChanged")
            pDontNeedTest.Add("get_TopLevel")
            pDontNeedTest.Add("set_TopLevel")
            pDontNeedTest.Add("get_TopMost")
            pDontNeedTest.Add("set_TopMost")
            pDontNeedTest.Add("get_TransparencyKey")
            pDontNeedTest.Add("set_TransparencyKey")
            pDontNeedTest.Add("get_IsRestrictedWindow")
            pDontNeedTest.Add("get_WindowState")
            pDontNeedTest.Add("set_WindowState")
            pDontNeedTest.Add("Activate")
            pDontNeedTest.Add("add_Activated")
            pDontNeedTest.Add("remove_Activated")
            pDontNeedTest.Add("add_Closing")
            pDontNeedTest.Add("remove_Closing")
            pDontNeedTest.Add("add_Closed")
            pDontNeedTest.Add("remove_Closed")
            pDontNeedTest.Add("add_Deactivate")
            pDontNeedTest.Add("remove_Deactivate")
            pDontNeedTest.Add("add_Load")
            pDontNeedTest.Add("remove_Load")
            pDontNeedTest.Add("add_MdiChildActivate")
            pDontNeedTest.Add("remove_MdiChildActivate")
            pDontNeedTest.Add("add_MenuComplete")
            pDontNeedTest.Add("remove_MenuComplete")
            pDontNeedTest.Add("add_MenuStart")
            pDontNeedTest.Add("remove_MenuStart")
            pDontNeedTest.Add("add_InputLanguageChanged")
            pDontNeedTest.Add("remove_InputLanguageChanged")
            pDontNeedTest.Add("add_InputLanguageChanging")
            pDontNeedTest.Add("remove_InputLanguageChanging")
            pDontNeedTest.Add("AddOwnedForm")
            pDontNeedTest.Add("Close")
            pDontNeedTest.Add("LayoutMdi")
            pDontNeedTest.Add("RemoveOwnedForm")
            pDontNeedTest.Add("SetDesktopBounds")
            pDontNeedTest.Add("SetDesktopLocation")
            pDontNeedTest.Add("ShowDialog")
            pDontNeedTest.Add("get_ParentForm")
            pDontNeedTest.Add("Validate")
            pDontNeedTest.Add("get_AutoScrollMargin")
            pDontNeedTest.Add("set_AutoScrollMargin")
            pDontNeedTest.Add("get_AutoScrollPosition")
            pDontNeedTest.Add("set_AutoScrollPosition")
            pDontNeedTest.Add("get_AutoScrollMinSize")
            pDontNeedTest.Add("set_AutoScrollMinSize")
            pDontNeedTest.Add("get_DockPadding")
            pDontNeedTest.Add("ScrollControlIntoView")
            pDontNeedTest.Add("SetAutoScrollMargin")
            pDontNeedTest.Add("get_AccessibilityObject")
            pDontNeedTest.Add("get_AccessibleDefaultActionDescription")
            pDontNeedTest.Add("set_AccessibleDefaultActionDescription")
            pDontNeedTest.Add("get_AccessibleDescription")
            pDontNeedTest.Add("set_AccessibleDescription")
            pDontNeedTest.Add("get_AccessibleName")
            pDontNeedTest.Add("set_AccessibleName")
            pDontNeedTest.Add("get_AccessibleRole")
            pDontNeedTest.Add("set_AccessibleRole")
            pDontNeedTest.Add("add_BackColorChanged")
            pDontNeedTest.Add("remove_BackColorChanged")
            pDontNeedTest.Add("add_BackgroundImageChanged")
            pDontNeedTest.Add("remove_BackgroundImageChanged")
            pDontNeedTest.Add("get_DataBindings")
            pDontNeedTest.Add("ResetBindings")
            pDontNeedTest.Add("add_BindingContextChanged")
            pDontNeedTest.Add("remove_BindingContextChanged")
            pDontNeedTest.Add("get_Bottom")
            pDontNeedTest.Add("get_Bounds")
            pDontNeedTest.Add("set_Bounds")
            pDontNeedTest.Add("get_CanFocus")
            pDontNeedTest.Add("get_CanSelect")
            pDontNeedTest.Add("get_Capture")
            pDontNeedTest.Add("set_Capture")
            pDontNeedTest.Add("get_CausesValidation")
            pDontNeedTest.Add("set_CausesValidation")
            pDontNeedTest.Add("add_CausesValidationChanged")
            pDontNeedTest.Add("remove_CausesValidationChanged")
            pDontNeedTest.Add("get_ClientRectangle")
            pDontNeedTest.Add("get_CompanyName")
            pDontNeedTest.Add("get_ContainsFocus")
            pDontNeedTest.Add("add_ContextMenuChanged")
            pDontNeedTest.Add("remove_ContextMenuChanged")
            pDontNeedTest.Add("get_Controls")
            pDontNeedTest.Add("get_Created")
            pDontNeedTest.Add("add_CursorChanged")
            pDontNeedTest.Add("remove_CursorChanged")
            pDontNeedTest.Add("get_IsDisposed")
            pDontNeedTest.Add("get_Disposing")
            pDontNeedTest.Add("add_DockChanged")
            pDontNeedTest.Add("remove_DockChanged")
            pDontNeedTest.Add("get_Enabled")
            pDontNeedTest.Add("set_Enabled")
            pDontNeedTest.Add("add_EnabledChanged")
            pDontNeedTest.Add("remove_EnabledChanged")
            pDontNeedTest.Add("add_FontChanged")
            pDontNeedTest.Add("remove_FontChanged")
            pDontNeedTest.Add("add_ForeColorChanged")
            pDontNeedTest.Add("remove_ForeColorChanged")
            pDontNeedTest.Add("get_HasChildren")
            pDontNeedTest.Add("get_Height")
            pDontNeedTest.Add("set_Height")
            pDontNeedTest.Add("get_IsHandleCreated")
            pDontNeedTest.Add("get_ImeMode")
            pDontNeedTest.Add("set_ImeMode")
            pDontNeedTest.Add("get_IsAccessible")
            pDontNeedTest.Add("set_IsAccessible")
            pDontNeedTest.Add("get_Left")
            pDontNeedTest.Add("set_Left")
            pDontNeedTest.Add("get_Location")
            pDontNeedTest.Add("set_Location")
            pDontNeedTest.Add("add_LocationChanged")
            pDontNeedTest.Add("remove_LocationChanged")
            pDontNeedTest.Add("get_Name")
            pDontNeedTest.Add("set_Name")
            pDontNeedTest.Add("get_Parent")
            pDontNeedTest.Add("set_Parent")
            pDontNeedTest.Add("get_ProductName")
            pDontNeedTest.Add("get_ProductVersion")
            pDontNeedTest.Add("get_RecreatingHandle")
            pDontNeedTest.Add("get_Region")
            pDontNeedTest.Add("set_Region")
            pDontNeedTest.Add("get_Right")
            pDontNeedTest.Add("add_RightToLeftChanged")
            pDontNeedTest.Add("remove_RightToLeftChanged")
            pDontNeedTest.Add("add_SizeChanged")
            pDontNeedTest.Add("remove_SizeChanged")
            pDontNeedTest.Add("get_TabStop")
            pDontNeedTest.Add("set_TabStop")
            pDontNeedTest.Add("add_TabStopChanged")
            pDontNeedTest.Add("remove_TabStopChanged")
            pDontNeedTest.Add("get_Tag")
            pDontNeedTest.Add("set_Tag")
            pDontNeedTest.Add("add_TextChanged")
            pDontNeedTest.Add("remove_TextChanged")
            pDontNeedTest.Add("get_Top")
            pDontNeedTest.Add("set_Top")
            pDontNeedTest.Add("get_TopLevelControl")
            pDontNeedTest.Add("get_Visible")
            pDontNeedTest.Add("set_Visible")
            pDontNeedTest.Add("add_VisibleChanged")
            pDontNeedTest.Add("remove_VisibleChanged")
            pDontNeedTest.Add("get_Width")
            pDontNeedTest.Add("set_Width")
            pDontNeedTest.Add("get_WindowTarget")
            pDontNeedTest.Add("set_WindowTarget")
            pDontNeedTest.Add("add_Click")
            pDontNeedTest.Add("remove_Click")
            pDontNeedTest.Add("add_ControlAdded")
            pDontNeedTest.Add("remove_ControlAdded")
            pDontNeedTest.Add("add_ControlRemoved")
            pDontNeedTest.Add("remove_ControlRemoved")
            pDontNeedTest.Add("add_DragDrop")
            pDontNeedTest.Add("remove_DragDrop")
            pDontNeedTest.Add("add_DragEnter")
            pDontNeedTest.Add("remove_DragEnter")
            pDontNeedTest.Add("add_DragOver")
            pDontNeedTest.Add("remove_DragOver")
            pDontNeedTest.Add("add_DragLeave")
            pDontNeedTest.Add("remove_DragLeave")
            pDontNeedTest.Add("add_GiveFeedback")
            pDontNeedTest.Add("remove_GiveFeedback")
            pDontNeedTest.Add("add_HandleCreated")
            pDontNeedTest.Add("remove_HandleCreated")
            pDontNeedTest.Add("add_HandleDestroyed")
            pDontNeedTest.Add("remove_HandleDestroyed")
            pDontNeedTest.Add("add_HelpRequested")
            pDontNeedTest.Add("remove_HelpRequested")
            pDontNeedTest.Add("add_Invalidated")
            pDontNeedTest.Add("remove_Invalidated")
            pDontNeedTest.Add("add_Paint")
            pDontNeedTest.Add("remove_Paint")
            pDontNeedTest.Add("add_QueryContinueDrag")
            pDontNeedTest.Add("remove_QueryContinueDrag")
            pDontNeedTest.Add("add_QueryAccessibilityHelp")
            pDontNeedTest.Add("remove_QueryAccessibilityHelp")
            pDontNeedTest.Add("add_DoubleClick")
            pDontNeedTest.Add("remove_DoubleClick")
            pDontNeedTest.Add("add_Enter")
            pDontNeedTest.Add("remove_Enter")
            pDontNeedTest.Add("add_GotFocus")
            pDontNeedTest.Add("remove_GotFocus")
            pDontNeedTest.Add("add_ImeModeChanged")
            pDontNeedTest.Add("remove_ImeModeChanged")
            pDontNeedTest.Add("add_KeyDown")
            pDontNeedTest.Add("remove_KeyDown")
            pDontNeedTest.Add("add_KeyPress")
            pDontNeedTest.Add("remove_KeyPress")
            pDontNeedTest.Add("add_KeyUp")
            pDontNeedTest.Add("remove_KeyUp")
            pDontNeedTest.Add("add_Layout")
            pDontNeedTest.Add("remove_Layout")
            pDontNeedTest.Add("add_Leave")
            pDontNeedTest.Add("remove_Leave")
            pDontNeedTest.Add("add_LostFocus")
            pDontNeedTest.Add("remove_LostFocus")
            pDontNeedTest.Add("add_MouseDown")
            pDontNeedTest.Add("remove_MouseDown")
            pDontNeedTest.Add("add_MouseEnter")
            pDontNeedTest.Add("remove_MouseEnter")
            pDontNeedTest.Add("add_MouseLeave")
            pDontNeedTest.Add("remove_MouseLeave")
            pDontNeedTest.Add("add_MouseHover")
            pDontNeedTest.Add("remove_MouseHover")
            pDontNeedTest.Add("add_MouseMove")
            pDontNeedTest.Add("remove_MouseMove")
            pDontNeedTest.Add("add_MouseUp")
            pDontNeedTest.Add("remove_MouseUp")
            pDontNeedTest.Add("add_MouseWheel")
            pDontNeedTest.Add("remove_MouseWheel")
            pDontNeedTest.Add("add_Move")
            pDontNeedTest.Add("remove_Move")
            pDontNeedTest.Add("add_Resize")
            pDontNeedTest.Add("remove_Resize")
            pDontNeedTest.Add("add_ChangeUICues")
            pDontNeedTest.Add("remove_ChangeUICues")
            pDontNeedTest.Add("add_StyleChanged")
            pDontNeedTest.Add("remove_StyleChanged")
            pDontNeedTest.Add("add_SystemColorsChanged")
            pDontNeedTest.Add("remove_SystemColorsChanged")
            pDontNeedTest.Add("add_Validating")
            pDontNeedTest.Add("remove_Validating")
            pDontNeedTest.Add("add_Validated")
            pDontNeedTest.Add("remove_Validated")
            pDontNeedTest.Add("add_ParentChanged")
            pDontNeedTest.Add("remove_ParentChanged")
            pDontNeedTest.Add("BringToFront")
            pDontNeedTest.Add("Contains")
            pDontNeedTest.Add("CreateGraphics")
            pDontNeedTest.Add("CreateControl")
            pDontNeedTest.Add("DoDragDrop")
            pDontNeedTest.Add("FindForm")
            pDontNeedTest.Add("Focus")
            pDontNeedTest.Add("GetChildAtPoint")
            pDontNeedTest.Add("GetContainerControl")
            pDontNeedTest.Add("GetNextControl")
            pDontNeedTest.Add("Hide")
            pDontNeedTest.Add("Invalidate")
            pDontNeedTest.Add("PerformLayout")
            pDontNeedTest.Add("PointToClient")
            pDontNeedTest.Add("PointToScreen")
            pDontNeedTest.Add("ResetImeMode")
            pDontNeedTest.Add("RectangleToClient")
            pDontNeedTest.Add("RectangleToScreen")
            pDontNeedTest.Add("ResumeLayout")
            pDontNeedTest.Add("Scale")
            pDontNeedTest.Add("Select")
            pDontNeedTest.Add("SelectNextControl")
            pDontNeedTest.Add("SendToBack")
            pDontNeedTest.Add("SetBounds")
            pDontNeedTest.Add("Show")
            pDontNeedTest.Add("SuspendLayout")
            pDontNeedTest.Add("Update")
            pDontNeedTest.Add("get_Container")
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

    Public Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20) & _
                    " Local(MB):" & System.GC.GetTotalMemory(True) / (2 ^ 20)
    End Function

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
