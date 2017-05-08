Attribute VB_Name = "HIMUtil"
Option Explicit

Public Declare Function CloseFile Lib "HIMUtil.dll" (unitnum As Long) As Long

Declare Function Read_All_MODFLOW_Unf_Heads Lib "HIMUtil.dll" (xArray!, nCol&, nRow&, layno&, _
                                                             infSource&, infType&, infUnit&, ByVal infName$, _
                                                             outfType&, outfUnit&, ByVal outfName$, _
                                                             ByVal len_infName As Integer, ByVal len_outfName As Integer) _
                                                             As Long
Declare Function Read_MODFLOW_Unformatted Lib "HIMUtil.dll" (xArray!, nCol&, nRow&, layno&, _
                                                             infSource&, infType&, infUnit&, ByVal infName$, _
                                                             outfType&, outfUnit&, ByVal outfName$, _
                                                             ByVal len_infName As Integer, ByVal len_outfName As Integer) _
                                                             As Long
Declare Function Read_MODFLOW_List Lib "HIMUtil.dll" (xArray!, layArray!, colArray!, rowArray!, _
                                                      nCol&, nRow&, nCells&, _
                                                      infType&, infUnit&, ByVal infName$, _
                                                      outfType&, outfUnit&, ByVal outfName$, _
                                                      ByVal len_infName As Integer, ByVal len_outfName As Integer) _
                                                      As Long
Declare Function Read_Integer_Array Lib "HIMUtil.dll" (IA&, ByVal ANAME As String, II&, JJ&, K&, _
                                                       infUnit&, ByVal infName As String, _
                                                       ByVal len_ANAME As Integer, ByVal len_infName As Integer) As Long
Declare Function Read_Real_Array Lib "HIMUtil.dll" (a!, ByVal ANAME As String, II&, JJ&, K&, _
                                                    infUnit&, ByVal infName As String, _
                                                    ByVal len_ANAME As Integer, ByVal len_infName As Integer) As Long
Declare Function Write_Integer_Array Lib "HIMUtil.dll" (iArray&, ByVal TEXT As String, nCol&, nRow&, ILAY&, _
                                                        outfUnit&, outfType&, ByVal outfName As String, _
                                                        IHEAD&, headerUnit&, _
                                                        ByVal len_TEXT As Integer, ByVal len_outfName As Integer)
Declare Function Write_Real_Array Lib "HIMUtil.dll" (rArray!, ByVal TEXT As String, nCol&, nRow&, ILAY&, _
                                                     outfUnit, outfType, ByVal outfName As String, _
                                                     IHEAD&, headerUnit&, _
                                                     ByVal len_TEXT As Integer, ByVal len_outfName As Integer)
Declare Function Write_Binout Lib "HIMUtil.dll" (ByVal Filename As String, Nval&, Values!, NameLen&, _
                                                 ByVal NameString As String, ByVal Opname As String, OpID&, _
                                                 ByVal Section As String, UnitFlag&, PrintLevel&, CurDate&, _
                                                 First As Long, len_Filename As Integer, _
                                                 ByVal len_Namelist As Integer, ByVal len_Opname As Integer, _
                                                 ByVal len_Section As Integer) As Long


