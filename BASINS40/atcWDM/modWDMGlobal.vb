Option Strict Off
Option Explicit On

Imports atcData
Imports ATCutility

Module WDMGlobal

    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Private pUnitsDef As atcAttributeDefinition
    Private pUnitsDefEditable As atcAttributeDefinition

    ' used by clsTserWDM and clsTSerSWATDbf (in clsTSerDBF.cls)
    Public Function UnitsAttributeDefinition(Optional ByRef Editable As Boolean = False) As atcAttributeDefinition
        If pUnitsDef Is Nothing Then 'Need to create pUnitsDef and pUnitsDefEditable
            pUnitsDef = New atcAttributeDefinition
            pUnitsDefEditable = New atcAttributeDefinition

            pUnitsDef.Name = "Units"
            pUnitsDefEditable.Name = "Units"

            pUnitsDef.TypeString = "String"
            pUnitsDefEditable.TypeString = "String"

            pUnitsDef.Editable = False
            pUnitsDefEditable.Editable = True

            pUnitsDefEditable.ValidList = GetAllUnitsInCategory("all")
            'pUnitsDef.ValidValues = pUnitsDefEditable.ValidValues
        End If

        If Editable Then
            Return pUnitsDefEditable
        Else
            Return pUnitsDef
        End If
    End Function

End Module
