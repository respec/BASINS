Attribute VB_Name = "UtilProjection"
Attribute VB_Ext_KEY = "RVB_UniqueId" ,"3607BBEB0022"
Option Explicit
Public Sub LL_2_UTM(Lat#, lon#, northing#, easting#, zone&)

  'Function to convert from latitude and longitude
  'to UTM northing and easting and zone
  'See pp.48-65;269-270, USGS Map Projections Working Manual, J.P.Snyder
  
  'Note that input latitude and longitide coordinates are assumed to be
  'in decimal degree units.

    Dim lat_rad#
    Dim e_prime#, lambda0#
    Dim ax#, C#, M#, n#, t#, rtemp#
    Dim coef1#, coef2#, coef3#, coef4#
    Dim a#, e#, e2#
    Dim i&
    Dim RAD2DEG#, DEG2RAD#
    Dim K0#, M0#

    'North American Datum 27 (NAD27) (also Clarke 1866) parameters
    a = 6378206.4    'NAD27 equatorial radius
    e = 0.0822719    'NAD27 eccentricity
    e2 = (e * e)     'NAD27 eccentricity squared

    'Other needed map parameters
    RAD2DEG = 57.29577951308  'Radian to degree conversion
    DEG2RAD = 0.01745329252   'Degree to radian conversion
    K0 = 0.9996               'Scale factor on UTM Central Meridian
    M0 = 0#                   'Distance along meridian from equator to point

    'Calculate some needed values
    e_prime = e2 / (1# - e2)   'Equation 8.12
  
    'The four following values are from Eq 3.21
    coef1 = 1# - (e ^ 2#) / 4# - 3# * (e ^ 4#) / 64# - 5# * (e ^ 6#) / 256#
    coef1 = a * coef1 * DEG2RAD
    coef2 = a * (3# * (e ^ 2#) / 8# + 3# * (e ^ 4#) / 32# + 45# * (e ^ 6#) / 1024#)
    coef3 = a * (15# * (e ^ 4#) / 256# + 45# * (e ^ 6#) / 1024#)
    coef4 = a * (35# * (e ^ 6#) / 3072#)

    'Calculate the Northing and Easting
    'Assign appropriate central meridian to UTM zone based on input longitude
    zone = Int(((lon + 177#) / 6# + 1.5))
    lambda0 = (zone - 1) * 6 - 177

    'Equations 8.12 - 8.15
    lat_rad = Lat * DEG2RAD
    n = a / Sqr(1# - e2 * Sin(lat_rad) * Sin(lat_rad))
    t = Tan(lat_rad) * Tan(lat_rad)
    C = e_prime * Cos(lat_rad) * Cos(lat_rad)
    ax = Cos(lat_rad) * ((lon - lambda0) * DEG2RAD)

    'Equation 3.22, The distance along central meridian to given latitude
    M = (111132.0894 * Lat) - (16216.94 * Sin(2# * lat_rad)) _
         + (17.21 * Sin(4# * lat_rad)) - (0.02 * Sin(6# * lat_rad))
    M = (coef1 * Lat) - (coef2 * Sin(2# * lat_rad)) _
         + (coef3 * Sin(4# * lat_rad)) - (coef4 * Sin(6# * lat_rad))

    'Equations 8.9 and 8.10
    easting = K0 * n * (ax + (1# - t + C) * (ax ^ 3#) / 6# _
                    + (5# - 18# * t + (t ^ 2#) + 72# * C - 58# * e_prime) _
                    * (ax ^ 5#) / 120#) + 500000
    rtemp = ((ax ^ 2#) / 2# + (5# - t + 9# * C + 4# * (C ^ 2#)) _
                     * (ax ^ 4#) / 24# + (61# - 58# * t + (t ^ 2#) _
                     + 600# * C - 330# * e_prime) * (ax ^ 6#) / 720#)
    northing = K0 * (M + n * Tan(lat_rad) * rtemp)
                     
End Sub


Public Sub UTM_2_LL(northing#, easting#, zone&, Lat#, lon#)

  'Function to calculate Latitude and Longitude coordinates from UTM coordinates
  'See pp.48-65;270-271, USGS Map Projections Working Manual, J.P.Snyder

  'Note that output latitude and longitide coordinates are in decimal
  'degree units.

    Dim e_prime#, east_temp#, lambda0#, M#, mu#
    Dim phi1#, phi1_deg#, temp#, temp2#
    Dim c1#, D#, e1#, n1#, t1#, r1#
    Dim a#, e#, e2#
    Dim i&
    Dim RAD2DEG#, DEG2RAD#
    Dim K0#, M0#

    'North American Datum 27 (NAD27) (also Clarke 1866) parameters
    a = 6378206.4    'NAD27 equatorial radius
    e = 0.0822719    'NAD27 eccentricity
    e2 = (e * e)     'NAD27 eccentricity squared

    'Other needed map parameters
    RAD2DEG = 57.29577951308  'Radian to degree conversion
    DEG2RAD = 0.01745329252   'Degree to radian conversion
    K0 = 0.9996               'Scale factor on UTM Central Meridian
    M0 = 0#                   'Distance along meridian from equator to point

    'Calculate some needed values outside the loop
    e_prime = e2 / (1# - e2)
    e1 = (1# - Sqr(1# - e2)) / (1# + Sqr(1# - e2))
    temp = (a * (1# - e2 / 4# - 3# * (e ^ 4#) / 64# - 5# * (e ^ 6#) / 256#))
  
    'Calculate longitude at Central Meridian for given UTM zone
    lambda0 = (zone - 1) * 6 - 177
  
    'Subtract "false" easting (Central Meridian value) from entered values
    east_temp = easting - 500000#

    'Calculate values for Eqns 8.12, 8.20, 3.24, 7.19
    M = M0 + northing / K0
    mu = M / temp

    'Calculate value for Eqn 3.26
    phi1 = mu + (3# * e1 / 2# - 27# * (e1 ^ 3#) / 32#) * Sin(2# * mu) _
              + (21# * e1 * e1 / 16# - 55# * (e1 ^ 4#) / 32#) * Sin(4# * mu) _
              + (151# * (e1 ^ 3#) / 96#) * Sin(6# * mu)
    phi1_deg = phi1 * RAD2DEG
  
    'Calculate values for Eqns 8.21 - 8.25
    c1 = e_prime * Cos(phi1) * Cos(phi1)
    t1 = Tan(phi1) * Tan(phi1)
    n1 = a / Sqr(1# - e2 * Sin(phi1) * Sin(phi1))
    temp2 = 1# - e2 * Sin(phi1) * Sin(phi1)
    r1 = a * (1# - e2) / (temp2 ^ 1.5)
    D = east_temp / (n1 * K0)

    'Calculate values for Eqns 8.17 and 8.18
    Lat = phi1_deg - (n1 * Tan(phi1) / r1) * (RAD2DEG * ((D ^ 2#) / 2# _
                    - (5# + 3# * t1 + 10# * c1 - 4# * (c1 ^ 2#) _
                    - 9# * (e_prime ^ 2#)) * (D ^ 4#) / 24# _
                    + (61# + 90# * t1 + 298# * c1 + 45# * (t1 ^ 2#) _
                    - 252# * (e_prime ^ 2#) - 3# * (c1 ^ 2#)) * (D ^ 6#) / 720#))

    lon = lambda0 + (RAD2DEG * (D - (1# + 2# * t1 + c1) * (D ^ 3#) / 6# _
                     + (5# - 2# * c1 + 28# * t1 - 3# * (c1 ^ 2#) + 8# * e_prime _
                     + 24# * (t1 ^ 2#)) * (D ^ 5#) / 120#) / Cos(phi1))

End Sub

Public Sub LL_2_AL(dlat#, dlon#, LAM0#, x#, y#)

'     Converts latitude and longitude (degrees) to the Albers
'        equal area projection for the United States.

'     DLAT   - Latitude in degrees.
'     DLON   - Longitude in degrees.
'     LAM0   - base longitude
'     X      - X Albers coordinates
'     Y      - Y Albers coordinates
 
'      Dim RHO#, RHO1SQ#, FR2B#, SPSI1#, RHO0#, BD#, DEGRAD#, DIF#

'      RHO1SQ = 84607444756165#
'      FR2B = 134661958430670#
'      SPSI1 = 0.4924235601035
'      RHO0 = 9914713.649668
'      RHO0 = 9909400#
'      BD = 0.010521490583632
'      DEGRAD = 0.017453292519943

'      RHO = Sqr(RHO1SQ + FR2B * (SPSI1 - Sin(DEGRAD * dlat)))
'      DIF = BD * (dlon - LAM0)
      
'      x = RHO * Sin(DIF)
'      y = RHO0 - RHO * Cos(DIF)

'      above replaced by jlkittle 12/15/98 because inverse failed and
'        meaning of constants were unknown
      Static INIT&
      Static RADIAN#, e2#, ECCEN#, PHI#
      Static XN#, C#, P0#
      Static AA#, OLON#
      Dim temp#, Q#, P#, D#, THETA#
      
      If INIT <> 1 Or OLON <> LAM0 Then
        Call LL2ALB_INIT(RADIAN#, XN#, AA#, PHI#, e2#, P0#, ECCEN#, C#)
        'Debug.Print RADIAN, XN, AA, PHI, E2, P0, ECCEN, C
        OLON = LAM0
        INIT = 1
      End If
      
      PHI = dlat / RADIAN
      temp = 1# - e2 * Sin(PHI) * Sin(PHI)
      Q = Log((1# - ECCEN * Sin(PHI)) / (1# + ECCEN * Sin(PHI)))
      Q = Q * (1# / (2# * ECCEN))
      Q = (Sin(PHI) / temp) - Q
      Q = (1# - e2) * Q
      P = AA * (Sqr(C - XN * Q)) / XN
      D = dlon - OLON
      THETA = XN * D
      THETA = THETA / RADIAN
      x = P * Sin(THETA)
      y = P0 - P * Cos(THETA)

End Sub
Public Sub AL_2_LL(x#, y#, LAM0#, dlat#, dlon#)
'     transform from X/Y Albers conic equal-area projection to Long/Lat

'     X      - X Albers coordinates
'     Y      - Y Albers coordinates
'     DLAT   - Latitude in degrees.
'     DLON   - Longitude in degrees.

      Static INIT&
      Static RADIAN#, e2#, ECCEN#
      Static XN#, C#, P0#, PHI#
      Static OLON#, AA#
      Dim P#, THETA#, Q#, t#, j#, RPHI#, temp#, temp1#, temp2#
      Dim i&
      
      If INIT <> 1 Or OLON <> LAM0 Then
        Call LL2ALB_INIT(RADIAN#, XN#, AA#, PHI#, e2#, P0#, ECCEN#, C#)
        'Debug.Print RADIAN, XN, AA, PHI, E2, P0, ECCEN, C
        OLON = LAM0
        INIT = 1
      End If

'     do transformation
      P = Sqr(x * x + (P0 - y) * (P0 - y))
      THETA = Atn(x / (P0 - y))
      THETA = THETA * RADIAN
      Q = (C - (P * XN / AA) * (P * XN / AA)) / XN
      t = Q / (1# - e2)
      'PHI = ArcSIN(Q / 2#) * RADIAN
      j = Q / 2#
      PHI = Atn(j / Sqr(-j * j + 1)) * RADIAN
      i = 0
10:
        i = i + 1
        RPHI = PHI / RADIAN
        temp = (1# - e2 * Sin(RPHI) * Sin(RPHI))
        temp1 = t - (Sin(RPHI) / temp)
        temp2 = Log((1# - ECCEN * Sin(RPHI)) / (1# + ECCEN * Sin(RPHI)))
        temp2 = temp2 * (1# / (2# * ECCEN))
        temp1 = temp1 + temp2
        temp1 = temp1 * RADIAN
        temp1 = temp1 * (temp * temp) / (2# * Cos(RPHI))
        PHI = PHI + temp1
      If (Abs(temp1) > 0.0000001 And i < 100) Then GoTo 10

      dlat = PHI
      dlon = OLON + (THETA / XN)

End Sub
Public Sub MASP_2_LL(ByVal x#, ByVal y#, dlat#, dlon#)
'     transform from X/Y Massachusetts State Plane to Long/Lat
'     formula for the ellipsoid

'     X      - X MA State Plane coordinates
'     Y      - Y MA State Plane coordinates
'     DLAT   - Latitude in degrees.
'     DLON   - Longitude in degrees.

      Static INIT&
      Static pi#, t0#, t1#, t2#, m1#, m2#, n#, f#, ro#, a#
      Static e#, e2#, th0#, th1#, th2#
      
      Dim r#, t#, th#, i&, temp#, temp1#, temp2#
      
      If INIT <> 1 Then
        pi = 3.14159265358979
        a = 6378137 'for GRS80
        e2 = 0.00676866
        e = 0.0822719
        th0 = 41 * pi / 180 'reference lat
        th1 = 41.716666667 * pi / 180 'std parallel 1
        th2 = 42.683333333 * pi / 180 'std parallel 2
        m1 = Cos(th1) / Sqr(1 - (e2 * (Sin(th1)) ^ 2))
        m2 = Cos(th2) / Sqr(1 - (e2 * (Sin(th2)) ^ 2))
        t1 = Tan((pi / 4) - (th1 / 2)) / ((1 - e * Sin(th1)) / (1 + e * Sin(th1))) ^ (e / 2)
        t2 = Tan((pi / 4) - (th2 / 2)) / ((1 - e * Sin(th2)) / (1 + e * Sin(th2))) ^ (e / 2)
        t0 = Tan((pi / 4) - (th0 / 2)) / ((1 - e * Sin(th0)) / (1 + e * Sin(th0))) ^ (e / 2)
        n = Log(m1 / m2) / Log(t1 / t2)
        f = m1 / (n * (t1 ^ n))
        ro = a * f * (t0 ^ n)
        'INIT = 1
      End If

'     do transformation
      x = x - 200000
      y = y - 750000
      r = Sqr((x ^ 2) + ((ro - y) ^ 2))
      th = Atn(x / (ro - y)) * 180 / pi
      t = (r / (a * f)) ^ (1 / n)
      dlat = 90 - 2 * Atn(t) * 180 / pi
      i = 0
10:
        i = i + 1
        temp = dlat
        temp2 = t * (((1 - e * Sin(temp * pi / 180)) / (1 + e * Sin(temp * pi / 180)))) ^ (e / 2)
        dlat = 90 - 2 * Atn(temp2) * 180 / pi
        temp1 = temp - dlat
      If (Abs(temp1) > 0.00000001 And i < 100) Then GoTo 10
      dlon = th / n - 71.5
End Sub
Public Sub LL_2_MASP(dlat#, dlon#, x#, y#)
'     transform from Long/Lat to Mass State Plane
'     formula for the ellipsoid

'     DLAT   - Latitude in degrees.
'     DLON   - Longitude in degrees.
'     X      - X MA State Plane coordinates
'     Y      - Y MA State Plane coordinates

      Static INIT&
      Static pi#, t0#, t1#, t2#, m1#, m2#, n#, f#, ro#, a#
      Static e#, e2#, th0#, th1#, th2#
      
      Dim r#, t#, th#, i&, temp#, temp1#, temp2#
      
      If INIT <> 1 Then
        pi = 3.14159265358979
        a = 6378137 'for GRS80
        e2 = 0.00676866
        e = 0.0822719
        th0 = 41 * pi / 180 'reference lat
        th1 = 41.716666667 * pi / 180 'std parallel 1
        th2 = 42.683333333 * pi / 180 'std parallel 2
        m1 = Cos(th1) / Sqr(1 - (e2 * (Sin(th1)) ^ 2))
        m2 = Cos(th2) / Sqr(1 - (e2 * (Sin(th2)) ^ 2))
        t1 = Tan((pi / 4) - (th1 / 2)) / ((1 - e * Sin(th1)) / (1 + e * Sin(th1))) ^ (e / 2)
        t2 = Tan((pi / 4) - (th2 / 2)) / ((1 - e * Sin(th2)) / (1 + e * Sin(th2))) ^ (e / 2)
        t0 = Tan((pi / 4) - (th0 / 2)) / ((1 - e * Sin(th0)) / (1 + e * Sin(th0))) ^ (e / 2)
        n = Log(m1 / m2) / Log(t1 / t2)
        f = m1 / (n * (t1 ^ n))
        ro = a * f * (t0 ^ n)
        INIT = 1
      End If

'     do transformation
      dlat = dlat * pi / 180
      t = Tan((pi / 4) - (dlat / 2)) / ((1 - e * Sin(dlat)) / (1 + e * Sin(dlat))) ^ (e / 2)
      r = a * f * (t ^ n)
      th = n * (dlon + 71.5)
      x = r * Sin(th * pi / 180)
      y = ro - r * Cos(th * pi / 180)
      x = x + 200000
      y = y + 750000
End Sub
Private Sub LL2ALB_INIT(RADIAN#, XN#, AA#, PHI#, e2#, P0#, ECCEN#, C#)
      Dim TEMP11#, TEMP22#
      Dim temp#, Q#, t#, RPHI#
      Dim SUP#, SLP#, OLON#, OLAT#
      Dim a#, b#, j#
      Dim PHI0#, SLP1#, SUP1#
      Dim TEMP0#, temp1#, temp2#, XM1#, XM2#, Q0#, Q1#, Q2#

      RADIAN = 57.2957795
'     compute transformation constatns
      OLAT = 23#
      a = 6378206.4
      b = 6356583.8
      SLP = 29.5
      SUP = 45.5
      PHI0 = OLAT / RADIAN
      SLP1 = SLP / RADIAN
      SUP1 = SUP / RADIAN
      e2 = (a * a - b * b) / (a * a)
      ECCEN = Sqr(e2)
      TEMP0 = 1# - e2 * Sin(PHI0) * Sin(PHI0)
      temp1 = 1# - e2 * Sin(SLP1) * Sin(SLP1)
      TEMP11 = Sqr(temp1)
      temp2 = 1# - e2 * Sin(SUP1) * Sin(SUP1)
      TEMP22 = Sqr(temp2)
      XM1 = Cos(SLP1) / TEMP11
      XM2 = Cos(SUP1) / TEMP22
      Q1 = Log((1# - ECCEN * Sin(SLP1)) / (1# + ECCEN * Sin(SLP1)))
      Q1 = Q1 * (1# / (2# * ECCEN))
      Q1 = (Sin(SLP1) / temp1) - Q1
      Q1 = (1# - e2) * Q1
      Q2 = Log((1# - ECCEN * Sin(SUP1)) / (1# + ECCEN * Sin(SUP1)))
      Q2 = Q2 * (1# / (2# * ECCEN))
      Q2 = (Sin(SUP1) / temp2) - Q2
      Q2 = (1# - e2) * Q2
      Q0 = Log((1# - ECCEN * Sin(PHI0)) / (1# + ECCEN * Sin(PHI0)))
      Q0 = Q0 * (1# / (2# * ECCEN))
      Q0 = (Sin(PHI0) / TEMP0) - Q0
      Q0 = (1# - e2) * Q0
      XN = (XM1 * XM1 - XM2 * XM2) / (Q2 - Q1)
      C = XM1 * XM1 + XN * Q1
      P0 = a * (Sqr(C - XN * Q0)) / XN
      AA = a
End Sub

