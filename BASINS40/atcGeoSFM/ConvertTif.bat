@echo Setting environment for using the GDAL and MapServer tools.

set ocipath=0
set _path="%PATH:;=" "%"
for %%p in (%_path%) do if not "%%~p"=="" if exist %%~p\oci.dll set ocipath=1


:setenv2
SET GD=C:\gdal
SET PATH=%GD%\bin;%GD%\bin\gdal\python\osgeo;%GD%\bin\proj\apps;%GD%\bin\gdal\apps;%GD%\bin\ms\apps;%GD%\bin\gdal\csharp;%GD%\bin\ms\csharp;%GD%\bin\curl;%PATH%
SET GDAL_DATA=%GD%\bin\gdal-data
SET GDAL_DRIVER_PATH=%GD%\bin\gdal\plugins
SET PYTHONPATH=%GD%\bin\gdal\python\osgeo
SET PROJ_LIB=%GD%\bin\proj\SHARE

