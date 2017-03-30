namespace Sab_Toolbox
{
    partial class Megapack_Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Megapack_Editor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openKnownFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mega0megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mega1megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mega2megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dLCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dlc01mega0megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilopackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.start0kiloPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGerman0kiloPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.belleStart0kiloPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamic0megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.palettes0megapackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamic0megapackToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileToOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rotColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.extractButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImage = global::Sab_Toolbox.Properties.Resources.Official_Saboteur_Game_Cover_Art;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(847, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openKnownFileToolStripMenuItem,
            this.selectFileToOpenToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openKnownFileToolStripMenuItem
            // 
            this.openKnownFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.megapackToolStripMenuItem,
            this.kilopackToolStripMenuItem,
            this.dynamicPackToolStripMenuItem});
            this.openKnownFileToolStripMenuItem.Name = "openKnownFileToolStripMenuItem";
            this.openKnownFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openKnownFileToolStripMenuItem.Text = "Open Known File";
            // 
            // megapackToolStripMenuItem
            // 
            this.megapackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mega0megapackToolStripMenuItem,
            this.mega1megapackToolStripMenuItem,
            this.mega2megapackToolStripMenuItem,
            this.dLCToolStripMenuItem1});
            this.megapackToolStripMenuItem.Name = "megapackToolStripMenuItem";
            this.megapackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.megapackToolStripMenuItem.Text = "Megapack";
            // 
            // mega0megapackToolStripMenuItem
            // 
            this.mega0megapackToolStripMenuItem.Name = "mega0megapackToolStripMenuItem";
            this.mega0megapackToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mega0megapackToolStripMenuItem.Text = "Mega0.megapack";
            this.mega0megapackToolStripMenuItem.Click += new System.EventHandler(this.mega0megapackToolStripMenuItem_Click);
            // 
            // mega1megapackToolStripMenuItem
            // 
            this.mega1megapackToolStripMenuItem.Name = "mega1megapackToolStripMenuItem";
            this.mega1megapackToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mega1megapackToolStripMenuItem.Text = "Mega1.megapack";
            this.mega1megapackToolStripMenuItem.Click += new System.EventHandler(this.mega1megapackToolStripMenuItem_Click);
            // 
            // mega2megapackToolStripMenuItem
            // 
            this.mega2megapackToolStripMenuItem.Name = "mega2megapackToolStripMenuItem";
            this.mega2megapackToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mega2megapackToolStripMenuItem.Text = "Mega2.megapack";
            this.mega2megapackToolStripMenuItem.Click += new System.EventHandler(this.mega2megapackToolStripMenuItem_Click);
            // 
            // dLCToolStripMenuItem1
            // 
            this.dLCToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dlc01mega0megapackToolStripMenuItem});
            this.dLCToolStripMenuItem1.Name = "dLCToolStripMenuItem1";
            this.dLCToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.dLCToolStripMenuItem1.Text = "DLC";
            // 
            // dlc01mega0megapackToolStripMenuItem
            // 
            this.dlc01mega0megapackToolStripMenuItem.Name = "dlc01mega0megapackToolStripMenuItem";
            this.dlc01mega0megapackToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.dlc01mega0megapackToolStripMenuItem.Text = "dlc01mega0.megapack";
            this.dlc01mega0megapackToolStripMenuItem.Click += new System.EventHandler(this.dlc01mega0megapackToolStripMenuItem_Click);
            // 
            // kilopackToolStripMenuItem
            // 
            this.kilopackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.start0kiloPackToolStripMenuItem,
            this.startGerman0kiloPackToolStripMenuItem,
            this.belleStart0kiloPackToolStripMenuItem});
            this.kilopackToolStripMenuItem.Name = "kilopackToolStripMenuItem";
            this.kilopackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.kilopackToolStripMenuItem.Text = "Kilopack";
            // 
            // start0kiloPackToolStripMenuItem
            // 
            this.start0kiloPackToolStripMenuItem.Name = "start0kiloPackToolStripMenuItem";
            this.start0kiloPackToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.start0kiloPackToolStripMenuItem.Text = "Start0.kiloPack";
            this.start0kiloPackToolStripMenuItem.Click += new System.EventHandler(this.start0kiloPackToolStripMenuItem_Click);
            // 
            // startGerman0kiloPackToolStripMenuItem
            // 
            this.startGerman0kiloPackToolStripMenuItem.Name = "startGerman0kiloPackToolStripMenuItem";
            this.startGerman0kiloPackToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.startGerman0kiloPackToolStripMenuItem.Text = "Start_German0.kiloPack";
            this.startGerman0kiloPackToolStripMenuItem.Click += new System.EventHandler(this.startGerman0kiloPackToolStripMenuItem_Click);
            // 
            // belleStart0kiloPackToolStripMenuItem
            // 
            this.belleStart0kiloPackToolStripMenuItem.Name = "belleStart0kiloPackToolStripMenuItem";
            this.belleStart0kiloPackToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.belleStart0kiloPackToolStripMenuItem.Text = "BelleStart0.kiloPack";
            this.belleStart0kiloPackToolStripMenuItem.Click += new System.EventHandler(this.belleStart0kiloPackToolStripMenuItem_Click);
            // 
            // dynamicPackToolStripMenuItem
            // 
            this.dynamicPackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dynamic0megapackToolStripMenuItem,
            this.palettes0megapackToolStripMenuItem,
            this.dLCToolStripMenuItem});
            this.dynamicPackToolStripMenuItem.Name = "dynamicPackToolStripMenuItem";
            this.dynamicPackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dynamicPackToolStripMenuItem.Text = "Dynamic pack";
            // 
            // dynamic0megapackToolStripMenuItem
            // 
            this.dynamic0megapackToolStripMenuItem.Name = "dynamic0megapackToolStripMenuItem";
            this.dynamic0megapackToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.dynamic0megapackToolStripMenuItem.Text = "Dynamic0.megapack";
            this.dynamic0megapackToolStripMenuItem.Click += new System.EventHandler(this.dynamic0megapackToolStripMenuItem_Click);
            // 
            // palettes0megapackToolStripMenuItem
            // 
            this.palettes0megapackToolStripMenuItem.Name = "palettes0megapackToolStripMenuItem";
            this.palettes0megapackToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.palettes0megapackToolStripMenuItem.Text = "Palettes0.megapack";
            this.palettes0megapackToolStripMenuItem.Click += new System.EventHandler(this.palettes0megapackToolStripMenuItem_Click);
            // 
            // dLCToolStripMenuItem
            // 
            this.dLCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dynamic0megapackToolStripMenuItem1});
            this.dLCToolStripMenuItem.Name = "dLCToolStripMenuItem";
            this.dLCToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.dLCToolStripMenuItem.Text = "DLC";
            // 
            // dynamic0megapackToolStripMenuItem1
            // 
            this.dynamic0megapackToolStripMenuItem1.Name = "dynamic0megapackToolStripMenuItem1";
            this.dynamic0megapackToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.dynamic0megapackToolStripMenuItem1.Text = "Dynamic0.megapack";
            this.dynamic0megapackToolStripMenuItem1.Click += new System.EventHandler(this.dynamic0megapackToolStripMenuItem1_Click);
            // 
            // selectFileToOpenToolStripMenuItem
            // 
            this.selectFileToOpenToolStripMenuItem.Name = "selectFileToOpenToolStripMenuItem";
            this.selectFileToOpenToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.selectFileToOpenToolStripMenuItem.Text = "Select File To Open";
            this.selectFileToOpenToolStripMenuItem.Click += new System.EventHandler(this.selectFileToOpenToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.xColumn,
            this.YColumn,
            this.ZColumn,
            this.rotColumn});
            this.dataGridView1.Location = new System.Drawing.Point(293, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(543, 360);
            this.dataGridView1.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            // 
            // xColumn
            // 
            this.xColumn.HeaderText = "X";
            this.xColumn.Name = "xColumn";
            // 
            // YColumn
            // 
            this.YColumn.HeaderText = "Y";
            this.YColumn.Name = "YColumn";
            // 
            // ZColumn
            // 
            this.ZColumn.HeaderText = "Z";
            this.ZColumn.Name = "ZColumn";
            // 
            // rotColumn
            // 
            this.rotColumn.HeaderText = "Rotation";
            this.rotColumn.Name = "rotColumn";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.LightGray;
            this.treeView1.Location = new System.Drawing.Point(12, 36);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(275, 314);
            this.treeView1.TabIndex = 5;
            // 
            // extractButton
            // 
            this.extractButton.Enabled = false;
            this.extractButton.Location = new System.Drawing.Point(12, 357);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(275, 39);
            this.extractButton.TabIndex = 6;
            this.extractButton.Text = "Extract SBLA Files";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // Megapack_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(847, 408);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Megapack_Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sab-Toolbox v 0.0 - Megapack Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFileToOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openKnownFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem megapackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kilopackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamicPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem mega0megapackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mega1megapackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mega2megapackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem start0kiloPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startGerman0kiloPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem belleStart0kiloPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamic0megapackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem palettes0megapackToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn YColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rotColumn;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem dLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamic0megapackToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dLCToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dlc01mega0megapackToolStripMenuItem;
    }
}