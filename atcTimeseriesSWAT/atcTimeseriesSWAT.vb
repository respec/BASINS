Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Partial Public Class atcTimeseriesSWAT
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWAT Data Files (*.pcp, *.tmp, output.rch, .sub, .hru)|*.pcp;*.tmp;output.rch;output.sub;output.hru;output.hrux"
    Private Shared pNaN As Double = GetNaN()
    Private pTableDelimited As Boolean = False
    Private pBaseDataField As Integer
    Private pSubIdField As Integer
    Private pMONcontains As Integer = 0 'IPRINT from file.cio, 0=Monthly, 1=Daily, 2=Yearly
    Private pTimeUnit As atcTimeUnit = atcTimeUnit.TUMonth
    Private pYearBase As Integer = 1900
    Private pNumValues As Integer = 0
    Private pSaveSubwatershedId As Boolean = False
    Private pHeaderSize As Integer = 0
    Private pRecordLength As Integer = 0
    Private pDates As atcTimeseries = Nothing 'Can share dates since they will be the same for all ts in a file
    Private pType As String = ""

    Public Enum SWATDATATYPE
        SWATDATATYPE_pcp
        SWATDATATYPE_tmp
        SWATDATATYPE_rch
        SWATDATATYPE_sub
        SWATDATATYPE_hru
    End Enum

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SWAT Data Files"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::" & Description
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True 'yes, this class can open files
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False 'can save met data when called by code, but does not yet support saving to user-selected file
        End Get
    End Property

    Public Property DataType() As SWATDATATYPE
        Get
            Return pType
        End Get
        Set(ByVal value As SWATDATATYPE)
            If pType Is Nothing Or pType = "" Then
                pType = value
            End If
        End Set
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            Select Case IO.Path.GetExtension(Specification).ToLower
                Case ".pcp"
                    pType = SWATDATATYPE.SWATDATATYPE_pcp
                    Return OpenMet(aAttributes)
                Case ".tmp"
                    pType = SWATDATATYPE.SWATDATATYPE_tmp
                    Return OpenMet(aAttributes)
                Case ".hru"
                    pType = SWATDATATYPE.SWATDATATYPE_hru
                    Return OpenOutput(aAttributes)
                Case ".sub"
                    pType = SWATDATATYPE.SWATDATATYPE_sub
                    Return OpenOutput(aAttributes)
                Case ".rch"
                    pType = SWATDATATYPE.SWATDATATYPE_rch
                    Return OpenOutput(aAttributes)
                Case Else
                    Throw New ApplicationException("Unknown SWAT Data File Type: " & aFileName)
            End Select
        End If
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                       Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        'TODO: when called from client code, the pType is unknow, need a way to set it
        Select Case pType
            Case SWATDATATYPE.SWATDATATYPE_pcp
                WriteMetPcp(SaveFileName, ExistAction)
            Case SWATDATATYPE.SWATDATATYPE_tmp
                WriteMetTmp(SaveFileName, ExistAction)
            Case Else
                Throw New ApplicationException("Cannot save to SWAT Data File Type: " & IO.Path.GetExtension(Specification))
        End Select
    End Function

    Private Sub AddTsToList(ByVal aReadTS As atcTimeseries, _
                            ByVal aReadThese As Generic.List(Of atcTimeseries), _
                            ByVal aReadLocation As Generic.List(Of Integer), _
                            ByVal aUniqueLocations As Generic.List(Of String), _
                            ByVal aReadField As Generic.List(Of Integer), _
                            ByVal aReadValues As Generic.List(Of Double()))
        Dim lField As Integer = aReadTS.Attributes.GetValue("FieldIndex", 0)
        If lField < 1 Then
            Logger.Dbg("Dataset does not have a field index:" & aReadTS.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else
            aReadThese.Add(aReadTS)
            Dim lLocation As String = aReadTS.Attributes.GetValue("Location", "<unk>")
            Dim lLocationIndex As Integer = aUniqueLocations.IndexOf(lLocation)
            If lLocationIndex < 0 Then
                lLocationIndex = aUniqueLocations.Count
                aUniqueLocations.Add(lLocation)
            End If
            aReadLocation.Add(lLocationIndex)
            aReadField.Add(lField)
            Dim lVd(pNumValues) As Double 'array of double data values
            For lValueIndex As Integer = 0 To pNumValues
                lVd(lValueIndex) = pNaN
            Next
            aReadValues.Add(lVd)
        End If
    End Sub

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("Dataset not from this source:" & aReadMe.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else
            Select Case pType
                Case SWATDATATYPE.SWATDATATYPE_pcp, SWATDATATYPE.SWATDATATYPE_tmp
                    ReadDataMet(aReadMe)

                Case SWATDATATYPE.SWATDATATYPE_hru, _
                     SWATDATATYPE.SWATDATATYPE_rch, _
                     SWATDATATYPE.SWATDATATYPE_sub
                    ReadDataOutput(aReadMe)
            End Select
        End If
    End Sub

    Public Sub New()
        Filter = pFilter
    End Sub
End Class