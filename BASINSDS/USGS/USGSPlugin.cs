using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;

namespace HydrologicToolbox.USGS
{
    public class USGSPlugin : Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-PluginStoredValueDate";
        private const string AboutPanelKey = "kUSGS";
        private  DateTime _storedValue;

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
            const string SampleMenuKey = "kSample1";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "USGS"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, "Baseflow", null) { Enabled = true });
            header.Add(new SimpleActionItem(SampleMenuKey, "RECESS", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "RORA", OnMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenu1", "SWSTAT"));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu1", "Integrated Design Flow", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu1", "Frequency", OnMenuClickEventHandler));
        }

        private void OnMenuClickEventHandler(object sender, EventArgs e)
        {
            var act = ((SimpleActionItem) sender).Caption;
            //MessageBox.Show("Clicked " + act);
            switch (act)
            {
                case "Alpha":
                case "Bravo":
                    var frmAbout = new frmTest();
                    frmAbout.ShowDialog();
                    break;
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
