Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility

Friend Class atcWdmHandle
    'Copyright 2005-7 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Implements IDisposable

    Dim pUnit As Integer 'fortran unit number of wdm file
    Dim pNeedToClose As Boolean = True
    Private Const pMaxFileNameLength As Integer = 64 'TODO: make this bigger in the fortran

    Public ReadOnly Property Unit() As Integer
        Get
            Return pUnit
        End Get
    End Property

    Public Sub New(ByVal aRWCFlg As Integer, ByVal aFileName As String)
        'aRWCFlg: 0- normal open of existing WDM file,
        '         1- open WDM file as read only,
        '         2- open new WDM file
        Dim lFileName As String
        Dim lAttr As FileAttribute
        Dim lRetcod As Integer

        pUnit = 0

        lFileName = AbsolutePath(aFileName, CurDir())
TestFileName:
        If lFileName.Length > pMaxFileNameLength Then
            Dim lShortFileName As String
            If FileExists(lFileName) Then
                lShortFileName = ConvertLongPathToShort(lFileName)
            Else
                lShortFileName = ConvertLongPathToShort(PathNameOnly(lFileName)) & g_PathChar & FilenameNoPath(lFileName)
            End If

            If lShortFileName.Length > pMaxFileNameLength Then
                If Logger.Msg("Cannot open WDM file with total path greater than 64 characters." & vbCrLf _
                             & aFileName & vbCrLf _
                             & "Browse for file in another folder?" & vbCrLf _
                             & "(Hint: you could copy the file to a folder with a shorter path.)", MsgBoxStyle.YesNo, "WDM Path Too Long") = MsgBoxResult.Yes Then
                    Dim lOpenDialog As New Windows.Forms.OpenFileDialog
                    With lOpenDialog
                        .Title = "Find '" & IO.Path.GetFileName(lFileName) & "' in another folder"
                        .FileName = aFileName
                        .Filter = "WDM files|*.wdm"
                        .FilterIndex = 0
                        If .ShowDialog = Windows.Forms.DialogResult.OK Then
                            lFileName = .FileName
                            GoTo TestFileName
                        End If
                    End With
                End If
                Throw New Exception("File Name Too Long. aFileName= '" & aFileName & "'" & vbCrLf & "lFileName= '" & lFileName & "'" & vbCrLf & _
                                    "Length " & lFileName.Length & " (and shortend length " & lShortFileName.Length & ") greater than Maximum of " & pMaxFileNameLength)
            Else
                lFileName = lShortFileName
            End If
        End If

        If Not FileExists(lFileName) AndAlso aRWCFlg <> 2 Then
            'Logger.Msg("Could not find " & aFileName, "atcWdmHandle")
        Else
            Try
                'Logger.Dbg("atcWdmHandle:New:VBOpen:" & lFileName)
                'Dim lWdmFile As New atcwdmfilevb.atcWDMfile
                'If lWdmFile.Open(lFileName) Then
                '    Logger.Dbg("atcWdmHandle:New:VBOpenDone")
                '    Dim lS As String = lWdmFile.ToString
                '    Logger.Dbg(lS)
                'Else
                '    Logger.Dbg("atcWdmHandle:New:VBOpen:False")
                'End If
            Catch ex As Exception
                Logger.Msg(ex.ToString)
            End Try
            If aRWCFlg = 0 Then
                lAttr = GetAttr(lFileName) 'if read only, change to not read only
                If (lAttr And FileAttribute.ReadOnly) <> 0 Then
                    lAttr = lAttr - FileAttribute.ReadOnly
                    SetAttr(lFileName, lAttr)
                End If
            End If

            pUnit = -2 'code to try to use existing unit number from INQUIRE_NAME
            'Logger.Dbg("atcWdmHandle:OpenB4:" & lFileName)
            Try
                pUnit = F90_INQNAM(lFileName, Len(lFileName))
                If pUnit = 0 Then
                    F90_WDBOPNR(aRWCFlg, lFileName, pUnit, lRetcod, CShort(Len(lFileName)))
                    pNeedToClose = True
                Else
                    'Logger.Dbg("atcWdmHandle:New:UsingOpenWdm:" & pUnit)
                    pNeedToClose = False
                End If
            Catch e As System.BadImageFormatException
                Logger.Msg("atcWdmHandle:Exception:Check HassEnt.dll Compile/Link Output" & e.ToString)
            End Try
            'Logger.Dbg("atcWdmHandle:OpenAft:Unit:Retcod:" & pUnit & ":" & lRetcod)

            If lRetcod <> 0 Then
                If lRetcod = 159 Then
                    Logger.Msg("WDM file " & lFileName & " is in use by another application, retcod 159", "atcWdmHandle")
                Else
                    Logger.Msg("WDM file " & lFileName & " open failed, retcod " & lRetcod, "atcWdmHandle")
                End If
                pUnit = 0
            End If
        End If
            Dim lMsg As String = "atcWdmHandle:New:" & pUnit & ":" & lRetcod & ":" & lFileName
            F90_MSG(lMsg, Len(lMsg))
    End Sub

    Public Sub Dispose() Implements System.IDisposable.Dispose
        Dim lRetcod As Integer

        'Logger.Dbg("atcWdmHandle:Dispose:" & pUnit)

        If pNeedToClose AndAlso pUnit > 0 Then
            lRetcod = F90_WDFLCL(pUnit)
            If lRetcod <> 0 Then
                'Logger.Dbg("atcWdmHandle:WDFLCL:retcod:" & lRetcod)
            End If
        Else
            'Logger.Dbg("atcWdmHandle:Dispose:DidNotNeedToClose")
        End If
        'Dim lMsg As String = "atcWdmHandle:Dispose:" & pUnit & ":" & lRetcod
        'F90_MSG(lMsg, Len(lMsg))
        pUnit = 0
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub
End Class
