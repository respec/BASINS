Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMonthData_NET.HspfMonthData")> Public Class HspfMonthData
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pMonthDataTables As Collection 'of HspfMonthDataTable
    Private pUci As HspfUci
    Private pComment As String

    ReadOnly Property Caption() As String
        Get
            Caption = "Month Data"
        End Get
    End Property


    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Property Uci() As HspfUci
        Get
            Uci = pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public ReadOnly Property MonthDataTables() As Collection
        Get
            MonthDataTables = pMonthDataTables
        End Get
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlMonthDataEdit"
        End Get
    End Property

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True)
    End Sub

    Public Sub New()
        MyBase.New()
        pMonthDataTables = New Collection
    End Sub

    Public Sub ReadUciFile()
        Dim done As Boolean
        Dim init, OmCode As Integer
        Dim retkey, retcod As Integer
        Dim cbuff As String
        'UPGRADE_NOTE: val was upgraded to val_Renamed. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim i, rectyp As Integer
        Dim val_Renamed As Single
        Dim myMonthDataTable As HspfMonthDataTable

        If pUci.FastFlag Then
            GetCommentBeforeBlock("MONTH-DATA", pComment)
        End If

        OmCode = HspfOmCode("MONTH-DATA")
        init = 1
        done = False
        retkey = -1
        Do Until done
            If pUci.FastFlag Then
                GetNextRecordFromBlock("MONTH-DATA", retkey, cbuff, rectyp, retcod)
            Else
                Call REM_XBLOCK((Me.Uci), OmCode, init, retkey, cbuff, retcod)
            End If
            init = 0
            If InStr(cbuff, "END") Then 'skip this
            ElseIf InStr(cbuff, "MONTH-DATA") > 0 Then  'another one
                myMonthDataTable = New HspfMonthDataTable
                myMonthDataTable.Id = CInt(Right(cbuff, 3))
                myMonthDataTable.Block = Me
                If pUci.FastFlag Then
                    GetNextRecordFromBlock("MONTH-DATA", retkey, cbuff, rectyp, retcod)
                Else
                    Call REM_XBLOCK((Me.Uci), OmCode, init, retkey, cbuff, retcod)
                End If
                For i = 1 To 12
                    val_Renamed = CSng(Mid(cbuff, 1 + (i - 1) * 6, 6))
                    myMonthDataTable.MonthValue(i) = val_Renamed
                Next i
                pMonthDataTables.Add(myMonthDataTable)
            End If
            If retcod <> 2 Then
                done = True
            End If
        Loop
    End Sub

    Public Sub WriteUciFile(ByRef f As Integer)
        Dim i, j As Integer
        Dim s, t As String
        Dim lMonthDataTable As HspfMonthDataTable

        With pMonthDataTables
            If .Count() > 0 Then 'something to write
                If Len(pComment) > 0 Then
                    PrintLine(f, pComment)
                End If
                PrintLine(f, " ")
                PrintLine(f, "MONTH-DATA")
                PrintLine(f, " ")
                For i = 1 To .Count()
                    lMonthDataTable = .Item(i)
                    PrintLine(f, "  MONTH-DATA     " & myFormatI((lMonthDataTable.Id), 3))
                    s = ""
                    For j = 1 To 12
                        t = Space(6)
                        t = RSet(CStr(lMonthDataTable.MonthValue(j)), Len(t))
                        s = s & t
                    Next j
                    PrintLine(f, s)
                    PrintLine(f, "  END MONTH-DATA " & myFormatI((lMonthDataTable.Id), 3))
                    PrintLine(f, " ")
                Next i
                PrintLine(f, "END MONTH-DATA")
            End If
        End With
    End Sub
End Class