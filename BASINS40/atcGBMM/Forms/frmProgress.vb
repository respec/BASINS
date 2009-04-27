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
        Static LastTime As Date = #1/1/2000#
        If Me.lblProgress.InvokeRequired Then
            Dim d As New SetProgressDelegate(AddressOf SetProgress)
            Me.Invoke(d, New Object() {"", Value, Maximum})
        Else
            If Value = 0 OrElse Now.Subtract(LastTime).TotalSeconds > 0.1 Then
                With barProgress
                    .Style = ProgressBarStyle.Blocks
                    .Maximum = Maximum
                    .Value = Value
                    .Refresh()
                End With
                'Debug.Print(lblProgress.Text & ":" & Value & "," & Maximum)
                BringToFront()
                Application.DoEvents()
                LastTime = Now
            End If
            Return Not m_IsCancelled
        End If
    End Function

    Delegate Function SetProgressOverallDelegate(ByVal StatusText As String, ByVal Value As Integer, ByVal MaxValue As Integer) As Boolean

    ''' <summary>
    ''' Set the status text and current and maximum value for the progress bar; if cancel was pressed, will return false
    ''' </summary>
    Public Function SetProgressOverall(ByVal StatusText As String, ByVal Value As Integer, ByVal Maximum As Integer) As Boolean
        If Me.lblProgress.InvokeRequired Then
            Dim d As New SetProgressOverallDelegate(AddressOf SetProgressOverall)
            Me.Invoke(d, New Object() {StatusText, Value, Maximum})
        Else
            lblProgressOverall.Text = StatusText
            With barProgressOverall
                .Maximum = Maximum
                .Value = Value
            End With
            lblProgress.Text = "Computing..."
            barProgress.Value = 0
            Refresh()
            Application.DoEvents()
            BringToFront()
            Return Not IsCancelled
        End If
    End Function

    Delegate Function SetProgressDelegate(ByVal StatusText As String, ByVal Value As Integer, ByVal Maximum As Integer) As Boolean

    ''' <summary>
    ''' Set the status text and current and maximum value for the progress bar; if cancel was pressed, will return false
    ''' </summary>
    Public Function SetProgress(ByVal StatusText As String, ByVal Value As Integer, ByVal Maximum As Integer) As Boolean
        Static LastText As String = ""
        If Me.lblProgress.InvokeRequired Then
            Dim d As New SetProgressDelegate(AddressOf SetProgress)
            Me.Invoke(d, New Object() {StatusText, Value, Maximum})
        Else
            If StatusText <> "" And StatusText <> LastText Then
                lblProgress.Text = StatusText
                lblProgress.Refresh()
                LastText = StatusText
                Value = 0
            End If
            Return SetProgress(Value, Maximum)
        End If
    End Function

    ''' <summary>
    ''' Set the status text and progressbar style to marquee; if cancel was pressed, will return false
    ''' </summary>
    Public Function SetProgress(ByVal StatusText As String, Optional ByVal ProgressBarStyle As Windows.Forms.ProgressBarStyle = Windows.Forms.ProgressBarStyle.Blocks) As Boolean
        barProgress.Style = ProgressBarStyle
        barProgress.MarqueeAnimationSpeed = 100
        lblProgress.Text = StatusText
        Refresh()
        Application.DoEvents()
        BringToFront()
        Return Not IsCancelled
    End Function

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        lblProgress.Text = "Cancelling..."
        m_IsCancelled = True
    End Sub

    Private Sub frmProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_IsCancelled = False
        lblProgressOverall.Text = "Initializing..."
        lblProgress.Text = ""
        With barProgressOverall
            .Minimum = 0
            .Maximum = 100
            .Value = 0
            .Refresh()
            Application.DoEvents()
        End With
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
    ''' Text to appear in the status window; if not set will default to "Computing..."
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

    Public Property ProgressBarStyle() As Windows.Forms.ProgressBarStyle
        Get
            Return barProgress.Style
        End Get
        Set(ByVal value As Windows.Forms.ProgressBarStyle)
            barProgress.Style = value
        End Set
    End Property
End Class