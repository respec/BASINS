Option Strict Off
Option Explicit On
Module FtnIO
    '##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Private LastLenRd As Integer
    Private LastLenWr As Integer
    Private LastHdrLen As Integer

    Function FtnUnfSeqInitRd(ByRef n As String) As Integer
        Dim f As Integer
        Dim b As Byte

        f = FreeFile()
        FileOpen(f, n, OpenMode.Binary, OpenAccess.Read)
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(f, b)
        If b <> &HFDS Then 'lahey code for FtnUnfSeq (UserManual-App A)
            FileClose(f)
            f = -1
        End If
        LastLenRd = 0

        FtnUnfSeqInitRd = f

    End Function

    Function FtnUnfSeqInitWr(ByRef n As String) As Integer
        Dim f As Integer
        Dim b As Byte

        f = FreeFile()
        FileOpen(f, n, OpenMode.Binary, OpenAccess.Write)
        'put hex header at top of file
        b = &HFDS
        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(f, b)
        LastLenWr = 0
        LastHdrLen = 0

        FtnUnfSeqInitWr = f

    End Function

    Sub FtnUnfSeqReset()
        LastLenRd = 0
    End Sub

    Function FtnUnfSeqRecLen(ByRef f As Short) As Integer
        Dim b As Byte
        Dim RecLen, c As Integer
        Dim bytes As Short

        If LastLenRd > 0 Then 'flush the trailing record length
            c = 64
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(f, b)
            While LastLenRd > c
                c = c * 256
                'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FileGet(f, b)
            End While
        End If
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(f, b) 'now the record length for this record
        bytes = b And 3
        RecLen = (b - bytes) \ 4 'truncate
        c = 64
        Do While bytes > 0
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(f, b)
            bytes = bytes - 1
            RecLen = RecLen + b * c
            c = c * 256
        Loop
        LastLenRd = RecLen
        FtnUnfSeqRecLen = RecLen

    End Function

    Sub FtnUnfSeqWrHead(ByRef f As Short, ByRef l As Integer)
        Dim b As Byte
        Dim x As Integer
        Dim c, i As Integer
        Dim r As Short

        r = 0
        If LastLenWr > 0 Then 'finish trailing record length
            If LastLenWr >= &H400000 Then 'will need 4 bytes
                r = r + 1
                c = LastLenWr \ &H100S 'truncate
                b = c
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(f, b)
                LastLenWr = LastLenWr - (c * &H100S)
            End If
            If LastLenWr >= &H4000S Or r > 0 Then 'at least 3 bytes
                r = r + 1
                c = LastLenWr \ &H100S 'truncate
                b = c
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(f, b)
                LastLenWr = LastLenWr - (c * &H100S)
            End If
            If LastLenWr >= &H40S Or r > 0 Then 'at least 2 bytes
                r = r + 1
                c = LastLenWr \ &H40S 'truncate
                b = c
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(f, b)
                LastLenWr = LastLenWr - (c * &H40S)
            End If
            If LastLenWr > 0 Or r > 0 Then 'at least 1 byte
                b = LastLenWr * 4 + r
                LastLenWr = 0
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(f, b)
            End If
        End If

        If l > 0 Then
            If l >= &H400000 Then
                r = 3
            ElseIf l >= &H4000S Then
                r = 2
            ElseIf l >= &H40S Then
                r = 1
            Else
                r = 0
            End If
            LastLenWr = l + r + 1
            c = l Mod &H40S
            x = (l - c) \ &H40S 'truncate
            b = c * 4 + r
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(f, b)
            For i = 1 To r Step 1
                c = x Mod &H100S
                x = (x - c) \ &H100S 'truncate
                b = c
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(f, b)
            Next i
        End If
    End Sub

    Sub FtnUnfSeqWrStr(ByRef f As Object, ByRef l As Object, ByRef s As Object)
        Dim b As Byte
        Dim lc, ls As Integer

        ls = Len(s)

        'UPGRADE_WARNING: Couldn't resolve default property of object l. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        For lc = 1 To l
            If lc > ls Then
                b = &H20S 'pad with blank
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object s. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                b = Asc(Mid(s, lc, 1))
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object f. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(f, b)
        Next lc
    End Sub

    Public Function FtnUnFmtClean(ByRef b() As Byte) As Byte()
        Dim c() As Byte
        Dim iOu, l, iIn, i As Integer
        Dim r As Object
        Dim first As Boolean

        If b(0) = &HFDS Then 'this is a lahey unformatted byte stream
            l = UBound(b)
            ReDim c(l)
            iIn = 1
            iOu = 0
            first = True
            While iIn < l
                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r = FtnUnfSeqRecLenMem(b, iIn, first)
                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                For i = 1 To r
                    c(iOu) = b(iIn)
                    iOu = iOu + 1
                    iIn = iIn + 1
                Next i
            End While
            ReDim Preserve c(iOu - 1)
            Return c
        Else
            Return b
        End If
    End Function

    Private Function FtnUnfSeqRecLenMem(ByRef b() As Byte, ByRef ind As Integer, ByRef first As Boolean) As Integer
        Dim RecLen, c As Integer
        Dim bytes As Short
        Static LastLen As Integer

        If first Then
            LastLen = 0
            first = False
        Else
            c = 64
            ind = ind + 1
            While LastLen > c
                c = c * 256
                ind = ind + 1
            End While
        End If
        If ind > UBound(b) Then 'all done
            RecLen = 1
        Else
            bytes = b(ind) And 3
            RecLen = b(ind) / 4
            ind = ind + 1
            c = 64
            Do While bytes > 0
                bytes = bytes - 1
                RecLen = RecLen + b(ind) * c
                ind = ind + 1
                c = c * 256
            Loop
        End If
        LastLen = RecLen - 1
        FtnUnfSeqRecLenMem = RecLen - 1
    End Function
End Module
