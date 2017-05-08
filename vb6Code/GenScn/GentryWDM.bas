Attribute VB_Name = "GentryWDM"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'this code was put here to make wdm update functionality available to
'genscn for stream depletion program

'Public Sub UpdateWDMTs(sdt&(), edt&(), Vals#(), Tsr As Timser, retcod&)

    'update a WDM time-series data set with a buffer of time-series values
    'sdt  - start date of updated time series
    'edt  - end date of updated time series
    'Vals - buffer of time-series values
    'Tsr  - time series being updated

'    Dim i&, nv&, lnv&, ioff&, ts(0) As Timser, tsbuf!()
'    Dim stu&, sts&, sdtr&, ssdt&(5), sedt&(5)
'
'    'determine number of values being updated
'    Call timdif(sdt(0), edt(0), Tsr.Tu, Tsr.ts, lnv)
'    i = F90_TIMCHK(edt(0), Tsr.edat(0))
'    If i = 1 Then
'      'won't be updating to end of available data,
'      'need to update all data since WDTPUT only overwrites to end of all data
'      ts(0) = Tsr
'      'determine number of values in entire time series
'      Call timdif(ts(0).sdat(0), ts(0).edat(0), ts(0).Tu, ts(0).ts, nv)
'      If nv > ts(0).NVal Then 'not all data retrieved for this time series
'        'save current time specs
'        stu = ctunit
'        sts = CTStep
'        sdtr = CDTran
'        Call CopyI(6, CSDat(), ssdt())
'        Call CopyI(6, CEDat(), sedt())
'        'update time specs to retrieve all available data
'        ctunit = ts(0).Tu
'        CTStep = ts(0).ts
'        CDTran = ts(0).Dtran
'        Call CopyI(6, ts(0).sdat(), CSDat())
'        Call CopyI(6, ts(0).edat(), CEDat())
'        Call FillTimSer(1, ts())
'        'restore saved time specs
'        ctunit = stu
'        CTStep = sts
'        CDTran = sdtr
'        Call CopyI(6, ssdt(), CSDat())
'        Call CopyI(6, sedt(), CEDat())
'      End If
'      'find offset of start of values and start of timeseries
'      Call timdif(ts(0).sdat(0), sdt(0), ts(0).Tu, ts(0).ts, ioff)
'      'put updated time series in value buffer
'      For i = 0 To lnv - 1
'        ts(0).Vals(ioff + i) = CSng(Vals(i))
'      Next i
'      'now update WDM data set
'      Call F90_WDTPUT(p.WDMFiles(1).fileUnit, CLng(ts(0).id), ts(0).ts, ts(0).sdat(0), ts(0).NVal, 1, 0, ts(0).Tu, ts(0).Vals(0), retcod)
'    Else 'just update WDM data set to end of data using WDTPUT
'      ReDim tsbuf(lnv)
'      For i = 0 To lnv - 1
'        tsbuf(i) = CSng(Vals(i))
'      Next i
'      Call F90_WDTPUT(p.WDMFiles(1).fileUnit, CLng(Tsr.id), Tsr.ts, sdt(0), lnv, 1, 0, Tsr.Tu, tsbuf(0), retcod)
'    End If

'End Sub
