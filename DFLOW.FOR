C***************************************************************
C                                                              *
C                           D F L O W                          *
C                                                              *
C***************************************************************
C
C  PERSONAL COMPUTER VERSION OF DFLOW PROGRAM
C
C  THIS PROGRAM FINDS BIOLOGICALLY-BASED, EXTREME VALUE OR
C  HUMAN HEALTH (HARMONIC MEAN) DESIGN FLOWS FROM AN
C  HISTORICAL RECORD OF DAILY STREAMFLOWS. REFER TO
C  US EPA TECHNICAL GUIDANCE MANUAL FOR PERFORMING WASTE
C  LOAD ALLOCATION, BOOK VI, DESIGN CONDITIONS, CHAPTER 1,
C  STREAM DESIGN FLOW FOR STEADY-STATE MODELING (AUGUST 1986)
C  FOR DEFINITIONS OF TERMS AND DESCRIPTIONS OF COMPUTATIONS.
C
C  WRITTEN BY L. ROSSMAN, US EPA-RREL, CINCINNATI, OH 45268
C  VERSION 2.0, FEBRUARY 2, 1990
C  VERSION 2.1, APRIL 23, 1993
C
C     *******************************************************
C     *                  IMPORTANT NOTE                     *
C     *                  ---------------                    *
C     * THIS PROGRAM ASSUMES THE EXISTENCE OF A DATA FILE   *
C     * OF DAILY STREAMFLOWS ON UNIT 1. THIS FILE IS CALLED *
C     * "RIVER".FLO WHERE "RIVER" IS A USER-SUPPLIED NAME   *
C     * THAT CONTAINS DAILY STREAMFLOWS. tHE FIRST LINE OF  *
C     * THIS FILE CONTAINS THE 8-DIGIT  USGS GAGE ID NUMBER *
C     * STARTING IN COLUMN 1. tHE REMAINING LINES CONTAIN   *
C     * THE YEAR, MONTH, DAY (E.G., 88,3,22 FOR MARCH 22,   *
C     * 1988) AND FLOW (CFS) SEPARATED BY COMMAS OR SPACES  *
C     * FOR EACH DAY OF RECORD IN CHRONOLOGICAL ORDER.      *
C     * MISSING VALUES CAN EITHER BE ABSENT FROM THE FILE   *
C     * OR ASSIGNED A POSITIVE NUMBER < 1.E-10. A HARDCOPY  *
C     * OF THE OUTPUT FROM THE PROGRAM IS SAVED IN A FILE   *
C     * CALLED "RIVER".LOG.                                 *
C     *******************************************************
C
C  THE PROGRAM OPERATES ACCORDING TO THE FOLLOWING FLOW DIAGRAM:
C    (NAMES OF PERTINENT SUBROUTINES ARE SHOWN IN PARENTHESES)
C
C      --------------------------------------------------------
C      !       READ IN FLOW GAGE NO. & LOCATION (STATID)      !
C      --------------------------------------------------------
C                               !
C                               !
C       -------------------------------------------------------
C       !   DETERMINE TYPE OF DESIGN FLOW TO COMPUTE (MENU)   !
C       -------------------------------------------------------
C                               !
C                               !
C       -------------------------------------------------------
C       !    OBTAIN INPUT DATA FROM USER (PARAM & INPUT)      !
C       -------------------------------------------------------
C                               !
C                               !
C       -------------------------------------------------------
C       !         SEARCH FOR DESIGN FLOW (SEARCH)             !
C       !-----------------------------------------------------!
C       !!         READ IN DAILY FLOW DATA (FLOW)            !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !!   IF HUMAN HEALTH DESIGN FLOW REQUESTED, FIND     !!
C       !!      HARMONIC MEAN FLOW (HMEAN) AND QUIT          !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !! COMPUTE RUNNING AVERAGES ON DAILY FLOWS (RUNAVG)  !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !!  DETERMINE LOWEST AVERAGE IN EACH YEAR (XTREME)   !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !!  USE LOG PEARSON 3 DISTRIBUTION TO FIND EXTREME   !!
C       !!          VALUE DESIGN FLOW (FLP3)                 !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !!  USE METHOD OF FALSE POSITION TO FIND BIO-BASED   !!
C       !!               DESIGN FLOW (MFP)                   !!
C       !!                      !                            !!
C       !!                      !                            !!
C       !!   COUNT UP HISTORICAL DESIGN FLOW EXCURSIONS &    !!
C       !!    DISPLAY EXCURSION TABLE (XCOUNT & XTABLE)      !!
C       !-----------------------------------------------------!
C       -------------------------------------------------------
C
C=================================================================
C                           MAIN MODULE
C=================================================================
C
C  VARIABLES IN COMMON ARE INITIALIZED IN BLOCK DATA SUB-PROGRAM
C  SN = FLOW GAGE STATION ID NUMBER
C  SL = FLOW GAGE STATION LOCATION
C   I = TYPE OF DESIGN FLOW DESIRED
C
       CHARACTER*80 SN
       CHARACTER*12 FNAME2
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
C
C  OPEN FLOW AND LOG DATA FILES
       CALL FOPEN(FNAME2)
C
C  ASK USER TO CHOOSE TYPE OF DESIGN FLOW DESIRED
10     CALL MENU(I)
       IF (I .EQ. 4) THEN
          CLOSE(UNIT=1)
          CLOSE(UNIT=2)
          WRITE(IO,102) FNAME2
          STOP ' '
       ENDIF
       REWIND 1
       READ(1,100) SN
C
C  GET ADDITIONAL DATA FROM USER, THEN SEARCH FOR DESIGN FLOW
       CALL PARAM(I)
       WRITE(IO,101) SN(1:8)
       WRITE(IF,101) SN(1:8)
       CALL SEARCH(I)
       GOTO 10
C
100    FORMAT(A80)
101    FORMAT(//' DESIGN FLOWS FOR USGS GAGE ',A8/1X,71('='))
102    FORMAT(/' A RECORD OF THIS SESSION CAN BE FOUND IN FILE ',A12/)
       END
C
C
C**********************************************************************
       SUBROUTINE FOPEN(FNAME2)
C**********************************************************************
       CHARACTER*8  RIVER
       CHARACTER*12 FNAME1
       CHARACTER*12 FNAME2
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
C
C  PROMPT USER FOR RIVER NAME
       WRITE(IO,100)
       READ(IN,101) RIVER
       I1 = NRIGHT(RIVER,1,8)
       I2 = NLEFT(RIVER,I1,8)
       FNAME1 = RIVER(I1:I2)//'.FLO'
       OPEN(UNIT=1,FILE=FNAME1,STATUS='OLD',ERR=10)
       FNAME2 = RIVER(I1:I2)//'.LOG'
       OPEN(UNIT=2,FILE=FNAME2)
       RETURN
10     WRITE(IO,102)
       STOP ' '
       RETURN
100    FORMAT(/
     1 '                      D F L O W'/
     2 '                     Version 2.0'/
     3 ' A Program to Compute Design Streamflows in Accordance'/
     4 ' with the US EPA Technical Guidance Manual for Performing'/
     5 ' Waste Load Allocation, Book VI, Chapter 1, August 1986'/
     6 ' =========================================================='//
     7 ' NAME OF RIVER TO ANALYZE (8 OR LESS CHARACTERS)  ==> '\)
101    FORMAT(A8)
102    FORMAT(/' **************************************'
     2        /' * NO FLOW FILE EXISTS FOR THIS RIVER *'
     3        /' **************************************'/)
       END
C
C
C********************************************************************
       FUNCTION NRIGHT(S,L1,L2)
C********************************************************************
C
C  DETERMINES POSITION OF FIRST NON-BLANK CHARACTER IN STRING S
       CHARACTER S,BLANK
       DATA BLANK/' '/
C
       DO 10 I = L1, L2
          IF (S(I:I) .NE. BLANK) GOTO 20
10     CONTINUE
       NRIGHT = L2
       RETURN
20     NRIGHT = I
       RETURN
       END
C
C
C********************************************************************
       FUNCTION NLEFT(S,L1,L2)
C********************************************************************
C
C  DETERMINES POSITION JUST BEFORE FIRST BLANK CHARACTER IN STRING S
       CHARACTER S,BLANK
       DATA BLANK/' '/
C
       DO 10 I = L1, L2
          IF (S(I:I) .EQ. BLANK) GOTO 20
10     CONTINUE
       NLEFT = L2
       RETURN
20     NLEFT = I-1
       RETURN
       END
C
C
C********************************************************************
       SUBROUTINE MENU(I)
C********************************************************************
C
C  ASKS USER TO SPECIFY TYPE OF DESIGN FLOW DESIRED
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
C
10     WRITE(IO,101)
       READ(IN,*) I
       IF (I .LT. 1 .OR. I .GT. 4) GOTO 10
       K = I
       IF (I .EQ. 1) THEN
20        WRITE(IO,102)
          READ(IN,*) I1
          IF (I1 .LT. 1 .OR. I1 .GT. 2) GOTO 20
          I = 10 + I1
       ENDIF
       RETURN
C
101    FORMAT(/' ENTER THE NUMBER OF THE DESIGN FLOW YOU ',
     2          'WISH TO CALCULATE:'/
     3        /'    1 - AQUATIC LIFE, BIOLOGICALLY-BASED'
     4        /'    2 - AQUATIC LIFE, EXTREME VALUE'
     5        /'    3 - HUMAN HEALTH, HARMONIC MEAN'
     6        /'    4 - EXIT THE PROGRAM')
102    FORMAT(/' WHICH TYPE OF WATER QUALITY CRITERION APPLIES:'/
     2        /'    1 - CRITERION MAXIMUM CONCENTRATION (ACUTE)'
     3        /'    2 - CRITERION CONTINUOUS CONCENTRATION (CHRONIC)')
       END
C
C
C*********************************************************************
       SUBROUTINE PARAM(I)
C*********************************************************************
C
C  DETERMINES IF USER HAS TO ENTER DEFINITIONAL PARAMETERS
C  SEE BLOCK DATA SUB-PROGRAM FOR DEFINITION OF VARIABLES
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
C
C  ESTABLISH DEFAULT PARAMETER VALUES
       M = 1
       XMR = 3.0
       XME = 5.0
       XMI = 120.0
       J1 = 0
       J2 = 99
C
C  I > 10 FOR BIO-BASED DESIGN FLOW
       IF (I .GT. 10) THEN
          IF (I .EQ. 12) M = 4
10        WRITE(IO,101)
          READ(IN,*) I1
          IF (I1 .LT. 1 .OR. I1 .GT. 2) GOTO 10
          IF (I1 .EQ. 2) CALL INPUT
          WRITE(IF,111) M,XMR,XMI,XME
       ENDIF
C
C  I = 2 FOR EXTREME VALUE DESIGN FLOW
       IF (I .EQ. 2) CALL INPUT
C
C  FOR ALL DESIGN FLOWS, ESTABLISH WHICH PORTION OF FLOW RECORD
C  TO EXAMINE
20     WRITE(IO,102)
       READ(IN,*) J
       IF (J .LT. 1 .OR. J .GT. 2) GOTO 20
       IF (J .EQ. 1) RETURN
       WRITE(IO,109)
       READ(IN,*) J1
       WRITE(IO,110)
       READ(IN,*) J2
       IF (J1 .GE. 1900) J1 = J1 - 1900
       IF (J2 .GE. 1900) J2 = J2 - 1900
       RETURN
101    FORMAT(/' USE DEFAULT SETTINGS FOR THE BIO-BASED'
     2         ' DESIGN FLOW PARAMETERS'
     3        /' (AS DESCRIBED IN US EPA TECHNICAL GUIDANCE MANUAL ON'
     4         ' DESIGN FLOWS)'/
     5        /'    1 - YES'
     6        /'    2 - NO')
102    FORMAT(/' HOW DO YOU WANT TO ANALYZE THE AVAILABLE FLOW RECORD:'/
     3        /'    1 - ANALYZE THE ENTIRE AVAILABLE RECORD'
     4        /'    2 - ANALYZE ONLY A PORTION OF THE RECORD')
109    FORMAT(/' WHAT IS THE FIRST YEAR OF THE FLOW RECORD TO',
     2         ' BE ANALYZED')
110    FORMAT(/' WHAT IS THE LAST YEAR  OF THE FLOW RECORD TO',
     2         ' BE ANALYZED')
111    FORMAT(/' BIO-BASED DESIGN FLOW PARAMETERS'/1X,71('-')/
     1        ' LENGTH OF FLOW AVERAGING PERIOD        : ',
     2 I3,'   DAYS'/
     3        ' AVERAGE INTERVAL BETWEEN EXCURSIONS    : ',
     4 F5.1,' YEARS'/
     5        ' LENGTH OF EXCURSION CLUSTERING PERIOD  : ',
     6 F5.1,' DAYS'/  ' MAX. NUMBER OF EXCURSIONS PER CLUSTER  : ',
     7 F5.1)
       END
C
C
C*********************************************************************
       SUBROUTINE INPUT
C*********************************************************************
C
C  PROMPTS USER TO ENTER DESIGN FLOW DEFINITIONAL PARAMETERS
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
C
       WRITE(IO,102)
C
C  K =1 FOR BIO-BASED DESIGN FLOW
       IF (K .EQ. 1) THEN
10        WRITE(IO,103)
          READ(IN,*) M
          IF (M .LT. 1 .OR. M .GT. 30) GOTO 10
20        WRITE(IO,104)
          READ(IN,*) XMR
          IF (XMR .LE. 0. .OR. XMR .GT. 500) GOTO 20
30        WRITE(IO,105)
          READ(IN,*) XMI
          IF (XMI .LT. 0.) GOTO 30
40        WRITE(IO,106)
          READ(IN,*) XME
          IF (XME .LT. 0.) GOTO 40
C
C  K = 2 FOR EXTREME VALUE DESIGN FLOW
       ELSE
50        WRITE(IO,107)
          READ(IN,*) M
          IF (M .LT. 1 .OR. M .GT. 30) GOTO 50
60        WRITE(IO,108)
          READ(IN,*) XMR
          IF (XMR .LT. 0. .OR. XMR .GT. 500) GOTO 60
       END IF
       RETURN
C
102    FORMAT(/' ENTER VALUES FOR THE FOLLOWING PARAMETERS'/
     2         ' (TYPICAL VALUES ARE SHOWN IN PARENTHESES):'/)
103    FORMAT(/' NUMBER OF DAYS IN A FLOW AVERAGING PERIOD',
     2       /' (1 FOR ACUTE OR 4 FOR CHRONIC CRITERIA)')
104    FORMAT(/' AVERAGE NUMBER OF YEARS BETWEEN EXCURSIONS (3)')
105    FORMAT(/' LENGTH OF EXCURSION CLUSTERING PERIOD (120 DAYS)')
106    FORMAT(/' MAXIMUM NUMBER OF EXCURSIONS COUNTED PER CLUSTER (5)')
107    FORMAT(' FLOW AVERAGING PERIOD (7 DAYS)')
108    FORMAT(/' RETURN PERIOD ON YEARS WITH EXCURSIONS (10 YEARS)')

       END
C
C
C******************************************************************
       SUBROUTINE SEARCH(I)
C******************************************************************
C
C  CONDUCTS SEARCH FOR DESIGN FLOW
C
C  A      = ALLOWED NUMBER OF EXCURSIONS OVER FLOW RECORD
C  DFLOW  = DESIGN FLOW
C  NY     = NUMBER OF COMPLETE WATER YEARS IN RECORD
C  NDAYS  = NUMBER OF DAYS OF RECORDED FLOWS
C  Y      = VECTOR OF ANNUAL LOW FLOWS
C  XDFLOW = BIO-BASED EXCURSIONS ASSOCIATED WITH DFLOW
C  ALL OTHER VARIABLES ARE DEFINED IN BLOCK DATA SUB-PROGRAM
C
       PARAMETER (NMAX=100)
       PARAMETER (UKNOWN=1.E20)
       CHARACTER*13 CTYPE(2)
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       DIMENSION Y(NMAX)
       DATA XMAX/100/, CTYPE/'CMC (ACUTE)','CCC (CHRONIC)'/
C
C  RESTORE MISSING VALUES TO FLOW VECTOR
       IF (JD2 .GT. JD1) THEN
          DO 5 N = 1, JD2-JD1
5         X(N) = UKNOWN
       ENDIF
C
C  READ IN DAILY STREAMFLOWS & ADJUST ACTUAL PERIOD OF RECORD.
C  FOR AQUATIC LIFE DESIGN FLOW, COMPUTE M-DAY RUNNING AVERAGES
C  & FIND THEIR ANNUAL MINIMUMS.
       CALL FLOW(J1,J2,NDAYS)
       IF (NDAYS .EQ. 0) GOTO 20
       IF (K .NE. 3) THEN
          CALL RUNAVG(M,K)
          CALL XTREME(Y,NY)
          IF (NY .LT. XMR) GOTO 20
       ENDIF
       J1 = 1900 + J1
       J2 = 1900 + J2
       WRITE(IO,101) J1,J2
       WRITE(IF,101) J1,J2
       WRITE(IO,111) NDAYS
       WRITE(IF,111) NDAYS
C
C  FOR HUMAN HEALTH DESIGN FLOW, FIND HARMONIC MEAN & RETURN
       IF (K .EQ. 3) THEN
          DFLOW = HMEAN(X,(JD2-JD1))
          WRITE(IO,105) DFLOW
          WRITE(IF,105) DFLOW
          CALL PAUSE(IN,IO)
          RETURN
       ENDIF
C
C  FOR AQUATIC LIFE DESIGN FLOW, BEGIN SEARCH WITH DFLOW = EXTREME
C  VALUE DESIGN FLOW. STOP IF THIS IS THE TYPE OF FLOW REQUESTED.
       DFLOW = FLP3(XMR,Y,NY)
       IF (K .EQ. 2) THEN
          MR = XMR + .5
          WRITE(IO,102) M,MR,DFLOW
          WRITE(IF,102) M,MR,DFLOW
          CALL PAUSE(IN,IO)
          RETURN
       END IF
C
C  OTHERWISE USE METHOD OF FALSE POSITION TO FIND BIO-BASED
C  DESIGN FLOW SOMEWHERE BETWEEN ZERO & EXTREME VALUE FLOW
       A = FLOAT(NDAYS)/365./XMR
       CALL MFP(A,DFLOW)
       WRITE(IO,103) A
       WRITE(IF,103) A
       WRITE(IO,104) CTYPE(I-10),DFLOW
       WRITE(IF,104) CTYPE(I-10),DFLOW
       CALL PAUSE(IN,IO)
C
C  ALLOW USER TO REQUEST AN EXCURSION TABLE FOR A SPECIFIED FLOW
10     WRITE(IO,106)
       READ(IN,*) DFLOW
       IF (DFLOW .EQ. 0.0) RETURN
       WRITE(IO,108)
       WRITE(IF,108)
       CALL XCOUNT(DFLOW,XDFLOW)
       IF (XDFLOW .GT. XMAX) THEN
          WRITE(IO,107) XDFLOW
          WRITE(IF,107) XDFLOW
          CALL PAUSE(IN,IO)
       ELSE
          WRITE(IO,109) J1,J2,DFLOW
          WRITE(IF,109) J1,J2,DFLOW
          CALL XTABLE(IO)
          CALL XTABLE(IF)
          CALL PAUSE(IN,IO)
       ENDIF
       GOTO 10
C
C  ERROR MESSAGE FOR INSUFFICIENT FLOW RECORD
20     WRITE(IO,110)
       WRITE(IF,110)
       CALL PAUSE(IN,IO)
       RETURN
C
101    FORMAT(' PERIOD OF RECORD ANALYZED',15X,' : ',I4,' TO ',I4)
102    FORMAT(1X,I2,'-Q-',I2,' DESIGN FLOW',21X,' : ',F12.2,' CFS'/)
103    FORMAT(' ALLOWED NUMBER OF EXCURSIONS',12X,' : ',F12.2)
104    FORMAT(' BIO-BASED ',A13,' DESIGN FLOW',5X,' : ',F12.2,' CFS'/)
105    FORMAT(' HUMAN HEALTH (HARMONIC MEAN) DESIGN FLOW : ',F12.2,
     2        ' CFS'/)
106    FORMAT(/' ENTER A FLOW (CFS) FOR WHICH YOU WANT AN',
     2        /' EXCURSION TABLE (OR 0 FOR NO TABLE)')
107    FORMAT(' NUMBER OF EXCURSIONS AT THIS DESIGN FLOW : ',F8.0/
     2        ' (TOO MANY TO DISPLAY AN EXCURSION TABLE)')
108    FORMAT(//)
109    FORMAT(' WATER QUALITY EXCURSIONS FOR ',I4,'-',I4,
     2        ' AT DESIGN FLOW OF',F12.2,' CFS')
110    FORMAT(' PERIOD OF RECORD TOO SHORT.'/)
111    FORMAT(' NUMBER OF DAYS IN FLOW RECORD',11X,' : ',I12)
       END
C
C*****************************************************************
       SUBROUTINE PAUSE(IN,IO)
C*****************************************************************
       WRITE(IO,10)
10     FORMAT(' PRESS <ENTER> KEY TO CONTINUE ...')
       READ(IN,20) ANS
20     FORMAT(A1)
       RETURN
       END
C
C
C*****************************************************************
       SUBROUTINE FLOW(J1,J2,NDAYS)
C*****************************************************************
C
C  READS IN DAILY FLOW DATA FROM FILE KNOWN AS UNIT 1.
C  YEAR EQUAL TO '99' OR A POSITIVE FLOW VALUE LESS THAN 1.E-10
C  INDICATES A MISSING VALUE.
C
C
C  JDN    = JULIAN DAY (01/01/00 = 1 WITH FEB. 29TH IGNORED)
C  MDAY   = DAY OF MONTH
C  MONTH  = MONTH OF YEAR
C  MYR    = YEAR OF 20-TH CENTURY
C  NDAYS  = NUMBER OF DAYS OF RECORDED FLOWS
C  NDY(I) = CUMULATIVE DAYS IN CALENDAR YEAR UP TO MONTH I
C  Q      = DAILY FLOW VALUE (CFS)
C  OTHER VARIABLES ARE DEFINED IN BLOCK DATA SUB-PROGRAM
C
       REAL*4 Q
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       DIMENSION NDY(12)
       DATA NDY/0,31,59,90,120,151,181,212,243,273,304,334/
C
       JD1 = 0
       JDN = 0
       NDAYS = 0
C
C  POSITION FLOW FILE TO FIRST DAY OF WATER YEAR J1
104       READ(1,*,END=50) MYR,MONTH,MDAY,Q
105       IF (MONTH .NE. M1) GOTO 104
106       IF (MYR .LT. J1) GOTO 104
       J1 = MYR
       JD1 = J1*365 + NDY(M1)
       JD2 = J2*365 + 365 + NDY(M1)
       GOTO 20
C
C  READ IN NEXT DAY'S FLOW, CHECK FOR MISSING VALUE, COMPUTE
C  JULIAN DAY, AND STORE VALUE IN X
107       READ(1,*,END=50) MYR,MONTH,MDAY,Q
108       IF (MYR .EQ. 99 .OR. MONTH .EQ. 99) GOTO 107
20     JJ = 365*MYR + NDY(MONTH) + MDAY
       IF (JJ .GT. JD2) GOTO 50
       JDN = JJ
       NDAYS = NDAYS + 1
109    IF (Q .EQ. 0.0 .OR. Q .GT. 1.0E-10) X(JDN-JD1) = Q
       GOTO 107
C
C  END OF FLOW RECORD. ADJUST ENDING YEAR J2.
50     J2 = (JDN-NDY(M1)-1)/365
       JD2 = JDN
       RETURN
C
       END
C
C
C*******************************************************************
       SUBROUTINE RUNAVG(M,K)
C*******************************************************************
C
C  COMPUTES M-DAY RUNNING AVERAGES OF FLOWS IN X
C
C  A     = RUNNING AVERAGE ELEMENT
C  B     = RECORD OF PREVIOUS RUNNING AVERAGE ELEMENTS
C  K     = 1 FOR BIO-BASED DESIGN FLOW, 2 FOR EXTREME VALUE
C  L     = CURRENT DAY WITHIN RUNNING AVERAGE PERIOD
C  L0    = NUMBER OF ZERO FLOWS IN RUNNING AVERAGE PERIOD
C  M,EM  = LENGTH OF RUNNING AVERAGE PERIOD
C  SUM   = SUM OF RUNNING AVERAGE ELEMENTS
C  OTHER VARIABLES ARE DEFINED IN BLOCK DATA SUB-PROGRAM
C
       PARAMETER (UKNOWN=1.E20)
       REAL*8 A,B,SUM
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       DIMENSION B(365)
C
C  INITIALIZE RUNNING AVERAGE COUNT & SUM
       EM = M
       L = 0
       L0 = 0
       SUM = 0.0
C
C  EXAMINE EACH DAY OF RECORD
       N2 = JD2 - JD1
       DO 20 N = 1,N2
C
C  CURRENT DAY'S FLOW IS MISSING: RESET RUNNING AVERAGE COUNT
          IF (X(N) .EQ. UKNOWN) THEN
             L = 0
             L0 = 0
             SUM = 0.0
          ELSE
C
C  OTHERWISE ADD CURRENT FLOW (OR ITS RECIPROCAL) TO RUNNING AVERAGE
C  NOTE: EXTREME VALUE DESIGN FLOW USES ARITHMETIC RUNNING AVG.,
C        BIO-BASED DESIGN FLOW USES HARMONIC RUNNING AVG.
             A = X(N)
             IF (K .EQ. 1 .AND. A .GT. 0.0) A = 1.0/A
C  RUNNING AVERAGE BLOCK NOT FULL:
             IF (L .LT. M) THEN
                SUM = SUM + A
                L = L + 1
                IF (A .EQ. 0.0) L0 = L0 + 1
                B(L) = A
             ELSE
C  RUNNING AVERAGE BLOCK FULL:
                SUM = SUM - B(1) + A
                IF (B(1) .EQ. 0.0) L0 = L0 - 1
                IF (M .GT. 1) THEN
                   DO 10 I = 2,M
                      B(I-1) = B(I)
10                 CONTINUE
                ENDIF
                IF (A .EQ. 0.0) L0 = L0 + 1
                B(M) = A
             END IF
C
C  REPLACE X(N) WITH MISSING VALUE. IF RUNNING AVERAGE BLOCK IS FULL,
C  RECORD CURRENT RUNNING AVG. (AFTETETEADJUSTMENT FOR ZERO FLOW DAYS)
C  IN X(N-M+1).
             X(N) = UKNOWN
             IF (L .EQ. M) THEN
                IF (K .EQ. 1) THEN
                   EL = M - L0
                   IF (L0 .EQ. M) THEN
                      X(N-M+1) = 0.0
                   ELSE
                      X(N-M+1) = EL/EM*EL/SUM
                   ENDIF
                ELSE
                   X(N-M+1) = SUM/EM
                ENDIF
             END IF
          END IF
20     CONTINUE
       RETURN
       END
C
C
C*******************************************************************
       SUBROUTINE XTREME(Y,NY)
C*******************************************************************
C
C  FINDS MINIMUM AVERAGE FLOW IN EACH YEAR OF RECORD
C
C  NY    = NUMBER OF YEARS OF COMPLETE FLOW RECORDS
C  Y     = VECTOR OF MINIMUM ANNUAL FLOWS
C  OTHER VARIABLES ARE DEFINED IN BLOCK DATA SUB-PROGRAM
C
       PARAMETER (HUGE=1.E20)
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       DIMENSION Y(1)
C
       NY = 0
       N2 = JD2 - JD1
       J = 0
C
10     YTMP = HUGE
       DO 20 I = 1,365
          J = J + 1
          IF (J .GT. N2) GOTO 30
          IF (X(J) .LT. YTMP) YTMP = X(J)
20     CONTINUE
C
30     IF (YTMP .LT. HUGE) THEN
          NY = NY + 1
          Y(NY) = YTMP
       END IF
       IF (J .LT. N2) GOTO 10
       RETURN
       END
C
C
C*******************************************************************
       FUNCTION HMEAN(X,N)
C*******************************************************************
C
C  DETERMINES HARMONIC MEAN OF N VALUES IN X
C
       PARAMETER (UKNOWN=1.E20)
       REAL*8 DX,DSUM,DR
       DIMENSION X(1)
       DATA ZERO/0.D0/, ONE/1.D0/
C
       NDAYS = 0
       NZEROS = 0
       DSUM = ZERO
C
C  EXAMINE EACH DATA VALUE;
C  IF VALUE IS ZERO, THEN ADD TO ZERO COUNT;
C  OTHERWISE ADD RECIPROCAL OF VALUE TO DSUM
       DO 10 J = 1, N
          IF (X(J) .NE. UKNOWN) THEN
             NDAYS = NDAYS + 1
           DX = X(J)
             IF (DX .EQ. ZERO) THEN
                NZEROS = NZEROS + 1
             ELSE
                DSUM = DSUM + ONE/DX
             ENDIF
          ENDIF
10     CONTINUE
C
C  COMPUTE HARMONIC MEAN FROM DSUM, ADJUSTING FOR FREQUENCY OF
C  ZERO VALUES IN X
       DR = DFLOAT(NDAYS - NZEROS)/DFLOAT(NDAYS)
       HMEAN = DFLOAT(NDAYS-NZEROS)/DSUM*DR
       RETURN
       END
C
C
C*******************************************************************
       FUNCTION FLP3(XMR,Y,NY)
C*******************************************************************
C
C  COMPUTES LOG PEARSON III QUANTILE WITH RETURN PERIOD OF XMR YEARS
C  FROM NY DATA VALUES IN VECTOR Y.
C
C  NOTE: PARAMETER NMAX ESTABLISHES MAXIMUM DIMENSION OF Y.
C
       PARAMETER (NMAX=100)
       DIMENSION Y(1), YPOS(NMAX)
C
C  FIND MEAN (U), STD. DEV. (S), AND SKEWNESS (G) OF
C  POSITIVE VALUES IN Y
       N = 0
       NN = MIN(NY,NMAX)
       DO 10 J = 1,NN
          IF (Y(J) .GT. 0.0) THEN
             N = N + 1
             YPOS(N) = ALOG(Y(J))
          END IF
10     CONTINUE
       IF (N .GT. 1) CALL STATS(YPOS,N,U,S,G)
C
C  GIVEN RETURN PERIOD (XMR), FIND FOLLOWING:
C    1. F0 = FRACTION OF RECORD WITH ZERO VALUES
C    2.  P = FRACTION OF NON-ZERO RECORD CORRESPONDING TO RETURN PERIOD
C    3. FLP3 = LOG PEARSON QUANTILE CORRESPONDING TO P
C  WHERE FNZ = FREQUENCY FACTOR FOR NORMAL DISTRIBUTION
C        FNK = FREQUENCY FACTOR FOR GAMMA DISTRIBUTION
       F0 = FLOAT(NN-N)/FLOAT(NN)
       P  = (1.0/XMR - F0)/(1.0-F0)
       FLP3 = 0.0
       IF (P .LE. 0.0) RETURN
       FLP3 = EXP(U + FNK( FNZ(P), G )*S)
       RETURN
       END
C
C
C********************************************************************
       SUBROUTINE STATS(Y,N,U,S,G)
C********************************************************************
C
C  COMPUTES MEAN (U), STANDARD DEVIATION (S), & SKEWNESS (G) OF THE
C  N VALUES IN Y
C
       REAL*8 S1,S2,S3,T,T1,T2
       DIMENSION Y(1)
C
       U = 0.0
       S = 0.0
       G = 0.0
       S1 = 0.0
       S2 = 0.0
       S3 = 0.0
       DO 10 J = 1,N
          T = Y(J)
          S1 = S1 + T
          S2 = S2 + T*T
          S3 = S3 + T*T*T
10     CONTINUE
       T = N
       U = S1/T
       T1 = S2 - S1*S1/T
       S  = DSQRT(T1/(T-1.0))
       T1 = S3 - 3.0*S2*S1/T + 2.0*(S1*S1*S1)/T/T
       T2 = (S2/T - S1*S1/T/T)**1.5
       G  = T1/T2/T
       RETURN
       END
C
C
C*********************************************************************
       FUNCTION FNZ(P)
C*********************************************************************
C
C  NORMAL FREQUENCY FACTOR CORRESPONDING TO PROBABILITY P
C  REF: JOINER & ROSENBLATT, JASA, 66:394, 1971
C
       FNZ = 4.91*( (P**0.14) - ( (1.0-P)**0.14) )
       RETURN
       END
C
C
C********************************************************************
       FUNCTION FNK(Z,G)
C********************************************************************
C
C  GAMMA FREQUENCY FACTOR CORRESPONDING TO NORMAL FREQUENCY FACTOR Z
C  AND SKEWNESS G
C  REF: WILSON & HILFERTY, PROC. NAS, 17:684-688, 1931
C
       FNK = (2.0/G)*( (1.0+(G*Z/6.0)-(G*G/36.0))**3 - 1.0)
       RETURN
       END
C
C
C********************************************************************
       SUBROUTINE MFP(A,DFLOW)
C********************************************************************
C
C  USES METHOD OF FALSE POSITION TO FIND DESIGN FLOW 'DFLOW'
C  THAT RESULTS IN NO MORE THAN 'A' EXCURSIONS
C
       DATA EPS/0.005/
C
C  SET LOWER & UPPER BOUNDS ON DESIGN FLOW
       FL = 0.0
       FU = DFLOW
C
C  COUNT EXCURSIONS AT BOUNDS
       XL = 0.0
5      CALL XCOUNT(FU,XU)
       IF (XU .LT. A) THEN
          FU = 2.*FU + 1.
          GOTO 5
       ENDIF
C
C  CHECK ON CONVERGENCE OF BOUNDS
10     IF (FU-FL .LT. EPS*FU) GOTO 40
       IF (ABS(FU-FL) .LT. 0.1) GOTO 40
       IF (ABS(A-XU) .LT. 0.005) GOTO 20
       IF (ABS(A-XL) .LT. 0.005) GOTO 40
C
C  INTERPOLATE BETWEEN BOUNDS TO FIND NEW TRIAL DESIGN FLOW
       FT = FL + (FU-FL)*(A-XL)/(XU-XL)
       CALL XCOUNT(FT,XT)
C
C  IF EXCURSIONS XT AT TRIAL FLOW FT <= TARGET A, THEN
C  MAKE FT NEW LOWER BOUND
       IF (XT .LE. A) THEN
          FL = FT
          XL = XT
C
C  ELSE MAKE FT NEW UPPER BOUND & RE-CHECK CONVERGENCE
       ELSE
          FU = FT
          XU = XT
       ENDIF
       GOTO 10
C
C  GET TO HERE IF XU = A. SET FL = FU.
20     FL = FU
C
C  SEARCH ENDS WITH FL EQUAL TO DESIGN FLOW
40     DFLOW = FL
       RETURN
       END
C
C
C*******************************************************************
       SUBROUTINE XCOUNT(F,XF)
C*******************************************************************
C
C  COUNTS NUMBER OF BIO-BASED EXCURSIONS (XF) RESULTING FROM
C  DESIGN FLOW (F)
C  SEE BLOCK DATA SUB-PROGRAM FOR DEFINITION OF VARIABLES
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
C
       EM = M
C
C  START COUNTING PROCESS WITH A FICTITIOUS EXCURSION PERIOD
       NXP = 1
       JXP1(1) = 0
       JXP2(1) = -99
C
C  EXAMINE EACH RUNNING AVERAGE IN FLOW RECORD
       DO 10 N = 1, JD2-JD1
C
C  IF HISTORICAL FLOW < DESIGN FLOW THEN DO THE FOLLOWING:
          IF (X(N) .LT. F) THEN
C
C  1) START A NEW EXCURSION PERIOD IF CURRENT DAY N IS
C     MORE THAN A DAY BEYOND END OF CURRENT PERIOD
             IF (NXP .EQ. 500) RETURN
             IF (N .GT. JXP2(NXP)+1 .OR.
     2           JXP2(NXP)-JXP1(NXP) .GE. XMI) CALL NEWXP(N)
C
C  2) INCREMENT EXCURSION COUNT IN CURRENT PERIOD
             LENGTH = MAX0(0, N+M-1-JXP2(NXP))
             XPXP(NXP) = XPXP(NXP) + FLOAT(LENGTH)/EM
C
C  3) UPDATE END OF CURRENT EXCURSION PERIOD
             IF (LENGTH .GT. 0) JXP2(NXP) = JXP2(NXP) + LENGTH
C
C  4) UPDATE SUM OF EXCURSION MAGNITUDES
             NX(NXP) = NX(NXP) + 1
             SX(NXP) = SX(NXP) + F/AMAX1(X(N),0.001)
          END IF
10     CONTINUE
C
C  SEGREGATE EXCURSION PERIODS INTO CLUSTERS (I.E., LOW-FLOW PERIODS)
C  & COUNT UP TOTAL NUMBER OF EXCURSIONS IN ALL CLUSTERS
       XF = 0.0
       NXC = 0
       IF (NXP .GT. 1) CALL CLUSTR(XF)
       RETURN
       END
C
C
C********************************************************************
       SUBROUTINE CLUSTR(XF)
C********************************************************************
C
C  SEGREGATES EXCURSION PERIODS INTO CLUSTERS (CALLED BY XCOUNT)
C  SEE BLOCK DATA SUB-PROGRAM FOR DEFINTION OF VARIABLES
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
C
C  DEFINE JSTART = START DAY OF NEXT CLUSTER
C            XMI = MAX. DURATION OF A CLUSTER
C  & SET JSTART TO NEGATIVE VALUE TO BOOTSTRAP PROCESS
       JSTART = -XMI
C
C  NOW EXAMINE EACH EXCURSION PERIOD (RECALL THAT 1ST PERIOD
C  IS A FICTITIOUS ONE)
       DO 10 J = 2,NXP
C
C  START A NEW CLUSTER IF POSSIBLE
          IF (JXP2(J) - JSTART .GT. XMI) THEN
             NXC = NXC + 1
             XPXC(NXC) = 0.0
             JSTART = JXP1(J)
             KXC1(NXC) = J
          END IF
C
C  ADD CURRENT EXCURSION PERIOD TO CLUSTER:
C     1. ADJUST END DAY OF CLUSTER,
C     2. ADD PERIOD'S EXCURSIONS TO CLUSTER'S (UP TO LIMIT = XME)
          KXC2(NXC) = J
          XPXC(NXC) = AMIN1(XME, XPXC(NXC)+XPXP(J))
10     CONTINUE
C
C  COUNT UP EXCURSIONS IN ALL CLUSTERS
       DO 20 J = 1,NXC
          XF = XF + XPXC(J)
20     CONTINUE
       RETURN
       END
C
C
C********************************************************************
       SUBROUTINE NEWXP(N)
C********************************************************************
C
C  INITIALIZES VARIABLES THAT DEFINE A NEW EXCURSION PERIOD (CALLED
C  BY XCOUNT). SEE BLOCK DATA SUB-PROGRAM FOR DEFINITIONS OF VARIABLES.
C
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
       NXP = NXP + 1
       JXP1(NXP) = N
       JXP2(NXP) = N - 1
       XPXP(NXP) = 0.0
       NX(NXP)   = 0
       SX(NXP)   = 0.0
       RETURN
       END
C
C
C*********************************************************************
       SUBROUTINE XTABLE(IOU)
C*********************************************************************
C
C  DISPLAYS AN EXCURSION TABLE FOR CURRENT DESIGN FLOW.
C  SEE BLOCK DATA SUB-PROGRAM FOR DEFINITIONS OF VARIABLES.
C
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
       DATA LMAX/22/
C
C  WRITE TABLE HEADINGS
       WRITE(IOU,101)
       LINE = 7
C
C  EXAMINE EACH EXCURSION CLUSTER (I.E., LOW-FLOW PERIOD)
       XSUM = 0.0
       IF (NXC .EQ. 0) THEN
          WRITE(IOU,105)
          RETURN
       ENDIF
       DO 20 L = 1,NXC
          IF (XPXC(L) .EQ. 0.0) GOTO 20
          XSUM = XSUM + XPXC(L)
          NLINES = KXC2(L) - KXC1(L) + 2
          IF (LINE + NLINES .GT. LMAX .AND. IOU .EQ. IO) THEN
             CALL PAUSE(IN,IO)
             LINE = 1
          ENDIF
          DO 10 J = KXC1(L), KXC2(L)
             CALL XEVENT(J,XMONTH,MDAY,MYR,MXCUR,XCD)
             IF (J .EQ. KXC1(L)) THEN
                WRITE(IOU,102) XMONTH,MDAY,MYR,XPXC(L),
     2                         XMONTH,MDAY,MYR,MXCUR,XCD
             ELSE
                WRITE(IOU,103) XMONTH,MDAY,MYR,MXCUR,XCD
             END IF
10        CONTINUE
          LINE = LINE + NLINES
20     CONTINUE
C
C  DISPLAY TOTAL NUMBER OF EXCURSIONS
       IF (LINE + 4 .GT. LMAX .AND. IOU .EQ. IO) THEN
          CALL PAUSE(IN,IO)
       ENDIF
       WRITE(IOU,104) XSUM
       RETURN
C
101    FORMAT(1X,72('-')/1X,'|',7X,'CLUSTER PERIOD',7X,'|',
     2  12X,'EXCURSION PERIODS',12X,'|'/1X,72('-')/1X,
     3  '|',17X,' NUMBER OF |',17X,'DURATION',3X,' AVERAGE %   |'
     4  /1X,'|  START DATE',5X,'EXCURSIONS |  START DATE    ',
     5  '  (DAYS)    EXCURSION *  |')
C
102    FORMAT(1X,72('-')/1X,'|  ',A3,' ',I2,', ',I4,5X,F6.2,
     2  '   |  ',A3,' ',I2,', ',I4,5X,I3,8X,F6.1,5X,'|')
C
103    FORMAT(1X,'|',28X,'|  ',A3,' ',I2,', ',I4,5X,I3,8X,F6.1,5X,'|')
C
104    FORMAT(1X,72('-')/1X,'|',6X,'TOTAL',7X,F6.2,4X,'|',41X,'|'
     2  /1X,72('-')/' * PERCENTAGE BY WHICH A CRITERION CONCENTRATION',
     3  ' WOULD BE EXCEEDED.')
C
105    FORMAT(1X,72('-')/1X,'|',13X,'THERE WERE NO EXCURSIONS',
     2 ' AT THIS DESIGN FLOW',13X,'|'/1X,72('=')/)
       END
C
C
C********************************************************************
       SUBROUTINE XEVENT(J,XMONTH,MDAY,MYR,MXCUR,XCD)
C********************************************************************
C
C  DETERMINES PARAMETERS OF EXCURSION PERIOD J.
C
C  J      = EXCURSION PERIOD INDEX
C  MDAY   = DAY OF MONTH
C  MYR    = YEAR
C  MXCUR  = DURATION IN DAYS OF EXCURSION PERIOD
C  XCD    = % EXCEEDANCE OF WQ LIMIT DURING PERIOD
C  XMONTH = MONTH OF YEAR
C  SEE BLOCK DATA SUB-PROGRAM FOR DEFINITION OF OTHER VARIABLES
C
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
C
C  FIND YR-MONTH-DAY OF START OF EXCURSION PERIOD J
       N = JD1 + JXP1(J)
       CALL DATE(N,XMONTH,MDAY,MYR)
C
C  FIND DURATION OF PERIOD & % EXCEEDANCE OF WATER QUALITY CRITERION
       MXCUR = JXP2(J) - JXP1(J) + 1
       XCD = 0.0
       IF (NX(J) .GT. 0) XCD = (SX(J)/FLOAT(NX(J))-1.0)*100.0
       RETURN
       END
C
C
C*********************************************************************
       SUBROUTINE DATE(N,XMONTH,MDAY,MYR)
C*********************************************************************
C
C  FINDS DAY-MONTH-YEAR OF JULIAN DAY N.
C
       DIMENSION YMONTH(12), NDY(12)
       DATA YMONTH/'JAN','FEB','MAR','APR','MAY','JUN',
     2             'JUL','AUG','SEP','OCT','NOV','DEC'/
       DATA NDY/0,31,59,90,120,151,181,212,243,273,304,334/
C
       MYR = (N-1)/365
       L = N - (365*MYR)
       DO 10 J = 1,12
          MONTH = 12 - J + 1
          IF (L .GT. NDY(MONTH)) GOTO 20
10     CONTINUE
       MONTH = 1
20     XMONTH = YMONTH(MONTH)
       MDAY = L - NDY(MONTH)
       MYR = 1900 + MYR
       RETURN
       END
C
C
C*******************************************************************
       BLOCK DATA
C*******************************************************************
C
C  IN      = UNIT NUMBER FOR KEYBOARD INPUT
C  IO      = UNIT NUMBER FOR CRT DISPLAY OUTPUT
C  IF      = UNIT NUMBER FOR FILE OUTPUT
C  M       = LENGTH OF AVERAGING PERIOD (DAYS)
C  XMR     = RETURN PERIOD ON EXCURSIONS (YRS)
C  XME     = MAX. EXCURSIONS PER LOW-FLOW PERIOD
C  XMI     = MAX. DAYS IN A LOW-FLOW PERIOD
C  J1      = START YEAR OF FLOW RECORD
C  J2      = END YEAR OF FLOW RECORD
C  K       = 1 FOR BIO-BASED FLOW, 2 FOR EXTREME VALUE FLOW
C
C  M1      = FIRST MONTH OF WATER YEAR
C  JD1     = JULIAN DAY JUST PRIOR TO START OF FLOW RECORD
C  JD2     = JULIAN DAY OF LAST DAY OF FLOW RECORD
C  X       = VECTOR OF DAILY FLOWS OR RUNNING AVERAGES
C
C  NXP     = NUMBER OF EXCURSION PERIODS
C  JXP1    = START DAY OF EXCURSION PERIOD
C  JXP2    = END DAY OF EXCURSION PERIOD
C  NX      = NUMBER OF EXCURSIONS WITHIN EXCURSION PERIOD
C  SX      = SUM OF MAGNITUDES OF EXCURSIONS WITHIN PERIOD
C  XPXP    = NUMBER OF EXCURSION DAYS IN AN EXCURSION PERIOD
C  NXC     = NUMBER OF EXCURSION CLUSTERS (I.E., LOW-FLOW PERIODS)
C  XPXC    = NUMBER OF EXCURSION DAYS IN A CLUSTER
C  KXC1    = EXCURSION PERIOD AT START OF CLUSTER
C  KXC2    = EXCURSION PERIOD AT END OF CLUSTER
C
       PARAMETER (UKNOWN=1.E20)
       COMMON/PARAMS/ IN,IO,IF,M,XMR,XME,XMI,J1,J2,K
       COMMON/RECORD/ M1,JD1,JD2,X(32000)
       COMMON/EXCUR/ NXP,JXP1(500),JXP2(500),NX(500),SX(500),
     2               XPXP(500),NXC,XPXC(500),KXC1(500),KXC2(500)
C
C  ENTER MISSING VALUES (1.0E20) FOR ALL DAILY FLOWS
       DATA X/32000*UKNOWN/
       DATA JD1,JD2/0,0/
C
C  MAKE APRIL THE FIRST MONTH OF WATER YEAR FOR LOW FLOW ANALYSIS
       DATA M1/4/
C
C  ASSIGN NUMBERS TO INPUT & OUTPUT DEVICES
       DATA IN,IO,IF/5,6,2/
       END

