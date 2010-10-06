Public Interface IReport
    ReadOnly Property Body() As System.Text.StringBuilder
    ReadOnly Property MetaData() As System.Text.StringBuilder
    Sub AppendLine(Optional ByVal aString As String = "")
    Sub Append(Optional ByVal aString As String = "")
End Interface

Public Class ReportText
    Implements IReport

    Private pBody As New System.Text.StringBuilder
    Private pMetaData As New System.Text.StringBuilder

    Public Sub New(Optional ByVal aString As String = "")
        pBody.Append(aString)
        pMetaData.AppendLine("MetaData")
        pMetaData.AppendLine("User" & vbTab & Environment.UserName & vbTab & _
                             "Machine" & vbTab & Environment.MachineName & vbTab & _
                             "CLRVersion" & vbTab & Environment.Version.ToString & vbTab & _
                             "OSVersion" & vbTab & Environment.OSVersion.ToString & vbTab & _
                             "ProcessorCount" & vbTab & Environment.ProcessorCount.ToString)
        pMetaData.AppendLine(AssemblyMetadata(System.Reflection.Assembly.GetCallingAssembly))
    End Sub

    Public Function AssemblyMetadata(ByVal aAssembly As System.Reflection.Assembly) As String
        Dim lStackTrace As New StackTrace(False)
        Dim lFrameIndex As Integer = 1
        Dim lFrame As StackFrame = lStackTrace.GetFrame(lFrameIndex)
        While (lFrame.GetMethod.Name = ".ctor")
            lFrameIndex += 1
            lFrame = lStackTrace.GetFrame(lFrameIndex)
        End While
        Dim lString As String = "Assembly" & vbTab & aAssembly.Location & vbTab & _
                                "Version" & vbTab & System.Diagnostics.FileVersionInfo.GetVersionInfo(aAssembly.Location).FileVersion & vbTab & _
                                "Dated" & vbTab & System.IO.File.GetLastWriteTime(aAssembly.Location).ToString & vbTab & _
                                "CalledBy" & vbTab & lFrame.GetMethod.Name & vbTab & _
                                "In" & vbTab & lFrame.GetMethod.ReflectedType.FullName
        Return lString
    End Function

    ReadOnly Property Body() As System.Text.StringBuilder Implements IReport.Body
        Get
            Return pBody
        End Get
    End Property

    ReadOnly Property MetaData() As System.Text.StringBuilder Implements IReport.MetaData
        Get
            Return pMetaData
        End Get
    End Property

    Public Sub AppendLine(Optional ByVal aString As String = "") Implements IReport.AppendLine
        pBody.AppendLine(aString)
    End Sub

    Public Sub Append(Optional ByVal aString As String = "") Implements IReport.Append
        pBody.Append(aString)
    End Sub

    Public Overrides Function ToString() As String
        Return pBody.ToString & vbCrLf & pMetaData.ToString
    End Function
End Class
