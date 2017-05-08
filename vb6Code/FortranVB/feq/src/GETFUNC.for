C     ***********
C     *         *
C     * GET_STRING_FROM_FT
C     *         *
C     ***********

      CHARACTER*16 FUNCTION GET_STRING_FROM_FT(FTP)

C     Get a string stored in a function-table header block  and
C     return it. 

      IMPLICIT NONE
      INTEGER FTP

      INCLUDE 'arsize.prm'
      INCLUDE 'ftable.com'
      
C     Called program units
      CHARACTER*8  DP_TO_CHAR
      EXTERNAL DP_TO_CHAR

C     Local

      CHARACTER CS8*8, CS16*16
      REAL*4 SP(2)
      REAL*8 DP


      EQUIVALENCE (SP(1), DP)
C***********************************************************************
C     Get upper half of string.
      SP(1) = FTAB(FTP)
      SP(2) = FTAB(FTP+1)
      CS8 = DP_TO_CHAR(DP)
      CS16(1:8) = CS8
C     Get lower half of string
      SP(1) = FTAB(FTP+2)
      SP(2) = FTAB(FTP+3)
      CS8 = DP_TO_CHAR(DP)
      CS16(9:16) = CS8
      GET_STRING_FROM_FT = CS16
      RETURN
      END      
C     ***********
C     *         *
C     * DP_TO_CHAR
C     *         *
C     ***********

      CHARACTER*8 FUNCTION DP_TO_CHAR(DP_ARGUMENT)

C     Transfer the characters placed in the DP_ARGUMENT
C     by the function CHAR_TO_DP to a character string.

      IMPLICIT NONE

      REAL*8 DP_ARGUMENT

C     Local

      CHARACTER*8 TP
C***********************************************************************
      WRITE(TP,'(A8)')  DP_ARGUMENT
      DP_TO_CHAR = TP
      RETURN
      END
