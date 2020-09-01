namespace Pexel
{
    partial class ConvertPexelDatabase
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
            this.m_pathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_browseButton = new System.Windows.Forms.Button();
            this.m_convertButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_pathTextBox
            // 
            this.m_pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pathTextBox.Location = new System.Drawing.Point(13, 30);
            this.m_pathTextBox.Name = "m_pathTextBox";
            this.m_pathTextBox.Size = new System.Drawing.Size(515, 20);
            this.m_pathTextBox.TabIndex = 0;
            this.m_pathTextBox.Text = "new_pex_db.sqlite";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Destination";
            // 
            // m_browseButton
            // 
            this.m_browseButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.m_browseButton.AutoSize = true;
            this.m_browseButton.Location = new System.Drawing.Point(534, 28);
            this.m_browseButton.Name = "m_browseButton";
            this.m_browseButton.Size = new System.Drawing.Size(34, 23);
            this.m_browseButton.TabIndex = 2;
            this.m_browseButton.Text = "...";
            this.m_browseButton.UseVisualStyleBackColor = true;
            this.m_browseButton.Click += new System.EventHandler(this.m_browseButton_Click);
            // 
            // m_convertButton
            // 
            this.m_convertButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.m_convertButton.Location = new System.Drawing.Point(493, 57);
            this.m_convertButton.Name = "m_convertButton";
            this.m_convertButton.Size = new System.Drawing.Size(75, 23);
            this.m_convertButton.TabIndex = 3;
            this.m_convertButton.Text = "Convert";
            this.m_convertButton.UseVisualStyleBackColor = true;
            this.m_convertButton.Click += new System.EventHandler(this.m_convertButton_Click);
            // 
            // ConvertPexelDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 90);
            this.Controls.Add(this.m_convertButton);
            this.Controls.Add(this.m_browseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_pathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConvertPexelDatabase";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Convert Pexel Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_pathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_browseButton;
        private System.Windows.Forms.Button m_convertButton;
    }
}