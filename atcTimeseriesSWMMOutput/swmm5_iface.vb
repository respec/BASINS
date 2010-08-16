Imports System.IO
Imports MapWinUtility

Public Class SWMM5_OutputFile
    Dim pBinaryFileStream As FileStream
    Dim pBinaryReader As BinaryReader

    Public Const SUBCATCH = 0
    Public Const NODE = 1
    Public Const LINK = 2
    Public Const SYS = 3

    Private Const RECORDSIZE = 4           ' number of bytes per file record
    Private Const MAX_SUBCATCH_RESULTS = 7
    Private Const MAX_NODE_RESULTS = 7
    Private Const MAX_LINK_RESULTS = 6
    Private Const MAX_SYS_RESULTS = 14

    Private SubcatchVars As Integer        ' number of subcatch reporting variable
    Private SubcatchPropCodes() As Integer
    Private SubcatchPropValues(,) As Single

    Private NodeVars As Integer            ' number of node reporting variables
    Private NodePropCodes() As Integer
    Private NodePropValues(,) As Single

    Private LinkVars As Integer            ' number of link reporting variables
    Private LinkPropCodes() As Integer
    Private LinkPropValues(,) As Single

    Private SysVars As Integer             ' number of system reporting variables

    Public TimeStarts() As Double

    Public OffsetObjectIdNames As Integer
    Public OffsetObjectProperties As Integer
    Public OffsetComputedResults As Integer  ' file position where results start
    Private BytesPerPeriod As Integer         ' number of bytes used for storing results in file each reporting period
 
    Public Version As Integer
    Public SWMM_Nperiods As Integer        ' number of reporting periods
    Public SWMM_FlowUnits As Integer       ' flow units code
    Public SWMM_Nsubcatch As Integer       ' number of subcatchments
    Public SWMM_SubcatchId() As String
    Public SWMM_Nnodes As Integer          ' number of drainage system nodes
    Public SWMM_NodeId() As String
    Public SWMM_Nlinks As Integer          ' number of drainage system links
    Public SWMM_LinkId() As String
    Public SWMM_Npolluts As Integer        ' number of pollutants tracked
    Public SWMM_PollutId() As String
    Public SWMM_PollutConcCode() As Integer
    Public SWMM_StartDate As Double        ' start date of simulation
    Public SWMM_ReportStep As Integer      ' reporting time step (seconds)

    Function OpenSwmmOutFile(ByVal OutFile As String) As Integer
        '------------------------------------------------------------------------------
        '  Input:   outFile = name of binary output file
        '  Output:  returns 0 if successful, 1 if binary file invalid because
        '           SWMM 5 ran with errors, or 2 if the file cannot be opened
        '  Purpose: opens the binary output file created by a SWMM 5 run and
        '           retrieves the following simulation data that can be
        '           accessed by the application:
        '           SWMM_Nperiods = number of reporting periods
        '           SWMM_FlowUnits = flow units code
        '           SWMM_Nsubcatch = number of subcatchments
        '           SWMM_Nnodes = number of drainage system nodes
        '           SWMM_Nlinks = number of drainage system links
        '           SWMM_Npolluts = number of pollutants tracked
        '           SWMM_StartDate = start date of simulation
        '           SWMM_ReportStep = reporting time step (seconds)
        '------------------------------------------------------------------------------

        ' --- open the output file
        pBinaryFileStream = New FileStream(OutFile, FileMode.Open, FileAccess.Read)
        pBinaryReader = New BinaryReader(pBinaryFileStream)

    End Function

    Function GetSwmmResult(ByVal iType As Integer, ByVal iIndex As Integer, _
                           ByVal vIndex As Integer, ByVal period As Integer, _
                           ByRef Value As Single) As Integer
        '------------------------------------------------------------------------------
        '  Input:   iType = type of object whose value is being sought
        '                   (0 = subcatchment, 1 = node, 2 = link, 3 = system
        '           iIndex = index of item being sought (starting from 0)
        '           vIndex = index of variable being sought (see Interfacing Guide)
        '           period = reporting period index (starting from 1)
        '  Output:  value = value of variable being sought;
        '           function returns 1 if successful, 0 if not
        '  Purpose: finds the result of a specific variable for a given object
        '           at a specified time period.
        '------------------------------------------------------------------------------
        '// --- compute offset into output file
        Dim offset1 As Integer = OffsetComputedResults + ((period - 1) * BytesPerPeriod) + (2 * RECORDSIZE) ' + 1
        Dim offset2 As Integer = 0

        If iType = SUBCATCH Then
            offset2 = RECORDSIZE * (iIndex * SubcatchVars + vIndex)
        ElseIf iType = NODE Then
            offset2 = RECORDSIZE * (SWMM_Nsubcatch * SubcatchVars + _
                                    iIndex * NodeVars + vIndex)
        ElseIf iType = LINK Then
            offset2 = RECORDSIZE * (SWMM_Nsubcatch * SubcatchVars + _
                                    SWMM_Nnodes * NodeVars + _
                                    iIndex * LinkVars + vIndex)
        ElseIf iType = SYS Then
            offset2 = RECORDSIZE * (SWMM_Nsubcatch * SubcatchVars + _
                                    SWMM_Nnodes * NodeVars + _
                                    SWMM_Nlinks * LinkVars + vIndex)
        Else
            Return 0
        End If

        ' --- re-position the file and read result
        Dim offset As Integer = offset1 + offset2
        pBinaryFileStream.Seek(offset, SeekOrigin.Begin)
        Value = pBinaryReader.ReadSingle()
        Return 1
    End Function

    Public Sub CloseSwmmOutFile()
        '------------------------------------------------------------------------------
        '  Input:   none
        '  Output:  none
        '  Purpose: closes the binary output file.
        '------------------------------------------------------------------------------
        pBinaryReader.Close()
        pBinaryFileStream.Close()
    End Sub

    Public Sub New(ByVal OutFileName As String)
        OpenSwmmOutFile(OutFileName)
        ' --- check that file contains at least 14 records
        If pBinaryFileStream.Length < 14 * RECORDSIZE Then
            pBinaryFileStream.Close()
            Throw New ApplicationException("Not enough output")
        End If

        With pBinaryReader
            Try
                ' --- read parameters from end of file
                pBinaryFileStream.Seek(pBinaryFileStream.Length - (6 * RECORDSIZE), SeekOrigin.Begin)
                'pBinaryFileStream.Seek(-5 * RECORDSIZE, SeekOrigin.End)
                OffsetObjectIdNames = .ReadInt32
                OffsetObjectProperties = .ReadInt32 ' offset0
                OffsetComputedResults = .ReadInt32  ' StartPos
                SWMM_Nperiods = .ReadInt32
                ReDim TimeStarts(SWMM_Nperiods)
                Dim errCode As Integer = .ReadInt32
                If errCode <> 0 Then
                    Throw New ApplicationException("Error code in output file")
                End If

                Dim magic2 As Integer = .ReadInt32

                ' --- read magic number from beginning of file
                pBinaryFileStream.Seek(0, SeekOrigin.Begin)
                Dim magic1 As Integer = .ReadInt32

                ' --- perform error checks
                If magic1 <> magic2 Then
                    Throw New ApplicationException("Magic number mismatch")
                ElseIf SWMM_Nperiods = 0 Then
                    Throw New ApplicationException("Nperiods = 0")
                End If

                ' --- otherwise read additional parameters from start of file
                Version = .ReadInt32
                SWMM_FlowUnits = .ReadInt32
                SWMM_Nsubcatch = .ReadInt32
                SWMM_Nnodes = .ReadInt32
                SWMM_Nlinks = .ReadInt32
                SWMM_Npolluts = .ReadInt32

                ' --- save details about output locations
                Dim lLength As Integer
                If SWMM_Nsubcatch > 0 Then
                    ReDim SWMM_SubcatchId(SWMM_Nsubcatch - 1)
                    For lIndex As Integer = 0 To SWMM_Nsubcatch - 1
                        lLength = .ReadInt32
                        Dim lChars() As Char = New String(.ReadChars(lLength))
                        SWMM_SubcatchId(lIndex) = New String(lChars)
                    Next
                End If
                If SWMM_Nnodes > 0 Then
                    ReDim SWMM_NodeId(SWMM_Nnodes - 1)
                    For lIndex As Integer = 0 To SWMM_Nnodes - 1
                        lLength = .ReadInt32
                        SWMM_NodeId(lIndex) = New String(.ReadChars(lLength))
                    Next
                End If
                If SWMM_Nlinks > 0 Then
                    ReDim SWMM_LinkId(SWMM_Nlinks - 1)
                    For lIndex As Integer = 0 To SWMM_Nlinks - 1
                        lLength = .ReadInt32
                        SWMM_LinkId(lIndex) = New String(.ReadChars(lLength))
                    Next
                End If
                If SWMM_Npolluts > 0 Then
                    ReDim SWMM_PollutId(SWMM_Npolluts - 1)
                    For lIndex As Integer = 0 To SWMM_Npolluts - 1
                        lLength = .ReadInt32
                        SWMM_PollutId(lIndex) = New String(.ReadChars(lLength))
                    Next
                    '-- read codes of pollutant concentration units
                    ReDim SWMM_PollutConcCode(SWMM_Npolluts - 1)
                    For lIndex As Integer = 0 To SWMM_Npolluts - 1
                        lLength = .ReadInt32
                        SWMM_PollutConcCode(lIndex) = lLength
                    Next
                End If

                ' --- Skip over saved subcatch/node/link input values
                Dim loffset As Integer = (SWMM_Nsubcatch + 2) * RECORDSIZE + _
                                         (3 * SWMM_Nnodes + 4) * RECORDSIZE + _
                                         (5 * SWMM_Nlinks + 6) * RECORDSIZE
                loffset += OffsetObjectProperties
                pBinaryFileStream.Seek(loffset, SeekOrigin.Begin)

                ' --- read number & codes of computed variables
                SubcatchVars = .ReadInt32
                ReDim SubcatchPropCodes(SubcatchVars - 1)
                For lVarIndex As Integer = 0 To SubcatchVars - 1
                    SubcatchPropCodes(lVarIndex) = .ReadInt32
                Next

                NodeVars = .ReadInt32
                ReDim NodePropCodes(NodeVars - 1)
                For lVarIndex As Integer = 0 To NodeVars - 1
                    NodePropCodes(lVarIndex) = .ReadInt32
                Next

                LinkVars = .ReadInt32
                ReDim LinkPropCodes(LinkVars - 1)
                For lVarIndex As Integer = 0 To LinkVars - 1
                    LinkPropCodes(lVarIndex) = .ReadInt32
                Next

                SysVars = .ReadInt32

                ' --- read data just before start of output results
                pBinaryFileStream.Seek(OffsetComputedResults - 3 * RECORDSIZE, SeekOrigin.Begin)
                SWMM_StartDate = .ReadDouble
                TimeStarts(0) = SWMM_StartDate
                SWMM_ReportStep = .ReadInt32

                ' --- compute number of bytes stored per reporting period
                BytesPerPeriod = RECORDSIZE * 2 'datestamp
                BytesPerPeriod += RECORDSIZE * SWMM_Nsubcatch * SubcatchVars
                BytesPerPeriod += RECORDSIZE * SWMM_Nnodes * NodeVars
                BytesPerPeriod += RECORDSIZE * SWMM_Nlinks * LinkVars
                BytesPerPeriod += RECORDSIZE * SysVars

                'Get the timeseries date/time, shared among outputs
                For lTimeIndex As Integer = 1 To SWMM_Nperiods
                    pBinaryFileStream.Seek(OffsetComputedResults + ((lTimeIndex - 1) * BytesPerPeriod), SeekOrigin.Begin)
                    TimeStarts(lTimeIndex) = .ReadDouble
                Next
            Catch ex As Exception
                Logger.Dbg("SWMMOutputFileProblem " & ex.Message & vbCrLf & ex.StackTrace)
            Finally
                CloseSwmmOutFile()
            End Try
        End With
    End Sub
End Class

