'********************************************************************************************************
'File Name: clsDataManagement.vb
'Description: "Data Management" related functions such as checking for metadata, moving and deleting shapefiles, etc.
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

Public Class DataManagement
    'TODO: DeleteShapeFile() {delete all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'TODO: MoveShapeFile() {move all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'TODO: RenameShapeFile() {rename all shape files, including dbf, shp, shx, prj, aux, xml, etc.)
    'NOTE: These are all implemented in GeoProc already

    Public Shared Function MetaDataExists(ByVal FileName As String) As Boolean
        If GetMetaDataFiles(FileName) Is Nothing Then Return False
        Return True
    End Function

    Public Shared Function GetMetaDataFiles(ByVal FileName As String) As String()
        Try
            Dim tStr As String
            Dim list As New ArrayList

            'if this is a directory (as in the case  of ESRI Grids)
            'chop off the '\' character
            If FileName.Chars(FileName.Length - 1) = "\"c Then
                FileName = FileName.Substring(0, FileName.Length - 1)
            End If

            ' try just adding the extension to test 
            tStr = FileName & ".htm"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            tStr = FileName & ".html"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            tStr = FileName & ".xml"
            If System.IO.File.Exists(tStr) Then
                list.Add(tStr)
            End If

            'now try chopping off the original extension, and then adding the one we want to test
            Dim ext As String = System.IO.Path.GetExtension(FileName)
            FileName = FileName.Substring(0, FileName.Length - ext.Length)

            tStr = FileName & ".htm"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            tStr = FileName & ".html"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            tStr = FileName & ".xml"
            If System.IO.File.Exists(tStr) And list.Contains(tStr) = False Then
                list.Add(tStr)
            End If

            If list.Count = 0 Then
                Return Nothing
            Else
                Return CType(list.ToArray(GetType(String)), String())
            End If
        Catch
            Return Nothing
        End Try
    End Function
    '''<summary>
    '''Chris Michaelis July 2 2005
    '''Used to compare write times between the first and second file.
    '''Returns true if File2 is newer than File1
    ''' </summary>
    Public Shared Function CheckFile2Newest(ByVal File1 As String, ByVal File2 As String, Optional ByVal SameIfWithinXMinutes As Single = 2) As Boolean
        If System.IO.File.Exists(File1) And System.IO.File.Exists(File2) Then
            ' Allow a variance of up to 3 minutes. Slower systems will have the image
            ' write date a bit after the grid write date. If it takes longer than 3 minutes
            ' it's either a very large grid or a very slow computer - either way, the additional
            ' delay is noncritical.
            Dim f1_d As Date = System.IO.File.GetLastWriteTime(File1)
            Dim f2_d As Date = System.IO.File.GetLastWriteTime(File2)

            If f1_d.Equals(f2_d) Then Return True
            If Math.Abs(f1_d.Subtract(f2_d).TotalMinutes) < SameIfWithinXMinutes Then Return True

            'If the image was made *after* the grid this is OK as well.
            If (f1_d.Subtract(f2_d).TotalMinutes < 0) Then Return True

            Return False
        Else
            ' They don't match if they don't exist...
            Return False
        End If
    End Function
End Class
