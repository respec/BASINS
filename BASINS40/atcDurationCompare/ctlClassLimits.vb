Imports MapWinUtility

Public Class ctlClassLimits
    Public Event SettingChanged()
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

    Dim pMin As Double = -9999.0
    Dim pMax As Double = -9999.0

    Public Property LowerBound() As Double
        Get
            Return pMin
        End Get
        Set(ByVal value As Double)
            pMin = value
        End Set
    End Property

    Public Property UpperBound() As Double
        Get
            Return pMax
        End Get
        Set(ByVal value As Double)
            pMax = value
        End Set
    End Property

    Private pUsePresetClasses As Boolean = False
    Public ReadOnly Property UsePresetClasses() As Boolean
        Get
            Return pUsePresetClasses
        End Get
    End Property

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If chkSWSTATDefault.Checked Then
            pUsePresetClasses = True
            GoTo Preset
        Else
            pUsePresetClasses = False
        End If

        If txtNumClasses.Text = "" Then
            Logger.Msg("Please specify the total number of class limits.")
            txtNumClasses.Focus()
            Exit Sub
        End If

        NumClasses = txtNumClasses.ValueInteger
        If NumClasses <= 0 Then
            Logger.Msg("Number of classes must be greater than zero.")
            txtNumClasses.Focus()
            Exit Sub
        End If

        Dim lTemp As Double = 0.0
        Dim lMinIsSet As Boolean = False
        If txtMin.Text.Length > 0 Then
            If IsNumeric(txtMin.Text) And Double.TryParse(txtMin.Text, lTemp) Then
                LowerBound = lTemp
                lMinIsSet = True
            End If
        End If

        Dim lMaxIsSet As Boolean = False
        If txtMax.Text.Length > 0 Then
            If IsNumeric(txtMax.Text) And Double.TryParse(txtMax.Text, lTemp) Then
                UpperBound = lTemp
                lMaxIsSet = True
            End If
        End If

        If Not lMinIsSet Then LowerBound = -9999.0
        If Not lMaxIsSet Then UpperBound = -9999.0

Preset:
        RaiseEvent SettingChanged()
    End Sub
End Class
