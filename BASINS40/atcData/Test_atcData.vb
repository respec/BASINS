Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports ATCUtility.modReflection

<TestFixture()> Public Class Test_Builder
  Public Sub TestsAllPresent()
    Dim lTestBuildStatus as String
    lTestBuildStatus = BuildMissingTests("c:\test\")
    Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
  End Sub
End Class

<TestFixture()> Public Class Test_modTimeseriesMath

  Public Sub TestSubsetByDate()
    'SubsetByDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyBaseAttributes()
    'CopyBaseAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmSelectData

  Public Sub Testset_AutoScaleBaseSize()
    'set_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScaleBaseSize()
    'get_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ActiveControl()
    'set_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveControl()
    'get_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScroll()
    'set_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScroll()
    'get_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Handle()
    'get_Handle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_InvokeRequired()
    'get_InvokeRequired()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetText()
    'ResetText()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRefresh()
    'Refresh()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetRightToLeft()
    'ResetRightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetForeColor()
    'ResetForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetFont()
    'ResetFont()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetCursor()
    'ResetCursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBackColor()
    'ResetBackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPreProcessMessage()
    'PreProcessMessage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Text()
    'set_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Text()
    'get_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_RightToLeft()
    'set_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RightToLeft()
    'get_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ForeColor()
    'set_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ForeColor()
    'get_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Font()
    'set_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Font()
    'get_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Focused()
    'get_Focused()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Dock()
    'set_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Dock()
    'get_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DisplayRectangle()
    'get_DisplayRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Cursor()
    'set_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Cursor()
    'get_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ContextMenu()
    'set_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContextMenu()
    'get_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BindingContext()
    'set_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BindingContext()
    'get_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackgroundImage()
    'set_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackgroundImage()
    'get_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackColor()
    'set_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackColor()
    'get_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Anchor()
    'set_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Anchor()
    'get_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowDrop()
    'set_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowDrop()
    'get_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDispose()
    'Dispose()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Disposed()
    'remove_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Disposed()
    'add_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Site()
    'set_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Site()
    'get_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateObjRef()
    'CreateObjRef()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitializeLifetimeService()
    'InitializeLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetLifetimeService()
    'GetLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAskUser()
    'AskUser()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AcceptButton()
    'get_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AcceptButton()
    'set_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveMdiChild()
    'get_ActiveMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowTransparency()
    'get_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowTransparency()
    'set_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScale()
    'get_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScale()
    'set_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FormBorderStyle()
    'get_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FormBorderStyle()
    'set_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CancelButton()
    'get_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CancelButton()
    'set_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientSize()
    'get_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ClientSize()
    'set_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ControlBox()
    'get_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ControlBox()
    'set_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopBounds()
    'get_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopBounds()
    'set_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopLocation()
    'get_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopLocation()
    'set_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DialogResult()
    'get_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DialogResult()
    'set_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HelpButton()
    'get_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_HelpButton()
    'set_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Icon()
    'get_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Icon()
    'set_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiChild()
    'get_IsMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiContainer()
    'get_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsMdiContainer()
    'set_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_KeyPreview()
    'get_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_KeyPreview()
    'set_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximizedBoundsChanged()
    'add_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximizedBoundsChanged()
    'remove_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximumSize()
    'get_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximumSize()
    'set_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximumSizeChanged()
    'add_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximumSizeChanged()
    'remove_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Menu()
    'get_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Menu()
    'set_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimumSize()
    'get_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimumSize()
    'set_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MinimumSizeChanged()
    'add_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MinimumSizeChanged()
    'remove_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximizeBox()
    'get_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximizeBox()
    'set_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiChildren()
    'get_MdiChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiParent()
    'get_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MdiParent()
    'set_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MergedMenu()
    'get_MergedMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimizeBox()
    'get_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimizeBox()
    'set_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Modal()
    'get_Modal()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Opacity()
    'get_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Opacity()
    'set_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_OwnedForms()
    'get_OwnedForms()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Owner()
    'get_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Owner()
    'set_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ShowInTaskbar()
    'get_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ShowInTaskbar()
    'set_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Size()
    'get_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Size()
    'set_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SizeGripStyle()
    'get_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_SizeGripStyle()
    'set_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_StartPosition()
    'get_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_StartPosition()
    'set_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabIndex()
    'get_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabIndex()
    'set_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabIndexChanged()
    'add_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabIndexChanged()
    'remove_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevel()
    'get_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopLevel()
    'set_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopMost()
    'get_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopMost()
    'set_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TransparencyKey()
    'get_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TransparencyKey()
    'set_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsRestrictedWindow()
    'get_IsRestrictedWindow()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowState()
    'get_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowState()
    'set_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestActivate()
    'Activate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Activated()
    'add_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Activated()
    'remove_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closing()
    'add_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closing()
    'remove_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closed()
    'add_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closed()
    'remove_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Deactivate()
    'add_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Deactivate()
    'remove_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Load()
    'add_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Load()
    'remove_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MdiChildActivate()
    'add_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MdiChildActivate()
    'remove_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuComplete()
    'add_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuComplete()
    'remove_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuStart()
    'add_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuStart()
    'remove_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanged()
    'add_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanged()
    'remove_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanging()
    'add_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanging()
    'remove_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddOwnedForm()
    'AddOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClose()
    'Close()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayoutMdi()
    'LayoutMdi()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveOwnedForm()
    'RemoveOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopBounds()
    'SetDesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopLocation()
    'SetDesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShowDialog()
    'ShowDialog()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ParentForm()
    'get_ParentForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestValidate()
    'Validate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMargin()
    'get_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMargin()
    'set_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollPosition()
    'get_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollPosition()
    'set_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMinSize()
    'get_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMinSize()
    'set_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DockPadding()
    'get_DockPadding()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScrollControlIntoView()
    'ScrollControlIntoView()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetAutoScrollMargin()
    'SetAutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibilityObject()
    'get_AccessibilityObject()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDefaultActionDescription()
    'get_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDefaultActionDescription()
    'set_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDescription()
    'get_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDescription()
    'set_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleName()
    'get_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleName()
    'set_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleRole()
    'get_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleRole()
    'set_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackColorChanged()
    'add_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackColorChanged()
    'remove_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackgroundImageChanged()
    'add_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackgroundImageChanged()
    'remove_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataBindings()
    'get_DataBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBindings()
    'ResetBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BindingContextChanged()
    'add_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BindingContextChanged()
    'remove_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bottom()
    'get_Bottom()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bounds()
    'get_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Bounds()
    'set_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanFocus()
    'get_CanFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanSelect()
    'get_CanSelect()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capture()
    'get_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capture()
    'set_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CausesValidation()
    'get_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CausesValidation()
    'set_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CausesValidationChanged()
    'add_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CausesValidationChanged()
    'remove_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientRectangle()
    'get_ClientRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CompanyName()
    'get_CompanyName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContainsFocus()
    'get_ContainsFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ContextMenuChanged()
    'add_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ContextMenuChanged()
    'remove_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Controls()
    'get_Controls()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Created()
    'get_Created()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CursorChanged()
    'add_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CursorChanged()
    'remove_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsDisposed()
    'get_IsDisposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Disposing()
    'get_Disposing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DockChanged()
    'add_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DockChanged()
    'remove_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Enabled()
    'get_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Enabled()
    'set_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_EnabledChanged()
    'add_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_EnabledChanged()
    'remove_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_FontChanged()
    'add_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_FontChanged()
    'remove_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ForeColorChanged()
    'add_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ForeColorChanged()
    'remove_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HasChildren()
    'get_HasChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Height()
    'get_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Height()
    'set_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsHandleCreated()
    'get_IsHandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ImeMode()
    'get_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ImeMode()
    'set_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsAccessible()
    'get_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsAccessible()
    'set_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Left()
    'get_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Left()
    'set_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Location()
    'get_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Location()
    'set_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LocationChanged()
    'add_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LocationChanged()
    'remove_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Name()
    'set_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Parent()
    'get_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Parent()
    'set_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductName()
    'get_ProductName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductVersion()
    'get_ProductVersion()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RecreatingHandle()
    'get_RecreatingHandle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Region()
    'get_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Region()
    'set_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Right()
    'get_Right()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_RightToLeftChanged()
    'add_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_RightToLeftChanged()
    'remove_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SizeChanged()
    'add_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SizeChanged()
    'remove_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabStop()
    'get_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabStop()
    'set_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabStopChanged()
    'add_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabStopChanged()
    'remove_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Tag()
    'get_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Tag()
    'set_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TextChanged()
    'add_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TextChanged()
    'remove_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Top()
    'get_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Top()
    'set_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevelControl()
    'get_TopLevelControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Visible()
    'get_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Visible()
    'set_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_VisibleChanged()
    'add_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_VisibleChanged()
    'remove_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Width()
    'get_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Width()
    'set_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowTarget()
    'get_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowTarget()
    'set_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Click()
    'add_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Click()
    'remove_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlAdded()
    'add_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlAdded()
    'remove_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlRemoved()
    'add_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlRemoved()
    'remove_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragDrop()
    'add_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragDrop()
    'remove_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragEnter()
    'add_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragEnter()
    'remove_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragOver()
    'add_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragOver()
    'remove_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragLeave()
    'add_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragLeave()
    'remove_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GiveFeedback()
    'add_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GiveFeedback()
    'remove_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleCreated()
    'add_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleCreated()
    'remove_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleDestroyed()
    'add_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleDestroyed()
    'remove_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HelpRequested()
    'add_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HelpRequested()
    'remove_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Invalidated()
    'add_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Invalidated()
    'remove_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Paint()
    'add_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Paint()
    'remove_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryContinueDrag()
    'add_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryContinueDrag()
    'remove_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryAccessibilityHelp()
    'add_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryAccessibilityHelp()
    'remove_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DoubleClick()
    'add_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DoubleClick()
    'remove_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Enter()
    'add_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Enter()
    'remove_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GotFocus()
    'add_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GotFocus()
    'remove_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ImeModeChanged()
    'add_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ImeModeChanged()
    'remove_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyDown()
    'add_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyDown()
    'remove_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyPress()
    'add_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyPress()
    'remove_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyUp()
    'add_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyUp()
    'remove_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Layout()
    'add_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Layout()
    'remove_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Leave()
    'add_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Leave()
    'remove_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LostFocus()
    'add_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LostFocus()
    'remove_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseDown()
    'add_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseDown()
    'remove_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseEnter()
    'add_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseEnter()
    'remove_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseLeave()
    'add_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseLeave()
    'remove_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseHover()
    'add_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseHover()
    'remove_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseMove()
    'add_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseMove()
    'remove_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseUp()
    'add_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseUp()
    'remove_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseWheel()
    'add_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseWheel()
    'remove_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Move()
    'add_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Move()
    'remove_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Resize()
    'add_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Resize()
    'remove_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangeUICues()
    'add_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangeUICues()
    'remove_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_StyleChanged()
    'add_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_StyleChanged()
    'remove_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SystemColorsChanged()
    'add_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SystemColorsChanged()
    'remove_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validating()
    'add_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validating()
    'remove_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validated()
    'add_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validated()
    'remove_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ParentChanged()
    'add_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ParentChanged()
    'remove_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBringToFront()
    'BringToFront()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateGraphics()
    'CreateGraphics()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateControl()
    'CreateControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDoDragDrop()
    'DoDragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindForm()
    'FindForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFocus()
    'Focus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetChildAtPoint()
    'GetChildAtPoint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetContainerControl()
    'GetContainerControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetNextControl()
    'GetNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestHide()
    'Hide()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvalidate()
    'Invalidate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPerformLayout()
    'PerformLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToClient()
    'PointToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToScreen()
    'PointToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetImeMode()
    'ResetImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToClient()
    'RectangleToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToScreen()
    'RectangleToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResumeLayout()
    'ResumeLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScale()
    'Scale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelect()
    'Select()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelectNextControl()
    'SelectNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSendToBack()
    'SendToBack()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetBounds()
    'SetBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShow()
    'Show()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSuspendLayout()
    'SuspendLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUpdate()
    'Update()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Container()
    'get_Container()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_GridSource

  Public Sub Testset_Alignment()
    'set_Alignment()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Alignment()
    'get_Alignment()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Columns()
    'set_Columns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Columns()
    'get_Columns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Rows()
    'set_Rows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Rows()
    'get_Rows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CellValue()
    'set_CellValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CellValue()
    'get_CellValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangedRows()
    'add_ChangedRows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedRows()
    'remove_ChangedRows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedColumns()
    'remove_ChangedColumns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangedColumns()
    'add_ChangedColumns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangedValue()
    'add_ChangedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedValue()
    'remove_ChangedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmManager

  Public Sub Testset_AutoScaleBaseSize()
    'set_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScaleBaseSize()
    'get_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ActiveControl()
    'set_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveControl()
    'get_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScroll()
    'set_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScroll()
    'get_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Handle()
    'get_Handle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_InvokeRequired()
    'get_InvokeRequired()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetText()
    'ResetText()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRefresh()
    'Refresh()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetRightToLeft()
    'ResetRightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetForeColor()
    'ResetForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetFont()
    'ResetFont()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetCursor()
    'ResetCursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBackColor()
    'ResetBackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPreProcessMessage()
    'PreProcessMessage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Text()
    'set_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Text()
    'get_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_RightToLeft()
    'set_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RightToLeft()
    'get_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ForeColor()
    'set_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ForeColor()
    'get_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Font()
    'set_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Font()
    'get_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Focused()
    'get_Focused()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Dock()
    'set_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Dock()
    'get_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DisplayRectangle()
    'get_DisplayRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Cursor()
    'set_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Cursor()
    'get_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ContextMenu()
    'set_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContextMenu()
    'get_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BindingContext()
    'set_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BindingContext()
    'get_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackgroundImage()
    'set_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackgroundImage()
    'get_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackColor()
    'set_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackColor()
    'get_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Anchor()
    'set_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Anchor()
    'get_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowDrop()
    'set_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowDrop()
    'get_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDispose()
    'Dispose()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Disposed()
    'remove_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Disposed()
    'add_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Site()
    'set_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Site()
    'get_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateObjRef()
    'CreateObjRef()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitializeLifetimeService()
    'InitializeLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetLifetimeService()
    'GetLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEdit()
    'Edit()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AcceptButton()
    'get_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AcceptButton()
    'set_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveMdiChild()
    'get_ActiveMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowTransparency()
    'get_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowTransparency()
    'set_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScale()
    'get_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScale()
    'set_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FormBorderStyle()
    'get_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FormBorderStyle()
    'set_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CancelButton()
    'get_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CancelButton()
    'set_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientSize()
    'get_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ClientSize()
    'set_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ControlBox()
    'get_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ControlBox()
    'set_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopBounds()
    'get_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopBounds()
    'set_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopLocation()
    'get_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopLocation()
    'set_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DialogResult()
    'get_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DialogResult()
    'set_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HelpButton()
    'get_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_HelpButton()
    'set_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Icon()
    'get_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Icon()
    'set_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiChild()
    'get_IsMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiContainer()
    'get_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsMdiContainer()
    'set_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_KeyPreview()
    'get_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_KeyPreview()
    'set_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximizedBoundsChanged()
    'add_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximizedBoundsChanged()
    'remove_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximumSize()
    'get_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximumSize()
    'set_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximumSizeChanged()
    'add_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximumSizeChanged()
    'remove_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Menu()
    'get_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Menu()
    'set_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimumSize()
    'get_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimumSize()
    'set_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MinimumSizeChanged()
    'add_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MinimumSizeChanged()
    'remove_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximizeBox()
    'get_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximizeBox()
    'set_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiChildren()
    'get_MdiChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiParent()
    'get_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MdiParent()
    'set_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MergedMenu()
    'get_MergedMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimizeBox()
    'get_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimizeBox()
    'set_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Modal()
    'get_Modal()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Opacity()
    'get_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Opacity()
    'set_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_OwnedForms()
    'get_OwnedForms()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Owner()
    'get_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Owner()
    'set_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ShowInTaskbar()
    'get_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ShowInTaskbar()
    'set_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Size()
    'get_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Size()
    'set_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SizeGripStyle()
    'get_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_SizeGripStyle()
    'set_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_StartPosition()
    'get_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_StartPosition()
    'set_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabIndex()
    'get_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabIndex()
    'set_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabIndexChanged()
    'add_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabIndexChanged()
    'remove_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevel()
    'get_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopLevel()
    'set_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopMost()
    'get_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopMost()
    'set_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TransparencyKey()
    'get_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TransparencyKey()
    'set_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsRestrictedWindow()
    'get_IsRestrictedWindow()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowState()
    'get_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowState()
    'set_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestActivate()
    'Activate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Activated()
    'add_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Activated()
    'remove_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closing()
    'add_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closing()
    'remove_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closed()
    'add_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closed()
    'remove_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Deactivate()
    'add_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Deactivate()
    'remove_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Load()
    'add_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Load()
    'remove_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MdiChildActivate()
    'add_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MdiChildActivate()
    'remove_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuComplete()
    'add_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuComplete()
    'remove_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuStart()
    'add_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuStart()
    'remove_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanged()
    'add_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanged()
    'remove_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanging()
    'add_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanging()
    'remove_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddOwnedForm()
    'AddOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClose()
    'Close()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayoutMdi()
    'LayoutMdi()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveOwnedForm()
    'RemoveOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopBounds()
    'SetDesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopLocation()
    'SetDesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShowDialog()
    'ShowDialog()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ParentForm()
    'get_ParentForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestValidate()
    'Validate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMargin()
    'get_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMargin()
    'set_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollPosition()
    'get_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollPosition()
    'set_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMinSize()
    'get_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMinSize()
    'set_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DockPadding()
    'get_DockPadding()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScrollControlIntoView()
    'ScrollControlIntoView()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetAutoScrollMargin()
    'SetAutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibilityObject()
    'get_AccessibilityObject()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDefaultActionDescription()
    'get_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDefaultActionDescription()
    'set_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDescription()
    'get_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDescription()
    'set_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleName()
    'get_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleName()
    'set_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleRole()
    'get_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleRole()
    'set_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackColorChanged()
    'add_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackColorChanged()
    'remove_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackgroundImageChanged()
    'add_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackgroundImageChanged()
    'remove_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataBindings()
    'get_DataBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBindings()
    'ResetBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BindingContextChanged()
    'add_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BindingContextChanged()
    'remove_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bottom()
    'get_Bottom()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bounds()
    'get_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Bounds()
    'set_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanFocus()
    'get_CanFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanSelect()
    'get_CanSelect()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capture()
    'get_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capture()
    'set_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CausesValidation()
    'get_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CausesValidation()
    'set_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CausesValidationChanged()
    'add_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CausesValidationChanged()
    'remove_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientRectangle()
    'get_ClientRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CompanyName()
    'get_CompanyName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContainsFocus()
    'get_ContainsFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ContextMenuChanged()
    'add_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ContextMenuChanged()
    'remove_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Controls()
    'get_Controls()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Created()
    'get_Created()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CursorChanged()
    'add_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CursorChanged()
    'remove_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsDisposed()
    'get_IsDisposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Disposing()
    'get_Disposing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DockChanged()
    'add_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DockChanged()
    'remove_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Enabled()
    'get_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Enabled()
    'set_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_EnabledChanged()
    'add_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_EnabledChanged()
    'remove_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_FontChanged()
    'add_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_FontChanged()
    'remove_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ForeColorChanged()
    'add_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ForeColorChanged()
    'remove_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HasChildren()
    'get_HasChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Height()
    'get_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Height()
    'set_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsHandleCreated()
    'get_IsHandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ImeMode()
    'get_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ImeMode()
    'set_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsAccessible()
    'get_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsAccessible()
    'set_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Left()
    'get_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Left()
    'set_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Location()
    'get_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Location()
    'set_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LocationChanged()
    'add_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LocationChanged()
    'remove_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Name()
    'set_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Parent()
    'get_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Parent()
    'set_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductName()
    'get_ProductName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductVersion()
    'get_ProductVersion()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RecreatingHandle()
    'get_RecreatingHandle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Region()
    'get_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Region()
    'set_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Right()
    'get_Right()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_RightToLeftChanged()
    'add_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_RightToLeftChanged()
    'remove_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SizeChanged()
    'add_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SizeChanged()
    'remove_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabStop()
    'get_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabStop()
    'set_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabStopChanged()
    'add_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabStopChanged()
    'remove_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Tag()
    'get_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Tag()
    'set_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TextChanged()
    'add_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TextChanged()
    'remove_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Top()
    'get_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Top()
    'set_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevelControl()
    'get_TopLevelControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Visible()
    'get_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Visible()
    'set_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_VisibleChanged()
    'add_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_VisibleChanged()
    'remove_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Width()
    'get_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Width()
    'set_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowTarget()
    'get_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowTarget()
    'set_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Click()
    'add_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Click()
    'remove_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlAdded()
    'add_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlAdded()
    'remove_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlRemoved()
    'add_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlRemoved()
    'remove_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragDrop()
    'add_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragDrop()
    'remove_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragEnter()
    'add_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragEnter()
    'remove_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragOver()
    'add_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragOver()
    'remove_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragLeave()
    'add_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragLeave()
    'remove_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GiveFeedback()
    'add_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GiveFeedback()
    'remove_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleCreated()
    'add_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleCreated()
    'remove_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleDestroyed()
    'add_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleDestroyed()
    'remove_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HelpRequested()
    'add_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HelpRequested()
    'remove_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Invalidated()
    'add_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Invalidated()
    'remove_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Paint()
    'add_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Paint()
    'remove_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryContinueDrag()
    'add_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryContinueDrag()
    'remove_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryAccessibilityHelp()
    'add_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryAccessibilityHelp()
    'remove_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DoubleClick()
    'add_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DoubleClick()
    'remove_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Enter()
    'add_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Enter()
    'remove_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GotFocus()
    'add_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GotFocus()
    'remove_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ImeModeChanged()
    'add_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ImeModeChanged()
    'remove_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyDown()
    'add_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyDown()
    'remove_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyPress()
    'add_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyPress()
    'remove_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyUp()
    'add_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyUp()
    'remove_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Layout()
    'add_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Layout()
    'remove_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Leave()
    'add_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Leave()
    'remove_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LostFocus()
    'add_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LostFocus()
    'remove_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseDown()
    'add_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseDown()
    'remove_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseEnter()
    'add_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseEnter()
    'remove_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseLeave()
    'add_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseLeave()
    'remove_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseHover()
    'add_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseHover()
    'remove_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseMove()
    'add_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseMove()
    'remove_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseUp()
    'add_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseUp()
    'remove_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseWheel()
    'add_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseWheel()
    'remove_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Move()
    'add_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Move()
    'remove_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Resize()
    'add_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Resize()
    'remove_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangeUICues()
    'add_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangeUICues()
    'remove_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_StyleChanged()
    'add_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_StyleChanged()
    'remove_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SystemColorsChanged()
    'add_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SystemColorsChanged()
    'remove_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validating()
    'add_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validating()
    'remove_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validated()
    'add_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validated()
    'remove_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ParentChanged()
    'add_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ParentChanged()
    'remove_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBringToFront()
    'BringToFront()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateGraphics()
    'CreateGraphics()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateControl()
    'CreateControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDoDragDrop()
    'DoDragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindForm()
    'FindForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFocus()
    'Focus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetChildAtPoint()
    'GetChildAtPoint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetContainerControl()
    'GetContainerControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetNextControl()
    'GetNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestHide()
    'Hide()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvalidate()
    'Invalidate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPerformLayout()
    'PerformLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToClient()
    'PointToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToScreen()
    'PointToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetImeMode()
    'ResetImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToClient()
    'RectangleToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToScreen()
    'RectangleToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResumeLayout()
    'ResumeLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScale()
    'Scale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelect()
    'Select()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelectNextControl()
    'SelectNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSendToBack()
    'SendToBack()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetBounds()
    'SetBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShow()
    'Show()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSuspendLayout()
    'SuspendLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUpdate()
    'Update()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Container()
    'get_Container()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmDataSource

  Public Sub Testset_AutoScaleBaseSize()
    'set_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScaleBaseSize()
    'get_AutoScaleBaseSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ActiveControl()
    'set_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveControl()
    'get_ActiveControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScroll()
    'set_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScroll()
    'get_AutoScroll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Handle()
    'get_Handle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_InvokeRequired()
    'get_InvokeRequired()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetText()
    'ResetText()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRefresh()
    'Refresh()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetRightToLeft()
    'ResetRightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetForeColor()
    'ResetForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetFont()
    'ResetFont()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetCursor()
    'ResetCursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBackColor()
    'ResetBackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPreProcessMessage()
    'PreProcessMessage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Text()
    'set_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Text()
    'get_Text()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_RightToLeft()
    'set_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RightToLeft()
    'get_RightToLeft()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ForeColor()
    'set_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ForeColor()
    'get_ForeColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Font()
    'set_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Font()
    'get_Font()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Focused()
    'get_Focused()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Dock()
    'set_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Dock()
    'get_Dock()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DisplayRectangle()
    'get_DisplayRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Cursor()
    'set_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Cursor()
    'get_Cursor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ContextMenu()
    'set_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContextMenu()
    'get_ContextMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BindingContext()
    'set_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BindingContext()
    'get_BindingContext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackgroundImage()
    'set_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackgroundImage()
    'get_BackgroundImage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_BackColor()
    'set_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BackColor()
    'get_BackColor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Anchor()
    'set_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Anchor()
    'get_Anchor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowDrop()
    'set_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowDrop()
    'get_AllowDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDispose()
    'Dispose()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Disposed()
    'remove_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Disposed()
    'add_Disposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Site()
    'set_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Site()
    'get_Site()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateObjRef()
    'CreateObjRef()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitializeLifetimeService()
    'InitializeLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetLifetimeService()
    'GetLifetimeService()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAskUser()
    'AskUser()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AcceptButton()
    'get_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AcceptButton()
    'set_AcceptButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ActiveMdiChild()
    'get_ActiveMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AllowTransparency()
    'get_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AllowTransparency()
    'set_AllowTransparency()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScale()
    'get_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScale()
    'set_AutoScale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FormBorderStyle()
    'get_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FormBorderStyle()
    'set_FormBorderStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CancelButton()
    'get_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CancelButton()
    'set_CancelButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientSize()
    'get_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ClientSize()
    'set_ClientSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ControlBox()
    'get_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ControlBox()
    'set_ControlBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopBounds()
    'get_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopBounds()
    'set_DesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DesktopLocation()
    'get_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DesktopLocation()
    'set_DesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DialogResult()
    'get_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DialogResult()
    'set_DialogResult()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HelpButton()
    'get_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_HelpButton()
    'set_HelpButton()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Icon()
    'get_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Icon()
    'set_Icon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiChild()
    'get_IsMdiChild()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsMdiContainer()
    'get_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsMdiContainer()
    'set_IsMdiContainer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_KeyPreview()
    'get_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_KeyPreview()
    'set_KeyPreview()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximizedBoundsChanged()
    'add_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximizedBoundsChanged()
    'remove_MaximizedBoundsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximumSize()
    'get_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximumSize()
    'set_MaximumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MaximumSizeChanged()
    'add_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MaximumSizeChanged()
    'remove_MaximumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Menu()
    'get_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Menu()
    'set_Menu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimumSize()
    'get_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimumSize()
    'set_MinimumSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MinimumSizeChanged()
    'add_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MinimumSizeChanged()
    'remove_MinimumSizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MaximizeBox()
    'get_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MaximizeBox()
    'set_MaximizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiChildren()
    'get_MdiChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MdiParent()
    'get_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MdiParent()
    'set_MdiParent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MergedMenu()
    'get_MergedMenu()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_MinimizeBox()
    'get_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_MinimizeBox()
    'set_MinimizeBox()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Modal()
    'get_Modal()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Opacity()
    'get_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Opacity()
    'set_Opacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_OwnedForms()
    'get_OwnedForms()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Owner()
    'get_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Owner()
    'set_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ShowInTaskbar()
    'get_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ShowInTaskbar()
    'set_ShowInTaskbar()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Size()
    'get_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Size()
    'set_Size()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SizeGripStyle()
    'get_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_SizeGripStyle()
    'set_SizeGripStyle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_StartPosition()
    'get_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_StartPosition()
    'set_StartPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabIndex()
    'get_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabIndex()
    'set_TabIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabIndexChanged()
    'add_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabIndexChanged()
    'remove_TabIndexChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevel()
    'get_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopLevel()
    'set_TopLevel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopMost()
    'get_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TopMost()
    'set_TopMost()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TransparencyKey()
    'get_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TransparencyKey()
    'set_TransparencyKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsRestrictedWindow()
    'get_IsRestrictedWindow()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowState()
    'get_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowState()
    'set_WindowState()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestActivate()
    'Activate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Activated()
    'add_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Activated()
    'remove_Activated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closing()
    'add_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closing()
    'remove_Closing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Closed()
    'add_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Closed()
    'remove_Closed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Deactivate()
    'add_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Deactivate()
    'remove_Deactivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Load()
    'add_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Load()
    'remove_Load()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MdiChildActivate()
    'add_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MdiChildActivate()
    'remove_MdiChildActivate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuComplete()
    'add_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuComplete()
    'remove_MenuComplete()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MenuStart()
    'add_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MenuStart()
    'remove_MenuStart()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanged()
    'add_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanged()
    'remove_InputLanguageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_InputLanguageChanging()
    'add_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_InputLanguageChanging()
    'remove_InputLanguageChanging()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddOwnedForm()
    'AddOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClose()
    'Close()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayoutMdi()
    'LayoutMdi()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveOwnedForm()
    'RemoveOwnedForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopBounds()
    'SetDesktopBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetDesktopLocation()
    'SetDesktopLocation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShowDialog()
    'ShowDialog()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ParentForm()
    'get_ParentForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestValidate()
    'Validate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMargin()
    'get_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMargin()
    'set_AutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollPosition()
    'get_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollPosition()
    'set_AutoScrollPosition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AutoScrollMinSize()
    'get_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AutoScrollMinSize()
    'set_AutoScrollMinSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DockPadding()
    'get_DockPadding()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScrollControlIntoView()
    'ScrollControlIntoView()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetAutoScrollMargin()
    'SetAutoScrollMargin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibilityObject()
    'get_AccessibilityObject()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDefaultActionDescription()
    'get_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDefaultActionDescription()
    'set_AccessibleDefaultActionDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleDescription()
    'get_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleDescription()
    'set_AccessibleDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleName()
    'get_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleName()
    'set_AccessibleName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AccessibleRole()
    'get_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_AccessibleRole()
    'set_AccessibleRole()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackColorChanged()
    'add_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackColorChanged()
    'remove_BackColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BackgroundImageChanged()
    'add_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BackgroundImageChanged()
    'remove_BackgroundImageChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataBindings()
    'get_DataBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetBindings()
    'ResetBindings()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_BindingContextChanged()
    'add_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_BindingContextChanged()
    'remove_BindingContextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bottom()
    'get_Bottom()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Bounds()
    'get_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Bounds()
    'set_Bounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanFocus()
    'get_CanFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanSelect()
    'get_CanSelect()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capture()
    'get_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capture()
    'set_Capture()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CausesValidation()
    'get_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CausesValidation()
    'set_CausesValidation()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CausesValidationChanged()
    'add_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CausesValidationChanged()
    'remove_CausesValidationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ClientRectangle()
    'get_ClientRectangle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CompanyName()
    'get_CompanyName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ContainsFocus()
    'get_ContainsFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ContextMenuChanged()
    'add_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ContextMenuChanged()
    'remove_ContextMenuChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Controls()
    'get_Controls()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Created()
    'get_Created()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_CursorChanged()
    'add_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_CursorChanged()
    'remove_CursorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsDisposed()
    'get_IsDisposed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Disposing()
    'get_Disposing()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DockChanged()
    'add_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DockChanged()
    'remove_DockChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Enabled()
    'get_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Enabled()
    'set_Enabled()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_EnabledChanged()
    'add_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_EnabledChanged()
    'remove_EnabledChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_FontChanged()
    'add_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_FontChanged()
    'remove_FontChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ForeColorChanged()
    'add_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ForeColorChanged()
    'remove_ForeColorChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_HasChildren()
    'get_HasChildren()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Height()
    'get_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Height()
    'set_Height()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsHandleCreated()
    'get_IsHandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ImeMode()
    'get_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ImeMode()
    'set_ImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsAccessible()
    'get_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_IsAccessible()
    'set_IsAccessible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Left()
    'get_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Left()
    'set_Left()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Location()
    'get_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Location()
    'set_Location()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LocationChanged()
    'add_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LocationChanged()
    'remove_LocationChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Name()
    'set_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Parent()
    'get_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Parent()
    'set_Parent()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductName()
    'get_ProductName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProductVersion()
    'get_ProductVersion()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_RecreatingHandle()
    'get_RecreatingHandle()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Region()
    'get_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Region()
    'set_Region()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Right()
    'get_Right()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_RightToLeftChanged()
    'add_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_RightToLeftChanged()
    'remove_RightToLeftChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SizeChanged()
    'add_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SizeChanged()
    'remove_SizeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TabStop()
    'get_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TabStop()
    'set_TabStop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TabStopChanged()
    'add_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TabStopChanged()
    'remove_TabStopChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Tag()
    'get_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Tag()
    'set_Tag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_TextChanged()
    'add_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_TextChanged()
    'remove_TextChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Top()
    'get_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Top()
    'set_Top()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TopLevelControl()
    'get_TopLevelControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Visible()
    'get_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Visible()
    'set_Visible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_VisibleChanged()
    'add_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_VisibleChanged()
    'remove_VisibleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Width()
    'get_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Width()
    'set_Width()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_WindowTarget()
    'get_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_WindowTarget()
    'set_WindowTarget()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Click()
    'add_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Click()
    'remove_Click()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlAdded()
    'add_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlAdded()
    'remove_ControlAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ControlRemoved()
    'add_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ControlRemoved()
    'remove_ControlRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragDrop()
    'add_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragDrop()
    'remove_DragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragEnter()
    'add_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragEnter()
    'remove_DragEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragOver()
    'add_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragOver()
    'remove_DragOver()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DragLeave()
    'add_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DragLeave()
    'remove_DragLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GiveFeedback()
    'add_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GiveFeedback()
    'remove_GiveFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleCreated()
    'add_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleCreated()
    'remove_HandleCreated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HandleDestroyed()
    'add_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HandleDestroyed()
    'remove_HandleDestroyed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_HelpRequested()
    'add_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_HelpRequested()
    'remove_HelpRequested()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Invalidated()
    'add_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Invalidated()
    'remove_Invalidated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Paint()
    'add_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Paint()
    'remove_Paint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryContinueDrag()
    'add_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryContinueDrag()
    'remove_QueryContinueDrag()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_QueryAccessibilityHelp()
    'add_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_QueryAccessibilityHelp()
    'remove_QueryAccessibilityHelp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_DoubleClick()
    'add_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_DoubleClick()
    'remove_DoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Enter()
    'add_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Enter()
    'remove_Enter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_GotFocus()
    'add_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_GotFocus()
    'remove_GotFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ImeModeChanged()
    'add_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ImeModeChanged()
    'remove_ImeModeChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyDown()
    'add_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyDown()
    'remove_KeyDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyPress()
    'add_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyPress()
    'remove_KeyPress()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_KeyUp()
    'add_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_KeyUp()
    'remove_KeyUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Layout()
    'add_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Layout()
    'remove_Layout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Leave()
    'add_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Leave()
    'remove_Leave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_LostFocus()
    'add_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_LostFocus()
    'remove_LostFocus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseDown()
    'add_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseDown()
    'remove_MouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseEnter()
    'add_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseEnter()
    'remove_MouseEnter()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseLeave()
    'add_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseLeave()
    'remove_MouseLeave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseHover()
    'add_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseHover()
    'remove_MouseHover()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseMove()
    'add_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseMove()
    'remove_MouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseUp()
    'add_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseUp()
    'remove_MouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_MouseWheel()
    'add_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_MouseWheel()
    'remove_MouseWheel()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Move()
    'add_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Move()
    'remove_Move()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Resize()
    'add_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Resize()
    'remove_Resize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangeUICues()
    'add_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangeUICues()
    'remove_ChangeUICues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_StyleChanged()
    'add_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_StyleChanged()
    'remove_StyleChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_SystemColorsChanged()
    'add_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_SystemColorsChanged()
    'remove_SystemColorsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validating()
    'add_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validating()
    'remove_Validating()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Validated()
    'add_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Validated()
    'remove_Validated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ParentChanged()
    'add_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ParentChanged()
    'remove_ParentChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBringToFront()
    'BringToFront()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateGraphics()
    'CreateGraphics()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreateControl()
    'CreateControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDoDragDrop()
    'DoDragDrop()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindForm()
    'FindForm()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFocus()
    'Focus()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetChildAtPoint()
    'GetChildAtPoint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetContainerControl()
    'GetContainerControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetNextControl()
    'GetNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestHide()
    'Hide()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInvalidate()
    'Invalidate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPerformLayout()
    'PerformLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToClient()
    'PointToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointToScreen()
    'PointToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResetImeMode()
    'ResetImeMode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToClient()
    'RectangleToClient()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRectangleToScreen()
    'RectangleToScreen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestResumeLayout()
    'ResumeLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScale()
    'Scale()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelect()
    'Select()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSelectNextControl()
    'SelectNextControl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSendToBack()
    'SendToBack()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetBounds()
    'SetBounds()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShow()
    'Show()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSuspendLayout()
    'SuspendLayout()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUpdate()
    'Update()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Container()
    'get_Container()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcTimeseriesStatistics

  Public Sub TestAddDataSet()
    'AddDataSet()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Specification()
    'set_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Specification()
    'get_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSave()
    'Save()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadData()
    'ReadData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpen()
    'Open()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanSave()
    'get_CanSave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanOpen()
    'get_CanOpen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataSets()
    'get_DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AvailableOperations()
    'get_AvailableOperations()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMessage()
    'Message()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestItemClicked()
    'ItemClicked()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShapesSelected()
    'ShapesSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersCleared()
    'LayersCleared()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerSelected()
    'LayerSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerRemoved()
    'LayerRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersAdded()
    'LayersAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapDragFinished()
    'MapDragFinished()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseUp()
    'MapMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseMove()
    'MapMouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseDown()
    'MapMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapExtentsChanged()
    'MapExtentsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseUp()
    'LegendMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseDown()
    'LegendMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendDoubleClick()
    'LegendDoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTerminate()
    'Terminate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitialize()
    'Initialize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectSaving()
    'ProjectSaving()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectLoading()
    'ProjectLoading()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Version()
    'get_Version()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BuildDate()
    'get_BuildDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SerialNumber()
    'get_SerialNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Author()
    'get_Author()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataManager()
    'get_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DataManager()
    'set_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcTimeseries

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Value()
    'get_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Value()
    'set_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Values()
    'get_Values()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Values()
    'set_Values()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValueAttributes()
    'get_ValueAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValueAttributes()
    'set_ValueAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Dates()
    'get_Dates()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Dates()
    'set_Dates()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_numValues()
    'get_numValues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_numValues()
    'set_numValues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEnsureValuesRead()
    'EnsureValuesRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValuesNeedToBeRead()
    'get_ValuesNeedToBeRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValuesNeedToBeRead()
    'set_ValuesNeedToBeRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Serial()
    'get_Serial()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasons

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsAMPM

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsHour

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfMonth

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfWeek

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfYear

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsMonth

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsYear

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsThresholdTS

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasonsYearSubset

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataSource

  Public Sub TestAddDataSet()
    'AddDataSet()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Specification()
    'set_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Specification()
    'get_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSave()
    'Save()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadData()
    'ReadData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpen()
    'Open()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanSave()
    'get_CanSave()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CanOpen()
    'get_CanOpen()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataSets()
    'get_DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AvailableOperations()
    'get_AvailableOperations()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMessage()
    'Message()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestItemClicked()
    'ItemClicked()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShapesSelected()
    'ShapesSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersCleared()
    'LayersCleared()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerSelected()
    'LayerSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerRemoved()
    'LayerRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersAdded()
    'LayersAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapDragFinished()
    'MapDragFinished()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseUp()
    'MapMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseMove()
    'MapMouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseDown()
    'MapMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapExtentsChanged()
    'MapExtentsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseUp()
    'LegendMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseDown()
    'LegendMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendDoubleClick()
    'LegendDoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTerminate()
    'Terminate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitialize()
    'Initialize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectSaving()
    'ProjectSaving()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectLoading()
    'ProjectLoading()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Version()
    'get_Version()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BuildDate()
    'get_BuildDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SerialNumber()
    'get_SerialNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Author()
    'get_Author()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataManager()
    'get_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DataManager()
    'set_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_EnumExistAction

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetTypeCode()
    'GetTypeCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCompareTo()
    'CompareTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataSet

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Serial()
    'get_Serial()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataPlugin

  Public Sub TestMessage()
    'Message()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestItemClicked()
    'ItemClicked()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShapesSelected()
    'ShapesSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersCleared()
    'LayersCleared()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerSelected()
    'LayerSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerRemoved()
    'LayerRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersAdded()
    'LayersAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapDragFinished()
    'MapDragFinished()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseUp()
    'MapMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseMove()
    'MapMouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseDown()
    'MapMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapExtentsChanged()
    'MapExtentsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseUp()
    'LegendMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseDown()
    'LegendMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendDoubleClick()
    'LegendDoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTerminate()
    'Terminate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitialize()
    'Initialize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectSaving()
    'ProjectSaving()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectLoading()
    'ProjectLoading()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Version()
    'get_Version()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BuildDate()
    'get_BuildDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SerialNumber()
    'get_SerialNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Author()
    'get_Author()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataManager

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_OpenedData()
    'add_OpenedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_OpenedData()
    'remove_OpenedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Basins()
    'get_Basins()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataSources()
    'get_DataSources()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDataSets()
    'DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SelectionAttributes()
    'get_SelectionAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DisplayAttributes()
    'get_DisplayAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetPlugins()
    'GetPlugins()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpenDataSource()
    'OpenDataSource()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserSelectDataSource()
    'UserSelectDataSource()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserSelectData()
    'UserSelectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserManage()
    'UserManage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_XML()
    'get_XML()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_XML()
    'set_XML()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_OpenedDataEventHandler

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataGroup

  Public Sub Testset_ItemByIndex()
    'set_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByIndex()
    'get_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetEnumerator()
    'GetEnumerator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsSynchronized()
    'get_IsSynchronized()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SyncRoot()
    'get_SyncRoot()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Count()
    'get_Count()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyTo()
    'CopyTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveAt()
    'RemoveAt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemove()
    'Remove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsert()
    'Insert()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOf()
    'IndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsFixedSize()
    'get_IsFixedSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsReadOnly()
    'get_IsReadOnly()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAdd()
    'Add()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Item()
    'set_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Item()
    'get_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTrimToSize()
    'TrimToSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToArray()
    'ToArray()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSort()
    'Sort()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetRange()
    'GetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetRange()
    'SetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReverse()
    'Reverse()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveRange()
    'RemoveRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLastIndexOf()
    'LastIndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsertRange()
    'InsertRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBinarySearch()
    'BinarySearch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddRange()
    'AddRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capacity()
    'set_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capacity()
    'get_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Removed()
    'remove_Removed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Removed()
    'add_Removed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Added()
    'remove_Added()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Added()
    'add_Added()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByKey()
    'get_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByKey()
    'set_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChangeTo()
    'ChangeTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOfSerial()
    'IndexOfSerial()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SelectedData()
    'get_SelectedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_SelectedData()
    'set_SelectedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Keys()
    'get_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Keys()
    'set_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByKey()
    'RemoveByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromKey()
    'IndexFromKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_AddedEventHandler

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_RemovedEventHandler

  Public Sub TestInvoke()
    'Invoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEndInvoke()
    'EndInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBeginInvoke()
    'BeginInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataDisplay

  Public Sub TestShow()
    'Show()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMessage()
    'Message()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestItemClicked()
    'ItemClicked()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestShapesSelected()
    'ShapesSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersCleared()
    'LayersCleared()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerSelected()
    'LayerSelected()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayerRemoved()
    'LayerRemoved()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLayersAdded()
    'LayersAdded()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapDragFinished()
    'MapDragFinished()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseUp()
    'MapMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseMove()
    'MapMouseMove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapMouseDown()
    'MapMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMapExtentsChanged()
    'MapExtentsChanged()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseUp()
    'LegendMouseUp()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendMouseDown()
    'LegendMouseDown()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLegendDoubleClick()
    'LegendDoubleClick()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTerminate()
    'Terminate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitialize()
    'Initialize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectSaving()
    'ProjectSaving()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestProjectLoading()
    'ProjectLoading()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Version()
    'get_Version()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_BuildDate()
    'get_BuildDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SerialNumber()
    'get_SerialNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Author()
    'get_Author()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataAttributes

  Public Sub TestAddDefinition()
    'AddDefinition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetDefinition()
    'GetDefinition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAllDefinitions()
    'AllDefinitions()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIsSimple()
    'IsSimple()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByIndex()
    'set_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByIndex()
    'get_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetEnumerator()
    'GetEnumerator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsSynchronized()
    'get_IsSynchronized()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SyncRoot()
    'get_SyncRoot()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Count()
    'get_Count()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyTo()
    'CopyTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveAt()
    'RemoveAt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemove()
    'Remove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsert()
    'Insert()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOf()
    'IndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsFixedSize()
    'get_IsFixedSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsReadOnly()
    'get_IsReadOnly()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAdd()
    'Add()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Item()
    'set_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Item()
    'get_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTrimToSize()
    'TrimToSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToArray()
    'ToArray()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSort()
    'Sort()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetRange()
    'GetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetRange()
    'SetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReverse()
    'Reverse()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveRange()
    'RemoveRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLastIndexOf()
    'LastIndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsertRange()
    'InsertRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBinarySearch()
    'BinarySearch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddRange()
    'AddRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capacity()
    'set_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capacity()
    'get_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddHistory()
    'AddHistory()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Owner()
    'get_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Owner()
    'set_Owner()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestValuesSortedByName()
    'ValuesSortedByName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContainsAttribute()
    'ContainsAttribute()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetFormattedValue()
    'GetFormattedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetValue()
    'GetValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetValue()
    'SetValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCalculateAll()
    'CalculateAll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDiscardCalculated()
    'DiscardCalculated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetDefinedValue()
    'GetDefinedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Keys()
    'get_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Keys()
    'set_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByKey()
    'RemoveByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByKey()
    'get_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByKey()
    'set_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromKey()
    'IndexFromKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChangeTo()
    'ChangeTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDefinedValue

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcAttributeDefinition

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Name()
    'get_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Name()
    'set_Name()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Calculated()
    'get_Calculated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Calculator()
    'get_Calculator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Calculator()
    'set_Calculator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Category()
    'set_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Description()
    'set_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Help()
    'get_Help()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Help()
    'set_Help()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Editable()
    'get_Editable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Editable()
    'set_Editable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ID()
    'get_ID()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ID()
    'set_ID()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TypeString()
    'get_TypeString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TypeString()
    'set_TypeString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValidList()
    'get_ValidList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValidList()
    'set_ValidList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DefaultValue()
    'get_DefaultValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DefaultValue()
    'set_DefaultValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Min()
    'get_Min()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Min()
    'set_Min()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Max()
    'get_Max()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Max()
    'set_Max()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class
