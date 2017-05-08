C**********************************************************************
      COMMON/FTABLE/ ITMP,  FTAB, FTPNT

      INTEGER FTPNT(MFTNUM+5)

      REAL FTAB(0:MRFTAB)

      INTEGER ITMP, ITAB(0:MRFTAB)

      REAL RTMP

      EQUIVALENCE (FTAB, ITAB)      
      EQUIVALENCE (ITMP, RTMP)

      SAVE /FTABLE/
C**********************************************************************

