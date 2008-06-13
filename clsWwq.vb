Partial Class SwatInput
    Private pWwq As clsWwq = New clsWwq(Me)
    ReadOnly Property Wwq() As clsWwq
        Get
            Return pWwq
        End Get
    End Property

    ''' <summary>
    ''' WWQ Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsWwq
        Private pSwatInput As SwatInput
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table() As DataTable
            pSwatInput.Status("Reading WWQ from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM wwq;")
        End Function
        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            Dim pR As DataRow = aTable.Rows(0)
            pSwatInput.Status("Writing WWQ text ...")

            Dim lSB As New System.Text.StringBuilder            

            ' --------------------------------------------------------------------------------
            ' 1st Line
            ' --------------------------------------------------------------------------------
            lSB.AppendLine("Watershed water quality file" + Space(10) + ".wwq file " & DateNowString() & "ARCGIS-SWAT interface AV")
            ' --------------------------------------------------------------------------------
            ' 2nd Line
            ' ------2-LAO
            lSB.AppendLine(MakeString(pR.Item(1), 0, 4, 4) + "| LAO : Light averaging option")
            ' ------3-IGROPT
            lSB.AppendLine(MakeString(pR.Item(2), 0, 4, 4) + "| IGROPT : Algal specific growth rate option")
            ' ------ 4-AI0
            lSB.AppendLine(MakeString(pR.Item(3), 3, 4, 8) + "| AI0 : Ratio of chlorophyll-a to algal biomass [µg-chla/mg algae]")
            ' ------ 5-AI1
            lSB.AppendLine(MakeString(pR.Item(4), 3, 4, 8) + "| AI1 : Fraction of algal biomass that is nitrogen [mg N/mg alg]")
            ' ------ 6-AI2
            lSB.AppendLine(MakeString(pR.Item(5), 3, 4, 8) + "| AI2 : Fraction of algal biomass that is phosphorus [mg P/mg alg]")
            ' ------ 7-AI3
            lSB.AppendLine(MakeString(pR.Item(6), 3, 4, 8) + "| AI3 : The rate of oxygen production per unit of algal photosynthesis [mg O2/mg alg)]")
            ' ------ 8-AI4
            lSB.AppendLine(MakeString(pR.Item(7), 3, 4, 8) + "| AI4 : The rate of oxygen uptake per unit of algal respiration [mg O2/mg alg)]")
            ' ------ 9-AI5
            lSB.AppendLine(MakeString(pR.Item(8), 3, 4, 8) + "| AI5 : The rate of oxygen uptake per unit of NH3-N oxidation [mg O2/mg NH3-N]")
            ' ------ 10-AI6
            lSB.AppendLine(MakeString(pR.Item(9), 3, 4, 8) + "| AI6 : The rate of oxygen uptake per unit of NO2-N oxidation [mg O2/mg NO2-N]")
            ' ------ 11-MUMAX
            lSB.AppendLine(MakeString(pR.Item(10), 3, 4, 8) + "| MUMAX : Maximum specific algal growth rate at 20º C [day-1]")
            ' ------ 12-RHOQ
            lSB.AppendLine(MakeString(pR.Item(11), 3, 4, 8) + "| RHOQ : Algal respiration rate at 20º C [day-1]")
            ' ------ 13-TFACT
            lSB.AppendLine(MakeString(pR.Item(12), 3, 4, 8) + "| TFACT : Fraction of solar radiation computed in the temperature heat balance that is photosynthetically active")
            ' ------14-K_1
            lSB.AppendLine(MakeString(pR.Item(13), 3, 4, 8) + "| K_1 : Half-saturation coefficient for light [kJ/(m2·min)]")
            ' ------15-K_N
            lSB.AppendLine(MakeString(pR.Item(14), 3, 4, 8) + "| K_N : Michaelis-Menton half-saturation constant for nitrogen [mg N/lL]")
            ' ------16-K_P
            lSB.AppendLine(MakeString(pR.Item(15), 3, 4, 8) + "| K_P : Michaelis-Menton half-saturation constant for phosphorus [mg P/l]")
            ' ------17-LAMBDA0
            lSB.AppendLine(MakeString(pR.Item(16), 3, 4, 8) + "| LAMBDA0 : Non-algal portion of the light extinction coefficient [m-1]")
            ' ------ 18-LAMBDA1
            lSB.AppendLine(MakeString(pR.Item(17), 3, 4, 8) + "| LAMBDA1 : Linear algal self-shading coefficient [m-1·(µg chla/l)-1)]")
            ' ------ 19-LAMBDA2
            lSB.AppendLine(MakeString(pR.Item(18), 3, 4, 8) & "| LAMBDA2 : Nonlinear algal self-shading coefficient [m-1·(µg chla/l)-2]")
            ' ------ 20-P_N
            lSB.AppendLine(MakeString(pR.Item(19), 3, 4, 8) + "| P_N : Algal preference factor for ammonia")

            IO.File.WriteAllText(pSwatInput.OutputFolder & "\basins.wwq", lSB.ToString)
            ReplaceNonAscii(pSwatInput.OutputFolder & "\basins.wwq")
        End Sub
    End Class
End Class
