Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports atcWdmVb.atcWdmFileHandle

Friend Class atcMsgWDMvb
    'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Private pAttributes As atcCollection 'of clsAttributeDefinition
    Private pAttributeArray(512) As atcAttributeDefinition

    Public ReadOnly Property Attributes() As atcCollection
        Get
            Attributes = pAttributes
        End Get
    End Property

    Public Sub New()
        Dim lAttr As atcAttributeDefinition

        Dim lFileName As String = "C:\dev\BASINS40\Bin\Plugins\BASINS\hspfmsg.wdm"
        Dim lWdmMsg As New atcWdmFileHandle(1, lFileName)

        Dim lAttributeDsn As Int32 = lWdmMsg.ReadInt32(Wdm_Fields.DSFST_Attribute)
        While lAttributeDsn > 0 'loop thru all possible attributes 
            Dim lBaseRec As Int32 = lWdmMsg.FirstRecordNumberFromDsn(lAttributeDsn)
            Dim lRec As Int32 = lBaseRec
            Dim lPointDataBlocks As Int32 = lWdmMsg.ReadInt32(lRec, 11) 'PDAT
            Dim lPointDataValues As Int32 = lWdmMsg.ReadInt32 'PDATV
            Dim lDataPointerInUseCount As Int32 = lWdmMsg.ReadInt32(lRec, lPointDataBlocks) 'GRPCNT
            Dim lFreePosition As UInt32 = lWdmMsg.ReadUInt32  'FREPOS
            Dim lDataPointerCount As Int32 = (lPointDataValues - lPointDataBlocks - 2) / 4
            For lInd As Int32 = 1 To lDataPointerCount
                Dim lV As Int32 = lWdmMsg.ReadInt32
                If lV > 0 Then
                    lAttr = New atcAttributeDefinition
                    lAttr.Category = "WDM"
                    lAttr.Name &= Long2String(lV)
                    lV = lWdmMsg.ReadUInt32
                    lAttr.ID = lV And &H1FF
                    lAttr.Name &= Chr((lV >> 9) And &HFF)
                    lAttr.Name &= Chr((lV >> 17) And &HFF)
                    lAttr.DefaultValue = lWdmMsg.ReadUInt32 'save location of details for later
                    lV = lWdmMsg.ReadUInt32
                    Dim lAttrType As Int32 = 1 + (lV >> 28)

                    lAttr.Name = lAttr.Name.Trim
                    Select Case lAttrType
                        Case 1
                            lAttr.TypeString = "Integer"
                        Case 2 : lAttr.TypeString = "Single"
                        Case 3
                            lAttr.TypeString = "String"
                            lAttr.Max = (lV >> 21) And &H7F
                    End Select
                    If InStr("-TGROUP-TSFORM-VBTIME-COMPFG-TSFILL-TSBYR-TSBMO-TSBDY-TSBHR-TSPREC-TSSTEP-TCODE-Time Units-Time Step-", _
                             "-" & lAttr.Name & "-") > 0 Then
                        lAttr.Editable = False
                    Else
                        lAttr.Editable = True
                    End If
                    pAttributeArray(lAttr.ID) = lAttr
                End If
            Next

            lAttributeDsn = lWdmMsg.ReadInt32(lBaseRec, 2)
        End While

        Try
            For lInd As Int32 = 0 To pAttributeArray.GetUpperBound(0)
                lAttr = pAttributeArray(lInd)
                If Not (lAttr Is Nothing) Then
                    Dim lDataPointerGroup As Int32 = lAttr.DefaultValue
                    If lDataPointerGroup > 0 Then 'data in this group
                        Dim lDataRecord As UInt32 = lDataPointerGroup >> 9
                        Dim lDataOffset As UInt32 = lDataPointerGroup And &H1FF
                        Logger.Dbg("  Attribute:" & lAttr.ID & ":" & lDataRecord & ":" & lDataOffset)
                        lWdmMsg.Seek(lDataRecord, lDataOffset)

                        Dim lRMax As Single 'max allowed value
                        Dim lRMin As Single 'min allowed value
                        Dim lRDef As Single 'default value
                        Dim lHlpPos As Integer 'help position within message file record
                        Dim lHlpLen As Integer 'help length
                        Dim lHlpRec As Integer 'help record withing message file
                        Dim lValidLen As Integer 'length of valid values
                        Dim lDesc As String = "" 'description 
                        Dim lValid As String = "" 'valid values, comma delimeted

                        'get info about attribute from message file
                        'Call F90_WDSAGY(lMsgUnit, lIndex, lLen, lType, lRMin, lRMax, lRDef, lHlpLen, lHlpRec, lHlpPos, lValidLen, lName, lDesc, lValid)

                        lAttr.Description = lDesc

                        lAttr.ValidList = New ArrayList(lValid.Split(Chr(44))) ' 44=comma

                        If lAttr.TypeString <> "String" Then
                            'numeric, save valid range
                            lAttr.Min = lRMin
                            lAttr.Max = lRMax
                        End If

                        lAttr.DefaultValue = lRDef

                        'use hlen, hrec, hpos to get myAttr.help 
                        '(missing entry point to WDGCVL in hass_ent?)
                        lAttr.Help = ""

                    End If
                End If
            Next
        Catch ex As Exception
            Stop
        End Try

        pAttributes = New atcCollection
        For lInd As Int32 = 0 To pAttributeArray.GetUpperBound(0)
            If pAttributeArray(lInd) Is Nothing Then
                lAttr = New atcAttributeDefinition
                lAttr.Name = "Dummy" & lInd
                lAttr.ID = lInd
                'pAttributes.Add(lAttr, "K" & lInd)
            Else
                lAttr = pAttributeArray(lInd)
            End If
            pAttributes.Add(lAttr.Name.ToLower, lAttr)
        Next
        Logger.Dbg("MessageFileReadWithVB!")
    End Sub
End Class
