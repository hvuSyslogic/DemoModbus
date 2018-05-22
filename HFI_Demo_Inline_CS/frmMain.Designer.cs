namespace HFI_Demo_Inline_CS
{
   partial class frmMain
   {
      /// <summary>
      /// Erforderliche Designervariable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Verwendete Ressourcen bereinigen.
      /// </summary>
      /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (this.components != null))
         {
            this.components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Vom Windows Form-Designer generierter Code

      /// <summary>
      /// Erforderliche Methode für die Designerunterstützung.
      /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
         this.tcAXI = new System.Windows.Forms.TabControl();
         this.tabController = new System.Windows.Forms.TabPage();
         this.gbxErrorLogging = new System.Windows.Forms.GroupBox();
         this.tbxMessages = new System.Windows.Forms.TextBox();
         this.gbxControllerHandling = new System.Windows.Forms.GroupBox();
         this.cbxWatchdog = new System.Windows.Forms.CheckBox();
         this.cbxError = new System.Windows.Forms.CheckBox();
         this.cbxRun = new System.Windows.Forms.CheckBox();
         this.btnClearLog = new System.Windows.Forms.Button();
         this.cbxConnect = new System.Windows.Forms.CheckBox();
         this.btnDisable = new System.Windows.Forms.Button();
         this.btnEnable = new System.Windows.Forms.Button();
         this.tbxControllerType = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.tbxConnection = new System.Windows.Forms.TextBox();
         this.lblControllerConnection = new System.Windows.Forms.Label();
         this.gbxInterbusHandling = new System.Windows.Forms.GroupBox();
         this.cbxBusPF = new System.Windows.Forms.CheckBox();
         this.cbxBusBUS = new System.Windows.Forms.CheckBox();
         this.cbxBusDetect = new System.Windows.Forms.CheckBox();
         this.label11 = new System.Windows.Forms.Label();
         this.tbxBusParameterregister2 = new System.Windows.Forms.TextBox();
         this.label10 = new System.Windows.Forms.Label();
         this.tbxBusParameterregister = new System.Windows.Forms.TextBox();
         this.cbxBusRun = new System.Windows.Forms.CheckBox();
         this.cbxBusActive = new System.Windows.Forms.CheckBox();
         this.cbxBusReady = new System.Windows.Forms.CheckBox();
         this.btnAutoStart = new System.Windows.Forms.Button();
         this.btnAlarmStop = new System.Windows.Forms.Button();
         this.gbxInputData = new System.Windows.Forms.GroupBox();
         this.label12 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.tbxInpByteArray = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.tbxInpValue = new System.Windows.Forms.TextBox();
         this.cbxInpBit_1 = new System.Windows.Forms.CheckBox();
         this.cbxInpBit_0 = new System.Windows.Forms.CheckBox();
         this.gbxOutputData = new System.Windows.Forms.GroupBox();
         this.btnWriteValues = new System.Windows.Forms.Button();
         this.tbxOutputByteArray = new System.Windows.Forms.TextBox();
         this.tbxOutputValue = new System.Windows.Forms.TextBox();
         this.cbxOutputBit_1 = new System.Windows.Forms.CheckBox();
         this.cbxOutputBit_0 = new System.Windows.Forms.CheckBox();
         this.label13 = new System.Windows.Forms.Label();
         this.label14 = new System.Windows.Forms.Label();
         this.label15 = new System.Windows.Forms.Label();
         this.label16 = new System.Windows.Forms.Label();
         this.label17 = new System.Windows.Forms.Label();
         this.tabPCP = new System.Windows.Forms.TabPage();
         this.cbxReadDataError = new System.Windows.Forms.CheckBox();
         this.btnClear = new System.Windows.Forms.Button();
         this.lbxLogging = new System.Windows.Forms.ListBox();
         this.cbxPCPError = new System.Windows.Forms.CheckBox();
         this.cbxPCPReady = new System.Windows.Forms.CheckBox();
         this.cbxWriteDataDone = new System.Windows.Forms.CheckBox();
         this.cbxReadDataValid = new System.Windows.Forms.CheckBox();
         this.btnWriteData_2 = new System.Windows.Forms.Button();
         this.btnReadData_2 = new System.Windows.Forms.Button();
         this.btnDisable_2 = new System.Windows.Forms.Button();
         this.btnEnable_2 = new System.Windows.Forms.Button();
         this.tmrMainFormUpdate = new System.Windows.Forms.Timer(this.components);
         this.tcAXI.SuspendLayout();
         this.tabController.SuspendLayout();
         this.gbxErrorLogging.SuspendLayout();
         this.gbxControllerHandling.SuspendLayout();
         this.gbxInterbusHandling.SuspendLayout();
         this.gbxInputData.SuspendLayout();
         this.gbxOutputData.SuspendLayout();
         this.tabPCP.SuspendLayout();
         this.SuspendLayout();
         // 
         // tcAXI
         // 
         this.tcAXI.Controls.Add(this.tabController);
         this.tcAXI.Controls.Add(this.tabPCP);
         this.tcAXI.ItemSize = new System.Drawing.Size(56, 18);
         this.tcAXI.Location = new System.Drawing.Point(8, 6);
         this.tcAXI.Name = "tcAXI";
         this.tcAXI.SelectedIndex = 0;
         this.tcAXI.Size = new System.Drawing.Size(808, 465);
         this.tcAXI.TabIndex = 1;
         // 
         // tabController
         // 
         this.tabController.Controls.Add(this.gbxErrorLogging);
         this.tabController.Controls.Add(this.gbxControllerHandling);
         this.tabController.Controls.Add(this.gbxInterbusHandling);
         this.tabController.Controls.Add(this.gbxInputData);
         this.tabController.Controls.Add(this.gbxOutputData);
         this.tabController.Location = new System.Drawing.Point(4, 22);
         this.tabController.Name = "tabController";
         this.tabController.Size = new System.Drawing.Size(800, 439);
         this.tabController.TabIndex = 0;
         this.tabController.Text = "Controller";
         // 
         // gbxErrorLogging
         // 
         this.gbxErrorLogging.Controls.Add(this.tbxMessages);
         this.gbxErrorLogging.Location = new System.Drawing.Point(392, 6);
         this.gbxErrorLogging.Name = "gbxErrorLogging";
         this.gbxErrorLogging.Size = new System.Drawing.Size(398, 168);
         this.gbxErrorLogging.TabIndex = 42;
         this.gbxErrorLogging.TabStop = false;
         this.gbxErrorLogging.Text = "Error logging";
         // 
         // tbxMessages
         // 
         this.tbxMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.tbxMessages.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbxMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tbxMessages.Location = new System.Drawing.Point(3, 16);
         this.tbxMessages.Multiline = true;
         this.tbxMessages.Name = "tbxMessages";
         this.tbxMessages.ReadOnly = true;
         this.tbxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.tbxMessages.Size = new System.Drawing.Size(392, 149);
         this.tbxMessages.TabIndex = 2;
         // 
         // gbxControllerHandling
         // 
         this.gbxControllerHandling.Controls.Add(this.cbxWatchdog);
         this.gbxControllerHandling.Controls.Add(this.cbxError);
         this.gbxControllerHandling.Controls.Add(this.cbxRun);
         this.gbxControllerHandling.Controls.Add(this.btnClearLog);
         this.gbxControllerHandling.Controls.Add(this.cbxConnect);
         this.gbxControllerHandling.Controls.Add(this.btnDisable);
         this.gbxControllerHandling.Controls.Add(this.btnEnable);
         this.gbxControllerHandling.Controls.Add(this.tbxControllerType);
         this.gbxControllerHandling.Controls.Add(this.label7);
         this.gbxControllerHandling.Controls.Add(this.tbxConnection);
         this.gbxControllerHandling.Controls.Add(this.lblControllerConnection);
         this.gbxControllerHandling.Location = new System.Drawing.Point(16, 8);
         this.gbxControllerHandling.Name = "gbxControllerHandling";
         this.gbxControllerHandling.Size = new System.Drawing.Size(368, 166);
         this.gbxControllerHandling.TabIndex = 41;
         this.gbxControllerHandling.TabStop = false;
         this.gbxControllerHandling.Text = "Controller Handling";
         // 
         // cbxWatchdog
         // 
         this.cbxWatchdog.AutoCheck = false;
         this.cbxWatchdog.Location = new System.Drawing.Point(160, 121);
         this.cbxWatchdog.Name = "cbxWatchdog";
         this.cbxWatchdog.Size = new System.Drawing.Size(136, 18);
         this.cbxWatchdog.TabIndex = 29;
         this.cbxWatchdog.Text = "Watchdog";
         // 
         // cbxError
         // 
         this.cbxError.AutoCheck = false;
         this.cbxError.Location = new System.Drawing.Point(160, 102);
         this.cbxError.Name = "cbxError";
         this.cbxError.Size = new System.Drawing.Size(136, 18);
         this.cbxError.TabIndex = 28;
         this.cbxError.Text = "Error";
         // 
         // cbxRun
         // 
         this.cbxRun.AutoCheck = false;
         this.cbxRun.Location = new System.Drawing.Point(160, 83);
         this.cbxRun.Name = "cbxRun";
         this.cbxRun.Size = new System.Drawing.Size(136, 18);
         this.cbxRun.TabIndex = 9;
         this.cbxRun.Text = "Run";
         // 
         // btnClearLog
         // 
         this.btnClearLog.Location = new System.Drawing.Point(16, 129);
         this.btnClearLog.Name = "btnClearLog";
         this.btnClearLog.Size = new System.Drawing.Size(128, 23);
         this.btnClearLog.TabIndex = 3;
         this.btnClearLog.Text = "Clear error log";
         this.btnClearLog.UseVisualStyleBackColor = true;
         this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
         // 
         // cbxConnect
         // 
         this.cbxConnect.AutoCheck = false;
         this.cbxConnect.Location = new System.Drawing.Point(160, 64);
         this.cbxConnect.Name = "cbxConnect";
         this.cbxConnect.Size = new System.Drawing.Size(136, 18);
         this.cbxConnect.TabIndex = 8;
         this.cbxConnect.Text = "Connect";
         // 
         // btnDisable
         // 
         this.btnDisable.Location = new System.Drawing.Point(16, 96);
         this.btnDisable.Name = "btnDisable";
         this.btnDisable.Size = new System.Drawing.Size(128, 23);
         this.btnDisable.TabIndex = 7;
         this.btnDisable.Text = "Disable";
         this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
         // 
         // btnEnable
         // 
         this.btnEnable.Location = new System.Drawing.Point(16, 64);
         this.btnEnable.Name = "btnEnable";
         this.btnEnable.Size = new System.Drawing.Size(128, 23);
         this.btnEnable.TabIndex = 6;
         this.btnEnable.Text = "Enable";
         this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
         // 
         // tbxControllerType
         // 
         this.tbxControllerType.Location = new System.Drawing.Point(160, 16);
         this.tbxControllerType.Name = "tbxControllerType";
         this.tbxControllerType.ReadOnly = true;
         this.tbxControllerType.Size = new System.Drawing.Size(200, 20);
         this.tbxControllerType.TabIndex = 26;
         // 
         // label7
         // 
         this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label7.Location = new System.Drawing.Point(80, 16);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(64, 16);
         this.label7.TabIndex = 27;
         this.label7.Text = "Type";
         this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // tbxConnection
         // 
         this.tbxConnection.Location = new System.Drawing.Point(160, 40);
         this.tbxConnection.Name = "tbxConnection";
         this.tbxConnection.Size = new System.Drawing.Size(128, 20);
         this.tbxConnection.TabIndex = 1;
         // 
         // lblControllerConnection
         // 
         this.lblControllerConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblControllerConnection.Location = new System.Drawing.Point(72, 40);
         this.lblControllerConnection.Name = "lblControllerConnection";
         this.lblControllerConnection.Size = new System.Drawing.Size(72, 16);
         this.lblControllerConnection.TabIndex = 2;
         this.lblControllerConnection.Text = "Connection";
         this.lblControllerConnection.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // gbxInterbusHandling
         // 
         this.gbxInterbusHandling.Controls.Add(this.cbxBusPF);
         this.gbxInterbusHandling.Controls.Add(this.cbxBusBUS);
         this.gbxInterbusHandling.Controls.Add(this.cbxBusDetect);
         this.gbxInterbusHandling.Controls.Add(this.label11);
         this.gbxInterbusHandling.Controls.Add(this.tbxBusParameterregister2);
         this.gbxInterbusHandling.Controls.Add(this.label10);
         this.gbxInterbusHandling.Controls.Add(this.tbxBusParameterregister);
         this.gbxInterbusHandling.Controls.Add(this.cbxBusRun);
         this.gbxInterbusHandling.Controls.Add(this.cbxBusActive);
         this.gbxInterbusHandling.Controls.Add(this.cbxBusReady);
         this.gbxInterbusHandling.Controls.Add(this.btnAutoStart);
         this.gbxInterbusHandling.Controls.Add(this.btnAlarmStop);
         this.gbxInterbusHandling.Location = new System.Drawing.Point(16, 180);
         this.gbxInterbusHandling.Name = "gbxInterbusHandling";
         this.gbxInterbusHandling.Size = new System.Drawing.Size(648, 100);
         this.gbxInterbusHandling.TabIndex = 40;
         this.gbxInterbusHandling.TabStop = false;
         this.gbxInterbusHandling.Text = "INTERBUS Handling and Diagnostic";
         // 
         // cbxBusPF
         // 
         this.cbxBusPF.AutoCheck = false;
         this.cbxBusPF.Location = new System.Drawing.Point(288, 48);
         this.cbxBusPF.Name = "cbxBusPF";
         this.cbxBusPF.Size = new System.Drawing.Size(136, 16);
         this.cbxBusPF.TabIndex = 48;
         this.cbxBusPF.Text = "INTERBUS PF";
         // 
         // cbxBusBUS
         // 
         this.cbxBusBUS.AutoCheck = false;
         this.cbxBusBUS.Location = new System.Drawing.Point(288, 72);
         this.cbxBusBUS.Name = "cbxBusBUS";
         this.cbxBusBUS.Size = new System.Drawing.Size(136, 16);
         this.cbxBusBUS.TabIndex = 47;
         this.cbxBusBUS.Text = "INTERBUS Bus Fail";
         // 
         // cbxBusDetect
         // 
         this.cbxBusDetect.AutoCheck = false;
         this.cbxBusDetect.Location = new System.Drawing.Point(288, 24);
         this.cbxBusDetect.Name = "cbxBusDetect";
         this.cbxBusDetect.Size = new System.Drawing.Size(136, 16);
         this.cbxBusDetect.TabIndex = 46;
         this.cbxBusDetect.Text = "INTERBUS Detect";
         // 
         // label11
         // 
         this.label11.Location = new System.Drawing.Point(472, 56);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(136, 16);
         this.label11.TabIndex = 45;
         this.label11.Text = "Parameterregister 2  (hex)";
         // 
         // tbxBusParameterregister2
         // 
         this.tbxBusParameterregister2.Location = new System.Drawing.Point(432, 56);
         this.tbxBusParameterregister2.Name = "tbxBusParameterregister2";
         this.tbxBusParameterregister2.ReadOnly = true;
         this.tbxBusParameterregister2.Size = new System.Drawing.Size(32, 20);
         this.tbxBusParameterregister2.TabIndex = 44;
         // 
         // label10
         // 
         this.label10.Location = new System.Drawing.Point(472, 24);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(128, 16);
         this.label10.TabIndex = 43;
         this.label10.Text = "Parameterregister (hex)";
         // 
         // tbxBusParameterregister
         // 
         this.tbxBusParameterregister.Location = new System.Drawing.Point(432, 24);
         this.tbxBusParameterregister.Name = "tbxBusParameterregister";
         this.tbxBusParameterregister.ReadOnly = true;
         this.tbxBusParameterregister.Size = new System.Drawing.Size(32, 20);
         this.tbxBusParameterregister.TabIndex = 42;
         // 
         // cbxBusRun
         // 
         this.cbxBusRun.AutoCheck = false;
         this.cbxBusRun.Location = new System.Drawing.Point(160, 72);
         this.cbxBusRun.Name = "cbxBusRun";
         this.cbxBusRun.Size = new System.Drawing.Size(136, 16);
         this.cbxBusRun.TabIndex = 41;
         this.cbxBusRun.Text = "INTERBUS Run";
         // 
         // cbxBusActive
         // 
         this.cbxBusActive.AutoCheck = false;
         this.cbxBusActive.Location = new System.Drawing.Point(160, 48);
         this.cbxBusActive.Name = "cbxBusActive";
         this.cbxBusActive.Size = new System.Drawing.Size(136, 16);
         this.cbxBusActive.TabIndex = 40;
         this.cbxBusActive.Text = "INTERBUS Active";
         // 
         // cbxBusReady
         // 
         this.cbxBusReady.AutoCheck = false;
         this.cbxBusReady.Location = new System.Drawing.Point(160, 24);
         this.cbxBusReady.Name = "cbxBusReady";
         this.cbxBusReady.Size = new System.Drawing.Size(136, 16);
         this.cbxBusReady.TabIndex = 39;
         this.cbxBusReady.Text = "INTERBUS Ready";
         // 
         // btnAutoStart
         // 
         this.btnAutoStart.Location = new System.Drawing.Point(16, 56);
         this.btnAutoStart.Name = "btnAutoStart";
         this.btnAutoStart.Size = new System.Drawing.Size(128, 23);
         this.btnAutoStart.TabIndex = 38;
         this.btnAutoStart.Text = "Autostart";
         this.btnAutoStart.Click += new System.EventHandler(this.btnAutoStart_Click);
         // 
         // btnAlarmStop
         // 
         this.btnAlarmStop.Location = new System.Drawing.Point(16, 24);
         this.btnAlarmStop.Name = "btnAlarmStop";
         this.btnAlarmStop.Size = new System.Drawing.Size(128, 23);
         this.btnAlarmStop.TabIndex = 37;
         this.btnAlarmStop.Text = "Alarmstop";
         this.btnAlarmStop.Click += new System.EventHandler(this.btnAlarmStop_Click);
         // 
         // gbxInputData
         // 
         this.gbxInputData.Controls.Add(this.label12);
         this.gbxInputData.Controls.Add(this.label6);
         this.gbxInputData.Controls.Add(this.label1);
         this.gbxInputData.Controls.Add(this.label3);
         this.gbxInputData.Controls.Add(this.tbxInpByteArray);
         this.gbxInputData.Controls.Add(this.label2);
         this.gbxInputData.Controls.Add(this.tbxInpValue);
         this.gbxInputData.Controls.Add(this.cbxInpBit_1);
         this.gbxInputData.Controls.Add(this.cbxInpBit_0);
         this.gbxInputData.Location = new System.Drawing.Point(16, 284);
         this.gbxInputData.Name = "gbxInputData";
         this.gbxInputData.Size = new System.Drawing.Size(168, 144);
         this.gbxInputData.TabIndex = 38;
         this.gbxInputData.TabStop = false;
         this.gbxInputData.Text = "Input Data (read only)";
         // 
         // label12
         // 
         this.label12.Location = new System.Drawing.Point(16, 96);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(112, 16);
         this.label12.TabIndex = 26;
         this.label12.Text = "ByteArray Variable";
         // 
         // label6
         // 
         this.label6.Location = new System.Drawing.Point(16, 56);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(96, 16);
         this.label6.TabIndex = 25;
         this.label6.Text = "Integer Variable";
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(16, 24);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(96, 16);
         this.label1.TabIndex = 24;
         this.label1.Text = "Boolean Variables";
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(128, 112);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(32, 16);
         this.label3.TabIndex = 23;
         this.label3.Text = "(hex)";
         // 
         // tbxInpByteArray
         // 
         this.tbxInpByteArray.Location = new System.Drawing.Point(16, 112);
         this.tbxInpByteArray.Name = "tbxInpByteArray";
         this.tbxInpByteArray.ReadOnly = true;
         this.tbxInpByteArray.Size = new System.Drawing.Size(104, 20);
         this.tbxInpByteArray.TabIndex = 22;
         this.tbxInpByteArray.Text = "-";
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(96, 72);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(40, 16);
         this.label2.TabIndex = 21;
         this.label2.Text = "(hex)";
         // 
         // tbxInpValue
         // 
         this.tbxInpValue.Location = new System.Drawing.Point(16, 72);
         this.tbxInpValue.Name = "tbxInpValue";
         this.tbxInpValue.ReadOnly = true;
         this.tbxInpValue.Size = new System.Drawing.Size(72, 20);
         this.tbxInpValue.TabIndex = 20;
         this.tbxInpValue.Text = "-";
         // 
         // cbxInpBit_1
         // 
         this.cbxInpBit_1.AutoCheck = false;
         this.cbxInpBit_1.Location = new System.Drawing.Point(72, 40);
         this.cbxInpBit_1.Name = "cbxInpBit_1";
         this.cbxInpBit_1.Size = new System.Drawing.Size(48, 16);
         this.cbxInpBit_1.TabIndex = 19;
         this.cbxInpBit_1.Text = "Bit 1";
         // 
         // cbxInpBit_0
         // 
         this.cbxInpBit_0.AutoCheck = false;
         this.cbxInpBit_0.Location = new System.Drawing.Point(16, 40);
         this.cbxInpBit_0.Name = "cbxInpBit_0";
         this.cbxInpBit_0.Size = new System.Drawing.Size(48, 16);
         this.cbxInpBit_0.TabIndex = 18;
         this.cbxInpBit_0.Text = "Bit 0";
         // 
         // gbxOutputData
         // 
         this.gbxOutputData.Controls.Add(this.btnWriteValues);
         this.gbxOutputData.Controls.Add(this.tbxOutputByteArray);
         this.gbxOutputData.Controls.Add(this.tbxOutputValue);
         this.gbxOutputData.Controls.Add(this.cbxOutputBit_1);
         this.gbxOutputData.Controls.Add(this.cbxOutputBit_0);
         this.gbxOutputData.Controls.Add(this.label13);
         this.gbxOutputData.Controls.Add(this.label14);
         this.gbxOutputData.Controls.Add(this.label15);
         this.gbxOutputData.Controls.Add(this.label16);
         this.gbxOutputData.Controls.Add(this.label17);
         this.gbxOutputData.Location = new System.Drawing.Point(200, 284);
         this.gbxOutputData.Name = "gbxOutputData";
         this.gbxOutputData.Size = new System.Drawing.Size(240, 144);
         this.gbxOutputData.TabIndex = 39;
         this.gbxOutputData.TabStop = false;
         this.gbxOutputData.Text = "Output Data (read/write)";
         // 
         // btnWriteValues
         // 
         this.btnWriteValues.Location = new System.Drawing.Point(152, 112);
         this.btnWriteValues.Name = "btnWriteValues";
         this.btnWriteValues.Size = new System.Drawing.Size(80, 23);
         this.btnWriteValues.TabIndex = 42;
         this.btnWriteValues.Text = "Write Values";
         this.btnWriteValues.Click += new System.EventHandler(this.btnWriteValues_Click);
         // 
         // tbxOutputByteArray
         // 
         this.tbxOutputByteArray.Location = new System.Drawing.Point(16, 112);
         this.tbxOutputByteArray.Name = "tbxOutputByteArray";
         this.tbxOutputByteArray.Size = new System.Drawing.Size(104, 20);
         this.tbxOutputByteArray.TabIndex = 30;
         this.tbxOutputByteArray.Enter += new System.EventHandler(this.OnOutput_Enter);
         // 
         // tbxOutputValue
         // 
         this.tbxOutputValue.Location = new System.Drawing.Point(16, 72);
         this.tbxOutputValue.Name = "tbxOutputValue";
         this.tbxOutputValue.Size = new System.Drawing.Size(72, 20);
         this.tbxOutputValue.TabIndex = 29;
         this.tbxOutputValue.Enter += new System.EventHandler(this.OnOutput_Enter);
         // 
         // cbxOutputBit_1
         // 
         this.cbxOutputBit_1.Location = new System.Drawing.Point(72, 40);
         this.cbxOutputBit_1.Name = "cbxOutputBit_1";
         this.cbxOutputBit_1.Size = new System.Drawing.Size(48, 16);
         this.cbxOutputBit_1.TabIndex = 28;
         this.cbxOutputBit_1.Text = "Bit 1";
         this.cbxOutputBit_1.Click += new System.EventHandler(this.OnOutput_Enter);
         // 
         // cbxOutputBit_0
         // 
         this.cbxOutputBit_0.Location = new System.Drawing.Point(16, 40);
         this.cbxOutputBit_0.Name = "cbxOutputBit_0";
         this.cbxOutputBit_0.Size = new System.Drawing.Size(48, 16);
         this.cbxOutputBit_0.TabIndex = 27;
         this.cbxOutputBit_0.Text = "Bit 0";
         this.cbxOutputBit_0.Click += new System.EventHandler(this.OnOutput_Enter);
         // 
         // label13
         // 
         this.label13.Location = new System.Drawing.Point(16, 96);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(112, 16);
         this.label13.TabIndex = 26;
         this.label13.Text = "ByteArray Variable";
         // 
         // label14
         // 
         this.label14.Location = new System.Drawing.Point(16, 56);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(96, 16);
         this.label14.TabIndex = 25;
         this.label14.Text = "Integer Variable";
         // 
         // label15
         // 
         this.label15.Location = new System.Drawing.Point(16, 24);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(96, 16);
         this.label15.TabIndex = 24;
         this.label15.Text = "Boolean Variables";
         // 
         // label16
         // 
         this.label16.Location = new System.Drawing.Point(120, 112);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(32, 16);
         this.label16.TabIndex = 23;
         this.label16.Text = "(hex)";
         // 
         // label17
         // 
         this.label17.Location = new System.Drawing.Point(88, 72);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(40, 16);
         this.label17.TabIndex = 21;
         this.label17.Text = "(hex)";
         // 
         // tabPCP
         // 
         this.tabPCP.Controls.Add(this.cbxReadDataError);
         this.tabPCP.Controls.Add(this.btnClear);
         this.tabPCP.Controls.Add(this.lbxLogging);
         this.tabPCP.Controls.Add(this.cbxPCPError);
         this.tabPCP.Controls.Add(this.cbxPCPReady);
         this.tabPCP.Controls.Add(this.cbxWriteDataDone);
         this.tabPCP.Controls.Add(this.cbxReadDataValid);
         this.tabPCP.Controls.Add(this.btnWriteData_2);
         this.tabPCP.Controls.Add(this.btnReadData_2);
         this.tabPCP.Controls.Add(this.btnDisable_2);
         this.tabPCP.Controls.Add(this.btnEnable_2);
         this.tabPCP.Location = new System.Drawing.Point(4, 22);
         this.tabPCP.Name = "tabPCP";
         this.tabPCP.Size = new System.Drawing.Size(800, 439);
         this.tabPCP.TabIndex = 2;
         this.tabPCP.Text = "PCP Communication";
         // 
         // cbxReadDataError
         // 
         this.cbxReadDataError.AutoCheck = false;
         this.cbxReadDataError.Location = new System.Drawing.Point(144, 102);
         this.cbxReadDataError.Name = "cbxReadDataError";
         this.cbxReadDataError.Size = new System.Drawing.Size(114, 16);
         this.cbxReadDataError.TabIndex = 13;
         this.cbxReadDataError.Text = "ReadDataError";
         // 
         // btnClear
         // 
         this.btnClear.Location = new System.Drawing.Point(264, 356);
         this.btnClear.Name = "btnClear";
         this.btnClear.Size = new System.Drawing.Size(528, 24);
         this.btnClear.TabIndex = 10;
         this.btnClear.Text = "Clear";
         this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
         // 
         // lbxLogging
         // 
         this.lbxLogging.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lbxLogging.ItemHeight = 15;
         this.lbxLogging.Location = new System.Drawing.Point(264, 16);
         this.lbxLogging.Name = "lbxLogging";
         this.lbxLogging.Size = new System.Drawing.Size(528, 334);
         this.lbxLogging.TabIndex = 9;
         // 
         // cbxPCPError
         // 
         this.cbxPCPError.AutoCheck = false;
         this.cbxPCPError.Location = new System.Drawing.Point(144, 32);
         this.cbxPCPError.Name = "cbxPCPError";
         this.cbxPCPError.Size = new System.Drawing.Size(114, 16);
         this.cbxPCPError.TabIndex = 8;
         this.cbxPCPError.Text = "PCP Error";
         // 
         // cbxPCPReady
         // 
         this.cbxPCPReady.AutoCheck = false;
         this.cbxPCPReady.Location = new System.Drawing.Point(144, 16);
         this.cbxPCPReady.Name = "cbxPCPReady";
         this.cbxPCPReady.Size = new System.Drawing.Size(114, 16);
         this.cbxPCPReady.TabIndex = 7;
         this.cbxPCPReady.Text = "PCP Ready";
         // 
         // cbxWriteDataDone
         // 
         this.cbxWriteDataDone.AutoCheck = false;
         this.cbxWriteDataDone.Location = new System.Drawing.Point(144, 120);
         this.cbxWriteDataDone.Name = "cbxWriteDataDone";
         this.cbxWriteDataDone.Size = new System.Drawing.Size(114, 16);
         this.cbxWriteDataDone.TabIndex = 6;
         this.cbxWriteDataDone.Text = "WriteDataDone";
         // 
         // cbxReadDataValid
         // 
         this.cbxReadDataValid.AutoCheck = false;
         this.cbxReadDataValid.Location = new System.Drawing.Point(144, 83);
         this.cbxReadDataValid.Name = "cbxReadDataValid";
         this.cbxReadDataValid.Size = new System.Drawing.Size(114, 16);
         this.cbxReadDataValid.TabIndex = 5;
         this.cbxReadDataValid.Text = "ReadDataValid";
         // 
         // btnWriteData_2
         // 
         this.btnWriteData_2.Location = new System.Drawing.Point(16, 112);
         this.btnWriteData_2.Name = "btnWriteData_2";
         this.btnWriteData_2.Size = new System.Drawing.Size(104, 24);
         this.btnWriteData_2.TabIndex = 4;
         this.btnWriteData_2.Text = "Write Data CR2";
         this.btnWriteData_2.Click += new System.EventHandler(this.btnWriteData_2_Click);
         // 
         // btnReadData_2
         // 
         this.btnReadData_2.Location = new System.Drawing.Point(16, 80);
         this.btnReadData_2.Name = "btnReadData_2";
         this.btnReadData_2.Size = new System.Drawing.Size(104, 24);
         this.btnReadData_2.TabIndex = 3;
         this.btnReadData_2.Text = "Read Data CR2";
         this.btnReadData_2.Click += new System.EventHandler(this.btnReadData_2_Click);
         // 
         // btnDisable_2
         // 
         this.btnDisable_2.Location = new System.Drawing.Point(16, 48);
         this.btnDisable_2.Name = "btnDisable_2";
         this.btnDisable_2.Size = new System.Drawing.Size(104, 24);
         this.btnDisable_2.TabIndex = 2;
         this.btnDisable_2.Text = "Disable CR2";
         this.btnDisable_2.Click += new System.EventHandler(this.btnDisable_2_Click);
         // 
         // btnEnable_2
         // 
         this.btnEnable_2.Location = new System.Drawing.Point(16, 16);
         this.btnEnable_2.Name = "btnEnable_2";
         this.btnEnable_2.Size = new System.Drawing.Size(104, 24);
         this.btnEnable_2.TabIndex = 1;
         this.btnEnable_2.Text = "Enable CR2";
         this.btnEnable_2.Click += new System.EventHandler(this.btnEnable_2_Click);
         // 
         // tmrMainFormUpdate
         // 
         this.tmrMainFormUpdate.Enabled = true;
         this.tmrMainFormUpdate.Tick += new System.EventHandler(this.tmrMainFormUpdate_Tick);
         // 
         // frmMain
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(824, 483);
         this.Controls.Add(this.tcAXI);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "frmMain";
         this.Text = "HFI Demo C#";
         this.Load += new System.EventHandler(this.frmMain_Load);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
         this.tcAXI.ResumeLayout(false);
         this.tabController.ResumeLayout(false);
         this.gbxErrorLogging.ResumeLayout(false);
         this.gbxErrorLogging.PerformLayout();
         this.gbxControllerHandling.ResumeLayout(false);
         this.gbxControllerHandling.PerformLayout();
         this.gbxInterbusHandling.ResumeLayout(false);
         this.gbxInterbusHandling.PerformLayout();
         this.gbxInputData.ResumeLayout(false);
         this.gbxInputData.PerformLayout();
         this.gbxOutputData.ResumeLayout(false);
         this.gbxOutputData.PerformLayout();
         this.tabPCP.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TabControl tcAXI;
      private System.Windows.Forms.TabPage tabController;
      private System.Windows.Forms.GroupBox gbxControllerHandling;
      private System.Windows.Forms.CheckBox cbxRun;
      private System.Windows.Forms.CheckBox cbxConnect;
      private System.Windows.Forms.Button btnDisable;
      private System.Windows.Forms.Button btnEnable;
      private System.Windows.Forms.TextBox tbxControllerType;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox tbxConnection;
      private System.Windows.Forms.Label lblControllerConnection;
      private System.Windows.Forms.GroupBox gbxInterbusHandling;
      private System.Windows.Forms.CheckBox cbxBusPF;
      private System.Windows.Forms.CheckBox cbxBusBUS;
      private System.Windows.Forms.CheckBox cbxBusDetect;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.TextBox tbxBusParameterregister2;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.TextBox tbxBusParameterregister;
      private System.Windows.Forms.CheckBox cbxBusRun;
      private System.Windows.Forms.CheckBox cbxBusActive;
      private System.Windows.Forms.CheckBox cbxBusReady;
      private System.Windows.Forms.Button btnAutoStart;
      private System.Windows.Forms.Button btnAlarmStop;
      private System.Windows.Forms.GroupBox gbxInputData;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox tbxInpByteArray;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox tbxInpValue;
      private System.Windows.Forms.CheckBox cbxInpBit_1;
      private System.Windows.Forms.CheckBox cbxInpBit_0;
      private System.Windows.Forms.GroupBox gbxOutputData;
      private System.Windows.Forms.Button btnWriteValues;
      private System.Windows.Forms.TextBox tbxOutputByteArray;
      private System.Windows.Forms.TextBox tbxOutputValue;
      private System.Windows.Forms.CheckBox cbxOutputBit_1;
      private System.Windows.Forms.CheckBox cbxOutputBit_0;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.TabPage tabPCP;
      private System.Windows.Forms.Button btnClear;
      private System.Windows.Forms.ListBox lbxLogging;
      private System.Windows.Forms.CheckBox cbxPCPError;
      private System.Windows.Forms.CheckBox cbxPCPReady;
      private System.Windows.Forms.CheckBox cbxWriteDataDone;
      private System.Windows.Forms.CheckBox cbxReadDataValid;
      private System.Windows.Forms.Button btnWriteData_2;
      private System.Windows.Forms.Button btnReadData_2;
      private System.Windows.Forms.Button btnDisable_2;
      private System.Windows.Forms.Button btnEnable_2;
      private System.Windows.Forms.GroupBox gbxErrorLogging;
      private System.Windows.Forms.TextBox tbxMessages;
      private System.Windows.Forms.Button btnClearLog;
      private System.Windows.Forms.Timer tmrMainFormUpdate;
      private System.Windows.Forms.CheckBox cbxWatchdog;
      private System.Windows.Forms.CheckBox cbxError;
      private System.Windows.Forms.CheckBox cbxReadDataError;
   }
}

