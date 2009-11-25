import System
System.Environment.CurrentDirectory = 'c:\Dev\BASINS40\python'

import clr
clr.AddReferenceToFile("MapWinUtility.dll")
import MapWinUtility
MapWinUtility.Logger.StartToFile("python.log")
MapWinUtility.Logger.Dbg("MapWinUtility:Colors:ClassDir " + str( dir (MapWinUtility.Colors)))
v = MapWinUtility.Colors.HSLToRGB(5,5,5)
MapWinUtility.Logger.Dbg("MessageFromPython " + str(v))

systemObjects = ['System', 'System.Drawing', 'System.Windows.Forms', 'System.XML']
for o in systemObjects:
  MapWinUtility.Logger.Dbg("Object " + o)
  clr.AddReferenceByPartialName(o)

basinsObjects = ['MapWinInterfaces', 'atcUtility', 'atcControls', 'atcData', 'atcTimeseriesMath', 'atcTimeseriesRDB', 'atcGraph']
for o in basinsObjects:
  MapWinUtility.Logger.Dbg("Object " + o)
  clr.AddReferenceToFile(o + ".dll")

import MapWindow.PluginInterfaces
MapWinUtility.Logger.Dbg("MapWindow.PluginInterfaces:ClassDir " + str( dir (MapWindow.PluginInterfaces)))

import atcUtility
MapWinUtility.Logger.Dbg("atcUtility:ClassDir " + str( dir (atcUtility)))
import atcControls
MapWinUtility.Logger.Dbg("atcControls:ClassDir " + str( dir (atcControls)))
import atcData 
MapWinUtility.Logger.Dbg("atcData:ClassDir " + str( dir (atcData)))
import atcData.atcDataSource
MapWinUtility.Logger.Dbg("atcData.atcDataSource:ClassDir " + str( dir (atcData.atcDataSource)))
import atcData.atcTimeseriesSource
MapWinUtility.Logger.Dbg("atcData.atcTimeseriesSource:ClassDir " + str( dir (atcData.atcTimeseriesSource)))

import atcTimeseriesMath
MapWinUtility.Logger.Dbg("atcTimeseriesMath:ClassDir " + str( dir (atcTimeseriesMath)))

MapWinUtility.Logger.Dbg("CreateAatcTimeseriesMath")
s = atcTimeseriesMath.atcTimeseriesMath()
MapWinUtility.Logger.Dbg("CreateDone" + s.Name)
MapWinUtility.Logger.Dbg("atcTimeseriesMath:ToString " + str(s))

MapWinUtility.Logger.Dbg("atcTimeseriesMath:AvailableOperations " + str (s.AvailableOperations.Count))

import atcData.atcTimeseries
MapWinUtility.Logger.Dbg("atcData.atcTimeseries:ClassDir " + str( dir (atcData.atcTimeseries)))
import atcData.modTimeseriesMath
MapWinUtility.Logger.Dbg("atcData.modTimeseriesMath:ClassDir " + str( dir (atcData.modTimeseriesMath)))
import atcUtility.modDate
MapWinUtility.Logger.Dbg("atcUtility.modDate:ClassDir " + str( dir (atcUtility.modDate)))

t = atcData.modTimeseriesMath.NewTimeseries(20000,20100,atcUtility.modDate.atcTimeUnit.TUDay,1)
n = t.numValues
MapWinUtility.Logger.Dbg("Timeseries.numValues " + str(n))
    



