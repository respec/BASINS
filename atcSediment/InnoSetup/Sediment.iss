[CustomMessages]
AppName=Sediment

[Setup]
AppName=BASINS USLE Sediment Tool
AppVerName=BASINS USLE Sediment Tool Version 1.0
AppPublisher=Clayton Engineering
AppPublisherURL=http://www.claytoneng.pro/
AppSupportURL=http://www.claytoneng.pro/
AppUpdatesURL=http://www.claytoneng.pro/
DefaultDirName=C:\Basins\bin\Plugins\Sediment
OutputBaseFilename=Sediment 1.0 Setup
SetupIconFile=c:\basins\bin\basins.ico
Compression=lzma
SolidCompression=yes
ShowLanguageDialog=no

[Types]
Name: "full"; Description: "Full installation"

[Tasks]

[Dirs]

[Files]
Source: "C:\basins\bin\plugins\sediment\*"; Excludes: "error.fil,interop.mapwin*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]

[Run]

