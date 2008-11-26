Imports System.Windows.Forms

''' <summary>
''' Modeless form showing task status text and progressbar and cancel button
''' </summary>
Public Class frmProgress
    Private m_IsCancelled As Boolean

    ''' <summary>
    ''' Set the current and maximum value for the progress bar; if cancel was pressed, will return false
    ''' </summary>
    Public Function SetProgress(ByVal Value As Integer, ByVal Maximum As Integer) As Boolean
        With barProgress
            .Maximum = Maximum
            .Value = Value
        End With
        BringToFront()
        Application.DoEvents()
        Return Not m_IsCancelled
    End Function

    ''' <summary>
    ''' Set the status text and current and maximum value for the progress bar; if cancel was pressed, will return false
    ''' </summary>
    Public Function SetProgress(ByVal StatusText As String, ByVal Value As Integer, ByVal Maximum As Integer) As Boolean
        lblProgress.Text = StatusText
        With barProgress
            .Maximum = Maximum
            .Value = Value
        End With
        Refresh()
        Application.DoEvents()
        BringToFront()
        Return Not IsCancelled
    End Function

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        m_IsCancelled = True
    End Sub

    Private Sub frmProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_IsCancelled = False
        lblProgress.Text = "Initializing..."
        With barProgress
            .Minimum = 0
            .Maximum = 100
            .Value = 0
            .Refresh()
            Application.DoEvents()
        End With
        BringToFront()
    End Sub

    ''' <summary>
    ''' Return True if cancel button was pressed
    ''' </summary>
    Public Property IsCancelled() As Boolean
        Get
            Return m_IsCancelled
        End Get
        Set(ByVal value As Boolean)
            m_IsCancelled = value
        End Set
    End Property

    ''' <summary>
    ''' Maximum value of progress bar; defaults to 100
    ''' </summary>
    Public Property ProgressMax() As Integer
        Get
            Return barProgress.Maximum
        End Get
        Set(ByVal value As Integer)
            With barProgress
                If value < .Value Then .Value = value
                .Maximum = value
            End With
            BringToFront()
        End Set
    End Property

    ''' <summary>
    ''' Value of progress bar; will force to be between 0 and maximum value; defaults to 0
    ''' </summary>
    Public Property ProgressValue() As Integer
        Get
            Return barProgress.Value
        End Get
        Set(ByVal value As Integer)
            With barProgress
                .Value = Math.Min(Math.Max(value, .Minimum), .Maximum)
            End With
            BringToFront()
        End Set
    End Property

    ''' <summary>
    ''' Text to appear in the status window; if not set will default to "Initializing..."
    ''' </summary>
    Public Property Status() As String
        Get
            Return lblProgress.Text
        End Get
        Set(ByVal value As String)
            lblProgress.Text = value
            Refresh()
            Application.DoEvents()
            BringToFront()
        End Set
    End Property
End Class