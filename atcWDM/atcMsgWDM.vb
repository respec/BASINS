Option Strict On
Option Explicit On

Imports atcUtility
Imports MapWinUtility

''' <summary>
''' WDM attributes in memory, read from hspfmsg.wdm
''' </summary>
''' <remarks>
'''Copyright 2005 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
'''</remarks>
Friend Class atcMsgWDM
    Dim pAttributes As atcCollection 'of clsAttributeDefinition

    Public ReadOnly Property Attributes() As atcCollection
        Get
            Attributes = pAttributes
        End Get
    End Property

    Public Sub New()
        Dim lIndex As Integer 'index
        Dim lLen As Integer 'max dimension
        Dim lType As Integer 'type (1-Integer, 2-Real, 3-String, 0-Dummy)
        Dim lRMax As Single 'max allowed value
        Dim lRMin As Single 'min allowed value
        Dim lRDef As Single 'default value
        Dim lHlpPos As Integer 'help position within message file record
        Dim lHlpLen As Integer 'help length
        Dim lHlpRec As Integer 'help record withing message file
        Dim lValidLen As Integer 'length of valid values
        Dim lDesc As String = "" 'description 
        Dim lName As String = "" 'name
        Dim lValid As String = "" 'valid values, comma delimeted

        pAttributes = New atcCollection
        pAttributes.Add("<dummy>")

        'F90_MSG("WRITE", 5) 'turn on very detailed debugging of fortran to error.fil

        Dim lMsgHandle As atcWdmHandle = MsgHandle()
        Dim lMsgUnit As Integer = lMsgHandle.Unit

        If lMsgUnit > 0 Then
            For lIndex = 1 To 500 'loop thru all possible attributes 
                'get info about attribute from message file
                Dim lAttr As New atcData.atcAttributeDefinition
                lAttr.Category = "WDM"
                Call F90_WDSAGY(lMsgUnit, lIndex, lLen, lType, lRMin, lRMax, lRDef, lHlpLen, lHlpRec, lHlpPos, lValidLen, lName, lDesc, lValid)
                If lLen = 0 Then 'dummy
                    lAttr.Name = "Dummy" & lIndex
                    lAttr.ID = lIndex
                    pAttributes.Add(lAttr, "K" & lIndex)
                Else
                    lAttr.Name = lName
                    lAttr.ID = lIndex
                    lAttr.Description = lDesc
                    lAttr.ValidList = New ArrayList(lValid.Split(","c))
                    If InStr("-TGROUP-TSFORM-VBTIME-COMPFG-TSFILL-TSBYR-TSBMO-TSBDY-TSBHR-TSPREC-TSSTEP-TCODE-Time Units-Time Step-", "-" & lName & "-") > 0 Then
                        lAttr.Editable = False
                    Else
                        lAttr.Editable = True
                    End If

                    Select Case lType
                        Case 1 : lAttr.TypeString = "Integer"
                        Case 2 : lAttr.TypeString = "Single"
                        Case 3
                            lAttr.TypeString = "String"
                            lAttr.Max = lLen 'max length of string
                    End Select

                    If lAttr.TypeString <> "String" Then 'numeric, save valid range
                        lAttr.Min = lRMin
                        lAttr.Max = lRMax
                    End If

                    lAttr.DefaultValue = lRDef

                    'use hlen, hrec, hpos to get myAttr.help 
                    '(missing entry point to WDGCVL in hass_ent?)
                    lAttr.Help = ""

                    pAttributes.Add(lName.ToLower, lAttr)
                End If
            Next lIndex
        End If
        lMsgHandle.Dispose()
    End Sub

    Public Function MsgHandle() As atcWdmHandle
        Dim lMsgFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
        Return New atcWdmHandle(1, lMsgFileName)
    End Function
End Class
