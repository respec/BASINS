Option Strict Off
Option Explicit On
Imports MapWinUtility

Public Enum atcUnitSystem As Integer
    atcUnknown = 0
    atcEnglish = 1
    atcMetric = 2
End Enum

Public Module modUnits
    '##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Const UnitsTableName As String = "Units"
    Private Const CategoryTableName As String = "Category"
    Private Const SwatTableName As String = "SwatDbfParameter"

    Private pSaveUnitsDatabase As Chilkat.Xml = Nothing
    Private pAlreadyReportedErrOpen As Boolean

    'Debug.Print GetParameterUnits("LAI", "SwatDbfParameter", "sbs")
    Public Function GetParameterUnits(ByVal ParameterName As String, Optional ByVal FileType As String = "") As String
        'Static AlreadyReportedError As Boolean
        Dim table As Chilkat.Xml = GetTable(SwatTableName)
        Dim row As Chilkat.Xml
        Dim unitsID As String = ""

        On Error GoTo errHand

        GetParameterUnits = "Unknown"

        If Not table Is Nothing Then
            If Len(FileType) = 0 Then
                row = table.ExtractChildByName("Row", "Name", ParameterName)
            Else
                row = table.FirstChild 'Find row matching both ParameterName and FileType
                While Not row Is Nothing AndAlso _
                  (row.GetAttrValue("Name") <> ParameterName OrElse _
                   row.GetAttrValue("FileType") <> FileType)
                    If Not row.NextSibling2 Then row = Nothing
                End While
            End If
            If Not row Is Nothing Then 'Found a row with the correct ParameterName (and, if specified, FileType)
                unitsID = row.GetAttrValue("UnitsID")
                If IsNumeric(unitsID) Then
                    table = GetTable(UnitsTableName)
                    If Not table Is Nothing Then
                        row = table.ExtractChildByName("Row", "ID", unitsID)
                        If Not row Is Nothing Then
                            Return row.GetAttrValue("Name")
                        End If
                    End If
                End If
            End If
        End If
        Exit Function

errHand:
        'If Not AlreadyReportedError Then
        Logger.Msg("Error in GetParameterUnits" & vbCr & Err.Description)
        'AlreadyReportedError = True
        'End If
    End Function

    Public Function GetConversionFactor(ByVal fromUnits As String, ByVal toUnits As String) As Double
        'Static AlreadyReportedError As Boolean
        Dim table As Chilkat.Xml = GetTable(UnitsTableName)
        Dim ConversionFrom As String
        Dim ConversionTo As String

        On Error GoTo errHand

        If Not table Is Nothing Then
            ConversionFrom = table.ExtractChildByName("Row", "Name", fromUnits).GetAttrValue("PerDefaultUnit")
            ConversionTo = table.ExtractChildByName("Row", "Name", toUnits).GetAttrValue("PerDefaultUnit")
            If IsNumeric(ConversionFrom) And IsNumeric(ConversionTo) And ConversionFrom <> 0 Then
                GetConversionFactor = CDbl(ConversionTo) / CDbl(ConversionFrom)
            End If
        End If
        Exit Function

errHand:
        'If AlreadyReportedError Then
        'Logger.dbg("Error in GetConversionFactor from: " & fromUnits & " to: " & toUnits & vbCr & Err.Description)
        'Else
        Logger.Msg("Cound not find factor from: " & fromUnits & " to: " & toUnits & vbCr & Err.Description, "ATCutility.modUnits.GetConversionFactor")
        '  AlreadyReportedError = True
        'End If

    End Function

    Public Function GetUnitDescription(ByVal unitsName As String) As String
        Return GetTable(UnitsTableName).ExtractChildByName("Row", "Name", unitsName).GetAttrValue("Description")
    End Function

    Public Function GetUnitID(ByVal unitsName As String) As Integer
        Dim retval As String = GetTable(UnitsTableName).ExtractChildByName("Row", "Name", unitsName).GetAttrValue("ID")
        If IsNumeric(retval) Then
            Return CInt(retval)
        Else
            Return 0
        End If
    End Function

    Public Function GetUnitName(ByVal unitsID As Integer) As String
        Return GetTable(UnitsTableName).ExtractChildByName("Row", "ID", CStr(unitsID)).GetAttrValue("Name")
    End Function

    Public Function GetUnitCategory(ByVal unitsName As String) As String
        'Static AlreadyReportedError As Boolean
        Dim table As Chilkat.Xml = GetTable(UnitsTableName)
        Dim CategoryID As String

        On Error GoTo errHand
        GetUnitCategory = "Unknown"
        If Not table Is Nothing Then
            CategoryID = table.ExtractChildByName("Row", "Name", unitsName).GetAttrValue("CategoryID")
            If IsNumeric(CategoryID) Then
                table = GetTable(CategoryTableName)
                If Not table Is Nothing Then
                    GetUnitCategory = table.ExtractChildByName("Row", "ID", CategoryID).GetAttrValue("Name")
                End If
            End If
        End If
        Exit Function

errHand:
        'If Not AlreadyReportedError Then
        Logger.Msg("Could not Get Unit Category for '" & unitsName & "'" & vbCr & Err.Description, "ATCutility.modUnits.GetUnitCategory")
        'AlreadyReportedError = True
        'End If
    End Function

    'If Category = "all" then all units in all categories are returned
    Public Function GetAllUnitsInCategory(ByVal Category As String) As ArrayList 'of String
        'Static AlreadyReportedError As Boolean
        Dim CategoryID As String
        Dim unitsTable As Chilkat.Xml = GetTable(UnitsTableName)
        Dim row As Chilkat.Xml
        Dim retval As New ArrayList

        On Error GoTo errHand

        GetAllUnitsInCategory = retval 'We will try to add unit names below

        If Not unitsTable Is Nothing Then
            If LCase(Category) = "all" Then
                CategoryID = -1
            Else
                CategoryID = GetTable(CategoryTableName).ExtractChildByName("Row", "Name", Category).GetAttrValue("ID")
            End If

            row = unitsTable.FirstChild
            While Not row Is Nothing
                If CategoryID = -1 OrElse row.GetAttrValue("CategoryID") = CategoryID Then
                    retval.Add(row.GetAttrValue("Name"))
                End If
                If Not row.NextSibling2 Then row = Nothing
            End While
        End If
        Exit Function

errHand:
        'If Not AlreadyReportedError Then
        Logger.Msg("Error in GetAllUnitsInCategory for: " & Category & vbCr & Err.Description, "ATCutility.modUnits.GetAllUnitsInCategory")
        'AlreadyReportedError = True
        'End If
    End Function

    Public Function GetAllUnitCategories() As ArrayList 'of String
        Dim db As Chilkat.Xml = unitsDB()
        Dim retval As New ArrayList

        GetAllUnitCategoriesHelper(retval, db)
        Return retval

    End Function

    Private Sub GetAllUnitCategoriesHelper(ByRef aList As ArrayList, ByVal aXML As Chilkat.Xml)
        Dim xmlChild As Chilkat.Xml = aXML.FirstChild
        If Not xmlChild Is Nothing Then 'Loop through children
            Do
                GetAllUnitCategoriesHelper(aList, xmlChild)
            Loop While xmlChild.NextSibling2()
        End If

        'TODO: find the XML that matches, add the right names to aList
        'rs = GetRecordSet("Select Category.Name from Category ", " where Category.Name<>'Unknown'", "")
        'retval.Add((rs.Fields(0).value))
    End Sub

    Private Function GetTable(ByVal aTableName As String) As Chilkat.Xml
        Dim db As Chilkat.Xml = unitsDB()
        If Not db Is Nothing Then
            Return db.ExtractChildByName("Table", "Name", aTableName)
        End If
        Return Nothing
    End Function

    Private Function unitsDB() As Chilkat.Xml
        unitsDB = Nothing
        Dim DBpath As String = ""

        Try
            If pSaveUnitsDatabase Is Nothing Then
                pSaveUnitsDatabase = New Chilkat.Xml
                DBpath = FindFile("Please locate ATCoUnits.xml", "ATCoUnits.xml")
                If FileExists(DBpath) Then
                    If Not pSaveUnitsDatabase.LoadXmlFile(DBpath) Then
                        Logger.Msg("Could not open units database '" & DBpath & "'" & vbCrLf & pSaveUnitsDatabase.LastErrorText, "ATCutility.modUnits")
                    End If
                End If
            End If
            unitsDB = pSaveUnitsDatabase

        Catch e As Exception
            If Not pAlreadyReportedErrOpen Then
                Logger.Msg("Error opening units database '" & DBpath & "'" & vbCr & Err.Description, "ATCutility.modUnits")
                pAlreadyReportedErrOpen = True
            End If
        End Try
    End Function

    Public Function UnitSystem(ByVal aUnitSystem As atcUnitSystem) As String
        Select Case aUnitSystem
            Case atcUnitSystem.atcEnglish : Return "English"
            Case atcUnitSystem.atcMetric : Return "Metric"
            Case Else : Return "Unknown"
        End Select
        Return Nothing
    End Function
End Module