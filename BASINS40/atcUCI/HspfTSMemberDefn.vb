Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfTSMemberDef_NET.HspfTSMemberDef")> _
Public Class HspfTSMemberDef
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pName As String
    Private pId As Integer
    Private pTSGroupID As Integer
    Private pParent As HspfTSGroupDef
    Private pSCLU As Integer
    Private pSGRP As Integer
    Private pmdim1 As Integer
    Private pmdim2 As Integer
    Private pmaxsb1 As Integer
    Private pmaxsb2 As Integer
    Private pmkind As Integer
    Private psptrn As Integer
    Private pmsect As Integer
    Private pmio As Integer
    Private posvbas As Integer
    Private posvoff As Integer
    Private peunits As String
    Private pltval1 As Single
    Private pltval2 As Single
    Private pltval3 As Single
    Private pltval4 As Single
    Private pdefn As String
    Private pmunits As String
    Private pltval5 As Single
    Private pltval6 As Single
    Private pltval7 As Single
    Private pltval8 As Single

    Public Property Name() As String
        Get
            Name = pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Id = pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property TSGroupId() As Integer
        Get
            TSGroupId = pTSGroupID
        End Get
        Set(ByVal Value As Integer)
            pTSGroupID = Value
        End Set
    End Property

    Public Property Parent() As HspfTSGroupDef
        Get
            Parent = pParent
        End Get
        Set(ByVal Value As HspfTSGroupDef)
            pParent = Value
        End Set
    End Property

    Public Property SCLU() As Integer
        Get
            SCLU = pSCLU
        End Get
        Set(ByVal Value As Integer)
            pSCLU = Value
        End Set
    End Property

    Public Property SGRP() As Integer
        Get
            SGRP = pSGRP
        End Get
        Set(ByVal Value As Integer)
            pSGRP = Value
        End Set
    End Property

    Public Property mdim1() As Integer
        Get
            mdim1 = pmdim1
        End Get
        Set(ByVal Value As Integer)
            pmdim1 = Value
        End Set
    End Property

    Public Property mdim2() As Integer
        Get
            mdim2 = pmdim2
        End Get
        Set(ByVal Value As Integer)
            pmdim2 = Value
        End Set
    End Property

    Public Property maxsb1() As Integer
        Get
            maxsb1 = pmaxsb1
        End Get
        Set(ByVal Value As Integer)
            pmaxsb1 = Value
        End Set
    End Property

    Public Property maxsb2() As Integer
        Get
            maxsb2 = pmaxsb2
        End Get
        Set(ByVal Value As Integer)
            pmaxsb2 = Value
        End Set
    End Property

    Public Property mkind() As Integer
        Get
            mkind = pmkind
        End Get
        Set(ByVal Value As Integer)
            pmkind = Value
        End Set
    End Property

    Public Property sptrn() As Integer
        Get
            sptrn = psptrn
        End Get
        Set(ByVal Value As Integer)
            psptrn = Value
        End Set
    End Property

    Public Property msect() As Integer
        Get
            msect = pmsect
        End Get
        Set(ByVal Value As Integer)
            pmsect = Value
        End Set
    End Property

    Public Property mio() As Integer
        Get
            mio = pmio
        End Get
        Set(ByVal Value As Integer)
            pmio = Value
        End Set
    End Property

    Public Property osvbas() As Integer
        Get
            osvbas = posvbas
        End Get
        Set(ByVal Value As Integer)
            posvbas = Value
        End Set
    End Property

    Public Property osvoff() As Integer
        Get
            osvoff = posvoff
        End Get
        Set(ByVal Value As Integer)
            posvoff = Value
        End Set
    End Property

    Public Property eunits() As String
        Get
            eunits = peunits
        End Get
        Set(ByVal Value As String)
            peunits = Value
        End Set
    End Property

    Public Property ltval1() As Single
        Get
            ltval1 = pltval1
        End Get
        Set(ByVal Value As Single)
            pltval1 = Value
        End Set
    End Property

    Public Property ltval2() As Single
        Get
            ltval2 = pltval2
        End Get
        Set(ByVal Value As Single)
            pltval2 = Value
        End Set
    End Property

    Public Property ltval3() As Single
        Get
            ltval3 = pltval3
        End Get
        Set(ByVal Value As Single)
            pltval3 = Value
        End Set
    End Property

    Public Property ltval4() As Single
        Get
            ltval4 = pltval4
        End Get
        Set(ByVal Value As Single)
            pltval4 = Value
        End Set
    End Property

    Public Property defn() As String
        Get
            defn = pdefn
        End Get
        Set(ByVal Value As String)
            pdefn = Value
        End Set
    End Property

    Public Property munits() As String
        Get
            munits = pmunits
        End Get
        Set(ByVal Value As String)
            pmunits = Value
        End Set
    End Property

    Public Property ltval5() As Single
        Get
            ltval5 = pltval5
        End Get
        Set(ByVal Value As Single)
            pltval5 = Value
        End Set
    End Property

    Public Property ltval6() As Single
        Get
            ltval6 = pltval6
        End Get
        Set(ByVal Value As Single)
            pltval6 = Value
        End Set
    End Property

    Public Property ltval7() As Single
        Get
            ltval7 = pltval7
        End Get
        Set(ByVal Value As Single)
            pltval7 = Value
        End Set
    End Property

    Public Property ltval8() As Single
        Get
            ltval8 = pltval8
        End Get
        Set(ByVal Value As Single)
            pltval8 = Value
        End Set
    End Property
End Class