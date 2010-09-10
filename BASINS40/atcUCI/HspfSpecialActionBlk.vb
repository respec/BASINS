'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfSpecialActionBlk
    'Private pActions As Collection 'of HspfSpecialAction
    'Private pDistributes As Collection 'of HspfSpecialDistribute
    'Private pUserDefineNames As Collection 'of HspfSpecialUserDefineName
    'Private pUserDefineQuans As Collection 'of HspfSpecialUserDefineQuans
    'Private pConditions As Collection 'of HspfSpecialCondition
    Private pRecords As Generic.List(Of HspfSpecialRecord)

    Public Comment As String = ""
    Public Uci As HspfUci = Nothing
    Public ReadOnly Caption As String = "Special Actions Block"
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlSpecialActionEdit"

    'ReadOnly Property Actions() As Collection
    '    Get
    '        Return pActions
    '    End Get
    'End Property

    'ReadOnly Property Distributes() As Collection
    '    Get
    '        Return pDistributes
    '    End Get
    'End Property

    'ReadOnly Property UserDefineNames() As Collection
    '    Get
    '        Return pUserDefineNames
    '    End Get
    'End Property

    'ReadOnly Property UserDefineQuans() As Collection
    '    Get
    '        Return pUserDefineQuans
    '    End Get
    'End Property

    'ReadOnly Property Conditions() As Collection
    '    Get
    '        Return pConditions
    '    End Get
    'End Property

    ReadOnly Property Records() As Generic.List(Of HspfSpecialRecord)
        Get
            Return pRecords
        End Get
    End Property

    Public Sub ReadUciFile()
        Me.Comment = GetCommentBeforeBlock("SPEC-ACTIONS")

        Dim lOmCode As Integer = HspfOmCode("SPEC-ACTIONS")
        Dim lInit As Integer = 1
        Dim lDone As Boolean = False
        Dim lMoreUvNames As Integer = 0
        Dim lRetKey As Integer = -1
        Dim lUCIRecord As String = Nothing
        Dim lUCIRecordType As Integer
        Dim lReturnCode As Integer
        Do Until lDone
            GetNextRecordFromBlock("SPEC-ACTIONS", lRetKey, lUCIRecord, lUCIRecordType, lReturnCode)

            lInit = 0
            If lReturnCode = 2 Then 'normal record
                Dim lSpecialRecord As New HspfSpecialRecord
                With lSpecialRecord
                    .Text = lUCIRecord
                    If Len(Trim(lUCIRecord)) = 0 Or InStr(lUCIRecord, "***") > 0 Then
                        .SpecType = HspfData.HspfSpecialRecordType.hComment
                    ElseIf Left(Trim(lUCIRecord), 3) = "IF " Or Left(Trim(lUCIRecord), 4) = "ELSE" Or Left(Trim(lUCIRecord), 6) = "END IF" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hCondition
                    ElseIf Mid(lUCIRecord, 3, 6) = "DISTRB" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hDistribute
                    ElseIf Mid(lUCIRecord, 3, 6) = "UVNAME" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hUserDefineName
                        'look at how many uvnames to come
                        lMoreUvNames = CShort(Mid(lUCIRecord, 17, 3))
                        lMoreUvNames = Int((lMoreUvNames - 1) / 2) 'lines to come
                    ElseIf Mid(lUCIRecord, 3, 6) = "UVQUAN" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hUserDefineQuan
                    Else
                        If lMoreUvNames > 0 Then
                            .SpecType = HspfData.HspfSpecialRecordType.hUserDefineName
                            If Left(.Text, 5) <> "     " Then 'see if record needs padding
                                .Text = "                  " & .Text
                            End If
                            lMoreUvNames -= 1
                        Else
                            .SpecType = HspfData.HspfSpecialRecordType.hAction
                        End If
                    End If
                End With
                pRecords.Add(lSpecialRecord)
            Else
                lDone = True
            End If
        Loop
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New System.Text.StringBuilder
        If pRecords.Count > 0 Then
            If Me.Comment.Length > 0 Then
                lSB.AppendLine(Me.Comment)
            End If
            lSB.AppendLine("SPEC-ACTIONS")
            For Each lRecord As atcUCI.HspfSpecialRecord In pRecords
                lSB.AppendLine(lRecord.Text)
            Next
            lSB.AppendLine("END SPEC-ACTIONS")
        End If
        Return lSB.ToString
    End Function

    Public Sub New()
        MyBase.New()
        pRecords = New Generic.List(Of HspfSpecialRecord)
        'pActions = New Collection
        'pDistributes = New Collection
        'pUserDefineNames = New Collection
        'pUserDefineQuans = New Collection
        'pConditions = New Collection
    End Sub

    Public Function HspfSpecialRecordName(ByRef aType As HspfData.HspfSpecialRecordType) As String
        Dim lRecordName As String

        Select Case aType
            Case HspfData.HspfSpecialRecordType.hComment : lRecordName = "Comment"
            Case HspfData.HspfSpecialRecordType.hAction : lRecordName = "Action"
            Case HspfData.HspfSpecialRecordType.hDistribute : lRecordName = "Distribute"
            Case HspfData.HspfSpecialRecordType.hUserDefineName : lRecordName = "User Defn Name"
            Case HspfData.HspfSpecialRecordType.hUserDefineQuan : lRecordName = "User Defn Quan"
            Case HspfData.HspfSpecialRecordType.hCondition : lRecordName = "Condition"
            Case Else : lRecordName = "Unknown"
        End Select

        Return lRecordName
    End Function
End Class