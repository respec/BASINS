''' <summary>
''' Interface for handling status and progress updates
''' </summary>
Public Interface IProgressStatus
    ''' <summary>
    ''' Log the progress of a long-running task
    ''' </summary>
    ''' <param name="aCurrentPosition">Current position/item of task</param>
    ''' <param name="aLastPosition">Final position/item of task</param>
    ''' <remarks>
    ''' A final call when the task is done with aCurrent = aLast 
    ''' indicates completion and should clear the progress display.
    ''' </remarks>
    Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer)

    ''' <summary>
    ''' Update the current status message
    ''' </summary>
    ''' <param name="aStatusMessage">Description of current processing status</param>
    ''' <remarks></remarks>
    Sub Status(ByVal aStatusMessage As String)
End Interface

''' <summary>
''' Default implementation does nothing
''' </summary>
Public Class NullProgressStatus
    Implements IProgressStatus

    Public Sub Progress(ByVal aCurrentPosition As Integer, _
                        ByVal aLastPosition As Integer) Implements IProgressStatus.Progress
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements IProgressStatus.Status
    End Sub
End Class

''' <summary>
''' Use StatusMonitor
''' </summary>
Public Class MonitorProgressStatus
    Implements IProgressStatus

    Public Sub Progress(ByVal aCurrentPosition As Integer, _
                        ByVal aLastPosition As Integer) Implements IProgressStatus.Progress
        SendMonitorMessage("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements IProgressStatus.Status
        If aStatusMessage Is Nothing OrElse aStatusMessage.Length = 0 Then
            SendMonitorMessage("HIDE")
        ElseIf aStatusMessage.StartsWith("LABEL ") Then
            SendMonitorMessage(aStatusMessage)
        Else
            SendMonitorMessage("LABEL 1 " & aStatusMessage)
        End If
    End Sub
End Class