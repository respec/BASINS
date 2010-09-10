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
    Private pCategories As Collection(Of HspfCategory)
    Friend Uci As HspfUci
    Public Comment As String
    Public ReadOnly Caption As String = "Category Block"
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlCategoryBlkEdit"

    Public ReadOnly Property Categories() As Collection(Of HspfCategory)
        Get
            Return pCategories
        End Get
    End Property

    Public Property Value(ByVal Index As Integer) As HspfCategory
        Get
            If Index > 0 And Index <= pCategories.Count Then
                'TODO: check this index
                Value = pCategories.Item(Index - 1)
            Else
                Value = New HspfCategory
                Value.Name = ""
                Value.Tag = ""
                Value.Id = pCategories.Count + 1
            End If
        End Get
        Set(ByVal Value As HspfCategory) '????
            If Index <= pCategories.Count Then
                pCategories.RemoveAt(Index)
                pCategories.Insert(Index, Value)
            ElseIf Index = pCategories.Count + 1 Then
                pCategories.Add(Value)
            Else 'error?
            End If
        End Set
    End Property

    Public Sub Clear()
        pCategories.Clear()
    End Sub

    Public Sub Add(ByRef aCategory As HspfCategory)
        pCategories.Add(aCategory)
    End Sub

    Public Sub AddFromSpecs(ByRef aName As String, ByRef aTag As String)
        Dim lCategory As New HspfCategory
        lCategory.Name = aName
        lCategory.Tag = aTag
        lCategory.Id = pCategories.Count + 1
        pCategories.Add(lCategory)
    End Sub

    Public Sub Remove(ByRef aIndex As Integer)
        If aIndex >= 0 And aIndex <= pCategories.Count Then
            pCategories.RemoveAt(aIndex)
        End If
    End Sub

    Public Sub New()
        MyBase.New()
        pCategories = New Collection(Of HspfCategory)
    End Sub

    Private Sub Update()
        Uci.Edited = True
    End Sub

    'Public Function Check() As String
    '	'verify values are correct in relation to each other and other tables
    '       Return ""
    'End Function

    Friend Sub ReadUciFile()

        Comment = GetCommentBeforeBlock("CATEGORY")

        Dim lInit As Integer = 1
        Dim lOmCode As Integer = HspfOmCode("CATEGORY")
        Dim lComment As String = ""
        Dim lRetKey As Integer = -1
        Dim lRetCod As Integer = 0
        Dim lRecTyp As Integer
        Dim lCBuff As String = Nothing

        Try
            Do
                GetNextRecordFromBlock("CATEGORY", lRetKey, lCBuff, lRecTyp, lRetCod)

                If lRetCod = 10 Then
                    Exit Do
                ElseIf lRecTyp = 0 Then
                    Dim lCategory As New HspfCategory
                    If lCBuff.Substring(3, 2).Trim.Length > 0 Then
                        lCategory.Tag = StrRetRem(lCBuff)
                    Else
                        lCategory.Tag = ""
                    End If
                    lCategory.Name = lCBuff.Trim
                    lCategory.Comment = lComment.Trim
                    lCategory.Id = pCategories.Count + 1
                    pCategories.Add(lCategory)
                    lComment = ""
                ElseIf lRecTyp = -1 And lInit = 0 Then  'dont save first comment, its the header
                    'save comment
                    If lComment.Length = 0 Then
                        lComment = lCBuff
                    Else
                        lComment &= vbCrLf & lCBuff
                    End If
                ElseIf lRetCod = 2 And lRecTyp = -2 Then
                    'save blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment &= vbCrLf & " "
                    End If
                End If
                lInit = 0
            Loop
        Catch lEx As ApplicationException
            Logger.Msg(lEx.Message & vbCr & vbCr & lCBuff, MsgBoxStyle.Critical, "Error in ReadUciFile")
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Dim lSb As New StringBuilder
        If Comment.Length > 0 Then
            lSb.AppendLine(Comment)
        End If
        lSb.AppendLine("CATEGORY")
        lSb.AppendLine("   <> <----catnam----> *** ")
        For Each lCategory As HspfCategory In pCategories
            If lCategory.Comment.Length > 0 Then
                lSb.AppendLine(lCategory.Comment)
            End If
            lSb.AppendLine(Space(3) & lCategory.Tag & Space(1) & lCategory.Name)
        Next
        lSb.AppendLine("END CATEGORY")
        Return lSb.ToString
    End Function
End Class

Public Class HspfCategory
    Public Id As Integer
    Public Tag As String
    Public Name As String
    Public Comment As String 'preceeding comment
End Class