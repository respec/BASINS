Imports RDotNet

Module modR
    Dim pEngine As REngine = Nothing

    Public Sub Main()
        If My.Application.CommandLineArgs.Count > 1 Then
            Try
                Dim lMsg As String = ""
                Dim lEngine As REngine = GetEngine(lMsg)
                If lEngine IsNot Nothing Then
                    Dim lResult As String() = {"No Result"}
                    Dim lResultFilename As String = My.Application.CommandLineArgs(0)
                    For Each lArg As String In My.Application.CommandLineArgs
                        If lArg <> lResultFilename Then
                            If IO.File.Exists(lArg) Then
                                lResult = lEngine.Evaluate(System.IO.File.ReadAllText(lArg)).AsCharacter().ToArray()
                            Else
                                MsgBox("R script file not found" & vbCrLf & lArg, MsgBoxStyle.Critical, "Run R")
                            End If
                        End If
                    Next

                    'lEngine.Evaluate(txtR.Text)

                    'SetIntegerVectorFromString(lEngine, "InputYears", txtYears.Text)
                    'SetDoubleVectorFromString(lEngine, "InputValues", txtValues.Text)
                    'Dim lConfidencePercent As Double = 95
                    'Double.TryParse(txtConfidence.Text, lConfidencePercent)
                    'lEngine.SetSymbol("InputPercentConfidence", lEngine.CreateNumeric(lConfidencePercent))
                    'lEngine.SetSymbol("InputWhat", lEngine.CreateInteger(ComboResult.SelectedIndex))
                    'Dim lResult As String() = lEngine.Evaluate("fnGetTrend(InputYears, InputValues, InputPercentConfidence, InputWhat)").AsCharacter().ToArray()
                    'Dim lResult As String() = lEngine.Evaluate("InputYears").AsCharacter().ToArray()
                    IO.File.WriteAllLines(lResultFilename, lResult)
                    'MsgBox(String.Join(Environment.NewLine, lResult), MsgBoxStyle.OkOnly, "Result")
                ElseIf lEngine Is Nothing OrElse Not String.IsNullOrEmpty(lMsg) Then
                    Dim lResult As String() = {"No R"}
                    If Not String.IsNullOrEmpty(lMsg) Then
                        lResult(0) = lMsg
                    End If
                    Dim lResultFilename As String = My.Application.CommandLineArgs(0)
                    IO.File.WriteAllLines(lResultFilename, lResult)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical, "Error running R")
            End Try
        End If
    End Sub

    Public Function GetEngine(ByRef aMsg As String) As REngine
        If pEngine Is Nothing Then
TryGetInstance:
            Try
                pEngine = REngine.GetInstance()
            Catch ex As Exception
                If ex.Message.Contains("Windows Registry key 'SOFTWARE\R-core' not found") Then
                    If MsgBox("R was not found. Download R now?", MsgBoxStyle.YesNo, "Could not start R engine") = MsgBoxResult.Yes Then
                        OpenFile("http://rweb.crmda.ku.edu/cran/bin/windows/base/")
                        If MsgBox("Retry after installing R, or Cancel", MsgBoxStyle.RetryCancel) = MsgBoxResult.Retry Then
                            GoTo TryGetInstance
                        End If
                    Else
                        aMsg = "No R: not installed"
                    End If
                Else
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Could not start R engine")
                    aMsg = "No R: couldnot start"
                End If
            End Try
        End If
        Return pEngine
    End Function

    Public Sub DisposeEngine()
        If pEngine IsNot Nothing Then
            Try
                pEngine.Dispose()
            Catch
            End Try
            pEngine = Nothing
        End If
    End Sub

    Public Sub SetIntegerVectorFromString(ByVal aEngine As REngine, ByVal aName As String, ByVal aValues As String)
        Dim lList As New List(Of Integer)
        For Each lNumberText As String In StringToList(aValues)
            Dim lNumber As Integer
            If Integer.TryParse(lNumberText, lNumber) Then
                lList.Add(lNumber)
            End If
        Next
        aEngine.SetSymbol(aName, aEngine.CreateIntegerVector(lList))
    End Sub

    Public Sub SetDoubleVectorFromString(ByVal aEngine As REngine, ByVal aName As String, ByVal aValues As String)
        Dim lList As New List(Of Double)
        For Each lNumberText As String In StringToList(aValues)
            Dim lNumber As Double
            If Double.TryParse(lNumberText, lNumber) Then
                lList.Add(lNumber)
            End If
        Next
        aEngine.SetSymbol(aName, aEngine.CreateNumericVector(lList))
    End Sub

    Private Function StringToList(ByVal aValues As String) As String()
        aValues = aValues.Replace(", ", ",")
        aValues = aValues.Replace(" ", ",")
        While aValues.Contains(",,")
            aValues = aValues.Replace(",,", ",")
        End While
        Return aValues.Split(",")
    End Function

    'Open a file using the default method the system would have used if it was double-clicked
    Public Sub OpenFile(ByVal FileOrURL As String, Optional ByVal Wait As Boolean = False)
        Dim newProcess As New Process
        Try
            If FileOrURL <> "" Then
                'Use a .NET process() to launch the file or URL
                newProcess.StartInfo.FileName = FileOrURL
                newProcess.Start()
                If Wait Then
                    newProcess.WaitForExit()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Module
