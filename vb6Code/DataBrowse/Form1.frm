VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   2895
   ClientLeft      =   165
   ClientTop       =   555
   ClientWidth     =   7080
   LinkTopic       =   "Form1"
   ScaleHeight     =   2895
   ScaleWidth      =   7080
   Begin VB.PictureBox Picture1 
      Height          =   2175
      Left            =   120
      ScaleHeight     =   2115
      ScaleWidth      =   2475
      TabIndex        =   2
      Top             =   600
      Width           =   2535
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Command1"
      Height          =   495
      Left            =   1320
      TabIndex        =   1
      Top             =   0
      Width           =   1335
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Command1"
      Height          =   495
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   1335
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub Command1_Click()
  
  Dim ShapeStuff As New CShape_IO                     'object var for instance to CShape_IO
  Dim ShpfileHeader As ShapeDefines.T_MainFileHeader  'Var to hold the shape header
  Dim indexheader As ShapeDefines.T_IndexRecordHeader 'var to hold the index header
  Dim apointRec As ShapeDefines.T_shpXYPoint          'var to hold point records
  Dim alineRec As ShapeDefines.T_shpPolyLine          'var to hold polyline records
  Dim apgonRec As ShapeDefines.T_shpPolygon           'var to hold polygon records
  Dim shptype As Long                                 'holds shapetype
  Dim result As Long                                  'for boolean func returns
  Dim i As Long                                       'loop variable
  Dim count As Long                                   'used to hold the number of records
                                                      'in the shapefile
  '*********************************************************************************
  ' the following line needs to point to a valid point type shape file
  'if you want to keep the file then you need to make a copy of it
  'because the file is changed
  'point it to a valid point shape of your choice and un-comment the line
  result = ShapeStuff.ShapeFileOpen("s:\vb\atcocontrols\gis\points.shp", Readwrite)
  
  count = ShapeStuff.getRecordCount
  ShpfileHeader = ShapeStuff.getShapeHeader
  shptype = ShpfileHeader.ShapeType
   
  Debug.Print "here are all the points of the point shape file"
  For i = 1 To count
     apointRec = ShapeStuff.getXYPoint(i)
     Debug.Print apointRec.thePoint.X, " ", apointRec.thePoint.Y
  Next
  Debug.Print "*****************************************************"
  apointRec = ShapeStuff.getXYPoint(1)                        'get record #1
  Debug.Print "here is record number 1 of the point shapefile"
  Debug.Print apointRec.thePoint.X, " ", apointRec.thePoint.Y 'print the point
  Debug.Print "*****************************************************"
  apointRec.thePoint.X = apointRec.thePoint.X + 1             'add 1 to both of the values
  apointRec.thePoint.Y = apointRec.thePoint.Y + 1
  ShapeStuff.putXYPoint 1, apointRec                          'update the record
  apointRec = ShapeStuff.getXYPoint(1)                        'read the new updated point
  Debug.Print "here is record 1 after the update and has been re-read"
  Debug.Print apointRec.thePoint.X, " ", apointRec.thePoint.Y 'print the point
  ShapeStuff.FileShutDown                                     'closes the shapefiles
  
  
  '*********************************************************************************
  ' the following line needs to point to a valid polygon type shape file
  'if you want to keep the file then you need to make a copy of it
  'because the file is changed
  'point it to a valid polygon shape of your choice and un-comment the line
  result = ShapeStuff.ShapeFileOpen("s:\vb\atcocontrols\gis\cnty.shp", Readwrite)
  ShpfileHeader = ShapeStuff.getShapeHeader
  shptype = ShpfileHeader.ShapeType
  count = ShapeStuff.getRecordCount
  
  Dim Npts&
  
  Debug.Print "*****************************************************"
  Debug.Print "here are all the vertices of the 1st record of the polygon shapefile"
  apgonRec = ShapeStuff.getPolygon(1)
  If apgonRec.NumPoints - 1 < 10 Then Npts = apgonRec.NumPoints - 1 Else Npts = 10
  For i = 0 To Npts
      Debug.Print apgonRec.thePoints(i).X, " ", apgonRec.thePoints(i).Y 'print all rec points
      apgonRec.thePoints(i).X = apgonRec.thePoints(i).X + 5 'add one to each x and y point
      apgonRec.thePoints(i).Y = apgonRec.thePoints(i).Y + 5
  Next
  
  Debug.Print "*****************************************************"
  Debug.Print "here are all the vertices of the 1st record of the polygon shapefile after the update"
  ShapeStuff.putPolygon 1, apgonRec      'update the record
  apgonRec = ShapeStuff.getPolygon(1)     'get the newly created record
  For i = 0 To Npts     'print each vertex of the shape record
      Debug.Print apgonRec.thePoints(i).X, " ", apgonRec.thePoints(i).Y 'print all rec points
  Next
  ShapeStuff.FileShutDown         'close down the open shapefile
  
  Set ShapeStuff = Nothing        'get rid of the object variable
  
End Sub

Private Sub Command2_Click()
  Dim l1 As New Layer, l2 As New Layer, l3 As New Layer
  'l1.ShapeFile = "s:\vb\Basins\rf1.shp"
  'l1.Render Picture1
  
  'l2.ShapeFile = "s:\vb\Basins\cnty.shp"
  'l2.Render Picture1
  
  l3.ShapeFile = "s:\vb\Basins\points.shp"
  l3.Render Picture1

End Sub

Private Sub Form_Load()

End Sub
