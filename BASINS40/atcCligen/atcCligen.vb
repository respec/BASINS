Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Reflection

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcCligen
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private Shared pFileFilter As String = "Cligen Parameter Files (*.par)|*.par"
    Private pErrorDescription As String
    Private pSpecification As String
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Cligen Weather Generator"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Cligen"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "Meteorologic Generation"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True 'yes, this class can open files
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return True 'yes, can save after editing values
        End Get
    End Property

    Public Overrides Property Specification() As String
        Get
            Return pSpecification
        End Get
        Set(ByVal newValue As String)
            pSpecification = newValue
        End Set
    End Property

    Public Overrides Function Open(ByVal aOperationName As String, _
                          Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean

        Dim lOk As Boolean
        Dim lAttDef As atcAttributeDefinition
        Dim lAttDef2 As atcAttributeDefinition
        Dim lParmFile As String = ""
        Dim lOutFile As String = ""
        Dim lSYr As Integer
        Dim lNYrs As Integer
        Dim lIncDaily As Boolean
        Dim lIncHourly As Boolean

        If aArgs Is Nothing Then
            Dim lForm As New frmCliGen
            lOk = lForm.AskUser(lParmFile, lOutFile, lSYr, lNYrs, lIncDaily, lIncHourly)
        Else
            lParmFile = aArgs.GetValue("CliGen Parm")
            lOutFile = aArgs.GetValue("CliGen Out")
            lSYr = aArgs.GetValue("Start Year")
            lNYrs = aArgs.GetValue("Num Years")
            lIncDaily = aArgs.GetValue("Include Daily")
            lIncHourly = aArgs.GetValue("Include Hourly")
            lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Start Year")
        lAttDef2 = atcDataAttributes.GetDefinition("Num Years")
        If lOk And lSYr >= lAttDef.Min And lSYr <= lAttDef.Max And _
           lNYrs >= lAttDef2.Min And lNYrs <= lAttDef2.Max Then 'run CliGen
            If RunCliGen(lParmFile, lOutFile, lSYr, lNYrs) Then 'successful run
                Dim lCliGenOut As New atcDataSourceCligen
                Dim lArgs As New atcDataAttributes
                lArgs.Clear()
                lArgs.Add("Include Daily", lIncDaily)
                lArgs.Add("Include Hourly", lIncHourly)
                If lCliGenOut.Open(lOutFile, lArgs) Then 'add output datasets to this class' datasets
                    Me.DataSets.AddRange(lCliGenOut.DataSets)
                End If
            Else
                Logger.Msg("CliGen run not successful using following inputs:" & vbCrLf & _
                           "  Parameter file:  " & lParmFile & vbCrLf & _
                           "  Output file:     " & lOutFile & vbCrLf & _
                           "  Starting Year:   " & lSYr & vbCrLf & _
                           "  Number of Years: " & lNYrs, "Run CliGen Problem")
            End If
        End If
        Return (Me.DataSets.Count > 0)

    End Function

    Public Overrides ReadOnly Property AvailableOperations() As atcData.atcDataAttributes
        Get
            Dim lOperations As atcDataAttributes
            If Not pAvailableOperations Is Nothing Then
                lOperations = pAvailableOperations
            Else
                lOperations = New atcDataAttributes
                Dim lArguments As atcDataAttributes

                Dim defParmFile As New atcAttributeDefinition
                With defParmFile
                    .Name = "CliGen Parm"
                    .Description = "CliGen Parameter File of Monthly Averages"
                    .Editable = True
                    .TypeString = "String"
                End With

                Dim defOutFile As New atcAttributeDefinition
                With defOutFile
                    .Name = "CliGen Out"
                    .Description = "CliGen Output File Generated Timeseries"
                    .Editable = True
                    .TypeString = "String"
                End With

                Dim defStartYear As New atcAttributeDefinition
                With defStartYear
                    .Name = "Start Year"
                    .Description = "Starting Year of CliGen Run"
                    .Min = 0
                    .Max = 9999
                    .DefaultValue = 2000
                    .Editable = True
                    .TypeString = "Integer"
                End With

                Dim defNumYears As New atcAttributeDefinition
                With defNumYears
                    .Name = "Num Years"
                    .Description = "Number of Years for CliGen Run"
                    .Min = 1
                    .Max = 9999
                    .DefaultValue = 1
                    .Editable = True
                    .TypeString = "Integer"
                End With

                Dim defIncludeDaily As New atcAttributeDefinition
                With defIncludeDaily
                    .Name = "Include Daily"
                    .Description = "Include Daily data as output timeseries"
                    .DefaultValue = False
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                Dim defIncludeHourly As New atcAttributeDefinition
                With defIncludeHourly
                    .Name = "Include Hourly"
                    .Description = "Include Hourly data as output timeseries"
                    .DefaultValue = False
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                Dim lCliGen As New atcAttributeDefinition
                With lCliGen
                    .Name = "CliGen"
                    '.Category = "Meteorologic Generation"
                    .Description = "Run CliGen to Generate Meteorologic Timeseries"
                    .Editable = False
                    .TypeString = "atcTimeseries"
                    .Calculator = Me
                End With

                lArguments = New atcDataAttributes
                lArguments.SetValue(defParmFile, Nothing)
                lArguments.SetValue(defOutFile, Nothing)
                lArguments.SetValue(defStartYear, Nothing)
                lArguments.SetValue(defNumYears, Nothing)
                lArguments.SetValue(defIncludeDaily, Nothing)
                lArguments.SetValue(defIncludeHourly, Nothing)

                lOperations.SetValue(lCliGen, Nothing, lArguments)
            End If

            Return lOperations
        End Get
    End Property

    Private Function RunCliGen(ByVal aParmFileName As String, ByVal aOutFileName As String, ByVal aStartYear As Integer, ByVal aNumYears As Integer) As Boolean
        Dim lStr As String
        Dim lExeName As String = FindFile("Please Locate CliGen Executable", "CliGen522564.exe", "*.exe", "Executable Files (*.exe)|*.exe")

        If FileExists(aOutFileName) Then 'delete pre-existing output file
            Kill(aOutFileName)
        End If
        lStr = """" & lExeName & """" & " -b" & aStartYear & " -y" & aNumYears & " -i" & aParmFileName & _
               " -o" & aOutFileName & " -F -t5 >" & FilenameNoExt(aParmFileName) & ".out"
        Shell(lStr, AppWinStyle.Hide, True)
        If FileExists(aOutFileName) Then
            Return True
        Else
            Return False
        End If

    End Function

#If GISProvider = "DotSpatial" Then
    Public Sub Initialize()
        Dim lAvlOps As atcDataAttributes = AvailableOperations()
    End Sub
#Else
    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        Dim lAvlOps As atcDataAttributes = AvailableOperations()
        MyBase.Initialize(aMapWin, aParentHandle)
    End Sub
#End If

End Class