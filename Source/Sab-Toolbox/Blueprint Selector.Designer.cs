namespace Sab_Toolbox
{
    partial class Blueprint_Selector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Blueprint_Selector));
            this.weaponsButton = new System.Windows.Forms.Button();
            this.vehiclesButton = new System.Windows.Forms.Button();
            this.willToFightButton = new System.Windows.Forms.Button();
            this.camerasButton = new System.Windows.Forms.Button();
            this.generalButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // weaponsButton
            // 
            this.weaponsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weaponsButton.Location = new System.Drawing.Point(12, 85);
            this.weaponsButton.Name = "weaponsButton";
            this.weaponsButton.Size = new System.Drawing.Size(260, 71);
            this.weaponsButton.TabIndex = 5;
            this.weaponsButton.Text = "Weapons";
            this.weaponsButton.UseVisualStyleBackColor = true;
            this.weaponsButton.Click += new System.EventHandler(this.weaponsButton_Click);
            // 
            // vehiclesButton
            // 
            this.vehiclesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vehiclesButton.Location = new System.Drawing.Point(12, 158);
            this.vehiclesButton.Name = "vehiclesButton";
            this.vehiclesButton.Size = new System.Drawing.Size(260, 71);
            this.vehiclesButton.TabIndex = 6;
            this.vehiclesButton.Text = "Vehicles";
            this.vehiclesButton.UseVisualStyleBackColor = true;
            // 
            // willToFightButton
            // 
            this.willToFightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.willToFightButton.Location = new System.Drawing.Point(12, 12);
            this.willToFightButton.Name = "willToFightButton";
            this.willToFightButton.Size = new System.Drawing.Size(260, 71);
            this.willToFightButton.TabIndex = 4;
            this.willToFightButton.Text = "Will To Fight Color Scheme";
            this.willToFightButton.UseVisualStyleBackColor = true;
            this.willToFightButton.Click += new System.EventHandler(this.willToFightButton_Click);
            // 
            // camerasButton
            // 
            this.camerasButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camerasButton.Location = new System.Drawing.Point(12, 231);
            this.camerasButton.Name = "camerasButton";
            this.camerasButton.Size = new System.Drawing.Size(260, 71);
            this.camerasButton.TabIndex = 7;
            this.camerasButton.Text = "Cameras";
            this.camerasButton.UseVisualStyleBackColor = true;
            // 
            // generalButton
            // 
            this.generalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generalButton.Location = new System.Drawing.Point(12, 308);
            this.generalButton.Name = "generalButton";
            this.generalButton.Size = new System.Drawing.Size(260, 71);
            this.generalButton.TabIndex = 8;
            this.generalButton.Text = "General";
            this.generalButton.UseVisualStyleBackColor = true;
            // 
            // Blueprint_Selector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 390);
            this.Controls.Add(this.generalButton);
            this.Controls.Add(this.weaponsButton);
            this.Controls.Add(this.vehiclesButton);
            this.Controls.Add(this.willToFightButton);
            this.Controls.Add(this.camerasButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Blueprint_Selector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Selector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button weaponsButton;
        private System.Windows.Forms.Button vehiclesButton;
        private System.Windows.Forms.Button willToFightButton;
        private System.Windows.Forms.Button camerasButton;
        private System.Windows.Forms.Button generalButton;
    }
}