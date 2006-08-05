'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports atcWdmVb.atcWdmFileHandle

Friend Class atcMsgWDMvb
    Private pAttributes As atcCollection 'of clsAttributeDefinition
    Private pAttributeArray(512) As atcAttributeDefinition
    Private Shared pAttributeTypeName() = {"None", "Integer", "Single", "String"}

    Public ReadOnly Property Attributes() As atcCollection
        Get
            Attributes = pAttributes
        End Get
    End Property

    Public Sub New()
        Logger.Dbg("MessageFileReadWithVB")
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

        ' fill in details about each attribute
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
                    lDataOffset += 1 'count reading of lBlockIdentifier
                    Dim lBlockWordsFromNextRecord As UInt32 = 0
                    Dim lNumWords As UInt32
                    Dim lInfoID As UInt32
                    Dim lValid As String = ""
                    While lBlockIdentifier > 0
                        If lBlockWordsFromNextRecord = 0 Then
                            Dim lTlen As UInt32 = lBlockIdentifier And &H1FF
                            lNumWords = Math.Ceiling(lTlen / 4)
                            lInfoID = (lBlockIdentifier >> 9) And &HFFFF
                        Else
                            lBlockWordsFromNextRecord = 0
                        End If

                        lDataOffset += lNumWords
                        If lDataOffset > 512 Then
                            'Logger.Dbg("DataOffset:" & lDataOffset & " too big, skip to next")
                            lBlockWordsFromNextRecord = lDataOffset - 512 - 1
                            lNumWords -= lBlockWordsFromNextRecord
                        End If

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
                                lValid &= lWdmMsg.ReadString(lNumWords)
                            Case 5 ' Default
                                lAttr.DefaultValue &= lWdmMsg.ReadString(lNumWords)
                            Case 6 ' Description
                                lAttr.Description &= lWdmMsg.ReadString(lNumWords)
                            Case 7 ' Help
                                lAttr.Help &= lWdmMsg.ReadString(lNumWords)
                            Case Else
                                Logger.Dbg("Unknown Block ID:Rec:Off:" & lInfoID & ":" & lDataRecord & ":" & lDataOffset)
                        End Select

                        If lDataOffset > 512 Then 'move to next record
                            lDataRecord = lWdmMsg.ReadInt32(CInt(lDataRecord), 4)
                            lDataOffset = 5
                            lWdmMsg.Seek(lDataRecord, lDataOffset)
                            'Logger.Dbg("  NewRecord:" & lDataRecord)
                        End If
                        If lBlockWordsFromNextRecord = 0 Then
                            lBlockIdentifier = lWdmMsg.ReadInt32
                            lDataOffset += 1 'count reading of lBlockIdentifier
                        Else
                            lNumWords = lBlockWordsFromNextRecord
                        End If
                    End While

                    While lValid.Length > 0
                        lAttr.ValidList.Add(StrRetRem(lValid))
                    End While
                End If
            End If
        Next

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
        Logger.Dbg("MessageFileReadWithVBDone!")
    End Sub
End Class
