// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.ctrlController
// Assembly: HFI_Visu_FX46, Version=3.2.6053.23250, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: A9FB10B7-9AE3-4F4C-88CF-1D5F3BF257DC
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Visu_FX46.dll

using PhoenixContact.HFI.Inline;
using PhoenixContact.HFI.Visualization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PhoenixContact.HFI.Visualization
{
  [ToolboxBitmap(typeof (ctrlController), "picController.bmp")]
  public class ctrlController : UserControl
  {
    private List<IController> _varController;
    private IController _myController;
    private System.Windows.Forms.Timer _locTimer;
    private SelectControllerHandler _hdSelectController;
    private SelectControllerHandler _hdBeforeDeactivateController;
    private bool _fpControllerReady;
    private Thread tDisable;
    private GroupBox grpProperties;
    private Label lblPdDataCycleTime;
    private TextBox txtProcessDataCycleTime;
    private Label lblMailboxCycleTime;
    private TextBox txtMailboxCycleTime;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblName;
    private TextBox txtName;
    private Label lblInputObjectCount;
    private Label lblInputObjectStartAddress;
    private Label lblInputObjectEndAddress;
    private Label lblOutputObjectEndAddress;
    private Label lblOutputObjectStartAddress;
    private Label lblOutputObjectCount;
    private TextBox txtInputObjectCount;
    private TextBox txtInputObjectStartAddress;
    private TextBox txtInputObjectEndAddress;
    private TextBox txtOutputObjectCount;
    private TextBox txtOutputObjectStartAddress;
    private TextBox txtOutputObjectEndAddress;
    private Label label1;
    private Label label2;
    private GroupBox grpControl;
    private CheckBox cbxConnect;
    private Button btnEnable;
    private Button btnDisable;
    private Button btnAutoStart;
    private Button btnClearWatchdog;
    private CheckBox cbxWatchdogOccurred;
    private CheckBox cbxError;
    private Label lblConnectionstring;
    private TextBox txtConnectionstring;
    private GroupBox grpController;
    private CheckBox cbxRun;
    private GroupBox gbxControllerList;
    private Label lblObjects;
    private ListBox listController;
    private Button btnPropertiesShow;
    private Button btnGetVersionInfo;
    private Button btnWriteChanges;
    private Button btnCancel;
    private CheckBox cbxWatchdogDeaktivate;
    private Label lblSvcFileName;
    private Label lblStartup;
    private ComboBox cmbControllerStartup;
    private ComboBox cmbWatchdogTimeout;
    private TextBox tbxInternalState;
    private Container components;

    private void InitializeComponent()
    {
      this.grpController = new GroupBox();
      this.btnPropertiesShow = new Button();
      this.gbxControllerList = new GroupBox();
      this.lblObjects = new Label();
      this.listController = new ListBox();
      this.grpProperties = new GroupBox();
      this.cmbWatchdogTimeout = new ComboBox();
      this.cmbControllerStartup = new ComboBox();
      this.cbxWatchdogDeaktivate = new CheckBox();
      this.btnWriteChanges = new Button();
      this.btnCancel = new Button();
      this.lblConnectionstring = new Label();
      this.txtConnectionstring = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.txtOutputObjectEndAddress = new TextBox();
      this.txtOutputObjectStartAddress = new TextBox();
      this.txtOutputObjectCount = new TextBox();
      this.txtInputObjectEndAddress = new TextBox();
      this.txtInputObjectStartAddress = new TextBox();
      this.txtInputObjectCount = new TextBox();
      this.lblOutputObjectEndAddress = new Label();
      this.lblOutputObjectStartAddress = new Label();
      this.lblOutputObjectCount = new Label();
      this.lblInputObjectEndAddress = new Label();
      this.lblInputObjectStartAddress = new Label();
      this.lblInputObjectCount = new Label();
      this.lblPdDataCycleTime = new Label();
      this.txtProcessDataCycleTime = new TextBox();
      this.lblMailboxCycleTime = new Label();
      this.txtMailboxCycleTime = new TextBox();
      this.lblSvcFileName = new Label();
      this.lblStartup = new Label();
      this.lblDescription = new Label();
      this.txtDescription = new TextBox();
      this.lblName = new Label();
      this.txtName = new TextBox();
      this.grpControl = new GroupBox();
      this.tbxInternalState = new TextBox();
      this.btnGetVersionInfo = new Button();
      this.cbxRun = new CheckBox();
      this.cbxError = new CheckBox();
      this.cbxWatchdogOccurred = new CheckBox();
      this.btnClearWatchdog = new Button();
      this.btnAutoStart = new Button();
      this.btnDisable = new Button();
      this.cbxConnect = new CheckBox();
      this.btnEnable = new Button();
      this.grpController.SuspendLayout();
      this.gbxControllerList.SuspendLayout();
      this.grpProperties.SuspendLayout();
      this.grpControl.SuspendLayout();
      this.SuspendLayout();
      this.grpController.Controls.Add((Control) this.btnPropertiesShow);
      this.grpController.Controls.Add((Control) this.gbxControllerList);
      this.grpController.Controls.Add((Control) this.grpProperties);
      this.grpController.Controls.Add((Control) this.grpControl);
      this.grpController.Dock = DockStyle.Fill;
      this.grpController.Location = new Point(0, 0);
      this.grpController.Name = "grpController";
      this.grpController.Size = new Size(704, 292);
      this.grpController.TabIndex = 0;
      this.grpController.TabStop = false;
      this.btnPropertiesShow.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnPropertiesShow.Image = (Image) Resources.undo;
      this.btnPropertiesShow.Location = new Point(295, 10);
      this.btnPropertiesShow.Name = "btnPropertiesShow";
      this.btnPropertiesShow.Size = new Size(21, 21);
      this.btnPropertiesShow.TabIndex = 1;
      this.btnPropertiesShow.UseVisualStyleBackColor = true;
      this.btnPropertiesShow.Click += new EventHandler(this.btnPropertiesShow_Click);
      this.gbxControllerList.Controls.Add((Control) this.lblObjects);
      this.gbxControllerList.Controls.Add((Control) this.listController);
      this.gbxControllerList.Location = new Point(8, 16);
      this.gbxControllerList.Name = "gbxControllerList";
      this.gbxControllerList.Size = new Size(160, 269);
      this.gbxControllerList.TabIndex = 0;
      this.gbxControllerList.TabStop = false;
      this.gbxControllerList.Text = "Controller List:";
      this.lblObjects.Location = new Point(2, 18);
      this.lblObjects.Name = "lblObjects";
      this.lblObjects.Size = new Size(152, 18);
      this.lblObjects.TabIndex = 8;
      this.lblObjects.Text = "Available Controller: 0";
      this.lblObjects.TextAlign = ContentAlignment.BottomLeft;
      this.listController.Location = new Point(2, 39);
      this.listController.Name = "listController";
      this.listController.Size = new Size(154, 225);
      this.listController.TabIndex = 0;
      this.listController.SelectedIndexChanged += new EventHandler(this.listController_SelectedIndexChanged);
      this.grpProperties.Controls.Add((Control) this.cmbWatchdogTimeout);
      this.grpProperties.Controls.Add((Control) this.cmbControllerStartup);
      this.grpProperties.Controls.Add((Control) this.cbxWatchdogDeaktivate);
      this.grpProperties.Controls.Add((Control) this.btnWriteChanges);
      this.grpProperties.Controls.Add((Control) this.btnCancel);
      this.grpProperties.Controls.Add((Control) this.lblConnectionstring);
      this.grpProperties.Controls.Add((Control) this.txtConnectionstring);
      this.grpProperties.Controls.Add((Control) this.label2);
      this.grpProperties.Controls.Add((Control) this.label1);
      this.grpProperties.Controls.Add((Control) this.txtOutputObjectEndAddress);
      this.grpProperties.Controls.Add((Control) this.txtOutputObjectStartAddress);
      this.grpProperties.Controls.Add((Control) this.txtOutputObjectCount);
      this.grpProperties.Controls.Add((Control) this.txtInputObjectEndAddress);
      this.grpProperties.Controls.Add((Control) this.txtInputObjectStartAddress);
      this.grpProperties.Controls.Add((Control) this.txtInputObjectCount);
      this.grpProperties.Controls.Add((Control) this.lblOutputObjectEndAddress);
      this.grpProperties.Controls.Add((Control) this.lblOutputObjectStartAddress);
      this.grpProperties.Controls.Add((Control) this.lblOutputObjectCount);
      this.grpProperties.Controls.Add((Control) this.lblInputObjectEndAddress);
      this.grpProperties.Controls.Add((Control) this.lblInputObjectStartAddress);
      this.grpProperties.Controls.Add((Control) this.lblInputObjectCount);
      this.grpProperties.Controls.Add((Control) this.lblPdDataCycleTime);
      this.grpProperties.Controls.Add((Control) this.txtProcessDataCycleTime);
      this.grpProperties.Controls.Add((Control) this.lblMailboxCycleTime);
      this.grpProperties.Controls.Add((Control) this.txtMailboxCycleTime);
      this.grpProperties.Controls.Add((Control) this.lblSvcFileName);
      this.grpProperties.Controls.Add((Control) this.lblStartup);
      this.grpProperties.Controls.Add((Control) this.lblDescription);
      this.grpProperties.Controls.Add((Control) this.txtDescription);
      this.grpProperties.Controls.Add((Control) this.lblName);
      this.grpProperties.Controls.Add((Control) this.txtName);
      this.grpProperties.Location = new Point(327, 16);
      this.grpProperties.Name = "grpProperties";
      this.grpProperties.Size = new Size(368, 269);
      this.grpProperties.TabIndex = 0;
      this.grpProperties.TabStop = false;
      this.grpProperties.Text = "Controller Properties";
      this.cmbWatchdogTimeout.FormattingEnabled = true;
      this.cmbWatchdogTimeout.Location = new Point((int) sbyte.MaxValue, 92);
      this.cmbWatchdogTimeout.Name = "cmbWatchdogTimeout";
      this.cmbWatchdogTimeout.Size = new Size(176, 21);
      this.cmbWatchdogTimeout.TabIndex = 3;
      this.cmbControllerStartup.FormattingEnabled = true;
      this.cmbControllerStartup.Location = new Point((int) sbyte.MaxValue, 69);
      this.cmbControllerStartup.Name = "cmbControllerStartup";
      this.cmbControllerStartup.Size = new Size(176, 21);
      this.cmbControllerStartup.TabIndex = 2;
      this.cbxWatchdogDeaktivate.AutoSize = true;
      this.cbxWatchdogDeaktivate.CheckAlign = ContentAlignment.MiddleRight;
      this.cbxWatchdogDeaktivate.Location = new Point(8, 117);
      this.cbxWatchdogDeaktivate.Name = "cbxWatchdogDeaktivate";
      this.cbxWatchdogDeaktivate.Size = new Size(134, 17);
      this.cbxWatchdogDeaktivate.TabIndex = 4;
      this.cbxWatchdogDeaktivate.Text = "Watchdog Deaktivate:";
      this.cbxWatchdogDeaktivate.TextAlign = ContentAlignment.MiddleRight;
      this.cbxWatchdogDeaktivate.UseVisualStyleBackColor = true;
      this.btnWriteChanges.Location = new Point(268, 234);
      this.btnWriteChanges.Name = "btnWriteChanges";
      this.btnWriteChanges.Size = new Size(94, 23);
      this.btnWriteChanges.TabIndex = 9;
      this.btnWriteChanges.Text = "Write Changes";
      this.btnWriteChanges.UseVisualStyleBackColor = true;
      this.btnWriteChanges.Click += new EventHandler(this.btnWriteChanges_Click);
      this.btnCancel.Location = new Point(107, 235);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(155, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Refresh / Cancel Changes";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblConnectionstring.Location = new Point(6, 136);
      this.lblConnectionstring.Name = "lblConnectionstring";
      this.lblConnectionstring.Size = new Size(120, 18);
      this.lblConnectionstring.TabIndex = 33;
      this.lblConnectionstring.Text = "Connection String:";
      this.lblConnectionstring.TextAlign = ContentAlignment.MiddleRight;
      this.txtConnectionstring.Location = new Point((int) sbyte.MaxValue, 136);
      this.txtConnectionstring.Name = "txtConnectionstring";
      this.txtConnectionstring.Size = new Size(121, 20);
      this.txtConnectionstring.TabIndex = 5;
      this.txtConnectionstring.Text = "-";
      this.label2.Location = new Point(319, 158);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 18);
      this.label2.TabIndex = 31;
      this.label2.Text = "[ms]";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(175, 158);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 18);
      this.label1.TabIndex = 30;
      this.label1.Text = "[ms]";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.txtOutputObjectEndAddress.Location = new Point(319, 202);
      this.txtOutputObjectEndAddress.Name = "txtOutputObjectEndAddress";
      this.txtOutputObjectEndAddress.ReadOnly = true;
      this.txtOutputObjectEndAddress.Size = new Size(32, 20);
      this.txtOutputObjectEndAddress.TabIndex = 29;
      this.txtOutputObjectEndAddress.Text = "-";
      this.txtOutputObjectStartAddress.Location = new Point(223, 202);
      this.txtOutputObjectStartAddress.Name = "txtOutputObjectStartAddress";
      this.txtOutputObjectStartAddress.ReadOnly = true;
      this.txtOutputObjectStartAddress.Size = new Size(32, 20);
      this.txtOutputObjectStartAddress.TabIndex = 28;
      this.txtOutputObjectStartAddress.Text = "-";
      this.txtOutputObjectCount.Location = new Point((int) sbyte.MaxValue, 202);
      this.txtOutputObjectCount.Name = "txtOutputObjectCount";
      this.txtOutputObjectCount.ReadOnly = true;
      this.txtOutputObjectCount.Size = new Size(32, 20);
      this.txtOutputObjectCount.TabIndex = 27;
      this.txtOutputObjectCount.Text = "-";
      this.txtInputObjectEndAddress.Location = new Point(319, 180);
      this.txtInputObjectEndAddress.Name = "txtInputObjectEndAddress";
      this.txtInputObjectEndAddress.ReadOnly = true;
      this.txtInputObjectEndAddress.Size = new Size(32, 20);
      this.txtInputObjectEndAddress.TabIndex = 26;
      this.txtInputObjectEndAddress.Text = "-";
      this.txtInputObjectStartAddress.Location = new Point(223, 180);
      this.txtInputObjectStartAddress.Name = "txtInputObjectStartAddress";
      this.txtInputObjectStartAddress.ReadOnly = true;
      this.txtInputObjectStartAddress.Size = new Size(32, 20);
      this.txtInputObjectStartAddress.TabIndex = 25;
      this.txtInputObjectStartAddress.Text = "-";
      this.txtInputObjectCount.Location = new Point((int) sbyte.MaxValue, 180);
      this.txtInputObjectCount.Name = "txtInputObjectCount";
      this.txtInputObjectCount.ReadOnly = true;
      this.txtInputObjectCount.Size = new Size(32, 20);
      this.txtInputObjectCount.TabIndex = 24;
      this.txtInputObjectCount.Text = "-";
      this.lblOutputObjectEndAddress.Location = new Point(254, 202);
      this.lblOutputObjectEndAddress.Name = "lblOutputObjectEndAddress";
      this.lblOutputObjectEndAddress.Size = new Size(64, 18);
      this.lblOutputObjectEndAddress.TabIndex = 23;
      this.lblOutputObjectEndAddress.Text = "End Addr.:";
      this.lblOutputObjectEndAddress.TextAlign = ContentAlignment.MiddleRight;
      this.lblOutputObjectStartAddress.Location = new Point(158, 202);
      this.lblOutputObjectStartAddress.Name = "lblOutputObjectStartAddress";
      this.lblOutputObjectStartAddress.Size = new Size(64, 18);
      this.lblOutputObjectStartAddress.TabIndex = 22;
      this.lblOutputObjectStartAddress.Text = "Start Addr.:";
      this.lblOutputObjectStartAddress.TextAlign = ContentAlignment.MiddleRight;
      this.lblOutputObjectCount.Location = new Point(6, 202);
      this.lblOutputObjectCount.Name = "lblOutputObjectCount";
      this.lblOutputObjectCount.Size = new Size(120, 18);
      this.lblOutputObjectCount.TabIndex = 21;
      this.lblOutputObjectCount.Text = "Output Object Counter:";
      this.lblOutputObjectCount.TextAlign = ContentAlignment.MiddleRight;
      this.lblInputObjectEndAddress.Location = new Point(254, 180);
      this.lblInputObjectEndAddress.Name = "lblInputObjectEndAddress";
      this.lblInputObjectEndAddress.Size = new Size(64, 18);
      this.lblInputObjectEndAddress.TabIndex = 20;
      this.lblInputObjectEndAddress.Text = "End Addr.:";
      this.lblInputObjectEndAddress.TextAlign = ContentAlignment.MiddleRight;
      this.lblInputObjectStartAddress.Location = new Point(157, 180);
      this.lblInputObjectStartAddress.Name = "lblInputObjectStartAddress";
      this.lblInputObjectStartAddress.Size = new Size(64, 18);
      this.lblInputObjectStartAddress.TabIndex = 19;
      this.lblInputObjectStartAddress.Text = "Start Addr.:";
      this.lblInputObjectStartAddress.TextAlign = ContentAlignment.MiddleRight;
      this.lblInputObjectCount.Location = new Point(6, 180);
      this.lblInputObjectCount.Name = "lblInputObjectCount";
      this.lblInputObjectCount.Size = new Size(120, 18);
      this.lblInputObjectCount.TabIndex = 18;
      this.lblInputObjectCount.Text = "Input Object Counter:";
      this.lblInputObjectCount.TextAlign = ContentAlignment.MiddleRight;
      this.lblPdDataCycleTime.Location = new Point(6, 158);
      this.lblPdDataCycleTime.Name = "lblPdDataCycleTime";
      this.lblPdDataCycleTime.Size = new Size(120, 18);
      this.lblPdDataCycleTime.TabIndex = 17;
      this.lblPdDataCycleTime.Text = "Process Data Cycle:";
      this.lblPdDataCycleTime.TextAlign = ContentAlignment.MiddleRight;
      this.txtProcessDataCycleTime.Location = new Point((int) sbyte.MaxValue, 158);
      this.txtProcessDataCycleTime.Name = "txtProcessDataCycleTime";
      this.txtProcessDataCycleTime.Size = new Size(48, 20);
      this.txtProcessDataCycleTime.TabIndex = 6;
      this.txtProcessDataCycleTime.Text = "-";
      this.lblMailboxCycleTime.Location = new Point(215, 158);
      this.lblMailboxCycleTime.Name = "lblMailboxCycleTime";
      this.lblMailboxCycleTime.Size = new Size(48, 18);
      this.lblMailboxCycleTime.TabIndex = 15;
      this.lblMailboxCycleTime.Text = "Mailbox:";
      this.lblMailboxCycleTime.TextAlign = ContentAlignment.MiddleRight;
      this.txtMailboxCycleTime.Location = new Point(271, 158);
      this.txtMailboxCycleTime.Name = "txtMailboxCycleTime";
      this.txtMailboxCycleTime.Size = new Size(48, 20);
      this.txtMailboxCycleTime.TabIndex = 7;
      this.txtMailboxCycleTime.Text = "-";
      this.lblSvcFileName.Location = new Point(6, 92);
      this.lblSvcFileName.Name = "lblSvcFileName";
      this.lblSvcFileName.Size = new Size(120, 18);
      this.lblSvcFileName.TabIndex = 13;
      this.lblSvcFileName.Text = "Watchdog Timeout:";
      this.lblSvcFileName.TextAlign = ContentAlignment.MiddleRight;
      this.lblStartup.Location = new Point(6, 67);
      this.lblStartup.Name = "lblStartup";
      this.lblStartup.Size = new Size(120, 18);
      this.lblStartup.TabIndex = 11;
      this.lblStartup.Text = "Startup:";
      this.lblStartup.TextAlign = ContentAlignment.MiddleRight;
      this.lblDescription.Location = new Point(6, 35);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(120, 16);
      this.lblDescription.TabIndex = 7;
      this.lblDescription.Text = "Description:";
      this.lblDescription.TextAlign = ContentAlignment.MiddleRight;
      this.txtDescription.Location = new Point((int) sbyte.MaxValue, 35);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(232, 32);
      this.txtDescription.TabIndex = 1;
      this.txtDescription.Text = "-";
      this.lblName.Location = new Point(6, 13);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(120, 18);
      this.lblName.TabIndex = 1;
      this.lblName.Text = "Name:";
      this.lblName.TextAlign = ContentAlignment.MiddleRight;
      this.txtName.Location = new Point((int) sbyte.MaxValue, 13);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(232, 20);
      this.txtName.TabIndex = 0;
      this.txtName.Text = "-";
      this.grpControl.Controls.Add((Control) this.tbxInternalState);
      this.grpControl.Controls.Add((Control) this.btnGetVersionInfo);
      this.grpControl.Controls.Add((Control) this.cbxRun);
      this.grpControl.Controls.Add((Control) this.cbxError);
      this.grpControl.Controls.Add((Control) this.cbxWatchdogOccurred);
      this.grpControl.Controls.Add((Control) this.btnClearWatchdog);
      this.grpControl.Controls.Add((Control) this.btnAutoStart);
      this.grpControl.Controls.Add((Control) this.btnDisable);
      this.grpControl.Controls.Add((Control) this.cbxConnect);
      this.grpControl.Controls.Add((Control) this.btnEnable);
      this.grpControl.Location = new Point(174, 29);
      this.grpControl.Name = "grpControl";
      this.grpControl.Size = new Size(142, 256);
      this.grpControl.TabIndex = 22;
      this.grpControl.TabStop = false;
      this.grpControl.Text = "Controller Handling";
      this.tbxInternalState.Location = new Point(11, 83);
      this.tbxInternalState.Name = "tbxInternalState";
      this.tbxInternalState.ReadOnly = true;
      this.tbxInternalState.Size = new Size(120, 20);
      this.tbxInternalState.TabIndex = 31;
      this.tbxInternalState.Text = "-";
      this.btnGetVersionInfo.Location = new Point(11, 221);
      this.btnGetVersionInfo.Name = "btnGetVersionInfo";
      this.btnGetVersionInfo.Size = new Size(120, 25);
      this.btnGetVersionInfo.TabIndex = 4;
      this.btnGetVersionInfo.Text = "Get Version Info";
      this.btnGetVersionInfo.Click += new EventHandler(this.btnGetVersionInfo_Click);
      this.cbxRun.AutoCheck = false;
      this.cbxRun.Location = new Point(11, 31);
      this.cbxRun.Name = "cbxRun";
      this.cbxRun.Size = new Size(125, 16);
      this.cbxRun.TabIndex = 29;
      this.cbxRun.Text = "Run";
      this.cbxError.AutoCheck = false;
      this.cbxError.Location = new Point(11, 47);
      this.cbxError.Name = "cbxError";
      this.cbxError.Size = new Size(128, 16);
      this.cbxError.TabIndex = 28;
      this.cbxError.Text = "Error";
      this.cbxWatchdogOccurred.AutoCheck = false;
      this.cbxWatchdogOccurred.Location = new Point(11, 63);
      this.cbxWatchdogOccurred.Name = "cbxWatchdogOccurred";
      this.cbxWatchdogOccurred.Size = new Size(125, 17);
      this.cbxWatchdogOccurred.TabIndex = 27;
      this.cbxWatchdogOccurred.Text = "Watchdog Occurred";
      this.btnClearWatchdog.Location = new Point(11, 193);
      this.btnClearWatchdog.Name = "btnClearWatchdog";
      this.btnClearWatchdog.Size = new Size(120, 25);
      this.btnClearWatchdog.TabIndex = 3;
      this.btnClearWatchdog.Text = "Watchdog Clear";
      this.btnClearWatchdog.Click += new EventHandler(this.btnClearWatchdog_Click);
      this.btnAutoStart.Location = new Point(11, 165);
      this.btnAutoStart.Name = "btnAutoStart";
      this.btnAutoStart.Size = new Size(120, 25);
      this.btnAutoStart.TabIndex = 2;
      this.btnAutoStart.Text = "Auto Start";
      this.btnAutoStart.Click += new EventHandler(this.btnAutoStart_Click);
      this.btnDisable.Location = new Point(11, 137);
      this.btnDisable.Name = "btnDisable";
      this.btnDisable.Size = new Size(120, 25);
      this.btnDisable.TabIndex = 1;
      this.btnDisable.Text = "Disable";
      this.btnDisable.Click += new EventHandler(this.btnDisable_Click);
      this.cbxConnect.AutoCheck = false;
      this.cbxConnect.Location = new Point(11, 15);
      this.cbxConnect.Name = "cbxConnect";
      this.cbxConnect.Size = new Size(120, 16);
      this.cbxConnect.TabIndex = 23;
      this.cbxConnect.Text = "Connect";
      this.btnEnable.Location = new Point(11, 109);
      this.btnEnable.Name = "btnEnable";
      this.btnEnable.Size = new Size(120, 25);
      this.btnEnable.TabIndex = 0;
      this.btnEnable.Text = "Enable";
      this.btnEnable.Click += new EventHandler(this.btnEnable_Click);
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Controls.Add((Control) this.grpController);
      this.MaximumSize = new Size(704, 292);
      this.MinimumSize = new Size(326, 292);
      this.Name = nameof (ctrlController);
      this.Size = new Size(704, 292);
      this.Load += new EventHandler(this.ctrlIBS_G4_Load);
      this.grpController.ResumeLayout(false);
      this.gbxControllerList.ResumeLayout(false);
      this.grpProperties.ResumeLayout(false);
      this.grpProperties.PerformLayout();
      this.grpControl.ResumeLayout(false);
      this.grpControl.PerformLayout();
      this.ResumeLayout(false);
    }

    public ctrlController()
    {
      this.InitializeComponent();
      this._varController = new List<IController>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void ctrlIBS_G4_Load(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.ControlText))
        return;
      this.ControlText = this.Name;
    }

    private void listController_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listController.SelectedIndex <= -1)
        return;
      this._myController = this._varController[this.listController.SelectedIndex];
      this.ShowProperties();
      if (this._hdSelectController == null)
        return;
      this._hdSelectController((object) this, (object) this._myController);
    }

    private void btnEnable_Click(object sender, EventArgs e)
    {
      if (this._myController == null || this._myController.Connect)
        return;
      this._myController.Enable();
    }

    private void btnDisable_Click(object sender, EventArgs e)
    {
      if (this._myController == null || !this._myController.Connect && !this._myController.Error)
        return;
      if (this.tDisable == null)
      {
        this.tDisable = new Thread(new ParameterizedThreadStart(this.RunDisable));
        this.tDisable.Start((object) this._myController);
      }
      else
      {
        if (this.tDisable.ThreadState == ThreadState.Running || this.tDisable.ThreadState == ThreadState.WaitSleepJoin)
          return;
        this.tDisable = new Thread(new ParameterizedThreadStart(this.RunDisable));
        this.tDisable.Start((object) this._myController);
      }
    }

    private void btnAutoStart_Click(object sender, EventArgs e)
    {
      if (this._myController == null || !this._myController.Connect)
        return;
      this._myController.AutoStart();
    }

    private void btnClearWatchdog_Click(object sender, EventArgs e)
    {
      if (this._myController == null || !this._myController.Connect)
        return;
      this._myController.WatchdogClear();
    }

    private void btnPropertiesShow_Click(object sender, EventArgs e)
    {
      if (this.grpProperties.Visible)
        this.SizeMin();
      else
        this.SizeMax();
    }

    private void btnGetVersionInfo_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show(this._myController.VersionInfo.ToString(), Application.ProductName);
    }

    private void btnWriteChanges_Click(object sender, EventArgs e)
    {
      this._myController.Name = this.txtName.Text;
      this._myController.Description = this.txtDescription.Text;
      this._myController.Startup = (ControllerStartup) this.cmbControllerStartup.SelectedItem;
      this._myController.WatchdogTimeout = (WatchdogMonitoringTime) this.cmbWatchdogTimeout.SelectedItem;
      this._myController.WatchdogDeactivate = this.cbxWatchdogDeaktivate.Checked;
      this._myController.Connection = this.txtConnectionstring.Text;
      int result;
      if (int.TryParse(this.txtProcessDataCycleTime.Text, out result))
        this._myController.UpdateProcessDataCycleTime = result;
      if (int.TryParse(this.txtMailboxCycleTime.Text, out result))
        this._myController.UpdateMailboxTime = result;
      this.ShowProperties();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.ShowProperties();
    }

    public string ControlText
    {
      get
      {
        return this.grpController.Text;
      }
      set
      {
        this.grpController.Text = value;
      }
    }

    public event SelectControllerHandler OnSelectController
    {
      add
      {
        this._hdSelectController += value;
      }
      remove
      {
        this._hdSelectController -= value;
      }
    }

    public event SelectControllerHandler BeforeDeactivateController
    {
      add
      {
        this._hdBeforeDeactivateController += value;
      }
      remove
      {
        this._hdBeforeDeactivateController -= value;
      }
    }

    public int AddObject(IController ControllerObject)
    {
      this._varController.Add(ControllerObject);
      int num = this.listController.Items.Add((object) ControllerObject.Name);
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varController.Count.ToString());
      if (num != 0)
        return num;
      this._myController = this._varController[0];
      this.listController.SelectedIndex = 0;
      this.ShowData();
      return 1;
    }

    public void ClearObject()
    {
      this._varController.Clear();
      this.listController.Items.Clear();
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varController.Count.ToString());
      this._myController = (IController) null;
      this.ShowData();
    }

    public System.Windows.Forms.Timer UpdateData
    {
      set
      {
        this._locTimer = value;
        this._locTimer.Tick += new EventHandler(this._locTimer_Tick);
      }
    }

    public void SizeMin()
    {
      this.grpProperties.Visible = false;
      this.grpController.Size = new Size(326, 292);
      this.btnPropertiesShow.Image = (Image) Resources.redo;
      this.Size = new Size(326, 292);
    }

    public void SizeMax()
    {
      this.grpProperties.Visible = true;
      this.grpController.Size = new Size(704, 292);
      this.Size = new Size(704, 292);
      this.BringToFront();
      this.btnPropertiesShow.Image = (Image) Resources.undo;
    }

    private void ShowProperties()
    {
      if (this._myController != null)
      {
        try
        {
          this.txtName.Text = this._myController.Name;
          this.txtDescription.Text = this._myController.Description;
          this.cbxWatchdogDeaktivate.Checked = this._myController.WatchdogDeactivate;
          this.cmbControllerStartup.Items.Clear();
          foreach (int num in (ControllerStartup[]) Enum.GetValues(typeof (ControllerStartup)))
            this.cmbControllerStartup.Items.Add((object) (ControllerStartup) num);
          this.cmbControllerStartup.Text = this._myController.Startup.ToString();
          this.cmbWatchdogTimeout.Items.Clear();
          foreach (int num in (WatchdogMonitoringTime[]) Enum.GetValues(typeof (WatchdogMonitoringTime)))
            this.cmbWatchdogTimeout.Items.Add((object) (WatchdogMonitoringTime) num);
          this.cmbWatchdogTimeout.Text = this._myController.WatchdogTimeout.ToString();
          this.txtConnectionstring.Text = this._myController.Connection;
          this.txtProcessDataCycleTime.Text = this._myController.UpdateProcessDataCycleTime.ToString();
          TextBox mailboxCycleTime = this.txtMailboxCycleTime;
          int num1 = this._myController.UpdateMailboxTime;
          string str1 = num1.ToString();
          mailboxCycleTime.Text = str1;
          TextBox inputObjectCount = this.txtInputObjectCount;
          num1 = this._myController.InputObjectList.Count;
          string str2 = num1.ToString();
          inputObjectCount.Text = str2;
          TextBox objectStartAddress1 = this.txtInputObjectStartAddress;
          num1 = this._myController.InputObjectStartAddress;
          string str3 = num1.ToString();
          objectStartAddress1.Text = str3;
          TextBox objectEndAddress1 = this.txtInputObjectEndAddress;
          num1 = this._myController.InputObjectEndAddress;
          string str4 = num1.ToString();
          objectEndAddress1.Text = str4;
          TextBox outputObjectCount = this.txtOutputObjectCount;
          num1 = this._myController.OutputObjectList.Count;
          string str5 = num1.ToString();
          outputObjectCount.Text = str5;
          TextBox objectStartAddress2 = this.txtOutputObjectStartAddress;
          num1 = this._myController.OutputObjectStartAddress;
          string str6 = num1.ToString();
          objectStartAddress2.Text = str6;
          TextBox objectEndAddress2 = this.txtOutputObjectEndAddress;
          num1 = this._myController.OutputObjectEndAddress;
          string str7 = num1.ToString();
          objectEndAddress2.Text = str7;
        }
        catch
        {
        }
      }
      else
      {
        this.txtName.Text = "-";
        this.txtDescription.Text = "-";
        this.cbxWatchdogDeaktivate.Checked = false;
        this.cmbControllerStartup.Text = "-";
        this.cmbWatchdogTimeout.Text = "-";
        this.txtProcessDataCycleTime.Text = "-";
        this.txtMailboxCycleTime.Text = "-";
        this.txtInputObjectCount.Text = "-";
        this.txtInputObjectStartAddress.Text = "-";
        this.txtInputObjectEndAddress.Text = "-";
        this.txtOutputObjectCount.Text = "-";
        this.txtOutputObjectStartAddress.Text = "-";
        this.txtOutputObjectEndAddress.Text = "-";
      }
    }

    private void ShowData()
    {
      this.ShowProperties();
      if (this._myController != null)
      {
        try
        {
          this.cbxConnect.Checked = this._myController.Connect;
          this.cbxRun.Checked = this._myController.Run;
          this.cbxError.Checked = this._myController.Error;
          this.cbxWatchdogOccurred.Checked = this._myController.WatchdogOccurred;
          this.tbxInternalState.Text = this._myController.InternalState;
        }
        catch
        {
        }
      }
      else
      {
        this.cbxConnect.Checked = false;
        this.cbxRun.Checked = false;
        this.cbxError.Checked = false;
        this.cbxWatchdogOccurred.Checked = false;
        this.tbxInternalState.Text = "-";
      }
    }

    private void _locTimer_Tick(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      try
      {
        this.cbxConnect.Checked = this._myController.Connect;
        this.cbxRun.Checked = this._myController.Run;
        this.cbxError.Checked = this._myController.Error;
        this.cbxWatchdogOccurred.Checked = this._myController.WatchdogOccurred;
        this.tbxInternalState.Text = this._myController.InternalState;
        if (this._myController.Connect && !this._fpControllerReady)
        {
          this._fpControllerReady = true;
          this.txtName.ReadOnly = true;
          this.txtDescription.ReadOnly = true;
          this.cmbControllerStartup.Enabled = false;
          this.cmbWatchdogTimeout.Enabled = false;
          this.txtConnectionstring.ReadOnly = true;
          this.cbxWatchdogDeaktivate.Enabled = false;
          this.txtProcessDataCycleTime.ReadOnly = true;
          this.txtMailboxCycleTime.ReadOnly = true;
          this.btnCancel.Enabled = false;
          this.btnWriteChanges.Enabled = false;
        }
        if (this._myController.Connect || !this._fpControllerReady)
          return;
        this._fpControllerReady = false;
        this.txtName.ReadOnly = false;
        this.txtDescription.ReadOnly = false;
        this.cmbControllerStartup.Enabled = true;
        this.cmbWatchdogTimeout.Enabled = true;
        this.txtConnectionstring.ReadOnly = false;
        this.cbxWatchdogDeaktivate.Enabled = true;
        this.txtProcessDataCycleTime.ReadOnly = false;
        this.txtMailboxCycleTime.ReadOnly = false;
        this.btnCancel.Enabled = true;
        this.btnWriteChanges.Enabled = true;
      }
      catch
      {
      }
    }

    private void RunDisable(object Controller)
    {
      if (this._hdBeforeDeactivateController != null)
        this._hdBeforeDeactivateController((object) this, Controller);
      if (((IController) Controller).Run)
        Thread.Sleep(((IController) Controller).UpdateMailboxTime * 2);
      this._myController.Disable();
    }
  }
}
