Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcBasinsObsWQ
Imports atcGraph
Imports MapWindow.Interfaces
Imports ZedGraph
Imports MapWinUtility
Imports System
Imports atcHspfBinOut

Module GraphBasicMPCAWQ
    Private pbasename As String
    Private AxisUpper As Integer = 0
    Private AxisLower As Integer = 0
    Private pWQGraphSpecification As Object
    Private pWorkingDirectory As String
    Private pObservedWQBaseFileName As String
    Private Const pTimeseries1Axis As String = "Aux"
    Private Const pTimeseries1IsPoint As Boolean = False
    Private Const pTimeseries2Axis As String = "Aux"
    Private Const pTimeseries2IsPoint As Boolean = False
    Private Const pTimeseries3Axis As String = "Left"
    Private Const pTimeseries3IsPoint As Boolean = False
    Private Const pTimeseries4Axis As String = "Left"
    Private Const pTimeseries4IsPoint As Boolean = True
    Private Const pTimeseries5Axis As String = "Left"
    Private Const pTimeseries5IsPoint As Boolean = True
    Private Const pTimeseries6Axis As String = "Left"
    Private Const pTimeseries6IsPoint As Boolean = True
    Private Const pTimeseries7Axis As String = "Left"
    Private Const pTimeseries7IsPoint As Boolean = True
    Private Const pTimeseries8Axis As String = "Left"
    Private Const pTimeseries8IsPoint As Boolean = True
    Private Const pTimeseries9Axis As String = "Right"
    Private Const pTimeseries9IsPoint As Boolean = False
    Private pLeftAxisLogScale As Boolean = False
    Private pTimeseriesConstituent, pLeftYAxisLabel, pLeftAuxAxisLabel, foldername As String
    Private pHSPFExe As String = "C:\Basins41\models\HSPF\bin\WinHspfLt.exe"
    Private location As String = ""
    Private constituent As String = ""

    Private pGraphSpecMPCAWQ(,) As Object = { _
    {"RCH104", "DOX", 2003, 1, 1, 2009, 12, 31, "West Leaf Lake", "", ""}, _
    {"RCH104", "TP", 2003, 1, 1, 2009, 12, 31, "West Leaf Lake", "", ""}, _
    {"RCH104", "TSS", 2003, 1, 1, 2009, 12, 31, "West Leaf Lake", "", ""}, _
    {"RCH104", "TW", 2003, 1, 1, 2009, 12, 31, "West Leaf Lake", "", ""}, _
    {"RCH106", "TP", 2003, 1, 1, 2009, 12, 31, "East Leaf Lake", "", ""}, _
    {"RCH121", "DOX", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "NO3", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "ORC", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "PO4", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "TAM", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "TORN", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "TP", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "TSS", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH121", "TW", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH123", "TP", 2003, 1, 1, 2009, 12, 31, "Wolf Lake", "", ""}, _
    {"RCH125", "DOX", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "NO3", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "ORC", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "PO4", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "TAM", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "TORN", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "TP", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "TSS", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH125", "TW", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH131", "TW", 2003, 1, 1, 2009, 12, 31, "H13058001", "", ""}, _
    {"RCH301", "DOX", 2003, 1, 1, 2009, 12, 31, "Andrew Lake", "", ""}, _
    {"RCH301", "TP", 2003, 1, 1, 2009, 12, 31, "Andrew Lake", "", ""}, _
    {"RCH301", "TW", 2003, 1, 1, 2009, 12, 31, "Andrew Lake", "", ""}, _
    {"RCH302", "DOX", 2003, 1, 1, 2009, 12, 31, "Mary Lake", "", ""}, _
    {"RCH302", "TP", 2003, 1, 1, 2009, 12, 31, "Mary Lake", "", ""}, _
    {"RCH302", "TSS", 2003, 1, 1, 2009, 12, 31, "Mary Lake", "", ""}, _
    {"RCH302", "TW", 2003, 1, 1, 2009, 12, 31, "Mary Lake", "", ""}, _
    {"RCH304", "DOX", 2003, 1, 1, 2009, 12, 31, "Lobster Lk East", "Lobster Lk West", ""}, _
    {"RCH304", "TP", 2003, 1, 1, 2009, 12, 31, "Lobster Lk East", "Lobster Lk West", ""}, _
    {"RCH304", "TSS", 2003, 1, 1, 2009, 12, 31, "Lobster Lk East", "Lobster Lk West", ""}, _
    {"RCH304", "TW", 2003, 1, 1, 2009, 12, 31, "Lobster Lk East", "Lobster Lk West", ""}, _
    {"RCH308", "TP", 2003, 1, 1, 2009, 12, 31, "Irene Lake", "", ""}, _
    {"RCH308", "TW", 2003, 1, 1, 2009, 12, 31, "Irene Lake", "", ""}, _
    {"RCH309", "TP", 2003, 1, 1, 2009, 12, 31, "Miltona Lake", "", ""}, _
    {"RCH309", "TSS", 2003, 1, 1, 2009, 12, 31, "Miltona Lake", "", ""}, _
    {"RCH309", "TW", 2003, 1, 1, 2009, 12, 31, "Miltona Lake", "", ""}, _
    {"RCH310", "DOX", 2003, 1, 1, 2009, 12, 31, "Ida Lake", "", ""}, _
    {"RCH310", "TP", 2003, 1, 1, 2009, 12, 31, "Ida Lake", "", ""}, _
    {"RCH310", "TW", 2003, 1, 1, 2009, 12, 31, "Ida Lake", "", ""}, _
    {"RCH311", "DOX", 2003, 1, 1, 2009, 12, 31, "Latoka Lake", "", ""}, _
    {"RCH311", "TP", 2003, 1, 1, 2009, 12, 31, "Latoka Lake", "", ""}, _
    {"RCH311", "TSS", 2003, 1, 1, 2009, 12, 31, "Latoka Lake", "", ""}, _
    {"RCH311", "TW", 2003, 1, 1, 2009, 12, 31, "Latoka Lake", "", ""}, _
    {"RCH315", "DOX", 2003, 1, 1, 2009, 12, 31, "Victoria Lake", "", ""}, _
    {"RCH315", "TP", 2003, 1, 1, 2009, 12, 31, "Victoria Lake", "", ""}, _
    {"RCH315", "TSS", 2003, 1, 1, 2009, 12, 31, "Victoria Lake", "", ""}, _
    {"RCH315", "TW", 2003, 1, 1, 2009, 12, 31, "Victoria Lake", "", ""}, _
    {"RCH316", "DOX", 2003, 1, 1, 2009, 12, 31, "Geneva Lake", "", ""}, _
    {"RCH316", "TP", 2003, 1, 1, 2009, 12, 31, "Geneva Lake", "", ""}, _
    {"RCH316", "TW", 2003, 1, 1, 2009, 12, 31, "Geneva Lake", "", ""}, _
    {"RCH317", "DOX", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH317", "NO3", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH317", "PO4", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH317", "TP", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH317", "TSS", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH317", "TW", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH318", "DOX", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH318", "NO3", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH318", "PO4", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH318", "TP", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH318", "TSS", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH318", "TW", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH319", "DOX", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH319", "NO3", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH319", "PO4", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH319", "TP", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH319", "TSS", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH319", "TW", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH320", "TP", 2003, 1, 1, 2009, 12, 31, "Le Homme Dieu L", "", ""}, _
    {"RCH320", "TW", 2003, 1, 1, 2009, 12, 31, "Le Homme Dieu L", "", ""}, _
    {"RCH321", "DOX", 2003, 1, 1, 2009, 12, 31, "Carlos Lake", "Darling Lake", ""}, _
    {"RCH321", "ORC", 2003, 1, 1, 2009, 12, 31, "Carlos Lake", "", ""}, _
    {"RCH321", "TP", 2003, 1, 1, 2009, 12, 31, "Carlos Lake", "Darling Lake", ""}, _
    {"RCH321", "TSS", 2003, 1, 1, 2009, 12, 31, "Carlos Lake", "", ""}, _
    {"RCH321", "TW", 2003, 1, 1, 2009, 12, 31, "Carlos Lake", "Darling Lake", ""}, _
    {"RCH322", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "", ""}, _
    {"RCH322", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3", "LONG PRAIRIE R, 1/2 MI N OF CARLOS", ""}, _
    {"RCH326", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH326", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH329", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "", ""}, _
    {"RCH329", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14", "LONG PRAIRIE R ON BR AT CR 90", ""}, _
    {"RCH331", "DOX", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "NO3", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "PO4", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "TAM", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "TP", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "TSS", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH331", "TW", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH332", "DOX", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "NO3", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "PO4", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "TAM", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "TP", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "TSS", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH332", "TW", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH333", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH333", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "DOX", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "NO3", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "PO4", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "TAM", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "TP", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "TSS", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH336", "TW", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH338", "DOX", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "NO3", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "PO4", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "TAM", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "TP", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "TSS", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH338", "TW", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH339", "ORC", 2003, 1, 1, 2009, 12, 31, "05245295", "", ""}, _
    {"RCH339", "TORN", 2003, 1, 1, 2009, 12, 31, "05245295", "", ""}, _
    {"RCH339", "DOX", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "NO3", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "PO4", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TAM", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TP", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TSS", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TW", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH341", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "ORC", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH341", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "H14034001", ""}, _
    {"RCH342", "DOX", 2003, 1, 1, 2009, 12, 31, "Shamineau Lake", "", ""}, _
    {"RCH342", "TP", 2003, 1, 1, 2009, 12, 31, "Shamineau Lake", "", ""}, _
    {"RCH342", "TW", 2003, 1, 1, 2009, 12, 31, "Shamineau Lake", "", ""}, _
    {"RCH344", "TP", 2003, 1, 1, 2009, 12, 31, "Alexander Lake", "", ""}, _
    {"RCH345", "TP", 2003, 1, 1, 2009, 12, 31, "Fish Trap Lake", "", ""}, _
    {"RCH345", "TW", 2003, 1, 1, 2009, 12, 31, "Fish Trap Lake", "", ""}, _
    {"RCH347", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "ORC", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH347", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH400", "DOX", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "NO3", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "PO4", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "TAM", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "TP", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "TSS", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH400", "TW", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH501", "TP", 2003, 1, 1, 2009, 12, 31, "Bad Medicine Lk", "", ""}, _
    {"RCH502", "TP", 2003, 1, 1, 2009, 12, 31, "Big Basswood Lk", "", ""}, _
    {"RCH502", "TW", 2003, 1, 1, 2009, 12, 31, "Big Basswood Lk", "", ""}, _
    {"RCH506", "TP", 2003, 1, 1, 2009, 12, 31, "Two Inlets Lake", "", ""}, _
    {"RCH506", "TW", 2003, 1, 1, 2009, 12, 31, "Two Inlets Lake", "", ""}, _
    {"RCH508", "DOX", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH508", "ORC", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH508", "TP", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH508", "TSS", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH508", "TW", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH509", "TP", 2003, 1, 1, 2009, 12, 31, "Eagle Lake", "", ""}, _
    {"RCH509", "TW", 2003, 1, 1, 2009, 12, 31, "Eagle Lake", "", ""}, _
    {"RCH510", "TP", 2003, 1, 1, 2009, 12, 31, "Potato Lake", "", ""}, _
    {"RCH510", "TW", 2003, 1, 1, 2009, 12, 31, "Potato Lake", "", ""}, _
    {"RCH511", "DOX", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "NO3", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "ORC", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "PO4", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "TAM", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "TP", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "TSS", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH511", "TW", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH512", "DOX", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "NO3", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "PO4", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "TAM", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "TP", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "TSS", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "", ""}, _
    {"RCH512", "TW", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "FISHHOOK R AT 3RD ST E IN PARK RAPIDS", ""}, _
    {"RCH514", "DOX", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH514", "NO3", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH514", "TAM", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH514", "TP", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH514", "TSS", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH514", "TW", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH515", "DOX", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH515", "NO3", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH515", "PO4", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "", ""}, _
    {"RCH515", "TAM", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH515", "TP", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH515", "TSS", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH515", "TW", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH516", "TW", 2003, 1, 1, 2009, 12, 31, "Straight R downstream of USGS gage", "", ""}, _
    {"RCH517", "DOX", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH517", "NO3", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH517", "TAM", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH517", "TP", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH517", "TSS", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH517", "TW", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH518", "DOX", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "", ""}, _
    {"RCH518", "NO3", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "", ""}, _
    {"RCH518", "TAM", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "", ""}, _
    {"RCH518", "TP", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "", ""}, _
    {"RCH518", "TSS", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "", ""}, _
    {"RCH518", "TW", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake", "FISHHOOK R AT MN-87", ""}, _
    {"RCH520", "DOX", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH520", "ORC", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH520", "TP", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH520", "TSS", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH520", "TW", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH521", "NO3", 2003, 1, 1, 2009, 12, 31, "05243200", "", ""}, _
    {"RCH521", "ORC", 2003, 1, 1, 2009, 12, 31, "05243200", "", ""}, _
    {"RCH521", "PO4", 2003, 1, 1, 2009, 12, 31, "05243200", "", ""}, _
    {"RCH521", "TORN", 2003, 1, 1, 2009, 12, 31, "05243200", "", ""}, _
    {"RCH521", "DOX", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "05243200", "Shell R at inlet to Blueberry Lake"}, _
    {"RCH521", "NO3", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "", ""}, _
    {"RCH521", "TAM", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "05243200", ""}, _
    {"RCH521", "TP", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "05243200", ""}, _
    {"RCH521", "TSS", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "05243200", ""}, _
    {"RCH521", "TW", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71", "05243200", "Shell R at inlet to Blueberry Lake"}, _
    {"RCH522", "DOX", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH522", "NO3", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH522", "TAM", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH522", "TP", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH522", "TSS", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH522", "TW", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH523", "DOX", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH523", "NO3", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH523", "TAM", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH523", "TP", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH523", "TSS", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH523", "TW", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH524", "DOX", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "Blueberry R at inlet to Blueberry Lake", ""}, _
    {"RCH524", "NO3", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "", ""}, _
    {"RCH524", "TAM", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "", ""}, _
    {"RCH524", "TP", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "", ""}, _
    {"RCH524", "TSS", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "", ""}, _
    {"RCH524", "TW", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "Blueberry 2 blocks W of US-71", "Blueberry R at inlet to Blueberry Lake"}, _
    {"RCH525", "DOX", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "Blueberry Lake", ""}, _
    {"RCH525", "NO3", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "", ""}, _
    {"RCH525", "TAM", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "", ""}, _
    {"RCH525", "TP", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "Blueberry Lake", ""}, _
    {"RCH525", "TSS", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "Blueberry Lake", ""}, _
    {"RCH525", "TW", 2003, 1, 1, 2009, 12, 31, "Shell R at Blueberry Br Rd", "Blueberry Lake", ""}, _
    {"RCH526", "DOX", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH526", "NO3", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH526", "TAM", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH526", "TP", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH526", "TSS", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH526", "TW", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 21", "Fishhook River below Long Lake", ""}, _
    {"RCH527", "DOX", 2003, 1, 1, 2009, 12, 31, "Lower Twin Lake", "", ""}, _
    {"RCH527", "TP", 2003, 1, 1, 2009, 12, 31, "Lower Twin Lake", "", ""}, _
    {"RCH527", "TSS", 2003, 1, 1, 2009, 12, 31, "Lower Twin Lake", "", ""}, _
    {"RCH527", "TW", 2003, 1, 1, 2009, 12, 31, "Lower Twin Lake", "", ""}, _
    {"RCH529", "DOX", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH529", "NO3", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH529", "TAM", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH529", "TP", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH529", "TSS", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH529", "TW", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH530", "DOX", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "NO3", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "TAM", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "TP", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "TSS", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "TW", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH530", "PO4", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 23", "", ""}, _
    {"RCH531", "DOX", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk Mid", "", ""}, _
    {"RCH531", "NO3", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk Mid", "", ""}, _
    {"RCH531", "TP", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk Mid", "Mantrap Lk East", ""}, _
    {"RCH531", "TSS", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk Mid", "", ""}, _
    {"RCH531", "TW", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk Mid", "Mantrap Lk East", ""}, _
    {"RCH532", "TP", 2003, 1, 1, 2009, 12, 31, "Lower Bottle Lk", "", ""}, _
    {"RCH532", "TW", 2003, 1, 1, 2009, 12, 31, "Lower Bottle Lk", "", ""}, _
    {"RCH533", "TP", 2003, 1, 1, 2009, 12, 31, "Big Sand Lake", "", ""}, _
    {"RCH533", "TW", 2003, 1, 1, 2009, 12, 31, "Big Sand Lake", "", ""}, _
    {"RCH534", "TP", 2003, 1, 1, 2009, 12, 31, "Little Sand Lk", "", ""}, _
    {"RCH534", "TW", 2003, 1, 1, 2009, 12, 31, "Little Sand Lk", "", ""}, _
    {"RCH535", "TP", 2003, 1, 1, 2009, 12, 31, "Belle Taine Lk", "", ""}, _
    {"RCH535", "TW", 2003, 1, 1, 2009, 12, 31, "Belle Taine Lk", "", ""}, _
    {"RCH536", "TP", 2003, 1, 1, 2009, 12, 31, "11th Crow Wing", "", ""}, _
    {"RCH536", "TW", 2003, 1, 1, 2009, 12, 31, "11th Crow Wing", "", ""}, _
    {"RCH537", "DOX", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH537", "ORC", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH537", "TP", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH537", "TSS", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH537", "TW", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH544", "TP", 2003, 1, 1, 2009, 12, 31, "3rd Crow Wing", "4th Crow Wing", ""}, _
    {"RCH544", "TW", 2003, 1, 1, 2009, 12, 31, "3rd Crow Wing", "4th Crow Wing", ""}, _
    {"RCH546", "TP", 2003, 1, 1, 2009, 12, 31, "1st Crow Wing", "", ""}, _
    {"RCH546", "TW", 2003, 1, 1, 2009, 12, 31, "1st Crow Wing", "", ""}, _
    {"RCH588", "DOX", 2003, 1, 1, 2009, 12, 31, "Margaret Lk", "", ""}, _
    {"RCH588", "TP", 2003, 1, 1, 2009, 12, 31, "Margaret Lk", "", ""}, _
    {"RCH588", "TSS", 2003, 1, 1, 2009, 12, 31, "Margaret Lk", "", ""}, _
    {"RCH588", "TW", 2003, 1, 1, 2009, 12, 31, "Margaret Lk", "", ""}, _
    {"RCH591", "DOX", 2003, 1, 1, 2009, 12, 31, "Sibley Lake", "", ""}, _
    {"RCH591", "TP", 2003, 1, 1, 2009, 12, 31, "Sibley Lake", "", ""}, _
    {"RCH591", "TSS", 2003, 1, 1, 2009, 12, 31, "Sibley Lake", "", ""}, _
    {"RCH591", "TW", 2003, 1, 1, 2009, 12, 31, "Sibley Lake", "", ""}, _
    {"RCH592", "DOX", 2003, 1, 1, 2009, 12, 31, "Lower Cullen Lk", "", ""}, _
    {"RCH592", "TP", 2003, 1, 1, 2009, 12, 31, "Lower Cullen Lk", "", ""}, _
    {"RCH592", "TW", 2003, 1, 1, 2009, 12, 31, "Lower Cullen Lk", "", ""}, _
    {"RCH596", "TP", 2003, 1, 1, 2009, 12, 31, "Upper Gull Lk", "", ""}, _
    {"RCH596", "TW", 2003, 1, 1, 2009, 12, 31, "Upper Gull Lk", "", ""}, _
    {"RCH597", "TP", 2003, 1, 1, 2009, 12, 31, "Edward Lake", "", ""}, _
    {"RCH597", "TW", 2003, 1, 1, 2009, 12, 31, "Edward Lake", "", ""}, _
    {"RCH598", "TP", 2003, 1, 1, 2009, 12, 31, "North Long Lake", "", ""}, _
    {"RCH598", "TW", 2003, 1, 1, 2009, 12, 31, "North Long Lake", "", ""}, _
    {"RCH600", "DOX", 2003, 1, 1, 2009, 12, 31, "Gull Lake", "", ""}, _
    {"RCH600", "TP", 2003, 1, 1, 2009, 12, 31, "Gull Lake", "", ""}, _
    {"RCH600", "TSS", 2003, 1, 1, 2009, 12, 31, "Gull Lake", "", ""}, _
    {"RCH600", "TW", 2003, 1, 1, 2009, 12, 31, "Gull Lake", "", ""}, _
    {"RCH104", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "West Leaf Lake", "", ""}, _
    {"RCH106", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "East Leaf Lake", "", ""}, _
    {"RCH121", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "05244385", "", ""}, _
    {"RCH122", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH123", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Wolf Lake", "", ""}, _
    {"RCH125", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "05244409", "", ""}, _
    {"RCH131", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "H13058001", "", ""}, _
    {"RCH133", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH301", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Andrew Lake", "", ""}, _
    {"RCH302", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Mary Lake", "", ""}, _
    {"RCH304", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Lobster Lk East", "Lobster Lake West", ""}, _
    {"RCH308", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Irene Lake", "", ""}, _
    {"RCH309", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Miltona Lake", "", ""}, _
    {"RCH310", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Ida Lake", "", ""}, _
    {"RCH311", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Latoka Lake", "", ""}, _
    {"RCH315", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Victoria Lake", "", ""}, _
    {"RCH316", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Geneva Lake", "", ""}, _
    {"RCH317", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Winona Lake", "", ""}, _
    {"RCH318", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Agnes Lake", "", ""}, _
    {"RCH319", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Henry Lake", "", ""}, _
    {"RCH320", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Le Homme Dieu L", "", ""}, _
    {"RCH321", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Carlos Lake, Darling Lake", "", ""}, _
    {"RCH322", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "", ""}, _
    {"RCH324", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH326", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", "", ""}, _
    {"RCH327", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH329", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14", ""}, _
    {"RCH331", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE", "", ""}, _
    {"RCH332", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21", "", ""}, _
    {"RCH333", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH335", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH336", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", "", ""}, _
    {"RCH338", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST", "", ""}, _
    {"RCH339", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "05245295", "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH341", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", "H14034001", ""}, _
    {"RCH342", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Shamineau Lake", "", ""}, _
    {"RCH344", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Alexander Lake", "", ""}, _
    {"RCH345", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Fish Trap Lake", "", ""}, _
    {"RCH347", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", "", ""}, _
    {"RCH400", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", "", ""}, _
    {"RCH501", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Bad Medicine Lk", "", ""}, _
    {"RCH502", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Big Basswood Lk", "", ""}, _
    {"RCH506", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Two Inlets Lake", "", ""}, _
    {"RCH508", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Island Lake", "", ""}, _
    {"RCH509", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Eagle Lake", "", ""}, _
    {"RCH510", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Potato Lake", "", ""}, _
    {"RCH511", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Portage Lake", "", ""}, _
    {"RCH512", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Fishhook Lake", "FISHHOOK R AT 3RD ST E IN PARK RAPIDS", ""}, _
    {"RCH514", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Straight Lake", "", ""}, _
    {"RCH515", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "STRAIGHT R AT US HWY 71", "Straight River nr USGS gage", ""}, _
    {"RCH516", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Straight R downstream of USGS gage", "", ""}, _
    {"RCH517", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Long Lake", "", ""}, _
    {"RCH518", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "FISHHOOK R AT MN-87", "Fishhook River above Long Lake", ""}, _
    {"RCH520", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Shell Lake", "", ""}, _
    {"RCH521", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "05243200", "Shell R at inlet to Blueberry Lake", "Shell R at US-71"}, _
    {"RCH522", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th", "", ""}, _
    {"RCH523", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156", "", ""}, _
    {"RCH524", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71", "Blueberry R at inlet to Blueberry Lake", "Blueberry 2 blocks W of US-71"}, _
    {"RCH525", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Blueberry Lake", "Shell R at Blueberry Br Rd", ""}, _
    {"RCH526", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Fishhook River below Long Lake", "Shell R at CSAH 21", ""}, _
    {"RCH527", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Lower Twin Lake", "", ""}, _
    {"RCH529", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Stocking Lake", "", ""}, _
    {"RCH530", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Shell R at CR 13", "Shell R at CSAH 23", "Shell R at CSAH 24"}, _
    {"RCH531", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Mantrap Lk East", "Mantrap Lk Mid", ""}, _
    {"RCH532", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Lower Bottle Lk", "", ""}, _
    {"RCH533", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Big Sand Lake", "", ""}, _
    {"RCH534", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Little Sand Lk", "", ""}, _
    {"RCH535", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Belle Taine Lk", "", ""}, _
    {"RCH536", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "11th Crow Wing", "", ""}, _
    {"RCH537", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "9th Crow Wing", "", ""}, _
    {"RCH538", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH544", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "3rd Crow Wing", "4th Crow Wing", ""}, _
    {"RCH546", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "1st Crow Wing", "", ""}, _
    {"RCH557", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""}, _
    {"RCH582", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Placid Lake", "", ""}, _
    {"RCH588", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Margaret Lk", "", ""}, _
    {"RCH591", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Sibley Lake", "", ""}, _
    {"RCH592", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Lower Cullen Lk", "", ""}, _
    {"RCH593", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Hubert Lake", "", ""}, _
    {"RCH596", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Upper Gull Lk", "", ""}, _
    {"RCH597", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Edward Lake", "", ""}, _
    {"RCH598", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "North Long Lake", "", ""}, _
    {"RCH599", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Round Lake", "", ""}, _
    {"RCH600", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "Gull Lake", "", ""}, _
    {"RCH700", "CHLOROA", 2003, 1, 1, 2009, 12, 31, "", "", ""} _
    }

    Private pGraphs_MPCAWQ_Val(,) As Object = { _
    {"RCH104", "TP", 1995, 1, 1, 2002, 12, 31, "West Leaf Lake", ""}, _
    {"RCH106", "TP", 1995, 1, 1, 2002, 12, 31, "East Leaf Lake", ""}, _
    {"RCH123", "DOX", 1995, 1, 1, 2002, 12, 31, "Wolf Lake", ""}, _
    {"RCH123", "TP", 1995, 1, 1, 2002, 12, 31, "Wolf Lake", ""}, _
    {"RCH123", "TSS", 1995, 1, 1, 2002, 12, 31, "Wolf Lake", ""}, _
    {"RCH123", "TW", 1995, 1, 1, 2002, 12, 31, "Wolf Lake", ""}, _
    {"RCH301", "TP", 1995, 1, 1, 2002, 12, 31, "Andrew Lake", ""}, _
    {"RCH302", "TP", 1995, 1, 1, 2002, 12, 31, "Mary Lake", ""}, _
    {"RCH304", "TP", 1995, 1, 1, 2002, 12, 31, "Lobster Lk East", ""}, _
    {"RCH308", "DOX", 1995, 1, 1, 2002, 12, 31, "Irene Lake", ""}, _
    {"RCH308", "NO3", 1995, 1, 1, 2002, 12, 31, "Irene Lake", ""}, _
    {"RCH308", "TP", 1995, 1, 1, 2002, 12, 31, "Irene Lake", ""}, _
    {"RCH308", "TSS", 1995, 1, 1, 2002, 12, 31, "Irene Lake", ""}, _
    {"RCH308", "TW", 1995, 1, 1, 2002, 12, 31, "Irene Lake", ""}, _
    {"RCH309", "TP", 1995, 1, 1, 2002, 12, 31, "Miltona Lake", ""}, _
    {"RCH310", "DOX", 1995, 1, 1, 2002, 12, 31, "Ida Lake", ""}, _
    {"RCH310", "TP", 1995, 1, 1, 2002, 12, 31, "Ida Lake", ""}, _
    {"RCH310", "TSS", 1995, 1, 1, 2002, 12, 31, "Ida Lake", ""}, _
    {"RCH310", "TW", 1995, 1, 1, 2002, 12, 31, "Ida Lake", ""}, _
    {"RCH311", "DOX", 1995, 1, 1, 2002, 12, 31, "Latoka Lake", ""}, _
    {"RCH311", "TP", 1995, 1, 1, 2002, 12, 31, "Latoka Lake", ""}, _
    {"RCH311", "TW", 1995, 1, 1, 2002, 12, 31, "Latoka Lake", ""}, _
    {"RCH315", "DOX", 1995, 1, 1, 2002, 12, 31, "Victoria Lake", ""}, _
    {"RCH315", "TP", 1995, 1, 1, 2002, 12, 31, "Victoria Lake", ""}, _
    {"RCH315", "TW", 1995, 1, 1, 2002, 12, 31, "Victoria Lake", ""}, _
    {"RCH316", "DOX", 1995, 1, 1, 2002, 12, 31, "Geneva Lake", ""}, _
    {"RCH316", "TP", 1995, 1, 1, 2002, 12, 31, "Geneva Lake", ""}, _
    {"RCH316", "TW", 1995, 1, 1, 2002, 12, 31, "Geneva Lake", ""}, _
    {"RCH317", "DOX", 1995, 1, 1, 2002, 12, 31, "Winona Lake", ""}, _
    {"RCH317", "TP", 1995, 1, 1, 2002, 12, 31, "Winona Lake", ""}, _
    {"RCH317", "TW", 1995, 1, 1, 2002, 12, 31, "Winona Lake", ""}, _
    {"RCH318", "DOX", 1995, 1, 1, 2002, 12, 31, "Agnes Lake", ""}, _
    {"RCH318", "TP", 1995, 1, 1, 2002, 12, 31, "Agnes Lake", ""}, _
    {"RCH318", "TW", 1995, 1, 1, 2002, 12, 31, "Agnes Lake", ""}, _
    {"RCH319", "DOX", 1995, 1, 1, 2002, 12, 31, "Henry Lake", ""}, _
    {"RCH319", "TP", 1995, 1, 1, 2002, 12, 31, "Henry Lake", ""}, _
    {"RCH319", "TW", 1995, 1, 1, 2002, 12, 31, "Henry Lake", ""}, _
    {"RCH320", "DOX", 1995, 1, 1, 2002, 12, 31, "Le Homme Dieu L", ""}, _
    {"RCH320", "TP", 1995, 1, 1, 2002, 12, 31, "Le Homme Dieu L", ""}, _
    {"RCH320", "TSS", 1995, 1, 1, 2002, 12, 31, "Le Homme Dieu L", ""}, _
    {"RCH320", "TW", 1995, 1, 1, 2002, 12, 31, "Le Homme Dieu L", ""}, _
    {"RCH321", "DOX", 1995, 1, 1, 2002, 12, 31, "Carlos Lake", "Darling Lake"}, _
    {"RCH321", "TP", 1995, 1, 1, 2002, 12, 31, "Carlos Lake", "Darling Lake"}, _
    {"RCH321", "TW", 1995, 1, 1, 2002, 12, 31, "Carlos Lake", "Darling Lake"}, _
    {"RCH326", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH326", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE", ""}, _
    {"RCH329", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH329", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 90", "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH332", "DOX", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "NO3", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "PO4", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "TAM", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "TP", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "TSS", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH332", "TW", 1995, 1, 1, 2002, 12, 31, "EAGLE CR ON BRG AT CSAH 21", ""}, _
    {"RCH333", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH333", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "DOX", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "NO3", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "PO4", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "TAM", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "TP", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "TSS", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH336", "TW", 1995, 1, 1, 2002, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD", ""}, _
    {"RCH339", "DOX", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "NO3", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "PO4", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TAM", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TP", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TSS", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH339", "TW", 1995, 1, 1, 2002, 12, 31, "MORAN CR ON BR AT 255TH AVE", ""}, _
    {"RCH341", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "ORC", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH341", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT CR 65", ""}, _
    {"RCH345", "DOX", 1995, 1, 1, 2002, 12, 31, "Fish Trap Lake", ""}, _
    {"RCH345", "TP", 1995, 1, 1, 2002, 12, 31, "Fish Trap Lake", ""}, _
    {"RCH345", "TSS", 1995, 1, 1, 2002, 12, 31, "Fish Trap Lake", ""}, _
    {"RCH345", "TW", 1995, 1, 1, 2002, 12, 31, "Fish Trap Lake", ""}, _
    {"RCH347", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH347", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10", ""}, _
    {"RCH400", "DOX", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "NO3", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "PO4", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "TAM", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "TP", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "TSS", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH400", "TW", 1995, 1, 1, 2002, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR", ""}, _
    {"RCH501", "TP", 1995, 1, 1, 2002, 12, 31, "Bad Medicine Lk", ""}, _
    {"RCH506", "TP", 1995, 1, 1, 2002, 12, 31, "Two Inlets Lake", ""}, _
    {"RCH509", "TP", 1995, 1, 1, 2002, 12, 31, "Eagle Lake", ""}, _
    {"RCH510", "TP", 1995, 1, 1, 2002, 12, 31, "Potato Lake", ""}, _
    {"RCH511", "TP", 1995, 1, 1, 2002, 12, 31, "Portage Lake", ""}, _
    {"RCH512", "TP", 1995, 1, 1, 2002, 12, 31, "Fishhook Lake", ""}, _
    {"RCH514", "TP", 1995, 1, 1, 2002, 12, 31, "Straight Lake", ""}, _
    {"RCH515", "DOX", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "NO3", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "PO4", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "TAM", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "TP", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "TSS", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH515", "TW", 1995, 1, 1, 2002, 12, 31, "STRAIGHT R AT US HWY 71", ""}, _
    {"RCH517", "DOX", 1995, 1, 1, 2002, 12, 31, "Long Lake", ""}, _
    {"RCH517", "NO3", 1995, 1, 1, 2002, 12, 31, "Long Lake", ""}, _
    {"RCH517", "TP", 1995, 1, 1, 2002, 12, 31, "Long Lake", ""}, _
    {"RCH517", "TSS", 1995, 1, 1, 2002, 12, 31, "Long Lake", ""}, _
    {"RCH517", "TW", 1995, 1, 1, 2002, 12, 31, "Long Lake", ""}, _
    {"RCH520", "TP", 1995, 1, 1, 2002, 12, 31, "Shell Lake", ""}, _
    {"RCH520", "TW", 1995, 1, 1, 2002, 12, 31, "Shell Lake", ""}, _
    {"RCH525", "TP", 1995, 1, 1, 2002, 12, 31, "Blueberry Lake", ""}, _
    {"RCH526", "TW", 1995, 1, 1, 2002, 12, 31, "Fishhook River below Long Lake", ""}, _
    {"RCH527", "TP", 1995, 1, 1, 2002, 12, 31, "Lower Twin Lake", ""}, _
    {"RCH531", "TP", 1995, 1, 1, 2002, 12, 31, "Mantrap Lk East", ""}, _
    {"RCH532", "TP", 1995, 1, 1, 2002, 12, 31, "Lower Bottle Lk", ""}, _
    {"RCH533", "TP", 1995, 1, 1, 2002, 12, 31, "Big Sand Lake", ""}, _
    {"RCH534", "TP", 1995, 1, 1, 2002, 12, 31, "Little Sand Lk", ""}, _
    {"RCH535", "TP", 1995, 1, 1, 2002, 12, 31, "Belle Taine Lk", ""}, _
    {"RCH537", "TP", 1995, 1, 1, 2002, 12, 31, "9th Crow Wing", ""}, _
    {"RCH544", "TP", 1995, 1, 1, 2002, 12, 31, "3rd Crow Wing", "4th Crow Wing"}, _
    {"RCH546", "TP", 1995, 1, 1, 2002, 12, 31, "1st Crow Wing", ""}, _
    {"RCH582", "DOX", 1995, 1, 1, 2002, 12, 31, "Placid Lake", ""}, _
    {"RCH582", "TP", 1995, 1, 1, 2002, 12, 31, "Placid Lake", ""}, _
    {"RCH582", "TSS", 1995, 1, 1, 2002, 12, 31, "Placid Lake", ""}, _
    {"RCH582", "TW", 1995, 1, 1, 2002, 12, 31, "Placid Lake", ""}, _
    {"RCH588", "NO3", 1995, 1, 1, 2002, 12, 31, "Margaret Lk", ""}, _
    {"RCH588", "PO4", 1995, 1, 1, 2002, 12, 31, "Margaret Lk", ""}, _
    {"RCH588", "TAM", 1995, 1, 1, 2002, 12, 31, "Margaret Lk", ""}, _
    {"RCH588", "TP", 1995, 1, 1, 2002, 12, 31, "Margaret Lk", ""}, _
    {"RCH592", "DOX", 1995, 1, 1, 2002, 12, 31, "Lower Cullen Lk", ""}, _
    {"RCH592", "TP", 1995, 1, 1, 2002, 12, 31, "Lower Cullen Lk", ""}, _
    {"RCH592", "TSS", 1995, 1, 1, 2002, 12, 31, "Lower Cullen Lk", ""}, _
    {"RCH592", "TW", 1995, 1, 1, 2002, 12, 31, "Lower Cullen Lk", ""}, _
    {"RCH593", "TP", 1995, 1, 1, 2002, 12, 31, "Hubert Lake", ""}, _
    {"RCH593", "TSS", 1995, 1, 1, 2002, 12, 31, "Hubert Lake", ""}, _
    {"RCH593", "TW", 1995, 1, 1, 2002, 12, 31, "Hubert Lake", ""}, _
    {"RCH598", "DOX", 1995, 1, 1, 2002, 12, 31, "North Long Lake", ""}, _
    {"RCH598", "PO4", 1995, 1, 1, 2002, 12, 31, "North Long Lake", ""}, _
    {"RCH598", "TP", 1995, 1, 1, 2002, 12, 31, "North Long Lake", ""}, _
    {"RCH598", "TSS", 1995, 1, 1, 2002, 12, 31, "North Long Lake", ""}, _
    {"RCH598", "TW", 1995, 1, 1, 2002, 12, 31, "North Long Lake", ""}, _
    {"RCH599", "DOX", 1995, 1, 1, 2002, 12, 31, "Round Lake", ""}, _
    {"RCH599", "NO3", 1995, 1, 1, 2002, 12, 31, "Round Lake", ""}, _
    {"RCH599", "TP", 1995, 1, 1, 2002, 12, 31, "Round Lake", ""}, _
    {"RCH599", "TSS", 1995, 1, 1, 2002, 12, 31, "Round Lake", ""}, _
    {"RCH599", "TW", 1995, 1, 1, 2002, 12, 31, "Round Lake", ""} _
    }

    Private pGraphSpecMPCAStage(,) As Object = { _
    {"RCH309", "STAGE", 2003, 1, 1, 2009, 10, 31, "Miltona Lake"}, _
    {"RCH310", "STAGE", 2003, 1, 1, 2009, 11, 30, "Ida Lake"}, _
    {"RCH311", "STAGE", 2003, 1, 1, 2009, 10, 31, "Latoka Lake"}, _
    {"RCH315", "STAGE", 2003, 1, 1, 2009, 11, 30, "Victoria Lake"}, _
    {"RCH316", "STAGE", 2003, 1, 1, 2009, 11, 30, "Geneva Lake"}, _
    {"RCH317", "STAGE", 2003, 1, 1, 2008, 8, 31, "Winona Lake"}, _
    {"RCH320", "STAGE", 2003, 1, 1, 2009, 6, 30, "Le Homme Dieu L"}, _
    {"RCH321", "STAGE", 2003, 1, 1, 2009, 6, 30, "Carlos Lake"}, _
    {"RCH322", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON CSAH 3"}, _
    {"RCH326", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R. W OF LONG PRAIRIE"}, _
    {"RCH329", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CSAH 14"}, _
    {"RCH331", "STAGE", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON 175TH AVE"}, _
    {"RCH332", "STAGE", 2003, 1, 1, 2009, 12, 31, "EAGLE CR ON BRG AT CSAH 21"}, _
    {"RCH333", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT OAK RIDGE RD"}, _
    {"RCH336", "STAGE", 2003, 1, 1, 2009, 12, 31, "TURTLE CK ON BR AT OAK RIDGE RD"}, _
    {"RCH338", "STAGE", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON 464TH ST"}, _
    {"RCH339", "STAGE", 2003, 1, 1, 2009, 12, 31, "MORAN CR ON BR AT 255TH AVE"}, _
    {"RCH341", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 65"}, _
    {"RCH347", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R BRIDGE ON US-10"}, _
    {"RCH509", "STAGE", 2003, 1, 1, 2009, 12, 31, "Eagle Lake"}, _
    {"RCH510", "STAGE", 2003, 1, 1, 2009, 12, 31, "Potato Lake"}, _
    {"RCH511", "STAGE", 2003, 1, 1, 2009, 11, 30, "Portage Lake"}, _
    {"RCH512", "STAGE", 2003, 1, 1, 2009, 12, 31, "Fish Hook Lake"}, _
    {"RCH514", "STAGE", 2003, 1, 1, 2009, 12, 31, "Straight Lake"}, _
    {"RCH517", "STAGE", 2003, 1, 1, 2009, 12, 31, "Long Lake"}, _
    {"RCH518", "STAGE", 2003, 1, 1, 2009, 12, 31, "Fishhook River above Long Lake"}, _
    {"RCH521", "STAGE", 2003, 1, 1, 2009, 12, 31, "Shell R at US-71"}, _
    {"RCH522", "STAGE", 2003, 1, 1, 2009, 12, 31, "Blueberry R at 384th"}, _
    {"RCH523", "STAGE", 2003, 1, 1, 2009, 12, 31, "Kettle R at CR-156"}, _
    {"RCH524", "STAGE", 2003, 1, 1, 2009, 12, 31, "Blueberry R at US-71"}, _
    {"RCH525", "STAGE", 2003, 1, 1, 2009, 12, 31, "Blueberry Lake"}, _
    {"RCH526", "STAGE", 2003, 1, 1, 2009, 12, 31, "Fishhook River below Long Lake"}, _
    {"RCH529", "STAGE", 2003, 1, 1, 2009, 12, 31, "Stocking Lake"}, _
    {"RCH530", "STAGE", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 24"}, _
    {"RCH535", "STAGE", 2003, 1, 1, 2009, 11, 30, "Belle Taine Lk"}, _
    {"RCH538", "STAGE", 2003, 1, 1, 2009, 10, 31, "8th Crow Wing"}, _
    {"RCH544", "STAGE", 2003, 1, 1, 2009, 10, 31, "4th Crow Wing"}, _
    {"RCH546", "STAGE", 2003, 1, 1, 2009, 6, 30, "1st Crow Wing"}, _
    {"RCH600", "STAGE", 2003, 1, 1, 2009, 12, 31, "Gull Lake"} _
    }
    '{"RCH329", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT RIVERSIDE DR"}, _
    '{"RCH329", "STAGE", 2003, 1, 1, 2009, 12, 31, "LONG PRAIRIE R ON BR AT CR 90"}, _
    '{"RCH530", "STAGE", 2003, 1, 1, 2009, 12, 31, "Shell R at CSAH 23"}, _

    Private Sub Initialize()
        Dim lTestName As String = "MPCAWQ"
        'Dim lTestName As String = "MPCAValWQ"
        Select Case lTestName
            Case "MPCAWQ"
                pWorkingDirectory = "M:\modelout\"
                pbasename = "MPCAWQ"
                pWQGraphSpecification = pGraphSpecMPCAWQ
                pObservedWQBaseFileName = "T:\MPCA\WQ_data.dbf"
            Case "MPCAValWQ"
                pWorkingDirectory = "C:\BASINS\modelout\"
                pbasename = "MPCAValWQ"
                pWQGraphSpecification = pGraphs_MPCAWQ_Val
                pObservedWQBaseFileName = "T:\MPCA\WQ_data.dbf"
            Case "MPCAStage"
                pWorkingDirectory = "M:\modelout\"
                pbasename = "MPCAStage"
                pWQGraphSpecification = pGraphSpecMPCAStage
                pObservedWQBaseFileName = "T:\MPCA\Lake_Stage.dbf"
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        Dim pLastIndex As Integer = pWQGraphSpecification.GetUpperBound(0)
        ChDriveDir(pWorkingDirectory)
        Select Case pbasename
            Case "MPCAWQ"
                'Dim lUCIName As String = pWorkingDirectory & "RECal03\RECal03.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                'lUCIName = pWorkingDirectory & "LPCal03\LPCal03.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                'lUCIName = pWorkingDirectory & "CWCal03\CWCal03.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                Dim pOutputWDMFileName1 As String = pWorkingDirectory & "CWCal03\CWCal03.wdm"
                Dim wdmfileinfo As System.IO.FileInfo = New System.IO.FileInfo(pOutputWDMFileName1)
                Dim LastWriteTime As Date = wdmfileinfo.LastWriteTime
                Dim lRunmade As String = wdmfileinfo.LastWriteTime.ToString
                wdmfileinfo = New System.IO.FileInfo(pWorkingDirectory & "LPCal03\LPCal03.wdm")
                If LastWriteTime < wdmfileinfo.LastWriteTime Then
                    lRunmade = wdmfileinfo.LastWriteTime.ToString
                    LastWriteTime = wdmfileinfo.LastWriteTime
                End If

                wdmfileinfo = New System.IO.FileInfo(pWorkingDirectory & "RECal03\RECal03.wdm")
                If LastWriteTime < wdmfileinfo.LastWriteTime Then
                    lRunmade = wdmfileinfo.LastWriteTime.ToString
                End If

                foldername = pWorkingDirectory & "WQGraphs_" & pbasename & "_" & Format(Year(lRunmade), "00") & "_" & Format(Month(lRunmade), "00") & _
                                          "_" & Format(Day(lRunmade), "00") & "_" & Format(Hour(lRunmade), "00") & "_" & Format(Minute(lRunmade), "00")

                System.IO.Directory.CreateDirectory(foldername)
                System.IO.File.Copy(pWorkingDirectory & "CWCal03\CWCal03.uci", foldername & "\CWCal03.uci")
                System.IO.File.Copy(pWorkingDirectory & "LPCal03\LPCal03.uci", foldername & "\LPCal03.uci")
                System.IO.File.Copy(pWorkingDirectory & "RECal03\RECal03.uci", foldername & "\RECal03.uci")

                Dim lDataSource2 As New atcDataSourceBasinsObsWQ
                lDataSource2.Open(pObservedWQBaseFileName)
                For lGraphIndex As Integer = 0 To pLastIndex
                    pLeftAxisLogScale = False
                    pTimeseriesConstituent = pWQGraphSpecification(lGraphIndex, 1)
                    Select Case pTimeseriesConstituent
                        Case "TW"
                            pLeftYAxisLabel = "Water Temperature (" & ChrW(186) & "F)"
                            'AxisUpper = 100
                        Case "CHLOROA"
                            pLeftYAxisLabel = "Phytoplankton as Chlorophyll A (" & ChrW(181) & "g/l)"
                        Case "DOX"
                            pLeftYAxisLabel = "Dissolved Oxygen (mg/l)"
                            'AxisUpper = 18
                        Case "TSS"
                            pLeftYAxisLabel = "Total Suspended Solids (mg/l)"
                            'AxisUpper = 1500
                            pLeftAxisLogScale = True
                        Case "ORC"
                            pLeftYAxisLabel = "Organic Carbon (mg/l)"
                            'AxisUpper = 10
                        Case "TORN"
                            pLeftYAxisLabel = "Total Organic Nitrogen (mg/l)"
                            'AxisUpper = 2
                        Case "TAM"
                            pLeftYAxisLabel = "Ammonia (mg/l as N)"
                            'AxisUpper = 1
                        Case "NO3"
                            pLeftYAxisLabel = "Nitrate + Nitrite (mg/l as N)"
                            'AxisUpper = 10
                        Case "TP"
                            pLeftYAxisLabel = "Total Phosphorus (mg/l)"
                            'AxisUpper = 2
                        Case "PO4"
                            pLeftYAxisLabel = "Orthophosphate (mg/l as P)"
                            'AxisUpper = 2
                    End Select
                    Dim lSDate(5) As Integer : lSDate(0) = pWQGraphSpecification(lGraphIndex, 2) : lSDate(1) _
                                               = pWQGraphSpecification(lGraphIndex, 3) : lSDate(2) = pWQGraphSpecification(lGraphIndex, 4)
                    Dim lSDateJ As Double = Date2J(lSDate)
                    Dim lEDate(5) As Integer : lEDate(0) = pWQGraphSpecification(lGraphIndex, 5) : lEDate(1) = _
                                                pWQGraphSpecification(lGraphIndex, 6) : lEDate(2) = pWQGraphSpecification(lGraphIndex, 7)
                    Dim lEdatej As Double = Date2J(lEDate)
                    pbasename = pTimeseriesConstituent & "_" & pWQGraphSpecification(lGraphIndex, 0) & "_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 3), "00") & _
                                Format(pWQGraphSpecification(lGraphIndex, 4), "00") & Right(pWQGraphSpecification(lGraphIndex, 2), 2) & "_to_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 6), "00") & Format(pWQGraphSpecification(lGraphIndex, 7), "00") & _
                                Right(pWQGraphSpecification(lGraphIndex, 5), 2)
                    Dim lTimeseriesGroup As New atcTimeseriesGroup
                    Dim ReachNumber As Integer = Mid(pWQGraphSpecification(lGraphIndex, 0), 4, 6)
                    'get timeseries 1
                    Dim lDataSource1 As New atcDataSourceWDM

                    Dim lTser1 As atcTimeseries = Nothing

                    Select Case ReachNumber
                        Case 100 To 200
                            pOutputWDMFileName1 = pWorkingDirectory & "RECal03\RECal03.wdm"
                        Case 300 To 400
                            pOutputWDMFileName1 = pWorkingDirectory & "LPCal03\LPCal03.wdm"
                        Case 500 To 700
                            pOutputWDMFileName1 = pWorkingDirectory & "CWCal03\CWCal03.wdm"
                    End Select

                    If lDataSource1.Open(pOutputWDMFileName1) Then
                        location = pWQGraphSpecification(lGraphIndex, 0)
                        constituent = pWQGraphSpecification(lGraphIndex, 1)

                        'Simulated Flow aggregated to daily
                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", "FLOW")(0)
                        lTser1.Attributes.SetValue("YAxis", "Aux")
                        lTser1.Attributes.SetValue("Point", False)
                        'If Not pTimeseriesConstituent = "TSS" Then
                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        'End If
                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        pLeftAuxAxisLabel = "Flow (cfs)"
                        lTser1 = Nothing

                        'Observed Flow
                        
                        lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                                                FindData("Constituent", "FLOW")(0)
                        Select Case location
                            Case "RCH400"
                                lTser1 = lDataSource1.DataSets.FindData("Location", "05245100"). _
                                                FindData("Constituent", "FLOW")(0)
                            Case "RCH515"
                                lTser1 = lDataSource1.DataSets.FindData("Location", "05243725"). _
                                                FindData("Constituent", "FLOW")(0)
                            Case "RCH557"
                                lTser1 = lDataSource1.DataSets.FindData("Location", "05244000"). _
                                                FindData("Constituent", "FLOW")(0)
                            Case "RCH700"
                                lTser1 = lDataSource1.DataSets.FindData("Location", "05247500"). _
                                                FindData("Constituent", "FLOW")(0)
                        End Select
                        If lTser1 IsNot Nothing Then
                            lTser1.Attributes.SetValue("YAxis", "Aux")
                            lTser1.Attributes.SetValue("Point", "True")
                            lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        End If
                        lTser1 = Nothing
                        If (pWQGraphSpecification(lGraphIndex, 9) <> "") Then
                            lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 9)). _
                                                FindData("Constituent", "FLOW")(0)
                            If lTser1 IsNot Nothing Then
                                lTser1.Attributes.SetValue("YAxis", "Aux")
                                lTser1.Attributes.SetValue("Point", "True")
                                lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                            End If
                            lTser1 = Nothing
                        End If

                        'Simulated constituent data

                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        lTser1.Attributes.SetValue("YAxis", "Left")
                        lTser1.Attributes.SetValue("Point", False)
                        'lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))

                        'Simulated constituent data, aggregated to daily
                        'If Not pTimeseriesConstituent = "TSS" Then
                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        'End If
                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        lTser1 = Nothing

                        'Observed constituent data
                        lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                                                    FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        If lTser1 IsNot Nothing Then
                            lTser1.Attributes.SetValue("YAxis", "Left")
                            lTser1.Attributes.SetValue("Point", "True")
                            If lTser1.Attributes.Count > 60 Then
                                lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTser1 = SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing)
                            If lTser1.numValues < 1 Then Continue For
                            lTimeseriesGroup.Add(lTser1)
                            lTser1 = Nothing
                        End If

                        If (pWQGraphSpecification(lGraphIndex, 9) <> "") Then
                            lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 9)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                            If lTser1 IsNot Nothing Then
                                lTser1.Attributes.SetValue("YAxis", "Left")
                                lTser1.Attributes.SetValue("Point", "True")
                                If lTser1.Attributes.Count > 60 Then
                                    lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                                End If
                                lTser1 = SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing)
                                If lTser1.numValues < 1 Then Continue For
                                lTimeseriesGroup.Add(lTser1)
                                lTser1 = Nothing
                            End If

                            If (pWQGraphSpecification(lGraphIndex, 10) <> "") Then
                                lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 10)). _
                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                                If lTser1 IsNot Nothing Then
                                    lTser1.Attributes.SetValue("YAxis", "Left")
                                    lTser1.Attributes.SetValue("Point", "True")
                                    If lTser1.Attributes.Count > 60 Then
                                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                                    End If
                                    lTser1 = SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing)
                                    If lTser1.numValues < 1 Then Continue For
                                    lTimeseriesGroup.Add(lTser1)
                                    lTser1 = Nothing
                                End If
                            End If

                        End If

                    Else
                        Logger.Msg("Unable to Open " & pOutputWDMFileName1)
                    End If
                    GraphTimeseriesBatch(lTimeseriesGroup)
                    If lDataSource1 IsNot Nothing Then lDataSource1.Clear()
                Next
                If lDataSource2 IsNot Nothing Then lDataSource2.Clear()

            Case "MPCAValWQ"
                Dim lUCIName As String = pWorkingDirectory & "REVal\REVal.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                'lUCIName = pWorkingDirectory & "LPVal\LPVal.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                'lUCIName = pWorkingDirectory & "CWVal\CWVal.uci"
                'Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & lUCIName)
                'Logger.Flush()
                'LaunchProgram(pHSPFExe, pWorkingDirectory, lUCIName)
                'Logger.Dbg("HSPFRun Finished")

                Dim pOutputWDMFileName1 As String = pWorkingDirectory & "CWVal\CWVal.wdm"
                Dim wdmfileinfo As System.IO.FileInfo = New System.IO.FileInfo(pOutputWDMFileName1)
                Dim LastWriteTime As Date = wdmfileinfo.LastWriteTime
                Dim lRunmade As String = wdmfileinfo.LastWriteTime.ToString
                wdmfileinfo = New System.IO.FileInfo(pWorkingDirectory & "LPVal\LPVal.wdm")
                If LastWriteTime < wdmfileinfo.LastWriteTime Then
                    lRunmade = wdmfileinfo.LastWriteTime.ToString
                    LastWriteTime = wdmfileinfo.LastWriteTime
                End If

                wdmfileinfo = New System.IO.FileInfo(pWorkingDirectory & "REVal\REVal.wdm")
                If LastWriteTime < wdmfileinfo.LastWriteTime Then
                    lRunmade = wdmfileinfo.LastWriteTime.ToString
                End If

                foldername = pWorkingDirectory & "WQGraphs_" & pbasename & "_" & Format(Year(lRunmade), "00") & "_" & Format(Month(lRunmade), "00") & _
                                          "_" & Format(Day(lRunmade), "00") & "_" & Format(Hour(lRunmade), "00") & "_" & Format(Minute(lRunmade), "00")
                System.IO.Directory.CreateDirectory(foldername)
                System.IO.File.Copy(pWorkingDirectory & "CWVal\CWVal.uci", foldername & "\CWVal.uci")
                System.IO.File.Copy(pWorkingDirectory & "LPVal\LPVal.uci", foldername & "\LPVal.uci")
                System.IO.File.Copy(pWorkingDirectory & "REVal\REVal.uci", foldername & "\REVal.uci")
                Dim lDataSource2 As New atcDataSourceBasinsObsWQ
                lDataSource2.Open(pObservedWQBaseFileName)
                For lGraphIndex As Integer = 0 To pLastIndex
                    pLeftAxisLogScale = False
                    pTimeseriesConstituent = pWQGraphSpecification(lGraphIndex, 1)
                    Select Case pTimeseriesConstituent
                        Case "TW"
                            pLeftYAxisLabel = "Water Temperature (" & ChrW(186) & "F)"
                            AxisUpper = 100
                        Case "CHLOROA"
                            pLeftYAxisLabel = "Phytoplankton as Chlorophyll A (" & ChrW(181) & "g/l)"
                        Case "DOX"
                            pLeftYAxisLabel = "Dissolved Oxygen (mg/l)"
                            AxisUpper = 18
                        Case "TSS"
                            pLeftYAxisLabel = "Total Suspended Solids (mg/l)"
                            AxisUpper = 1500
                            pLeftAxisLogScale = True
                        Case "ORC"
                            pLeftYAxisLabel = "Organic Carbon (mg/l)"
                            AxisUpper = 10
                        Case "TORN"
                            pLeftYAxisLabel = "Total Organic Nitrogen (mg/l)"
                            AxisUpper = 2
                        Case "TAM"
                            pLeftYAxisLabel = "Ammonia (mg/l as N)"
                            AxisUpper = 1
                        Case "NO3"
                            pLeftYAxisLabel = "Nitrate + Nitrite (mg/l as N)"
                            AxisUpper = 10
                        Case "TP"
                            pLeftYAxisLabel = "Total Phosphorus (mg/l)"
                            AxisUpper = 2
                        Case "PO4"
                            pLeftYAxisLabel = "Orthophosphate (mg/l as P)"
                            AxisUpper = 2
                    End Select
                    Dim lSDate(5) As Integer : lSDate(0) = pWQGraphSpecification(lGraphIndex, 2) : lSDate(1) _
                                               = pWQGraphSpecification(lGraphIndex, 3) : lSDate(2) = pWQGraphSpecification(lGraphIndex, 4)
                    Dim lSDateJ As Double = Date2J(lSDate)
                    Dim lEDate(5) As Integer : lEDate(0) = pWQGraphSpecification(lGraphIndex, 5) : lEDate(1) = _
                                                pWQGraphSpecification(lGraphIndex, 6) : lEDate(2) = pWQGraphSpecification(lGraphIndex, 7)
                    Dim lEdatej As Double = Date2J(lEDate)
                    pbasename = pTimeseriesConstituent & "_" & pWQGraphSpecification(lGraphIndex, 0) & "_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 3), "00") & Format(pWQGraphSpecification(lGraphIndex, 4), "00") & _
                                Right(pWQGraphSpecification(lGraphIndex, 2), 2) & "_to_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 6), "00") & Format(pWQGraphSpecification(lGraphIndex, 7), "00") & _
                                Right(pWQGraphSpecification(lGraphIndex, 5), 2)
                    Dim lTimeseriesGroup As New atcTimeseriesGroup
                    Dim ReachNumber As Integer = Mid(pWQGraphSpecification(lGraphIndex, 0), 4, 6)
                    'get timeseries 1
                    Dim lDataSource1 As New atcDataSourceWDM

                    Dim lTser1 As atcTimeseries = Nothing

                    Select Case ReachNumber
                        Case 100 To 200
                            pOutputWDMFileName1 = pWorkingDirectory & "REVal\REVal.wdm"
                        Case 300 To 400
                            pOutputWDMFileName1 = pWorkingDirectory & "LPVal\LPVal.wdm"
                        Case 500 To 700
                            pOutputWDMFileName1 = pWorkingDirectory & "CWVal\CWVal.wdm"
                    End Select

                    If lDataSource1.Open(pOutputWDMFileName1) Then
                        location = pWQGraphSpecification(lGraphIndex, 0)
                        constituent = pWQGraphSpecification(lGraphIndex, 1)

                        'Simulated Flow aggregated to daily
                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", "FLOW")(0)
                        lTser1.Attributes.SetValue("YAxis", "Aux")
                        lTser1.Attributes.SetValue("Point", False)
                        'If Not pTimeseriesConstituent = "TSS" Then
                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        'End If

                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        pLeftAuxAxisLabel = "Flow (cfs)"
                        lTser1 = Nothing

                        'Observed Flow
                        lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                                                FindData("Constituent", "FLOW")(0)
                        If lTser1 IsNot Nothing Then
                            lTser1.Attributes.SetValue("YAxis", "Aux")
                            lTser1.Attributes.SetValue("Point", "True")
                            lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        End If
                        lTser1 = Nothing
                        If (pWQGraphSpecification(lGraphIndex, 9) <> "") Then
                            lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 9)). _
                                                FindData("Constituent", "FLOW")(0)
                            If lTser1 IsNot Nothing Then
                                lTser1.Attributes.SetValue("YAxis", "Aux")
                                lTser1.Attributes.SetValue("Point", "True")
                                lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                            End If
                            lTser1 = Nothing
                        End If

                        'Simulated constituent data
                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        lTser1.Attributes.SetValue("YAxis", "Left")
                        lTser1.Attributes.SetValue("Point", False)
                        'lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))

                        'Simulated constituent data, aggregated to daily
                        'If Not pTimeseriesConstituent = "TSS" Then
                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        'End If

                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        lTser1 = Nothing

                        'Observed constituent data
                        lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        lTser1.Attributes.SetValue("YAxis", "Left")
                        lTser1.Attributes.SetValue("Point", "True")
                        If lTser1.Attributes.Count > 60 Then
                            lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        End If
                        lTser1 = SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing)
                        If lTser1.numValues < 1 Then Continue For
                        lTimeseriesGroup.Add(lTser1)
                        lTser1 = Nothing

                        If (pWQGraphSpecification(lGraphIndex, 9) <> "") Then
                            lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 9)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                            lTser1.Attributes.SetValue("YAxis", "Left")
                            lTser1.Attributes.SetValue("Point", "True")
                            If lTser1.Attributes.Count > 60 Then
                                lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            End If
                            lTser1 = SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing)
                            If lTser1.numValues < 1 Then Continue For
                            lTimeseriesGroup.Add(lTser1)
                            lTser1 = Nothing

                        End If

                    Else
                        Logger.Msg("Unable to Open " & pOutputWDMFileName1)
                    End If
                    GraphTimeseriesBatch(lTimeseriesGroup)
                    If lDataSource1 IsNot Nothing Then lDataSource1.Clear()
                Next
                If lDataSource2 IsNot Nothing Then lDataSource2.Clear()

            Case "MPCAStage"
                Dim pOutputWDMFileName1 As String = pWorkingDirectory & "CWCal03\CWCal03.wdm"
                Dim wdmfileinfo As System.IO.FileInfo = New System.IO.FileInfo(pOutputWDMFileName1)
                Dim lRunmade As String = wdmfileinfo.LastWriteTime.ToString
                foldername = pWorkingDirectory & pbasename & "_Graphs_" & Format(Year(lRunmade), "00") & "_" & Format(Month(lRunmade), "00") & _
                                          "_" & Format(Day(lRunmade), "00") & "_" & Format(Hour(lRunmade), "00") & "_" & Format(Minute(lRunmade), "00")
                Dim lDataSource2 As New atcDataSourceBasinsObsWQ
                lDataSource2.Open(pObservedWQBaseFileName)
                Dim largest As Double = Double.MinValue
                Dim smallest As Double = Double.MaxValue
                Dim ValueUpper As Double
                Dim ValueLower As Double
                
                For lGraphIndex As Integer = 0 To pLastIndex
                    pTimeseriesConstituent = pWQGraphSpecification(lGraphIndex, 1)
                    pLeftYAxisLabel = "Stage (ft)"
                    Dim lSDate(5) As Integer : lSDate(0) = pWQGraphSpecification(lGraphIndex, 2) : lSDate(1) _
                                   = pWQGraphSpecification(lGraphIndex, 3) : lSDate(2) = pWQGraphSpecification(lGraphIndex, 4)
                    Dim lSDateJ As Double = Date2J(lSDate)
                    Dim lEDate(5) As Integer : lEDate(0) = pWQGraphSpecification(lGraphIndex, 5) : lEDate(1) = _
                                                pWQGraphSpecification(lGraphIndex, 6) : lEDate(2) = pWQGraphSpecification(lGraphIndex, 7)
                    Dim lEdatej As Double = Date2J(lEDate)
                    pbasename = pWQGraphSpecification(lGraphIndex, 0) & "_" & pTimeseriesConstituent & "_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 3), "00") & _
                                Format(pWQGraphSpecification(lGraphIndex, 4), "00") & Right(pWQGraphSpecification(lGraphIndex, 2), 2) & "_to_" & _
                                Format(pWQGraphSpecification(lGraphIndex, 6), "00") & Format(pWQGraphSpecification(lGraphIndex, 7), "00") & _
                                Right(pWQGraphSpecification(lGraphIndex, 5), 2)
                    Dim lTimeseriesGroup As New atcTimeseriesGroup
                    Dim ReachNumber As Integer = Mid(pWQGraphSpecification(lGraphIndex, 0), 4, 6)
                    'get timeseries 1
                    Dim lDataSource1 As New atcDataSourceWDM

                    Dim lTser1 As atcTimeseries = Nothing

                    Select Case ReachNumber
                        Case 100 To 200
                            pOutputWDMFileName1 = pWorkingDirectory & "RECal03\RECal03.wdm"
                        Case 300 To 400
                            pOutputWDMFileName1 = pWorkingDirectory & "LPCal03\LPCal03.wdm"
                        Case 500 To 700
                            pOutputWDMFileName1 = pWorkingDirectory & "CWCal03\CWCal03.wdm"
                    End Select

                    If lDataSource1.Open(pOutputWDMFileName1) Then
                        'Simulated Flow aggregated to daily
                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", "FLOW")(0)
                        lTser1.Attributes.SetValue("YAxis", "Aux")
                        lTser1.Attributes.SetValue("Point", False)
                        lTser1 = Aggregate(lTser1, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        pLeftAuxAxisLabel = "Flow (cfs)"
                        lTser1 = Nothing

                        'Observed constituent data
                        lTser1 = lDataSource2.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 8)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        lTser1.Attributes.SetValue("YAxis", "Left")
                        lTser1.Attributes.SetValue("Point", "True")

                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        lTser1 = Nothing

                        'Simulated constituent data
                        lTser1 = lDataSource1.DataSets.FindData("Location", pWQGraphSpecification(lGraphIndex, 0)). _
                                                FindData("Constituent", pWQGraphSpecification(lGraphIndex, 1))(0)
                        lTser1.Attributes.SetValue("YAxis", "Left")
                        lTser1.Attributes.SetValue("Point", False)
                        lTimeseriesGroup.Add(SubsetByDate(lTser1, lSDateJ, lEdatej, Nothing))
                        Array.Sort(lTser1.Values)
                        ValueUpper = lTser1.Values.GetValue(lTser1.Values.GetUpperBound(0))
                        ValueLower = lTser1.Values.GetValue(0)
                        AxisUpper = ValueUpper + 10
                        AxisLower = Math.Max(0, Convert.ToInt32(ValueLower) - 10)
                        lTser1 = Nothing

                    Else
                        Logger.Msg("Unable to Open " & pOutputWDMFileName1)
                    End If
                    GraphTimeseriesBatchSTAGE(lTimeseriesGroup)
                    If lDataSource1 IsNot Nothing Then lDataSource1.Clear()
                Next
                If lDataSource2 IsNot Nothing Then lDataSource2.Clear()
        End Select

    End Sub

    <CLSCompliant(False)> _
    Public Sub FormatPanes(ByVal aZgc As ZedGraph.ZedGraphControl)

        For Each lPane As ZedGraph.GraphPane In aZgc.MasterPane.PaneList()
            FormatPanePrintable(lPane)
        Next
    End Sub

    Public Sub FormatPanePrintable(ByVal aPane As ZedGraph.GraphPane, Optional ByVal aMainPane As Boolean = True)
        FormatPaneWithDefaults(aPane)
        With aPane


            If aMainPane Then

                For Each lCurve As ZedGraph.LineItem In .CurveList
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH33", "Sally Br. (RCH 33, Area: 9.8 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH27", "Little Pine Knot Cr. (RCH 27, Area: 4.4 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH10 ", "Little Juniper Cr. (RCH 10, Area: 16.2 sq. mi. )")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH17", "Upatoi Cr. (RCH 17, Area: 187 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH31 ", "Sally Br. (RCH 31, Area: 4.6 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH49", "Hollis Cr. (RCH 49, Area: 5.2 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH35", "Bonham Cr. (RCH 35, Area: 4.9 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH46", "Upatoi Cr./MCB Bridge (RCH 46, Area: 339.7 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH614", "Upper Upatoi Cr. (RCH 614, Area: 150.3 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH639", "Upper Randall Cr. (RCH 639)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH45", "Randall Cr. (RCH 45, Area: 51.6 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH626", "Pine Knot Cr. (RCH 626)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH30", "Pine Knot Cr. (RCH 30)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH662", "Ochille Cr. (RCH 662)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH666", "Tiger Cr. (RCH 666)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH72", "Upatoi Cr./WWTP (RCH 72)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH311", "Oswichee Cr. (RCH 311)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH108", "Bull Cr. (RCH 108)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("K11W", "K11W (Area: 0.89 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("D13", "D13 (Area: 0.29 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("K13", "K13 (Area: 1.28 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("K11E", "K11E (Area: 1.42 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("F4", "F4 (Area: 0.83 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("F3", "F3 (Area: 0.48 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("D12", "D12 (Area: 0.81 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("F1E", "F1E (Area: 0.28 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("K20", "K20 (Area: 0.13 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("D6", "D6 (Area: 1.18 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("F1W", "F1W (Area: 0.39 sq. mi.)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("O13", "O13 (Area: 0.82 sq. mi.)")
                Next
            Else
                For Each lCurve As ZedGraph.LineItem In .CurveList
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH33", "Sally Br. (RCH 33)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH27", "Little Pine Knot Cr. (RCH 27)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH10 ", "Little Juniper Cr. (RCH 10 )")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH17", "Upatoi Cr. (RCH 17)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH31 ", "Sally Br. (RCH 31)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH49", "Hollis Cr. (RCH 49)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH35", "Bonham Cr. (RCH 35)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH46", "Upatoi Cr./MCB Bridge (RCH 46)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH614", "Upper Upatoi Cr. (RCH 614)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH639", "Upper Randall Cr. (RCH 639)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH45", "Randall Cr. (RCH 45)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH626", "Pine Knot Cr. (RCH 626)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH30", "Pine Knot Cr. (RCH 30)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH662", "Ochille Cr. (RCH 662)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH666", "Tiger Cr. (RCH 666)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH72", "Upatoi Cr./WWTP (RCH 72)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH311", "Oswichee Cr. (RCH 311)")
                    lCurve.Label.Text = lCurve.Label.Text.Replace("RCH108", "Bull Cr. (RCH 108)")

                Next

            End If
            With .Legend
                .Position = LegendPos.Float
                .Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top) 'You can change the legend position here
                .FontSpec.Size = 13
                .FontSpec.Border.IsVisible = False
                .FontSpec.IsBold = True

            End With

            .XAxis.Scale.FontSpec.Size = 13
            .XAxis.Scale.FontSpec.IsBold = True
            .XAxis.Title.Text = ""
            .YAxis.Scale.FontSpec.Size = 13
            .YAxis.Scale.FontSpec.IsBold = True
            .YAxis.Title.FontSpec.Size = 18
            .Border.IsVisible = False

            For Each lCurve As ZedGraph.LineItem In .CurveList
                lCurve.Label.Text = lCurve.Label.Text.Replace("SIMQ", "Simulated Flow")
                lCurve.Label.Text = lCurve.Label.Text.Replace("SIMULATE", "Simulated")
                lCurve.Label.Text = lCurve.Label.Text.Replace("TW", "Water Temperature")
                lCurve.Label.Text = lCurve.Label.Text.Replace("DOX", "Dissolved Oxygen")
                lCurve.Label.Text = lCurve.Label.Text.Replace("PREC", "Precip")
            Next

        End With

    End Sub
    Sub GraphTimeseriesBatchTAU(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = foldername & "\" & pbasename
        Dim lZgc As ZedGraphControl = CreateZgc(, 1280, 1024)
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPaneMain As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem
        lCurve = lPaneMain.CurveList.Item(0)
        lCurve.Color = Drawing.Color.Red
        lCurve.Line.Width = 2
        System.IO.Directory.CreateDirectory(foldername)
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.Dispose()

    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcTimeseriesGroup)
        'Dim lOutFileName As String = "C:\BASINS41\modelout\MPCA\" & pbasename
        Dim lOutFileName As String = foldername & "\" & pbasename
        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPaneMain, lPaneAux As GraphPane
        Dim lCurveAux, lCurveMain As ZedGraph.LineItem
        'First Curve in Main and Auxiliary pane is simulated, and the second curve is the observed data.
        lPaneAux = lZgc.MasterPane.PaneList(0)
        lPaneMain = lZgc.MasterPane.PaneList(1)
        lPaneMain.Legend.Fill.Type = FillType.Solid
        lPaneMain.Legend.Fill.Color = Drawing.Color.LightGray
        lPaneAux.Legend.Fill.Type = FillType.Solid
        lPaneAux.Legend.Fill.Color = Drawing.Color.LightGray

        lCurveAux = lPaneAux.CurveList.Item(0) 'lCurveAux is simulated data
        lCurveAux.Line.StepType = StepType.NonStep
        lCurveAux.Color = Drawing.Color.Red
        lCurveAux.Line.Width = 1
        lPaneAux.YAxis.Title.Text = pLeftAuxAxisLabel

        If lPaneAux.CurveList.Count > 1 Then
            lCurveAux = lPaneAux.CurveList.Item(1) 'lCurveAux is observed data
            lCurveAux.Color = Drawing.Color.Blue
            If lCurveAux.Points.Count > 200 Then
                lCurveAux.Line.IsVisible = True
                lCurveAux.Symbol.Type = SymbolType.None
                lCurveAux.Line.Width = 1
                lCurveAux.Line.StepType = StepType.NonStep
            Else
                lCurveAux.Line.IsVisible = False
                lCurveAux.Symbol.Type = SymbolType.Circle
                lCurveAux.Symbol.Fill.IsVisible = True
                lCurveAux.Symbol.Size = 4
            End If
        End If

        If lPaneAux.CurveList.Count > 2 Then
            lCurveAux = lPaneAux.CurveList.Item(2) 'lCurveAux is observed data for the second location
            lCurveAux.Color = Drawing.Color.Green
            lCurveAux.Symbol.Type = SymbolType.Circle
            lCurveAux.Symbol.Fill.IsVisible = True
            If lCurveAux.Points.Count > 60 Then
                lCurveAux.Symbol.Size = 4
            Else
                lCurveAux.Symbol.Size = 7
            End If
        End If

        'lCurveMain = lPaneMain.CurveList.Item(0) ' lCurveMain is the hourly simulated data
        'lCurveMain.Color = Drawing.Color.Red
        'lCurveMain.Line.StepType = StepType.NonStep
        'lCurveMain.Line.Width = 2

        lCurveMain = lPaneMain.CurveList.Item(0) ' lCurveMain is the aggregated daily simulated data
        lCurveMain.Color = Drawing.Color.Red
        'lCurveMain.Line.StepType = StepType.NonStep
        lCurveMain.Line.Width = 1

        If lPaneMain.CurveList.Count > 1 Then
            lCurveMain = lPaneMain.CurveList.Item(1) 'lCurveMain is observed data
            lCurveMain.Color = Drawing.Color.Blue
            lCurveMain.Symbol.Type = SymbolType.Circle
            lCurveMain.Symbol.Fill.IsVisible = True
            lCurveMain.Symbol.Size = 7

            If lPaneMain.CurveList.Count > 2 Then
                lCurveMain = lPaneMain.CurveList.Item(2) 'lCurveMain is observed data at second station
                lCurveMain.Color = Drawing.Color.Green
                lCurveMain.Symbol.Type = SymbolType.Circle
                lCurveMain.Symbol.Fill.IsVisible = True
                lCurveMain.Symbol.Size = 7
                If lPaneMain.CurveList.Count > 3 Then
                    lCurveMain = lPaneMain.CurveList.Item(3) 'lCurveMain is observed data at third station
                    lCurveMain.Color = Drawing.Color.Purple
                    lCurveMain.Symbol.Type = SymbolType.Circle
                    lCurveMain.Symbol.Fill.IsVisible = True
                    lCurveMain.Symbol.Size = 7
                End If
            End If
        End If

        lPaneMain.YAxis.Scale.Min = 0

        If pLeftAxisLogScale Then
            lPaneMain.YAxis.Type = AxisType.Log
            lPaneMain.YAxis.Scale.Min = 0.1
        End If

        Dim lXlabel As String = lPaneMain.XAxis.Title.Text
        lPaneMain.YAxis.Title.Text = pLeftYAxisLabel
        If pLeftYAxisLabel.Contains("Total Suspended") AndAlso lPaneMain.YAxis.Scale.Max < 100 Then
            lPaneMain.YAxis.Scale.Max = 100

        End If

        'lPaneMain.YAxis.Scale.Max = AxisUpper
        lPaneMain.AxisChange()
        lPaneAux.YAxis.Scale.Min = 0
        lPaneAux.AxisChange()
        Try
            lZgc.SaveIn(lOutFileName & ".png")

        Catch ex As Exception
        End Try
        lZgc.Dispose()
    End Sub

    Sub GraphTimeseriesBatchSTAGE(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = foldername & "\" & pbasename
        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPaneMain, lPaneAux As GraphPane
        Dim lCurveMain, lCurveAux As ZedGraph.LineItem
        'First Curve in Main and Aux pane is simulated, and the second curve is the observed data.
        lPaneAux = lZgc.MasterPane.PaneList(0)
        lPaneMain = lZgc.MasterPane.PaneList(1)
        lPaneMain.Legend.Fill.Type = FillType.Solid
        lPaneMain.Legend.Fill.Color = Drawing.Color.LightGray
        lPaneAux.Legend.Fill.Type = FillType.Solid
        lPaneAux.Legend.Fill.Color = Drawing.Color.LightGray

        lCurveAux = lPaneAux.CurveList.Item(0) 'lCurveAux is simulated data
        lCurveAux.Line.StepType = StepType.NonStep
        lCurveAux.Color = Drawing.Color.Red
        lCurveAux.Line.Width = 2
        lPaneAux.YAxis.Title.Text = pLeftAuxAxisLabel

        lCurveMain = lPaneMain.CurveList.Item(1) ' lCurveMain is the hourly simulated data
        lCurveMain.Color = Drawing.Color.Red
        'lCurveMain.Line.StepType = StepType.NonStep
        lCurveMain.Line.Width = 2

        lCurveMain = lPaneMain.CurveList.Item(0) 'lCurveMain is observed data
        lCurveMain.Color = Drawing.Color.Blue
        lCurveMain.Symbol.Type = SymbolType.Circle
        lCurveMain.Symbol.Fill.IsVisible = True
        lCurveMain.Symbol.Size = 7

        Dim lXlabel As String = lPaneMain.XAxis.Title.Text
        lPaneMain.YAxis.Title.Text = pLeftYAxisLabel
        
        'lPaneMain.YAxis.Scale.Min = AxisLower
        'lPaneMain.YAxis.Scale.Max = AxisUpper
        lPaneMain.AxisChange()
        System.IO.Directory.CreateDirectory(foldername)
        lZgc.SaveIn(lOutFileName & ".png")

        lZgc.Dispose()
    End Sub

    Sub GraphScatterBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lOutFileName As String = pbasename & "_scat"
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphScatter(aDataGroup, lZgc)
        lGrapher.AddFitLine()
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        With lPane
            ScaleAxis(aDataGroup, .YAxis)
            lZgc.SaveIn(lOutFileName & ".png")
            lZgc.SaveIn(lOutFileName & ".emf")

            .YAxis.Type = ZedGraph.AxisType.Log
            ScaleAxis(aDataGroup, .YAxis)
            .XAxis.Type = ZedGraph.AxisType.Log
            .XAxis.Scale.Min = .YAxis.Scale.Min
            .XAxis.Scale.Max = .YAxis.Scale.Max
        End With
        lOutFileName = pbasename & "_scat_log"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphDurationBatch(ByVal aDataGroup As atcTimeseriesGroup)

        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
        Dim lGrapher As New clsGraphProbability(aDataGroup, lZgc)
        Dim lPane As GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lCurve As ZedGraph.LineItem
        lCurve = lPane.CurveList(0)
        Dim PlotConstituent As String = lCurve.Label.Text
        If lCurve.Label.Text.Contains("FLOW") Then
            lPane.YAxis.Title.Text = "Daily Simulated Flow (cfs)"
        ElseIf lCurve.Label.Text.Contains("SDEP") Then

            lPane.YAxis.Scale.MinAuto = False
            lPane.YAxis.Scale.Min = 1
            lPane.YAxis.Scale.Max = 40
            lPane.XAxis.Scale.Max = 90
            lPane.YAxis.Title.Text = "Snow Depth (inches)"
            lPane.YAxis.Type = AxisType.Linear
            lPane.Legend.FontSpec.Size = 12
            lPane.AxisChange()
        End If
        Dim lOutFileName As String = foldername & "\" & pbasename & "_dur"
        System.IO.Directory.CreateDirectory(foldername)
        lZgc.SaveIn(lOutFileName & ".png")
        'lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
        Dim lNewDisplay As New atcDataDisplay
        Dim lType As System.Type = lNewDisplay.GetType()
        Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(lType)
        lNewDisplay = lAssembly.CreateInstance(lType.FullName)
        lNewDisplay.Show(aDataGroup)

    End Sub

    Sub GraphResidualBatch(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphResidual(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_residual"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Sub GraphCumDifBatch(ByVal aDataGroup As atcTimeseriesGroup, ByVal pBaseName As String)
        Dim lZgc As ZedGraphControl = CreateZgc()
        Dim lGrapher As New clsGraphCumulativeDifference(aDataGroup, lZgc)
        Dim lOutFileName As String = pBaseName & "_cumDif"
        lZgc.SaveIn(lOutFileName & ".png")
        lZgc.SaveIn(lOutFileName & ".emf")
    End Sub
End Module
