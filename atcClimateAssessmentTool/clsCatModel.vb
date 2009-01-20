Imports atcData
Imports atcUtility

''' <summary>
''' Base class for models to be run by CAT
''' </summary>
''' <remarks></remarks>
Public Interface clsCatModel

    Event BaseScenarioSet(ByVal aBaseScenario As String)

    Property BaseScenario() As String

    Function ScenarioRun(ByVal aNewScenarioName As String, _
                         ByVal aModifiedData As atcTimeseriesGroup, _
                         ByVal aPreparedInput As String, _
                         ByVal aRunModel As Boolean, _
                         ByVal aShowProgress As Boolean, _
                         ByVal aKeepRunning As Boolean) As atcCollection 'of atcDataSource

    Property XML() As String

End Interface
