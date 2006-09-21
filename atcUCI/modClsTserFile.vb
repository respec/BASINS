Option Strict Off
Option Explicit On
Module modClsTserFile
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
    'Private Const TserFileClassName As String = "ATCTSfile"
    Public TserFiles As New atcData.atcDataManager(Nothing)
	
    Public Sub InitAtCoTser()
        'Probably we don't need this at all any more
        '        Dim regKey As String(,)
        '        Dim iKey As Integer 'index of registry key being examined

        '        TserFiles = Nothing
        '		On Error GoTo TSerPlugInsError
        '        TserFiles = New atcData.atcDataManager(Nothing)
        '		On Error GoTo 0
        '        regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
        '        If IsNothing(regKey) Then
        '            SaveSetting("ATCoPlugin", TserFileClassName, TserFileClassName, TserFileClassName & ".dll")
        '            regKey = GetAllSettings("ATCoPlugin", TserFileClassName)
        '        End If
        '        For iKey = LBound(regKey, 1) To UBound(regKey, 1)
        '            Dim lWDM As New atcWDM.atcDataSourceWDM
        '            If Not TserFiles.OpenDataSource(lWDM, CStr(regKey(iKey, 0)), Nothing) Then
        '                MsgBox("Could not load Timeseries " & regKey(iKey, 0) & " from " & regKey(iKey, 1) & vbCr & TserFiles.ErrorDescription)
        '            End If
        '        Next iKey

        '		Exit Sub

        'TSerPlugInsError: 
        '		MsgBox("Could not load Timeseries from " & TserFileClassName & vbCr & TserFiles.ErrorDescription)
    End Sub
End Module