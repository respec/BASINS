'********************************************************************************************************
'File Name: clsLog.vb
'Description: A simple log file creation class.
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

Imports System.IO
Public Class Log
    Private Shared fileName As String = "UtilsLog.txt"

    Public Shared Sub SetLogFile(ByVal newFilename As String)
        fileName = newFilename
    End Sub

    '''<summary>
    '''Deletes all previous error messages.
    '''</summary>
    Public Shared Sub ClearLog()
        If System.IO.File.Exists(fileName) Then
            System.IO.File.Delete(fileName)
        End If
    End Sub

    '''<summary>
    '''Provides access to the last message recieved through 
    '''the MapWinX library.
    '''</summary>
    '''<returns>A description of the problem encountered. 
    '''</returns>
    Public Shared Function GetLastMsg() As String
        Dim sr As StreamReader
        Dim sLine As String = ""
        Dim errorMsg As String = ""
        If File.Exists(fileName) Then
            Try
                sr = New StreamReader(fileName)
                Do
                    sLine = sr.ReadLine
                    If Not sLine Is Nothing Then errorMsg = sLine
                Loop While Not sLine Is Nothing

                sr.Close()
            Catch generatedExceptionVariable0 As IOException
                Return ("Error Reading File!!")
            End Try
        End If
        If errorMsg.Equals("") Then
            errorMsg = "No errors were recorded."
        End If
        Return errorMsg
    End Function

    '''<summary>
    '''Sets the last message recieved through
    '''the MapWinX library.
    '''</summary>
    '''<param name="Msg">A string describing the problem encountered or the message to write.</param>
    Public Shared Sub PutMsg(ByVal Msg As String)
        Dim sw As StreamWriter
        If Not File.Exists(fileName) Then
            Try
                sw = File.CreateText(fileName)
                sw.WriteLine(Now.ToString() + " " + Msg)
                sw.Close()
            Catch generatedExceptionVariable0 As IOException
                Debug.WriteLine("Error writing to file!!")
            End Try
        Else
            Try
                sw = File.AppendText(fileName)
                sw.WriteLine(Now.ToString() + " " + Msg)
                sw.Close()
            Catch generatedExceptionVariable0 As IOException
                Debug.WriteLine("Error writing to file!!!")
            End Try
        End If
    End Sub

End Class
