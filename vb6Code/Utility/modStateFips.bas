Attribute VB_Name = "modStateFips"
Option Explicit

Private Const MAX_FIPS = 78
Private Initialized As Boolean
Private pAbbrev(MAX_FIPS) As String
Private pName(MAX_FIPS) As String
Private pFips() As String

Public Function Abbrev2FIPS(ByVal Abbrev As String) As String
  Dim lAbbrev As String, I As Integer
  lAbbrev = UCase(Trim(Abbrev))
  If Not Initialized Then Initialize
  For I = 1 To MAX_FIPS
    If lAbbrev = pAbbrev(I) Then
      If I < 10 Then
        Abbrev2FIPS = "0" & I
      Else
        Abbrev2FIPS = I
      End If
      Exit Function
    End If
  Next
  Abbrev2FIPS = "00"
End Function

Public Function FIPS2Abbrev(ByVal FIPS As String) As String
  If Not Initialized Then Initialize
  FIPS2Abbrev = "  "
  If IsNumeric(FIPS) Then
    Dim iFips As Integer
    iFips = CInt(FIPS)
    If iFips > 0 And iFips <= MAX_FIPS Then
      FIPS2Abbrev = pAbbrev(iFips)
    End If
  End If
End Function

Public Function FIPS2Name(ByVal FIPS As String) As String
  If Not Initialized Then Initialize
  FIPS2Name = "Unknown State (FIPS " & FIPS & ")"
  If IsNumeric(FIPS) Then
    Dim iFips As Integer
    iFips = CInt(FIPS)
    If iFips > 0 And iFips <= MAX_FIPS Then
      FIPS2Name = pName(iFips)
    End If
  End If
End Function

Public Function Abbrev2Name(ByVal Abbrev As String) As String
  Dim lAbbrev As String, I As Integer
  lAbbrev = UCase(Trim(Abbrev))
  If Not Initialized Then Initialize
  Abbrev2Name = "Unknown State (Abbrev " & Abbrev & ")"
  For I = 1 To MAX_FIPS
    If lAbbrev = pAbbrev(I) Then
      Abbrev2Name = pName(I)
      Exit Function
    End If
  Next
End Function

Private Sub Initialize()
  pAbbrev(2) = "AK":  pName(2) = "Alaska"
  pAbbrev(1) = "AL":  pName(1) = "Alabama"
  pAbbrev(5) = "AR":  pName(5) = "Arkansas"
  pAbbrev(60) = "AS": pName(60) = "American Samoa"
  pAbbrev(4) = "AZ":  pName(4) = "Arizona"
  pAbbrev(6) = "CA":  pName(6) = "California"
  pAbbrev(8) = "CO":  pName(8) = "Colorado"
  pAbbrev(9) = "CT":  pName(9) = "Connecticut"
  pAbbrev(11) = "DC": pName(11) = "District Of Columbia"
  pAbbrev(10) = "DE": pName(10) = "Delaware"
  pAbbrev(12) = "FL": pName(12) = "Florida"
  pAbbrev(13) = "GA": pName(13) = "Georgia"
  pAbbrev(66) = "GU": pName(66) = "Guam"
  pAbbrev(15) = "HI": pName(15) = "Hawaii"
  pAbbrev(19) = "IA": pName(19) = "Iowa"
  pAbbrev(16) = "ID": pName(16) = "Idaho"
  pAbbrev(17) = "IL": pName(17) = "Illinois"
  pAbbrev(18) = "IN": pName(18) = "Indiana"
  pAbbrev(20) = "KS": pName(20) = "Kansas"
  pAbbrev(21) = "KY": pName(21) = "Kentucky"
  pAbbrev(22) = "LA": pName(22) = "Louisiana"
  pAbbrev(25) = "MA": pName(25) = "Massachusetts"
  pAbbrev(24) = "MD": pName(24) = "Maryland"
  pAbbrev(23) = "ME": pName(23) = "Maine"
  pAbbrev(26) = "MI": pName(26) = "Michigan"
  pAbbrev(27) = "MN": pName(27) = "Minnesota"
  pAbbrev(29) = "MO": pName(29) = "Missouri"
  pAbbrev(28) = "MS": pName(28) = "Mississippi"
  pAbbrev(30) = "MT": pName(30) = "Montana"
  pAbbrev(37) = "NC": pName(37) = "North Carolina"
  pAbbrev(38) = "ND": pName(38) = "North Dakota"
  pAbbrev(31) = "NE": pName(31) = "Nebraska"
  pAbbrev(33) = "NH": pName(33) = "New Hampshire"
  pAbbrev(34) = "NJ": pName(34) = "New Jersey"
  pAbbrev(35) = "NM": pName(35) = "New Mexico"
  pAbbrev(32) = "NV": pName(32) = "Nevada"
  pAbbrev(36) = "NY": pName(36) = "New York"
  pAbbrev(39) = "OH": pName(39) = "Ohio"
  pAbbrev(40) = "OK": pName(40) = "Oklahoma"
  pAbbrev(41) = "OR": pName(41) = "Oregon"
  pAbbrev(42) = "PA": pName(42) = "Pennsylvania"
  pAbbrev(72) = "PR": pName(72) = "Puerto Rico"
  pAbbrev(44) = "RI": pName(44) = "Rhode Island"
  pAbbrev(45) = "SC": pName(45) = "South Carolina"
  pAbbrev(46) = "SD": pName(46) = "South Dakota"
  pAbbrev(47) = "TN": pName(47) = "Tennessee"
  pAbbrev(48) = "TX": pName(48) = "Texas"
  pAbbrev(49) = "UT": pName(49) = "Utah"
  pAbbrev(51) = "VA": pName(51) = "Virginia"
  pAbbrev(78) = "VI": pName(78) = "Virgin Islands"
  pAbbrev(50) = "VT": pName(50) = "Vermont"
  pAbbrev(53) = "WA": pName(53) = "Washington"
  pAbbrev(55) = "WI": pName(55) = "Wisconsin"
  pAbbrev(54) = "WV": pName(54) = "West Virginia"
  pAbbrev(56) = "WY": pName(56) = "Wyoming"
  pAbbrev(72) = "PR": pName(56) = "Puerto Rico"
  pAbbrev(78) = "VI": pName(56) = "Virgin Islands"
End Sub
