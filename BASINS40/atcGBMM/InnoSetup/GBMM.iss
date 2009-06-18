[CustomMessages]
AppName=GBMM

[Setup]
AppName=BASINS Grid-Based Mercury Model
AppVerName=BASINS GBMM Version 3.0
AppPublisher=Clayton Engineering
AppPublisherURL=http://www.claytoneng.pro/
AppSupportURL=http://www.claytoneng.pro/
AppUpdatesURL=http://www.claytoneng.pro/
DefaultDirName=C:\Basins\bin\Plugins\GBMM
OutputBaseFilename=GBMM 3.0 Setup
SetupIconFile=c:\basins\bin\basins.ico
Compression=lzma
SolidCompression=yes
ShowLanguageDialog=no

[Types]
Name: "full"; Description: "Full installation"

[Tasks]

[Dirs]

[Files]
Source: "C:\basins\bin\plugins\gbmm\*"; Excludes: "error.fil,interop.mapwin*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]

[Run]

