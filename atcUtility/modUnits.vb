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

    Private pSaveUnitsDatabase As Xml.XmlDocument = Nothing
    Private pAlreadyReportedErrOpen As Boolean

    'Debug.Print GetParameterUnits("LAI", "SwatDbfParameter", "sbs")
    Public Function GetParameterUnits(ByVal ParameterName As String, Optional ByVal FileType As String = "") As String
        'Static AlreadyReportedError As Boolean
        Dim table As Xml.XmlNode = GetTable(SwatTableName)
        Dim row As Xml.XmlNode = Nothing
        Dim unitsID As String = ""

        On Error GoTo errHand

        GetParameterUnits = "Unknown"

        If Not table Is Nothing Then
            If Len(FileType) = 0 Then
                row = ExtractChildByName(table, "Row", "Name", ParameterName)
            Else
                For Each row In table.ChildNodes 'Find row matching both ParameterName and FileType
                    If row.Attributes.GetNamedItem("Name").InnerText = ParameterName AndAlso _
                       row.Attributes.GetNamedItem("FileType").InnerText = FileType Then Exit For
                Next
            End If
            If Not row Is Nothing Then 'Found a row with the correct ParameterName (and, if specified, FileType)
                unitsID = row.Attributes.GetNamedItem("UnitsID").InnerText
                If IsNumeric(unitsID) Then
                    table = GetTable(UnitsTableName)
                    If Not table Is Nothing Then
                        row = ExtractChildByName(table, "Row", "ID", unitsID)
                        If Not row Is Nothing Then
                            Return row.Attributes.GetNamedItem("Name").InnerText
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
        Dim lConversionFactor As Double = GetNaN()
        Try
            Dim lUnitsTableNameNode As Xml.XmlNode = GetTable(UnitsTableName)
            If lUnitsTableNameNode IsNot Nothing Then
                Dim ConversionFrom As String = ExtractChildByName(lUnitsTableNameNode, "Row", "Name", fromUnits).Attributes.GetNamedItem("PerDefaultUnit").InnerText
                Dim ConversionTo As String = ExtractChildByName(lUnitsTableNameNode, "Row", "Name", toUnits).Attributes.GetNamedItem("PerDefaultUnit").InnerText
                If IsNumeric(ConversionFrom) AndAlso IsNumeric(ConversionTo) AndAlso ConversionFrom <> 0 Then
                    lConversionFactor = CDbl(ConversionTo) / CDbl(ConversionFrom)
                End If
            End If
        Catch lEx As Exception
            'If AlreadyReportedError Then
            'Logger.dbg("Error in GetConversionFactor from: " & fromUnits & " to: " & toUnits & vbCr & Err.Description)
            'Else
            Logger.Msg("Cound not find factor from: " & fromUnits & " to: " & toUnits & vbCr & lEx.Message, "ATCutility.modUnits.GetConversionFactor")
            '  AlreadyReportedError = True
            'End If
        End Try
        Return lConversionFactor
    End Function

    Public Function GetUnitDescription(ByVal unitsName As String) As String
        Return ExtractChildByName(GetTable(UnitsTableName), "Row", "Name", unitsName).Attributes.GetNamedItem("Description").InnerText
    End Function

    Public Function GetUnitID(ByVal unitsName As String) As Integer
        Dim retval As String = ExtractChildByName(GetTable(UnitsTableName), "Row", "Name", unitsName).Attributes.GetNamedItem("ID").InnerText
        If IsNumeric(retval) Then
            Return CInt(retval)
        Else
            Return 0
        End If
    End Function

    Public Function GetUnitName(ByVal unitsID As Integer) As String
        Return ExtractChildByName(GetTable(UnitsTableName), "Row", "ID", CStr(unitsID)).Attributes.GetNamedItem("Name").InnerText
    End Function

    Public Function GetUnitCategory(ByVal unitsName As String) As String
        'Static AlreadyReportedError As Boolean
        Dim table As Xml.XmlNode = GetTable(UnitsTableName)
        Dim CategoryID As String

        On Error GoTo errHand
        GetUnitCategory = "Unknown"
        If Not table Is Nothing Then
            CategoryID = ExtractChildByName(table, "Row", "Name", unitsName).Attributes.GetNamedItem("CategoryID").InnerText
            If IsNumeric(CategoryID) Then
                table = GetTable(CategoryTableName)
                If Not table Is Nothing Then
                    GetUnitCategory = ExtractChildByName(table, "Row", "ID", CategoryID).Attributes.GetNamedItem("Name").InnerText
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
        Dim unitsTable As Xml.XmlNode = GetTable(UnitsTableName)
        Dim row As Xml.XmlNode
        Dim retval As New ArrayList

        On Error GoTo errHand

        GetAllUnitsInCategory = retval 'We will try to add unit names below

        If Not unitsTable Is Nothing Then
            If LCase(Category) = "all" Then
                CategoryID = -1
            Else
                CategoryID = ExtractChildByName(GetTable(CategoryTableName), "Row", "Name", Category).Attributes.GetNamedItem("ID").InnerText
            End If

            For Each row In unitsTable.ChildNodes
                If CategoryID = -1 OrElse row.Attributes.GetNamedItem("CategoryID").InnerText = CategoryID Then
                    retval.Add(row.Attributes.GetNamedItem("Name").InnerText)
                End If
            Next
        End If
        Exit Function

errHand:
        'If Not AlreadyReportedError Then
        Logger.Msg("Error in GetAllUnitsInCategory for: " & Category & vbCr & Err.Description, "ATCutility.modUnits.GetAllUnitsInCategory")
        'AlreadyReportedError = True
        'End If
    End Function

    Public Function GetAllUnitCategories() As ArrayList 'of String
        Dim db As Xml.XmlNode = unitsDB()
        Dim retval As New ArrayList

        GetAllUnitCategoriesHelper(retval, db)
        Return retval

    End Function

    Private Sub GetAllUnitCategoriesHelper(ByRef aList As ArrayList, ByVal aXML As Xml.XmlNode)
        For Each xmlChild As Xml.XmlNode In aXML.ChildNodes
            GetAllUnitCategoriesHelper(aList, xmlChild)
        Next

        'TODO: find the XML that matches, add the right names to aList
        'rs = GetRecordSet("Select Category.Name from Category ", " where Category.Name<>'Unknown'", "")
        'retval.Add((rs.Fields(0).value))
    End Sub

    Private Function GetTable(ByVal aTableName As String) As Xml.XmlNode
        Dim db As Xml.XmlNode = unitsDB()
        If Not db Is Nothing Then
            Return ExtractChildByName(db, "Table", "Name", aTableName)
        End If
        Return Nothing
    End Function

    Private Function unitsDB() As Xml.XmlNode
        unitsDB = Nothing
        Dim DBpath As String = ""

        Try
            If pSaveUnitsDatabase Is Nothing Then
                DBpath = FindFile("Please locate ATCoUnits.xml", "ATCoUnits.xml")
                If FileExists(DBpath) Then
                    pSaveUnitsDatabase = New Xml.XmlDocument
                    pSaveUnitsDatabase.Load(DBpath)
                End If
            End If
            unitsDB = pSaveUnitsDatabase.FirstChild

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

    Private Function ExtractChildByName(ByVal aParent As Xml.XmlNode, ByVal aTag As String, ByVal aAttName As String, ByVal aAttValue As String) As Xml.XmlNode
        aTag = aTag.ToLower 'Make search for node name non-case-sensitive, but aValue is still case-sensitive
        For Each lChild As Xml.XmlNode In aParent.ChildNodes
            Try
                If lChild.Name.ToLower = aTag AndAlso lChild.Attributes.GetNamedItem(aAttName).InnerText = aAttValue Then
                    Return lChild
                End If
            Catch 'Ignore error if we can't find an attribute, just move on to next node
            End Try
        Next
        Return Nothing
    End Function
End Module