Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility

Friend Class atcMsgWDM
  'Copyright 2005 by AQUA TERRA Consultants - Royalty-free use permitted under open source license

  Dim pMsgUnit As Integer 'fortran unit number of message wdm file
  Dim pAttributes As Collection 'of clsAttributeDefinition

  Public ReadOnly Property MsgUnit() As Integer
    Get
      Return pMsgUnit
    End Get
  End Property

  'Public ReadOnly Property Attrib(ByVal AttrName As String) As atcAttributeDefinition
  '  'TODO who uses this? why not use clsDataAttributes.GetDefinition?
  '  Get
  '    Try
  '      Attrib = pAttributes.Item(AttrName)
  '    Catch
  '      Attrib = Nothing
  '    End Try
  '  End Get
  'End Property

  Public ReadOnly Property Attributes() As Collection
    Get
      Attributes = pAttributes
    End Get
  End Property

  Public Sub New()
    pAttributes = New Collection
    Dim lIndex As Integer 'index
    Dim ilen As Integer 'max dimension
    Dim itype As Integer 'type (1-Integer, 2-Real, 3-String, 0-Dummy)
    Dim rmax As Single 'max allowed value
    Dim rmin As Single 'min allowed value
    Dim rdef As Single 'default value
    Dim hpos As Integer 'help position within message file record
    Dim hlen As Integer 'help length
    Dim hrec As Integer 'help record withing message file
    Dim vlen As Integer 'length of valid values
    Dim desc As String  'description 
    Dim aName As String 'name
    Dim valid As String 'valid values, comma delimeted

    Dim myAttr As atcAttributeDefinition

    F90_MSG("WRITE", 5)
    Dim hspfMsgFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
    If Not FileExists(hspfMsgFileName) Then
      LogMsg("Could not find hspfmsg.wdm - " & hspfMsgFileName, "atcMsgWdm")
    Else
      pMsgUnit = F90_WDBOPN(0, hspfMsgFileName, Len(hspfMsgFileName))

      For lIndex = 1 To 500 'loop thru all possible attributes 

        'get info about attribute from HSPFMSG.WDM
        Call F90_WDSAGY(pMsgUnit, lIndex, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, aName, desc, valid)

        myAttr = New atcAttributeDefinition

        If ilen = 0 Then 'dummy
          myAttr.Name = "Dummy" & lIndex
          myAttr.ID = lIndex
          pAttributes.Add(myAttr, "K" & lIndex)
        Else
          myAttr.Name = aName
          myAttr.ID = lIndex
          myAttr.Description = desc
          myAttr.ValidList = New ArrayList(valid.Split(","))

          If InStr("-TGROUP-TSFORM-VBTIME-COMPFG-TSFILL-TSBYR-TSBMO-TSBDY-TSBHR-TSPREC-TSSTEP-TCODE-Time Units-Time Step-", "-" & aName & "-") > 0 Then
            myAttr.Editable = False
          Else
            myAttr.Editable = True
          End If

          Select Case itype
            Case 1 : myAttr.TypeString = "Integer"
            Case 2 : myAttr.TypeString = "Single"
            Case 3
              myAttr.TypeString = "String"
              myAttr.Max = ilen 'max length of string
          End Select

          If myAttr.TypeString <> "String" Then 'numeric, save valid range
            myAttr.Min = rmin
            myAttr.Max = rmax
          End If

          myAttr.DefaultValue = rdef

          'use hlen, hrec, hpos to get myAttr.help 
          '(missing entry point to WDGCVL in hass_ent?)
          myAttr.Help = ""

          pAttributes.Add(myAttr, aName)

        End If
      Next lIndex
    End If
  End Sub

  Protected Overrides Sub Finalize()
    If pMsgUnit > 0 Then
      Dim i As Integer = F90_WDMCLO(pMsgUnit)
      pMsgUnit = 0
    End If
  End Sub
End Class
