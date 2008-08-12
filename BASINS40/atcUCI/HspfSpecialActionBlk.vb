'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfSpecialActionBlk

    Private pActions As Collection 'of HspfSpecialAction
    Private pDistributes As Collection 'of HspfSpecialDistribute
    Private pUserDefineNames As Collection 'of HspfSpecialUserDefineName
    Private pUserDefineQuans As Collection 'of HspfSpecialUserDefineQuans
    Private pConditions As Collection 'of HspfSpecialCondition
    Private pRecords As Collection 'of HspfSpecialRecord
    Private pComment As String
    Private pUci As HspfUci

    Friend Property Uci() As HspfUci
        Get
            Uci = pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Caption = "Special Actions Block"
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

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlSpecialActionEdit"
        End Get
    End Property

    ReadOnly Property Actions() As Collection
        Get
            Actions = pActions
        End Get
    End Property

    ReadOnly Property Distributes() As Collection
        Get
            Distributes = pDistributes
        End Get
    End Property

    ReadOnly Property UserDefineNames() As Collection
        Get
            UserDefineNames = pUserDefineNames
        End Get
    End Property

    ReadOnly Property UserDefineQuans() As Collection
        Get
            UserDefineQuans = pUserDefineQuans
        End Get
    End Property

    ReadOnly Property Conditions() As Collection
        Get
            Conditions = pConditions
        End Get
    End Property

    ReadOnly Property Records() As Collection
        Get
            Records = pRecords
        End Get
    End Property

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True)
    End Sub

    Public Sub ReadUciFile()
        Dim done As Boolean
        Dim init, OmCode As Integer
        Dim retkey, retcod As Integer
        Dim cbuff As String = Nothing
        Dim rectyp As Integer
        Dim mySpecialRecord As HspfSpecialRecord
        Dim moreUvnames As Integer

        If pUci.FastFlag Then
            pComment = GetCommentBeforeBlock("SPEC-ACTIONS")
        End If

        moreUvnames = 0
        OmCode = HspfOmCode("SPEC-ACTIONS")
        init = 1
        done = False
        retkey = -1
        Do Until done
            If pUci.FastFlag Then
                GetNextRecordFromBlock("SPEC-ACTIONS", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -2 'force return of comments/blanks
                Call REM_XBLOCKEX((Me.Uci), OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            init = 0
            If retcod = 2 Then 'normal record
                mySpecialRecord = New HspfSpecialRecord
                With mySpecialRecord
                    .Text = cbuff
                    If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
                        .SpecType = HspfData.HspfSpecialRecordType.hComment
                    ElseIf Left(Trim(cbuff), 3) = "IF " Or Left(Trim(cbuff), 4) = "ELSE" Or Left(Trim(cbuff), 6) = "END IF" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hCondition
                    ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hDistribute
                    ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hUserDefineName
                        'look at how many uvnames to come
                        moreUvnames = CShort(Mid(cbuff, 17, 3))
                        moreUvnames = Int((moreUvnames - 1) / 2) 'lines to come
                    ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
                        .SpecType = HspfData.HspfSpecialRecordType.hUserDefineQuan
                    Else
                        If moreUvnames > 0 Then
                            .SpecType = HspfData.HspfSpecialRecordType.hUserDefineName
                            If Left(.Text, 5) <> "     " Then 'see if record needs padding
                                .Text = "                  " & .Text
                            End If
                            moreUvnames = moreUvnames - 1
                        Else
                            .SpecType = HspfData.HspfSpecialRecordType.hAction
                        End If
                    End If
                End With
                pRecords.Add(mySpecialRecord)
            Else
                done = True
            End If
        Loop
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        Dim i As Integer

        If pRecords.Count > 0 Then
            If pComment.Length > 0 Then
                lSB.AppendLine(pComment)
            End If
            lSB.AppendLine("SPEC-ACTIONS")
            With pRecords
                For i = 1 To .Count
                    lSB.AppendLine(.Item(i).Text)
                Next i
            End With
            lSB.AppendLine("END SPEC-ACTIONS")
        End If
        Return lSB.ToString
    End Function

    Public Sub New()
        MyBase.New()
        pRecords = New Collection
        pActions = New Collection
        pDistributes = New Collection
        pUserDefineNames = New Collection
        pUserDefineQuans = New Collection
        pConditions = New Collection
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