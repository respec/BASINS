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
        Dim lWdmMsg As atcWdmFileHandle
        Dim lBinaryReader As IO.BinaryReader = GetEmbeddedFileAsBinaryReader("hspfmsg.wdm")
        If lBinaryReader Is Nothing Then
            Dim lFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
            lWdmMsg = New atcWdmFileHandle(1, lFileName)
        Else
            lWdmMsg = New atcWdmFileHandle(lBinaryReader)
        End If
        Dim lAttributeDsn As Int32 = lWdmMsg.ReadInt32(Wdm_Fields.DSFST_Attribute)
        While lAttributeDsn > 0 'loop through all possible attributes 
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

        ' Fill in details about each attribute
        pAttributes = New atcCollection
        For lInd As Int32 = 0 To pAttributeArray.GetUpperBound(0)
            lAttr = pAttributeArray(lInd)
            If pAttributeArray(lInd) Is Nothing Then
                lAttr = New atcAttributeDefinition
                lAttr.Name = "Attribute#" & lInd
                lAttr.ID = lInd
            Else
                Dim lDataPointerGroup As Int32 = lAttr.DefaultValue
                lAttr.DefaultValue = Nothing 'finished using DefaultValue to store pointer
                If lDataPointerGroup > 0 Then 'data in this group
                    Dim lDataRecord As UInt32 = lDataPointerGroup >> 9
                    Dim lDataOffset As UInt32 = lDataPointerGroup And &H1FF
                    'If lAttr.ID = 267 Then Stop Else Debug.Print(lAttr.ID)
                    'Logger.Dbg("  Attribute:" & lAttr.ID & ":" & lDataRecord & ":" & lDataOffset)
                    lWdmMsg.Seek(lDataRecord, lDataOffset)
                    Dim lBlockIdentifier As UInt32
                    Dim lTlen As UInt32
                    Dim lBlockWordsFromNextRecord As UInt32 = 0
                    Dim lNumWords As UInt32
                    Dim lInfoID As UInt32
                    Dim lValid As String = ""

                    Do ' Loop to read block ends when lInfoID = 0
                        If lBlockWordsFromNextRecord = 0 Then 'Starting a new block
                            lBlockIdentifier = lWdmMsg.ReadInt32
                            lDataOffset += 1 'count reading of lBlockIdentifier

                            lInfoID = (lBlockIdentifier >> 9) And &HFFFF
                            If lInfoID = 0 Then
                                Exit Do ' All Done
                            End If

                            lTlen = lBlockIdentifier And &H1FF
                            lNumWords = Math.Ceiling(lTlen / 4)
                        Else ' Continuing block in next record
                            lDataRecord = lWdmMsg.ReadInt32(CInt(lDataRecord), 4)
                            lDataOffset = 5
                            lWdmMsg.Seek(lDataRecord, lDataOffset)
                            lNumWords = lBlockWordsFromNextRecord
                            lBlockWordsFromNextRecord = 0
                        End If

                        lDataOffset += lNumWords
                        If lDataOffset > 512 Then
                            'Logger.Dbg("DataOffset:" & lDataOffset & " too big, skip to next")
                            lBlockWordsFromNextRecord = lDataOffset - 512 - 1
                            lNumWords -= lBlockWordsFromNextRecord
                        End If

                        Select Case lInfoID
                            Case 3 ' Range
                                If ReferenceEquals(lAttr.TypeString, pAttributeTypeName(1)) Then 'Integer Range
                                    lAttr.Min = CDbl(lWdmMsg.ReadInt32)
                                    lAttr.Max = CDbl(lWdmMsg.ReadInt32)
                                ElseIf ReferenceEquals(lAttr.TypeString, pAttributeTypeName(2)) Then 'Single Range
                                    lAttr.Min = CDbl(lWdmMsg.ReadSingle)
                                    lAttr.Max = CDbl(lWdmMsg.ReadSingle)
                                Else
                                    Logger.Dbg("WARNING: Range not read for " & lAttr.Name & " type " & lAttr.TypeString & " ID:Rec:Off:" & lInfoID & ":" & lDataRecord & ":" & lDataOffset)
                                End If
                            Case 4 'Valid Responses
                                lValid &= lWdmMsg.ReadString(lNumWords)
                            Case 5 'Default Value
                                If ReferenceEquals(lAttr.TypeString, pAttributeTypeName(1)) Then 'Integer Default
                                    lAttr.DefaultValue = lWdmMsg.ReadInt32
                                    If lAttr.DefaultValue = -999 Then
                                        lAttr.DefaultValue = GetNaN()
                                        ' ElseIf lAttr.DefaultValue <> 0 Then
                                        '    Logger.Dbg("NonZeroDefault:" & lAttr.Name & ":" & lAttr.DefaultValue)
                                    End If
                                ElseIf ReferenceEquals(lAttr.TypeString, pAttributeTypeName(2)) Then 'Single Default
                                    lAttr.DefaultValue = lWdmMsg.ReadSingle
                                    If lAttr.DefaultValue = -999 Then
                                        lAttr.DefaultValue = GetNaN()
                                        ' ElseIf lAttr.DefaultValue <> 0 Then
                                        '    Logger.Dbg("NonZeroDefault:" & lAttr.Name & ":" & lAttr.DefaultValue)
                                    End If
                                ElseIf ReferenceEquals(lAttr.TypeString, pAttributeTypeName(3)) Then 'String Default
                                    lAttr.DefaultValue &= lWdmMsg.ReadString(lNumWords)
                                Else
                                    Logger.Dbg("WARNING: Default not read for " & lAttr.Name & " type " & lAttr.TypeString & " ID:Rec:Off:" & lInfoID & ":" & lDataRecord & ":" & lDataOffset)
                                End If
                            Case 6 ' Description
                                lAttr.Description &= lWdmMsg.ReadString(lNumWords)
                            Case 7 ' Help
                                lAttr.Help &= lWdmMsg.ReadString(lNumWords)
                            Case Else
                                Logger.Dbg("WARNING: Unknown Block ID:Rec:Off:" & lInfoID & ":" & lDataRecord & ":" & lDataOffset)
                        End Select
                    Loop

                    If lValid.Length > 0 Then
                        lAttr.ValidList = New ArrayList
                        While lValid.Length > 0
                            lAttr.ValidList.Add(StrRetRem(lValid))
                        End While
                    End If
                End If
            End If
            pAttributes.Add(lAttr.Name.ToLower, lAttr)
        Next
        Logger.Dbg("MessageFileReadWithVBDone!")
    End Sub
End Class
