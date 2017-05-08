@REM Compile GETFUNC.FOR (no object file)
@lf90 -ap -chk -g -co -f90 -lst -pca -sav -stchk -trace -w -wo -xref -win -c GETFUNC.FOR

@REM Compile LOOKUP.FOR (no object file)
@lf90 -ap -chk -g -co -f90 -lst -pca -sav -stchk -trace -w -wo -xref -win -c LOOKUP.FOR

@REM Relink C:\F90Apps\feq\src\FeqLib.exe (Target file does not exist)
@amrsp--f
@lf90 @AUTOMAKE.RSP -exe C:\F90Apps\feq\src\FeqLib.exe -fullwarn -g -win
:QUIT
