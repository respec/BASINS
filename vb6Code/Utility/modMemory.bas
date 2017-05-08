Attribute VB_Name = "modMemory"
Option Explicit

Private Type MEMORYSTATUS
    dwLength As Long
    dwMemoryLoad As Long
    dwTotalPhys As Long
    dwAvailPhys As Long
    dwTotalPageFile As Long
    dwAvailPageFile As Long
    dwTotalVirtual As Long
    dwAvailVirtual As Long
End Type

Private pUdtMemStatus As MEMORYSTATUS

Private Declare Sub GlobalMemoryStatus Lib _
"kernel32" (lpBuffer As MEMORYSTATUS)

Public Function AvailablePhysicalMemory() As Double
'Return Value in Megabytes
    Dim dblAns As Double
    GlobalMemoryStatus pUdtMemStatus
    dblAns = pUdtMemStatus.dwAvailPhys
    AvailablePhysicalMemory = BytesToMegabytes(dblAns)
    
End Function

Public Function TotalPhysicalMemory() As Double
'Return Value in Megabytes
    Dim dblAns As Double
    GlobalMemoryStatus pUdtMemStatus
    dblAns = pUdtMemStatus.dwTotalPhys
    TotalPhysicalMemory = BytesToMegabytes(dblAns)
End Function

Public Function PercentMemoryFree() As Double

   PercentMemoryFree = Format(AvailableMemory / TotalMemory * _
   100, "0#")
End Function

Public Function AvailablePageFile() As Double
'Return Value in Megabytes
    Dim dblAns As Double
    GlobalMemoryStatus pUdtMemStatus
    dblAns = pUdtMemStatus.dwAvailPageFile
    AvailablePageFile = BytesToMegabytes(dblAns)
End Function

Public Function PageFileSize() As Double
'Return Value in Megabytes
    Dim dblAns As Double
    GlobalMemoryStatus pUdtMemStatus
    dblAns = pUdtMemStatus.dwTotalPageFile
    PageFileSize = BytesToMegabytes(dblAns)

End Function

Public Function AvailableMemory() As Double
'Return Value in Megabytes
     AvailableMemory = AvailablePhysicalMemory + AvailablePageFile
End Function

Public Function TotalMemory() As Double
'Return Value in Megabytes
    TotalMemory = PageFileSize + TotalPhysicalMemory
End Function

Private Function BytesToMegabytes(Bytes As Double) As Double
 
  Dim dblAns As Double
  dblAns = (Bytes / 1024) / 1024
  BytesToMegabytes = Format(dblAns, "###,###,##0.00")
  
End Function
