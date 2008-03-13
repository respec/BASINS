Imports MapWinUtility
Imports atcUtility
Imports MapWindow.Interfaces
Imports mwOpenMetadataManager
Imports System
Imports System.Xml

Module MetaDataBatch
    Private pTestPath As String = "C:\dev\BASINS40\ScriptRunner\data\metadata\Orig"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)   'change to the directory of the testdata
        Logger.Dbg(" CurDir:" & Io.Directory.GetCurrentDirectory)

        Dim lMetaData As FGDCMetadata.MetadataMain
        Dim lSettings As New XmlReaderSettings
        lSettings.ProhibitDtd = False
        lSettings.ValidationType = ValidationType.DTD
        AddHandler lSettings.ValidationEventHandler, AddressOf ValidationEventHandler

        Dim lFileNames() As String = {"nhdplus.xml", "cnty.shp.xml", "nhdflowline.shp.xml", "nhdflowline.shp.Orig.xml"}
        For Each lFileName As String In lFileNames
            Logger.Dbg("----Process " & lFileName)
            Dim lStream As New System.IO.FileStream(lFileName, IO.FileMode.Open)
            Dim lXMLReader As XmlReader = XmlReader.Create(lStream, lSettings)
            Dim lXML As New XmlDocument
            lXML.Load(lXMLReader)
            lStream.Close()
            lStream = Nothing
            lXMLReader.Close()
            lXMLReader = Nothing
            lXML = Nothing

            lMetaData = New FGDCMetadata.MetadataMain
            Try
                lMetaData.Load(lFileName)
            Catch lEx As Exception
                Logger.Dbg(lEx.ToString)
            End Try
            If lMetaData.Errors.Count > 0 Then
                Logger.Dbg("-- " & lMetaData.Errors.Count & " Errors Found, Details:")
                For Each lError As String In lMetaData.Errors
                    Logger.Dbg(lError)
                Next
            End If
            lFileName = "..\rev\" & lFileName
            lMetaData.Save(lFileName)
            Logger.Dbg("---Saved " & lFileName)

            lStream = New System.IO.FileStream(lFileName, IO.FileMode.Open)
            lXMLReader = XmlReader.Create(lStream, lSettings)
            lXML = New XmlDocument
            lXML.Load(lXMLReader)
            lStream.Close()
            lStream = Nothing
            lXMLReader.Close()
            lXMLReader = Nothing
            lXML = Nothing
        Next
        Logger.Dbg("Done")
    End Sub

    Private Sub ValidationEventHandler(ByVal sender As Object, ByVal args As Schema.ValidationEventArgs)
        Dim lMsg As String = ""
        If args.Severity = Schema.XmlSeverityType.Warning Then
            lMsg &= "WARNING: "
        ElseIf args.Severity = Schema.XmlSeverityType.Error Then
            lMsg &= "ERROR: "
        End If
        Logger.Dbg(lMsg & args.Message)
    End Sub
End Module
