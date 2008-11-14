Imports System.Drawing

Public Class frmControl

    Dim pPChecBoxValues(11) As Integer
    Dim pIChecBoxValues(5) As Integer
    Dim pRChecBoxValues(9) As Integer

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        SSTabPIR.SelectTab(0)
        SSTabPIR.TabPages(0).Enabled = False
        SSTabPIR.TabPages(1).Enabled = False
        SSTabPIR.TabPages(2).Enabled = False


        If pUCI.OpnBlks("PERLND").Count > 0 Then
            SSTabPIR.TabPages(0).Enabled = True
            txtNoPERLND.Visible = False
            With pUCI.OpnBlks("PERLND").Tables("ACTIVITY")

                CheckBox1.Checked = .Parms(0).Value
                CheckBox2.Checked = .Parms(1).Value
                CheckBox3.Checked = .Parms(2).Value
                CheckBox4.Checked = .Parms(3).Value
                CheckBox5.Checked = .Parms(4).Value
                CheckBox6.Checked = .Parms(5).Value
                CheckBox7.Checked = .Parms(6).Value
                CheckBox8.Checked = .Parms(7).Value
                CheckBox9.Checked = .Parms(8).Value
                CheckBox10.Checked = .Parms(9).Value
                CheckBox11.Checked = .Parms(10).Value
                CheckBox12.Checked = .Parms(11).Value

            End With
        Else 'no perlnd
            SSTabPIR.TabPages(0).Enabled = False
            txtNoPERLND.Visible = True

            CheckBox1.Visible = False
            CheckBox2.Visible = False
            CheckBox3.Visible = False
            CheckBox4.Visible = False
            CheckBox5.Visible = False
            CheckBox6.Visible = False
            CheckBox7.Visible = False
            CheckBox8.Visible = False
            CheckBox9.Visible = False
            CheckBox10.Visible = False
            CheckBox11.Visible = False
            CheckBox12.Visible = False
            CheckBox13.Visible = False

        End If

        If pUCI.OpnBlks("IMPLND").Count > 0 Then
            SSTabPIR.TabPages(1).Enabled = True
            txtNoIMPLND.Visible = False
            With pUCI.OpnBlks("IMPLND").Tables("ACTIVITY")

                CheckBox13.Checked = .Parms(0).Value
                CheckBox14.Checked = .Parms(1).Value
                CheckBox15.Checked = .Parms(2).Value
                CheckBox16.Checked = .Parms(3).Value
                CheckBox17.Checked = .Parms(4).Value
                CheckBox18.Checked = .Parms(5).Value

            End With
        Else ' no implnd
            SSTabPIR.TabPages(1).Enabled = False
            txtNoIMPLND.Visible = True

            CheckBox13.Visible = False
            CheckBox14.Visible = False
            CheckBox15.Visible = False
            CheckBox16.Visible = False
            CheckBox17.Visible = False
            CheckBox18.Visible = False

        End If

        If pUCI.OpnBlks("RCHRES").Count > 0 Then
            SSTabPIR.TabPages(2).Enabled = True
            txtNoRCHRES.Visible = False
            With pUCI.OpnBlks("RCHRES").Tables("ACTIVITY")

                CheckBox19.Checked = .Parms(0).Value
                CheckBox20.Checked = .Parms(1).Value
                CheckBox21.Checked = .Parms(2).Value
                CheckBox22.Checked = .Parms(3).Value
                CheckBox23.Checked = .Parms(4).Value
                CheckBox24.Checked = .Parms(5).Value
                CheckBox25.Checked = .Parms(6).Value
                CheckBox26.Checked = .Parms(7).Value
                CheckBox27.Checked = .Parms(8).Value
                CheckBox28.Checked = .Parms(9).Value

            End With
        Else 'no rchres
            SSTabPIR.TabPages(2).Enabled = False
            txtNoIMPLND.Visible = True

            CheckBox19.Visible = False
            CheckBox20.Visible = False
            CheckBox21.Visible = False
            CheckBox22.Visible = False
            CheckBox23.Visible = False
            CheckBox24.Visible = False
            CheckBox25.Visible = False
            CheckBox26.Visible = False
            CheckBox27.Visible = False
            CheckBox28.Visible = False
        End If
    End Sub


    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub

    Private Sub CheckIChange(ByVal Index)

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SSTabPIR.TabPages(1).Enabled = False
        txtNoIMPLND.Visible = True

        CheckBox13.Visible = False
        CheckBox14.Visible = False
        CheckBox15.Visible = False
        CheckBox16.Visible = False
        CheckBox17.Visible = False
        CheckBox18.Visible = False
    End Sub

    Private Sub GenerateValuesArray()
        'pPChecBoxValues(0) = CheckBox1.CheckState
        'pPChecBoxValues(1) = CheckBox2.CheckState
        'pPChecBoxValues(2) = CheckBox3.CheckState
        'pPChecBoxValues(3) = CheckBox4.CheckState
        'pPChecBoxValues(4) = CheckBox5.CheckState
        'pPChecBoxValues(5) = CheckBox6.CheckState
        'pPChecBoxValues(6) = CheckBox7.CheckState
        'pPChecBoxValues(7) = CheckBox8.CheckState
        'pPChecBoxValues(8) = CheckBox9.CheckState
        'pPChecBoxValues(9) = CheckBox10.CheckState
        'pPChecBoxValues(10) = CheckBox11.CheckState
        'pPChecBoxValues(11) = CheckBox12.CheckState
    End Sub
    Private Sub ChecksChanged_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged, CheckBox3.CheckedChanged, CheckBox4.CheckedChanged, CheckBox5.CheckedChanged, CheckBox6.CheckedChanged, CheckBox7.CheckedChanged, CheckBox8.CheckedChanged, CheckBox9.CheckedChanged, CheckBox10.CheckedChanged, CheckBox11.CheckedChanged, CheckBox12.CheckedChanged, CheckBox13.CheckedChanged, CheckBox14.CheckedChanged, CheckBox15.CheckedChanged, CheckBox16.CheckedChanged, CheckBox17.CheckedChanged, CheckBox18.CheckedChanged, CheckBox19.CheckedChanged, CheckBox20.CheckedChanged, CheckBox21.CheckedChanged, CheckBox22.CheckedChanged, CheckBox23.CheckedChanged, CheckBox24.CheckedChanged, CheckBox25.CheckedChanged, CheckBox26.CheckedChanged, CheckBox27.CheckedChanged, CheckBox28.CheckedChanged
        Dim lClickedCheckBox As Windows.Forms.CheckBox = sender

        If lClickedCheckBox.Checked = True Then
            Select Case lClickedCheckBox.Name

                'PERLND Checkboxes
                Case "CheckBox2"
                    CheckBox1.Checked = True
                Case "CheckBox4"
                    CheckBox3.Checked = True
                Case "CheckBox5"
                    CheckBox1.Checked = True
                Case "CheckBox6"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox5.Checked = True
                Case "CheckBox7"
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                Case "CheckBox9"
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox8.Checked = True
                Case "CheckBox10"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                Case "CheckBox11"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                Case "CheckBox12"
                    CheckBox3.Checked = True
                    CheckBox8.Checked = True

                    'IMPLND Checkboxes
                Case "CheckBox14"
                    CheckBox13.Checked = True
                Case "CheckBox16"
                    CheckBox15.Checked = True
                Case "CheckBox17"
                    CheckBox13.Checked = True
                    CheckBox15.Checked = True
                Case "CheckBox18"
                    CheckBox15.Checked = True
                    CheckBox16.Checked = True

                    'RCHRES Checkboxes
                Case "CheckBox20"
                    CheckBox19.Checked = True
                Case "CheckBox21"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                Case "CheckBox22"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                Case "CheckBox23"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                Case "CheckBox24"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                Case "CheckBox25"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                Case "CheckBox26"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                Case "CheckBox27"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                Case "CheckBox28"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox21.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                    CheckBox27.Checked = True
            End Select
        End If

    End Sub
End Class