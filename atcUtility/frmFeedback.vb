Imports MapWinUtility

Public Class frmFeedback
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents lblEnterAmessage As System.Windows.Forms.Label
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents txtSystemInformation As System.Windows.Forms.TextBox
    Friend WithEvents chkSendSystemInformation As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFeedback))
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.lblEnterAmessage = New System.Windows.Forms.Label
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.txtSystemInformation = New System.Windows.Forms.TextBox
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.chkSendSystemInformation = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 19)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(63, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Your Name:"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(128, 16)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(285, 20)
        Me.txtName.TabIndex = 1
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(128, 40)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(285, 20)
        Me.txtEmail.TabIndex = 3
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(12, 43)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(101, 13)
        Me.lblEmail.TabIndex = 2
        Me.lblEmail.Text = "Your Email Address:"
        '
        'lblEnterAmessage
        '
        Me.lblEnterAmessage.AutoSize = True
        Me.lblEnterAmessage.Location = New System.Drawing.Point(12, 80)
        Me.lblEnterAmessage.Name = "lblEnterAmessage"
        Me.lblEnterAmessage.Size = New System.Drawing.Size(382, 13)
        Me.lblEnterAmessage.TabIndex = 4
        Me.lblEnterAmessage.Text = "Please describe what happened so the developers can find and fix the problem:"
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(12, 99)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMessage.Size = New System.Drawing.Size(401, 104)
        Me.txtMessage.TabIndex = 5
        '
        'txtSystemInformation
        '
        Me.txtSystemInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSystemInformation.BackColor = System.Drawing.SystemColors.Control
        Me.txtSystemInformation.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemInformation.Location = New System.Drawing.Point(12, 232)
        Me.txtSystemInformation.Multiline = True
        Me.txtSystemInformation.Name = "txtSystemInformation"
        Me.txtSystemInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtSystemInformation.Size = New System.Drawing.Size(401, 99)
        Me.txtSystemInformation.TabIndex = 7
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSend.Location = New System.Drawing.Point(185, 337)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(72, 24)
        Me.btnSend.TabIndex = 8
        Me.btnSend.Text = "Send"
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(263, 337)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(72, 24)
        Me.btnCopy.TabIndex = 9
        Me.btnCopy.Text = "Copy"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(341, 337)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        '
        'chkSendSystemInformation
        '
        Me.chkSendSystemInformation.AutoSize = True
        Me.chkSendSystemInformation.Checked = True
        Me.chkSendSystemInformation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSendSystemInformation.Location = New System.Drawing.Point(12, 209)
        Me.chkSendSystemInformation.Name = "chkSendSystemInformation"
        Me.chkSendSystemInformation.Size = New System.Drawing.Size(295, 17)
        Me.chkSendSystemInformation.TabIndex = 6
        Me.chkSendSystemInformation.Text = "Send the following system information with your message:"
        '
        'frmFeedback
        '
        Me.AcceptButton = Me.btnSend
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(425, 373)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtSystemInformation)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.lblEnterAmessage)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.chkSendSystemInformation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFeedback"
        Me.Text = "Feedback"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pSend As Boolean = False

    Public Function ShowFeedback(ByRef aName As String, _
                                 ByRef aEmail As String, _
                                 ByRef aMessage As String, _
                                 ByRef aSystemInformation As String, _
                                 ByVal aAddGenericSystemInfo As Boolean, _
                                 ByVal aAddDebugInfo As Boolean, _
                                 ByVal aAddModuleInfo As Boolean, _
                                 ByVal aAddFilesInFolder As String) As Boolean
        If aName IsNot Nothing AndAlso aName.Length > 0 Then
            txtName.Text = aName
        Else
            txtName.Text = GetSetting("BASINS4", "Feedback", "Name", "")
        End If

        If aEmail IsNot Nothing AndAlso aEmail.Length > 0 Then
            txtEmail.Text = aEmail
        Else
            txtEmail.Text = GetSetting("BASINS4", "Feedback", "Email", "")
        End If

        If aMessage IsNot Nothing Then txtMessage.Text = aMessage
        If aSystemInformation Is Nothing Then
            aSystemInformation = ""
        End If
        txtSystemInformation.Text = aSystemInformation
        Me.Show()
        Me.Refresh()
        Windows.Forms.Application.DoEvents()

        If aAddGenericSystemInfo Then
            txtSystemInformation.Text &= GetSystemInfo()
            txtSystemInformation.Refresh()
            Windows.Forms.Application.DoEvents()
        End If

        If aAddDebugInfo Then
            txtSystemInformation.Text &= GetDebugInfo()
            Windows.Forms.Application.DoEvents()
        End If

        If aAddModuleInfo Then
            txtSystemInformation.Text &= GetModuleInfo()
            Windows.Forms.Application.DoEvents()
        End If

        If FileExists(aAddFilesInFolder, True, False) Then
            txtSystemInformation.Text &= ReportFilesInDir(aAddFilesInFolder, True)
            Windows.Forms.Application.DoEvents()
        End If

        While Me.Visible
            System.Threading.Thread.Sleep(50)
            Windows.Forms.Application.DoEvents()
        End While

        If pSend Then
            aName = txtName.Text
            aEmail = txtEmail.Text
            aMessage = txtMessage.Text
            If chkSendSystemInformation.Checked Then
                aSystemInformation = txtSystemInformation.Text
            Else
                aSystemInformation = "System information not sent"
            End If
        End If
        Me.Close()
        Return pSend
    End Function

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim lFeedback As String = "Name: " & txtName.Text & vbCrLf
        lFeedback &= "Email: " & txtEmail.Text & vbCrLf
        lFeedback &= "Message: " & txtMessage.Text & vbCrLf
        lFeedback &= vbCrLf & txtSystemInformation.Text & vbCrLf
        Windows.Forms.Clipboard.SetDataObject(lFeedback)
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        SaveSetting("BASINS4", "Feedback", "Name", txtName.Text)
        SaveSetting("BASINS4", "Feedback", "Email", txtEmail.Text)
        pSend = True
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pSend = False
        Me.Hide()
    End Sub

    Public Function GetSystemInfo() As String
        Dim lFeedback As String = "Feedback at " & Now.ToString("u") & vbCrLf _
            & "User: " & Environment.UserName & vbCrLf _
            & "Machine: " & Environment.MachineName & vbCrLf _
            & "CLRVersion: " & Environment.Version.ToString & vbCrLf _
            & "LogFile: " & Logger.FileName & vbCrLf

        If IO.File.Exists(Logger.FileName) Then
            Try
                lFeedback &= WholeFileString(Logger.FileName) & vbCrLf
            Catch e As Exception
                lFeedback &= vbCrLf & "Logger file read failed, exception message:" & _
                             vbCrLf & e.ToString & vbCrLf & vbCrLf
            End Try
        End If

        Return lFeedback & "___________________________" & vbCrLf
    End Function

    'Verbose out put of process information, CPU info, memory info,
    'environment variables, etc.
    Public Shared Function GetDebugInfo() As String
        Dim lInfo As New System.Text.StringBuilder

        Try
            lInfo.AppendLine("atcUtility Assembly Version: " + Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString() + "." + Environment.Version.Revision.ToString() + "." + Environment.Version.Build.ToString())
            lInfo.AppendLine("Operating System: " + Environment.OSVersion.Platform.ToString())
            lInfo.AppendLine("Service Pack: " + Environment.OSVersion.ServicePack())
            lInfo.AppendLine("Major Version:	" + Environment.OSVersion.Version.Major.ToString())
            lInfo.AppendLine("Minor Version:	" + Environment.OSVersion.Version.Minor.ToString())
            lInfo.AppendLine("Revision:		" + Environment.OSVersion.Version.MajorRevision.ToString())
            lInfo.AppendLine("Build:		" + Environment.OSVersion.Version.Build.ToString())
            lInfo.AppendLine()
            lInfo.AppendLine("-------------------------------------------------")
            lInfo.Append("Logical Drives: ")
            For Each s As String In Environment.GetLogicalDrives()
                lInfo.Append(s & " ")
            Next
            lInfo.AppendLine()
            lInfo.AppendLine("System Directory: " + Environment.SystemDirectory)
            lInfo.AppendLine("Current Directory: " + Environment.CurrentDirectory)
            lInfo.AppendLine("Command Line: " + Environment.CommandLine)
            lInfo.AppendLine("Command Line Args: ")
            For Each s As String In Environment.GetCommandLineArgs
                lInfo.AppendLine(" " & s)
            Next
            lInfo.AppendLine()
            lInfo.AppendLine()
            lInfo.AppendLine("------------Environment Variables-----------------")
            For Each lVariable As DictionaryEntry In Environment.GetEnvironmentVariables
                Dim lVariableKey As String = lVariable.Key.ToString
                If lVariableKey.ToUpper = "PATH" Then
                    lInfo.AppendLine(lVariableKey & " =" & vbCrLf & " " & lVariable.Value.ToString.Replace(";", ";" & vbCrLf & " "))
                Else
                    lInfo.AppendLine(lVariableKey & " = " & lVariable.Value.ToString)
                End If
            Next
            lInfo.AppendLine()
            lInfo.AppendLine("------------Performance Info (Bytes)--------------")

            Dim currentProc As Process = Process.GetCurrentProcess()
            With currentProc
                .Refresh()
                lInfo.AppendLine("Private Memory:  " & .PrivateMemorySize64.ToString())
                lInfo.AppendLine("Virtual Memory:  " & .VirtualMemorySize64.ToString())
                lInfo.AppendLine("Total CPU time: " & .TotalProcessorTime.ToString())
                lInfo.AppendLine("Total User Mode CPU time: " & .UserProcessorTime.ToString())
                lInfo.AppendLine()
            End With
        Catch e As Exception
            lInfo.AppendLine("Exception in GetDebugInfo: " & e.Message)
        End Try
        lInfo.AppendLine("------------End Debug Info------------------------")

        Return lInfo.ToString()
    End Function

    Public Function GetModuleInfo() As String
        Dim lInfo As New System.Text.StringBuilder
        lInfo.AppendLine("------------Module Info:--------------------------")
        Try
            Dim currentProc As Process = Process.GetCurrentProcess()
            currentProc.Refresh()
            Dim myProcessModuleCollection As ProcessModuleCollection = currentProc.Modules
            Dim myProcessModule As ProcessModule
            For Each myProcessModule In myProcessModuleCollection
                Try
                    Windows.Forms.Application.DoEvents()
                    lInfo.Append("----Module Name:  ").AppendLine(myProcessModule.ModuleName)
                    lInfo.Append("    Path:  ").AppendLine(myProcessModule.FileName)
                    If myProcessModule.FileVersionInfo.FileVersion IsNot Nothing Then
                        lInfo.Append("    Version: ").AppendLine(myProcessModule.FileVersionInfo.FileVersion.ToString())
                    End If
                Catch
                End Try
            Next myProcessModule

            lInfo.AppendLine()
        Catch e As Exception
            lInfo.AppendLine("Exception in GetModuleInfo: " & e.Message)
        End Try
        lInfo.AppendLine("------------End Module Info------------------------")

        Return lInfo.ToString()
    End Function
End Class
