using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Symbology;
using System.Drawing.Drawing2D;
using atcData;
using atcUSGSRora;
using atcUSGSBaseflow;
using atcUSGSDF2P;
using atcUSGSRecess;
using atcDurationCompare;
using atcIDF;
using BASINS;


namespace USGSHydroToolbox
{
    public partial class frmMain : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl shell;
        public frmMain()
        {
            InitializeComponent();
            if (DesignMode) return;
            shell = this;
            appManager.LoadExtensions();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            /* DS Intro -------------------------------- 
            IFeatureSet fs = FeatureSet.Open(@"C:\Data\gis\utah\boundaries\Centroid.shp"); 
            IMapFeatureLayer mylayer = appMap.Layers.Add(fs);
            MakeYellowStars(mylayer);

            //it would be a lot more efficient to cast a layer to its precise geometry type
            //otherwise all operation will require casting on the generic layer object
            IMapPointLayer myPtlayer = mylayer as IMapPointLayer;
            MakeComplexSymbol(myPtlayer);

            if (!(myPtlayer == null))
            {
                //CategorizeCities(myPtlayer);
                CategorizeCitiesByAlgorithm(myPtlayer);
            }
            ------------------------------------------------ */

            if (Utilities.g_AppNameShort == "Hydro Toolbox")
            {
                this.Icon = Images.USGS;
                this.Text = Utilities.g_AppNameLong;
            }
            AddProjMenuItems(appManager.HeaderControl);
            AddDataMenuItems(appManager.HeaderControl);
            AddGeneralTSMenuItems(appManager.HeaderControl);
            AddGWMenuItems(appManager.HeaderControl);
            AddSWMenuItems(appManager.HeaderControl);
            AddHelpMenuItems(appManager.HeaderControl);
            Utilities.BASINSPlugin = new atcBasinsPlugIn();
            Utilities.BASINSPlugin.Initialize(appManager, 0);
            if (!atcMwGisUtility.GisUtilDS.MappingObjectSet())
                atcMwGisUtility.GisUtilDS.MappingObject = appManager;
            Utilities.TSMath.ShareDates = false;
            appManager.HeaderControl.Remove("kExtensions");
            //appManager.HeaderControl.Remove("kApplicationMenu");
            appManager.Map.MapFrame.LayerAdded += atcBasinsPlugIn.LayersAdded;
            appManager.Map.LayerAdded += atcBasinsPlugIn.LayersAdded;
            appManager.Map.Layers.LayerSelected += atcBasinsPlugIn.LayerSelected;
            appManager.Map.SelectionChanged += atcBasinsPlugIn.ShapesSelected;
            appManager.SerializationManager.Deserializing += Utilities.BASINSPlugin.ProjectLoadingDS;
            appManager.SerializationManager.Deserializing += UpdateUI;
            appManager.SerializationManager.Serializing += Utilities.BASINSPlugin.ProjectSavingDS;
            appManager.SerializationManager.NewProjectCreated += UpdateUI;
            appManager.SerializationManager.Serializing += UpdateUI;
            LoadRecentFiles();
        }

        private void UpdateUI(Object sender , SerializingEventArgs evt)
        {
            var projname = appManager.SerializationManager.CurrentProjectFile;
            if (!String.IsNullOrEmpty(projname))
            {
                var projid = System.IO.Path.GetFileNameWithoutExtension(projname);
                this.Text = Utilities.g_AppNameLong + " - " + projid;
            }
            else
                this.Text = Utilities.g_AppNameLong;

            //bool handled = true;
            //Utilities.BASINSPlugin.ItemClicked("mnuNew", ref handled);
        }

        private void AddDataMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kDataManager";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "Data"));

            // Add some child menus
            //header.Add(new SimpleActionItem(SampleMenuKey, "Download...", null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, "Download...", OnDataMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, "Open...", OnDataMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "Manage...", OnDataMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "New...", OnDataMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "Save In...", OnDataMenuClickEventHandler));
            //header.Add(new SimpleActionItem(SampleMenuKey, "Open Large Grid", OnDataMenuClickEventHandler));
        }

        private void OnDataMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            switch (act)
            {
                case "Download...":
                    D4EMDataDownload.DownloadDataPlugin.DSProject = appManager.SerializationManager;
                    var plugin = new D4EMDataDownload.DownloadDataPlugin();
                    plugin.Initialize(appManager, 0);
                    var handled = true;
                    plugin.Show("mnuDownloadDataD4EM", ref handled);
                    break;
                case "Open...":
                    var lFilesOnly = new System.Collections.ArrayList(1);
                    lFilesOnly.Add("File");
                    var src = atcDataManager.UserSelectDataSource(lFilesOnly);
                    if (src != null)
                        atcDataManager.OpenDataSource(src, "", null);
                    break;
                case "Manage...":
                    atcDataManager.UserManage();
                    break;
                case "New...":
                    break;
                case "Save In...":
                    break;
                case "Open Large Grid":
                    string gridfilename = @"D:\Data\gis\BigSiouxRiver_atDellRapids\Terrain\Wa21_clipped_projected.Wa21_clipped_ProjectRaster1.tif";
                    var gip = new DotSpatial.Data.Rasters.GdalExtension.GdalImageProvider();
                    //var gip = new DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider();
                    var imageData = gip.Open(gridfilename);
                    var imgLayer = new MapImageLayer(imageData);
                    //var imgLayer = new MapRasterLayer(imageData);
                    appManager.Map.Layers.Add(imgLayer);
                    //var mapgridlayer = new MapImageLayer();
                    //var symLayers = new System.Collections.Generic.List<DotSpatial.Symbology.ILayer>();
                    //ILayer imapgrid = mapgridlayer.OpenLayer(gridfilename, false, symLayers, appManager.ProgressHandler);
                    //appManager.Map.Layers.Add(imapgrid);

                    break;
            }
        }

        private void AddGWMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kGWTools";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "GW Tools"));

            // Add some child menus
            //header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.BASEFLOW), null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.AnalysisDescriptionGW(EAnalysisGW.BASEFLOW), OnGWMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.AnalysisDescriptionGW(EAnalysisGW.RORA), OnGWMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenugwt1", Utilities.AnalysisDescriptionGW(EAnalysisGW.ESTIMATEHYDROPARAM)));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenugwt1", Utilities.AnalysisDescriptionGW(EAnalysisGW.RECESS), OnGWMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenugwt1", Utilities.AnalysisDescriptionGW(EAnalysisGW.TWOPARAMFILTER), OnGWMenuClickEventHandler));
        }

        private void OnGWMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            if (act == Utilities.AnalysisDescriptionGW(EAnalysisGW.BASEFLOW))
            {
                var plugin = new clsUSGSBaseflowPlugin();
                plugin.Show();
            }
            else if (act == Utilities.AnalysisDescriptionGW(EAnalysisGW.RORA))
            {
                var plugin = new clsUSGSRoraPlugin();
                plugin.Show();
            }
            else if (act == Utilities.AnalysisDescriptionGW(EAnalysisGW.RECESS))
            {
                var plugin = new clsUSGSRecessAnalysis();
                plugin.Show();
            }
            else if (act == Utilities.AnalysisDescriptionGW(EAnalysisGW.TWOPARAMFILTER))
            {
                var plugin = new clsUSGSDF2PAnalysis();
                plugin.Show();
            }
        }

        private void AddSWMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kSWTools";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "SW Tools"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.AnalysisDescriptionSW(EAnalysisSW.DC), OnSWMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.AnalysisDescriptionSW(EAnalysisSW.DH), OnSWMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenuswt1", Utilities.AnalysisDescriptionSW(EAnalysisSW.IDF)));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", Utilities.AnalysisDescriptionSW(EAnalysisSW.INTERACTIVE), OnSWMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", Utilities.AnalysisDescriptionSW(EAnalysisSW.SWSTATBATCH), OnSWMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", Utilities.AnalysisDescriptionSW(EAnalysisSW.DFLOWBATCH), OnSWMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", Utilities.AnalysisDescriptionSW(EAnalysisSW.RUNBATCH), OnSWMenuClickEventHandler));
        }

        private void OnSWMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.DC))
            {
                var plugin = new clsDurationComparePlugin();
                plugin.Show(null);
            }
            else if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.DH))
            {
                var plugin = new clsDurationComparePlugin();
                plugin.ShowDH(null);
            }
            else if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.INTERACTIVE))
            {
                var plugin = new clsIDFPlugin();
                plugin.Show();
            }
            else if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.SWSTATBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
            else if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.DFLOWBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
            else if (act == Utilities.AnalysisDescriptionSW(EAnalysisSW.RUNBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
        }

        private void AddProjMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kProject";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "Project"));

            // Add some child menus
            //header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.BASEFLOW), null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.ProjectActionDescription(EProjectAction.NEW), OnProjMenuClickEventHandler) { Enabled = true });
            //header.Add(new SimpleActionItem(SampleMenuKey, Utilities.ProjectActionDescription(EProjectAction.OPEN), OnProjMenuClickEventHandler) { Enabled = true });
            //header.Add(new SimpleActionItem(SampleMenuKey, Utilities.ProjectActionDescription(EProjectAction.SAVE), OnProjMenuClickEventHandler));
            //header.Add(new SimpleActionItem(SampleMenuKey, Utilities.ProjectActionDescription(EProjectAction.SAVEAS), OnProjMenuClickEventHandler));
            //header.Add(new SimpleActionItem(SampleMenuKey, Utilities.ProjectActionDescription(EProjectAction.ARCHIVE), OnProjMenuClickEventHandler));
        }

        private void OnProjMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            bool handled = true;
            if (act == Utilities.ProjectActionDescription(EProjectAction.NEW))
            {
                Utilities.BASINSPlugin.ItemClicked("mnuNew", ref handled);
            }
            else if (act == Utilities.ProjectActionDescription(EProjectAction.OPEN))
            {
                Utilities.BASINSPlugin.ItemClicked("ProgramProjects_", ref handled);
                //MessageBox.Show("Use File->Open to open a project");
            }
            else if (act == Utilities.ProjectActionDescription(EProjectAction.SAVE))
            {
                MessageBox.Show("Use File->Save to save a project");
            }
            else if (act == Utilities.ProjectActionDescription(EProjectAction.SAVEAS))
            {
                MessageBox.Show("Use File-Save as to save a project to a different name");
            }
            else if (act == Utilities.ProjectActionDescription(EProjectAction.ARCHIVE))
            {
                MessageBox.Show("To be determined if archive is necessary.");
            }
        }

        private void AddHelpMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kHelp";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "Help"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.HelpDescription(EHelpAction.WEBPAGE), OnHelpMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.HelpDescription(EHelpAction.DOC), OnHelpMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.HelpDescription(EHelpAction.BUGREPORT), OnHelpMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.HelpDescription(EHelpAction.WELCOME), OnHelpMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.HelpDescription(EHelpAction.ABOUT), OnHelpMenuClickEventHandler));
        }

        private void OnHelpMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            bool handled = true;
            if (act == Utilities.HelpDescription(EHelpAction.WEBPAGE))
            {
                Utilities.BASINSPlugin.ItemClicked("ProgramWebPage", ref handled);
            }
            else if (act == Utilities.HelpDescription(EHelpAction.DOC))
            {
                Utilities.BASINSPlugin.ItemClicked("BasinsHelp", ref handled);
            }
            else if (act == Utilities.HelpDescription(EHelpAction.BUGREPORT))
            {
            }
            else if (act == Utilities.HelpDescription(EHelpAction.WELCOME))
            {
                Utilities.BASINSPlugin.Message("WELCOME_SCREEN", ref handled);
            }
            else if (act == Utilities.HelpDescription(EHelpAction.ABOUT))
            {
                Utilities.BASINSPlugin.ItemClicked("mnuAboutMapWindow", ref handled);
            }
        }

        private void AddGeneralTSMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kGTSTools";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "Time-Series Tools"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.ATTRIBUTES), OnTSMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.DATATREE), OnTSMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.EVENTS), OnTSMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.GRAPH), OnTSMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.LIST), OnTSMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.SUBSETFILTER), OnTSMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, Utilities.TSToolDescription(ETSTool.TREND), OnTSMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "tsgen", Utilities.TSToolDescription(ETSTool.GTS)));
            //header.Add(new MenuContainerItem(SampleMenuKey, "tsgen_tst1", "tst1", Utilities.TSToolDescription(ETSTool.MATH)));
            //header.Add(new MenuContainerItem(SampleMenuKey, "tsgen_tst2", "tst2", Utilities.TSToolDescription(ETSTool.SUBSET)));
            header.Add(new MenuContainerItem(SampleMenuKey, "tsgen", "tst1", Utilities.TSToolDescription(ETSTool.MATH)));
            header.Add(new MenuContainerItem(SampleMenuKey, "tsgen", "tst2", Utilities.TSToolDescription(ETSTool.SUBSET)));

            var TsMathOperations = Utilities.TSMathOperationNames(ETSMathOperationType.ALL);
            foreach (var Key in TsMathOperations)
            {
                if (Key.Contains("Subset") && Key.Contains("Filter"))
                {
                    //skip
                }
                else if (Key.Contains("Subset") || Key.Contains("Merge"))
                {
                    header.Add(new SimpleActionItem(SampleMenuKey, "tsgen_tst2", Key, OnTSMenuClickEventHandler));
                }
                else if (Key.Contains("Sum") || Key.Contains("Celsius"))
                {
                    //skip
                }
                else
                    header.Add(new SimpleActionItem(SampleMenuKey, "tsgen_tst1", Key, OnTSMenuClickEventHandler));
            }
            
            /*
            var subfilter = new SimpleActionItem(SampleMenuKey, "tsgen", Utilities.TSToolDescription(ETSTool.SUBSETFILTER), OnTSMenuClickEventHandler);
            subfilter.SmallImage = Images.Basins.ToBitmap();
            header.Add(subfilter);
            */
        }

        private void OnTSMenuClickEventHandler(object sender, EventArgs e)
        {
            //The order in which menu entries are tested is significant
            var act = ((SimpleActionItem)sender).Caption;
            if (act == Utilities.TSToolDescription(ETSTool.LIST))
            {
                var plugin = new atcList.atcListPlugin();
                plugin.Show();
            }
            else if (act == Utilities.TSToolDescription(ETSTool.ATTRIBUTES))
            {
                /*
                var plugin = new atcSeasons.atcSeasonPlugin();
                plugin.CleanUpMode = IDataManagement.ECleanUpMode.CALCULATED;
                atcDataSource lNewSource = plugin.ComputeClicked(plugin.Category + "_Attributes");
                //plugin.ItemClicked("BasinsCompute_Seasons_Attributes_Timeseries::Seasons", ref handled);
                if (lNewSource != null && lNewSource.DataSets.Count > 0) 
                {
                    ((atcSeasons.atcSeasonPlugin)lNewSource).CleanUpMode = IDataManagement.ECleanUpMode.CALCULATED;
                    string lTitle  = lNewSource.ToString();
                    atcDataManager.UserSelectDisplay(lTitle, ((atcSeasons.atcSeasonPlugin)lNewSource).DataSets);
                }
                */
                var plugin = new atcSeasonalAttributes.atcSeasonalAttributesPlugin();
                plugin.Show(null);
            }
            else if (act == Utilities.TSToolDescription(ETSTool.GRAPH))
            {
                var plugin = new atcGraph.atcGraphPlugin();
                plugin.Show();
            }
            else if (act == Utilities.TSToolDescription(ETSTool.DATATREE))
            {
                var plugin = new atcDataTree.atcDataTreePlugin();
                plugin.Show();
            }
            else if (act == Utilities.TSToolDescription(ETSTool.TREND))
            {
                var plugin = new atcIDF.clsIDFPlugin();
                plugin.ShowFunction("Trend");
            }
            else if (act.Contains("Events"))
            {
                var plugin = new atcSynopticAnalysis.atcSynopticAnalysisPlugin();
                plugin.Show();
            }
            else if (act.Contains("Subset") && act.Contains("Filter"))
            {
                atcDataManager.UserSelectDataOptions.Add("ShowFilterOption", false);
                var lform = new frmFilterData();
                atcTimeseriesGroup processed = lform.AskUser((atcTimeseriesGroup)atcDataManager.UserSelectData());
                var spec = "Split Filter";
                if (processed != null && processed.Count > 0)
                {
                    var seasonSpec = processed[0].Attributes.GetValue("SeasonDefinition", null);
                    if (seasonSpec != null)
                    {
                        spec += ": " + seasonSpec.ToString();
                    }
                    atcDataManager.RemoveDataSource(spec);
                    var processedDS = new atcSeasons.atcSeasonPlugin();
                    processedDS.Specification = spec;
                    processedDS.DataSets.AddRange(processed);
                    atcDataManager.DataSources.Add(processedDS);
                }
            }
            else if (act.Contains("Subset") || act.Contains("Merge"))
            {
                RunTSMath(Utilities.TSMath.Category + "_Date_" + act);
            }
            else if (!(act.Contains("Subset") || act.Contains("Celsius") || act.Contains("Sum") || act.Contains("Merge")))
            {
                RunTSMath(Utilities.TSMath.Category + "_Math_" + act);
            }
        }

        private void RunTSMath(string operation)
        {
            atcDataSource lNewSource = Utilities.TSMath.ComputeClicked(operation);
            if (!(lNewSource is null || lNewSource.DataSets.ToArray().Length == 0))
            {
                //create new dates to decouple from original time series
                var lTitle = lNewSource.ToString();
                var lgroup = new atcTimeseriesGroup();
                lgroup.AddRange(lNewSource.DataSets);
                atcDataManager.UserSelectDisplay(lTitle, lgroup);
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            bool handled = true;
            Utilities.BASINSPlugin.Message("WELCOME_SCREEN", ref handled);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var tsetting = new TSettings() { RecentFiles = DotSpatial.Data.Properties.Settings.Default.RecentFiles };
            var writer = new XmlSerializer(typeof(TSettings));
            string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string inifile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dir), "settings.ini");
            try
            {
                using (FileStream fs = new FileStream(inifile, FileMode.Create))
                {
                    writer.Serialize(fs, tsetting);
                }
            }
            catch (Exception ex)
            { }

            appManager.Map.MapFrame.LayerAdded -= atcBasinsPlugIn.LayersAdded;
            appManager.Map.LayerAdded -= atcBasinsPlugIn.LayersAdded;
            appManager.Map.Layers.LayerSelected -= atcBasinsPlugIn.LayerSelected;
            appManager.Map.SelectionChanged -= atcBasinsPlugIn.ShapesSelected;
            appManager.SerializationManager.Deserializing -= Utilities.BASINSPlugin.ProjectLoadingDS;
            appManager.SerializationManager.Deserializing -= UpdateUI;
            appManager.SerializationManager.Serializing -= Utilities.BASINSPlugin.ProjectSavingDS;
            appManager.SerializationManager.NewProjectCreated -= UpdateUI;
            appManager.SerializationManager.Serializing -= UpdateUI;
        }

        private void LoadRecentFiles()
        {
            string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string inifile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dir), "settings.ini");
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(inifile);
                var key = @"/TSettings/RecentFiles";
                var nodes = xmlDoc.DocumentElement.SelectNodes(key);
                foreach (XmlNode lnode in nodes[0].ChildNodes)
                {
                    DotSpatial.Data.Properties.Settings.Default.RecentFiles.Add(lnode.InnerText);
                }
            }
            catch (Exception ex)
            { }
        }

        #region "DS Intro" 
        private void MakeYellowStars(IMapFeatureLayer alayer)
        {
            alayer.Symbolizer = new PointSymbolizer(Color.Yellow, DotSpatial.Symbology.PointShape.Star, 16);
            alayer.Symbolizer.SetOutline(Color.Black, 1);
            //alayer.Symbolizer = new PointSymbolizer('A', "Freestyle Script", Color.Blue, 16);
            //alayer.Symbolizer = new PointSymbolizer(Images.wiki, 36);
        }

        private void MakeComplexSymbol(IMapPointLayer alayer)
        {   //Objective: Yellow stars in a Blue Circle 
            /* 
            Complex symbols can be created, simply by adding symbols to the Symbolizer.Symbols list. 
            There are three basic kinds of symbols, Simple, Character and Image based. 
            These have some common characteristics, like the Angle, Offset and Size, 
            which are stored on the base class. 
            In the derived classes, the characteristics that are specific to the sub‐class control 
            those aspects of symbology. 
            For creating new symbols, the Subclass can be used. 
            For working with individual symbols in the collection, you may need to test what type of 
            symbol you are working with before you will be able to control its properties. 
             */
            PointSymbolizer lPS = new PointSymbolizer(Color.Blue, DotSpatial.Symbology.PointShape.Ellipse, 16);
            lPS.Symbols.Add(new SimpleSymbol(Color.Yellow, DotSpatial.Symbology.PointShape.Star, 10));
            alayer.Symbolizer = lPS;
        }

        private void CategorizeCities(IMapPointLayer alayer)
        {
            PointScheme lScheme = new PointScheme();
            lScheme.Categories.Clear();
            PointCategory smallSize = new PointCategory(Color.Blue, DotSpatial.Symbology.PointShape.Rectangle, 4);
            smallSize.FilterExpression = "[Area] < 1e+08";
            smallSize.LegendText = "Small Cities";
            lScheme.AddCategory(smallSize);

            PointCategory largeSize = new PointCategory(Color.Yellow, DotSpatial.Symbology.PointShape.Star, 16);
            largeSize.FilterExpression = "[Area] >= 1e+08";
            largeSize.LegendText = "Large Cities";
            lScheme.AddCategory(largeSize);

            alayer.Symbology = lScheme;
        }
        private void CategorizeCitiesByAlgorithm(IMapPointLayer alayer)
        {
            /*
            There are a large number of settings that can be controlled directly using the PointScheme. 
            In this illustration the classification type is quantities, but this can also be 
            UniqueValues or custom. 
            The categories can always be edited programmatically after they are created, but this 
            simply controls what will happen when the CreateCategories method is ultimately called. 
            The interval snap methods include none, rounding, significant figures, and snapping to the 
            nearest value. These can help the appearance of the categories in the legend, 
            but it can also cause trouble. With Significant figures, the IntervalRoundingDigits controls the 
            number of significant figures instead. 
            One property is deceptive in its power. 
            The TemplateSymbolizer property allows you to control the basic appearance of the categories for 
            any property that is not being controlled by either the size or color ramping. 
            For example, if we wanted to add black borders to the stars above, we would simply add that 
            to the template symbolizer. In this case we chose to make them appear as stars and controlled 
            them to have equal sizes since UseSizeRange defaults to false, but UseColorRange defaults to true. 
             */
            PointScheme lScheme = new PointScheme();
            lScheme.Categories.Clear();
            lScheme.EditorSettings.ClassificationType = ClassificationType.Quantities;
            lScheme.EditorSettings.IntervalMethod = IntervalMethod.EqualInterval;
            lScheme.EditorSettings.IntervalSnapMethod = IntervalSnapMethod.Rounding;
            lScheme.EditorSettings.IntervalRoundingDigits = 5;
            lScheme.EditorSettings.TemplateSymbolizer = new PointSymbolizer(Color.Yellow, DotSpatial.Symbology.PointShape.Star, 16);
            lScheme.EditorSettings.FieldName = "Area";
            lScheme.CreateCategories(alayer.DataSet.DataTable);
            alayer.Symbology = lScheme;
        }

        private void SymbolizerLines(IMapFeatureLayer aLayer)
        {
            //Method 1. simple symbolizer
            aLayer.Symbolizer = new LineSymbolizer(Color.Brown, 1);

            //Method 2. Combined symbolizer
            LineSymbolizer road = new LineSymbolizer(Color.Yellow, 5);
            road.SetOutline(Color.Black, 1);
            aLayer.Symbolizer = road;

            /* Method 3. Symbology by unique values:
            HueSatLight = true, then the ramp is created by adjusting the 
            hue, saturation and lightness between the start and end colors. 
            HueSatLight = false, then the red, blue and green values are ramped instead. 
            
            In both cases, alpha (transparency) is ramped the same way. 
             */
            LineScheme lScheme = new LineScheme();
            lScheme.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
            lScheme.EditorSettings.FieldName = "CARTO";
            lScheme.CreateCategories(aLayer.DataSet.DataTable);
            aLayer.Symbology = lScheme;

            //Method 4. Collapsible field name in legend via 'AppearsInLegend'
            LineScheme lScheme1 = new LineScheme();
            lScheme1.Categories.Clear(); //redundant???
            LineCategory lowCat = new LineCategory(Color.Blue, 2);
            lowCat.FilterExpression = "[CARTO] = 3";
            lowCat.LegendText = "Low";
            LineCategory highCat = new LineCategory(
                Color.Red, 
                Color.Black, 
                6, 
                DashStyle.Solid, 
                LineCap.Triangle); ;
            highCat.FilterExpression = "[CARTO] = 2";
            highCat.LegendText = "High";
            lScheme1.AppearsInLegend = true;
            lScheme1.LegendText = "CARTO";
            lScheme1.Categories.Add(lowCat);
            aLayer.Symbology = lScheme1; 
            lScheme1.Categories.Add(highCat);

            /*Method 5. Lines with multiple strokes
            Each individual LineSymbolizer is made up of at least one, 
            but potentially several strokes overlapping each other
            */
            LineSymbolizer multiStrokeSym = new LineSymbolizer();
            multiStrokeSym.Strokes.Clear(); //redundant???
            CartographicStroke ties = new CartographicStroke(Color.Brown);
            ties.DashPattern = new float[] { 1 / 6f, 2 / 6f };
            ties.Width = 6;
            ties.EndCap = LineCap.Flat;
            ties.StartCap = LineCap.Flat;
            CartographicStroke rails = new CartographicStroke(Color.DarkGray);
            rails.CompoundArray = new float[] { .15f, .3f, .6f, .75f };
            rails.Width = 6;
            rails.EndCap = LineCap.Flat;
            rails.StartCap = LineCap.Flat;
            multiStrokeSym.Strokes.Add(ties);
            multiStrokeSym.Strokes.Add(rails);
            aLayer.Symbolizer = multiStrokeSym;
        }
        #endregion
    }
}
