namespace VS2015_CS_ETH_BK_DI8_DO4
{
   partial class frmMain
   {
      /// <summary>
      /// Erforderliche Designervariable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      
      #region Vom Windows Form-Designer generierter Code

      /// <summary>
      /// Erforderliche Methode für die Designerunterstützung.
      /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
      /// </summary>
      private void InitializeComponent()
      {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabController = new System.Windows.Forms.TabPage();
            this._ctrlController1 = new PhoenixContact.HFI.Visualization.ctrlController();
            this.tcAXI = new System.Windows.Forms.TabControl();
            this.tabController.SuspendLayout();
            this.tcAXI.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // tabController
            // 
            this.tabController.Controls.Add(this._ctrlController1);
            this.tabController.Location = new System.Drawing.Point(4, 22);
            this.tabController.Name = "tabController";
            this.tabController.Size = new System.Drawing.Size(1002, 691);
            this.tabController.TabIndex = 0;
            this.tabController.Text = "Controller and communication";
            this.tabController.UseVisualStyleBackColor = true;
            // 
            // _ctrlController1
            // 
            this._ctrlController1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._ctrlController1.ControlText = "ctrlController";
            this._ctrlController1.Location = new System.Drawing.Point(8, 8);
            this._ctrlController1.MaximumSize = new System.Drawing.Size(704, 292);
            this._ctrlController1.MinimumSize = new System.Drawing.Size(326, 292);
            this._ctrlController1.Name = "_ctrlController1";
            this._ctrlController1.Size = new System.Drawing.Size(704, 292);
            this._ctrlController1.TabIndex = 0;
            // 
            // tcAXI
            // 
            this.tcAXI.Controls.Add(this.tabController);
            this.tcAXI.Location = new System.Drawing.Point(12, 12);
            this.tcAXI.Name = "tcAXI";
            this.tcAXI.SelectedIndex = 0;
            this.tcAXI.Size = new System.Drawing.Size(1010, 717);
            this.tcAXI.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 729);
            this.Controls.Add(this.tcAXI);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1042, 768);
            this.Name = "frmMain";
            this.Text = "Template C# (ETH BK DI8 DO4)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabController.ResumeLayout(false);
            this.tcAXI.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.TabPage tabController;
        private PhoenixContact.HFI.Visualization.ctrlController _ctrlController1;
        private System.Windows.Forms.TabControl tcAXI;
    }
}

