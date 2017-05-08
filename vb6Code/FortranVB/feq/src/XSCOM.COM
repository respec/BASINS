C**********************************************************************
      INTEGER EFLAG, IN, STDOUT, OUTPUT, PROUT, LOUT

      REAL SSEPS

      REAL*8 TIME

      COMMON/XSCOM/ TIME, EFLAG, SSEPS, IN, STDOUT, OUTPUT, PROUT, LOUT
C**********************************************************************

