Option Strict Off
Option Explicit On 

Imports atcUtility
Imports MapWinUtility

Friend Class atcWdmHandle
  'Copyright 2005 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
  Implements IDisposable

  Dim pUnit As Integer 'fortran unit number of wdm file

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

    If Not FileExists(lFileName) AndAlso aRWCFlg <> 2 Then
      Logger.Msg("Could not find " & aFileName, "atcWdmHandle")
    Else
      Logger.Dbg("atcWdmHandle:New:Open:" & lFileName)
      If aRWCFlg = 0 Then
        lAttr = GetAttr(lFileName) 'if read only, change to not read only
        If (lAttr And FileAttribute.ReadOnly) <> 0 Then
          lAttr = lAttr - FileAttribute.ReadOnly
          SetAttr(lFileName, lAttr)
        End If
      End If

      pUnit = -2 'code to try to use existing unit number from INQUIRE_NAME
      F90_WDBOPNR(aRWCFlg, lFileName, pUnit, lRetcod, CShort(Len(lFileName)))

      Logger.Dbg("atcWdmHandle:New:Open:" & lFileName & ":" & pUnit)

      If lRetcod <> 0 Then
        If lRetcod = 159 Then
          Logger.Msg("WDM file " & lFileName & " is in use by another application, retcod 159", "atcWdmHandle")
        Else
          Logger.Msg("WDM file " & lFileName & " open failed, retcod " & lRetcod, "atcWdmHandle")
        End If
        pUnit = 0
      End If
    End If
  End Sub

  Public Sub Dispose() Implements System.IDisposable.Dispose
    Dim lRetcod As Integer

    Logger.Dbg("atcWdmHandle:Dispose:" & pUnit)

    If pUnit > 0 Then
      lRetcod = F90_WDFLCL(pUnit)
      If lRetcod = 0 Then
        pUnit = 0
      ElseIf lRetcod = -255 Then
        Logger.Dbg("atcWdmHandle:WDFLCL:retcod:" & lRetcod)
      Else
        Logger.Msg("retcod:" & lRetcod, "atcWdmHandle:WDFLCL")
      End If
    End If
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overrides Sub Finalize()
    Dispose()
  End Sub
End Class
