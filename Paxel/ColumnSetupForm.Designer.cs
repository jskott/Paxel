namespace Pexel
{
    partial class ColumnSetupForm
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
            this.m_tableComboBox = new System.Windows.Forms.ComboBox();
            this.m_availableColumns = new System.Windows.Forms.ListView();
            this.m_availableColumnaNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_configurationNameTextBox = new System.Windows.Forms.TextBox();
            this.m_configurationNameLabel = new System.Windows.Forms.Label();
            this.m_tableLabel = new System.Windows.Forms.Label();
            this.m_selectedColumnsList = new System.Windows.Forms.ListView();
            this.m_selectedColumnsHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_addColumnButton = new System.Windows.Forms.Button();
            this.m_removeColumnButton = new System.Windows.Forms.Button();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_okButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tableComboBox
            // 
            this.m_tableComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_tableComboBox.FormattingEnabled = true;
            this.m_tableComboBox.Location = new System.Drawing.Point(13, 62);
            this.m_tableComboBox.Name = "m_tableComboBox";
            this.m_tableComboBox.Size = new System.Drawing.Size(764, 21);
            this.m_tableComboBox.TabIndex = 0;
            this.m_tableComboBox.SelectedValueChanged += new System.EventHandler(this.OnTableComboSelectionChanged);
            // 
            // m_availableColumns
            // 
            this.m_availableColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_availableColumnaNameHeader});
            this.m_availableColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_availableColumns.LabelEdit = true;
            this.m_availableColumns.Location = new System.Drawing.Point(0, 0);
            this.m_availableColumns.Margin = new System.Windows.Forms.Padding(0);
            this.m_availableColumns.Name = "m_availableColumns";
            this.m_availableColumns.Size = new System.Drawing.Size(369, 514);
            this.m_availableColumns.TabIndex = 1;
            this.m_availableColumns.UseCompatibleStateImageBehavior = false;
            this.m_availableColumns.View = System.Windows.Forms.View.Details;
            this.m_availableColumns.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.OnAfterLabelEdit);
            this.m_availableColumns.SelectedIndexChanged += new System.EventHandler(this.OnAvailableColumnsSelectionChanged);
            this.m_availableColumns.DoubleClick += new System.EventHandler(this.OnAvailableColumnsDblClicked);
            // 
            // m_availableColumnaNameHeader
            // 
            this.m_availableColumnaNameHeader.Text = "Available Columns";
            this.m_availableColumnaNameHeader.Width = 222;
            // 
            // m_configurationNameTextBox
            // 
            this.m_configurationNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_configurationNameTextBox.Location = new System.Drawing.Point(13, 25);
            this.m_configurationNameTextBox.Name = "m_configurationNameTextBox";
            this.m_configurationNameTextBox.Size = new System.Drawing.Size(764, 20);
            this.m_configurationNameTextBox.TabIndex = 2;
            this.m_configurationNameTextBox.TextChanged += new System.EventHandler(this.OnConfigurationNameChanged);
            // 
            // m_configurationNameLabel
            // 
            this.m_configurationNameLabel.AutoSize = true;
            this.m_configurationNameLabel.Location = new System.Drawing.Point(13, 10);
            this.m_configurationNameLabel.Name = "m_configurationNameLabel";
            this.m_configurationNameLabel.Size = new System.Drawing.Size(103, 13);
            this.m_configurationNameLabel.TabIndex = 3;
            this.m_configurationNameLabel.Text = "Configuration Name:";
            // 
            // m_tableLabel
            // 
            this.m_tableLabel.AutoSize = true;
            this.m_tableLabel.Location = new System.Drawing.Point(13, 47);
            this.m_tableLabel.Name = "m_tableLabel";
            this.m_tableLabel.Size = new System.Drawing.Size(37, 13);
            this.m_tableLabel.TabIndex = 4;
            this.m_tableLabel.Text = "Table:";
            // 
            // m_selectedColumnsList
            // 
            this.m_selectedColumnsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_selectedColumnsHeader});
            this.m_selectedColumnsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_selectedColumnsList.LabelEdit = true;
            this.m_selectedColumnsList.Location = new System.Drawing.Point(397, 0);
            this.m_selectedColumnsList.Margin = new System.Windows.Forms.Padding(0);
            this.m_selectedColumnsList.Name = "m_selectedColumnsList";
            this.m_selectedColumnsList.Size = new System.Drawing.Size(370, 514);
            this.m_selectedColumnsList.TabIndex = 5;
            this.m_selectedColumnsList.UseCompatibleStateImageBehavior = false;
            this.m_selectedColumnsList.View = System.Windows.Forms.View.Details;
            this.m_selectedColumnsList.SelectedIndexChanged += new System.EventHandler(this.OnSelectedColumnsSelectionChanged);
            this.m_selectedColumnsList.DoubleClick += new System.EventHandler(this.OnSelectedColumnDblClicked);
            // 
            // m_selectedColumnsHeader
            // 
            this.m_selectedColumnsHeader.Text = "Selected Columns";
            this.m_selectedColumnsHeader.Width = 357;
            // 
            // m_addColumnButton
            // 
            this.m_addColumnButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_addColumnButton.Location = new System.Drawing.Point(0, 223);
            this.m_addColumnButton.Margin = new System.Windows.Forms.Padding(0);
            this.m_addColumnButton.Name = "m_addColumnButton";
            this.m_addColumnButton.Size = new System.Drawing.Size(22, 28);
            this.m_addColumnButton.TabIndex = 6;
            this.m_addColumnButton.Text = ">";
            this.m_addColumnButton.UseVisualStyleBackColor = true;
            this.m_addColumnButton.Click += new System.EventHandler(this.m_addColumnButton_Click);
            // 
            // m_removeColumnButton
            // 
            this.m_removeColumnButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_removeColumnButton.Location = new System.Drawing.Point(0, 251);
            this.m_removeColumnButton.Margin = new System.Windows.Forms.Padding(0);
            this.m_removeColumnButton.Name = "m_removeColumnButton";
            this.m_removeColumnButton.Size = new System.Drawing.Size(22, 28);
            this.m_removeColumnButton.TabIndex = 7;
            this.m_removeColumnButton.Text = "<";
            this.m_removeColumnButton.UseVisualStyleBackColor = true;
            this.m_removeColumnButton.Click += new System.EventHandler(this.m_removeColumnButton_Click);
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cancelButton.Location = new System.Drawing.Point(702, 624);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_cancelButton.TabIndex = 8;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new System.EventHandler(this.m_cancelButton_Click);
            // 
            // m_okButton
            // 
            this.m_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_okButton.Location = new System.Drawing.Point(621, 624);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new System.Drawing.Size(75, 23);
            this.m_okButton.TabIndex = 9;
            this.m_okButton.Text = "OK";
            this.m_okButton.UseVisualStyleBackColor = true;
            this.m_okButton.Click += new System.EventHandler(this.m_okButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.m_selectedColumnsList, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_availableColumns, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 89);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 514);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.m_addColumnButton, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.m_removeColumnButton, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(372, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(22, 502);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // ColumnSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 659);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.m_okButton);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_tableLabel);
            this.Controls.Add(this.m_configurationNameLabel);
            this.Controls.Add(this.m_configurationNameTextBox);
            this.Controls.Add(this.m_tableComboBox);
            this.Name = "ColumnSetupForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Column Selector";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_tableComboBox;
        private System.Windows.Forms.ListView m_availableColumns;
        private System.Windows.Forms.ColumnHeader m_availableColumnaNameHeader;
        private System.Windows.Forms.TextBox m_configurationNameTextBox;
        private System.Windows.Forms.Label m_configurationNameLabel;
        private System.Windows.Forms.Label m_tableLabel;
        private System.Windows.Forms.ListView m_selectedColumnsList;
        private System.Windows.Forms.ColumnHeader m_selectedColumnsHeader;
        private System.Windows.Forms.Button m_addColumnButton;
        private System.Windows.Forms.Button m_removeColumnButton;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.Button m_okButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}