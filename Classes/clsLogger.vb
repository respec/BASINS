Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Logger provides program logging to a file and progress messages.
''' All methods and variables are shared, so there is no need to 
''' create or pass an instance of Logger. Just make a reference to 
''' this library and use Logger.Dbg, Logger.Msg, Logger.Status, Logger.Progress
''' 
''' If a clsProgressStatus is set, then progress and status messages 
''' are sent to it, allowing display.
''' </summary>
''' <remarks></remarks>
Public Class Logger

    Private Const MillisecondsPerDay As Double = 86400000 '24 hours/day * 60 minutes/hour * 60 seconds/minute * 1000 milliseconds/second

    ''' <summary>
    ''' True to flush after each write to log file
    ''' False to buffer writes to log file and wait for explicit Flush() or operating system
    ''' </summary>
    ''' <remarks>False may save some time, True makes log files more likely to be helpful</remarks>
    Public Shared AutoFlush As Boolean = True

    ''' <summary>
    ''' Icon to be used on custom message boxes
    ''' </summary>
    Public Shared Icon As Drawing.Icon = Nothing

    'Private Shared pBusy As Integer = 0 'Incremented by setting Busy = True, decremented by setting Busy = False

    Private Shared pProgressRefresh As Double = 200 / MillisecondsPerDay
    Private Shared pProgressStartTime As Double = 0
    Private Shared pProgressLastUpdate As Double = 0

    'set to default value so we don't have to check whether it is set each time we use it
    Private Shared pProgressStatus As IProgressStatus = New NullProgressStatus

    Private Shared pFileStream As IO.StreamWriter
    Private Shared pTimeStamp As Boolean = True  'Default to including time stamps
    Private Shared pTimeStampRelative As Boolean = True  'zero time stamp at start of run
    Private Shared pTimeStampStart As Date = Now 'Start time stamps from start of run

    Private Shared pFileName As String = "" 'file to write logs to
    Private Shared pDisplayMessageBoxes As Boolean = True 'False to suppress message boxes

    Private Shared pLastDbgText As String = ""

    ''' <summary>
    ''' New is private to ensure Logger is a singleton
    ''' </summary>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Start logging to a file
    ''' </summary>
    ''' <param name="aLogFileName">Name of file to put subsequent log messages in</param>
    ''' <param name="aAppend">True to append to existing file, False to create new file</param>
    ''' <param name="aRenameExisting">True to rename old log files as needed to use aLogFileName,
    ''' False to overwrite existing log file</param>
    ''' <param name="aForceNameChange">True to change log file even if already logging to a file</param>
    ''' <remarks>If aLogFileName is blank and aForceNameChange = True, logging to file is stopped.</remarks>
    Public Shared Sub StartToFile(ByVal aLogFileName As String, _
                         Optional ByVal aAppend As Boolean = False, _
                         Optional ByVal aRenameExisting As Boolean = True, _
                         Optional ByVal aForceNameChange As Boolean = False)
        If aForceNameChange OrElse pFileStream Is Nothing Then 'log file name change is allowed
            If pFileStream IsNot Nothing Then 'Close the already-open file
                Try
                    pFileStream.Close()
                Catch ex As Exception
                End Try
                pFileStream = Nothing
            End If

            pFileName = aLogFileName

            If pFileName.Length > 0 Then
                MkDirPath(PathNameOnly(pFileName))

                If FileExists(pFileName) Then
                    If Not aAppend Then
                        If aRenameExisting Then
                            Rename(pFileName, MakeLogName(pFileName))
                        Else
                            Kill(pFileName)
                        End If
                    End If
                End If
                pFileStream = New StreamWriter(pFileName, aAppend)
                Dbg("StartToFile " & Format(Now, "yyyy-MM-dd hh:mm:ss"))
            End If
        End If
    End Sub

    ''' <summary>File name being logged to</summary>
    Public Shared ReadOnly Property FileName() As String
        Get
            Return pFileName
        End Get
    End Property

    ''' <summary>Append a number to a given file name if needed to make it unique</summary>
    Private Shared Function MakeLogName(ByVal aLogFileName As String) As String
        Dim lTryName As String
        Dim lTry As Integer = 1

        Do
            lTryName = FilenameNoExt(pFileName) & "#" & lTry & ".log"
            lTry += 1
        Loop While FileExists(lTryName)
        Return lTryName
    End Function

    ''' <summary>True to enable time stamping each log event</summary>
    Public Shared Property TimeStamping() As Boolean
        Set(ByVal aLogTimeStamp As Boolean)
            pTimeStamp = aLogTimeStamp
        End Set
        Get
            Return pTimeStamp
        End Get
    End Property

    ''' <summary>
    ''' True to start time stamps at zero when execution starts, stamp elapsed hh:mm:ss.milliseconds,
    ''' False to stamp each event with current date/time            YYYY-MM-DD@hh:mm:ss.milliseconds
    ''' </summary>
    Public Shared Property TimeStampRelative() As Boolean
        Get
            Return pTimeStampRelative
        End Get
        Set(ByVal newValue As Boolean)
            pTimeStampRelative = newValue
        End Set
    End Property

    ''' <summary>Progress and status messages are displayed by this object</summary>
    Public Shared Property ProgressStatus() As IProgressStatus
        Get
            Return pProgressStatus
        End Get
        Set(ByVal newValue As IProgressStatus)
            pProgressStatus = newValue
        End Set
    End Property

    ''' <summary>Write any pending log messages to the log file (if logging to a file)</summary>
    Public Shared Sub Flush()
        If pFileStream IsNot Nothing Then
            pFileStream.Flush()
        End If
    End Sub

    ''' <summary>
    ''' Add a "debug" message to the log. 
    ''' </summary>
    ''' <param name="aMessages">Any number of strings to add to the log, usually just one</param>
    ''' <remarks>
    ''' Debug messages are commonly used to indicate that execution has reached a certain line.
    ''' Important values just computed or about to be used are often included.
    ''' </remarks>
    Public Shared Sub Dbg(ByVal ParamArray aMessages() As String)
        Dim lText As String = ""
        Dim lFullMessage As String = ""
        For Each lMessage As Object In aMessages
            If lFullMessage.Length > 0 Then
                lFullMessage &= ":" & lMessage.ToString
            Else
                lFullMessage &= lMessage.ToString
            End If
        Next

        If pTimeStamp Then
            If pTimeStampRelative Then
                With (Now - pTimeStampStart)
                    lText &= Format(.Hours, "00") & ":" & _
                             Format(.Minutes, "00") & ":" & _
                             Format(.Seconds, "00") & "." & _
                             Format(.Milliseconds, "000") & vbTab
                End With
            Else
                With (Now)
                    lText &= Format(.Year, "0000") & "-" & _
                             Format(.Month, "00") & "-" & _
                             Format(.Day, "00") & "@" & _
                             Format(.Hour, "00") & ":" & _
                             Format(.Minute, "00") & ":" & _
                             Format(.Second, "00") & "." & _
                             Format(.Millisecond, "000") & vbTab
                End With
            End If
        End If

        pLastDbgText = MethodCallingLogger() & ":" & lFullMessage
        lText &= pLastDbgText

        Try
            If pFileStream IsNot Nothing Then
                pFileStream.WriteLine(lText)
                If AutoFlush Then pFileStream.Flush()
            End If
            Debug.WriteLine(lText)
        Catch lEx As Exception
            Debug.Write("FailedToLog:" & lText)
        End Try
    End Sub

    Public Shared Property LastDbgText() As String
        Get
            Return pLastDbgText
        End Get
        Set(ByVal newValue As String)
            pLastDbgText = newValue
        End Set
    End Property

    Public Shared Property DisplayMessageBoxes() As Boolean
        Get
            Return pDisplayMessageBoxes
        End Get
        Set(ByVal newValue As Boolean)
            pDisplayMessageBoxes = newValue
        End Set
    End Property

    ''' <summary>
    ''' Create a custom message box form
    ''' </summary>
    Private Shared Function CustomMsgBox() As frmCustomMsgBox
        Dim lMsgBox As New frmCustomMsgBox
        If Logger.Icon Is Nothing Then
            lMsgBox.ShowIcon = False
        Else
            lMsgBox.Icon = Logger.Icon
            lMsgBox.ShowIcon = True
        End If
        Return lMsgBox
    End Function

    Public Shared Function MsgCustom(ByVal aMessage As String, _
                                     ByVal aTitle As String, _
                                     ByVal ParamArray aButtonLabels() As String) As String
        Dbg("MsgCustom:" & aMessage & ":Title:" & aTitle & ":Style:" & String.Join(",", aButtonLabels))
        Dim lResult As String = ""
        If pDisplayMessageBoxes Then
            Dim lMsgBox As frmCustomMsgBox = CustomMsgBox()
            lResult = lMsgBox.AskUser(aMessage, aTitle, aButtonLabels)
            Dbg("MsgResult:" & lResult)
            pLastDbgText = ""
        Else 'Default to first button or button label containing "+"
            lResult = aButtonLabels(0)
            For Each lButtonLabel As String In aButtonLabels
                If lButtonLabel.Contains("+") Then lResult = lButtonLabel
            Next
            Dbg("MsgUserInteractionSkipped:Result:" & lResult)
            pLastDbgText = aMessage 'dont forget the skipped message
        End If
        Return lResult
    End Function

    Public Shared Function MsgCustomCheckbox(ByVal aMessage As String, _
                                             ByVal aTitle As String, _
                                             ByVal aRegistryCheckboxText As String, _
                                             ByVal aRegistryAppName As String, _
                                             ByVal aRegistrySection As String, _
                                             ByVal aRegistryKey As String, _
                                             ByVal ParamArray aButtonLabels() As String) As String
        Dbg("MsgCustom:" & aMessage & ":Title:" & aTitle & ":Style:" & String.Join(",", aButtonLabels))
        Dim lResult As String = ""

        Dim lRegistryLabel As String = GetSetting(aRegistryAppName, aRegistrySection, aRegistryKey, "")
        If lRegistryLabel.Length > 0 Then
            lResult = lRegistryLabel
        ElseIf pDisplayMessageBoxes Then
            Dim lMsgBox As frmCustomMsgBox = CustomMsgBox()
            lMsgBox.RegistryCheckboxText = aRegistryCheckboxText
            lMsgBox.RegistryAppName = aRegistryAppName
            lMsgBox.RegistrySection = aRegistrySection
            lMsgBox.RegistryKey = aRegistryKey
            lResult = lMsgBox.AskUser(aMessage, aTitle, aButtonLabels)
            Dbg("MsgResult:" & lResult)
            pLastDbgText = ""
        Else 'Default to first button or button label containing "+"
            lResult = aButtonLabels(0)
            For Each lButtonLabel As String In aButtonLabels
                If lButtonLabel.Contains("+") Then lResult = lButtonLabel
            Next
            Dbg("MsgUserInteractionSkipped:Result:" & lResult)
            pLastDbgText = aMessage 'dont forget the skipped message
        End If
        Return lResult
    End Function

    Public Overloads Shared Function Msg(ByVal aMessage As String) As MsgBoxResult
        Return Msg(aMessage, MsgBoxStyle.OkOnly)
    End Function

    Public Overloads Shared Function Msg(ByVal aMessage As String, _
                                         ByVal aTitle As String) As MsgBoxResult
        Return Msg(aMessage, MsgBoxStyle.OkOnly, aTitle)
    End Function

    Public Overloads Shared Function Msg(ByVal aMessage As String, _
                                         ByVal aMsgBoxStyle As MsgBoxStyle) As MsgBoxResult
        Dim lTitle As String = ""
        If aMessage.IndexOf(":") > 0 Then
            lTitle = Strings.StrSplit(aMessage, ":", "")
        End If
        Return Msg(aMessage, aMsgBoxStyle, lTitle)
    End Function

    ''' <summary>
    '''  Log the use of a Microsoft.VisualBasic.MsgBox 
    ''' </summary>
    ''' <param name="aMessage">Message to display</param>
    ''' <param name="aMsgBoxStyle">Message box style</param>
    ''' <param name="aTitle">Title of message box</param>
    ''' <returns>MsgBoxResult</returns>
    ''' <remarks>Logs when the message box is displayed and what the user selects</remarks>
    Public Overloads Shared Function Msg(ByVal aMessage As String, _
                                         ByVal aMsgBoxStyle As MsgBoxStyle, _
                                         ByVal aTitle As String) As MsgBoxResult
        Dim lMsgBoxDefault As MsgBoxResult
        Select Case aMsgBoxStyle
            Case MsgBoxStyle.AbortRetryIgnore
                lMsgBoxDefault = MsgBoxResult.Abort
            Case MsgBoxStyle.OkCancel
                lMsgBoxDefault = MsgBoxResult.Cancel
            Case MsgBoxStyle.OkOnly
                lMsgBoxDefault = MsgBoxResult.Ok
            Case MsgBoxStyle.RetryCancel
                lMsgBoxDefault = MsgBoxResult.Cancel
            Case MsgBoxStyle.YesNo
                lMsgBoxDefault = MsgBoxResult.No
            Case MsgBoxStyle.YesNoCancel
                lMsgBoxDefault = MsgBoxResult.Cancel
        End Select
        Return Msg(aMessage, aMsgBoxStyle, lMsgBoxDefault, aTitle)
    End Function

    ''' <summary>
    '''  Log the use of a Microsoft.VisualBasic.MsgBox 
    ''' </summary>
    ''' <param name="aMessage">Message to display</param>
    ''' <param name="aMsgBoxStyle">Message box style</param>
    ''' <param name="aMsgBoxDefault">Message box default result</param>
    ''' <param name="aTitle">Title of message box</param>
    ''' <returns>MsgBoxResult</returns>
    ''' <remarks>Logs when the message box is displayed and what the user selects</remarks>
    Public Overloads Shared Function Msg(ByVal aMessage As String, _
                                         ByVal aMsgBoxStyle As MsgBoxStyle, _
                                         ByVal aMsgBoxDefault As MsgBoxResult, _
                                         ByVal aTitle As String) As MsgBoxResult
        Dbg("Msg:" & aMessage & ":Title:" & aTitle & ":Style:" & aMsgBoxStyle)
        Flush()
        Dim lResult As MsgBoxResult

        If pDisplayMessageBoxes Then
            lResult = Microsoft.VisualBasic.MsgBox(aMessage, MsgBoxStyleAddDefaultButton(aMsgBoxStyle, aMsgBoxDefault), aTitle)
            Dbg("MsgResult:" & lResult & ":" & System.Enum.GetName(lResult.GetType, lResult))
            pLastDbgText = ""
        Else
            lResult = aMsgBoxDefault
            Dbg("MsgUserInteractionSkipped:Result:" & lResult & ":" & System.Enum.GetName(lResult.GetType, lResult))
            pLastDbgText = aMessage 'dont forget the skipped message
        End If
        Return lResult
    End Function

    Private Shared Function MsgBoxStyleAddDefaultButton(ByVal aMsgBoxStyle As MsgBoxStyle, _
                                                        ByVal aMsgBoxDefault As MsgBoxResult) As MsgBoxStyle
        If aMsgBoxStyle And MsgBoxStyle.AbortRetryIgnore Then
            Select Case aMsgBoxDefault
                Case MsgBoxResult.Abort : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton1
                Case MsgBoxResult.Retry : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton2
                Case MsgBoxResult.Ignore : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton3
            End Select
        ElseIf aMsgBoxStyle And MsgBoxStyle.OkCancel Then
            Select Case aMsgBoxDefault
                Case MsgBoxResult.Ok : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton1
                Case MsgBoxResult.Cancel : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton2
            End Select
        ElseIf aMsgBoxStyle And MsgBoxStyle.RetryCancel Then
            Select Case aMsgBoxDefault
                Case MsgBoxResult.Retry : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton1
                Case MsgBoxResult.Cancel : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton2
            End Select
        ElseIf aMsgBoxStyle And MsgBoxStyle.YesNo Then
            Select Case aMsgBoxDefault
                Case MsgBoxResult.Yes : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton1
                Case MsgBoxResult.No : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton2
            End Select
        ElseIf aMsgBoxStyle And MsgBoxStyle.YesNoCancel Then
            Select Case aMsgBoxDefault
                Case MsgBoxResult.Yes : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton1
                Case MsgBoxResult.No : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton2
                Case MsgBoxResult.Cancel : Return aMsgBoxStyle Or MsgBoxStyle.DefaultButton3
            End Select
        End If
        Return aMsgBoxStyle
    End Function

    ''' <summary>
    ''' Log the use of a Windows.Forms.MessageBox
    ''' </summary>
    ''' <param name="aText">Message to show</param>
    ''' <param name="aCaption">Caption for top of message box</param>
    ''' <param name="aButtons">Buttons to display</param>
    ''' <param name="aIcon">Icon to include in message box</param>
    ''' <param name="aDefaultResult">Button to press when user presses Enter</param>
    ''' <returns>DialogResult selected by user</returns>
    ''' <remarks>Logs when the message box is displayed and what the user selects</remarks>
    Public Shared Function Message(ByVal aText As String, _
                                   ByVal aCaption As String, _
                                   ByVal aButtons As MessageBoxButtons, _
                                   ByVal aIcon As MessageBoxIcon, _
                                   ByVal aDefaultResult As DialogResult) As DialogResult
        Dbg("Message:" & aText & ":Caption:" & aCaption & ":Buttons:" & aButtons.ToString)
        Flush()
        Dim lResult As DialogResult = DialogResult.OK
        If pDisplayMessageBoxes Then
            lResult = Windows.Forms.MessageBox.Show(aText, aCaption, aButtons, aIcon, _
                                                    DialogResultToMessageBoxDefaultButton(aButtons, aDefaultResult))
            Dbg("MessageResult:" & lResult & ":" & System.Enum.GetName(lResult.GetType, lResult))
            pLastDbgText = ""
        Else
            lResult = aDefaultResult
            Dbg("MessageUserInteractionSkipped:Result:" & lResult & ":" & System.Enum.GetName(lResult.GetType, lResult))
            pLastDbgText = aText 'dont forget the skipped message
        End If
        Return lResult
    End Function

    ''' <summary>
    ''' Log the use of a Windows.Forms.MessageBox
    ''' </summary>
    ''' <param name="aText">Message to show</param>
    ''' <param name="aCaption">Caption for top of message box</param>
    ''' <param name="aButtons">Buttons to display</param>
    ''' <param name="aIcon">Icon to include in message box</param>
    ''' <param name="aDefaultButton">Button to press when user presses Enter</param>
    ''' <returns>DialogResult selected by user</returns>
    ''' <remarks>Logs when the message box is displayed and what the user selects</remarks>
    Public Shared Function Message(ByVal aText As String, _
                                   ByVal aCaption As String, _
                                   ByVal aButtons As MessageBoxButtons, _
                                   ByVal aIcon As MessageBoxIcon, _
                                   ByVal aDefaultButton As MessageBoxDefaultButton) As DialogResult
        Return Message(aText, aCaption, aButtons, aIcon, _
                       MessageBoxDefaultButtonToDialogResult(aButtons, aDefaultButton))
    End Function

    ''' <summary>
    ''' Translate MessageBoxDefaultButton and a set of MessageBoxButtons into a DialogResult
    ''' </summary>
    ''' <param name="aButtons"></param>
    ''' <param name="aDefaultButton"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function MessageBoxDefaultButtonToDialogResult(ByVal aButtons As MessageBoxButtons, _
                                                                  ByVal aDefaultButton As MessageBoxDefaultButton) _
                                                                  As DialogResult
        Select Case aButtons
            Case MessageBoxButtons.AbortRetryIgnore
                Select Case aDefaultButton
                    Case MessageBoxDefaultButton.Button1 : Return DialogResult.Abort
                    Case MessageBoxDefaultButton.Button2 : Return DialogResult.Retry
                    Case MessageBoxDefaultButton.Button3 : Return DialogResult.Ignore
                End Select
            Case MessageBoxButtons.OK
                Return DialogResult.OK
            Case MessageBoxButtons.OKCancel
                Select Case aDefaultButton
                    Case MessageBoxDefaultButton.Button1 : Return DialogResult.OK
                    Case MessageBoxDefaultButton.Button2 : Return DialogResult.Cancel
                End Select
            Case MessageBoxButtons.RetryCancel
                Select Case aDefaultButton
                    Case MessageBoxDefaultButton.Button1 : Return DialogResult.Retry
                    Case MessageBoxDefaultButton.Button2 : Return DialogResult.Cancel
                End Select
            Case MessageBoxButtons.YesNo
                Select Case aDefaultButton
                    Case MessageBoxDefaultButton.Button1 : Return DialogResult.Yes
                    Case MessageBoxDefaultButton.Button2 : Return DialogResult.No
                End Select
            Case MessageBoxButtons.YesNoCancel
                Select Case aDefaultButton
                    Case MessageBoxDefaultButton.Button1 : Return DialogResult.Yes
                    Case MessageBoxDefaultButton.Button2 : Return DialogResult.No
                    Case MessageBoxDefaultButton.Button3 : Return DialogResult.Cancel
                End Select
        End Select
        Return DialogResult.OK
    End Function

    Private Shared Function DialogResultToMessageBoxDefaultButton(ByVal aButtons As MessageBoxButtons, _
                                                                  ByVal aDefaultButton As DialogResult) _
                                                                  As MessageBoxDefaultButton
        Select Case aButtons
            Case MessageBoxButtons.AbortRetryIgnore
                Select Case aDefaultButton
                    Case DialogResult.Abort : Return MessageBoxDefaultButton.Button1
                    Case DialogResult.Retry : Return MessageBoxDefaultButton.Button2
                    Case DialogResult.Ignore : Return MessageBoxDefaultButton.Button3
                End Select
            Case MessageBoxButtons.OK
                Return MessageBoxDefaultButton.Button1
            Case MessageBoxButtons.OKCancel
                Select Case aDefaultButton
                    Case DialogResult.OK : Return MessageBoxDefaultButton.Button1
                    Case DialogResult.Cancel : Return MessageBoxDefaultButton.Button2
                End Select
            Case MessageBoxButtons.RetryCancel
                Select Case aDefaultButton
                    Case DialogResult.Retry : Return MessageBoxDefaultButton.Button1
                    Case DialogResult.Cancel : Return MessageBoxDefaultButton.Button2
                End Select
            Case MessageBoxButtons.YesNo
                Select Case aDefaultButton
                    Case DialogResult.Yes : Return MessageBoxDefaultButton.Button1
                    Case DialogResult.No : Return MessageBoxDefaultButton.Button2
                End Select
            Case MessageBoxButtons.YesNoCancel
                Select Case aDefaultButton
                    Case DialogResult.Yes : Return MessageBoxDefaultButton.Button1
                    Case DialogResult.No : Return MessageBoxDefaultButton.Button2
                    Case DialogResult.Cancel : Return MessageBoxDefaultButton.Button3
                End Select
        End Select
        Return MessageBoxDefaultButton.Button1
    End Function

    ''' <summary>
    ''' Display the current status in the status bar
    ''' </summary>
    ''' <param name="aMessage">Status message</param>
    ''' <remarks></remarks>
    Public Shared Sub Status(ByVal aMessage As String)
        Try
            pProgressStatus.Status(aMessage)
        Catch ex As Exception 'Ignore exception if pProgressStatus fails
        End Try
    End Sub

    ''' <summary>
    ''' Display the current status in the status bar
    ''' </summary>
    ''' <param name="aMessage">Status message</param>
    ''' <param name="aLog">Write to log file flag</param>
    ''' <remarks></remarks>
    Public Shared Sub Status(ByVal aMessage As String, ByVal aLog As Boolean)
        If aLog Then
            Dbg("Status " & aMessage)
        End If
        Status(aMessage)
    End Sub

    ''' <summary>
    ''' Log the progress of a long-running task
    ''' </summary>
    ''' <param name="aCurrent">Current position/item of task</param>
    ''' <param name="aLast">Final position/item of task</param>
    ''' <remarks>
    ''' A final call when the task is done with aCurrent = aLast 
    ''' is desirable to clear the progress display.
    ''' If aLast = 100, then aCurrent is percent progress, but 
    ''' the caller should not convert aCurrent into percent if 
    ''' aLast is not 100.
    ''' </remarks>
    Public Shared Sub Progress(ByVal aCurrent As Integer, ByVal aLast As Integer)
        Try
            Dim lCurTime As Double = Now.ToOADate
            If aCurrent = aLast Then 'Reached end, stop showing progress bar
                pProgressStatus.Progress(aCurrent, aLast)
                pProgressStartTime = 0
                pProgressLastUpdate = 0
                Flush()
            ElseIf pProgressStartTime = 0 Then 'Starting new progress display
                pProgressStatus.Progress(aCurrent, aLast)
                pProgressStartTime = lCurTime
                pProgressLastUpdate = lCurTime
            ElseIf pProgressRefresh = 0 OrElse lCurTime - pProgressLastUpdate > pProgressRefresh Then
                'Long enough interval since last progress update
                pProgressStatus.Progress(aCurrent, aLast)
                Application.DoEvents() 'Allow user interaction such as minimizing window
                'Dbg("Progress " & aCurrent & " of " & aLast)
                Flush()
                pProgressLastUpdate = lCurTime
            End If
        Catch ex As Exception 'Ignore any exceptions while processing progress messages
        End Try
    End Sub

    ''' <summary>
    ''' Combination of Status(aMessage) and Progress(aCurrent, aLast)
    ''' </summary>
    ''' <param name="aMessage">Status message</param>
    ''' <param name="aCurrent">Current position/item of task</param>
    ''' <param name="aLast">Final position/item of task</param>
    ''' <remarks>It is preferred to use Status or Progress(aCurrent, aLast) if
    ''' only the functionality of one of them is desired.</remarks>
    Public Shared Sub Progress(ByVal aMessage As String, ByVal aCurrent As Integer, ByVal aLast As Integer)
        Status(aMessage)
        'Status(aMessage & "(" & aCurrent & " of " & aLast)
        Progress(aCurrent, aLast)
    End Sub

    ''' <summary>
    ''' Time in milliseconds that must elapse before a new progress update is passed to ProgressStatus
    ''' </summary>
    ''' <remarks>
    ''' A delay is desirable for tasks that make progress quickly because the
    ''' time to update displayed progress can become significant.
    ''' Default is 200 milliseconds so progress display is refreshed at most 5 times per second.
    ''' Set to zero to disable this feature.
    ''' </remarks>
    Public Shared Property ProgressRefresh() As Double
        Get
            Return pProgressRefresh * MillisecondsPerDay
        End Get
        Set(ByVal newValue As Double)
            pProgressRefresh = newValue / MillisecondsPerDay
        End Set
    End Property

    '''' <summary>
    '''' Set Busy = True to change to hourglass cursor, set Busy = False to return to normal cursor
    '''' </summary>
    '''' <returns>True if any code has set True and not yet set False</returns>
    '''' <remarks>Internally keeps track of nested busy status and only returns to normal after all busy callers have declared they are no longer busy</remarks>
    'Public Shared Property Busy() As Boolean
    '    Get
    '        If pBusy > 0 Then Return True Else Return False
    '    End Get
    '    Set(ByVal newValue As Boolean)
    '        If newValue Then
    '            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    '            pBusy += 1
    '            If pBusy = 1 Then 'We just became busy, so set the main cursor
    '                'pProgressStatus.Busy(True)
    '                'pMapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait '13
    '            End If
    '        Else
    '            If pBusy > 0 Then pBusy -= 1
    '            If pBusy = 0 Then 'Not busy any more, set cursor back to default
    '                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    '                'pProgressStatus.Busy(False)
    '                'pMapWin.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault '0
    '            End If
    '        End If
    '    End Set
    'End Property

    ''' <summary>Walks up the stack to find the method that called into this class</summary>
    ''' <returns>file name (if available) and method name of the method that called into this class</returns>
    ''' <remarks>stack is not available if not compiled in debug mode, so an empty string is returned</remarks>
    Private Shared Function MethodCallingLogger() As String
        Try
            Dim lStackTrace As New StackTrace(True)
            'Frame 0=this function, 1=a method in Logger since this is private, 2=first possible frame outside Logger
            Dim lModule As System.Reflection.Module = lStackTrace.GetFrame(0).GetMethod.Module
            Dim lFrameIndex As Integer = 2
            Dim lFrame As StackFrame = lStackTrace.GetFrame(lFrameIndex)

            'Walk up the stack until we are in a different module
            Try
                While lFrameIndex < lStackTrace.FrameCount AndAlso lFrame.GetMethod.Module.Equals(lModule)
                    lFrameIndex += 1
                    lFrame = lStackTrace.GetFrame(lFrameIndex)
                End While
            Catch e As Exception
                'could not go farther up stack
            End Try

            Dim lFrameFilename As String = lFrame.GetFileName
            If Not lFrameFilename Is Nothing Then
                Return MiscUtils.GetBaseName(lFrameFilename) & ":" & lFrame.GetMethod().Name
            Else
                Return lFrame.GetMethod().Name
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

#Region "StatusMonitor"
    'Methods for use with an external status monitor application which is not currently in use

    'Private Shared pStatusMonitorPID As Integer = -1

    'Public Shared Sub StartMonitor()
    '  If pStatusMonitorPID = -1 Then
    '    Dim exe As String = FindFile("Please locate StatusMonitor.exe", "StatusMonitor.exe")
    '    If exe.Length > 0 Then
    '      Dim lStartCmdLine As String = """" & exe & """ " & Process.GetCurrentProcess.Id & " " & pFileName
    '      Dbg("StartMonitor " & lStartCmdLine)
    '      Flush()
    '      pStatusMonitorPID = Shell(lStartCmdLine, AppWinStyle.NormalNoFocus)
    '    Else
    '      Dbg("StartMonitor Could not find StatusMonitor.exe")
    '    End If
    '  Else
    '    Dbg("LogStartMonitor - Already started PID " & pStatusMonitorPID)
    '  End If
    'End Sub

    'Public Shared Sub StopMonitor()
    '  If pStatusMonitorPID > -1 Then
    '    Dbg("EXIT")
    '    pStatusMonitorPID = 0
    '  End If
    'End Sub

    ''Send a command to the status monitor
    'Public Sub Cmd(ByVal aCommand As String)
    '  System.Diagnostics.Debug.WriteLine(aCommand)
    '  'If Not (gIPC Is Nothing) Then gIPC.SendMonitorMessage((aCommand))
    'End Sub
#End Region

End Class
