Attribute VB_Name = "modDownloadGlobal"
Option Explicit

Public Const cAppName = "DataDownload" 'Used for registry settings
Public gAppPath As String    '"C:\BASINS" (no trailing slash)
Public gCacheDir As String   '"C:\BASINS\cache\"
Public gProjectXML As String 'Projection instructions in XML, can't be stored in status for formatting reasons

