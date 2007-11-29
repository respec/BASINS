Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' 
''' </summary>
''' <remarks>Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
Public Class HspfCategoryBlk
    Private pCategories As Collection(Of HspfData.HspfCategory)
    Private pUci As HspfUci
    Private pComment As String

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
            Caption = "Category Block"
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
            EditControlName = "ATCoHspf.ctlCategoryBlkEdit"
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Count = pCategories.Count()
        End Get
    End Property

    Public Property Value(ByVal Index As Integer) As HspfData.HspfCategory
        Get
            If Index > 0 And Index <= pCategories.Count() Then
                'TODO: check this index
                Value = pCategories.Item(Index - 1)
            Else
                Value = New HspfData.HspfCategory
                Value.Name = ""
                Value.Tag = ""
            End If
        End Get
        Set(ByVal Value As HspfData.HspfCategory) '????
            If Index <= pCategories.Count() Then
                pCategories.RemoveAt(Index)
                pCategories.Insert(Index, Value)
            ElseIf Index = pCategories.Count() + 1 Then
                pCategories.Add(Value)
            Else 'error?
            End If
        End Set
    End Property

    Public Sub Clear()
        pCategories.Clear()
    End Sub

    Public Sub Add(ByRef newValue As HspfData.HspfCategory)
        pCategories.Add(newValue)
    End Sub

    Public Sub AddFromSpecs(ByRef newName As String, ByRef Tag As String)
        Dim newCategory As New HspfData.HspfCategory
        newCategory.Name = newName
        newCategory.Tag = Tag
        pCategories.Add(newCategory)
    End Sub

    Public Sub Remove(ByRef Index As Integer)
        If Index > 0 And Index <= pCategories.Count() Then
            pCategories.RemoveAt(Index)
        End If
    End Sub

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True)
    End Sub

    Public Sub New()
        MyBase.New()
        pCategories = New Collection(Of HspfData.HspfCategory)
    End Sub

    Private Sub Update()
        pUci.Edited = True
    End Sub

    'Public Function Check() As String
    '	'verify values are correct in relation to each other and other tables
    '       Return ""
    'End Function

    Friend Sub ReadUciFile()
        Dim lCategory As HspfData.HspfCategory
        Dim c As String
        Dim retkey, init, retcod, OmCode, rectyp As Integer
        Dim cbuff As String = Nothing

        On Error GoTo ErrHand

        If pUci.FastFlag Then
            GetCommentBeforeBlock("CATEGORY", pComment)
        End If

        retcod = 0
        init = 1
        OmCode = HspfOmCode("CATEGORY")
        c = ""
        retkey = -1
        Do
            If pUci.FastFlag Then
                GetNextRecordFromBlock("CATEGORY", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -1
                Call REM_XBLOCKEX((Me.Uci), OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            If retcod = 10 Then Exit Do
            If rectyp = 0 Then
                If Len(Trim(Mid(cbuff, 4, 2))) > 0 Then
                    lCategory.Tag = StrRetRem(cbuff)
                Else
                    lCategory.Tag = ""
                End If
                lCategory.Name = cbuff
                lCategory.Comment = c
                pCategories.Add(lCategory)
                c = ""
            ElseIf rectyp = -1 And init = 0 Then  'dont save first comment, its the header
                'save comment
                If Len(c) = 0 Then
                    c = cbuff
                Else
                    c = c & vbCrLf & cbuff
                End If
            ElseIf retcod = 2 And rectyp = -2 Then
                'save blank line
                If Len(c) = 0 Then
                    c = " "
                Else
                    c = c & vbCrLf & " "
                End If
            End If
            init = 0
        Loop

        Exit Sub

ErrHand:
        Logger.Msg(Err.Description & vbCr & vbCr & cbuff, MsgBoxStyle.Critical, "Error in ReadUciFile")
    End Sub

    Public Overrides Function ToString() As String
        Dim lSb As New StringBuilder
        If pComment.Length > 0 Then
            lSb.AppendLine(pComment)
        End If
        lSb.AppendLine("CATEGORY")
        lSb.AppendLine("   <> <----catnam----> *** ")
        For Each lCategory As HspfData.HspfCategory In pCategories
            If lCategory.Comment.Length > 0 Then
                lSb.AppendLine(lCategory.Comment)
            End If
            lSb.AppendLine(Space(3) & lCategory.Tag & Space(1) & lCategory.Name)
        Next
        lSb.AppendLine("END CATEGORY")
        Return lSb.ToString
    End Function
End Class