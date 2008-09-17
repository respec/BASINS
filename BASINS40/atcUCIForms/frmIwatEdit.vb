Imports atcUCI
Imports atcControls
Imports System

Public Class frmIwatEdit

    Dim lMyTable As HspfTable
    Dim lOptionVals As New ArrayList
    Dim lIdList As New ArrayList
    Dim lPrevCboLandIndex As Integer


    Public Sub Init(ByVal aParent As Object)
        Me.Text = "PWATER Simulation and Input Options"
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        lMyTable = aParent

        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lOper As Integer
        Dim tempTable As HspfTable

        cboLand.Items.Clear()
        cboLand.Cursor = Windows.Forms.Cursors.Hand
        cboLand.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList

        For Each lVOpn In lMyTable.Opn.Uci.OpnBlks("IMPLND").Ids
            lOpn = lVOpn
            cboLand.Items.Add(lOpn.Description & " (" & lOpn.Id & ")")
            lIdList.Add(lOpn.Id)
            'save a local copy of the values
            tempTable = lOpn.Tables(lMyTable.Name)
            For lOper = 0 To tempTable.Parms.Count - 1
                lOptionVals.Add(tempTable.Parms(lOper).Value)
            Next
        Next
        cboLand.SelectedIndex = 0
    End Sub
End Class