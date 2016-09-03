namespace SimCityBuildItBot
{
    partial class TestForm
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnNavigateTo = new System.Windows.Forms.Button();
            this.cboBuilding = new System.Windows.Forms.ComboBox();
            this.centreClick = new System.Windows.Forms.Button();
            this.belowClick = new System.Windows.Forms.Button();
            this.rightClick = new System.Windows.Forms.Button();
            this.leftClick = new System.Windows.Forms.Button();
            this.production1Steel = new System.Windows.Forms.Button();
            this.buildItem1 = new System.Windows.Forms.Button();
            this.commerceBuild1 = new System.Windows.Forms.Button();
            this.production2Woodlog = new System.Windows.Forms.Button();
            this.production3Seed = new System.Windows.Forms.Button();
            this.buildItem2 = new System.Windows.Forms.Button();
            this.buildItem3 = new System.Windows.Forms.Button();
            this.production1Textiles = new System.Windows.Forms.Button();
            this.buildTextiles = new System.Windows.Forms.Button();
            this.txtRequired = new System.Windows.Forms.TextBox();
            this.btnNavigateToTradeDepot = new System.Windows.Forms.Button();
            this.btnNavigateToGlobalTradeDepot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(13, 13);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(114, 23);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Select a Building";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(0, 445);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(885, 350);
            this.txtLog.TabIndex = 1;
            // 
            // btnNavigateTo
            // 
            this.btnNavigateTo.Location = new System.Drawing.Point(152, 13);
            this.btnNavigateTo.Name = "btnNavigateTo";
            this.btnNavigateTo.Size = new System.Drawing.Size(114, 23);
            this.btnNavigateTo.TabIndex = 2;
            this.btnNavigateTo.Text = "Navigate To";
            this.btnNavigateTo.UseVisualStyleBackColor = true;
            this.btnNavigateTo.Click += new System.EventHandler(this.btnNavigateTo_Click);
            // 
            // cboBuilding
            // 
            this.cboBuilding.FormattingEnabled = true;
            this.cboBuilding.Location = new System.Drawing.Point(152, 43);
            this.cboBuilding.Name = "cboBuilding";
            this.cboBuilding.Size = new System.Drawing.Size(121, 21);
            this.cboBuilding.TabIndex = 3;
            // 
            // centreClick
            // 
            this.centreClick.Location = new System.Drawing.Point(612, 76);
            this.centreClick.Name = "centreClick";
            this.centreClick.Size = new System.Drawing.Size(89, 23);
            this.centreClick.TabIndex = 4;
            this.centreClick.Text = "Centre";
            this.centreClick.UseVisualStyleBackColor = true;
            this.centreClick.Click += new System.EventHandler(this.centreClick_Click);
            // 
            // belowClick
            // 
            this.belowClick.Location = new System.Drawing.Point(612, 120);
            this.belowClick.Name = "belowClick";
            this.belowClick.Size = new System.Drawing.Size(89, 23);
            this.belowClick.TabIndex = 5;
            this.belowClick.Text = "Below Centre";
            this.belowClick.UseVisualStyleBackColor = true;
            this.belowClick.Click += new System.EventHandler(this.belowClick_Click);
            // 
            // rightClick
            // 
            this.rightClick.Location = new System.Drawing.Point(710, 41);
            this.rightClick.Name = "rightClick";
            this.rightClick.Size = new System.Drawing.Size(89, 23);
            this.rightClick.TabIndex = 6;
            this.rightClick.Text = "Right";
            this.rightClick.UseVisualStyleBackColor = true;
            this.rightClick.Click += new System.EventHandler(this.rightClick_Click);
            // 
            // leftClick
            // 
            this.leftClick.Location = new System.Drawing.Point(522, 41);
            this.leftClick.Name = "leftClick";
            this.leftClick.Size = new System.Drawing.Size(89, 23);
            this.leftClick.TabIndex = 7;
            this.leftClick.Text = "Left";
            this.leftClick.UseVisualStyleBackColor = true;
            this.leftClick.Click += new System.EventHandler(this.leftClick_Click);
            // 
            // production1Steel
            // 
            this.production1Steel.Location = new System.Drawing.Point(470, 76);
            this.production1Steel.Name = "production1Steel";
            this.production1Steel.Size = new System.Drawing.Size(89, 38);
            this.production1Steel.TabIndex = 8;
            this.production1Steel.Text = "Production 1 Steel";
            this.production1Steel.UseVisualStyleBackColor = true;
            this.production1Steel.Click += new System.EventHandler(this.production1_Click);
            // 
            // buildItem1
            // 
            this.buildItem1.Location = new System.Drawing.Point(418, 168);
            this.buildItem1.Name = "buildItem1";
            this.buildItem1.Size = new System.Drawing.Size(90, 41);
            this.buildItem1.TabIndex = 9;
            this.buildItem1.Text = "Factory Build Item 1";
            this.buildItem1.UseVisualStyleBackColor = true;
            this.buildItem1.Click += new System.EventHandler(this.buildItem1_Click);
            // 
            // commerceBuild1
            // 
            this.commerceBuild1.Location = new System.Drawing.Point(418, 262);
            this.commerceBuild1.Name = "commerceBuild1";
            this.commerceBuild1.Size = new System.Drawing.Size(90, 41);
            this.commerceBuild1.TabIndex = 10;
            this.commerceBuild1.Text = "Commerce Build Item 1";
            this.commerceBuild1.UseVisualStyleBackColor = true;
            this.commerceBuild1.Click += new System.EventHandler(this.commerceBuild1_Click);
            // 
            // production2Woodlog
            // 
            this.production2Woodlog.Location = new System.Drawing.Point(443, 120);
            this.production2Woodlog.Name = "production2Woodlog";
            this.production2Woodlog.Size = new System.Drawing.Size(89, 37);
            this.production2Woodlog.TabIndex = 11;
            this.production2Woodlog.Text = "Production 2 Wood log";
            this.production2Woodlog.UseVisualStyleBackColor = true;
            this.production2Woodlog.Click += new System.EventHandler(this.production2Woodlog_Click);
            // 
            // production3Seed
            // 
            this.production3Seed.Location = new System.Drawing.Point(738, 78);
            this.production3Seed.Name = "production3Seed";
            this.production3Seed.Size = new System.Drawing.Size(89, 36);
            this.production3Seed.TabIndex = 12;
            this.production3Seed.Text = "Production 3 seed";
            this.production3Seed.UseVisualStyleBackColor = true;
            this.production3Seed.Click += new System.EventHandler(this.production3Seed_Click);
            // 
            // buildItem2
            // 
            this.buildItem2.Location = new System.Drawing.Point(418, 204);
            this.buildItem2.Name = "buildItem2";
            this.buildItem2.Size = new System.Drawing.Size(90, 41);
            this.buildItem2.TabIndex = 13;
            this.buildItem2.Text = "Factory Build Item 2";
            this.buildItem2.UseVisualStyleBackColor = true;
            this.buildItem2.Click += new System.EventHandler(this.buildItem2_Click);
            // 
            // buildItem3
            // 
            this.buildItem3.Location = new System.Drawing.Point(737, 184);
            this.buildItem3.Name = "buildItem3";
            this.buildItem3.Size = new System.Drawing.Size(90, 41);
            this.buildItem3.TabIndex = 14;
            this.buildItem3.Text = "Factory Build Item 3";
            this.buildItem3.UseVisualStyleBackColor = true;
            this.buildItem3.Click += new System.EventHandler(this.buildItem3_Click);
            // 
            // production1Textiles
            // 
            this.production1Textiles.Location = new System.Drawing.Point(375, 76);
            this.production1Textiles.Name = "production1Textiles";
            this.production1Textiles.Size = new System.Drawing.Size(89, 38);
            this.production1Textiles.TabIndex = 17;
            this.production1Textiles.Text = "Production 1 Textiles";
            this.production1Textiles.UseVisualStyleBackColor = true;
            this.production1Textiles.Click += new System.EventHandler(this.production1Textiles_Click);
            // 
            // buildTextiles
            // 
            this.buildTextiles.Location = new System.Drawing.Point(332, 168);
            this.buildTextiles.Name = "buildTextiles";
            this.buildTextiles.Size = new System.Drawing.Size(90, 41);
            this.buildTextiles.TabIndex = 18;
            this.buildTextiles.Text = "Factory Build Textiles";
            this.buildTextiles.UseVisualStyleBackColor = true;
            this.buildTextiles.Click += new System.EventHandler(this.buildTextiles_Click);
            // 
            // txtRequired
            // 
            this.txtRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequired.Location = new System.Drawing.Point(0, 309);
            this.txtRequired.Multiline = true;
            this.txtRequired.Name = "txtRequired";
            this.txtRequired.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRequired.Size = new System.Drawing.Size(885, 139);
            this.txtRequired.TabIndex = 19;
            // 
            // btnNavigateToTradeDepot
            // 
            this.btnNavigateToTradeDepot.Location = new System.Drawing.Point(13, 99);
            this.btnNavigateToTradeDepot.Name = "btnNavigateToTradeDepot";
            this.btnNavigateToTradeDepot.Size = new System.Drawing.Size(164, 23);
            this.btnNavigateToTradeDepot.TabIndex = 20;
            this.btnNavigateToTradeDepot.Text = "Navigate To Trade Depot";
            this.btnNavigateToTradeDepot.UseVisualStyleBackColor = true;
            this.btnNavigateToTradeDepot.Click += new System.EventHandler(this.btnNavigateToTradeDepot_Click);
            // 
            // btnNavigateToGlobalTradeDepot
            // 
            this.btnNavigateToGlobalTradeDepot.Location = new System.Drawing.Point(13, 70);
            this.btnNavigateToGlobalTradeDepot.Name = "btnNavigateToGlobalTradeDepot";
            this.btnNavigateToGlobalTradeDepot.Size = new System.Drawing.Size(164, 23);
            this.btnNavigateToGlobalTradeDepot.TabIndex = 21;
            this.btnNavigateToGlobalTradeDepot.Text = "Navigate To Global Trade";
            this.btnNavigateToGlobalTradeDepot.UseVisualStyleBackColor = true;
            this.btnNavigateToGlobalTradeDepot.Click += new System.EventHandler(this.btnNavigateToGlobalTradeDepot_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 795);
            this.Controls.Add(this.btnNavigateToGlobalTradeDepot);
            this.Controls.Add(this.btnNavigateToTradeDepot);
            this.Controls.Add(this.txtRequired);
            this.Controls.Add(this.buildTextiles);
            this.Controls.Add(this.production1Textiles);
            this.Controls.Add(this.buildItem3);
            this.Controls.Add(this.buildItem2);
            this.Controls.Add(this.production3Seed);
            this.Controls.Add(this.production2Woodlog);
            this.Controls.Add(this.commerceBuild1);
            this.Controls.Add(this.buildItem1);
            this.Controls.Add(this.production1Steel);
            this.Controls.Add(this.leftClick);
            this.Controls.Add(this.rightClick);
            this.Controls.Add(this.belowClick);
            this.Controls.Add(this.centreClick);
            this.Controls.Add(this.cboBuilding);
            this.Controls.Add(this.btnNavigateTo);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnSelect);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnNavigateTo;
        private System.Windows.Forms.ComboBox cboBuilding;
        private System.Windows.Forms.Button centreClick;
        private System.Windows.Forms.Button belowClick;
        private System.Windows.Forms.Button rightClick;
        private System.Windows.Forms.Button leftClick;
        private System.Windows.Forms.Button production1Steel;
        private System.Windows.Forms.Button buildItem1;
        private System.Windows.Forms.Button commerceBuild1;
        private System.Windows.Forms.Button production2Woodlog;
        private System.Windows.Forms.Button production3Seed;
        private System.Windows.Forms.Button buildItem2;
        private System.Windows.Forms.Button buildItem3;
        private System.Windows.Forms.Button production1Textiles;
        private System.Windows.Forms.Button buildTextiles;
        private System.Windows.Forms.TextBox txtRequired;
        private System.Windows.Forms.Button btnNavigateToTradeDepot;
        private System.Windows.Forms.Button btnNavigateToGlobalTradeDepot;
    }
}