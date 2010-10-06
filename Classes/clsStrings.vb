'********************************************************************************************************
'File Name: clsStrings.vb
'Description: Useful utility functions dealing with or relating to strings.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Public Class Strings
    ''' <summary>
    ''' Divides string into 2 portions at position of 1st occurence of specified delimeter. Quote specifies a particular string that is exempt from the delimeter search.
    ''' </summary>
    ''' <param name="aSource">String to be analyzed</param>
    ''' <param name="aDelim">Single-character string delimeter</param>
    ''' <param name="aQuote">Multi-character string exempted from search</param>
    ''' <returns>
    ''' Returns leading portion of incoming string up to first occurence of delimeter.  
    ''' Returns input parameter without that portion. 
    ''' If no delimiter in string, returns whole string, and input parameter reduced to null string.
    ''' </returns>
    ''' <remarks>
    ''' Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
    ''' Example: StrSplit("'Julie, Todd', Jane, and Ray", ",", "'") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
    ''' </remarks>
    Public Shared Function StrSplit(ByRef aSource As String, ByRef aDelim As String, ByRef aQuote As String) As String
        Dim lRetval As String 'string to return as StrSplit
        Dim lQuotePosition As Integer 'character position of search through Source
        Dim lQuoted As Boolean = False 'whether quote was encountered in Source
        Dim lTrimLength As Integer 'long length of delimeter, or quote if encountered first
        Dim lQuoteLength As Integer = aQuote.Length 'length of quote

        aSource = aSource.TrimStart  'remove leading blanks
        If lQuoteLength > 0 Then
            lQuotePosition = aSource.IndexOf(aQuote)
            If lQuotePosition = 0 Then 'string beginning
                lTrimLength = lQuoteLength
                aSource = aSource.Substring(lTrimLength)
                lQuotePosition = aSource.IndexOf(aQuote) 'string end
                lQuoted = True
            Else
                lQuotePosition = aSource.IndexOf(aDelim)
                lTrimLength = aDelim.Length
            End If
        Else
            lQuotePosition = aSource.IndexOf(aDelim)
            lTrimLength = aDelim.Length
        End If

        If lQuotePosition > -1 Then 'found delimeter
            lRetval = aSource.Substring(0, lQuotePosition).TrimEnd  'string to return
            aSource = aSource.Substring(lQuotePosition + lTrimLength).Trim 'string remaining
            If lQuoted And aSource.Length > 0 Then
                If aSource.StartsWith(aDelim) Then
                    aSource = aSource.Substring(aDelim.Length + 1).Trim
                End If
            End If
        Else 'take it all
            lRetval = aSource
            aSource = "" 'nothing left
        End If

        Return lRetval
    End Function

    ''' <summary>
    ''' Converts specified text file to a string.
    ''' </summary>
    ''' <param name="aFilename">Name of text file to read</param>
    ''' <param name="aTimeoutMilliseconds">Keep trying for this much time</param>
    ''' <returns>Contents of specified text file as string.</returns>
    ''' <remarks></remarks>
    Public Shared Function WholeFileString(ByRef aFilename As String, Optional ByVal aTimeoutMilliseconds As Integer = 1000) As String
        Dim lTryUntil As Date = Now.AddMilliseconds(aTimeoutMilliseconds)
        While lTryUntil > Now
            Try
                Return IO.File.ReadAllText(aFilename)
            Catch ex As Exception
                If Now > lTryUntil Then
                    Return ""
                Else
                    System.Threading.Thread.Sleep(50)
                End If
            End Try
        End While
        Return ""
    End Function

    ''' <summary>
    ''' See if a String is empty or Nothing
    ''' </summary>
    ''' <param name="aString">String to check</param>
    ''' <returns>True is empty or Nothing, otherwise False</returns>
    ''' <remarks></remarks>
    Public Shared Function IsEmpty(ByVal aString As String) As Boolean
        If aString Is Nothing OrElse aString.Length < 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Saves incoming string to a text file.
    ''' </summary>
    ''' <param name="aFileName">Name of output text file</param>
    ''' <param name="aFileContents">Incoming string to be saved to file</param>
    ''' <remarks></remarks>
    Public Shared Sub SaveFileString(ByRef aFileName As String, ByRef aFileContents As String)
        Try
            Dim lDirectory As String = System.IO.Path.GetDirectoryName(aFileName)
            If Not IO.Directory.Exists(lDirectory) Then
                IO.Directory.CreateDirectory(lDirectory)
            End If
            IO.File.WriteAllText(aFileName, aFileContents)
        Catch lEx As Exception
            Logger.Dbg("SaveFileStringFailed " & lEx.ToString)
        End Try
    End Sub
End Class
