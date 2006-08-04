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
    Private Shared pAttributeTypeName() = {"None", "Integer", "Single", "String"}

    Public ReadOnly Property Attributes() As atcCollection
        Get
            Attributes = pAttributes
        End Get
    End Property

    Public Sub New()
        Dim lAttr As atcAttributeDefinition
        Dim lFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
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
                    lAttr.Name = lAttr.Name.Trim

                    lAttr.DefaultValue = lWdmMsg.ReadUInt32 'save location of details for later

                    lV = lWdmMsg.ReadUInt32
                    Dim lAttrType As Int32 = 1 + (lV >> 28)
                    lAttr.TypeString = pAttributeTypeName(lAttrType)
                    If lAttrType = 3 Then
                        lAttr.Max = (lV >> 21) And &H7F
                    End If
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
                    lAttr.DefaultValue = Nothing 'finished using DefaultValue to store pointer
                    If lDataPointerGroup > 0 Then 'data in this group
                        Dim lDataRecord As UInt32 = lDataPointerGroup >> 9
                        Dim lDataOffset As UInt32 = lDataPointerGroup And &H1FF
                        'Logger.Dbg("  Attribute:" & lAttr.ID & ":" & lDataRecord & ":" & lDataOffset)
                        lWdmMsg.Seek(lDataRecord, lDataOffset)
                        Dim lBlockIdentifier As UInt32 = lWdmMsg.ReadInt32
                        While lBlockIdentifier > 0
                            lDataOffset += 1 'count reading of lBlockIdentifier

                            Dim lTlen As UInt32 = lBlockIdentifier And &H1FF
                            Dim lInfoID As UInt32 = (lBlockIdentifier >> 9) And &HFFFF

                            lDataOffset += lTlen 'Is lTlen set for ID = 3?

                            Select Case lInfoID
                                Case 0 ' All done
                                    Exit While
                                Case 3 ' Range
                                    If ReferenceEquals(lAttr.TypeString, pAttributeTypeName(1)) Then ' Integer
                                        lAttr.Min = CDbl(lWdmMsg.ReadInt32)
                                        lAttr.Max = CDbl(lWdmMsg.ReadInt32)
                                    Else 'Single
                                        lAttr.Min = CDbl(lWdmMsg.ReadSingle)
                                        lAttr.Max = CDbl(lWdmMsg.ReadSingle)
                                    End If
                                Case 4 ' Valid responses
                                    If lAttr.ValidList Is Nothing Then lAttr.ValidList = New ArrayList
                                    lAttr.ValidList.Add(lWdmMsg.ReadString(lTlen))
                                Case 5 ' Default
                                    lAttr.DefaultValue = lWdmMsg.ReadString(lTlen)
                                Case 6 ' Description
                                    lAttr.Description &= lWdmMsg.ReadString(lTlen)
                                Case 7 ' Help
                                    lAttr.Help &= lWdmMsg.ReadString(lTlen)
                            End Select

                            If lDataOffset >= 511 Then 'move to next record
                                lDataRecord = lWdmMsg.ReadInt32(CInt(lDataRecord), 4)
                                lDataOffset = 5
                                lWdmMsg.Seek(lDataRecord, lDataOffset)
                                'Logger.Dbg("  NewRecord:" & lDataRecord)
                            End If
                            lBlockIdentifier = lWdmMsg.ReadInt32
                        End While
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
