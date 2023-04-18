using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using atcData;
using BASINS;
using DotSpatial.Data.Rasters.GdalExtension;

namespace USGSHydroToolbox
{
    public enum ETSMathOperationType
    {
        MATH,
        DATE,
        ALL
    }

    public enum ETSTool
    {
        [Description("Attributes")]
        ATTRIBUTES,
        [Description("Data Tree")]
        DATATREE,
        [Description("Events")]
        EVENTS,
        [Description("Graph")]
        GRAPH,
        [Description("List")]
        LIST,
        [Description("Trend")]
        TREND,
        [Description("Generate Time Series")]
        GTS,
        [Description("Math Functions")]
        MATH,
        [Description("Subset By Date")]
        SUBSET,
        [Description("Subset and Filter Time Series")]
        SUBSETFILTER
    }

    public enum EAnalysisGW
    {
        [Description("Base-Flow Separation")]
        BASEFLOW,
        [Description("Recharge Estimation with RORA")]
        RORA,
        [Description("Estimate Hydrograph Parameters")]
        ESTIMATEHYDROPARAM,
        [Description("RECESS")]
        RECESS,
        [Description("Two-Parameter Digital Filter")]
        TWOPARAMFILTER
    }

    public enum EAnalysisSW
    {
        [Description("Duration/Compare")]
        DC,
        [Description("Duration Hydrograph")]
        DH,
        [Description("USGS Integrated Design Flow (IDF)")]
        IDF,
        [Description("Interactive")]
        INTERACTIVE,
        [Description("Create SWSTAT Batch")]
        SWSTATBATCH,
        [Description("Create DFLOW Batch")]
        DFLOWBATCH,
        [Description("Run Existing Batch")]
        RUNBATCH
    }

    public enum EProjectAction
    {
        [Description("New HUC8 Project...")]
        NEW,
        [Description("Close")]
        CLOSE,
        [Description("Open HUC8 Project...")]
        OPEN,
        [Description("SAVE")]
        SAVE,
        [Description("Save as...")]
        SAVEAS,
        [Description("Archive")]
        ARCHIVE
    }

    public enum EHelpAction
    {
        [Description("Hydrologic Toolbox Web Page")]
        WEBPAGE,
        [Description("Hydrologic Toolbox Documentation")]
        DOC,
        [Description("Report a bug")]
        BUGREPORT,
        [Description("Welcome Screen")]
        WELCOME,
        [Description("About")]
        ABOUT
    }

    public class Utilities
    {

        public const string g_CacheDir = @"C:\USGSHydroToolbox\Cache\";
        public const string g_PathChar = @"\";
        public const string g_AppNameShort = @"Hydro Toolbox";
        public const string g_AppNameLong = @"USGS Hydrologic Toolbox";
        public static string dirtyLabel = "";

        public static atcBasinsPlugIn BASINSPlugin;

        public static atcTimeseriesMath.atcTimeseriesMath TSMath;

        public static string TSToolDescription(ETSTool ets)
        {
            switch (ets)
            {
                case ETSTool.DATATREE:
                    return "Data Tree";
                case ETSTool.ATTRIBUTES:
                    return "Attributes";
                case ETSTool.LIST:
                    return "List";
                case ETSTool.GTS:
                    return "Generate Time Series";
                case ETSTool.MATH:
                    return "Math Functions";
                case ETSTool.SUBSET:
                    return "Subset By Date";
                case ETSTool.GRAPH:
                    return "Graph";
                case ETSTool.TREND:
                    return "Trend";
                case ETSTool.EVENTS:
                    return "Events";
                case ETSTool.SUBSETFILTER:
                    return "Subset and Filter Time Series";
                default:
                    return "";
            }
        }

        public static string AnalysisDescriptionGW(EAnalysisGW ea)
        {
            switch (ea)
            {
                case EAnalysisGW.BASEFLOW:
                    return "Base-Flow Separation";
                case EAnalysisGW.RORA:
                    return "Recharge Estimation with RORA";
                case EAnalysisGW.ESTIMATEHYDROPARAM:
                    return "Estimate Hydrograph Parameters";
                case EAnalysisGW.RECESS:
                    return "RECESS";
                case EAnalysisGW.TWOPARAMFILTER:
                    return "Two-Parameter Digital Filter";
                default:
                    return "";
            }
        }

        public static string AnalysisDescriptionSW(EAnalysisSW ea)
        {
            switch (ea)
            {
                case EAnalysisSW.DC:
                    return "Duration/Compare";
                case EAnalysisSW.DH:
                    return "Duration Hydrograph";
                case EAnalysisSW.IDF:
                    return "USGS Integrated Design Flow (IDF)";
                case EAnalysisSW.INTERACTIVE:
                    return "Interactive";
                case EAnalysisSW.SWSTATBATCH:
                    return "Create SWSTAT Batch";
                case EAnalysisSW.DFLOWBATCH:
                    return "Create DFLOW Batch";
                case EAnalysisSW.RUNBATCH:
                    return "Run Existing Batch";
                default:
                    return "";
            }
        }

        public static string ProjectActionDescription(EProjectAction pa)
        {
            switch (pa)
            {
                case EProjectAction.ARCHIVE:
                    return "Archive...";
                case EProjectAction.NEW:
                    return "New HUC8 Project...";
                case EProjectAction.CLOSE:
                    return "Close";
                case EProjectAction.OPEN:
                    return "Open HUC8 Project...";
                case EProjectAction.SAVE:
                    return "Save";
                case EProjectAction.SAVEAS:
                    return "Save as...";
                default:
                    return "";
            }
        }

        public static string HelpDescription(EHelpAction ha)
        {
            switch (ha)
            {
                case EHelpAction.WEBPAGE:
                    return "Hydrologic Toolbox Web Page";
                case EHelpAction.ABOUT:
                    return "About";
                case EHelpAction.BUGREPORT:
                    return "Report a bug";
                case EHelpAction.DOC:
                    return "Hydrologic Toolbox Documentation";
                case EHelpAction.WELCOME:
                    return "Welcome Screen";
                default:
                    return "";
            }
        }

        public static List<string> TSMathOperationNames(ETSMathOperationType et)
        {
            var list = new List<string>();
            if (TSMath == null)
                TSMath = new atcTimeseriesMath.atcTimeseriesMath();

            foreach (var attr in TSMath.AvailableOperations)
            {
                string key = attr.Definition.Name;
                switch (et)
                {
                    case ETSMathOperationType.MATH:
                        if (key.Contains("Subset") || key.Contains("Merge") || key.Contains("Sum") || key.Contains("Celsius"))
                        {
                            //skip
                        } 
                        else
                        {
                            list.Add(key);
                        }

                        break;
                    case ETSMathOperationType.DATE:
                        if (key.Contains("Subset") || key.Contains("Merge"))
                            list.Add(key);
                        break;
                    default:
                        list.Add(key);
                        break;
                }
            }
            return list;
        }

        public static bool LoadDataSourcePlugins()
        {
            try
            {
                if (atcDataManager.DataSources == null)
                    atcDataManager.Clear();
                if (atcDataManager.DataPlugins.Count > 0)
                    return true;
                var att = new atcDataAttributes();
                atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared();
                //var datasources = atcDataManager.GetPlugins(typeof(atcDataSource));
                atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow.InitializeShared();
                var script = new atcTimeseriesScript.atcTimeseriesScriptPlugin();
                atcDataManager.DataPlugins.Add(script);
                var lRDB = new atcTimeseriesRDB.atcTimeseriesRDB();
                atcDataManager.DataPlugins.Add(lRDB);
                var listPlugin = new atcList.atcListPlugin();
                atcDataManager.DataPlugins.Add(listPlugin);
                var graphPlugin = new atcGraph.atcGraphPlugin();
                atcDataManager.DataPlugins.Add(graphPlugin);
                TSMath = new atcTimeseriesMath.atcTimeseriesMath();
                var saPlugin = new atcSeasonalAttributes.atcSeasonalAttributesPlugin();
                atcDataManager.DataPlugins.Add(saPlugin);
                var sPlugin = new atcSeasons.atcSeasonPlugin();
                atcDataManager.DataPlugins.Add(sPlugin);
                var bfPlugin = new atcTimeseriesBaseflow.atcTimeseriesBaseflow();
                atcDataManager.DataPlugins.Add(bfPlugin);

                var gdalRasterHandle = new DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider();
                var gdalImageHandle = new DotSpatial.Data.Rasters.GdalExtension.GdalImageProvider();
                DotSpatial.Data.DataManager.DefaultDataManager.PreferredProviders.Add("GdalImageProvider", gdalImageHandle);
                var gdalOgrHandle = new DotSpatial.Data.Rasters.GdalExtension.OgrDataProvider();
                DotSpatial.Data.DataManager.DefaultDataManager.PreferredProviders.Add("OgrDataProvider", gdalOgrHandle);
                //DotSpatial.Symbology.RasterLayer.MaxCellsInMemory = 80 * 60;

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.InnerException.Message);
                return false;
            }
        }
    }

    public class TSettings
    {
        public StringCollection RecentFiles;
    }
}
