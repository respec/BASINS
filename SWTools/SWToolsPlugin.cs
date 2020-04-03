using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using atcDurationCompare;
using atcIDF;

namespace SWTools
{
    public class SWToolsPlugin : DotSpatial.Controls.Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-SWTools";
        private const string AboutPanelKey = "kSWTools";
        private DateTime _storedValue;

        private enum EAnalysis
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

        private string AnalysisDescription(EAnalysis ea)
        {
            switch (ea)
            {
                case EAnalysis.DC:
                    return "Duration/Compare";
                case EAnalysis.DH:
                    return "Duration Hydrograph";
                case EAnalysis.IDF:
                    return "USGS Integrated Design Flow (IDF)";
                case EAnalysis.INTERACTIVE:
                    return "Interactive";
                case EAnalysis.SWSTATBATCH:
                    return "Create SWSTAT Batch";
                case EAnalysis.DFLOWBATCH:
                    return "Create DFLOW Batch";
                case EAnalysis.RUNBATCH:
                    return "Run Existing Batch";
                default:
                    return "";
            }
        }

        public override void Activate()
        {
            // add some menu items...
            AddMenuItems(App.HeaderControl);

            // code for saving plugin settings...
            App.SerializationManager.Serializing += ManagerSerializing;
            App.SerializationManager.Deserializing += ManagerDeserializing;

            //AddDockingPane();

            base.Activate();
        }

        public override void Deactivate()
        {
            // Do not forget to unsubscribe event handlers
            App.SerializationManager.Serializing -= ManagerSerializing;
            App.SerializationManager.Deserializing -= ManagerDeserializing;

            // Remove all GUI components which were added by plugin
            App.DockManager.Remove(AboutPanelKey);
            App.HeaderControl.RemoveAll();

            base.Deactivate();
        }

        private void AddMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kSWTools";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "SW-Tools"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.DC), OnMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.DH), OnMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenuswt1", AnalysisDescription(EAnalysis.IDF)));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", AnalysisDescription(EAnalysis.INTERACTIVE), OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", AnalysisDescription(EAnalysis.SWSTATBATCH), OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", AnalysisDescription(EAnalysis.DFLOWBATCH), OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenuswt1", AnalysisDescription(EAnalysis.RUNBATCH), OnMenuClickEventHandler));
        }

        private void OnMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            if (act == AnalysisDescription(EAnalysis.DC))
            {
                var plugin = new clsDurationComparePlugin();
                plugin.Show(null);
            }
            else if (act == AnalysisDescription(EAnalysis.DH))
            {
                var plugin = new clsDurationComparePlugin();
                plugin.ShowDH(null);
            }
            else if (act == AnalysisDescription(EAnalysis.INTERACTIVE))
            {
                var plugin = new clsIDFPlugin();
                plugin.Show();
            }
            else if (act == AnalysisDescription(EAnalysis.SWSTATBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
            else if (act == AnalysisDescription(EAnalysis.DFLOWBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
            else if (act == AnalysisDescription(EAnalysis.RUNBATCH))
            {
                var plugin = new clsIDFPlugin();
            }
        }

        private void AddDockingPane()
        {
            /*
            var form = new frmTest();
            form.okButton.Click += (o, args) => App.DockManager.HidePanel(AboutPanelKey);

            var aboutPanel = new DockablePanel(AboutPanelKey, "About", form.tableLayoutPanel, DockStyle.Right);
            App.DockManager.Add(aboutPanel);
            */
        }

        private void ManagerDeserializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            _storedValue = manager.GetCustomSetting(UniqueKeyPluginStoredValueDate, DateTime.Now);
        }

        private void ManagerSerializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            manager.SetCustomSetting(UniqueKeyPluginStoredValueDate, _storedValue);
        }
    }
}
