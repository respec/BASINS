Imports System.CodeDom.Compiler
Imports System.Reflection
Imports atcUtility
Imports MapWinUtility

Public Module modScript
  Public Function RunScript(ByVal aLanguage As String, _
                            ByVal aDLLfilename As String, _
                            ByVal aCode As String, _
                            ByRef aErrors As String, _
                            ByVal ParamArray aArgs() As Object) As Object
    Dim assy As System.Reflection.Assembly
    Dim instance As Object
    Dim assyTypes As Type() 'list of items within the assembly

    Dim MethodName As String = "ScriptMain" 'TODO: decide on entry point name

    Logger.Dbg("RunScript:Language:" & aLanguage & ":DllFileName:" & aDLLfilename)

    If aLanguage.ToLower = "dll" Then
      assy = System.Reflection.Assembly.LoadFrom(aCode)
    Else 'compile the code into an assembly
      Try
        If FileExists(aCode) Then
          Logger.Dbg("RunScript:CodeFoundIn:" & aCode)
          aCode = WholeFileString(aCode)
        End If
      Catch ex As Exception
        'Treat as code, not as file name
        Logger.Dbg("RunScript:CodeToCompile:" & vbCrLf & _
               "==================" & vbCrLf & _
               aCode & vbCrLf & _
               "==================" & vbCrLf & _
               "RunScript:EndCode")
      End Try

      assy = CompileScript(aLanguage, aCode, aErrors, aDLLfilename)

    End If

    If aErrors Is Nothing OrElse aErrors.Length = 0 Then
      assyTypes = assy.GetTypes()
      For Each typ As Type In assyTypes
        Dim scriptMethodInfo As MethodInfo = typ.GetMethod(MethodName)
        If Not scriptMethodInfo Is Nothing Then
          Logger.Dbg("RunScript:Invoke:" & scriptMethodInfo.Name)
          If aArgs Is Nothing Then
            Logger.Dbg("RunScript:No Args")
          Else
            For Each lArg As Object In aArgs
              Logger.Dbg("RunScript:Arg:" & lArg.GetType.ToString & ":<" & lArg.ToString & ">")
            Next
          End If
          Try
            Return scriptMethodInfo.Invoke(Nothing, aArgs) 'assy.CreateInstance(typ.Name)
          Catch ex As Exception
            Logger.Dbg("RunScript:Exception:" & ex.ToString)
            Return False
          End Try
        End If
      Next
      aErrors = "RunScript: '" & MethodName & "' not found"
    Else
      Logger.Dbg("RunScript:CompileErrors:" & aErrors)
    End If
  End Function

  'language = "vb" or "cs" or "js"
  Public Function CompileScript(ByVal aLanguage As String, _
                                ByVal aCode As String, _
                                ByRef aErrors As String, _
                       Optional ByVal aOutputFilename As String = "") As System.Reflection.Assembly
    Dim compiler As ICodeCompiler
    Dim params As CompilerParameters
    Dim results As CompilerResults
    Dim provider As CodeDomProvider
    Dim needSupportCode As Boolean = False
    Dim lSupportCode As String = ""

    If aCode.IndexOf("Public ") < 0 Then needSupportCode = True

    Select Case aLanguage.ToLower
      Case "cs" : provider = New Microsoft.CSharp.CSharpCodeProvider
      Case "js" : provider = Activator.CreateInstance("Microsoft.JScript", "Microsoft.JScript.JScriptCodeProvider").Unwrap()
      Case "vb" : provider = New Microsoft.VisualBasic.VBCodeProvider
      Case Else : provider = New Microsoft.VisualBasic.VBCodeProvider
    End Select

    params = New System.CodeDom.Compiler.CompilerParameters
    If aOutputFilename.Length = 0 Then
      params.GenerateInMemory = True      'Assembly is created in memory
    Else
      params.OutputAssembly = aOutputFilename
    End If
    params.TreatWarningsAsErrors = False
    params.WarningLevel = 4
    'params.ReferencedAssemblies.AddRange(refs)

    For Each refAssy As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies()

      Dim lAssyName As String = StrSplit(refAssy.FullName, ",", "")
      Select Case lAssyName
        Case "mscorlib", _
             "mwIdentifier", _
             "TableEditor.mw", _
             "ChilkatDotNet", _
             "MapWindow.resources", _
             "MapWinX"
          'Skip trying these for now, causing errors

          '"MapWinInterfaces", _
          '"AxInterop.MapWinGIS", _
          '"Interop.MapWinGIS", _

        Case Else
          If lAssyName.StartsWith("RemoveMe") Then
            'Don't add temporary assemblies
          Else
            If needSupportCode Then lSupportCode &= "Imports " & lAssyName & vbCrLf
            params.ReferencedAssemblies.Add(refAssy.Location)
          End If
      End Select
    Next

    If needSupportCode Then
      aCode = lSupportCode & vbCrLf _
                           & "Public Module ScriptModule" & vbCrLf _
                           & "  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As Object)" & vbCrLf _
                           & aCode & vbCrLf _
                           & "  End Sub" & vbCrLf _
                           & "End Module"
    End If

    Try
      provider = New Microsoft.VisualBasic.VBCodeProvider
      compiler = provider.CreateCompiler
      results = compiler.CompileAssemblyFromSource(params, aCode)
      If results.Errors.Count = 0 Then        'No compile errors or warnings
        Return results.CompiledAssembly
      Else
        For Each err As CompilerError In results.Errors
          aErrors &= (String.Format( _
              "Line {0}, Col {1}: Error {2} - {3}", _
              err.Line, err.Column, err.ErrorNumber, err.ErrorText)) & vbCrLf
        Next
      End If
    Catch ex As Exception
      'Compile errors don't throw exceptions. This is a deeper problem
      aErrors = ex.Message
    End Try

    If Not IsNothing(aErrors) AndAlso aErrors.Length > 0 Then
      Logger.Dbg("CompileScript:Code:" & aCode)
      Logger.Dbg("CompileScript:Errors:" & aErrors)
    End If

    Return Nothing

  End Function

  'Public Function CallStatic(ByVal assy As System.Reflection.Assembly, _
  '                           ByVal ClassName As String, _
  '                           ByVal MethodName As String, _
  '                           ByVal args() As Object) As Object
  '    Dim scriptType As Type
  '    Dim instance As Object

  '    'Get the type from the assembly. This will allow us access to properties and methods.
  '    scriptType = assy.GetType(ClassName)

  '    'And call the static function
  '    Return scriptType.InvokeMember(MethodName, _
  '       BindingFlags.InvokeMethod Or _
  '       BindingFlags.Public Or _
  '       BindingFlags.Static, _
  '       Nothing, Nothing, args)
  'End Function

  'Public Function CallFunction(ByVal assy As System.Reflection.Assembly, _
  '                           ByVal ClassName As String, _
  '                           ByVal MethodName As String, _
  '                           ByVal args() As Object) As Object
  '    Dim scriptType As Type
  '    Dim instance As Object

  '    'Get the type from the assembly.  This will allow us access to
  '    'all the properties and methods.
  '    scriptType = assy.GetType(ClassName)

  '    'Create an instance of my object
  '    instance = assy.CreateInstance(ClassName)

  '    'And call the non-static function
  '    Return scriptType.InvokeMember(MethodName, _
  '        BindingFlags.Public Or _
  '        BindingFlags.InvokeMethod Or _
  '        BindingFlags.Instance, _
  '        Nothing, instance, args)
  'End Function

  'Public Function SetAndGetProperty(ByVal assy As System.Reflection.Assembly, _
  '                       ByVal ClassName As String, _
  '                       ByVal MethodOrPropertyName As String, _
  '                       ByVal args() As Object) As Object
  '    Dim scriptType As Type
  '    Dim instance As Object

  '    'Get the type from the assembly.  
  '    scriptType = assy.GetType("Sample")

  '    'Create an instance of my object
  '    instance = assy.CreateInstance("Sample")

  '    'First set the property
  '    scriptType.InvokeMember("SampleProperty", _
  '        BindingFlags.Public Or BindingFlags.SetProperty Or BindingFlags.Instance, _
  '        Nothing, instance, args)

  '    Return scriptType.InvokeMember("SampleProperty", _
  '        BindingFlags.Public Or BindingFlags.GetProperty Or BindingFlags.Instance, _
  '        Nothing, instance, Nothing)

  'End Function

End Module
