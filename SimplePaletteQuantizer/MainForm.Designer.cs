namespace SimplePaletteQuantizer
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
            this.dialogOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.splitContainerPngSizes = new System.Windows.Forms.SplitContainer();
            this.splitContainerGifSizes = new System.Windows.Forms.SplitContainer();
            this.panelControls = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.pictureTarget2 = new System.Windows.Forms.PictureBox();
            this.splitterMain = new System.Windows.Forms.Splitter();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pictureTarget1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panelStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPngSizes)).BeginInit();
            this.splitContainerPngSizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGifSizes)).BeginInit();
            this.splitContainerGifSizes.Panel1.SuspendLayout();
            this.splitContainerGifSizes.Panel2.SuspendLayout();
            this.splitContainerGifSizes.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTarget2)).BeginInit();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTarget1)).BeginInit();
            this.SuspendLayout();
            // 
            // dialogOpenFile
            // 
            this.dialogOpenFile.Filter = "Supported images|*.png;*.jpg;*.gif;*.jpeg;*.bmp;*.tiff";
            // 
            // panelStatistics
            // 
            this.panelStatistics.AutoSize = true;
            this.panelStatistics.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelStatistics.Controls.Add(this.splitContainerPngSizes);
            this.panelStatistics.Controls.Add(this.splitContainerGifSizes);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatistics.Location = new System.Drawing.Point(5, 460);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Size = new System.Drawing.Size(674, 60);
            this.panelStatistics.TabIndex = 4;
            // 
            // splitContainerPngSizes
            // 
            this.splitContainerPngSizes.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerPngSizes.Location = new System.Drawing.Point(0, 30);
            this.splitContainerPngSizes.Name = "splitContainerPngSizes";
            // 
            // splitContainerPngSizes.Panel1
            // 
            this.splitContainerPngSizes.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainerPngSizes.Panel2
            // 
            this.splitContainerPngSizes.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainerPngSizes.Size = new System.Drawing.Size(674, 30);
            this.splitContainerPngSizes.SplitterDistance = 332;
            this.splitContainerPngSizes.TabIndex = 2;
            this.splitContainerPngSizes.TabStop = false;
            // 
            // splitContainerGifSizes
            // 
            this.splitContainerGifSizes.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerGifSizes.Location = new System.Drawing.Point(0, 0);
            this.splitContainerGifSizes.Name = "splitContainerGifSizes";
            // 
            // splitContainerGifSizes.Panel1
            // 
            this.splitContainerGifSizes.Panel1.Controls.Add(this.textBox1);
            this.splitContainerGifSizes.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainerGifSizes.Panel2
            // 
            this.splitContainerGifSizes.Panel2.Controls.Add(this.textBox2);
            this.splitContainerGifSizes.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainerGifSizes.Size = new System.Drawing.Size(674, 30);
            this.splitContainerGifSizes.SplitterDistance = 332;
            this.splitContainerGifSizes.TabIndex = 1;
            this.splitContainerGifSizes.TabStop = false;
            // 
            // panelControls
            // 
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControls.Location = new System.Drawing.Point(5, 520);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(674, 39);
            this.panelControls.TabIndex = 16;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.pictureTarget2);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(337, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(337, 450);
            this.panelRight.TabIndex = 1;
            // 
            // pictureTarget2
            // 
            this.pictureTarget2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureTarget2.Location = new System.Drawing.Point(0, 0);
            this.pictureTarget2.Name = "pictureTarget2";
            this.pictureTarget2.Size = new System.Drawing.Size(337, 450);
            this.pictureTarget2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTarget2.TabIndex = 12;
            this.pictureTarget2.TabStop = false;
            // 
            // splitterMain
            // 
            this.splitterMain.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitterMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterMain.Location = new System.Drawing.Point(332, 0);
            this.splitterMain.Name = "splitterMain";
            this.splitterMain.Size = new System.Drawing.Size(5, 450);
            this.splitterMain.TabIndex = 2;
            this.splitterMain.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.pictureTarget1);
            this.panelMain.Controls.Add(this.splitterMain);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(5, 5);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panelMain.Size = new System.Drawing.Size(674, 455);
            this.panelMain.TabIndex = 5;
            // 
            // pictureTarget1
            // 
            this.pictureTarget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureTarget1.Location = new System.Drawing.Point(0, 0);
            this.pictureTarget1.Name = "pictureTarget1";
            this.pictureTarget1.Size = new System.Drawing.Size(332, 450);
            this.pictureTarget1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTarget1.TabIndex = 13;
            this.pictureTarget1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(5, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(322, 20);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(5, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(328, 20);
            this.textBox2.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 564);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelStatistics);
            this.Controls.Add(this.panelControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple palette quantizer";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.panelStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPngSizes)).EndInit();
            this.splitContainerPngSizes.ResumeLayout(false);
            this.splitContainerGifSizes.Panel1.ResumeLayout(false);
            this.splitContainerGifSizes.Panel1.PerformLayout();
            this.splitContainerGifSizes.Panel2.ResumeLayout(false);
            this.splitContainerGifSizes.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGifSizes)).EndInit();
            this.splitContainerGifSizes.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTarget2)).EndInit();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTarget1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dialogOpenFile;
        private System.Windows.Forms.Panel panelStatistics;
        private System.Windows.Forms.SplitContainer splitContainerPngSizes;
        private System.Windows.Forms.SplitContainer splitContainerGifSizes;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.PictureBox pictureTarget2;
        private System.Windows.Forms.Splitter splitterMain;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pictureTarget1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

