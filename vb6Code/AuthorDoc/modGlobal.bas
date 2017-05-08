Attribute VB_Name = "modGlobal"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Global Const SourceExtension = ".txt"

'Variables mostly for conversion
Global BaseName$
Global ProjectFileName$ 'file containing list of source files
Global CurrentFilename$ 'current file in frmMain, txtMain

'Global Const RTF_START = "{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss MS Sans Serif;}}\pard\plain\fs17 "
'Global Const RTF_BOLD = "\plain\fs17\b "
'Global Const RTF_ITALIC = "\plain\fs17\i "
'Global Const RTF_UNDERLINE = "\plain\fs17\ul "
'Global Const RTF_BOLD = "\b "
'Global Const RTF_ITALIC = "\i "
'Global Const RTF_UNDERLINE = "\ul "
'
'Global Const RTF_BOLD_END = "\b0 "
'Global Const RTF_ITALIC_END = "\i0 "
'Global Const RTF_UNDERLINE_END = "\ul0 "

'Global Const RTF_PLAIN = "\plain\fs17 "
'Global Const RTF_PARAGRAPH = "\par "
'Global Const RTF_END = "}"

'Labels for popup context menu, set in frmMain Form_Load
Global CaptureNew$
Global CaptureReplace$
Global BrowseImage$
Global ViewImage$
Global SelectLink$
Global DeleteTag$

Global Keywords As Collection
Global FileKeywords As Collection
Global FindTimeout As Single
