using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using atcData;
using atcTimeseriesScript;
using D4EMDataDownload;

namespace DataManager
{
    public class DataManagerPlugin : Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-DSDataManager";
        private const string AboutPanelKey = "kData Manager";
        private DateTime _storedValue;

        public override void Activate()
        {
            // add some menu items...
            AddMenuItems(App.HeaderControl);

            // code for saving plugin settings...
            App.SerializationManager.Serializing += ManagerSerializing;
            App.SerializationManager.Deserializing += ManagerDeserializing;

            //App.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            //App.DockManager = null;
            //App.HeaderControl = null;
            //App.Legend = null;
            //App.Map = null;
            //App.ProgressHandler = null;

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
            const string SampleMenuKey = "kDataManager";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "Data"));

            // Add some child menus
            //header.Add(new SimpleActionItem(SampleMenuKey, "Download...", null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, "Download...", OnMenuClickEventHandler) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, "Open...", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "Manage...", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "New...", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "Save In...", OnMenuClickEventHandler));

            // Add sub menus
            /*
            header.Add(new MenuContainerItem(SampleMenuKey, "submenu1", "sub1"));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu1", "sub1sub1", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu1", "sub1sub2", OnMenuClickEventHandler));
            */
        }

        private void OnMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem)sender).Caption;
            //MessageBox.Show("Clicked " + act);
            switch (act)
            {
                case "Download...":
                    D4EMDataDownload.DownloadDataPlugin.DSProject = App.SerializationManager;
                    var plugin = new D4EMDataDownload.DownloadDataPlugin();
                    plugin.Initialize(App, 0);
                    var handled = true;
                    plugin.Show("mnuDownloadDataD4EM", ref handled);
                    break;
                case "Open...":
                    var src = atcDataManager.UserSelectDataSource();
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
            }
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
                var script = new atcTimeseriesScriptPlugin();
                atcDataManager.DataPlugins.Add(script);
                var lRDB = new atcTimeseriesRDB.atcTimeseriesRDB();
                atcDataManager.DataPlugins.Add(lRDB);
                return true;
            } 
            catch (Exception e)
            {
                return false;
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