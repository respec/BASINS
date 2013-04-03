﻿''' <summary>
''' Class to hold the specification for the time series to be used for a given input (new for version 2.0)
''' </summary>
''' <remarks>Time series specifications can be stored in strings and contain all information needed to do later query to actually get the data</remarks>
Public Class clsTimeSeriesSelection

    ''' <summary>
    ''' Symbol used to delimit time series string
    ''' </summary>
    Public Const DELIMITER As String = "~"

    ''' <summary>Standard selection types</summary>
    Public Enum enumSelectionType
        None
        Constant
        WRDB
        Database
    End Enum
    Friend Shared SelectionName() As String = {"NONE", "CONST", "WRDB", "DATA"}

    ''' <summary>Standard conversion factors</summary>
    Public Enum enumConversion
        None
        Cfs_Cms
        Mgd_Cms
        F_C
        Lb_Kg
        Hr_Dy
        Joules
        In_M
        Hr_Sec
    End Enum
    Public Shared ConversionText() As String = {"None", "CFS→CMS", "MGD→CMS", "°F→°C", "lb/day→Kg/day", "lang/hr→lang/day", "joule/m²/sec→lang/day", "in→m", "m/hr→m/sec"}
    Public Shared ConversionFactor() As String = {"1.00", "0.028317", "0.043813", "F2C", "0.45359", "24", "2.06500", "0.025400", "0.00027778"}

    Public SelectionType As enumSelectionType = enumSelectionType.None
    Public ConversionType As enumConversion = enumConversion.None
    Public ScaleFactor As Double = 1.0
    Public DataSource As String 'contains constant value, BASINS datasource name, WRDB Project Name, or full path to database file compatible with WRDBData
    Public Table As String = ""
    Public StationID As String = ""
    Public PCode As String = ""
    Private UseMapping As Boolean = False

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Instantiate time series selection
    ''' </summary>
    ''' <param name="aSelectionType">Type of selection source</param>
    ''' <param name="aConversion">Conversion multiplier string</param>
    ''' <param name="aDataSource">Name of data source (constant value, BASINS datasource name, WRDB project, or full path to database file</param>
    ''' <param name="aTable">Name of table if WRDB project or database</param>
    ''' <param name="aStationID">Selected Station ID</param>
    ''' <param name="aPCode">Selected PCode</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aSelectionType As enumSelectionType, Optional ByVal aConversion As enumConversion = enumConversion.None, Optional ByVal aScaleFactor As Double = 1.0, Optional ByVal aDataSource As String = "", Optional ByVal aTable As String = "", Optional ByVal aStationID As String = "", Optional ByVal aPCode As String = "")
        SelectionType = aSelectionType
        ConversionType = aConversion
        ScaleFactor = aScaleFactor
        DataSource = aDataSource
        Table = aTable
        If SelectionType = enumSelectionType.Database AndAlso IO.Path.GetFileNameWithoutExtension(DataSource) = Table Then Table = "" 'don't repeat
        StationID = aStationID
        PCode = aPCode
        If StationID = "" And PCode = "" Then UseMapping = True
    End Sub

    ''' <summary>
    ''' Instantiate time series selection given full selection string
    ''' </summary>
    ''' <param name="FullSelectionString">Full coded selection string specification in the form A~B~C~...</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal FullSelectionString As String)
        With FromFullString(FullSelectionString)
            Me.ConversionType = .ConversionType
            Me.ScaleFactor = .ScaleFactor
            Me.DataSource = .DataSource
            Me.PCode = .PCode
            Me.StationID = .StationID
            Me.SelectionType = .SelectionType
            Me.Table = .Table
        End With
    End Sub

    ''' <summary>
    ''' Return the string representation of the conversion type
    ''' </summary>
    Public Shared Function GetConversionName(ByVal ConversionType As enumConversion) As String
        Return [Enum].GetName(GetType(enumConversion), ConversionType)
    End Function

    ''' <summary>
    ''' Get the conversion type based on the string representation
    ''' </summary>
    ''' <param name="Name">Conversion name</param>
    Public Shared Function GetConversionType(ByVal Name As String) As enumConversion
        Return [Enum].Parse(GetType(enumConversion), Name)
    End Function

    ''' <summary>
    ''' Return the string representation of the conversion type
    ''' </summary>
    Public Function GetConversionName() As String
        Return [Enum].GetName(GetType(enumConversion), ConversionType)
    End Function

    ''' <summary>
    ''' Set the conversion type based on the string representation
    ''' </summary>
    ''' <param name="Name">Conversion name</param>
    Public Sub SetConversion(ByVal Name As String)
        ConversionType = [Enum].Parse(GetType(enumConversion), Name)
    End Sub

    ''' <summary>
    ''' Return short string to be displayed in grid (if None, return selection string; if database, only filename will be included, not full path)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Must store instance of this type in cell's tag so can retrieve full path to database file</remarks>
    Public Overrides Function ToString() As String
        Select Case SelectionType
            Case enumSelectionType.None
                Return "Click to select..."
            Case enumSelectionType.Constant, enumSelectionType.WRDB
                Return ToFullString()
            Case enumSelectionType.Database
                Dim s As String = String.Format("{1}{0}{2}{0}{3}", DELIMITER, SelectionName(SelectionType), IO.Path.GetFileName(DataSource), Table)
                If StationID <> "" Then s &= String.Format("{0}{1}{0}{2}", DELIMITER, StationID, PCode)
                If ConversionType <> enumConversion.None Or ScaleFactor <> 1.0 Then s &= String.Format("{0}{1}{0}{2}", DELIMITER, GetConversionName, ScaleFactor)
                Return s
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Return full string to be saved in file
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToFullString() As String
        Dim s As String = ""
        Select Case SelectionType
            Case enumSelectionType.None
                s = SelectionName(SelectionType)
            Case enumSelectionType.Constant
                s = String.Format("{1}{0}{2}", DELIMITER, SelectionName(SelectionType), DataSource)
            Case enumSelectionType.WRDB, enumSelectionType.Database
                s = String.Format("{1}{0}{2}{0}{3}", DELIMITER, SelectionName(SelectionType), DataSource, Table)
                If StationID <> "" Or PCode <> "" Or ConversionType <> enumConversion.None Then s &= String.Format("{0}{1}{0}{2}", DELIMITER, StationID, PCode)
                If ConversionType <> enumConversion.None Or ScaleFactor <> 1.0 Then s &= String.Format("{0}{1}{0}{2}", DELIMITER, GetConversionName, ScaleFactor)
            Case Else
                s = ""
        End Select
        Return s
    End Function

    ''' <summary>
    ''' Create instance given full selection string (better to use New)
    ''' </summary>
    ''' <param name="FullSelectionString">Full string in the form A~B~C~...</param>
    Shared Function FromFullString(ByVal FullSelectionString As String) As clsTimeSeriesSelection
        Dim ar() As String = FullSelectionString.Split(DELIMITER)
        Dim sel As New clsTimeSeriesSelection

        sel.SelectionType = Array.IndexOf(SelectionName, ar(0))          'first part is always type

        Select Case sel.SelectionType
            Case enumSelectionType.Constant
                sel.DataSource = ar(1)                                   'second is either constant value or basins timeseries name
            Case enumSelectionType.WRDB, enumSelectionType.Database
                sel.DataSource = ar(1)                                   'second is project name or database name
                sel.Table = ar(2)                                        'third is table name
                If ar.Length >= 5 Then
                    sel.StationID = ar(3)                                'fourth is station ID (optional; defaults to null meaning that mapping is to be used)
                    sel.PCode = ar(4)                                    'fifth is pcode (optional; defaults to null meaning that mapping is to be used)
                End If
                If ar.Length >= 6 Then GetConversion(ar(5), sel) '        sixth is conversion factor (optional; defaults to None)
                If ar.Length >= 7 Then sel.ScaleFactor = Val(ar(6                                               )) '     seventh is scale factor (optional; defaults to 1.0)
        End Select

        Return sel
    End Function

    ''' <summary>
    ''' Given conversion multiplier string, set conversion properties in selection
    ''' </summary>
    ''' <param name="Conversion">Conversion string</param>
    ''' <param name="sel">Instance of selection</param>
    Private Shared Sub GetConversion(ByVal Conversion As String, ByRef sel As clsTimeSeriesSelection)
        sel.ConversionType = [Enum].Parse(GetType(enumConversion), Conversion)
    End Sub

    Private Function ConvertedValue(ByVal Value As Double, ByVal ConversionType As enumConversion, ByVal ScaleFactor As Double) As Double
        Select Case ConversionType
            Case enumConversion.F_C
                Return (Value - 32.0) * 5.0 / 9.0 'don't apply scale factor to temperature conversions
            Case enumConversion.None
                Return Value * ScaleFactor
            Case Else
                Return (Value * ConversionFactor(ConversionType)) * ScaleFactor
        End Select
    End Function

    ''' <summary>
    ''' Return time series data points
    ''' </summary>
    ''' <param name="WaspProject">WASP Project that is currently active</param>
    ''' <param name="MappedStation">Because time series may be using mapping, this is 'Time Functions' or name of Segment</param>
    ''' <param name="MappedParameter">'Input Flows', Constituent Name, or Time Function Name in case using PCode mapping</param>
    ''' <returns>Sorted list of dates and time series values</returns>
    Public Function GetTimeSeries(ByRef WaspProject As atcWASP.atcWASPProject, ByVal MappedStation As String, ByVal MappedParameter As String) As SortedList(Of Date, Double)

        Dim ts As Generic.SortedList(Of Date, Double) = Nothing
        ts = New Generic.SortedList(Of Date, Double)

        Dim Sta As String = StationID
        Dim Pcd As String = PCode
        Dim Cnv As String = ConversionType
        Dim Scl As Double = ScaleFactor

        Select Case SelectionType
            Case enumSelectionType.WRDB, enumSelectionType.Database
                If String.IsNullOrEmpty(Sta) Then 'must use mapping
                    With WaspProject.StationMapping
                        If .ContainsKey(MappedStation) Then Sta = .Item(MappedStation)
                    End With
                End If
                If String.IsNullOrEmpty(Pcd) Then 'must use mapping
                    With WaspProject.PCodeMapping
                        If .ContainsKey(MappedParameter) Then
                            With .Item(MappedParameter)
                                Pcd = .PCode
                                Cnv = .ConversionType
                                Scl = .ScaleFactor
                            End With
                        End If
                    End With
                End If
        End Select

        Select Case SelectionType
            Case enumSelectionType.None
                'will just return empty timeseries
                'do not ever want to put zero-length time series in Wasp inp file; instead, put two point time series with zero values
                If ts.Count = 0 Then
                    ts.Add(WaspProject.SDate, 0)
                    ts.Add(WaspProject.EDate, 0)
                End If

            Case enumSelectionType.Constant
                ts.Add(WaspProject.SDate, ConvertedValue(Val(DataSource), Cnv, Scl))
                ts.Add(WaspProject.EDate, ConvertedValue(Val(DataSource), Cnv, Scl))

            Case enumSelectionType.WRDB
                Dim Project As New WRDB.Project.clsProject(DataSource)
                With Project
                    Dim sql As String = String.Format("SELECT Date_Time,Result FROM {0} WHERE Station_ID='{1}' AND PCode='{2}' AND Date_Time BETWEEN {3} AND {4} ORDER BY Date_Time", _
                                                      Me.Table, Sta, Pcd, .DB.DateTime(WaspProject.SDate), .DB.DateTime(WaspProject.EDate))
                    Dim dr As Common.DbDataReader = .DB.ExecuteReader(sql)
                    While dr IsNot Nothing AndAlso dr.Read
                        If Not dr.IsDBNull(1) Then
                            Dim dt As Date = dr.GetDateTime(0)
                            Dim val As Double = ConvertedValue(dr.GetValue(1), Cnv, Scl)
                            If dt >= WaspProject.SDate AndAlso dt <= WaspProject.EDate AndAlso Not ts.ContainsKey(dt) Then ts.Add(dt, val)
                        End If
                    End While
                    dr.Close()
                End With

            Case enumSelectionType.Database
                Dim DS As WRDB.DataSource.clsData = WRDB.DataSource.clsData.Instantiate(Me.DataSource)
                If Table <> "" Then DS.ActiveTable = Table

                Dim dt As DataTable = Nothing

                Try
                    dt = DS.GetDataTable(Sta, Pcd)
                Catch ex As Exception
                    'just let it return an empty time series--will flag later
                    Return ts
                End Try

                If dt IsNot Nothing Then
                    For r As Integer = 0 To dt.Rows.Count - 1
                        With dt.Rows(r)
                            If Not IsDBNull(.Item(0)) And Not IsDBNull(.Item(1)) Then
                                If .Item(0) >= WaspProject.SDate AndAlso .Item(0) <= WaspProject.EDate Then
                                    ts.Add(.Item(0), ConvertedValue(.Item(1), Cnv, Scl))
                                End If
                            End If
                        End With
                    Next
                End If
        End Select

        Return ts
    End Function

End Class
