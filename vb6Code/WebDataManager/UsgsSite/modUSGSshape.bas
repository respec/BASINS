Attribute VB_Name = "modUSGSshape"
Option Explicit

Public Enum gage_field
  AREA = 1
  PERIMETER = 2
  GAGE_ = 3
  GAGE_ID = 4
  AGCY = 5
  STCO = 6
  LATDD = 7
  LONGDD = 8
  REACH = 9
  Name = 10
  MNFLO = 11
  SVTEN = 12
  JAN = 13
  FEB = 14
  MAR = 15
  APR = 16
  MAY = 17
  JUN = 18
  JUL = 19
  AUG = 20
  SEP = 21
  Oct = 22
  NOV = 23
  DEC = 24
  ACCURACY = 25
  BREACH = 26
  BFIPS = 27
End Enum

Public Function NewGageDBF(Optional aNumRecords As Long = 0) As clsDBF
  Set NewGageDBF = New clsDBF
  With NewGageDBF
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CInt(Format(Now, "mm"))
    .Day = CInt(Format(Now, "dd"))

    .NumFields = 27

    .FieldName(1) = "AREA       "
    .FieldType(1) = "N"
    .FieldLength(1) = 12
    .FieldDecimalCount(1) = 3

    .FieldName(2) = "PERIMETER  "
    .FieldType(2) = "N"
    .FieldLength(2) = 12
    .FieldDecimalCount(2) = 3

    .FieldName(3) = "GAGE_      "
    .FieldType(3) = "N"
    .FieldLength(3) = 10
    .FieldDecimalCount(3) = 0

    .FieldName(4) = "GAGE_ID    "
    .FieldType(4) = "N"
    .FieldLength(4) = 10
    .FieldDecimalCount(4) = 0

    .FieldName(5) = "AGCY       "
    .FieldType(5) = "C"
    .FieldLength(5) = 20
    .FieldDecimalCount(5) = 0

    .FieldName(6) = "STCO       "
    .FieldType(6) = "C"
    .FieldLength(6) = 5
    .FieldDecimalCount(6) = 0

    .FieldName(7) = "LATDD      "
    .FieldType(7) = "N"
    .FieldLength(7) = 8
    .FieldDecimalCount(7) = 4

    .FieldName(8) = "LONGDD     "
    .FieldType(8) = "N"
    .FieldLength(8) = 8
    .FieldDecimalCount(8) = 4

    .FieldName(9) = "REACH      "
    .FieldType(9) = "C"
    .FieldLength(9) = 11
    .FieldDecimalCount(9) = 0

    .FieldName(10) = "NAME       "
    .FieldType(10) = "C"
    .FieldLength(10) = 20
    .FieldDecimalCount(10) = 0

    .FieldName(11) = "MNFLO      "
    .FieldType(11) = "C"
    .FieldLength(11) = 7
    .FieldDecimalCount(11) = 0

    .FieldName(12) = "SVTEN      "
    .FieldType(12) = "C"
    .FieldLength(12) = 7
    .FieldDecimalCount(12) = 0

    .FieldName(13) = "JAN        "
    .FieldType(13) = "C"
    .FieldLength(13) = 7
    .FieldDecimalCount(13) = 0

    .FieldName(14) = "FEB        "
    .FieldType(14) = "C"
    .FieldLength(14) = 7
    .FieldDecimalCount(14) = 0

    .FieldName(15) = "MAR        "
    .FieldType(15) = "C"
    .FieldLength(15) = 7
    .FieldDecimalCount(15) = 0

    .FieldName(16) = "APR        "
    .FieldType(16) = "C"
    .FieldLength(16) = 7
    .FieldDecimalCount(16) = 0

    .FieldName(17) = "MAY        "
    .FieldType(17) = "C"
    .FieldLength(17) = 7
    .FieldDecimalCount(17) = 0

    .FieldName(18) = "JUN        "
    .FieldType(18) = "C"
    .FieldLength(18) = 7
    .FieldDecimalCount(18) = 0

    .FieldName(19) = "JUL        "
    .FieldType(19) = "C"
    .FieldLength(19) = 7
    .FieldDecimalCount(19) = 0

    .FieldName(20) = "AUG        "
    .FieldType(20) = "C"
    .FieldLength(20) = 7
    .FieldDecimalCount(20) = 0

    .FieldName(21) = "SEP        "
    .FieldType(21) = "C"
    .FieldLength(21) = 7
    .FieldDecimalCount(21) = 0

    .FieldName(22) = "OCT        "
    .FieldType(22) = "C"
    .FieldLength(22) = 7
    .FieldDecimalCount(22) = 0

    .FieldName(23) = "NOV        "
    .FieldType(23) = "C"
    .FieldLength(23) = 7
    .FieldDecimalCount(23) = 0

    .FieldName(24) = "DEC        "
    .FieldType(24) = "C"
    .FieldLength(24) = 7
    .FieldDecimalCount(24) = 0

    .FieldName(25) = "ACCURACY   "
    .FieldType(25) = "N"
    .FieldLength(25) = 1
    .FieldDecimalCount(25) = 0

    .FieldName(26) = "BREACH     "
    .FieldType(26) = "C"
    .FieldLength(26) = 11
    .FieldDecimalCount(26) = 0

    .FieldName(27) = "BFIPS      "
    .FieldType(27) = "C"
    .FieldLength(27) = 5
    .FieldDecimalCount(27) = 0

    .NumRecords = aNumRecords
    .InitData
  End With
End Function

