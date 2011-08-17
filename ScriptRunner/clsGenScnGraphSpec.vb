Imports atcUtility
Imports MapWinUtility

Public Class clsGenScnGraphSpec
    Public Structure GSAxType
        Dim AType As Integer 'axis type 0TM,1AR,2LG,3-8PRB
        Dim SType As Integer 'sub axis type (time:0-undef,6-Yr,5-Mo,4-Dy,3-Hr)
        Dim STypeInt As Integer 'sub axis type Interval value
        Dim minv As Single 'min axis value
        Dim maxv As Single 'max axis value
        Dim NTic As Integer 'number of tics
        Dim TLen As Single 'tic length
        Dim label As String 'axis label
    End Structure
    Public Structure GSVarType
        Dim minv As Single
        Dim maxv As Single
        Dim WchAx As Integer
        Dim Trans As Integer
        Dim label As String
    End Structure
    Public Structure GSLineType
        Dim WchAx As Integer
        Dim LType As Integer
        Dim LThck As Integer
        Dim SType As Integer
        Dim Color As Integer
        Dim LegLbl As String
        Dim Value As String
    End Structure
    Public Structure GSCrvType
        'UPGRADE_NOTE: CType was upgraded to CType_Renamed.
        Dim CType_Renamed As Integer
        Dim LType As Integer
        Dim LThck As Integer
        Dim SType As Integer
        Dim Color As Integer
        'UPGRADE_WARNING: Fixed-length string size must fit in the buffer.
        Public LegLbl As String
    End Structure
    Private pSpecsLoaded As Boolean = True
    Public ReadOnly Property IsReady() As Boolean
        Get
            Return pSpecsLoaded
        End Get
    End Property
    Const POSMAX As Integer = 18
    Private pSpecification As String = ""
    Public Property Specification As String
        Get
            Return pSpecification
        End Get
        Set(value As String)
            pSpecification = value
            If pSpecification = "" Then
                pSpecsLoaded = False
            End If
        End Set
    End Property

    Private Const grayBasename As String = "gray" 'base name for shades of gray (gray0..gray255)
    Private Const grayNameNumStart As Short = 5 'len(grayBasename) + 1
    Private Const forceKnownColor As Boolean = True
    Private Lines() As GSLineType

    Public YLen, XLen, ALen As Single
    'UPGRADE_WARNING: Lower bound of array Axis was changed from 1 to 0.
    Public Axis(4) As GSAxType '1-leftY,2-rightY,3-Aux,4-X
    Public Gridx As Integer 'grid x:  1=yes
    Public Gridy As Integer
    Public rGridy As Integer
    Public Var(2 * POSMAX) As GSVarType
    Public Crv(POSMAX) As GSCrvType
    Public dtype(POSMAX) As Integer
    Public XLegLoc, YLegLoc As Integer
    Public XTxtLoc, YTxtLoc As Integer
    Public XtraText, Title As String
    Public DataLabelPosition As Integer '0=none, 1=horizontal, 2=vertical, 3=popup

    Private pLineCount As Integer
    Private pATCoRendDBF As atcTableDBF = Nothing
    Private pATCoRendFileName As String = "C:\Basins\models\HSPF\bin\ATCoRend.dbf"
    Private ReadOnly Property colorDB() As atcTableDBF
        Get
            If pATCoRendDBF Is Nothing Then
                pATCoRendDBF = New atcTableDBF
                If IO.File.Exists(pATCoRendFileName) Then
                    If pATCoRendDBF.OpenFile(pATCoRendFileName) Then
                        Return pATCoRendDBF
                    Else
                        Return Nothing
                    End If
                Else
                    'Try ask user to find it
                    Logger.Msg(pATCoRendFileName & vbCrLf & "is not found. Please locate the BASINS color dbase file.")
                    Dim lFileDialog As New Windows.Forms.OpenFileDialog()
                    lFileDialog.InitialDirectory = "c:\"
                    lFileDialog.Filter = "DBase files (*.dbf)|*.dbf|All files (*.*)|*.*"
                    lFileDialog.FilterIndex = 2
                    lFileDialog.RestoreDirectory = True
                    If lFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                        Try
                            If pATCoRendDBF.OpenFile(lFileDialog.FileName) Then Return pATCoRendDBF
                        Catch Ex As Exception
                            Logger.Msg("Cannot read file from disk. Original error: " & Ex.Message)
                            pATCoRendDBF = Nothing
                            Return Nothing
                        End Try
                    End If
                End If
            Else
                Return pATCoRendDBF
            End If
            Return Nothing
        End Get
    End Property

    Public Property NumLines() As Integer
        Get
            Return pLineCount
        End Get
        Set(ByVal Value As Integer)
            pLineCount = Value
            ReDim Preserve Lines(pLineCount)
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal aFileName)
        Specification = aFileName
        If Specification = "" Then
            pSpecsLoaded = False
        End If
    End Sub

    Public Sub RetrieveSpecs()

        Dim AplusYlen As Single
        Dim lAlen As Single
        Dim i As Integer
        Dim a As Integer
        Dim fu As Short
        Dim s As String ', rect As Rectangle
        Dim istr As String
        Dim rtyp As String
        Dim Utyp As String
        Dim lAlreadyAddedLine As Boolean

        Dim lFileName As String = Specification
        If Not IO.File.Exists(lFileName) Then
            Logger.Msg(lFileName & vbCrLf & " is not found. Please browse to correct graph spec file.")
            Dim lOpenFileDialog As New Windows.Forms.OpenFileDialog
            lOpenFileDialog.Filter = "Graph files (*.grf)|*.grf|All Files (*.*)|*.*"
            lOpenFileDialog.ShowDialog()
            If Len(lOpenFileDialog.FileName) > 0 Then
                lFileName = lOpenFileDialog.FileName
            End If
            If Not IO.File.Exists(lFileName) Then
                pSpecsLoaded = False
                Exit Sub
            End If
        Else
            Logger.Dbg("Specification file: " & lFileName)
        End If

        On Error GoTo ReadErr
        fu = FreeFile()
        FileOpen(fu, lFileName, OpenMode.Input)
        i = 0

        Dim qb As Integer
        Do While Not EOF(fu)
            istr = LineInput(fu)
            rtyp = UCase(StrRetRem(istr))
            Logger.Dbg("RetrieveSpecs:ReadRecord:" & rtyp & ":" & istr)

            Select Case rtyp
                Case ""
                Case "ALEN" : ALen = CSng(istr)
                Case "YLAB" : Axis(1).label = StrRetRem(istr)
                Case "YRLAB" : Axis(2).label = StrRetRem(istr)
                Case "ALAB" : Axis(3).label = StrRetRem(istr)
                Case "XLAB" : Axis(4).label = StrRetRem(istr)
                Case "YTYP" : Axis(1).AType = CInt(StrRetRem(istr))
                Case "YRTYP" : Axis(2).AType = CInt(StrRetRem(istr))
                Case "XTYP" : Axis(4).AType = CInt(StrRetRem(istr))
                Case "XGRD" : Gridx = CInt(StrRetRem(istr))
                Case "YGRD" : Gridy = CInt(StrRetRem(istr))
                Case "YRGRD" : rGridy = CInt(StrRetRem(istr))
                Case "SCALE" : i = CInt(StrRetRem(istr))
                    Axis(i).minv = CSng(StrRetRem(istr))
                    Axis(i).maxv = CSng(StrRetRem(istr))
                    Axis(i).NTic = CInt(StrRetRem(istr))
                Case "CURVE"
                    i = CInt(StrRetRem(istr))
                    With Crv(i)
                        .CType_Renamed = CInt(StrRetRem(istr))
                        .LType = CInt(StrRetRem(istr)) - 1
                        .LThck = CInt(StrRetRem(istr))
                        .SType = CInt(StrRetRem(istr))

                        .Color = -1
                        s = StrRetRem(istr)
                        If IsNumeric(s) Then
                            qb = CInt(s)
                            If qb > 0 And qb < 16 Then .Color = QBColor(CInt(s))
                        End If
                        If .Color = -1 Then .Color = TextOrNumericColor(s)

                        s = StrRetRem(istr)
                        If IsNumeric(s) Then
                            dtype(i) = CInt(s)
                        Else
                            istr = s & " " & istr
                        End If

                        .LegLbl = StrRetRem(istr)
                    End With
                Case "VAR"
                    i = CInt(StrRetRem(istr))
                    Var(i).WchAx = CInt(StrRetRem(istr))
                    'Var(i).Trans = CLng(StrRetRem(istr))
                    Var(i).label = StrRetRem(istr)
                Case "LINE"
                    If NumLines > 0 And Not lAlreadyAddedLine Then NumLines = 0
                    Logger.Msg("Adding line " & StrRetRem(istr))
                    AddLine(CInt(StrRetRem(istr)), CInt(StrRetRem(istr)), CInt(StrRetRem(istr)), CInt(StrRetRem(istr)), TextOrNumericColor(StrRetRem(istr)), StrSplit(istr, " ", "'"), ReplaceString(istr, "'", ""))
                    lAlreadyAddedLine = True
                Case "LOCLEGEND"
                    XLegLoc = CSng(StrRetRem(istr))
                    YLegLoc = CSng(StrRetRem(istr))
                Case "ADDTEXT"
                    XtraText = StrRetRem(istr)
                    XTxtLoc = CSng(StrRetRem(istr))
                    YTxtLoc = CSng(StrRetRem(istr))
                Case "TITLE"
                    Title = StrRetRem(istr)
                    'Text = StrRetRem(istr)
                Case "DATALABELS"
                    DataLabelPosition = CInt(StrRetRem(istr))
                Case Else
                    Logger.Msg("RetrieveSpecs:Unknown directive:" & rtyp & ":" & istr)
            End Select
        Loop
        Logger.Dbg("RetrieveSpecs:EndofMapFile")

        FileClose(fu)
        'ReDrawGraph(0)
        Exit Sub 'completed ok

ReadErr:
        Logger.Msg("A problem occurred reading the graph file " & lFileName & vbCrLf & Err.Description, 48, "Graph Problem")
        'Logger.Msg("RetrieveSpecs:Error:" & Err.Number & ":" & Err.Description & ":" & istr)
        FileClose(fu)
        pSpecsLoaded = False
    End Sub

    'value must be just a number along WchAx to align line with
    Public Sub AddLine(ByRef WchAx As Integer, ByRef LType As Integer, ByRef LThck As Integer, ByRef SType As Integer, ByRef Color As Integer, ByRef LegLbl As String, ByRef Value As String)
        NumLines = NumLines + 1
        With Lines(NumLines)
            If WchAx = 0 Then .WchAx = 4 Else .WchAx = WchAx
            .LType = LType - 1
            .LThck = LThck
            .SType = SType
            .Color = Color
            .LegLbl = LegLbl
            .Value = Value
        End With
    End Sub

    Public Function TextOrNumericColor(ByVal colr As String) As Integer
        Static AlreadyReportedError As Boolean = False
        Dim c As String
        c = LCase(Trim(colr))
        TextOrNumericColor = -1
        Dim r As String
        If IsNumeric(c) Then
            Return CInt(c)
        Else
            If Left(c, grayNameNumStart - 1) = LCase(grayBasename) Then
                r = Mid(colr, grayNameNumStart)
                If IsNumeric(r) Then Return RGB(CInt(r), CInt(r), CInt(r))
            End If
            If TextOrNumericColor = -1 Then
                If colorDB IsNot Nothing Then
                    If colorDB.FindFirst(1, c) Then
                        TextOrNumericColor = CInt(colorDB.Value(2))
                    Else
                        TextOrNumericColor = &H808080 'default to gray
                    End If
                Else
                    TextOrNumericColor = 1
                End If
            End If
        End If
        Exit Function
    End Function

    Public Sub Clear()
        Specification = ""
        pSpecsLoaded = False
        If pATCoRendDBF IsNot Nothing Then
            pATCoRendDBF.Clear()
            pATCoRendDBF = Nothing
        End If
        ReDim Axis(0)
        ReDim Crv(0)
        ReDim Var(0)
    End Sub

    Function StrSplit(ByRef Source As String, ByRef delim As String, ByRef quote As String) As String
        ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
        'delimeter. Quote specifies a particular string that is exempt from the delimeter search.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "Julie, Todd") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
        ' ##PARAM Source M String to be analyzed
        ' ##PARAM delim I Single-character string delimeter
        ' ##PARAM quote I Multi-character string exempted from search.
        ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
        'Returns input parameter without that portion. If no delimiter in string, _
        'returns whole string, and input parameter reduced to null string.
        Dim retval As String
        Dim i As Integer
        Dim quoted As Boolean
        Dim trimlen As Integer
        Dim quotlen As Integer
        ' ##LOCAL retval - string to return as StrSplit
        ' ##LOCAL i - long character position of search through Source
        ' ##LOCAL quoted - Boolean whether quote was encountered in Source
        ' ##LOCAL trimlen - long length of delimeter, or quote if encountered first
        ' ##LOCAL quotlen - long length of quote

        Source = LTrim(Source) 'remove leading blanks
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
            Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
            If quoted And Len(Source) > 0 Then
                If Left(Source, Len(delim)) = delim Then Source = Mid(Source, Len(delim) + 1)
            End If
        Else 'take it all
            retval = Source
            Source = "" 'nothing left
        End If

        StrSplit = retval

    End Function
End Class
