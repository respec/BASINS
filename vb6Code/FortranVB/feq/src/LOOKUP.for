C
C
C
      SUBROUTINE   XSTYPE
     I                   (STDOUT, VTYPE, ADRS)
 
C     + + + PURPOSE + + +
C     Dump valid list of cross section table types.
 
C     + + + DUMMY ARGUMENTS + + +
      INTEGER STDOUT, ADRS
      INTEGER VTYPE(*)
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     STDOUT   - Fortran unit number for user output and messages
C     VTYPE  - list of valid cross section table types
C     ADRS    - address of the cross section table
 
C     + + + LOCAL VARIABLES + + +
      INTEGER I

C     Called program units
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT
 
C     + + + OUTPUT FORMATS + + +
 50   FORMAT('0*ERR:195* Data deficiency in Tabid= ',A,' Valid Types:')
 52   FORMAT(11X,'Type=',I5)
C***********************************************************************
      WRITE(STDOUT,50) GET_STRING_FROM_FT(ADRS+16)
      DO 100 I=1,35
        IF(VTYPE(I).GT.0) THEN
          WRITE(STDOUT,52) I
        ENDIF
 100  CONTINUE
      RETURN
      END
C
C
C
      SUBROUTINE   XLKT20
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, K, DK, B, DB)
 
C     + + + PURPOSE + + +
C     Given depth find area, top-width, conveyance, etc.
C     using direct linear interpolation for top width.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, B, DB, DK, DT, K, T, YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, IT, L, LA, TYPE, XOFF
      REAL B0, DY, DYI, H, HH, K0, T0, Y, Y0
      

C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/1,10*0,1,7*0,6*1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:02* X-SECTION BELOW RANGE IN XLKT20',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:03* X-SECTION ABOVE RANGE IN XLKT20',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT20
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A              TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      IT = L + XOFF
      DY = FTAB(IT) - Y0
      H = Y - Y0
      HH = 0.5*H
      DYI = 1.0/DY
      DB = (FTAB(IT+4) - B0)*DYI
      DT =  (FTAB(IT+1) - T0)*DYI
      DK = (FTAB(IT+3) - K0)*DYI
      T = T0 + H*DT
      B = B0 + H*DB
      A = FTAB(L+2) + HH*(T + T0)
      K = K0 + H*DK
      DK = (K +K)*DK
      K = K*K
 
      RETURN
      END
C
C
C
      SUBROUTINE   XLKT21
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, J, K, DK, B, DB)
 
C     + + + PURPOSE + + +
C     Given depth find area, top-width, conveyance, etc.
C     using direct linear interpolation for top width.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, B, DB, DK, DT, J, K, T, YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     J      - first moment of area about water surface in the table
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, L, LA, TYPE, XOFF
      REAL A0, B0, DY, DYI, H, HH, J0, K0, T0, Y, Y0
 
C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/1,10*0,1,7*0,0,2*1,0,2*1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:02* X-SECTION BELOW RANGE IN XLKT21',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:03* X-SECTION ABOVE RANGE IN XLKT21',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT21
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4),
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      A0 = FTAB(L+2)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
      J0 = FTAB(L+5)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      DY = FTAB(L+XOFF) - Y0
      H = Y - Y0
      HH = 0.5*H
      DYI = 1.0/DY
      DB = (FTAB(L+XOFF+4) - B0)*DYI
      DT =  (FTAB(L+XOFF+1) - T0)*DYI
      DK = (FTAB(L+XOFF+3) - K0)*DYI
      T = T0 + H*DT
      B = B0 + H*DB
      A = A0 + HH*(T + T0)
      J = J0 + HH*(A + A0 - H*(T - T0)*0.1666667)
      K = K0 + H*DK
      DK = (K +K)*DK
      K = K*K
 
      RETURN
      END
C
C
C
      SUBROUTINE   XLKT22
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, J, K, DK, B, DB, ALP, DALP, QC)
 
C     + + + PURPOSE + + +
C     Given the depth, YA, find: A-area; T-top width; DT- derivative
C     of top width; J-first moment of area about water surface;
C     K-conveyance; DK=derivative of conveyance; B- beta;
C     DB- derivative of beta; ALP-alpha; and DALP-derivative of
C     alpha, and QC, critical flow.  Use linear interpolation
C     on logarithms of QC and depth.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, ALP, B, DALP, DB, DK, DT, J, K, QC, T, YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     J      - first moment of area about water surface in the table
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
C     ALP    - value of energy flux correction coefficient
C     DALP   - derivative wrt depth of the energy flux coefficient
C     QC     - critical flow
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, L, LA, TYPE, XOFF
      REAL A0, ALP0, B0, DY, H, HH, J0, K0, P, QC0, QC1, T0, Y, Y0, Y1
 
C     + + + INTRINSICS + + +
      INTRINSIC EXP, LOG
 
C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/11*0,1,9*0,1,0,0,1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:33* X-SECTION BELOW RANGE IN XLKT22',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:34* X-SECTION ABOVE RANGE IN XLKT22',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT22
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      A0 = FTAB(L+2)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
      J0 = FTAB(L+5)
      ALP0 = FTAB(L+6)
      QC0 = FTAB(L+7)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      Y1 = FTAB(L+XOFF)
      DY = Y1 - Y0
      H = Y - Y0
      HH = 0.5*H
      DB = (FTAB(L+XOFF+4) - B0)/DY
      DT =  (FTAB(L+XOFF+1) - T0)/DY
      DK = (FTAB(L+XOFF+3) - K0)/DY
      DALP = (FTAB(L+XOFF+6) - ALP0)/DY
      QC1 = FTAB(L+XOFF+7)
      T = T0 + H*DT
      B = B0 + H*DB
      ALP = ALP0 + H*DALP
      A = A0 + HH*(T + T0)
      J = J0 + HH*(A + A0 -H*(T - T0)/6.)
      K = K0 + H*DK
      DK = 2.*K*DK
      K = K*K
C     COMPUTE VALUE OF QC USING LOGARITHMS
 
      IF(Y0.EQ.0.0) THEN
        L = L + XOFF
        Y0 = FTAB(L)
        Y1 = FTAB(L+XOFF)
        QC0 = FTAB(L+7)
        QC1 = FTAB(L+XOFF+7)
      ENDIF
 
      IF(Y.GT.0.0) THEN
        P = LOG(Y/Y0)*LOG(QC1/QC0)/LOG(Y1/Y0)
        QC = QC0*EXP(P)
      ELSE
        QC = 0.0
      ENDIF
 
      RETURN
      END
C
C
C
      SUBROUTINE   XLKT23
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, K, DK, B, DB, MA, DMA, MQ, DMQ)
 
C     + + + PURPOSE + + +
C     Given depth, YA, compute the area, top width, derivative
C     of top width, DT, the conveyance, K, the derivative of conveyanc, DK,
C     the momentum flux correction coefficient, B, its
C     derivative, DB, the correction factor on area
C     to give volume per unit length for the distance axis, MA,
C     the derivative wrt depth of MA, DMA; the correction factor
C     on Q to give momentum per unit length for the distance
C     axis, MQ; and the derivative wrt depth of MQ, DMQ.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, B, DB, DK, DMA, DMQ, DT, K, MA, MQ, T, YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
C     MA     - weight factor on area to get volume per unit length
C     DMA    - derivative of the sinuosity correction of area wrt depth
C     MQ     - weight factor on flow to get momentum per unit length
C     DMQ    - derivative of the sinuosity correction of momentum wrt
C               depth
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, IT, L, LA, TYPE, XOFF
      REAL B0, DY, DYI, H, HH, K0, MA0, MQ0, T0, Y, Y0
 
C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/22*0,1,1,1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:02* X-SECTION BELOW RANGE IN XLKT23',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:03* X-SECTION ABOVE RANGE IN XLKT23',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT23
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      IT = L + XOFF
      DY = FTAB(IT) - Y0
      H = Y - Y0
      HH = 0.5*H
      DYI = 1.0/DY
      DB = (FTAB(IT+4) - B0)*DYI
      DT =  (FTAB(IT+1) - T0)*DYI
      DK = (FTAB(IT+3) - K0)*DYI
      T = T0 + H*DT
      B = B0 + H*DB
      A = FTAB(L+2) + HH*(T + T0)
      K = K0 + H*DK
      DK = (K +K)*DK
      K = K*K
 
      GOTO(23, 24, 25), TYPE-22
 
 23   CONTINUE
C       FIND THE CURVILINEAR ELEMENTS FROM TYPE 23
        MA0 = FTAB(L+5)
        MQ0 = FTAB(L+6)
        DMA = (FTAB(IT+5) - MA0)*DYI
        DMQ = (FTAB(IT+6) - MQ0)*DYI
        MA = MA0 + DMA*H
        MQ = MQ0 + DMQ*H
 
        RETURN
 
 24   CONTINUE
C       FIND THE CURVILINEAR ELEMENTS FROM TYPE 24
        MA0 = FTAB(L+6)
        MQ0 = FTAB(L+7)
        DMA = (FTAB(IT+6) - MA0)*DYI
        DMQ = (FTAB(IT+7) - MQ0)*DYI
        MA = MA0 + DMA*H
        MQ = MQ0 + DMQ*H
        RETURN

 25   CONTINUE
C       FIND THE CURVILINEAR ELEMENTS FROM TYPE 25
        MA0 = FTAB(L+8)
        MQ0 = FTAB(L+9)
        DMA = (FTAB(IT+8) - MA0)*DYI
        DMQ = (FTAB(IT+9) - MQ0)*DYI
        MA = MA0 + DMA*H
        MQ = MQ0 + DMQ*H
        RETURN
 
      END
C
C
C
      SUBROUTINE   XLKT24
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, J, K, DK, B, DB, MA, DMA, MQ, DMQ)
 
C     + + + PURPOSE + + +
C     Given depth find area, top-width, conveyance, etc.
C     using direct linear interpolation for top width.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, B, DB, DK, DMA, DMQ, DT, J, K, MA, MQ, T, YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     J      - first moment of area about water surface in the table
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
C     MA     - weight factor on area to get volume per unit length
C     DMA    - derivative of the sinuosity correction of area wrt depth
C     MQ     - weight factor on flow to get momentum per unit length
C     DMQ    - derivative of the sinuosity correction of momentum wrt
C               depth
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, IT, L, LA, TYPE, XOFF
      REAL A0, B0, DY, DYI, H, HH, J0, K0, MA0, MQ0, T0, Y, Y0
 
C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/23*0,1,1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:02* X-SECTION BELOW RANGE IN XLKT24',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:03* X-SECTION ABOVE RANGE IN XLKT24',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT24
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      A0 = FTAB(L+2)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
      J0 = FTAB(L+5)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      IT = L + XOFF
      DY = FTAB(IT) - Y0
      H = Y - Y0
      HH = 0.5*H
      DYI = 1.0/DY
      DB = (FTAB(L+XOFF+4) - B0)*DYI
      DT =  (FTAB(L+XOFF+1) - T0)*DYI
      DK = (FTAB(L+XOFF+3) - K0)*DYI
      T = T0 + H*DT
      B = B0 + H*DB
      A = A0 + HH*(T + T0)
      J = J0 + HH*(A + A0 - H*(T - T0)*0.1666667)
      K = K0 + H*DK
      DK = (K +K)*DK
      K = K*K
 
      IF(TYPE.EQ.24) THEN
C       FIND THE CURVILINEAR ELEMENTS FROM TYPE 24
        MA0 = FTAB(L+6)
        MQ0 = FTAB(L+7)
        DMA = (FTAB(IT+6) - MA0)*DYI
        DMQ = (FTAB(IT+7) - MQ0)*DYI
        MA = MA0 + DMA*H
        MQ = MQ0 + DMQ*H
      ELSE
C       FIND THE CURVILINEAR ELEMENTS FROM TYPE 25
        MA0 = FTAB(L+8)
        MQ0 = FTAB(L+9)
        DMA = (FTAB(IT+8) - MA0)*DYI
        DMQ = (FTAB(IT+9) - MQ0)*DYI
        MA = MA0 + DMA*H
        MQ = MQ0 + DMQ*H
      ENDIF
 
      RETURN
      END
C
C
C
      SUBROUTINE   XLKT25
     I                   (ADRS,
     M                    YA,
     O                    A, T, DT, J, K, DK, B, DB, ALP, DALP, QC, MA,
     O                    DMA, MQ, DMQ)
 
C     + + + PURPOSE + + +
C     Given the depth, YA, find: A-area; T-top width; DT- derivative
C     of top width; J-first moment of area about water surface;
C     K-conveyance; DK=derivative of conveyance; B- beta;
C     DB- derivative of beta; ALP-alpha;  DALP-derivative of
C     alpha;  QC, critical flow; MA= correction factor on area
C     to give volume per unit length for the distance axis;
C     DMA= derivative wrt depth of MA; MQ= correction factor
C     on Q to give momentum per unit length for the distance
C     axis; and DMQ= derivative wrt depth of MQ.
 
      IMPLICIT NONE
C     + + + DUMMY ARGUMENTS + + +
      INTEGER ADRS
      REAL A, ALP, B, DALP, DB, DK, DMA, DMQ, DT, J, K, MA, MQ, QC, T,
     A     YA
 
C     + + +DUMMY ARGUMENT DEFINITIONS + + +
C     ADRS   - address of the function table in FTAB/ITAB
C     YA     - depth to use for table look up
C     A      - Cross sectional area from the cross section table
C     T      - top width of the cross section
C     DT     - derivative of the top width with respect to depth
C     J      - first moment of area about water surface in the table
C     K      - conveyance
C     DK     - derivative of conveyance with respect to depth
C     B      - the value of the momentum flux correction coef. from the
C               table
C     DB     - derivative of B with respect to depth
C     ALP    - value of energy flux correction coefficient
C     DALP   - derivative wrt depth of the energy flux coefficient
C     QC     - critical flow
C     MA     - weight factor on area to get volume per unit length
C     DMA    - derivative of the sinuosity correction of area wrt depth
C     MQ     - weight factor on flow to get momentum per unit length
C     DMQ    - derivative of the sinuosity correction of momentum wrt
C               depth
 
C     + + + COMMON BLOCKS + + +
      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      INCLUDE 'xscom.com'
      INCLUDE 'offcom.com'
 
C     + + + SAVED VALUES + + +
      INTEGER VTYPE(35)
      SAVE VTYPE
 
C     + + + LOCAL VARIABLES + + +
      INTEGER HA, L, LA, TYPE, XOFF
      REAL A0, ALP0, B0, DY, H, HH, J0, K0, MA0, MQ0, P, QC0, QC1, T0,
     A     Y, Y0, Y1
 
C     + + + INTRINSICS + + +
      INTRINSIC EXP, LOG
 
C     + + + EXTERNAL NAMES + + +
      CHARACTER*16 GET_STRING_FROM_FT
      EXTERNAL  GET_STRING_FROM_FT, XSTYPE
 
C     + + + DATA INITIALIZATIONS + + +
      DATA VTYPE/24*0,1,10*0/
 
C     + + + OUTPUT FORMATS + + +
 2000 FORMAT(' ','*WRN:33* X-SECTION BELOW RANGE IN XLKT25',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
 2010 FORMAT(' ','*WRN:34* X-SECTION ABOVE RANGE IN XLKT25',
     A      /,1X,' TABLE ID   = ',A,
     B      /,1X,' STATION NUMBER = ',F10.3
     C      /,1X,' TIME           = ',F14.0,
     D      /,1X,' DEPTH          = ',F10.2)
C***********************************************************************
C     HA = HIGH ADDRESS
C     LA = LOW ADDRESS
C     L = ADDRESS FOUND ON THE LAST CALL TO XLKT25
 
      Y = YA
      HA = ITAB(ADRS)
      LA = ADRS + XTIOFF
      L = ITAB(ADRS+3)
 
      TYPE = ITAB(ADRS+2)
      XOFF = OFFVEC(TYPE)
      IF(VTYPE(TYPE).EQ.0) THEN
        CALL XSTYPE
     I             (STDOUT, VTYPE, ADRS)
        STOP 'Abnormal stop: errors found.'
      ENDIF
 
      IF(Y.GE.FTAB(L)) THEN
C       CHECK FOR ARGUMENT ABOVE MAX ARG IN THE TABLE
        IF(Y.GT.FTAB(HA)) THEN
          WRITE(STDOUT,2010) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = HA - XOFF
          Y = FTAB(HA)
          YA = Y
        ELSE
 100      CONTINUE
            IF(Y.GT.FTAB(L+XOFF)) THEN
              L = L + XOFF
              GOTO 100
            ENDIF
        ENDIF
      ELSE
C       CHECK FOR ARGUMENT BELOW MIN ARG IN THE TABLE
        IF(Y.LT.FTAB(LA)) THEN
          WRITE(STDOUT,2000) GET_STRING_FROM_FT(ADRS+16), FTAB(ADRS+4), 
     A                       TIME, Y
          L = LA
          Y = FTAB(L+XOFF)
          YA = Y
        ELSE
 110      CONTINUE
            L = L - XOFF
            IF(Y.LT.FTAB(L)) GOTO 110
        ENDIF
      ENDIF
C     AT THIS POINT L DEFINES THE LOW ARGUMENT END OF THE
C     INTERVAL CONTAINING THE ARGUMENT, PERHAPS ADJUSTED
C     FOR ARGUMENT OUT OF RANGE.
 
C     RESET POINTER FOR LAST ADDRESS
 
      ITAB(ADRS+3) = L
 
C     FETCH VALUES FROM FTAB
 
      Y0 = FTAB(L)
      T0 = FTAB(L+1)
      A0 = FTAB(L+2)
      K0 = FTAB(L+3)
      B0 = FTAB(L+4)
      J0 = FTAB(L+5)
      ALP0 = FTAB(L+6)
      QC0 = FTAB(L+7)
      MA0 = FTAB(L+8)
      MQ0 = FTAB(L+9)
 
C     DIRECT LINEAR INTERPOLATION FOR T AND SQRT(CONVEYANCE)
C     N.B. K IN FTAB IS SQRT(CONVEYANCE)
 
      Y1 = FTAB(L+XOFF)
      DY = Y1 - Y0
      H = Y - Y0
      HH = 0.5*H
      DB = (FTAB(L+XOFF+4) - B0)/DY
      DT =  (FTAB(L+XOFF+1) - T0)/DY
      DK = (FTAB(L+XOFF+3) - K0)/DY
      DALP = (FTAB(L+XOFF+6) - ALP0)/DY
      QC1 = FTAB(L+XOFF+7)
      DMA = (FTAB(L+XOFF+8) - MA0)/DY
      DMQ = (FTAB(L+XOFF+9) - MQ0)/DY
      T = T0 + H*DT
      B = B0 + H*DB
      ALP = ALP0 + H*DALP
      MA = MA0 + H*DMA
      MQ = MQ0 + H*DMQ
      A = A0 + HH*(T + T0)
      J = J0 + HH*(A + A0 -H*(T - T0)/6.)
      K = K0 + H*DK
      DK = 2.*K*DK
      K = K*K
C     COMPUTE VALUE OF QC USING LOGARITHMS
 
      IF(Y0.EQ.0.0) THEN
        L = L + XOFF
        Y0 = FTAB(L)
        Y1 = FTAB(L+XOFF)
        QC0 = FTAB(L+7)
        QC1 = FTAB(L+XOFF+7)
      ENDIF
 
      IF(Y.GT.0.0) THEN
        P = LOG(Y/Y0)*LOG(QC1/QC0)/LOG(Y1/Y0)
        QC = QC0*EXP(P)
      ELSE
        QC = 0.0
      ENDIF
 
      RETURN
      END
