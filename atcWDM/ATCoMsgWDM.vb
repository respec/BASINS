Option Strict Off
Option Explicit On 

Imports ATCdataStructures

Public Class ATCoMsgWDM
  'Copyright 2005 by AQUA TERRA Consultants
  Dim pMsgUnit As Integer
  Dim pAttributes As Collection 'of clsAttributeWDM

  Public WriteOnly Property MsgUnit() As Integer
    Set(ByVal Value As Integer)
      Dim i As Integer
      Dim ilen, itype As Integer
      Dim rmax, rmin, rdef As Single
      Dim hpos, hlen, hrec, vlen As Integer
      Dim desc, aName, valid As String
      Dim myAttr As clsAttributeWDM

      pMsgUnit = Value
      For i = 1 To 500
        Call F90_WDSAGY(pMsgUnit, i, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, aName, desc, valid)
        myAttr = New clsAttributeWDM
        If ilen = 0 Then 'dummy
          myAttr.Name = "Dummy" & i
          pAttributes.Add(myAttr, "K" & i)
        Else
          myAttr.Name = aName
          myAttr.Ind = i
          myAttr.Description = desc
          myAttr.ValidList = New ArrayList(valid.Split(","))

          If InStr("-TGROUP-TSFORM-VBTIME-COMPFG-TSFILL-TSBYR-TSBMO-TSBDY-TSBHR-TSPREC-TSSTEP-TCODE-Time Units-Time Step-", "-" & aName & "-") > 0 Then
            myAttr.Editable = False
          Else
            myAttr.Editable = True
          End If
          myAttr.ilen = ilen
          Select Case itype
            Case 1 : myAttr.TypeString = "Integer"
            Case 2 : myAttr.TypeString = "Single"
            Case 3 : myAttr.TypeString = "String"
          End Select
          'myAttr.itype = itype
          myAttr.Min = rmin
          myAttr.Max = rmax
          myAttr.DefaultValue = rdef
          myAttr.hlen = hlen
          myAttr.hrec = hrec
          myAttr.hpos = hpos
          myAttr.vlen = vlen
          pAttributes.Add(myAttr, aName)
        End If
      Next i

    End Set
  End Property

  Public ReadOnly Property Attrib(ByVal AttrName As String) As clsAttributeWDM
    Get
      On Error GoTo NoSuchAttribute
      Attrib = pAttributes.Item(AttrName)
      Exit Property
NoSuchAttribute:
      Attrib = Nothing
    End Get
  End Property

  Public ReadOnly Property Attributes() As Collection
    Get
      Attributes = pAttributes
    End Get
  End Property

  Public Sub New()
    MyBase.New()
    pAttributes = New Collection
  End Sub
End Class