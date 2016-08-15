namespace SimCityBuildItBot
{
    partial class BotForm
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnBuildAvailableItems = new System.Windows.Forms.Button();
            this.txtRequiredItems = new System.Windows.Forms.TextBox();
            this.txtRequired = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(0, 266);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(885, 529);
            this.txtLog.TabIndex = 1;
            // 
            // btnBuildAvailableItems
            // 
            this.btnBuildAvailableItems.Location = new System.Drawing.Point(12, 12);
            this.btnBuildAvailableItems.Name = "btnBuildAvailableItems";
            this.btnBuildAvailableItems.Size = new System.Drawing.Size(98, 65);
            this.btnBuildAvailableItems.TabIndex = 15;
            this.btnBuildAvailableItems.Text = "Start Crafting";
            this.btnBuildAvailableItems.UseVisualStyleBackColor = true;
            this.btnBuildAvailableItems.Click += new System.EventHandler(this.btnBuildAvailableItems_Click);
            // 
            // txtRequiredItems
            // 
            this.txtRequiredItems.Location = new System.Drawing.Point(12, 83);
            this.txtRequiredItems.Name = "txtRequiredItems";
            this.txtRequiredItems.Size = new System.Drawing.Size(188, 20);
            this.txtRequiredItems.TabIndex = 16;
            // 
            // txtRequired
            // 
            this.txtRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequired.Location = new System.Drawing.Point(0, 121);
            this.txtRequired.Multiline = true;
            this.txtRequired.Name = "txtRequired";
            this.txtRequired.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRequired.Size = new System.Drawing.Size(885, 139);
            this.txtRequired.TabIndex = 19;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(116, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 65);
            this.button1.TabIndex = 20;
            this.button1.Text = "Debug Form";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 795);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRequired);
            this.Controls.Add(this.txtRequiredItems);
            this.Controls.Add(this.btnBuildAvailableItems);
            this.Controls.Add(this.txtLog);
            this.Name = "BotForm";
            this.Text = "The Bot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnBuildAvailableItems;
        private System.Windows.Forms.TextBox txtRequiredItems;
        private System.Windows.Forms.TextBox txtRequired;
        private System.Windows.Forms.Button button1;
    }
}