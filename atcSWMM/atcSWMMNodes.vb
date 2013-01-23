Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMNodes
    Inherits KeyedCollection(Of String, atcSWMMNode)
    Implements IBlock

    Protected Overrides Function GetKeyForItem(ByVal aNode As atcSWMMNode) As String
        Dim lKey As String = aNode.Name
        Return lKey
    End Function

    Private pName As String = "[JUNCTIONS]" 'Note: also contains [OUTFALLS]
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lNode As atcSWMMNode In aEnumerable
            Me.Add(lNode)
        Next
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lType As String = lLines(0).TrimStart("[").TrimEnd("]")
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 2 Then
                lLines(I) = lLines(I).TrimStart(" ")
                Dim lItems As Generic.List(Of String) = atcSWMMProject.SplitSpaceDelimitedWithQuotes(lLines(I).Trim())
                Dim lNode As atcSWMMNode = Me(lItems(0))
                If lNode Is Nothing Then
                    lNode = New atcSWMMNode
                End If
                lNode.Type = lType
                For J As Integer = 0 To lItems.Count - 1
                    Select Case J
                        Case 0 : lNode.Name = lItems(J)
                        Case 1
                            If Not lType.ToLower = "coordinates" Then
                                lNode.InvertElevation = Double.Parse(lItems(J))
                            Else
                                lNode.XPos = Double.Parse(lItems(J))
                            End If
                        Case 2
                            If lType.ToLower = "junctions" Or lType.ToLower = "storage" Then
                                lNode.MaxDepth = Double.Parse(lItems(J))
                            ElseIf lType.ToLower = "outfalls" Then
                                lNode.OutfallType = lItems(J).Trim()
                            ElseIf lType.ToLower = "coordinates" Then
                                lNode.YPos = Double.Parse(lItems(J))
                            End If
                        Case 3
                            If lType.ToLower = "junctions" Or lType.ToLower = "storage" Then
                                lNode.InitDepth = Double.Parse(lItems(J))
                            ElseIf lType.ToLower = "outfalls" Then
                                lNode.StageTable = lItems(J).Trim() 'not sure how
                            End If
                        Case 4
                            If lType.ToLower = "junctions" Then
                                lNode.SurchargeDepth = Double.Parse(lItems(J))
                            ElseIf lType.ToLower = "outfalls" Then
                                lNode.TideGate = lItems(J).Trim()
                            ElseIf lType.ToLower = "storage" Then
                                lNode.ShapeCurve = lItems(J).Trim()
                            End If
                        Case 5
                            If lType.ToLower = "junctions" Then
                                lNode.PondedArea = Double.Parse(lItems(J))
                            ElseIf lType.ToLower = "storage" Then

                            End If

                    End Select
                Next

                If Not Me.Contains(lItems(0)) Then
                    Me.Add(lNode)
                    Me.ChangeItemKey(lNode, lItems(0).Trim())
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder

        lString.Append(pName & vbCrLf & _
                       ";;               Invert     Max.       Init.      Surcharge  Ponded    " & vbCrLf & _
                       ";;Name           Elev.      Depth      Depth      Depth      Area      " & vbCrLf & _
                       ";;-------------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lNode As atcSWMMNode In Me
            If lNode.Type = "JUNCTION" Then
                With lNode
                    lString.Append(StrPad(.Name, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InvertElevation, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.MaxDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InitDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.SurchargeDepth, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.PondedArea, "0.0"), 10, " ", False))
                    lString.Append(vbCrLf)
                End With
            End If
        Next

        lString.Append(vbCrLf)
        lString.Append("[OUTFALLS]" & vbCrLf & _
                       ";;               Invert     Outfall    Stage/Table      Tide" & vbCrLf & _
                       ";;Name           Elev.      Type       Time Series      Gate" & vbCrLf & _
                       ";;-------------- ---------- ---------- ---------------- ----" & vbCrLf)

        For Each lNode As atcSWMMNode In Me
            If lNode.Type = "OUTFALL" Then
                With lNode
                    lString.Append(StrPad(.Name, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(.InvertElevation, "0.0"), 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.OutfallType, 10, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.StageTable, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(.TideGate, 4, " ", False))
                    lString.Append(vbCrLf)
                End With
            End If
        Next

        Return lString.ToString
    End Function

    Public Function CoordinatesToString() As String
        Dim lString As New StringBuilder

        lString.Append("[COORDINATES]" & vbCrLf & _
                       ";;Node           X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lNode As atcSWMMNode In Me
            With lNode
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.XPos, "0.000"), 18, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.YPos, "0.000"), 18, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function
End Class

Public Class atcSWMMNode
    Public Name As String = ""
    Public Type As String = "" 'junction or outfall or storage unit
    Public InvertElevation As Double = -1.0 'in feet or meters
    Public MaxDepth As Double = -1.0
    Public InitDepth As Double = -1.0
    Public SurchargeDepth As Double = -1.0
    Public PondedArea As Double = -1.0
    Public OutfallType As String = ""
    Public StageTable As String = ""
    Public TideGate As String = ""
    Public ShapeCurve As String = String.Empty 'storage
    Public YPos As Double = 0.0
    Public XPos As Double = 0.0
End Class
