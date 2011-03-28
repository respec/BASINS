Imports atcData
Imports atcUtility

''' <summary>
''' Base class for models to be run by CAT
''' </summary>
''' <remarks></remarks>
Public Interface clsCatModel

    Event BaseModelSet(ByVal aBaseModel As String)

    Property BaseModel() As String

    Property SimulationStart() As Date

    Property SimulationEnd() As Date

    Function ModelRun(ByVal aNewModelName As String, _
                         ByVal aModifiedData As atcTimeseriesGroup, _
                         ByVal aPreparedInput As String, _
                         ByVal aRunModel As Boolean, _
                         ByVal aShowProgress As Boolean, _
                         ByVal aKeepRunning As Boolean) As atcCollection 'of atcDataSource

    Property XML() As String

End Interface
