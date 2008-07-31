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
            txtStartYear.Text = pHspfGlobalBlk.SDate(0)
            txtStartMo.Text = pHspfGlobalBlk.SDate(1)
            txtStartDay.Text = pHspfGlobalBlk.SDate(2)
            txtStartHr.Text = pHspfGlobalBlk.SDate(3)
            txtStartMin.Text = pHspfGlobalBlk.SDate(4)
            txtEndYear.Text = pHspfGlobalBlk.EDate(0)
            txtEndMo.Text = pHspfGlobalBlk.EDate(1)
            txtEndDay.Text = pHspfGlobalBlk.EDate(2)
            txtEndHr.Text = pHspfGlobalBlk.EDate(3)
            txtEndMin.Text = pHspfGlobalBlk.EDate(4)

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
        pHspfGlobalBlk.RunInf.Value = txtRunInfo.Text

        pHspfGlobalBlk.SDate(0) = txtStartYear.Text
        pHspfGlobalBlk.SDate(1) = txtStartMo.Text
        pHspfGlobalBlk.SDate(2) = txtStartDay.Text
        pHspfGlobalBlk.SDate(3) = txtStartHr.Text
        pHspfGlobalBlk.SDate(4) = txtStartMin.Text
        pHspfGlobalBlk.EDate(0) = txtEndYear.Text
        pHspfGlobalBlk.EDate(1) = txtEndMo.Text
        pHspfGlobalBlk.EDate(2) = txtEndDay.Text
        pHspfGlobalBlk.EDate(3) = txtEndHr.Text
        pHspfGlobalBlk.EDate(4) = txtEndMin.Text

    End Sub

    Public Sub New(ByVal aHspfGlobalBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfGlobalBlk
    End Sub
End Class
