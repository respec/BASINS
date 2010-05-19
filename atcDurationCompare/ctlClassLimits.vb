Imports MapWinUtility

Public Class ctlClassLimits
    Public Event ChangedNumClasses()
    'Dim pClassLimits As Double() = {0}
    'Public Property ClassLimits() As Double()
    '    Get
    '        Return pClassLimits
    '    End Get
    '    Set(ByVal value As Double())
    '        pClassLimits = value
    '    End Set
    'End Property

    Dim pNumClasses As Integer = 0
    Public Property NumClasses() As Integer
        Get
            Return pNumClasses
        End Get
        Set(ByVal value As Integer)
            pNumClasses = value
        End Set
    End Property

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If txtNumClasses.Text = "" Then
            Logger.Msg("Please specify the total number of class limits.")
            txtNumClasses.Focus()
            Exit Sub
        End If

        NumClasses = txtNumClasses.ValueInteger
        RaiseEvent ChangedNumClasses()

        'If Me.ParentForm IsNot Nothing Then
        '    If Me.ParentForm.Name = "frmAnalysis" Then
        '        CType(Me.ParentForm, frmAnalysis).lstClassLimits.DefaultValues = GenerateClasses(txtNumClasses.ValueInteger, CType(Me.ParentForm, frmAnalysis).DataGroup)
        '        CType(Me.ParentForm, frmAnalysis).
        '    End If
        'End If
    End Sub
End Class
