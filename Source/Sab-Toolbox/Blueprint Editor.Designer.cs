namespace Sab_Toolbox
{
    partial class Blueprint_Editor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Weapons");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Bullets");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Ammo Clips");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Weapons", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Vehicles");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Engines");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Transmissions");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Chassis");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Wheels");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Vehicles", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Props");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Will To Fight");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Cameras");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Blueprint_Editor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectKnownFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSaboteurBlueprintsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dLCBlueprintsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectOtherFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blueprints = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.printListButton = new System.Windows.Forms.Button();
            this.extractBlueprintsButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.menuStrip1.BackgroundImage = global::Sab_Toolbox.Properties.Resources.Official_Saboteur_Game_Cover_Art;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(990, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateNewToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // generateNewToolStripMenuItem
            // 
            this.generateNewToolStripMenuItem.Name = "generateNewToolStripMenuItem";
            this.generateNewToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.generateNewToolStripMenuItem.Text = "New";
            this.generateNewToolStripMenuItem.Click += new System.EventHandler(this.generateNewToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectKnownFileToolStripMenuItem,
            this.selectOtherFileToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // selectKnownFileToolStripMenuItem
            // 
            this.selectKnownFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainSaboteurBlueprintsToolStripMenuItem,
            this.dLCBlueprintsToolStripMenuItem});
            this.selectKnownFileToolStripMenuItem.Name = "selectKnownFileToolStripMenuItem";
            this.selectKnownFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.selectKnownFileToolStripMenuItem.Text = "Select Known File";
            // 
            // mainSaboteurBlueprintsToolStripMenuItem
            // 
            this.mainSaboteurBlueprintsToolStripMenuItem.Name = "mainSaboteurBlueprintsToolStripMenuItem";
            this.mainSaboteurBlueprintsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.mainSaboteurBlueprintsToolStripMenuItem.Text = "Saboteur Blueprints";
            this.mainSaboteurBlueprintsToolStripMenuItem.Click += new System.EventHandler(this.mainSaboteurBlueprintsToolStripMenuItem_Click);
            // 
            // dLCBlueprintsToolStripMenuItem
            // 
            this.dLCBlueprintsToolStripMenuItem.Name = "dLCBlueprintsToolStripMenuItem";
            this.dLCBlueprintsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.dLCBlueprintsToolStripMenuItem.Text = "DLC Blueprints";
            this.dLCBlueprintsToolStripMenuItem.Click += new System.EventHandler(this.dLCBlueprintsToolStripMenuItem_Click);
            // 
            // selectOtherFileToolStripMenuItem
            // 
            this.selectOtherFileToolStripMenuItem.Name = "selectOtherFileToolStripMenuItem";
            this.selectOtherFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.selectOtherFileToolStripMenuItem.Text = "Select Other";
            this.selectOtherFileToolStripMenuItem.Click += new System.EventHandler(this.selectOtherFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.editToolStripMenuItem.Text = "Settings";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click_1);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(12, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(966, 537);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(69, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Blueprints";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(780, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Blueprint Templates";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(761, 54);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "weaponsChild";
            treeNode1.Text = "Weapons";
            treeNode2.Name = "bulletsChild";
            treeNode2.Text = "Bullets";
            treeNode3.Name = "ammoClipChild";
            treeNode3.Text = "Ammo Clips";
            treeNode4.Name = "weaponsTree";
            treeNode4.Text = "Weapons";
            treeNode5.Name = "vehiclesChild";
            treeNode5.Text = "Vehicles";
            treeNode6.Name = "engineChild";
            treeNode6.Text = "Engines";
            treeNode7.Name = "transmissionChild";
            treeNode7.Text = "Transmissions";
            treeNode8.Name = "chassisChild";
            treeNode8.Text = "Chassis";
            treeNode9.Name = "wheelsChild";
            treeNode9.Text = "Wheels";
            treeNode10.Name = "vehicleTree";
            treeNode10.Text = "Vehicles";
            treeNode11.Name = "propsTree";
            treeNode11.Text = "Props";
            treeNode12.Name = "wtfTree";
            treeNode12.Text = "Will To Fight";
            treeNode13.Name = "camerasTree";
            treeNode13.Text = "Cameras";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13});
            this.treeView1.Size = new System.Drawing.Size(202, 457);
            this.treeView1.TabIndex = 14;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column9,
            this.Column10,
            this.Column7,
            this.Column8});
            this.dataGridView2.Location = new System.Drawing.Point(3, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(511, 455);
            this.dataGridView2.TabIndex = 16;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Hash Identifier";
            this.Column6.Name = "Column6";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "ASCII Identifier";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Function Description";
            this.Column10.Name = "Column10";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Data Value";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Data Type";
            this.Column8.Name = "Column8";
            // 
            // Blueprints
            // 
            this.Blueprints.Location = new System.Drawing.Point(23, 54);
            this.Blueprints.Name = "Blueprints";
            this.Blueprints.Size = new System.Drawing.Size(214, 457);
            this.Blueprints.TabIndex = 17;
            this.Blueprints.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.Blueprints_BeforeSelect);
            this.Blueprints.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Blueprints_AfterSelect);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(0, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(512, 455);
            this.dataGridView1.TabIndex = 15;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Hash Identifier";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Data Value (Hex)";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Data Value (Int32/Float)";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Size";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "i";
            this.Column5.Name = "Column5";
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 518);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(258, 34);
            this.button1.TabIndex = 18;
            this.button1.Text = "Full Data List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(507, 518);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(248, 34);
            this.button2.TabIndex = 19;
            this.button2.Text = "Known Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button2_MouseClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dataGridView2);
            this.panel1.Location = new System.Drawing.Point(241, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 458);
            this.panel1.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Location = new System.Drawing.Point(243, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(512, 458);
            this.panel2.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(458, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 22;
            // 
            // printListButton
            // 
            this.printListButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printListButton.Location = new System.Drawing.Point(893, 517);
            this.printListButton.Name = "printListButton";
            this.printListButton.Size = new System.Drawing.Size(70, 33);
            this.printListButton.TabIndex = 23;
            this.printListButton.Text = "Print List";
            this.printListButton.UseVisualStyleBackColor = true;
            this.printListButton.Visible = false;
            this.printListButton.Click += new System.EventHandler(this.printListButton_Click);
            // 
            // extractBlueprintsButton
            // 
            this.extractBlueprintsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extractBlueprintsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.extractBlueprintsButton.Location = new System.Drawing.Point(761, 517);
            this.extractBlueprintsButton.Name = "extractBlueprintsButton";
            this.extractBlueprintsButton.Size = new System.Drawing.Size(126, 33);
            this.extractBlueprintsButton.TabIndex = 24;
            this.extractBlueprintsButton.Text = "Extract Blueprints";
            this.extractBlueprintsButton.UseVisualStyleBackColor = true;
            this.extractBlueprintsButton.Visible = false;
            this.extractBlueprintsButton.Click += new System.EventHandler(this.extractBlueprintsButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(23, 518);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(214, 32);
            this.progressBar1.TabIndex = 25;
            this.progressBar1.Visible = false;
            // 
            // Blueprint_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(990, 581);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.extractBlueprintsButton);
            this.Controls.Add(this.printListButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Blueprints);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Blueprint_Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sab-Toolbox v 0.0 - Blueprint Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectKnownFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainSaboteurBlueprintsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dLCBlueprintsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectOtherFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TreeView Blueprints;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button printListButton;
        private System.Windows.Forms.Button extractBlueprintsButton;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}