Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfTable_NET.HspfTable")> Public Class HspfTable
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pDef As HspfTableDef
	Dim pOccurCount As Integer 'total number of occurances
	Dim pOccurNum As Integer 'nth occurance
	Dim pOccurIndex As Integer 'occurance with which this table is associated
	Dim pComment As String
	Dim pTableComment As String
	Dim pParms As Collection 'of HSPFParm
	Dim pOpn As HspfOperation
	Dim pEditAllSimilar As Boolean
	Dim pEdited As Boolean
	Dim pSuppID As Integer '>0 indicates parms on this record are in supplemental file
	Dim pCombineOK As Boolean
	
	Public Property OccurCount() As Integer
		Get
			OccurCount = pOccurCount
		End Get
		Set(ByVal Value As Integer)
			pOccurCount = Value
		End Set
	End Property
	
	Public Property OccurNum() As Integer
		Get
			OccurNum = pOccurNum
		End Get
		Set(ByVal Value As Integer)
			pOccurNum = Value
		End Set
	End Property
	
	Public Property OccurIndex() As Integer
		Get
			OccurIndex = pOccurIndex
		End Get
		Set(ByVal Value As Integer)
			pOccurIndex = Value
		End Set
	End Property
	
	Public Property Def() As HspfTableDef
		Get
			Def = pDef
		End Get
		Set(ByVal Value As HspfTableDef)
			pDef = Value
		End Set
	End Property
	
	Public Property Comment() As String
		Get
			Comment = pComment
		End Get
		Set(ByVal Value As String)
			pComment = Value
		End Set
	End Property
	
	Public Property TableComment() As String
		Get
			TableComment = pTableComment
		End Get
		Set(ByVal Value As String)
			pTableComment = Value
		End Set
	End Property
	
	Public ReadOnly Property Name() As String
		Get
			Name = pDef.Name
		End Get
	End Property

    ''' <summary>
    ''' Value of parameter with the given name
    ''' </summary>
    ''' <param name="aName">Name of parameter</param>
    Public Property ParmValue(ByVal aName As String) As String
        Get
            Return pParms.Item(aName).Value
        End Get
        Set(ByVal newValue As String)
            pParms.Item(aName).Value = newValue
        End Set
    End Property

    Public ReadOnly Property Parms() As Collection
        Get 'of HSPFParm
            Parms = pParms
        End Get
    End Property

    Public Property Opn() As HspfOperation
        Get
            Opn = pOpn
        End Get
        Set(ByVal Value As HspfOperation)
            pOpn = Value
        End Set
    End Property

    Public Property Edited() As Boolean
        Get
            Edited = pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then pOpn.Edited = True
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlTableEdit"
        End Get
    End Property

    Public ReadOnly Property EditAllSimilar() As Boolean
        Get
            EditAllSimilar = pEditAllSimilar
        End Get
    End Property

    Public ReadOnly Property Caption() As Object
        Get
            Return pOpn.Name & ":" & Name
        End Get
    End Property

    Public Property SuppID() As Integer
        Get
            SuppID = pSuppID
        End Get
        Set(ByVal Value As Integer)
            pSuppID = Value
        End Set
    End Property

    Public Property CombineOK() As Boolean
        Get
            CombineOK = pCombineOK
        End Get
        Set(ByVal Value As Boolean)
            pCombineOK = Value
        End Set
    End Property
    Public Function EditAllSimilarChange(ByRef newEditAllSimilar As Boolean) As Object
        EditAllSimilarChange = pEditAllSimilar
        pEditAllSimilar = newEditAllSimilar
    End Function

    Public Sub initTable(ByRef s As String)
        Dim lParm As HSPFParm
        Dim vParmDef As Object
        Dim lParmDef As HSPFParmDef
        Dim unitfg As Integer

        For Each vParmDef In pDef.ParmDefs
            lParmDef = vParmDef
            lParm = New HSPFParm
            lParm.Parent = Me
            lParm.Def = lParmDef
            lParm.Value = Trim(Mid(s, lParmDef.StartCol, lParmDef.Length))
            lParm.ValueAsRead = Mid(s, lParmDef.StartCol, lParmDef.Length)
            If lParm.ValueAsRead = "" And Len(s) > 0 Then lParm.ValueAsRead = " "
            If Len(lParm.Value) = 0 Then 'try default
                unitfg = pOpn.OpnBlk.Uci.GlobalBlock.emfg
                If unitfg = 1 Then
                    lParm.Value = lParm.Def.Default_Renamed
                Else
                    lParm.Value = lParm.Def.MetricDefault
                End If
            End If
            If Len(lParm.Value) > 0 Then
                If lParm.Def.Typ = 1 Then
                    If IsNumeric(lParm.Value) Then
                        lParm.Value = CStr(CInt(lParm.Value))
                    End If
                ElseIf lParm.Def.Typ = 2 Then
                    If IsNumeric(lParm.Value) Then
                        If CDbl(lParm.Value) > 10 ^ lParmDef.Length Then
                            lParm.Value = NumFmtRE(CSng(lParm.Value), (lParmDef.Length))
                        ElseIf lParmDef.Length <= 5 Then
                            If CDbl(lParm.Value) < 1 And CDbl(lParm.Value) > 0 Then
                                'lParm.Value = Format(CSng(lParm.Value), ".###")
                                lParm.Value = NumFmtRE(CSng(lParm.Value), (lParmDef.Length))
                            Else
                                lParm.Value = Format(CSng(lParm.Value), "0.###")
                            End If
                        ElseIf System.Math.Abs(CDbl(lParm.Value)) > 0 And System.Math.Abs(CDbl(lParm.Value)) <= 10 ^ -4 Then  'pbd bug fix for small e formats
                            lParm.Value = NumFmtRE(CSng(lParm.Value), (lParmDef.Length))
                        Else
                            lParm.Value = Format(CSng(lParm.Value), "0.####")
                        End If
                        'If lParmDef.Length <= 5 Then
                        '  Debug.Print Trim(Mid(s, lParmDef.StartCol, lParmDef.Length)), lParm.Value
                        'End If
                    End If
                End If
            End If
            pParms.Add(lParm, lParm.Name)
        Next vParmDef
    End Sub

    Public Sub Edit()
        '  Dim iresp&
        '  iresp = 1
        '  If Me.Name = "PWAT-PARM1" Or Me.Name = "IWAT-PARM1" Or Me.Name = "HYDR-PARM1" Then
        '    'choose regular or deluxe version to edit
        '    iresp = myMsgBox.Show("Choose an option for editing this table.", Me.Name & " Edit Option", "+-&Basic", "&Enhanced")
        '  End If
        '  If iresp <> 2 Then
        '    editInit Me, Me.Opn.OpnBlk.Uci.icon
        '  ElseIf Me.Name = "PWAT-PARM1" Then
        '    frmPwatEdit.init Me, Me.Opn.Uci.icon
        '    frmPwatEdit.Show vbModal
        '  ElseIf Me.Name = "IWAT-PARM1" Then
        '    frmIwatEdit.init Me, Me.Opn.Uci.icon
        '    frmIwatEdit.Show vbModal
        '  ElseIf Me.Name = "HYDR-PARM1" Then
        '    frmHydrEdit.init Me, Me.Opn.Uci.icon
        '    frmHydrEdit.Show vbModal
        '  End If
    End Sub

    Public Sub WriteUciFile(ByRef f As Short, Optional ByRef instance As Integer = 0)
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable
        Dim lParm As HSPFParm
        Dim SuppStr, s, t, tempValue As String
        Dim ilen, icon As Integer
        Dim i, j As Integer
        Dim tname As String
        Dim r As Single
        Dim firstopn As Boolean
        Dim pFlg As Boolean
        Dim p As String
        Dim units As Integer
        Dim lFirstOccur, lLastOccur As Integer

        units = Me.Opn.OpnBlk.Uci.GlobalBlock.emfg

        If instance = 0 Then
            lFirstOccur = 1
            lLastOccur = pOccurCount
        Else
            lFirstOccur = instance
            lLastOccur = instance
        End If

        For i = lFirstOccur To lLastOccur
            If i = 1 Then
                tname = pDef.Name
            Else
                tname = pDef.Name & ":" & i
            End If
            PrintLine(f, "  " & pDef.Name)

            pFlg = False
            firstopn = True
            For j = 1 To pOpn.OpnBlk.Ids.Count()
                On Error GoTo noTableForThisOper
                lOpn = pOpn.OpnBlk.NthOper(j)
                'write values here
                If Err.Number Then Resume
                If Not (lOpn.TableExists(tname)) Then
                    'no Table for this Operation
                Else
                    lTable = lOpn.Tables.Item(tname)
                    s = myFormatI((lOpn.Id), 5) & Space(5)
                    For Each lParm In lTable.Parms
                        With lParm
                            tempValue = lParm.Value
                            s = s & Space(.Def.StartCol - Len(s) - 1) 'pad prev field
                            If .Def.Typ = 0 Then ' ATCoTxt 'left justify strings
                                If .Def.Length < Len(tempValue) Then
                                    tempValue = Left(tempValue, .Def.Length)
                                End If
                                t = LTrim(tempValue)
                            Else
                                'not a string
                                'compare format of this value with the format as read
                                If NumericallyTheSame(.ValueAsRead, tempValue, .Def.Default_Renamed) Then
                                    'use the value as read
                                    t = .ValueAsRead
                                Else
                                    'right justify everything else
                                    t = Space(.Def.Length)
                                    If Len(tempValue) > .Def.Length Then
                                        r = CSng(tempValue)
                                        If (Len(CStr(r)) = .Def.Length + 1) And tempValue < CStr(1.0#) Then
                                            'just leave off leading zero
                                            t = RSet(Mid(CStr(r), 2), Len(t))
                                        Else
                                            t = RSet(NumFmtRE(CSng(tempValue), .Def.Length), Len(t))
                                        End If
                                    Else
                                        t = RSet(CStr(tempValue), Len(t))
                                    End If
                                End If
                            End If
                        End With
                        s = s & t
                    Next lParm
                    If lTable.SuppID > 0 Then 'include supplemental file ID for this record
                        SuppStr = "~" & lTable.SuppID & "~"
                        s = Left(s, 10) & SuppStr & Mid(s, 11 + Len(SuppStr))
                    End If
                    If pFlg Then
                        If compareTableString(1, 10, p, s) And lTable.CombineOK Then
                            s = Left(p, 5) & Left(s, 5) & Right(s, Len(s) - 10)
                        Else
                            ilen = Len(p)
                            If ilen > 80 Then
                                'this is a multi line table
                                If tname = "REPORT-CON" Then 'special case for this table
                                    icon = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                                    p = Mid(p, 1, 10 + (icon * 70))
                                End If
                                Call PrintMultiLine(f, p)
                            Else
                                PrintLine(f, p)
                                If Len(lTable.Comment) > 0 Then 'an comment associated with this operation
                                    PrintLine(f, lTable.Comment)
                                End If
                            End If
                        End If
                    End If
                    pFlg = True
                    If firstopn Then
                        If Len(lOpn.Tables.Item(tname).Comment) > 0 Then 'an associated comment
                            PrintLine(f, lOpn.Tables.Item(tname).Comment) 'pbd
                        Else
                            If units = 1 Then
                                PrintLine(f, pDef.HeaderE)
                            Else
                                PrintLine(f, pDef.HeaderM)
                            End If
                        End If
                        firstopn = False
                    End If
                    p = s
                    GoTo notMissingTableForThisOper
                End If
noTableForThisOper:
                If pFlg Then 'record pending
                    ilen = Len(p)
                    If ilen > 80 Then
                        'this is a multi line table
                        If tname = "REPORT-CON" Then 'special case for this table
                            icon = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                            p = Mid(p, 1, 10 + (icon * 70))
                        End If
                        Call PrintMultiLine(f, p)
                    Else
                        PrintLine(f, p)
                    End If
                    pFlg = False
                End If
notMissingTableForThisOper:
            Next j
            If pFlg Then 'record pending
                ilen = Len(p)
                If ilen > 80 Then
                    'this is a multi line table
                    If tname = "REPORT-CON" Then 'special case for this table
                        icon = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                        p = Mid(p, 1, 10 + (icon * 70))
                    End If
                    Call PrintMultiLine(f, p)
                Else
                    PrintLine(f, p)
                End If
            End If
            PrintLine(f, "  END " & pDef.Name & vbCrLf)
        Next i
    End Sub

    Public Sub New()
        MyBase.New()
        pDef = New HspfTableDef
        pParms = New Collection
        pOccurCount = 0
        pEditAllSimilar = True
        pCombineOK = True
    End Sub

    Private Sub PrintMultiLine(ByRef f As Short, ByRef p As String)
        Dim n, ilen, nlines, nchar As Integer
        Dim t As String

        ilen = Len(p)
        PrintLine(f, Mid(p, 1, 80))
        nlines = (ilen - 10) / 70
        If nlines > 3 Then 'make sure something in remaining lines
            p = RTrim(p)
            ilen = Len(p)
            nlines = (ilen - 10) / 70
        End If

        For n = 1 To nlines
            If n = nlines Then
                nchar = ilen - (n * 70) - 10
            Else
                nchar = 70
            End If
            If nchar > 0 Then
                t = Mid(p, 1, 10) & Mid(p, (n * 70) + 11, nchar)
                PrintLine(f, t)
            End If
        Next n
    End Sub

    Public Sub SetQualIndex(ByRef noccur As Integer, ByRef Nqual As Integer)
        Dim tabcnt, i As Integer
        Dim p As String = ""
        Dim t As String = ""
        Dim tname As String

        Select Case pDef.Name
            Case "GQ-GENDECAY" : t = "GQ-QALFG" : p = "QALFG6" 'rchres
            Case "GQ-HYDPM" : t = "GQ-QALFG" : p = "QALFG1"
            Case "GQ-ROXPM" : t = "GQ-QALFG" : p = "QALFG2"
            Case "GQ-PHOTPM" : t = "GQ-QALFG" : p = "QALFG3"
            Case "GQ-CFGAS" : t = "GQ-QALFG" : p = "QALFG4"
            Case "GQ-BIOPM" : t = "GQ-QALFG" : p = "QALFG5"
            Case "MON-BIO" : t = "GQ-QALFG" : p = "QALFG5"
            Case "GQ-SEDDECAY" : t = "GQ-QALFG" : p = "QALFG7"
            Case "GQ-KD" : t = "GQ-QALFG" : p = "QALFG7"
            Case "GQ-ADRATE" : t = "GQ-QALFG" : p = "QALFG7"
            Case "GQ-ADTHETA" : t = "GQ-QALFG" : p = "QALFG7"
            Case "GQ-SEDCONC" : t = "GQ-QALFG" : p = "QALFG7"
                'Case "MON-WATEMP": t = "GQ-QALFG": p = QALFG1
            Case "MON-PHVAL" : t = "GQ-QALFG" : p = "QALFG1"
            Case "MON-ROXYGEN" : t = "GQ-QALFG" : p = "QALFG2"
            Case "GQ-ALPHA" : t = "GQ-QALFG" : p = "QALFG3"
            Case "GQ-GAMMA" : t = "GQ-QALFG" : p = "QALFG3"
            Case "GQ-DELTA" : t = "GQ-QALFG" : p = "QALFG3"
            Case "GQ-CLDFACT" : t = "GQ-QALFG" : p = "QALFG3"
            Case "MON-CLOUD" : t = "GQ-QALFG" : p = "QALFG3"
            Case "MON-SEDCONC" : t = "GQ-QALFG" : p = "QALFG3"
            Case "MON-PHYTO" : t = "GQ-QALFG" : p = "QALFG3"
                'Case "GQ-DAUGHTER": t = "GQ-QALFG": p = QALFG1
            Case "MON-SQOLIM" : t = "QUAL-PROPS" : p = "VQOFG" 'perlnd
            Case "MON-POTFW" : t = "QUAL-PROPS" : p = "VPFWFG"
            Case "MON-POTFS" : t = "QUAL-PROPS" : p = "VPFSFG"
            Case "MON-ACCUM" : t = "QUAL-PROPS" : p = "VQOFG"
            Case "MON-IFLW-CONC" : t = "QUAL-PROPS" : p = "VIQCFG"
            Case "MON-GRND-CONC" : t = "QUAL-PROPS" : p = "VAQCFG"
            Case "MON-SQOLIM" : t = "QUAL-PROPS" : p = "VQOFG" 'implnd
            Case "MON-POTFW" : t = "QUAL-PROPS" : p = "VPFWFG"
            Case "MON-ACCUM" : t = "QUAL-PROPS" : p = "VQOFG"
        End Select

        If Len(t) > 0 Then
            tabcnt = 0
            tname = t
            For i = 1 To Nqual
                If i > 1 Then
                    tname = t & ":" & i
                End If
                If pOpn.TableExists(tname) Then
                    If pOpn.Tables.Item(tname).Parms(p).value > 0 Then
                        tabcnt = tabcnt + 1
                        If tabcnt = noccur Then
                            'this is the one this table belongs to
                            pOccurIndex = i
                        End If
                    End If
                End If
            Next i
        End If
    End Sub

    Public Function TableNeededForAllQuals() As Boolean

        If pDef.Name = "QUAL-INPUT" Or pDef.Name = "GQ-QALFG" Or pDef.Name = "GQ-FLG2" Or pDef.Name = "GQ-VALUES" Or pDef.Name = "QUAL-PROPS" Or pDef.Name = "GQ-QALDATA" Then
            TableNeededForAllQuals = True
        Else
            TableNeededForAllQuals = False
        End If

    End Function

    Private Function NumericallyTheSame(ByRef ValueAsRead As String, ByRef ValueStored As String, ByRef ValueDefault As String) As Boolean
        'see if the current table value is the same as the value as read from the uci
        '4. is the same as 4.0
        '"  " is the same as 1 if 1 is the default
        Dim rtemp1, rtemp2 As Single

        NumericallyTheSame = False
        If IsNumeric(ValueStored) Then
            If IsNumeric(ValueAsRead) Then
                'simple case
                rtemp1 = CSng(ValueAsRead)
                rtemp2 = CSng(ValueStored)
                If rtemp1 = rtemp2 Then
                    NumericallyTheSame = True
                End If
            ElseIf Len(ValueAsRead) > 0 And Len(Trim(ValueAsRead)) = 0 Then
                'one or more blank characters
                'see if the value stored is the same as the default
                rtemp1 = CSng(ValueStored)
                rtemp2 = CSng(ValueDefault)
                If rtemp1 = rtemp2 Then
                    'we can use the blanks
                    NumericallyTheSame = True
                End If
            End If
        End If
    End Function

    Shared Function NumFmtRE(ByVal rtmp As Single, Optional ByRef maxWidth As Integer = 16) As String
        ' ##SUMMARY Converts single-precision number to string with exponential syntax if length of number exceeds specified length.
        ' ##SUMMARY If unspecified, length defaults to 16.
        ' ##SUMMARY   Example: NumFmtRE(123000000, 7) = "1.23e-8"
        ' ##PARAM rtmp I Single-precision number to be formatted
        ' ##PARAM maxWidth I Length of string to be returned including decimal point and exponential syntax
        ' ##RETURNS Input parameter rtmp formatted, if necessary, to scientific notation.
        ' ##LOCAL LogVal - double-precision log10 value of rtmp
        ' ##LOCAL retval - string used as antecedent to NumFmtRE
        ' ##LOCAL expFormat - string syntax of exponential format
        ' ##LOCAL DecimalPlaces - long number of decimal places
        Dim LogVal As Double
        Dim retval As String
        Dim expFormat As String
        Dim DecimalPlaces As Integer

        retval = CStr(rtmp)
        NumFmtRE = retval

        If rtmp <> 0 And maxWidth > 0 Then
            If Len(retval) > maxWidth Then
                If Len(retval) - maxWidth = 1 And Left(retval, 2) = "0." Then
                    'special case, can just eliminate leading zero
                    retval = Mid(retval, 2)
                ElseIf Len(retval) - maxWidth = 1 And Left(retval, 3) = "-0." Then
                    'special case, can just eliminate leading zero
                    retval = "-" & Mid(retval, 3)
                Else
                    'Determine appropriate log syntax
                    LogVal = System.Math.Abs(Log10(System.Math.Abs(rtmp)))
                    If LogVal >= 100 Then
                        expFormat = "e-###"
                    ElseIf LogVal >= 10 Then
                        expFormat = "e-##"
                    Else
                        expFormat = "e-#"
                    End If
                    'Set appropriate decimal position
                    DecimalPlaces = maxWidth - Len(expFormat) - 2
                    'If DecimalPlaces < 1 Then DecimalPlaces = 1  'pbd changed to accomodate 1.e-5
                    If (DecimalPlaces < 0) Or (DecimalPlaces = 0 And rtmp > 1.0#) Then
                        DecimalPlaces = 1
                    End If

                    retval = Format(rtmp, "#." & New String("#", DecimalPlaces) & expFormat)
                End If
            End If
        End If
        NumFmtRE = retval
    End Function

End Class