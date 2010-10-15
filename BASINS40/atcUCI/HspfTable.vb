'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfTables
    Inherits KeyedCollection(Of String, HspfTable)
    Implements ICloneable

    Protected Overrides Function GetKeyForItem(ByVal aTable As HspfTable) As String
        Dim lTableKey As String
        If aTable.OccurNum > 1 Then
            lTableKey = aTable.Name & ":" & aTable.OccurNum
        Else
            lTableKey = aTable.Name
        End If
        Return lTableKey
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim lNewHspfTables As New HspfTables
        For Each lHspfTable As HspfTable In Me
            lNewHspfTables.Add(lHspfTable.Clone)
        Next
        Return lNewHspfTables
    End Function

    Public Sub AddRange(ByVal aHspfTables As HspfTables, ByVal aOperation As atcUCI.HspfOperation)
        For Each lHspfTable As HspfTable In aHspfTables
            lHspfTable.Opn = aOperation
            Me.Add(lHspfTable)
        Next
    End Sub
End Class

Public Class HspfTable
    Implements ICloneable

    Private pEditAllSimilar As Boolean
    Private pEdited As Boolean
    Private pSuppID As Integer '>0 indicates parms on this record are in supplemental file
    'Removed following variable (legacy from VB6???), prh & pbd, 6/4/2009
    'Private pCombineOK As Boolean

    Public Def As New HspfTableDef
    Public OccurCount As Integer 'total number of occurences
    Public OccurNum As Integer 'nth occurrence
    Public OccurIndex As Integer 'occurence with which this table is associated
    Public Comment As String = ""
    Public TableComment As String = ""
    Public Opn As HspfOperation
    Public SuppID As Integer
    Public CombineOK As Boolean
    Public ReadOnly Parms As New HspfParms
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlTableEdit"

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim lNewHspfTable As New HspfTable
        With lNewHspfTable
            .CombineOK = Me.CombineOK
            .Comment = Me.Comment
            .Def = Me.Def 'just a reference
            For Each lParm As atcUCI.HspfParm In Me.Parms
                .Parms.Add(lParm)
            Next
            .OccurCount = Me.OccurCount
            .OccurIndex = Me.OccurIndex
            .OccurNum = Me.OccurNum
            .TableComment = Me.TableComment
        End With
        Return lNewHspfTable
    End Function

    ''' <summary>
    ''' Value of parameter with the given name
    ''' </summary>
    ''' <param name="aName">Name of parameter</param>
    Public Property ParmValue(ByVal aName As String) As String
        Get
            Return Parms.Item(aName).Value
        End Get
        Set(ByVal newValue As String)
            Parms.Item(aName).Value = newValue
        End Set
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return Def.Name
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Return Opn.Name & ":" & Name
        End Get
    End Property

    Public Property Edited() As Boolean
        Get
            Return pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value AndAlso Opn IsNot Nothing Then
                Opn.Edited = True
            End If
        End Set
    End Property

    Public ReadOnly Property EditAllSimilar() As Boolean
        Get
            Return pEditAllSimilar
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        OccurCount = 0
        pEditAllSimilar = True
        'initialize public variable instead of legacy (VB6???) variable, prh & pbd, 6/4/2009
        'pCombineOK = True
        CombineOK = True
    End Sub

    Public Function EditAllSimilarChange(ByRef aEditAllSimilar As Boolean) As Object
        EditAllSimilarChange = pEditAllSimilar
        pEditAllSimilar = aEditAllSimilar
    End Function

    Public Sub InitTable(ByRef aInitValueString As String)
        Dim lParm As HspfParm
        Dim lUnitfg As Integer = 1
        If Opn IsNot Nothing AndAlso Opn.OpnBlk IsNot Nothing Then
            lUnitfg = Opn.OpnBlk.Uci.GlobalBlock.EmFg
        End If

        For Each lParmDef As HSPFParmDef In Def.ParmDefs
            lParm = New HspfParm
            lParm.Parent = Me
            lParm.Def = lParmDef
            lParm.Value = Trim(Mid(aInitValueString, lParmDef.StartCol, lParmDef.Length))
            lParm.ValueAsRead = Mid(aInitValueString, lParmDef.StartCol, lParmDef.Length)
            If lParm.ValueAsRead = "" And aInitValueString.Length > 0 Then
                lParm.ValueAsRead = " "
            End If
            If lParm.Value.Length = 0 Then 'try default
                If lUnitfg = 1 Then
                    lParm.Value = lParm.Def.DefaultValue
                Else
                    lParm.Value = lParm.Def.MetricDefault
                End If
            End If
            If lParm.Value.Length > 0 Then
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
                            'lParm.Value = Format(CSng(lParm.Value), "0.####")  'pbd -- why are we limiting to 4 decimal places?
                        End If
                        'If lParmDef.Length <= 5 Then
                        '  Debug.Print Trim(Mid(s, lParmDef.StartCol, lParmDef.Length)), lParm.Value
                        'End If
                    End If
                End If
            End If
            Parms.Add(lParm)
        Next lParmDef
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

    Public Overrides Function ToString() As String
        Return Me.ToStringByIndex(0)
    End Function

    Public Function ToStringByIndex(Optional ByRef aInstance As Integer = 0) As String
        Dim lFirstOccur, lLastOccur As Integer
        If aInstance = 0 Then
            lFirstOccur = 1
            lLastOccur = OccurCount
        Else
            lFirstOccur = aInstance
            lLastOccur = aInstance
        End If

        Dim lSB As New System.Text.StringBuilder
        For lOccur As Integer = lFirstOccur To lLastOccur
            Dim lTableName As String
            If lOccur = 1 Then
                lTableName = Def.Name
            Else
                If aInstance = 0 Then
                    lSB.AppendLine() 'add a blank line before additional occurrences of this table
                End If
                lTableName = Def.Name & ":" & lOccur
            End If
            lSB.AppendLine("  " & Def.Name)

            Dim lPendingFlag As Boolean = False
            Dim lFirstOpn As Boolean = True
            Dim lOutPend As String = Nothing 'pending record?
            For lOperIndex As Integer = 1 To Opn.OpnBlk.Ids.Count
                Dim lOperation As HspfOperation = Opn.OpnBlk.NthOper(lOperIndex)
                'write values here
                If lOperation IsNot Nothing AndAlso _
                   lOperation.TableExists(lTableName) Then
                    Dim lTable As HspfTable = lOperation.Tables.Item(lTableName)
                    Dim lOutRec As String = myFormatI((lOperation.Id), 5) & Space(5)
                    Dim lOutValue As String
                    For Each lParm As HspfParm In lTable.Parms
                        With lParm
                            Dim lValue As String = .Value
                            lOutRec = lOutRec & Space(.Def.StartCol - lOutRec.Length - 1) 'pad prev field
                            If .Def.Typ = 0 Then 'left justify strings
                                If .Def.Length < lValue.Length Then
                                    lValue = Left(lValue, .Def.Length)
                                End If
                                lOutValue = LTrim(lValue)
                            Else 'not a string
                                'compare format of this value with the format as read
                                If NumericallyTheSame(.ValueAsRead, lValue, .Def.DefaultValue) Then
                                    'use the value as read
                                    lOutValue = .ValueAsRead
                                Else
                                    'right justify everything else
                                    lOutValue = Space(.Def.Length)
                                    If lValue.Length > .Def.Length Then
                                        Dim r As Single = CSng(lValue)
                                        If (Len(CStr(r)) = .Def.Length + 1) And lValue < CStr(1.0#) Then
                                            'just leave off leading zero
                                            lOutValue = Mid(CStr(r), 2).PadLeft(lOutValue).Length
                                        Else
                                            lOutValue = NumFmtRE(CSng(lValue), .Def.Length).PadLeft(lOutValue.Length)
                                        End If
                                    Else
                                        lOutValue = CStr(lValue).PadLeft(lOutValue.Length)
                                    End If
                                End If
                            End If
                        End With
                        lOutRec &= lOutValue
                    Next lParm
                    If lTable.SuppID > 0 Then 'include supplemental file ID for this record
                        Dim lSuppStr As String = "~" & lTable.SuppID & "~"
                        lOutRec = Left(lOutRec, 10) & lSuppStr & Mid(lOutRec, 11 + lSuppStr.Length)
                    End If
                    If Not lOutPend Is Nothing AndAlso lPendingFlag Then
                        If CompareTableString(1, 10, lOutPend, lOutRec) And lTable.CombineOK Then
                            lOutRec = Left(lOutPend, 5) & Left(lOutRec, 5) & Right(lOutRec, lOutRec.Length - 10)
                        Else
                            If lOutPend.Length > 80 Then
                                'this is a multi line table
                                If lTableName = "REPORT-CON" Then 'special case for this table
                                    Dim lNCon As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                                    lOutPend = Mid(lOutPend, 1, 10 + (lNCon * 70))
                                ElseIf lTableName = "REPORT-SRC" Then 'special case for this table
                                    Dim lNSrc As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NSRC")
                                    lOutPend = Mid(lOutPend, 1, 10 + (lNSrc * 70))
                                End If
                                PrintMultiLine(lSB, lOutPend)
                            Else
                                lSB.AppendLine(lOutPend.TrimEnd)
                                If Not lTable.Comment Is Nothing AndAlso lTable.Comment.Length > 0 Then
                                    'an comment associated with this operation
                                    lSB.AppendLine(lTable.Comment)
                                End If
                            End If
                        End If
                    End If
                    lPendingFlag = True
                    If lFirstOpn Then
                        If lOperation.Tables.Item(lTableName).Comment.Length > 0 Then 'an associated comment
                            lSB.AppendLine(lOperation.Tables.Item(lTableName).Comment) 'pbd
                        Else
                            If Me.Opn.OpnBlk.Uci.GlobalBlock.EmFg = 1 Then
                                lSB.AppendLine(Def.HeaderE.TrimEnd)
                            Else
                                lSB.AppendLine(Def.HeaderM.TrimEnd)
                            End If
                        End If
                        lFirstOpn = False
                    End If
                    lOutPend = lOutRec
                    GoTo notMissingTableForThisOper
                End If
                If Not lOutPend Is Nothing AndAlso lPendingFlag Then 'record pending
                    If lOutPend.Length > 80 Then
                        'this is a multi line table
                        If lTableName = "REPORT-CON" Then 'special case for this table
                            Dim lNCon As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                            lOutPend = Mid(lOutPend, 1, 10 + (lNCon * 70))
                        ElseIf lTableName = "REPORT-SRC" Then 'special case for this table
                            Dim lNSrc As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NSRC")
                            lOutPend = Mid(lOutPend, 1, 10 + (lNSrc * 70))
                        End If
                        PrintMultiLine(lSB, lOutPend)
                    Else
                        lSB.AppendLine(lOutPend.TrimEnd)
                    End If
                    lPendingFlag = False
                End If
notMissingTableForThisOper:
            Next lOperIndex

            If Not lOutPend Is Nothing AndAlso lPendingFlag Then 'record pending
                If lOutPend.Length > 80 Then 'this is a multi line table
                    If lTableName = "REPORT-CON" Then 'special case for this table
                        Dim lNCon As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
                        lOutPend = Mid(lOutPend, 1, 10 + (lNCon * 70))
                    ElseIf lTableName = "REPORT-SRC" Then 'special case for this table
                        Dim lNSrc As Integer = Me.Opn.Tables.Item("REPORT-FLAGS").ParmValue("NSRC")
                        lOutPend = Mid(lOutPend, 1, 10 + (lNSrc * 70))
                    End If
                    PrintMultiLine(lSB, lOutPend)
                Else
                    lSB.AppendLine(lOutPend.TrimEnd)
                End If
            End If
            lSB.AppendLine("  END " & Def.Name)
        Next lOccur
        Return lSB.ToString
    End Function

    Private Sub PrintMultiLine(ByRef aSB As System.Text.StringBuilder, _
                               ByRef aOutPend As String)
        aSB.AppendLine(aOutPend.Substring(0, 80).TrimEnd) 'first line

        Dim lLength As Integer = aOutPend.Length
        Dim lNLinesMore As Integer = ((lLength - 10) / 70) - 1
        'If lNLinesMore > 3 Then 'make sure something in remaining lines
        '    lNLinesMore = aOutPend.TrimEnd
        '    lLen = aOutPend.Length
        '    lNLinesMore = (lLen - 10) / 70
        'End If

        Dim lNChar As Integer
        For lLineIndex As Integer = 1 To lNLinesMore
            If lLineIndex = lNLinesMore Then
                lNChar = lLength - (lLineIndex * 70) - 10
            Else
                lNChar = 70
            End If
            If lNChar > 0 Then
                Dim lOutRec As String
                lOutRec = aOutPend.Substring(0, 10) & _
                          aOutPend.Substring((lLineIndex * 70) + 10, lNChar)
                aSB.AppendLine(lOutRec.TrimEnd)
            End If
        Next lLineIndex
    End Sub

    Public Sub SetQualIndex(ByRef noccur As Integer, ByRef Nqual As Integer)
        Dim lParm As String = ""
        Dim lT As String = ""
        Select Case Def.Name
            Case "GQ-GENDECAY" : lT = "GQ-QALFG" : lParm = "QALFG6" 'rchres
            Case "GQ-HYDPM" : lT = "GQ-QALFG" : lParm = "QALFG1"
            Case "GQ-ROXPM" : lT = "GQ-QALFG" : lParm = "QALFG2"
            Case "GQ-PHOTPM" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "GQ-CFGAS" : lT = "GQ-QALFG" : lParm = "QALFG4"
            Case "GQ-BIOPM" : lT = "GQ-QALFG" : lParm = "QALFG5"
            Case "MON-BIO" : lT = "GQ-QALFG" : lParm = "QALFG5"
            Case "GQ-SEDDECAY" : lT = "GQ-QALFG" : lParm = "QALFG7"
            Case "GQ-KD" : lT = "GQ-QALFG" : lParm = "QALFG7"
            Case "GQ-ADRATE" : lT = "GQ-QALFG" : lParm = "QALFG7"
            Case "GQ-ADTHETA" : lT = "GQ-QALFG" : lParm = "QALFG7"
            Case "GQ-SEDCONC" : lT = "GQ-QALFG" : lParm = "QALFG7"
                'Case "MON-WATEMP": lT = "GQ-QALFG": lParm = QALFG1
            Case "MON-PHVAL" : lT = "GQ-QALFG" : lParm = "QALFG1"
            Case "MON-ROXYGEN" : lT = "GQ-QALFG" : lParm = "QALFG2"
            Case "GQ-ALPHA" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "GQ-GAMMA" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "GQ-DELTA" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "GQ-CLDFACT" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "MON-CLOUD" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "MON-SEDCONC" : lT = "GQ-QALFG" : lParm = "QALFG3"
            Case "MON-PHYTO" : lT = "GQ-QALFG" : lParm = "QALFG3"
                'Case "GQ-DAUGHTER": lT = "GQ-QALFG": lParm = QALFG1
            Case "MON-SQOLIM" : lT = "QUAL-PROPS" : lParm = "VQOFG" 'perlnd
            Case "MON-POTFW" : lT = "QUAL-PROPS" : lParm = "VPFWFG"
            Case "MON-POTFS" : lT = "QUAL-PROPS" : lParm = "VPFSFG"
            Case "MON-ACCUM" : lT = "QUAL-PROPS" : lParm = "VQOFG"
            Case "MON-IFLW-CONC" : lT = "QUAL-PROPS" : lParm = "VIQCFG"
            Case "MON-GRND-CONC" : lT = "QUAL-PROPS" : lParm = "VAQCFG"
            Case "MON-SQOLIM" : lT = "QUAL-PROPS" : lParm = "VQOFG" 'implnd
            Case "MON-POTFW" : lT = "QUAL-PROPS" : lParm = "VPFWFG"
            Case "MON-ACCUM" : lT = "QUAL-PROPS" : lParm = "VQOFG"
        End Select

        If lT.Length > 0 Then
            Dim lTableCount As Integer = 0
            Dim lTableName As String = lT
            For lQualIndex As Integer = 1 To Nqual
                If lQualIndex > 1 Then
                    lTableName = lT & ":" & lQualIndex
                End If
                If Opn.TableExists(lTableName) Then
                    If Opn.Tables.Item(lTableName).Parms.Item(lParm).Value > 0 Then
                        lTableCount += 1
                        If lTableCount = noccur Then
                            'this is the one this table belongs to
                            OccurIndex = lQualIndex
                        End If
                    End If
                End If
            Next lQualIndex
        End If
    End Sub

    Public Function TableNeededForAllQuals() As Boolean
        If Def.Name = "QUAL-INPUT" Or _
           Def.Name = "GQ-QALFG" Or _
           Def.Name = "GQ-FLG2" Or _
           Def.Name = "GQ-VALUES" Or _
           Def.Name = "QUAL-PROPS" Or _
           Def.Name = "GQ-QALDATA" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function NumericallyTheSame(ByRef aValueAsRead As String, _
                                        ByRef aValueStored As String, _
                                        ByRef aValueDefault As String) As Boolean
        'see if the current table value is the same as the value as read from the uci
        '4. is the same as 4.0
        '"  " is the same as 1 if 1 is the default
        Dim lNumericallyTheSame As Boolean = False

        If IsNumeric(aValueStored) Then
            If IsNumeric(aValueAsRead) Then 'simple case
                If Math.Abs(CSng(aValueAsRead) - CSng(aValueStored)) < 1.0E-20 Then
                    lNumericallyTheSame = True
                End If
            ElseIf aValueAsRead.Length > 0 AndAlso aValueAsRead.Trim.Length = 0 Then
                'one or more blank characters
                'see if the value stored is the same as the default
                If Math.Abs(CSng(aValueStored) - CSng(aValueDefault)) < 1.0E-20 Then 'we can use the blanks
                    lNumericallyTheSame = True
                End If
            End If
        End If
        Return lNumericallyTheSame
    End Function

    Shared Function NumFmtRE(ByVal aRVal As Single, Optional ByRef aMaxWidth As Integer = 16) As String
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
        Dim lNumFmtRE As String = aRVal.ToString
        If aRVal <> 0 And aMaxWidth > 0 Then
            If lNumFmtRE.Length > aMaxWidth Then
                If lNumFmtRE.Length - aMaxWidth = 1 And lNumFmtRE.StartsWith("0.") Then
                    'special case, can just eliminate leading zero
                    lNumFmtRE = lNumFmtRE.Substring(1)
                ElseIf lNumFmtRE.Length - aMaxWidth = 1 And lNumFmtRE.StartsWith("-0.") Then
                    'special case, can just eliminate leading zero
                    lNumFmtRE = "-" & lNumFmtRE.Substring(3)
                Else
                    'Determine appropriate log syntax
                    Dim lLogVal As Double = System.Math.Abs(Math.Log10(System.Math.Abs(aRVal)))
                    Dim lExpFormat As String
                    If lLogVal >= 100 Then
                        lExpFormat = "e-000"
                    ElseIf lLogVal >= 10 Then
                        lExpFormat = "e-00"
                    Else
                        lExpFormat = "e-0"
                    End If
                    'Set appropriate decimal position
                    Dim lDecimalPlaces As Integer = aMaxWidth - Len(lExpFormat) - 2
                    'If DecimalPlaces < 1 Then DecimalPlaces = 1  'pbd changed to accomodate 1.e-5
                    If (lDecimalPlaces < 0) Or (lDecimalPlaces = 0 And aRVal > 1.0#) Then
                        lDecimalPlaces = 1
                    End If
                    lNumFmtRE = Format(aRVal, "#." & New String("#", lDecimalPlaces) & lExpFormat)
                End If
            End If
        End If
        Return lNumFmtRE
    End Function
End Class