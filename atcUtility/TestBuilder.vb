Option Strict Off
Option Explicit On 
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports ATCutility.modReflection

<TestFixture()> Public Class Test_Builder
  Public Sub TestsAllPresent()
    Dim lTestBuildStatus As String

    lTestBuildStatus = BuildMissingTests("c:\test\")
    Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
  End Sub
End Class
