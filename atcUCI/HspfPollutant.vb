Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfPollutant_NET.HspfPollutant")> Public Class HspfPollutant
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Dim pId As Integer
	Dim pName As String
	Dim pModelType As String
	Dim pTables As Collection
	Dim pMassLinks As Collection
	Dim pOperations As Collection
	Dim pIndex As Integer
	
	Public Property Id() As Integer
		Get
			Id = pId
		End Get
		Set(ByVal Value As Integer)
			pId = Value
		End Set
	End Property
	Public Property Index() As Integer
		Get
			Index = pIndex
		End Get
		Set(ByVal Value As Integer)
			pIndex = Value
		End Set
	End Property
	Public Property Name() As String
		Get
			Name = pName
		End Get
		Set(ByVal Value As String)
			pName = Value
		End Set
	End Property
	Public Property ModelType() As String
		Get
			ModelType = pModelType
		End Get
		Set(ByVal Value As String)
			pModelType = Value
		End Set
	End Property
	Public ReadOnly Property Tables() As Collection
		Get 'of HspfTable
			Tables = pTables
		End Get
	End Property
	Public Property Operations() As Collection
		Get 'of HspfOperation
			Operations = pOperations
		End Get
		Set(ByVal Value As Collection) 'of HspfMassLinks
			Dim lOperation As HspfOperation
			Dim vOperation As Object
			For	Each vOperation In Value
				lOperation = vOperation
				pOperations.Add(lOperation)
			Next vOperation
		End Set
	End Property
	Public Property MassLinks() As Collection
		Get 'of HspfMasslink
			MassLinks = pMassLinks
		End Get
		Set(ByVal Value As Collection) 'of HspfMassLinks
			Dim lMassLink As HspfMassLink
			Dim vMassLink As Object
			For	Each vMassLink In Value
				lMassLink = vMassLink
				pMassLinks.Add(lMassLink)
			Next vMassLink
		End Set
	End Property
	
    Public Sub New()
        MyBase.New()
        pTables = New Collection
        pOperations = New Collection
        pMassLinks = New Collection
        pId = 0
        pName = ""
        pModelType = ""
        pIndex = 0
    End Sub
	
	Public Function TableExists(ByRef Name As String) As Boolean
		Dim vTable As Object
		
		On Error GoTo NoTable
		vTable = pTables.Item(Name)
		TableExists = True
		Exit Function
NoTable: 
		TableExists = False
	End Function
End Class