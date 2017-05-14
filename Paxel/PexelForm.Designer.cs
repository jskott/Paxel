namespace Pexel
{
    partial class PexelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PexelForm));
            this.m_mainMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.organParameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spatialFrequencyParameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.columnConfiguratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mainListView = new Pexel.PexelForm.FlickerFreeListView();
            this.m_organProgramsLabel = new System.Windows.Forms.Label();
            this.m_exportToExcelLabel = new System.Windows.Forms.Label();
            this.m_exportToExcelList = new System.Windows.Forms.ListView();
            this.m_filterEdit = new System.Windows.Forms.TextBox();
            this.m_mainMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_mainMain
            // 
            this.m_mainMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.m_mainMain.Location = new System.Drawing.Point(0, 0);
            this.m_mainMain.Name = "m_mainMain";
            this.m_mainMain.Size = new System.Drawing.Size(934, 24);
            this.m_mainMain.TabIndex = 0;
            this.m_mainMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.organParameterToolStripMenuItem,
            this.spatialFrequencyParameterToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // organParameterToolStripMenuItem
            // 
            this.organParameterToolStripMenuItem.Name = "organParameterToolStripMenuItem";
            this.organParameterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.organParameterToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.organParameterToolStripMenuItem.Text = "Organ Parameter";
            this.organParameterToolStripMenuItem.Click += new System.EventHandler(this.organParameterToolStripMenuItem_Click);
            // 
            // spatialFrequencyParameterToolStripMenuItem
            // 
            this.spatialFrequencyParameterToolStripMenuItem.Name = "spatialFrequencyParameterToolStripMenuItem";
            this.spatialFrequencyParameterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.spatialFrequencyParameterToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.spatialFrequencyParameterToolStripMenuItem.Text = "Spatial Frequency Parameter";
            this.spatialFrequencyParameterToolStripMenuItem.Click += new System.EventHandler(this.spatialFrequencyParameterToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportToExcelToolStripMenuItem,
            this.toolStripSeparator3,
            this.columnConfiguratorToolStripMenuItem,
            this.toolStripSeparator4,
            this.reloadToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.addToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.exportToExcelToolStripMenuItem.Text = "Export to Excel";
            this.exportToExcelToolStripMenuItem.Click += new System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(225, 6);
            // 
            // columnConfiguratorToolStripMenuItem
            // 
            this.columnConfiguratorToolStripMenuItem.Name = "columnConfiguratorToolStripMenuItem";
            this.columnConfiguratorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.columnConfiguratorToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.columnConfiguratorToolStripMenuItem.Text = "Column Configurator";
            this.columnConfiguratorToolStripMenuItem.Click += new System.EventHandler(this.columnConfiguratorToolStripMenuItem_Click);
            // 
            // m_mainListView
            // 
            this.m_mainListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_mainListView.FullRowSelect = true;
            this.m_mainListView.Location = new System.Drawing.Point(12, 51);
            this.m_mainListView.Name = "m_mainListView";
            this.m_mainListView.Size = new System.Drawing.Size(910, 275);
            this.m_mainListView.TabIndex = 1;
            this.m_mainListView.UseCompatibleStateImageBehavior = false;
            this.m_mainListView.View = System.Windows.Forms.View.Details;
            this.m_mainListView.VirtualMode = true;
            this.m_mainListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnClick);
            this.m_mainListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.OnGetVirtualItem);
            this.m_mainListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseDoubleClick);
            // 
            // m_organProgramsLabel
            // 
            this.m_organProgramsLabel.AutoSize = true;
            this.m_organProgramsLabel.Location = new System.Drawing.Point(13, 32);
            this.m_organProgramsLabel.Name = "m_organProgramsLabel";
            this.m_organProgramsLabel.Size = new System.Drawing.Size(83, 13);
            this.m_organProgramsLabel.TabIndex = 2;
            this.m_organProgramsLabel.Text = "Organ Programs";
            // 
            // m_exportToExcelLabel
            // 
            this.m_exportToExcelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_exportToExcelLabel.AutoSize = true;
            this.m_exportToExcelLabel.Location = new System.Drawing.Point(13, 363);
            this.m_exportToExcelLabel.Name = "m_exportToExcelLabel";
            this.m_exportToExcelLabel.Size = new System.Drawing.Size(161, 13);
            this.m_exportToExcelLabel.TabIndex = 2;
            this.m_exportToExcelLabel.Text = "Export Organ Programs To Excel";
            // 
            // m_exportToExcelList
            // 
            this.m_exportToExcelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_exportToExcelList.FullRowSelect = true;
            this.m_exportToExcelList.Location = new System.Drawing.Point(12, 379);
            this.m_exportToExcelList.Name = "m_exportToExcelList";
            this.m_exportToExcelList.Size = new System.Drawing.Size(910, 164);
            this.m_exportToExcelList.TabIndex = 1;
            this.m_exportToExcelList.UseCompatibleStateImageBehavior = false;
            this.m_exportToExcelList.View = System.Windows.Forms.View.Details;
            this.m_exportToExcelList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnExportToExcelDoubleClick);
            // 
            // m_filterEdit
            // 
            this.m_filterEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_filterEdit.Location = new System.Drawing.Point(12, 333);
            this.m_filterEdit.Name = "m_filterEdit";
            this.m_filterEdit.Size = new System.Drawing.Size(910, 20);
            this.m_filterEdit.TabIndex = 3;
            this.m_filterEdit.TextChanged += new System.EventHandler(this.OnFilterTextChanged);
            this.m_filterEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFilterKeyDown);
            // 
            // PexelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 555);
            this.Controls.Add(this.m_filterEdit);
            this.Controls.Add(this.m_exportToExcelLabel);
            this.Controls.Add(this.m_organProgramsLabel);
            this.Controls.Add(this.m_exportToExcelList);
            this.Controls.Add(this.m_mainListView);
            this.Controls.Add(this.m_mainMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_mainMain;
            this.Name = "PexelForm";
            this.Text = "Pexel - PEX to Excel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.m_mainMain.ResumeLayout(false);
            this.m_mainMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip m_mainMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.Label m_organProgramsLabel;
        private System.Windows.Forms.Label m_exportToExcelLabel;
        private System.Windows.Forms.ListView m_exportToExcelList;
        private System.Windows.Forms.TextBox m_filterEdit;
        private FlickerFreeListView m_mainListView;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem organParameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spatialFrequencyParameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem columnConfiguratorToolStripMenuItem;
    }
}

