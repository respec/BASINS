namespace USGSHydroToolbox
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.appManager = new DotSpatial.Controls.AppManager();
            this.appDockManager = new DotSpatial.Controls.SpatialDockManager();
            this.appLegendTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.appLegend = new DotSpatial.Controls.Legend();
            this.appMap = new DotSpatial.Controls.Map();
            ((System.ComponentModel.ISupportInitialize)(this.appDockManager)).BeginInit();
            this.appDockManager.Panel1.SuspendLayout();
            this.appDockManager.Panel2.SuspendLayout();
            this.appDockManager.SuspendLayout();
            this.appLegendTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // appManager
            // 
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.DockManager = this.appDockManager;
            this.appManager.HeaderControl = null;
            this.appManager.Legend = this.appLegend;
            this.appManager.Map = this.appMap;
            this.appManager.ProgressHandler = null;
            // 
            // appDockManager
            // 
            this.appDockManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appDockManager.Location = new System.Drawing.Point(0, 0);
            this.appDockManager.Name = "appDockManager";
            // 
            // appDockManager.Panel1
            // 
            this.appDockManager.Panel1.Controls.Add(this.appLegendTab);
            // 
            // appDockManager.Panel2
            // 
            this.appDockManager.Panel2.Controls.Add(this.appMap);
            this.appDockManager.Size = new System.Drawing.Size(800, 450);
            this.appDockManager.SplitterDistance = 266;
            this.appDockManager.TabControl1 = this.appLegendTab;
            this.appDockManager.TabControl2 = null;
            this.appDockManager.TabIndex = 0;
            // 
            // appLegendTab
            // 
            this.appLegendTab.Controls.Add(this.tabPage1);
            this.appLegendTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appLegendTab.Location = new System.Drawing.Point(0, 0);
            this.appLegendTab.Name = "appLegendTab";
            this.appLegendTab.SelectedIndex = 0;
            this.appLegendTab.Size = new System.Drawing.Size(266, 450);
            this.appLegendTab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.appLegend);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(258, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Legend";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // appLegend
            // 
            this.appLegend.BackColor = System.Drawing.Color.White;
            this.appLegend.ControlRectangle = new System.Drawing.Rectangle(0, 0, 252, 418);
            this.appLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appLegend.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.appLegend.HorizontalScrollEnabled = true;
            this.appLegend.Indentation = 30;
            this.appLegend.IsInitialized = false;
            this.appLegend.Location = new System.Drawing.Point(3, 3);
            this.appLegend.MinimumSize = new System.Drawing.Size(5, 5);
            this.appLegend.Name = "appLegend";
            this.appLegend.ProgressHandler = null;
            this.appLegend.ResetOnResize = false;
            this.appLegend.SelectionFontColor = System.Drawing.Color.Black;
            this.appLegend.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.appLegend.Size = new System.Drawing.Size(252, 418);
            this.appLegend.TabIndex = 0;
            this.appLegend.Text = "appLegend";
            this.appLegend.UseLegendForSelection = true;
            this.appLegend.VerticalScrollEnabled = true;
            // 
            // appMap
            // 
            this.appMap.AllowDrop = true;
            this.appMap.BackColor = System.Drawing.Color.White;
            this.appMap.CollisionDetection = false;
            this.appMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appMap.ExtendBuffer = false;
            this.appMap.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.appMap.IsBusy = false;
            this.appMap.IsZoomedToMaxExtent = false;
            this.appMap.Legend = this.appLegend;
            this.appMap.Location = new System.Drawing.Point(0, 0);
            this.appMap.Name = "appMap";
            this.appMap.ProgressHandler = null;
            this.appMap.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.appMap.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.appMap.RedrawLayersWhileResizing = false;
            this.appMap.SelectionEnabled = true;
            this.appMap.Size = new System.Drawing.Size(530, 450);
            this.appMap.TabIndex = 0;
            this.appMap.ZoomOutFartherThanMaxExtent = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.appDockManager);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.appDockManager.Panel1.ResumeLayout(false);
            this.appDockManager.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.appDockManager)).EndInit();
            this.appDockManager.ResumeLayout(false);
            this.appLegendTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DotSpatial.Controls.AppManager appManager;
        private DotSpatial.Controls.SpatialDockManager appDockManager;
        private System.Windows.Forms.TabControl appLegendTab;
        private System.Windows.Forms.TabPage tabPage1;
        private DotSpatial.Controls.Map appMap;
        private DotSpatial.Controls.Legend appLegend;
    }
}

