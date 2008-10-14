Option Strict On
Option Explicit On

''' <summary>
''' 
''' </summary>
''' <remarks>Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
Module HspfAddChar2Keyword
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public pLastOperationSerial As Integer

    ''' <summary>
    ''' adds a missing character to the end of an input keyword if needed
    ''' </summary>
    ''' <param name="aKeyword">keyword to add missing character to</param>
    ''' <returns>updated keyword</returns>
    ''' <remarks></remarks>
    Function AddChar2Keyword(ByRef aKeyword As String) As String
        Dim lKeyword As String = aKeyword

        Select Case lKeyword
            Case "MON-IFLW-CON" : lKeyword &= "C"
            Case "MON-GRND-CON" : lKeyword &= "C"
            Case "PEST-AD-FLAG" : lKeyword &= "S"
            Case "PHOS-AD-FLAG" : lKeyword &= "S"
            Case "TRAC-AD-FLAG" : lKeyword &= "S"
            Case "PLNK-AD-FLAG" : lKeyword &= "S"
            Case "HYDR-CATEGOR" : lKeyword &= "Y"
            Case Else
        End Select

        Return lKeyword
    End Function
End Module