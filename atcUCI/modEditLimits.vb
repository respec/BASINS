'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Module modEditLimits

    Private NoDsn() As Integer
    Private NoDsnCount, resp As Integer
    Private scen() As String
    Private locn() As String
    Private cons() As String
    Private cwdmid() As String
	
    Public Sub GetNonExistentDataSetInfo(ByRef i As Integer, _
                                         ByRef adsn As Integer, _
                                         ByRef wid As String, _
                                         ByRef s As String, _
                                         ByRef l As String, _
                                         ByRef c As String)

        adsn = NoDsn(i)
        wid = cwdmid(i)
        s = scen(i)
        l = locn(i)
        c = cons(i)
    End Sub
	
    Public Sub GetNonExistentDataSetCount(ByRef n As Integer)
        n = NoDsnCount
    End Sub
	
    Public Sub UpdateRespFromAddDataSet(ByRef i As Integer)
        '1 - cancel from 'add data set'
        '0 - ok from 'add data set'
        '-1 - 'add data set' not needed
        resp = i
    End Sub
End Module