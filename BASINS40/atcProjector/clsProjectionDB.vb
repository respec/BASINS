Imports System.Collections.Specialized
Imports atcUtility
Imports MapWinUtility

Public Class ProjectionDB
    Private pBaseProjections As ProjectionCollection
    Private pEllipsoids As ProjectionCollection
    Private pStandardProjections As ProjectionCollection

    Public ReadOnly Property BaseProjections() As ProjectionCollection
        Get
            If pBaseProjections Is Nothing Then ReadProjectionDatabase()
            Return pBaseProjections
        End Get
    End Property

    Public ReadOnly Property Ellipsoids() As ProjectionCollection
        Get
            If pEllipsoids Is Nothing Then ReadProjectionDatabase()
            Return pEllipsoids
        End Get
    End Property

    Public ReadOnly Property StandardProjections() As ProjectionCollection
        Get
            If pStandardProjections Is Nothing Then ReadProjectionDatabase()
            Return pStandardProjections
        End Get
    End Property

    Private Function OpenProjectionDatabase() As IatcTable
        OpenProjectionDatabase = Nothing
        Dim dbFilename As String
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        dbFilename = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\atcprj.dbf"
        If Not FileExists(dbFilename) Then
            Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
            dbFilename = lBasinsFolder & "\etc\atcprj.dbf"
        End If
        dbFilename = FindFile("Please locate table of projections atcprj.dbf", dbFilename)

        If dbFilename.Length = 0 Then
            Logger.Dbg("Could not find table of projections atcprj.dbf")
        Else
            OpenProjectionDatabase = atcTableOpener.OpenAnyTable(dbFilename)
            If OpenProjectionDatabase Is Nothing Then
                Logger.Dbg("Could not open table of projections atcprj.dbf")
            End If
        End If
    End Function

    Private Sub ReadProjectionDatabase()
        Dim iRecord As Integer
        Dim id As Integer
        Dim curProjection As Projection
        Dim prevProjection As Projection = Nothing

        pBaseProjections = New ProjectionCollection
        pEllipsoids = New ProjectionCollection
        pStandardProjections = New ProjectionCollection

        Dim tmpDBF As atcTable
        tmpDBF = OpenProjectionDatabase()

        If Not tmpDBF Is Nothing Then
            For iRecord = 1 To tmpDBF.NumRecords
                tmpDBF.CurrentRecord = iRecord
                curProjection = New Projection
                curProjection.ProjectionClass = tmpDBF.Value(1)
                curProjection.Category = Trim(tmpDBF.Value(2))
                curProjection.Name = tmpDBF.Value(3)
                curProjection.Zone = tmpDBF.Value(4)
                curProjection.Ellipsoid = tmpDBF.Value(5)
                For id = 1 To 6
                    curProjection.d(id) = tmpDBF.Value(id + 5)
                Next
                Select Case curProjection.Name
                    Case "defaults"
                        If prevProjection Is Nothing Then
                            Err.Raise("defaults found before projection in database")
                        Else
                            prevProjection.Defaults = curProjection
                        End If
                    Case "fieldnames"
                        If prevProjection Is Nothing Then
                            Err.Raise("fieldnames found before projection in database")
                        Else
                            prevProjection.Fieldnames = curProjection
                        End If
                    Case Else
                        If curProjection.ProjectionClass.Length > 0 And curProjection.Name.Length > 0 Then
                            'this is a projection
                            If curProjection.Category.Length > 0 Then
                                pStandardProjections.Add(curProjection.Category & curProjection.Name, curProjection)  'new StandardProjections type
                                prevProjection = curProjection
                            Else
                                pBaseProjections.Add(curProjection.Name, curProjection)   'new base type
                                pBaseProjections.Add(curProjection.ProjectionClass, curProjection)  'Also index it by abbreviation
                                prevProjection = curProjection
                            End If
                        ElseIf curProjection.Ellipsoid.Length > 0 Then
                            'this is an ellipsoid
                            If Len(curProjection.ProjectionClass) = 0 Then
                                If curProjection.d(2).Length > 0 Then
                                    curProjection.ProjectionClass += " b=" & curProjection.d(2)
                                Else
                                    curProjection.ProjectionClass += " rf=" & curProjection.d(3)
                                End If
                            End If
                            pEllipsoids.Add(curProjection.Ellipsoid, curProjection)
                            pEllipsoids.Add(curProjection.ProjectionClass, curProjection)  'also index by abbreviation
                        End If
                End Select
            Next
            tmpDBF.Clear()
        End If
    End Sub

    Public Sub AddCustomProjection(ByVal aName As String, _
                                   ByVal aProjectionClass As String, _
                                   ByVal aZone As String, _
                                   ByVal aSpheroid As String, _
                                   ByVal D1 As String, _
                                   ByVal D2 As String, _
                                   ByVal D3 As String, _
                                   ByVal D4 As String, _
                                   ByVal D5 As String, _
                                   ByVal D6 As String)
        Dim tmpDBF As atcTable
        tmpDBF = OpenProjectionDatabase()

        If Not tmpDBF Is Nothing Then
            With tmpDBF
                If .FindFirst(3, aName) Then
                    If Logger.Msg("A projection named '" & aName & "' already exists. Replace it?", MsgBoxStyle.YesNo, "Projection") = MsgBoxResult.Yes Then
                        GoTo WriteProjection
                    End If
                Else
                    .CurrentRecord = .NumRecords + 1
WriteProjection:
                    .Value(1) = aProjectionClass
                    .Value(2) = "Custom"
                    .Value(3) = aName
                    .Value(4) = aZone
                    .Value(5) = aSpheroid
                    .Value(6) = D1
                    .Value(7) = D2
                    .Value(8) = D3
                    .Value(9) = D4
                    .Value(10) = D5
                    .Value(11) = D6

                    .WriteFile(.FileName)
                    Logger.Msg("Current projection added to table in Custom category as '" & aName & "'", MsgBoxStyle.OkOnly, "Projection")
                End If
            End With
        End If
    End Sub
End Class
