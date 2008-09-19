Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditGlobalBlock
    Implements ctlEdit

    Dim pHspfGlobalBlk As HspfGlobalBlk
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Global Block"
        End Get
    End Property

    Public Property Changed() As Boolean Implements ctlEdit.Changed
        Get
            Return pChanged
        End Get
        Set(ByVal aChanged As Boolean)
            If aChanged <> pChanged Then
                pChanged = aChanged
                RaiseEvent Change(aChanged)
            End If
        End Set
    End Property

    Public Sub Add() Implements ctlEdit.Add
        'not needed 
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfGlobalBlk
        End Get
        Set(ByVal aHspfGlobalBlk As Object)
            pHspfGlobalBlk = aHspfGlobalBlk
            txtRunInfo.Text = pHspfGlobalBlk.RunInf.Value
            txtStartYr.ValueInteger = pHspfGlobalBlk.SDate(0)
            txtStartMo.ValueInteger = pHspfGlobalBlk.SDate(1)
            txtStartDay.ValueInteger = pHspfGlobalBlk.SDate(2)
            txtStartHr.ValueInteger = pHspfGlobalBlk.SDate(3)
            txtStartMin.ValueInteger = pHspfGlobalBlk.SDate(4)
            txtEndYr.ValueInteger = pHspfGlobalBlk.EDate(0)
            txtEndMo.ValueInteger = pHspfGlobalBlk.EDate(1)
            txtEndDay.ValueInteger = pHspfGlobalBlk.EDate(2)
            txtEndHr.ValueInteger = pHspfGlobalBlk.EDate(3)
            txtEndMin.ValueInteger = pHspfGlobalBlk.EDate(4)

            'Populate combo boxes
            Dim lOper As Integer
            For lOper = 0 To 10
                comboGen.Items.Add(lOper)
                comboSpout.Items.Add(lOper)
            Next
            comboRunFlag.Items.Add("Interp")
            comboRunFlag.Items.Add("Run")
            comboUnits.Items.Add("English")
            comboUnits.Items.Add("Metric")

            With pHspfGlobalBlk
                'Set the initial selection
                comboGen.SelectedIndex = .outlev.Value
                comboSpout.SelectedIndex = .spout.ToString
                comboRunFlag.SelectedIndex = .runfg.ToString
                comboUnits.SelectedIndex = .emfg.ToString - 1
            End With
        End Set
    End Property

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'not be needed
    End Sub

    Public Sub Save() Implements ctlEdit.Save

        With pHspfGlobalBlk
            .RunInf.Value = txtRunInfo.Text

            .SDate(0) = txtStartYr.ValueInteger
            .SDate(1) = txtStartMo.ValueInteger
            .SDate(2) = txtStartDay.ValueInteger
            .SDate(3) = txtStartHr.ValueInteger
            .SDate(4) = txtStartMin.ValueInteger
            .EDate(0) = txtEndYr.ValueInteger
            .EDate(1) = txtEndMo.ValueInteger
            .EDate(2) = txtEndDay.ValueInteger
            .EDate(3) = txtEndHr.ValueInteger
            .EDate(4) = txtEndMin.ValueInteger

            'Set the initial selection
            .outlev.Value = comboGen.SelectedIndex
            .spout = comboSpout.SelectedIndex
            .runfg = comboRunFlag.SelectedIndex
            .emfg = comboUnits.SelectedIndex + 1
        End With

    End Sub

    Public Sub New(ByVal aHspfGlobalBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfGlobalBlk
    End Sub
End Class
