Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmDebug
	Inherits System.Windows.Forms.Form
	
	'Copyright 2002 by AQUA TERRA Consultants
	
	'UPGRADE_WARNING: Event CheckSave.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub CheckSave_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckSave.CheckStateChanged
		Dim instSave As Boolean
		If instSave Then
			FileClose(101)
		Else
			'    cdlg.Filename = frmDebug.Caption & ".txt"
			'    cdlg.ShowSave
			'Open cdlg.Filename For Output As #101
		End If
		instSave = Not instSave
		'UPGRADE_WARNING: Couldn't resolve default property of object Switch(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		CheckSave.CheckState = VB.Switch(instSave, 1, Not instSave, 0)
	End Sub
	
	Private Sub cmdClear_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClear.Click
		Dim j As Integer
		For j = 0 To UBound(d)
			'UPGRADE_WARNING: Couldn't resolve default property of object d(j). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            d(j) = dnull
		Next j
		p = 0
		ReDo(True)
	End Sub
	
	Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
		Dim j As Integer
        Dim s As String = ""
        Dim X As String = ""
		
		'  cdlg.Filename = frmDebug.Caption & ".txt"
		'  cdlg.DialogTitle = "Save Debug File as"
		'  cdlg.ShowSave
		'  Open cdlg.Filename For Output As #102
		For j = p - 1 To 0 Step -1
			'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
            AddToBuff(s, X, j)
		Next j
		For j = UBound(d) To p Step -1
			'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
            AddToBuff(s, X, j)
		Next j
		FileClose(102)
		Exit Sub
    End Sub

    Private Sub AddToBuff(ByRef s As String, ByRef X As String, ByVal j As Integer)
        'UPGRADE_WARNING: Couldn't resolve default property of object d(j).lev. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object d().Msg. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Len(d(j).Msg) > 0 And lev >= d(j).lev Then
            'UPGRADE_WARNING: Couldn't resolve default property of object BldDebugRec(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            s = BldDebugRec(d(j))
            If X <> s Then
                PrintLine(102, s)
            End If
            X = s
        End If
    End Sub
	'UPGRADE_WARNING: Event frmDebug.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub frmDebug_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		'UPGRADE_ISSUE: Unable to determine which constant to upgrade vbNormal to. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B3B44E51-B5F1-4FD7-AA29-CAD31B71F487"'
		If WindowState = vbNormal Then
			If VB6.PixelsToTwipsX(Me.Width) < VB6.PixelsToTwipsX(fraOptions.Width) + 300 Then
				Me.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraOptions.Width) + 300)
			End If
			If VB6.PixelsToTwipsY(Me.Height) < VB6.PixelsToTwipsY(fraOptions.Height) + VB6.PixelsToTwipsY(txtDetails.Top) + 640 Then
				Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraOptions.Height) + VB6.PixelsToTwipsY(txtDetails.Top) + 640)
			End If
		End If
		txtDetails.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 300)
		fraOptions.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(fraOptions.Height) - 360)
		txtDetails.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraOptions.Top) - 480)
	End Sub
	
	Private Sub optInstSave_Click(ByRef Index As Short)
		If Index = 0 Then
			If Not (instSave) Then
				'      cdlg.Filename = frmDebug.Caption & ".txt"
				'      cdlg.DialogTitle = "Save Debug File as"
				'      cdlg.ShowSave
				'      Open cdlg.Filename For Output As #101
				instSave = True
			End If
		Else
			If instSave Then
				FileClose(101)
				instSave = False
			End If
		End If
	End Sub
	
	'UPGRADE_WARNING: Event ListType.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ListType_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ListType.SelectedIndexChanged
		ReDo(False)
	End Sub
	
	'UPGRADE_WARNING: Event txtFlsh.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtFlsh_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtFlsh.TextChanged
		If IsNumeric(txtFlsh.Text) Then
			flsh = CInt(txtFlsh.Text)
		Else
			Beep()
			txtFlsh.Text = CStr(flsh)
		End If
	End Sub
	
	'UPGRADE_WARNING: Event txtLev.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtLev_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLev.TextChanged
		If IsNumeric(txtLev.Text) Then
			lev = CInt(txtLev.Text)
			ReDo(True)
		Else
			Beep()
			txtLev.Text = CStr(lev)
		End If
	End Sub
End Class