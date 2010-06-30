Module Globals_v2
    'Declare this as global so that it can be accessed throughout the plug-in project.
    'The variable is initialized in the plugin_Initialize event.
    '3/16/2005 - dpa - Includes a global declaration of the mwTauDem.mwTaudemInterfacesClass
    Public g_MapWin As MapWindow.Interfaces.IMapWin
    Public g_handle As Integer
    'Public g_Taudem As New mwTauDem.mwTaudemInterfaceClass
    Public g_BaseDEM As String
    Public g_AutoForm As New frmAutomatic_v2
    'Public g_TaudemLib As New TKTAUDEMLib.TauDEM
    Public g_StatusBar As System.Windows.Forms.StatusBarPanel
    Public g_MaxFileSize As Long = 62914560 '60 meg
    Public g_DrawingMask As Boolean = False
    Public g_SelectingMask As Boolean = False
    Public g_DrawingOutletsOrInlets As Boolean = False
    Public g_DrawingInlets As Boolean = False
    Public g_DrawingReservoir As Boolean = False
    Public g_DrawingPointSource As Boolean = False
    Public g_SelectingOutlets As Boolean = False
    Public frmDrawSelect As New frmDrawSelectShape_v2
    Public frmLoadOutput As New frmLoadOutputs
    Public frmLoadDelinOutput As New frmLoadDelinOutputs

    'Flags for running delin steps efficiently. Some not used anymore
    Public preProcHasRan As Boolean = False
    Public threshDelinHasRan As Boolean = False
    Public snapHasRan As Boolean
    Public outletHasRan As Boolean
    Public runningAll As Boolean

    'Variables for previous state 
    Public lastDem As String
    Public lastOutlet As String
    Public lastStream As String
    Public lastMask As String
    Public lastThresh As String
    Public lastConvUnit As Integer = -1
    Public currSelectPath As String
    Public currDrawPath As String

    ' Actual type objects
    Public tdbFileList As tdbFileTypes_v2
    Public tdbChoiceList As tdbChoices_v2
End Module
