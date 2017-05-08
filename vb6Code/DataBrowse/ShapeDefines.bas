Attribute VB_Name = "ShapeDefines"
Option Explicit
'*****************************************************************************************
'ShapeDefines
'
'Contains all of the user defined types for the valid shape file types



'*****************************************************************************************
'Main file header stucture for both the shape and index files
Public Type T_MainFileHeader
    FileCode As Long        'big
    u1 As Long              'big
    u2 As Long              'big
    u3 As Long              'big
    u4 As Long              'big
    u5 As Long              'big
    FileLength As Long      'big
    version As Long         'Little
    ShapeType As Long       'Little
    BndBoxXmin As Double    'Little
    BndBoxYmin As Double    'Little
    BndBoxXmax As Double    'Little
    BndBoxYmax As Double    'Little
    BndBoxZmin As Double    'Little
    BndBoxZmax As Double    'Little
    BndBoxMmin As Double    'Little
    BndBoxMmax As Double    'Little
End Type

'*****************************************************************************************
'Shape file record header structure
Public Type T_RecordHeader
    RecordNumber As Long    'big
    ContentLength As Long   'big
End Type

'*****************************************************************************************
'index file record structure
Public Type T_IndexRecordHeader
    offset As Long              'big
    ContentLength As Long       'big
End Type

'*****************************************************************************************
'Structure for the null shape
Public Type T_NullShape
    ShapeType As Long
End Type

'*****************************************************************************************
' Basic point structure for standard point line and polygon types
Public Type T_BasicPoint
    x As Double
    y As Double
End Type
'*****************************************************************************************
'structure for the bounding box
Public Type T_BoundingBox
    xMin As Double
    yMin As Double
    xMax As Double
    yMax As Double
End Type

'*****************************************************************************************
'structure for the XYPoint shapetype
Public Type T_shpXYPoint
    ShapeType As Long
    thePoint As T_BasicPoint
End Type

'*****************************************************************************************
'structure for the XYMultiPoint shapetype
Public Type T_shpXYMultiPoint
    ShapeType As Long           'Little
    Box As T_BoundingBox          'Little
    NumPoints As Long           'Little
    thePoints() As T_BasicPoint   'Little    array of NumPoints
End Type

'*****************************************************************************************
'structure for the polyline shapetype
Public Type T_shpPolyLine
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    thePoints() As T_BasicPoint
End Type

'*****************************************************************************************
'structure for the polygon shapetype
Public Type T_shpPolygon
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    thePoints() As T_BasicPoint
End Type

'*****************************************************************************************
'structure for the pointM shapetype
Public Type T_shpPointM
    ShapeType As Long
    point As T_BasicPoint
    m As Double
End Type

'*****************************************************************************************
'structure for the measure range
Public Type T_MRange
    mMin As Double
    mMax As Double
End Type

'*****************************************************************************************
'structure for the MultiPointM shapetype
Public Type T_shpMultiPointM
    ShapeType As Long
    Box As T_BoundingBox
    NumPoints As Long
    thePoints() As T_BasicPoint
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the polylineM shapetype
Public Type T_shpPolyLineM
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    thePoints() As T_BasicPoint
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the polygonM shapetype
Public Type T_shpPolygonM
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    thePoints() As Long
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the Z Range
Public Type T_ZRange
    zMin As Double
    zMax As Double
End Type

'*****************************************************************************************
'structure for the pointZ shapetype
Public Type T_shpPointZ
    ShapeType As Long
    thePoint As T_BasicPoint
    Z As Double
    m As Double
End Type

'*****************************************************************************************
'structure for the MultiPointZ shapetype
Public Type T_shpMultiPointZ
    ShapeType As Long
    Box As T_BoundingBox
    NumPoints As Long
    thePoints() As T_BasicPoint
    ZRange As T_ZRange
    ZArray() As Double
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the polylineZ shapetype
Public Type T_shpPolyLineZ
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    thePoints() As T_BasicPoint
    ZRange As T_ZRange
    ZArray() As Double
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the polygonZ shapetype
Public Type T_shpPolygonZ
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    part() As Long
    thePoints() As T_BasicPoint
    ZRange As T_ZRange
    ZArray() As Double
    MRange As T_MRange
    MArray() As Double
End Type

'*****************************************************************************************
'structure for the MultiPatch shapetype
Public Type T_shpMultiPatch
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long
    NumPoints As Long
    Parts() As Long
    PartTypes() As Long
    thePoints() As T_BasicPoint
    ZRange As T_ZRange
    ZArray() As Double
    MRange As T_MRange
    MArray() As Double
End Type

