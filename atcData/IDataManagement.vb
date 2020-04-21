Public Interface IDataManagement

    Enum ECleanUpMode
        ALL = 0
        ATTRIBUTES
        CALCULATED
    End Enum

    Property CleanUpMode() As ECleanUpMode

End Interface
