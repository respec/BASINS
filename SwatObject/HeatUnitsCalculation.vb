Imports System.IO
''' <summary>
''' 
''' </summary>
''' <remarks>from OpenSWAT BASINS PlugIN - DO NOT MODIFY</remarks>
Module HeatUnitsCalculation
    Structure node
        Public num, itil As Integer
        Public cname As String
        Public to1, tb, daym() As Single
    End Structure

    Public Const MAX_CROP As Integer = 80

    Public list(MAX_CROP) As node
    Public mon() As Integer = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    Public nb() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}
    Public alat, phux, phuc, tmax(12), tmin(12), phu1(MAX_CROP)(), phus(MAX_CROP)() As Single
    Public buf As String
    Public ivar(MAX_CROP), iplant(MAX_CROP)() As Integer

    Public Function calHeatUnits(ByVal cropName As String, ByVal xCoor As Single, ByVal yCoor As Single, ByVal dataDirctory As String) As Single
        'Dim argv As String() = Environment.GetCommandLineArgs()


        Dim i, j, k As Integer
        Dim phutot, l, inx, iny As Single
        Dim datadir As String
        Dim cname As String = Trim(cropName)

        'Required to initialize second dimension of jagged array
        For i = 0 To MAX_CROP - 1
            phu1(i) = New Single(5) {}
            phus(i) = New Single(5) {}
            iplant(i) = New Integer(5) {}
            list(i).daym = New Single(5) {}
        Next

        inx = CType(xCoor, Single)
        iny = CType(yCoor, Single)

        For i = 0 To MAX_CROP - 1
            For j = 0 To 4
                phu1(i)(j) = 0.0
                phus(i)(j) = 0.0
                iplant(i)(j) = 0
            Next

            ivar(i) = 0
        Next

        datadir = String.Copy(Trim(dataDirctory))


        If Not datadir.EndsWith("\") Then
            datadir &= "\"
        End If

        'cname = "frsd"
        'datadir = "c:\phu\weather\"
        'inx = 40.5
        'iny = 87.2

        getlist(datadir.Trim())
        gettemp(datadir.Trim(), inx, iny)

        alat = inx
        phux = 500 - (alat - 22) * 15
        For i = 0 To MAX_CROP - 1
            For j = 0 To 4
                If list(i).daym(j) > 0.0 Then
                    proc1(phux, i, j)
                    k = iplant(i)(j) + CType(list(i).daym(j), Integer)
                    phu1(i)(j) = ahu(iplant(i)(j), k, i)
                    If list(i).daym(j) <= 0.0 Then
                        phu1(i)(j) = 0.0
                    End If
                    If list(i).itil = 2 Then
                        proc2(i, j)
                    End If
                End If
            Next
        Next

        For i = 0 To MAX_CROP - 1
            For j = 0 To 4
                If phu1(i)(j) <= 0.0 Then
                    If (ivar(i) = 0) And (phu1(i)(0) > 0.0) Then
                        ivar(i) = j
                    End If
                    j = 0
                    Exit For
                End If
                If list(i).itil = 1 Then
                    phuc = ahu(iplant(i)(j), 365, i)
                Else
                    phuc = ahu(0, 365, i)
                End If
                phuc /= 1.2
                If phuc > phu1(i)(j) Then
                    If phu1(i)(j) <= 0.0 Then
                        ivar(i) = j
                    Else
                        ivar(i) = j + 1
                    End If
                    j = 0
                    Exit For
                End If
            Next
        Next

        'list(0).to1 = 100.0
        'list(0).tb = 0.0
        phutot = ahu(0, 365, 0)
        For i = 0 To MAX_CROP - 1
            'list(i).to1 = 100.0
            'list(i).tb = 0.0

            For j = 0 To 4
                phus(i)(j) = ahu(j, iplant(i)(j), i) / phutot
            Next
        Next

        For i = 0 To MAX_CROP - 1
            If String.Compare(list(i).cname, cname) = 0 Then
                If ivar(i) > 0 Then
                    l = phu1(i)(ivar(i) - 1)
                Else
                    Console.WriteLine("Negative index.")
                    l = 0
                End If

                'Console.WriteLine("{0} {1} {2} {3} {4} {5}", list(i).num, list(i).cname, list(i).itil, list(i).to1, list(i).tb, l)
                Return l


                Exit For
            End If
        Next

    End Function

    Sub proc1(ByVal a As Single, ByVal b As Integer, ByVal c As Integer)
        Dim phuc1 As Single = 0.0
        Dim ta, phux1 As Single
        Dim i, j As Integer
        Dim exiting As Boolean

        phux1 = a
        For i = 0 To 11
            For j = 0 To mon(i) - 1
                ta = ((tmax(i) + tmin(i)) / 2.0) - list(b).tb
                If ta > 0.0 Then
                    phuc1 += ta
                End If
                If phuc1 > phux1 Then
                    iplant(b)(c) = jdt(j, i)
                    exiting = True
                    Exit For
                End If
            Next
            If exiting Then Exit For
        Next
    End Sub

    Sub proc2(ByVal b As Integer, ByVal c As Integer)
        Dim phuc1 As Single = 0.0
        Dim ta, phux1 As Single
        Dim i, j, mo As Integer
        Dim exiting As Boolean

        phux1 = phu1(b)(c) / 2.0
        For i = 0 To 11
            mo = 11 - i
            For j = 0 To mon(mo) - 1
                ta = ((tmax(mo) + tmin(mo)) / 2.0) - list(b).tb
                If ta > 0.0 Then
                    phuc1 += ta
                End If
                If phuc1 > phux1 Then
                    j = mon(mo) - j - 2
                    iplant(b)(c) = jdt(j, mo)
                    exiting = True
                    Exit For
                End If
            Next
            If exiting Then Exit For
        Next
    End Sub

    Function xmonth(ByVal a As Integer) As Integer
        If a < 32 Then
            Return 0
        ElseIf a < 61 Then
            Return 1
        ElseIf a < 92 Then
            Return 2
        ElseIf a < 122 Then
            Return 3
        ElseIf a < 153 Then
            Return 4
        ElseIf a < 183 Then
            Return 5
        ElseIf a < 214 Then
            Return 6
        ElseIf a < 245 Then
            Return 7
        ElseIf a < 275 Then
            Return 8
        ElseIf a < 306 Then
            Return 9
        ElseIf a < 336 Then
            Return 10
        Else
            Return 11
        End If
    End Function

    Function ahu(ByVal a As Integer, ByVal b As Integer, ByVal c As Integer) As Single
        Dim ahu1 As Single = 0.0
        Dim ta, humx As Single
        Dim i, mon As Integer

        For i = a To b
            mon = xmonth(i)
            ta = ((tmax(mon) + tmin(mon)) / 2) - list(c).tb
            If ta > 0.0 Then
                humx = list(c).to1 - list(c).tb
                If ta > humx Then
                    ta = humx
                End If
                ahu1 += ta
            End If
        Next

        Return ahu1
    End Function

    Function jdt(ByVal a As Integer, ByVal b As Integer) As Integer
        If b < 1 Then
            Return nb(b) + a
        Else
            Return nb(b) + a + 1
        End If
    End Function

    Sub getlist(ByVal str As String)
        Dim i, j As Integer

        Try
            Dim reader As StreamReader = File.OpenText(str & "phucrp.dat")
            Dim line As String

            For i = 0 To MAX_CROP - 1
                line = reader.ReadLine()

                If line.Trim().Length > 2 Then
                    list(i).num = CType(line.Substring(0, 2).Trim(), Integer)
                    If line.Trim().Length >= 8 Then
                        list(i).cname = CType(line.Substring(4, 4).Trim(), String)
                    Else
                        list(i).cname = CType(line.Substring(4, 3).Trim(), String)
                    End If

                    If line.Trim().Length > 8 Then
                        list(i).itil = CType(line.Substring(12, 6).Trim(), Integer)
                        list(i).to1 = CType(line.Substring(16, 8).Trim(), Single)
                        list(i).tb = CType(line.Substring(24, 8).Trim(), Single)
                    End If

                    For j = 0 To 4
                        If line.Trim().Length > (j * 8 + 32) Then
                            list(i).daym(j) = CType(line.Substring(32 + (j * 8), 8).Trim(), Single)
                        End If
                    Next
                End If

            Next

            reader.Close()
        Catch ex As Exception
            Console.WriteLine("ERROR: {0}", ex.Message)
        End Try
    End Sub

    Sub gettemp(ByVal str As String, ByRef a As Single, ByRef b As Single)
        Dim count As Integer = -1
        Dim list1(100)() As Single
        Dim i, j As Integer

        'Required to initialize second dimension of jagged array
        For i = 0 To list1.GetLength(0) - 1
            list1(i) = New Single(3) {}
        Next

        Try
            Dim reader As StreamReader = File.OpenText(str & "weather.lis")
            Dim line As String = reader.ReadLine()

            While Not line Is Nothing
                Dim x As Single = CType(line.Substring(23, 5).Trim(), Single)
                Dim y As Single = CType(line.Substring(30, 6).Trim(), Single)

                'If x.Equals(CSng(33.45)) Then
                '    MsgBox(y)
                'End If

                If (Math.Abs(x - CSng(a)) < 2) And (Math.Abs(y - CSng(b)) < 2) Then
                    count += 1
                    list1(count)(0) = x
                    list1(count)(1) = y
                End If

                line = reader.ReadLine()
            End While

            reader.Close()
        Catch ex As Exception
            Console.WriteLine("ERROR: {0}", ex.Message)
        End Try

        If count = -1 Then
            Console.WriteLine("Could not find station in lati. {0} and long. {1}", a, b)
            Return
        End If

        'Calculate the value of distance from given point
        For i = 0 To count - 1
            Dim x1, alat, y1, num As Double

            x1 = (list1(i)(0) - a) * 69.1
            alat = (list1(i)(0) + a) / 2
            y1 = 81.82 - (0.729 * alat)
            y1 = (list1(i)(1) - b) * y1
            num = (x1 * x1) + (y1 * y1)
            list1(i)(2) = CType(Math.Sqrt(num), Single)
        Next

        'Sort each point by smaller value of distance
        For i = 0 To count - 2
            For j = i + 1 To count - 1
                If list1(i)(2) > list1(j)(2) Then
                    Dim temp0, temp1, temp2 As Single

                    temp0 = list1(i)(0)
                    temp1 = list1(i)(1)
                    temp2 = list1(i)(2)
                    list1(i)(0) = list1(j)(0)
                    list1(i)(1) = list1(j)(1)
                    list1(i)(2) = list1(j)(2)
                    list1(j)(0) = temp0
                    list1(j)(1) = temp1
                    list1(j)(2) = temp2
                End If
            Next
        Next

        Try
            Dim reader As StreamReader = File.OpenText(str & "weather.lis")
            Dim line As String = reader.ReadLine()

            While Not line Is Nothing
                Dim x As Single = CType(line.Substring(23, 5).Trim(), Single)
                Dim y As Single = CType(line.Substring(30, 6).Trim(), Single)

                If (Math.Abs(x - list1(0)(0)) <= 0.001) And (Math.Abs(y - list1(0)(1)) <= 0.001) Then
                    Dim stationNumber As Integer = CType(line.Substring(44, 8).Trim(), Integer)
                    Dim fileName As String = line.Substring(52, 10).Trim()

                    'Console.WriteLine("Station: {0}, File: {1}", stationNumber, fileName)
                    settempdata(stationNumber, str & fileName)
                    Exit While
                End If

                line = reader.ReadLine()
            End While

            reader.Close()
        Catch ex As Exception
            Console.WriteLine("ERROR: {0}", ex.Message)
        End Try
    End Sub

    Sub settempdata(ByVal stationNumber As Integer, ByVal filePath As String)
        Try
            Dim reader As StreamReader = File.OpenText(filePath)
            Dim line As String = reader.ReadLine()
            Dim i As Integer

            For i = 1 To (stationNumber - 1) * 15 + 2
                line = reader.ReadLine()
            Next

            For i = 0 To 11
                tmax(i) = CType(line.Substring(i * 6, 6).Trim(), Single)
            Next

            line = reader.ReadLine()

            For i = 0 To 11
                tmin(i) = CType(line.Substring(i * 6, 6).Trim(), Single)
            Next

            reader.Close()
        Catch ex As Exception
            Console.WriteLine("ERROR: {0}", ex.Message)
        End Try
    End Sub
End Module
