namespace AutoZilla.Plugins.MSSQL
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
            this.components = new System.ComponentModel.Container();
            this.lblObjectName = new System.Windows.Forms.Label();
            this.txtObjectName = new System.Windows.Forms.TextBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabCommon = new System.Windows.Forms.TabPage();
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.btnCommentHeader = new System.Windows.Forms.Button();
            this.btnReadUnCommitted = new System.Windows.Forms.Button();
            this.btnTopStar = new System.Windows.Forms.Button();
            this.btnSelectStar = new System.Windows.Forms.Button();
            this.btnCountStar = new System.Windows.Forms.Button();
            this.tabRare = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.autoTemplateButton1 = new AutoZilla.Core.WinForms.AutoTemplateButton();
            this.tabMain.SuspendLayout();
            this.tabCommon.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblObjectName
            // 
            this.lblObjectName.AutoSize = true;
            this.lblObjectName.Location = new System.Drawing.Point(3, 8);
            this.lblObjectName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblObjectName.Name = "lblObjectName";
            this.lblObjectName.Size = new System.Drawing.Size(110, 23);
            this.lblObjectName.TabIndex = 0;
            this.lblObjectName.Text = "Object Name";
            // 
            // txtObjectName
            // 
            this.txtObjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjectName.Location = new System.Drawing.Point(115, 5);
            this.txtObjectName.Margin = new System.Windows.Forms.Padding(5);
            this.txtObjectName.Name = "txtObjectName";
            this.txtObjectName.Size = new System.Drawing.Size(344, 31);
            this.txtObjectName.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabCommon);
            this.tabMain.Controls.Add(this.tabRare);
            this.tabMain.Location = new System.Drawing.Point(12, 44);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(451, 359);
            this.tabMain.TabIndex = 2;
            // 
            // tabCommon
            // 
            this.tabCommon.Controls.Add(this.autoTemplateButton1);
            this.tabCommon.Controls.Add(this.btnCreateTable);
            this.tabCommon.Controls.Add(this.btnCommentHeader);
            this.tabCommon.Controls.Add(this.btnReadUnCommitted);
            this.tabCommon.Controls.Add(this.btnTopStar);
            this.tabCommon.Controls.Add(this.btnSelectStar);
            this.tabCommon.Controls.Add(this.btnCountStar);
            this.tabCommon.Location = new System.Drawing.Point(4, 32);
            this.tabCommon.Name = "tabCommon";
            this.tabCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommon.Size = new System.Drawing.Size(443, 323);
            this.tabCommon.TabIndex = 0;
            this.tabCommon.Text = "Common";
            this.tabCommon.UseVisualStyleBackColor = true;
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Location = new System.Drawing.Point(233, 7);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(162, 36);
            this.btnCreateTable.TabIndex = 5;
            this.btnCreateTable.Text = "Create Table";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            // 
            // btnCommentHeader
            // 
            this.btnCommentHeader.Location = new System.Drawing.Point(7, 175);
            this.btnCommentHeader.Name = "btnCommentHeader";
            this.btnCommentHeader.Size = new System.Drawing.Size(162, 36);
            this.btnCommentHeader.TabIndex = 4;
            this.btnCommentHeader.Text = "Comment (CA-C)";
            this.btnCommentHeader.UseVisualStyleBackColor = true;
            // 
            // btnReadUnCommitted
            // 
            this.btnReadUnCommitted.Location = new System.Drawing.Point(7, 133);
            this.btnReadUnCommitted.Name = "btnReadUnCommitted";
            this.btnReadUnCommitted.Size = new System.Drawing.Size(162, 36);
            this.btnReadUnCommitted.TabIndex = 3;
            this.btnReadUnCommitted.Text = "Read Unc. (CA-U)";
            this.btnReadUnCommitted.UseVisualStyleBackColor = true;
            // 
            // btnTopStar
            // 
            this.btnTopStar.Location = new System.Drawing.Point(7, 91);
            this.btnTopStar.Name = "btnTopStar";
            this.btnTopStar.Size = new System.Drawing.Size(162, 36);
            this.btnTopStar.TabIndex = 2;
            this.btnTopStar.Text = "Top * (CA-S)";
            this.btnTopStar.UseVisualStyleBackColor = true;
            // 
            // btnSelectStar
            // 
            this.btnSelectStar.Location = new System.Drawing.Point(7, 49);
            this.btnSelectStar.Name = "btnSelectStar";
            this.btnSelectStar.Size = new System.Drawing.Size(162, 36);
            this.btnSelectStar.TabIndex = 1;
            this.btnSelectStar.Text = "Select * (CA-F)";
            this.btnSelectStar.UseVisualStyleBackColor = true;
            // 
            // btnCountStar
            // 
            this.btnCountStar.Location = new System.Drawing.Point(7, 7);
            this.btnCountStar.Name = "btnCountStar";
            this.btnCountStar.Size = new System.Drawing.Size(162, 36);
            this.btnCountStar.TabIndex = 0;
            this.btnCountStar.Text = "Count * (CA-D)";
            this.btnCountStar.UseVisualStyleBackColor = true;
            // 
            // tabRare
            // 
            this.tabRare.Location = new System.Drawing.Point(4, 32);
            this.tabRare.Name = "tabRare";
            this.tabRare.Padding = new System.Windows.Forms.Padding(3);
            this.tabRare.Size = new System.Drawing.Size(443, 323);
            this.tabRare.TabIndex = 1;
            this.tabRare.Text = "Rare";
            this.tabRare.UseVisualStyleBackColor = true;
            // 
            // autoTemplateButton1
            // 
            this.autoTemplateButton1.Location = new System.Drawing.Point(35, 231);
            this.autoTemplateButton1.Name = "autoTemplateButton1";
            this.autoTemplateButton1.Size = new System.Drawing.Size(348, 36);
            this.autoTemplateButton1.TabIndex = 6;
            this.autoTemplateButton1.Text = "autoTemplateButton1";
            this.autoTemplateButton1.TextTemplate = null;
            this.autoTemplateButton1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 415);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.txtObjectName);
            this.Controls.Add(this.lblObjectName);
            this.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainForm";
            this.Text = "AutoZilla MSSQL Plugin";
            this.tabMain.ResumeLayout(false);
            this.tabCommon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblObjectName;
        private System.Windows.Forms.TextBox txtObjectName;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabCommon;
        private System.Windows.Forms.TabPage tabRare;
        private System.Windows.Forms.Button btnCountStar;
        private System.Windows.Forms.Button btnSelectStar;
        private System.Windows.Forms.Button btnTopStar;
        private System.Windows.Forms.Button btnReadUnCommitted;
        private System.Windows.Forms.Button btnCommentHeader;
        private System.Windows.Forms.Button btnCreateTable;
        private Core.WinForms.AutoTemplateButton autoTemplateButton1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}