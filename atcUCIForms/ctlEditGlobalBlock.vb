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
            txtStartYr.Value = pHspfGlobalBlk.SDate(0)
            txtStartMo.Value = pHspfGlobalBlk.SDate(1)
            txtStartDay.Value = pHspfGlobalBlk.SDate(2)
            txtStartHr.Value = pHspfGlobalBlk.SDate(3)
            txtStartMin.Value = pHspfGlobalBlk.SDate(4)
            txtEndYr.Value = pHspfGlobalBlk.EDate(0)
            txtEndMo.Value = pHspfGlobalBlk.EDate(1)
            txtEndDay.Value = pHspfGlobalBlk.EDate(2)
            txtEndHr.Value = pHspfGlobalBlk.EDate(3)
            txtEndMin.Value = pHspfGlobalBlk.EDate(4)

            'Populate combo boxes
            Dim i&
            For i = 0 To 10
                comboGen.Items.Add(i)
                comboSpout.Items.Add(i)
            Next i
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

            .SDate(0) = txtStartYr.Value
            .SDate(1) = txtStartMo.Value
            .SDate(2) = txtStartDay.Value
            .SDate(3) = txtStartHr.Value
            .SDate(4) = txtStartMin.Value
            .EDate(0) = txtEndYr.Value
            .EDate(1) = txtEndMo.Value
            .EDate(2) = txtEndDay.Value
            .EDate(3) = txtEndHr.Value
            .EDate(4) = txtEndMin.Value

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
