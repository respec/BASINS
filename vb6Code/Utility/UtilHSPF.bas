Attribute VB_Name = "UtilHSPF"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'returns first occurence of specified "Table" as a whole
'string, through 'END "Table"'
'UCIFile string is not modified
Public Function GetUCITable(UCIFile As String, Table As String) As String

  Dim retval As String, spos&, epos&

  'preceeding CR/LF indicates true start of table
  spos = InStr(UCIFile, vbCrLf & Table)
  epos = InStr(UCIFile, "END " & Table)
  If spos > 0 And epos > 0 And epos > spos Then
    spos = spos + 2 'don't include preceeding CR/LF
    retval = Mid(UCIFile, spos, epos + Len("END " & Table) - spos)
  Else
    retval = ""
  End If
  GetUCITable = retval

End Function

'Returns first substring of source up to delim
'Option to remove returned substring and delim from source
'Option to preserve blanks
Function StrSplitSave(Source$, delim$, quote$, RemoveString As Boolean, RemoveBlanks As Boolean) As String
    Dim retval$, i&, quoted As Boolean, trimlen&, quotlen&
    
    If RemoveBlanks Then Source = LTrim(Source) 'remove leading blanks
    quotlen = Len(quote)
    If quotlen > 0 Then
      i = InStr(Source, quote)
      If i = 1 Then 'string beginning
        trimlen = quotlen
        Source = Mid(Source, trimlen + 1)
        i = InStr(Source, quote) 'string end
        quoted = True
      Else
        i = InStr(Source, delim)
        trimlen = Len(delim)
      End If
    Else
      i = InStr(Source, delim)
      trimlen = Len(delim)
    End If
       
    If i > 0 Then 'found delimeter
      retval = Left(Source, i - 1) 'string to return
      If RemoveString Then 'take found string/delimeter out of source
        If RemoveBlanks Then 'trim preceeding blanks
          Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
        Else 'don't trim preceeding blanks
          Source = Mid(Source, i + trimlen) 'string remaining
        End If
        If quoted And Len(Source) > 0 Then
          If Left(Source, Len(delim)) = delim Then Source = Mid(Source, Len(delim) + 1)
        End If
      End If
    Else 'take it all
      retval = Source
      If RemoveString Then Source = "" 'nothing left
    End If
    
    StrSplitSave = retval
    
End Function

