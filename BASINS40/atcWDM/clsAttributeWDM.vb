Option Strict Off
Option Explicit On 

Imports ATCdataStructures

Public Class AttributeWDM
  Inherits AttributeDefinition
  'Copyright 2002 by AQUA TERRA Consultants

  Private pInd As Integer
  Private piLen As Integer
  Private phLen As Integer
  Private phRec As Integer
  Private phPos As Integer
  Private pvLen As Integer

  Private Function ATCclsAttributeDefinition_Dump() As String
    Dim retval As String
    retval = MyBase.ToString

    If pInd <> 0 Then retval &= " Ind: " & pInd
    If piLen <> 0 Then retval &= " iLen: " & piLen
    If phLen <> 0 Then retval &= " hLen: " & phLen
    If phRec <> 0 Then retval &= " hRec: " & phRec
    If phPos <> 0 Then retval &= " hPos: " & phPos
    If pvLen <> 0 Then retval &= " vLen: " & piLen

    Return retval
  End Function

  Public Property Ind() As Integer
    Get
      Ind = pInd
    End Get
    Set(ByVal Value As Integer)
      pInd = Value
    End Set
  End Property

  Public Property ilen() As Integer
    Get
      ilen = piLen
    End Get
    Set(ByVal Value As Integer)
      piLen = Value
    End Set
  End Property

  Public Property hlen() As Integer
    Get
      hlen = phLen
    End Get
    Set(ByVal Value As Integer)
      phLen = Value
    End Set
  End Property

  Public Property hrec() As Integer
    Get
      hrec = phRec
    End Get
    Set(ByVal Value As Integer)
      phRec = Value
    End Set
  End Property

  Public Property hpos() As Integer
    Get
      hpos = phPos
    End Get
    Set(ByVal Value As Integer)
      phPos = Value
    End Set
  End Property

  Public Property vlen() As Integer
    Get
      vlen = pvLen
    End Get
    Set(ByVal Value As Integer)
      pvLen = Value
    End Set
  End Property
End Class