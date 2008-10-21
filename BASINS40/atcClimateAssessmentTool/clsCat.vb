Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Windows.Forms

Public Class clsCat
    Friend Inputs As New Generic.List(Of atcVariation)
    Friend Endpoints As New Generic.List(Of atcVariation)
    Friend PreparedInputs As atcCollection
    Friend SaveAll As Boolean = False
    Friend ShowEachRunProgress As Boolean = False
    Friend BaseScenario As String = ""

    Friend CLIGEN_NAME As String = "Cligen"
    Friend StartFolderVariable As String = "{StartFolder}"

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lVariation As atcVariation

            lXML &= "<SaveAll>" & SaveAll & "</SaveAll>" & vbCrLf

            lXML &= "<ShowEachRun>" & ShowEachRunProgress & "</ShowEachRun>" & vbCrLf

            lXML &= "<UCI>" & vbCrLf
            lXML &= "  <FileName>" & BaseScenario & "</FileName>" & vbCrLf
            lXML &= "</UCI>" & vbCrLf

            If PreparedInputs Is Nothing Then
                lXML &= "<Variations>" & vbCrLf
                For Each lVariation In Inputs
                    lXML &= lVariation.XML
                Next
                lXML &= "</Variations>" & vbCrLf
            Else
                lXML &= "<PreparedInputs>"
                For Each lPreparedInput As String In PreparedInputs
                    lXML &= "<PreparedInput>" & lPreparedInput & "</PreparedInput>" & vbCrLf
                Next
                'For Each lPreparedInput As String In PreparedInputs
                '    lXML &= "<PreparedInput selected=""" & lstInputs.CheckedItems.Contains(lPreparedInput)
                '    lXML &= """>" & lPreparedInput & "</PreparedInput>" & vbCrLf
                'Next
                lXML &= "</PreparedInputs>"
            End If

            lXML &= "<Endpoints>" & vbCrLf
            For Each lVariation In Endpoints
                lXML &= lVariation.XML
            Next
            lXML &= "</Endpoints>" & vbCrLf

            Dim lStartFolder As String = CurDir()
            lXML = ReplaceStringNoCase(lXML, lStartFolder, StartFolderVariable)
            If lXML.Contains(StartFolderVariable) Then
                lXML = "<StartFolder>" & lStartFolder & "</StartFolder>" & vbCrLf & lXML
            End If

            lXML = "<BasinsCAT>" & vbCrLf & lXML & "</BasinsCAT>" & vbCrLf
            Return lXML
        End Get

        Set(ByVal newValue As String)
            Try
                Dim lXMLdoc As New Xml.XmlDocument
StartOver:
                lXMLdoc.LoadXml(newValue)
                Dim lNode As Xml.XmlNode = lXMLdoc.FirstChild
                If lNode.Name.ToLower.Equals("basinscat") Then
                    For Each lXML As Xml.XmlNode In lNode.ChildNodes
                        Dim lVariation As atcVariation
                        Dim lChild As Xml.XmlNode = lXML.FirstChild
                        Select Case lXML.Name.ToLower
                            Case "startfolder" 'Replace start folder in all XML if present
                                Dim lStartFolder As String = lXML.InnerText
                                newValue = ReplaceString(newValue, lXML.OuterXml, "")
                                newValue = ReplaceString(newValue, StartFolderVariable, lStartFolder)
                                GoTo StartOver
                            Case "saveall"
                                SaveAll = (lXML.InnerText.ToLower = "true")
                            Case "showeachrun"
                                ShowEachRunProgress = (lXML.InnerText.ToLower = "true")
                            Case "uci"
                                OpenUCI(AbsolutePath(lChild.InnerText, CurDir))
                            Case "preparedinputs"
                                If PreparedInputs Is Nothing Then
                                    PreparedInputs = New atcCollection
                                Else
                                    PreparedInputs.Clear()
                                End If
                                For Each lChild In lXML.ChildNodes
                                    PreparedInputs.Add(lChild.InnerText)
                                Next
                            Case "variations"
                                Inputs.Clear()
                                For Each lChild In lXML.ChildNodes
                                    If lChild.InnerXml.IndexOf(CLIGEN_NAME) >= 0 Then
                                        lVariation = New VariationCligen
                                    Else
                                        lVariation = New atcVariation
                                    End If
                                    lVariation.XML = lChild.OuterXml
                                    If Not lVariation.IsInput Then
                                        lVariation.IsInput = True
                                        Logger.Dbg("Assigned IsInput to loaded variation '" & lVariation.Name & "'")
                                    End If
                                    Inputs.Add(lVariation)
                                Next
                            Case "endpoints"
                                Endpoints.Clear()
                                For Each lChild In lXML.ChildNodes
                                    lVariation = New atcVariation
                                    lVariation.XML = lChild.OuterXml
                                    'Used to keep input variations in endpoints, skip them
                                    If Not lVariation.IsInput Then
                                        Endpoints.Add(lVariation)
                                    End If
                                Next
                        End Select
                    Next
                End If
            Catch e As Exception
                Logger.Msg("Could not load XML:" & vbCrLf & e.Message & vbCrLf & vbCrLf & newValue, "CAT XML Problem")
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Open data files referred to in this UCI file
    ''' </summary>
    ''' <param name="aUCIfilename">Full path of UCI file</param>
    ''' <remarks></remarks>
    Friend Sub OpenUCI(Optional ByVal aUCIfilename As String = "")
        If Not aUCIfilename Is Nothing And Not FileExists(aUCIfilename) Then
            If FileExists(aUCIfilename & ".uci") Then aUCIfilename &= ".uci"
        End If

        If aUCIfilename Is Nothing OrElse Not FileExists(aUCIfilename) Then
            Dim cdlg As New OpenFileDialog
            cdlg.Title = "Open UCI file containing base scenario"
            cdlg.Filter = "UCI files|*.uci|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aUCIfilename = cdlg.FileName
            End If
        End If

        If FileExists(aUCIfilename) Then
            BaseScenario = aUCIfilename
            Dim lUciFolder As String = PathNameOnly(aUCIfilename)
            ChDriveDir(lUciFolder)

            Dim lFullText As String = WholeFileString(aUCIfilename)
            For Each lWDMfilename As String In UCIFilesBlockFilenames(lFullText, "WDM")
                lWDMfilename = AbsolutePath(lWDMfilename, lUciFolder)
                OpenDataSource(lWDMfilename)
            Next
            For Each lBinOutFilename As String In UCIFilesBlockFilenames(lFullText, "BINO")
                lBinOutFilename = AbsolutePath(lBinOutFilename, lUciFolder)
                OpenDataSource(lBinOutFilename)
            Next
        End If
    End Sub

    Friend Function OpenDataSource(ByVal aFilename As String) As atcDataSource
        Dim lAddSource As Boolean = True
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            If lDataSource.Specification.ToLower = aFilename.ToLower Then 'already open
                Return lDataSource
            End If
        Next
        If lAddSource AndAlso FileExists(aFilename) Then
            Dim lDataSource As atcDataSource
            If aFilename.ToLower.EndsWith("wdm") Then
                lDataSource = New atcWDM.atcDataSourceWDM
            ElseIf aFilename.ToLower.EndsWith("hbn") Then
                lDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Else
                Throw New ApplicationException("Could not open '" & aFilename & "' in frmCAT:OpenDataSource")
            End If
            lDataSource.Specification = aFilename
            atcDataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing)
            Return lDataSource
        End If
        Return Nothing
    End Function
End Class
