Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Module modDebug
	'Copyright 2002 by AQUA TERRA Consultants
	
    Public Structure dbuff
        Dim Msg As String
        Dim lev As Integer
        Dim typ As String
        Dim tim As Date
        'UPGRADE_NOTE: mod was upgraded to mod_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim mod_Renamed As String
    End Structure
	Public d(1000) As dbuff
	Public dnull As dbuff
	
	Public p As Integer
	Public lev As Integer
	Public flsh As Integer
	Public instSave As Boolean
	
	Public Function BldDebugRec(ByRef d As dbuff) As Object
		Dim s As String
		
		s = d.tim & "  " & d.mod_Renamed
		s = s & Space(30 - Len(s)) & d.typ & d.lev & ":" & d.Msg
		'UPGRADE_WARNING: Couldn't resolve default property of object BldDebugRec. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		BldDebugRec = s
	End Function
	
	Public Sub DbgMsg(ByRef Msg As String, Optional ByRef level As Integer = 7, Optional ByRef modul As String = "?", Optional ByRef typ As String = "?")
		
		Debug.Print(VB6.TabLayout(Msg, level, modul, typ))
		
		Dim q As Boolean
		Dim l As Integer
		Dim DT As dbuff
		
		On Error GoTo lpt
		l = level
		If l <= flsh Then 'save it, dont flush
			DT.Msg = Msg
			DT.lev = l
			DT.tim = TimeOfDay
			DT.typ = UCase(typ)
			If p = 0 Then
				q = True
			ElseIf DT.Msg = d(p - 1).Msg And DT.tim = d(p - 1).tim Then 
				q = False
			Else
				q = True
			End If
			If q Then
				If instSave Then
					PrintLine(101, BldDebugRec(DT))
				End If
				'UPGRADE_WARNING: Couldn't resolve default property of object d(p). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				d(p) = DT
				p = p + 1
				If p = UBound(d) + 1 Then
					p = 0
				End If
				If frmDebug.Visible Then ReDo(True)
			End If
		End If
		Exit Sub
lpt: 
		Debug.Print(Msg)
	End Sub
	
	Public Sub ReDo(ByRef RefreshTypes As Boolean)
		Dim s, t, X As String
		Dim j As Integer
		Static InRedo As Boolean
		If InRedo Then Exit Sub Else InRedo = True
		If frmDebug.Visible = False Then Exit Sub
        If RefreshTypes Then frmDebug.ListType.Items.Clear()
        s = ""
		t = ""
		X = ""
		For j = p - 1 To 0 Step -1
            AddToBuff(s, X, t, j, RefreshTypes)
		Next j
		For j = UBound(d) To p Step -1
            AddToBuff(s, X, t, j, RefreshTypes)
		Next j
		If Len(t) > 0 Then
			frmDebug.txtDetails.Text = t
		Else
			frmDebug.txtDetails.Text = ""
		End If
		InRedo = False
    End Sub

    Private Sub AddToBuff(ByRef s As String, ByRef X As String, ByRef t As String, ByVal j As Integer, ByVal RefreshTypes As Boolean)
        'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim str_Renamed As Object
        Dim i As Integer
        Dim found As Boolean
        If Len(d(j).Msg) > 0 And lev >= d(j).lev Then
            With frmDebug.ListType
                i = 0
                found = False
                While i < frmDebug.ListType.Items.Count And Not found
                    If Left(VB6.GetItemString(frmDebug.ListType, i), 1) = d(j).typ Then found = True Else i = i + 1
                End While
                If RefreshTypes And Not found Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object Switch(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    str_Renamed = VB.Switch(d(j).typ = "C", "Calculation", d(j).typ = "E", "Error", d(j).typ = "F", "Focus", d(j).typ = "I", "In Routine", d(j).typ = "K", "Keyboard", d(j).typ = "L", "Load", d(j).typ = "M", "Mouse", d(j).typ = "O", "Out of Routine", d(j).typ = "P", "Property Change", d(j).typ = "W", "Window")
                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                    If IsDBNull(str_Renamed) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        str_Renamed = d(j).typ
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        str_Renamed = d(j).typ & " - " & str_Renamed
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    .Items.Add(str_Renamed)
                    i = .Items.Count - 1
                    .SetItemChecked(i, True)
                End If
                If frmDebug.ListType.GetItemChecked(i) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object BldDebugRec(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    s = BldDebugRec(d(j))
                    If X <> s Then
                        t = t & s & vbCrLf
                    End If
                    X = s
                End If
            End With
        End If
    End Sub
End Module