Attribute VB_Name = "modGlobal"
Option Explicit

Global Const FileListFilename = "FileList.txt"
Global Const SourceExtension = ".txt"

Global Const RTF_START = "{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss MS Sans Serif;}}{\colortbl ;\red0\green0\blue255;}\pard\plain\fs17 "
Global Const RTF_BOLD = "\plain\fs17\b "
Global Const RTF_ITALIC = "\plain\fs17\i "
Global Const RTF_UNDERLINE = "\plain\fs17\ul "
Global Const RTF_PLAIN = "\plain\fs17 "
Global Const RTF_HIGHLIGHT = "\plain\fs17\cb3 "
Global Const RTF_PARAGRAPH = "\par "
Global Const RTF_END = "}"

Global TopItem As clsVBitem
Global AllItems As Collection
Global IndentLen As Long
Global SourceComment As String
