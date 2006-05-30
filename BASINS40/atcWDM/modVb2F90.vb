Option Strict On
Option Explicit On

Module Vb2F90
    'Copyright 2001-6 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    'general routines
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)
    Declare Function F90_INQNAM Lib "hass_ent.dll" _
        (ByVal aName As String, ByVal aNameLen As Short) As Integer
    Declare Sub F90_WDBOPNR Lib "hass_ent.dll" _
        (ByRef aRwflg As Integer, _
         ByVal aName As String, _
         ByRef aUnit As Integer, ByRef aRetcod As Integer, ByVal aNameLen As Short)

    'wdm:wdatrb
    Declare Sub F90_WDBSAC Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, ByRef aMsgUnit As Integer, _
         ByRef aSaind As Integer, ByRef aSalen As Integer, ByRef aRetcod As Integer, _
         ByVal aVal As String, ByVal aValLen As Short)
    Declare Sub F90_WDBSAI Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, ByRef aMsgUnit As Integer, _
         ByRef aSaind As Integer, ByRef aSalen As Integer, ByRef aRetcod As Integer, _
         ByRef aVal As Integer)
    Declare Sub F90_WDBSAR Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, ByRef aMsgUnit As Integer, _
         ByRef aSaind As Integer, ByRef aSalen As Integer, ByRef aRetcod As Integer, _
         ByRef aVal As Single)
    Declare Sub F90_WDSAGY_XX Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aSaind As Integer, _
         ByRef aLen As Integer, ByRef aType As Integer, _
         ByRef aMin As Single, ByRef aMax As Single, ByRef aDef As Single, _
         ByRef aHLen As Integer, ByRef aHRec As Integer, ByRef aHPos As Integer, _
         ByRef aVLen As Integer, ByRef aIName As Integer, ByRef aIDesc As Integer, ByRef aIValid As Integer)

    'adwdm:wdmess
    Declare Sub F90_WDLBAX Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aDstype As Integer, ByRef aNDn As Integer, ByRef aNUp As Integer, _
         ByRef aNSa As Integer, ByRef aNSasp As Integer, ByRef aNDp As Integer, _
         ByRef aPsa As Integer)
    Declare Sub F90_WDDSDL Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aRetcod As Integer)
    Declare Sub F90_GETATT Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aInit As Integer, _
         ByRef aSaInd As Integer, ByRef aSaVal As Integer)

    'wdm:wdbtch
    Declare Sub F90_WDBSGI Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aSaInd As Integer, ByRef aSaLen As Integer, _
         ByRef aSaVal As Integer, ByRef aRetcod As Integer)
    Declare Sub F90_WDBSGR Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aSaInd As Integer, ByRef aSaLen As Integer, _
         ByRef aSaVal As Single, ByRef aRetcod As Integer)
    Declare Sub F90_WDDSRN Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsnOld As Integer, _
         ByRef aDsnNew As Integer, ByRef aRetcod As Integer)
    Declare Sub F90_WDBSGC_XX Lib "hass_ent.dll" _
        (ByRef l As Integer, ByRef l As Integer, _
         ByRef aSaInd As Integer, ByRef aSaLen As Integer, _
         ByRef aISaVal As Integer)
    Declare Sub F90_WDLBAD Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aDsType As Integer, ByRef aPsa As Integer)

    'adwdm:utwdmd
    Declare Function F90_WDFLCL Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer) As Integer
    Declare Sub F90_WDDSNX Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer)
    Declare Function F90_WDCKDT Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer) As Integer

    'wdm:wdtms1
    Declare Sub F90_WDTGET Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aDelt As Integer, ByRef aDates As Integer, ByRef aNval As Integer, _
         ByRef aDtran As Integer, ByRef aQualfg As Integer, ByRef aTunits As Integer, _
         ByRef aRVal As Single, ByRef aRetcod As Integer)
    Declare Sub F90_WDTPUT Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aDelt As Integer, ByRef aDates As Integer, ByRef aNval As Integer, _
         ByRef aDtran As Integer, ByRef aQualfg As Integer, ByRef aTunits As Integer, _
         ByRef aRVal As Single, ByRef aRetcod As Integer)

    'wdm:wdtms2
    Declare Sub F90_WTFNDT Lib "hass_ent.dll" _
        (ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
         ByRef aGpflg As Integer, ByRef aTdsfrc As Integer, _
         ByRef aSDate As Integer, ByRef aEDate As Integer, _
         ByRef aRetcod As Integer)

    Public Sub F90_WDBSGC(ByRef aWdmUnit As Integer, ByRef aDsn As Integer, _
                          ByRef aSaInd As Integer, ByRef aSaLen As Integer, _
                          ByRef aSaVal As String)
        Dim lVal(48) As Integer

        F90_WDBSGC_XX(aWdmUnit, aWdmUnit, aSaInd, aSaLen, lVal(0))
        NumChr(aSaLen, lVal, aSaVal)
    End Sub

    Private Sub NumChr(ByRef aLen As Integer, ByRef aIntStr() As Integer, ByRef aStr As String)
        aStr = ""
        For lInd As Integer = 0 To aLen - 1 'added "- 1" 8/16/2002 Mark Gray
            If aIntStr(lInd) > 0 Then
                aStr &= Chr(aIntStr(lInd))
            End If
        Next lInd
        aStr = RTrim(aStr)
    End Sub

    Public Sub F90_WDSAGY(ByRef aWdmUnit As Integer, ByRef aSaind As Integer, _
                   ByRef aLen As Integer, ByRef aType As Integer, _
                   ByRef aMin As Single, ByRef aMax As Single, ByRef aDef As Single, _
                   ByRef aHLen As Integer, ByRef aHRec As Integer, ByRef aHPos As Integer, _
                   ByRef aVLen As Integer, ByRef aName As String, ByRef aDesc As String, ByRef aValid As String)
        Dim lName(6) As Integer
        Dim lDesc(47) As Integer
        Dim lValid(240) As Integer
        F90_WDSAGY_XX(aWdmUnit, aSaind, _
                      aLen, aType, _
                      aMin, aMax, aDef, _
                      aHLen, aHRec, aHPos, _
                      aVLen, lName(0), lDesc(0), lValid(0))
        NumChr(6, lName, aName)
        NumChr(47, lDesc, aDesc)
        NumChr(aVLen, lValid, aValid)
    End Sub
End Module