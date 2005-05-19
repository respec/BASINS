Option Strict Off
Option Explicit On 
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports System.Reflection

Public Module modReflection

  Public Function BuildMissingTests(ByVal aSavePath As String) As String
    Dim s As String, t As String
    Dim lEntryModule As String, lEntryFunction As String
    Dim lTestModule As String, lTestFunction As String
    Dim lAddModule As String, lAddFunction As String, oAddModule As String
    Dim found As Boolean
    Dim sEntry As New Collection
    Dim sTest As New Collection
    Dim sAdd As New Collection
    Dim sRec As String
    Dim lFirstClass As Boolean

    Dim a As [Assembly] = Reflection.Assembly.GetCallingAssembly 'GetExecutingAssembly

    aSavePath = System.IO.Path.Combine(aSavePath, a.GetName.Name) 'byval does not change orig
    System.IO.Directory.CreateDirectory(aSavePath)

    s = ReflectAssemblyAsSTring(a)
    SaveFileString(aSavePath & "\Reflection.txt", s)

    While Len(s) > 0
      sRec = StrSplit(s, vbCrLf, "")
      If (InStr(sRec, "<none>") = 0 And InStr(sRec, "Public") > 0) Then
        If Left(LCase(sRec), 4) = "test" Then
          sTest.Add(sRec)
        Else
          sEntry.Add(sRec)
        End If
      End If
    End While

    s = ""
    For Each lEntry As String In sEntry
      lEntryModule = StrSplit(lEntry, ":", "")
      t = StrSplit(lEntry, ":", "")
      lEntryFunction = lEntry
      found = False
      s = s & lEntryModule & ":" & lEntryFunction
      For Each lTest As String In sTest
        lTestModule = StrSplit(lTest, ":", "")
        If "Test_" & lEntryModule = lTestModule Then
          t = StrSplit(lTest, ":", "")
          lTestFunction = Mid(lTest, 5)
          If lTestFunction = lEntryFunction Then 'already have it
            found = True
            s = s & ":match" & vbCrLf
            Exit For
          End If
        End If
      Next
      If Not found Then
        If lEntryModule = "modReflection" Then
          s = s & ":skip" & vbCrLf
        Else
          s = s & ":add" & vbCrLf
          sAdd.Add(lEntryModule & ":" & lEntryFunction)
        End If
      End If
    Next
    s = s & "CountTests:" & sTest.Count & vbCrLf
    s = s & "CountEntry:" & sEntry.Count & vbCrLf
    s = s & "CountAdd:" & sAdd.Count & vbCrLf
    SaveFileString(aSavePath & "\TestsToAdd.txt", s)

    s = "Option Strict Off" & vbCrLf & _
        "Option Explicit On" & vbCrLf & _
        "'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license" & vbCrLf & _
        vbCrLf & _
        "Imports NUnit.Framework" & vbCrLf & _
        "Imports ATCUtility.modReflection" & vbCrLf & _
        vbCrLf & _
        "<TestFixture()> Public Class Test_Builder" & vbCrLf & _
        "  Public Sub TestsAllPresent()" & vbCrLf & _
        "    Dim lTestBuildStatus as String" & vbCrLf & _
        "    lTestBuildStatus = BuildMissingTests(""c:\test\"")" & vbCrLf & _
        "    Assert.AreEqual(""All tests present."", lTestBuildStatus, lTestBuildStatus)" & vbCrLf & _
        "  End Sub" & vbCrLf & _
        "End Class" & vbCrLf & vbCrLf

    oAddModule = ""
    For Each lAdd As String In sAdd
      ' s = s & "process:" & lAdd & vbCrLf
      lAddModule = StrSplit(lAdd, ":", "")
      If lAddModule <> oAddModule Then
        If Len(oAddModule) > 0 Then
          s &= "End Class" & vbCrLf & vbCrLf
        End If
        s &= "<TestFixture()> Public Class Test_" & lAddModule & vbCrLf & vbCrLf
        oAddModule = lAddModule
      End If
      lAddFunction = StrSplit(lAdd, ":", "")
      ' how do we get some arguments?" 
      s &= "  Public Sub Test" & lAddFunction & "()" & vbCrLf & _
           "    '" & lAddFunction & "()" & vbCrLf & _
           "    Assert.Ignore(" & Chr(34) & "Test not yet written" & Chr(34) & ")" & vbCrLf & _
           "  End Sub" & vbCrLf & vbCrLf
    Next
    s = s & "End Class" & vbCrLf
    aSavePath = aSavePath & "\Test_" & a.GetName.Name & ".vb"
    SaveFileString(aSavePath, s)

    If sAdd.Count = 0 Then
      s = "All tests present."
    Else
      s = sAdd.Count & " tests needed. See '" & aSavePath & "' for test stub code."
    End If

    Return s

  End Function

  Private Function ReflectAssemblyAsSTring(ByVal mA As [Assembly]) As String

    Dim mytypes As Type() = mA.GetTypes
    Dim t As Type
    Dim s As String

    For Each t In mytypes
      s = s & MethodDetails(t, "Public,Static", BindingFlags.Public Or BindingFlags.Static)
      s = s & MethodDetails(t, "Public,Instance", BindingFlags.Public Or BindingFlags.Instance)
      s = s & MethodDetails(t, "NonPub,Static", BindingFlags.NonPublic Or BindingFlags.Static)
      s = s & MethodDetails(t, "NonPub,Instance", BindingFlags.NonPublic Or BindingFlags.Instance)
    Next t
    Return s
  End Function

  Private Function MethodDetails(ByRef aT As Type, ByVal aMethodType As String, ByVal aFlag As Integer) As String
    Dim s As String, t As String

    Dim mi As MethodInfo() = aT.GetMethods(aFlag)
    If mi.Length > 0 Then
      Dim m As MethodInfo
      For Each m In mi
        t = aT.Name & ":" & aMethodType & ":" & m.Name & vbCrLf
        If InStr(s, t) = 0 Then 'dont add duplicates
          s = s & t
        End If
        'm.Invoke(obj, Nothing)
      Next m
    Else
      s = s & aT.Name & ":" & aMethodType & ":" & "<none>" & vbCrLf
    End If
    Return s
  End Function
End Module
