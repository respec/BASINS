
namespace BatchSW
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using atcIDF;
    using atcBatchProcessing;
    using atcData;
    

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                usage();
                return;
            }
            bool silentconsole = false;
            var specfilepath = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/s" || args[i] == "/S")
                {
                    silentconsole = true;
                }
                if (args[i] == "/f" || args[i] == "/F")
                {
                    if (i + 1 < args.Length && string.IsNullOrEmpty(specfilepath))
                    {
                        specfilepath = args[i + 1];
                    }
                }
            }
            if (string.IsNullOrEmpty(specfilepath))
            {
                Console.WriteLine("Must provide batch run spec file path.");
                return;
            }
            
            //Setup the run
            var pBatchConfig = new atcIDF.clsBatchSpec();
            if (! System.IO.Path.IsPathRooted(specfilepath))
            {
                Console.WriteLine("Please provide a spec file with a valid file path.");
                return;
            }
            LoadDataSourcePlugins();
            pBatchConfig.RunModeConsole = true;
            pBatchConfig.SilentConsole = silentconsole;
            pBatchConfig.SpecFilename = specfilepath;
            pBatchConfig.PopulateScenarios();
            //Application.DoEvents();
            pBatchConfig.DoBatch();
        }

        static void usage()
        {
            Console.WriteLine("Surface Water Analysis Batch Run.\n\n");
            Console.WriteLine("BatchSW [/S] /F SpecFileFullPath\n");
            Console.WriteLine("[/S]                   Silent mode switch. Verbose if not specified.\n");
            Console.WriteLine("/F SpecFileFullPath    Must specify batch run spec file full path.\n");
        }

        public static bool LoadDataSourcePlugins()
        {
            try
            {
                if (atcDataManager.DataSources is null)
                    atcDataManager.Clear();
                if (atcDataManager.DataPlugins.Count > 0)
                    return true;
                var att = new atcDataAttributes();
                atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared();
                //var datasources = atcDataManager.GetPlugins(typeof(atcDataSource));
                atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow.InitializeShared();
                var lRDB = new atcTimeseriesRDB.atcTimeseriesRDB();
                atcDataManager.DataPlugins.Add(lRDB);
                var listPlugin = new atcList.atcListPlugin();
                atcDataManager.DataPlugins.Add(listPlugin);
                var graphPlugin = new atcGraph.atcGraphPlugin();
                atcDataManager.DataPlugins.Add(graphPlugin);
                var TSMath = new atcTimeseriesMath.atcTimeseriesMath();
                var saPlugin = new atcSeasonalAttributes.atcSeasonalAttributesPlugin();
                atcDataManager.DataPlugins.Add(saPlugin);
                var sPlugin = new atcSeasons.atcSeasonPlugin();
                atcDataManager.DataPlugins.Add(sPlugin);
                var bfPlugin = new atcTimeseriesBaseflow.atcTimeseriesBaseflow();
                atcDataManager.DataPlugins.Add(bfPlugin);

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.InnerException.Message);
                return false;
            }
        }
    }
}
