Imports atcData
Imports atcUtility
Imports System.IO

Public Class clsMRC
    Public Station As String
    Public DrainageArea As Double
    Public MaxLogQ As Double
    Public MinLogQ As Double
    Public MaxLength As Integer = 50
    Public CoeffA As Double
    Public CoeffB As Double
    Public CoeffC As Double

    Public Season As String

    Public TimeOrdinal(MaxLength) As Double
    Public LogQ(MaxLength) As Double

    Public CurveData As atcTimeseries

    Public FileCurvOut As String

    Private pRecSum As String
    Public Property RecSum() As String
        Get
            Return pRecSum
        End Get
        Set(ByVal value As String)
            If value.Length > 80 Then
                pRecSum = value
            End If
        End Set
    End Property

    Public ReadOnly Property Equation() As String
        Get
            Dim lStrCoA As String = String.Format("{0:0.00}", CoeffA)
            Dim lStrCoB As String = String.Format("{0:0.00}", CoeffB)
            Dim lStrCoC As String = String.Format("{0:0.00}", CoeffC)
            Return lStrCoA & " * LogQ^2 + " & lStrCoB & " * LogQ + " & lStrCoC
        End Get
    End Property
    Public Function BuildMRC() As Boolean

        Try
            If pRecSum IsNot Nothing AndAlso pRecSum.Length > 12 Then
                Dim lArr() As String = pRecSum.Split("\s+")
                Station = lArr(0)
                Season = lArr(1)
                Dim lDuration As String = lArr(2)
                MaxLogQ = lArr(8)
                MinLogQ = lArr(7)
                CoeffA = lArr(9)
                CoeffB = lArr(10)
                CoeffC = lArr(11)
            Else
                If Station Is Nothing OrElse Station = "" OrElse Season Is Nothing OrElse Season = "" OrElse _
                   (MaxLogQ = 0 AndAlso CoeffA = 0 AndAlso CoeffB = 0 AndAlso CoeffC = 0) Then
                    'Things are just not initialized, so quit
                    Return False
                End If
            End If
            Dim lDeltaLogQ As Double = (MaxLogQ - MinLogQ) / (MaxLength - 1)

            Dim lLogQ As Double = MaxLogQ
            For I As Integer = 1 To MaxLength
                TimeOrdinal(I) = CoeffA * (lLogQ ^ 2) + CoeffB * lLogQ + CoeffC
                LogQ(I) = lLogQ
                lLogQ -= lDeltaLogQ
            Next

            If CurveData IsNot Nothing Then
                CurveData.Clear() : CurveData = Nothing
            End If

            CurveData = New atcTimeseries(Nothing)
            With CurveData
                .Dates = New atcTimeseries(Nothing)
                .Dates.Values = TimeOrdinal
                .Values = LogQ
                '.SetInterval(atcTimeUnit.TUDay, 1)
            End With
            With CurveData.Attributes
                .SetValue("YAxis", "LEFT")
                .SetValue("Location", Station)
                .SetValue("Constituent", "")
                .SetValue("Scenario", Equation)
                .SetValue("Point", False)
                .SetValue("StepType", "nonstep")
                .SetValue("SJDay", TimeOrdinal(1))
                .SetValue("EJDay", TimeOrdinal(TimeOrdinal.Length - 1))
            End With

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function WriteCurvOutput() As Boolean

        If DrainageArea <= 0 Then
            Return False
        End If

        If Not Directory.Exists(Path.GetDirectoryName(FileCurvOut)) Then
            Return False
        End If

        Try
            Dim lAppend As Boolean = (File.Exists(FileCurvOut))
            Dim lSW As StreamWriter = New StreamWriter(FileCurvOut, lAppend)

            If Not lAppend Then
                lSW.WriteLine("T is time in days.")
                lSW.WriteLine("LogQcfs is the log of flow in CFS. ")
                lSW.WriteLine("LogQcfsmi is the log of flow in CFS per sq mile. ")
                lSW.WriteLine("Qcfs is the flow in CFS. ")
                lSW.WriteLine("Qcfsmi is the flow in CFS per square mile. ")
                lSW.WriteLine("-------------------------------------------------")
                lSW.WriteLine(" ")
                lSW.WriteLine("         T            LogQcfs       LogQcfsmi      .     Qcfs           Qcfsmi")
            End If

            lSW.WriteLine(" ")
            lSW.WriteLine(Station & "    " & Season)

            Dim lFieldLength As Integer = 15
            Dim lStrT As String
            Dim lStrLogQ As String
            Dim lStrLogQsmi As String
            Dim lStrQ As String
            Dim lStrQsmi As String
            Dim lFlowQ As Double

            For I As Integer = 1 To MaxLength
                lStrT = String.Format("{0.00000}", TimeOrdinal(I)).PadLeft(lFieldLength, " ")
                lStrLogQ = String.Format("{0.00000}", LogQ(I)).PadLeft(lFieldLength, " ")

                lFlowQ = 10 ^ LogQ(I)
                lStrLogQsmi = String.Format("{0.00000}", Math.Log10(lFlowQ / DrainageArea)).PadLeft(lFieldLength, " ")
                lStrQ = String.Format("{0.00000}", lFlowQ).PadLeft(lFieldLength, " ")
                lStrQsmi = String.Format("{0.00000}", lFlowQ / DrainageArea).PadLeft(lFieldLength, " ")

                lSW.WriteLine(lStrT & lStrLogQ & lStrLogQsmi & lStrQ & lStrQsmi)
            Next

            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            Return True
        Catch ex As Exception
            Return False
        End Try
        
    End Function

    Public Sub Clear()
        ReDim LogQ(0)
        ReDim TimeOrdinal(0)
        If CurveData IsNot Nothing Then CurveData.Clear()
    End Sub
End Class
