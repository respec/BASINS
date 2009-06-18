[CustomMessages]
AppName=WCS

[Setup]
AppName=BASINS Watershed Characterization System
AppVerName=BASINS WCS Version 1.0
AppPublisher=Clayton Engineering
AppPublisherURL=http://www.claytoneng.pro/
AppSupportURL=http://www.claytoneng.pro/
AppUpdatesURL=http://www.claytoneng.pro/
DefaultDirName=C:\Basins\bin\Plugins\WCS
OutputBaseFilename=WCS 1.0 Setup
SetupIconFile=c:\basins\bin\basins.ico
Compression=lzma
SolidCompression=yes
ShowLanguageDialog=no

[Types]
Name: "full"; Description: "Full installation"

[Components]

[Tasks]

[Dirs]

[Files]
Source: "C:\basins\bin\plugins\wcs\*"; Excludes: "error.fil,interop.*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]

[Run]

