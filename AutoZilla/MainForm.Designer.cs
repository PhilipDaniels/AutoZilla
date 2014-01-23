namespace AutoZilla
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.txtPluginPath = new System.Windows.Forms.TextBox();
            this.lblPluginPath = new System.Windows.Forms.Label();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.dgvPlugins = new System.Windows.Forms.DataGridView();
            this.tabMain.SuspendLayout();
            this.tabPlugins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugins)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPlugins);
            this.tabMain.Controls.Add(this.tabDebug);
            this.tabMain.Controls.Add(this.tabAbout);
            this.tabMain.Location = new System.Drawing.Point(13, 13);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(466, 413);
            this.tabMain.TabIndex = 0;
            // 
            // tabPlugins
            // 
            this.tabPlugins.Controls.Add(this.dgvPlugins);
            this.tabPlugins.Controls.Add(this.txtPluginPath);
            this.tabPlugins.Controls.Add(this.lblPluginPath);
            this.tabPlugins.Location = new System.Drawing.Point(4, 22);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlugins.Size = new System.Drawing.Size(458, 387);
            this.tabPlugins.TabIndex = 0;
            this.tabPlugins.Text = "Plugins";
            this.tabPlugins.UseVisualStyleBackColor = true;
            // 
            // txtPluginPath
            // 
            this.txtPluginPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPluginPath.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtPluginPath.Location = new System.Drawing.Point(77, 4);
            this.txtPluginPath.Name = "txtPluginPath";
            this.txtPluginPath.ReadOnly = true;
            this.txtPluginPath.Size = new System.Drawing.Size(375, 20);
            this.txtPluginPath.TabIndex = 1;
            // 
            // lblPluginPath
            // 
            this.lblPluginPath.AutoSize = true;
            this.lblPluginPath.Location = new System.Drawing.Point(7, 7);
            this.lblPluginPath.Name = "lblPluginPath";
            this.lblPluginPath.Size = new System.Drawing.Size(64, 13);
            this.lblPluginPath.TabIndex = 0;
            this.lblPluginPath.Text = "Plugin Path:";
            // 
            // tabDebug
            // 
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(458, 387);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Size = new System.Drawing.Size(458, 387);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // dgvPlugins
            // 
            this.dgvPlugins.AllowUserToAddRows = false;
            this.dgvPlugins.AllowUserToDeleteRows = false;
            this.dgvPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlugins.Location = new System.Drawing.Point(6, 36);
            this.dgvPlugins.Name = "dgvPlugins";
            this.dgvPlugins.ReadOnly = true;
            this.dgvPlugins.Size = new System.Drawing.Size(446, 345);
            this.dgvPlugins.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 438);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AutoZilla";
            this.tabMain.ResumeLayout(false);
            this.tabPlugins.ResumeLayout(false);
            this.tabPlugins.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlugins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Label lblPluginPath;
        private System.Windows.Forms.TextBox txtPluginPath;
        private System.Windows.Forms.DataGridView dgvPlugins;

    }
}

