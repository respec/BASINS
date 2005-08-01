Imports System.CodeDom.Compiler
Imports System.Reflection

Public Module modScript
  ', ByVal refs() As String
  Public Function RunScript(ByVal language As String, ByVal code As String, ByVal args() As Object, ByVal aFilename As String) As Object
    Dim errors As String
    Dim assy As System.Reflection.Assembly
    Dim instance As Object
    Dim assyTypes As Type() 'list of items within the assembly

    Dim MethodName As String = "Main" 'TODO: decide on entry point name

    'First compile the code into an assembly
    assy = CompileScript(language, code, errors, aFilename)

    If errors Is Nothing Then
      assyTypes = assy.GetTypes()
      For Each typ As Type In assyTypes
        Dim scriptMethodInfo As MethodInfo = typ.GetMethod(MethodName)
        If Not scriptMethodInfo Is Nothing Then
          Return scriptMethodInfo.Invoke(Nothing, args) 'assy.CreateInstance(typ.Name)

          'Return typ.InvokeMember(MethodName, _
          '   BindingFlags.InvokeMethod Or _
          '   BindingFlags.Public Or _
          '   BindingFlags.Static, _
          '   Nothing, Nothing, args)
        End If
      Next
      Return "RunScript: '" & MethodName & "' not found"
    Else
      Debug.WriteLine("Compile errors: " & vbCr & errors)
    End If
  End Function

  'refs() As String = {"System.dll", "Microsoft.VisualBasic.dll"}
  ', ByVal refs() As String

  'language = "vb" or "cs" or "js"
  Public Function CompileScript(ByVal language As String, _
                                ByVal code As String, _
                                ByRef errors As String, _
                       Optional ByVal OutputFilename As String = "") As System.Reflection.Assembly
    Dim compiler As ICodeCompiler
    Dim params As CompilerParameters
    Dim results As CompilerResults
    Dim provider As CodeDomProvider
    Select Case (language)
      Case "cs" : provider = New Microsoft.CSharp.CSharpCodeProvider
      Case "js" : provider = Activator.CreateInstance("Microsoft.JScript", "Microsoft.JScript.JScriptCodeProvider").Unwrap()
      Case "vb" : provider = New Microsoft.VisualBasic.VBCodeProvider
      Case Else : provider = New Microsoft.VisualBasic.VBCodeProvider
    End Select

    params = New System.CodeDom.Compiler.CompilerParameters
    If OutputFilename.Length = 0 Then
      params.GenerateInMemory = True      'Assembly is created in memory
    Else
      params.OutputAssembly = OutputFilename
    End If
    params.TreatWarningsAsErrors = False
    params.WarningLevel = 4
    'params.ReferencedAssemblies.AddRange(refs)

    For Each refAssy As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies()
      params.ReferencedAssemblies.Add(refAssy.Location)
    Next

    Try
      provider = New Microsoft.VisualBasic.VBCodeProvider
      compiler = provider.CreateCompiler
      results = compiler.CompileAssemblyFromSource(params, code)
      If results.Errors.Count = 0 Then        'No compile errors or warnings
        Return results.CompiledAssembly
      Else
        For Each err As CompilerError In results.Errors
          errors &= (String.Format( _
              "Line {0}, Col {1}: Error {2} - {3}", _
              err.Line, err.Column, err.ErrorNumber, err.ErrorText))
        Next
      End If
    Catch ex As Exception
      'Compile errors don't throw exceptions; you've got some deeper problem
      errors = ex.Message
    End Try

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
