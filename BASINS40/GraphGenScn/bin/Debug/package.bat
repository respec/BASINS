del /Q package\*.*
rmdir package
mkdir package
"S:\dev\DotSpatial\SupportFiles\ILMerge\ILMerge.exe" /out:package\GraphGenScn.exe GraphGenScn.exe MapWinUtility.dll atcUtility.dll atcData.dll atcTimeseriesFEQ.dll atcTimeseriesStatistics.dll atcWdm.dll ZedGraph.dll
del package\GraphGenScn.pdb
copy ATCoRend.dbf package
copy feqlib.dll package
copy hass_ent.dll package
copy LF90.EER package
copy LF90WIOD.DLL package
copy GraphColors.txt package