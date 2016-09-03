namespace SimCityBuildItBot
{
    partial class ShoppingListsForm
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
            this.listBoxShoppingList = new System.Windows.Forms.ListBox();
            this.listBoxSelected = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxAvailable = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtShoppingListName = new System.Windows.Forms.TextBox();
            this.btnAddShoppingList = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxShoppingList
            // 
            this.listBoxShoppingList.FormattingEnabled = true;
            this.listBoxShoppingList.Location = new System.Drawing.Point(13, 30);
            this.listBoxShoppingList.Name = "listBoxShoppingList";
            this.listBoxShoppingList.Size = new System.Drawing.Size(152, 303);
            this.listBoxShoppingList.TabIndex = 0;
            this.listBoxShoppingList.SelectedIndexChanged += new System.EventHandler(this.listBoxShoppingList_SelectedIndexChanged);
            // 
            // listBoxSelected
            // 
            this.listBoxSelected.FormattingEnabled = true;
            this.listBoxSelected.Location = new System.Drawing.Point(218, 30);
            this.listBoxSelected.Name = "listBoxSelected";
            this.listBoxSelected.Size = new System.Drawing.Size(152, 303);
            this.listBoxSelected.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Shopping List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Items Selected";
            // 
            // listBoxAvailable
            // 
            this.listBoxAvailable.FormattingEnabled = true;
            this.listBoxAvailable.Location = new System.Drawing.Point(448, 30);
            this.listBoxAvailable.Name = "listBoxAvailable";
            this.listBoxAvailable.Size = new System.Drawing.Size(194, 303);
            this.listBoxAvailable.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Items Available";
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Location = new System.Drawing.Point(376, 94);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(66, 49);
            this.btnRemoveItem.TabIndex = 6;
            this.btnRemoveItem.Text = ">";
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(376, 175);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(66, 49);
            this.btnAddItem.TabIndex = 7;
            this.btnAddItem.Text = "<";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 401);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Shopping List Name";
            // 
            // txtShoppingListName
            // 
            this.txtShoppingListName.Location = new System.Drawing.Point(19, 418);
            this.txtShoppingListName.Name = "txtShoppingListName";
            this.txtShoppingListName.Size = new System.Drawing.Size(198, 20);
            this.txtShoppingListName.TabIndex = 9;
            // 
            // btnAddShoppingList
            // 
            this.btnAddShoppingList.Location = new System.Drawing.Point(223, 416);
            this.btnAddShoppingList.Name = "btnAddShoppingList";
            this.btnAddShoppingList.Size = new System.Drawing.Size(75, 23);
            this.btnAddShoppingList.TabIndex = 10;
            this.btnAddShoppingList.Text = "Add";
            this.btnAddShoppingList.UseVisualStyleBackColor = true;
            this.btnAddShoppingList.Click += new System.EventHandler(this.btnAddShoppingList_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(19, 339);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(218, 339);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(66, 49);
            this.btnUp.TabIndex = 12;
            this.btnUp.Text = "^";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(304, 339);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(66, 49);
            this.btnDown.TabIndex = 13;
            this.btnDown.Text = "V";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // ShoppingListsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 457);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAddShoppingList);
            this.Controls.Add(this.txtShoppingListName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.btnRemoveItem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxAvailable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSelected);
            this.Controls.Add(this.listBoxShoppingList);
            this.Name = "ShoppingListsForm";
            this.Text = "Shopping Lists";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxShoppingList;
        private System.Windows.Forms.ListBox listBoxSelected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxAvailable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtShoppingListName;
        private System.Windows.Forms.Button btnAddShoppingList;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}