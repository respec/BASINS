Option Strict Off
Option Explicit On

Imports atcUtility

Friend Class frmDebugScript
	Inherits System.Windows.Forms.Form
	'Copyright 2002 by AQUA TERRA Consultants
	Private ButtonPressed As String
	
	'UPGRADE_NOTE: asc was upgraded to asc_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Sub ShowScript(ByRef asc_Renamed As clsATCscriptExpression)
		'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim str_Renamed As String
		Dim LineNum, delimPos, lastdelim As Integer
		ScriptAssigningLineNumbers = True
		str_Renamed = asc_Renamed.Printable
		ScriptAssigningLineNumbers = False
		Text = "Debug Script " & asc_Renamed.SubExpression(1).Printable
		txtCurrentLine.Text = ""
		lblCurrentLine.Text = "Current Input Line "
		LineNum = 0
		lastdelim = 1
		lstCurrentScript.Items.Clear()
		delimPos = InStr(lastdelim, str_Renamed, PrintEOL)
		While delimPos > 0
			lstCurrentScript.Items.Add(Mid(str_Renamed, lastdelim, delimPos - lastdelim))
			lastdelim = delimPos + Len(PrintEOL)
			delimPos = InStr(lastdelim, str_Renamed, PrintEOL)
		End While
		'Me.Show 'FIXME trouble with vbModal GenScn vs. WDMUtil
	End Sub
	
	Public Sub NextLine()
		txtCurrentLine.Text = CurrentLine
		lblCurrentLine.Text = "Current Input Line # " & CurrentLineNum
	End Sub
	
	Public Sub NewDate(ByRef index As Integer, ByRef jdy As Double)
        lblDate.Text = "Date(" & index & "): " & DumpDate(jdy)
	End Sub
	
    Public Sub NewValue(ByRef index As Integer, ByRef aValue As Double)
        lblValue.Text = "Value(" & index & "): " & DoubleToString(aValue)
    End Sub
	
	'UPGRADE_NOTE: asc was upgraded to asc_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Sub EvalExpression(ByRef asc_Renamed As clsATCscriptExpression)
		Static PreviousLine As Integer
		lstCurrentScript.SelectedIndex = asc_Renamed.Line - 1
		'txtCurrentScript.Text = asc.Printable
		If ButtonPressed = "Next" And PreviousLine = asc_Renamed.Line Then
			'Don't step through all the subexpressions on this line
		ElseIf ButtonPressed <> "Run" And ButtonPressed <> "Quit" Then 
			ButtonPressed = ""
			While ButtonPressed = ""
				System.Windows.Forms.Application.DoEvents()
			End While
			PreviousLine = asc_Renamed.Line
		End If
	End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNext.Click
		ButtonPressed = "Next"
	End Sub
	
	Private Sub cmdQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click
		ButtonPressed = "Quit"
		AbortScript = True
	End Sub
	
	Private Sub cmdRun_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRun.Click
		ButtonPressed = "Run"
		DebuggingScript = False
		Me.Hide()
	End Sub
	
	Private Sub cmdStep_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStep.Click
		ButtonPressed = "Step"
	End Sub
	
	Private Sub cmdStep_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles cmdStep.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		frmDebugScript_KeyDown(Me, New System.Windows.Forms.KeyEventArgs(KeyCode Or Shift * &H10000))
	End Sub
	
	Private Sub frmDebugScript_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Select Case KeyCode
			Case System.Windows.Forms.Keys.F8 : ButtonPressed = "Step"
			Case System.Windows.Forms.Keys.F5 : ButtonPressed = "Run"
		End Select
	End Sub
	
    Private Sub frmDebugScript_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        'Dim newWidth, newHeight As Single
        'newWidth = VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 2172
        'If newWidth > 100 Then
        '	txtCurrentLine.Width = VB6.TwipsToPixelsX(newWidth)
        '	lstCurrentScript.Width = VB6.TwipsToPixelsX(newWidth)
        '	fraButtons.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(fraButtons.Height) - 108)
        '	newHeight = (VB6.PixelsToTwipsY(fraButtons.Top) - 276) / 2
        '	If newHeight > 100 Then
        '		txtCurrentLine.Height = VB6.TwipsToPixelsY(newHeight)
        '		lstCurrentScript.Height = VB6.TwipsToPixelsY(newHeight)
        '		lstCurrentScript.Top = VB6.TwipsToPixelsY(newHeight + 78)
        '		lblCurProgramLine.Top = lstCurrentScript.Top
        '	End If
        'End If
    End Sub
End Class