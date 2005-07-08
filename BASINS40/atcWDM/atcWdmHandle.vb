Option Strict Off
Option Explicit On 

Imports atcUtility

Friend Class atcWdmHandle
  'Copyright 2005 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
  Implements IDisposable

  Dim pUnit As Integer 'fortran unit number of wdm file
  Dim pWasOpen As Boolean

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

    lFileName = AbsolutePath(aFileName, CurDir())

    If Not FileExists(lFileName) Then
      LogMsg("Could not find " & aFileName, "atcWdmHandle")
    Else
      If aRWCFlg = 0 Then
        lAttr = GetAttr(lFileName) 'if read only, change to not read only
        If (lAttr And FileAttribute.ReadOnly) <> 0 Then
          lAttr = lAttr - FileAttribute.ReadOnly
          SetAttr(lFileName, lAttr)
        End If
      End If

      pUnit = F90_INQNAM(lFileName, CShort(Len(lFileName)))
      'lWdmOpen = F90_WDMOPN(pFileUnit, FileName, Len(FileName))
      If pUnit = 0 Then
        pWasOpen = False
        F90_WDBOPNR(aRWCFlg, aFileName, pUnit, lRetcod, CShort(Len(lFileName)))
        'pUnit = F90_WDBOPN(aRWCFlg, aFileName, Len(aFileName))
      Else
        pWasOpen = True
      End If

      If lRetcod <> 0 Then
        If lRetcod = 159 Then
          LogMsg("WDM file " & aFileName & " is in use by another application, retcod 159", "atcWdmHandle")
        Else
          LogMsg("WDM file " & aFileName & " open failed, retcod " & lRetcod, "atcWdmHandle")
        End If
        pUnit = 0
      End If
    End If
  End Sub

  Public Sub Dispose() Implements System.IDisposable.Dispose
    Dim lRetcod As Integer

    If pUnit > 0 And Not pWasOpen Then
      lRetcod = F90_WDFLCL(pUnit)
      If lRetcod = 0 Then pUnit = 0
    End If
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overrides Sub Finalize()
    Dispose()
  End Sub
End Class
