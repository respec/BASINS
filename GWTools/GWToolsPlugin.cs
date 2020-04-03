using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using atcUSGSBaseflow;
using atcUSGSRora;
using atcUSGSRecess;
using atcUSGSDF2P;

namespace GWTools
{
    public class GWToolsPlugin : Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-GWTools";
        private const string AboutPanelKey = "kGWTools";
        private DateTime _storedValue;

        private enum EAnalysis
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

        private string AnalysisDescription(EAnalysis ea)
        {
            switch (ea)
            {
                case EAnalysis.BASEFLOW:
                    return "Base-Flow Separation";
                case EAnalysis.RORA:
                    return "Recharge Estimation with RORA";
                case EAnalysis.ESTIMATEHYDROPARAM:
                    return "Estimate Hydrograph Parameters";
                case EAnalysis.RECESS:
                    return "RECESS";
                case EAnalysis.TWOPARAMFILTER:
                    return "Two-Parameter Digital Filter";
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
            const string SampleMenuKey = "kGWTools";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "GW-Tools"));

            // Add some child menus
            //header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.BASEFLOW), null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.BASEFLOW), OnMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, AnalysisDescription(EAnalysis.RORA), OnMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenugwt1", AnalysisDescription(EAnalysis.ESTIMATEHYDROPARAM)));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenugwt1", AnalysisDescription(EAnalysis.RECESS), OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenugwt1", AnalysisDescription(EAnalysis.TWOPARAMFILTER), OnMenuClickEventHandler));
        }

        private void OnMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            if (act == AnalysisDescription(EAnalysis.BASEFLOW))
            {
                var plugin = new clsUSGSBaseflowPlugin();
                plugin.Show();
            } 
            else if (act == AnalysisDescription(EAnalysis.RORA))
            {
                var plugin = new clsUSGSRoraPlugin();
                plugin.Show();
            }
            else if (act == AnalysisDescription(EAnalysis.RECESS))
            {
                var plugin = new clsUSGSRecessAnalysis();
                plugin.Show();
            }
            else if (act == AnalysisDescription(EAnalysis.TWOPARAMFILTER))
            {
                var plugin = new clsUSGSDF2PAnalysis();
                plugin.Show();
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
