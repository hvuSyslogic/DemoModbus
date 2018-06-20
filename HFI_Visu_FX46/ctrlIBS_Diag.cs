// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.ctrlIBS_Diag
// Assembly: HFI_Visu_FX46, Version=3.2.6053.23250, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: A9FB10B7-9AE3-4F4C-88CF-1D5F3BF257DC
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Visu_FX46.dll

using PhoenixContact.HFI.Inline;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PhoenixContact.HFI.Visualization
{
  [ToolboxBitmap(typeof (ctrlIBS_Diag), "picDiag.bmp")]
  public class ctrlIBS_Diag : UserControl
  {
    private IInterbusG4 _myController;
    private Timer _locTimer;
    private GroupBox grpBusControl;
    private Button btnAlarmStop;
    private Button btnStartDataTransfer;
    private Button btnActivateConfig;
    private TextBox txtHandlingState;
    private GroupBox grpInterbus;
    private Button btnConfirmPF;
    private Button btnConfirmDiagnostic;
    private Button btnCreateConfiguration;
    private Button btnControllerRevision;
    private GroupBox grpBusDiagnostic;
    private TabControl tabBusState;
    private TabPage tabPageAllRegister;
    private TextBox txtBusDiagnosticStatusRegister;
    private Label label4;
    private Label label6;
    private Label label2;
    private Label label1;
    private TextBox txtProcessDataCycleTimeActual;
    private TextBox txtBusDiagnosticParameterRegister2;
    private Label lblDiagnosticParameterRegister2;
    private Label label5;
    private TextBox txtBusDiagnosticParameterRegister;
    private Label lblDiagnosticParameterRegister;
    private Label label3;
    private TabPage tabPageStateRegister;
    private Panel pnlRegister_2;
    private CheckBox chkBusSDSI;
    private CheckBox chkBusQUALITY;
    private CheckBox chkBusWARNING;
    private CheckBox chkBusDC_RESULT;
    private CheckBox chkBusSY_RESULT;
    private CheckBox chkBusRESULT;
    private CheckBox chkBusBASP;
    private CheckBox chkBusBSA;
    private Panel pnlRegister_1;
    private CheckBox chkBusREADY;
    private CheckBox chkBusACTIVE;
    private CheckBox chkBusRUN;
    private CheckBox chkBusDETECT;
    private CheckBox chkBusCTRL;
    private CheckBox chkBusBUS;
    private CheckBox chkBusPF;
    private CheckBox chkBusUSER;
    private Container components;

    public ctrlIBS_Diag()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpBusControl = new GroupBox();
      this.btnConfirmDiagnostic = new Button();
      this.btnConfirmPF = new Button();
      this.btnAlarmStop = new Button();
      this.btnStartDataTransfer = new Button();
      this.btnActivateConfig = new Button();
      this.txtHandlingState = new TextBox();
      this.btnCreateConfiguration = new Button();
      this.btnControllerRevision = new Button();
      this.grpInterbus = new GroupBox();
      this.grpBusDiagnostic = new GroupBox();
      this.tabBusState = new TabControl();
      this.tabPageStateRegister = new TabPage();
      this.pnlRegister_1 = new Panel();
      this.chkBusREADY = new CheckBox();
      this.chkBusACTIVE = new CheckBox();
      this.chkBusRUN = new CheckBox();
      this.chkBusDETECT = new CheckBox();
      this.chkBusCTRL = new CheckBox();
      this.chkBusBUS = new CheckBox();
      this.chkBusPF = new CheckBox();
      this.chkBusUSER = new CheckBox();
      this.pnlRegister_2 = new Panel();
      this.chkBusSDSI = new CheckBox();
      this.chkBusQUALITY = new CheckBox();
      this.chkBusWARNING = new CheckBox();
      this.chkBusDC_RESULT = new CheckBox();
      this.chkBusSY_RESULT = new CheckBox();
      this.chkBusRESULT = new CheckBox();
      this.chkBusBASP = new CheckBox();
      this.chkBusBSA = new CheckBox();
      this.tabPageAllRegister = new TabPage();
      this.txtBusDiagnosticStatusRegister = new TextBox();
      this.label4 = new Label();
      this.label6 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.txtProcessDataCycleTimeActual = new TextBox();
      this.txtBusDiagnosticParameterRegister2 = new TextBox();
      this.lblDiagnosticParameterRegister2 = new Label();
      this.label5 = new Label();
      this.txtBusDiagnosticParameterRegister = new TextBox();
      this.lblDiagnosticParameterRegister = new Label();
      this.label3 = new Label();
      this.grpBusControl.SuspendLayout();
      this.grpInterbus.SuspendLayout();
      this.grpBusDiagnostic.SuspendLayout();
      this.tabBusState.SuspendLayout();
      this.tabPageStateRegister.SuspendLayout();
      this.pnlRegister_1.SuspendLayout();
      this.pnlRegister_2.SuspendLayout();
      this.tabPageAllRegister.SuspendLayout();
      this.SuspendLayout();
      this.grpBusControl.Controls.Add((Control) this.btnConfirmDiagnostic);
      this.grpBusControl.Controls.Add((Control) this.btnConfirmPF);
      this.grpBusControl.Controls.Add((Control) this.btnAlarmStop);
      this.grpBusControl.Controls.Add((Control) this.btnStartDataTransfer);
      this.grpBusControl.Controls.Add((Control) this.btnActivateConfig);
      this.grpBusControl.Controls.Add((Control) this.txtHandlingState);
      this.grpBusControl.Controls.Add((Control) this.btnCreateConfiguration);
      this.grpBusControl.Location = new Point(6, 57);
      this.grpBusControl.Name = "grpBusControl";
      this.grpBusControl.Size = new Size(141, 215);
      this.grpBusControl.TabIndex = 23;
      this.grpBusControl.TabStop = false;
      this.grpBusControl.Text = "INTERBUS Services";
      this.btnConfirmDiagnostic.Location = new Point(11, 178);
      this.btnConfirmDiagnostic.Name = "btnConfirmDiagnostic";
      this.btnConfirmDiagnostic.Size = new Size(120, 23);
      this.btnConfirmDiagnostic.TabIndex = 5;
      this.btnConfirmDiagnostic.Text = "Confirm Diagnostic";
      this.btnConfirmDiagnostic.Click += new EventHandler(this.btnConfirmDiagnostic_Click);
      this.btnConfirmPF.Location = new Point(11, 154);
      this.btnConfirmPF.Name = "btnConfirmPF";
      this.btnConfirmPF.Size = new Size(120, 23);
      this.btnConfirmPF.TabIndex = 4;
      this.btnConfirmPF.Text = "Confirm PF Faults";
      this.btnConfirmPF.Click += new EventHandler(this.btnConfirmPF_Click);
      this.btnAlarmStop.Location = new Point(11, 125);
      this.btnAlarmStop.Name = "btnAlarmStop";
      this.btnAlarmStop.Size = new Size(120, 23);
      this.btnAlarmStop.TabIndex = 3;
      this.btnAlarmStop.Text = "Alarm Stop";
      this.btnAlarmStop.Click += new EventHandler(this.btnAlarmStop_Click);
      this.btnStartDataTransfer.Location = new Point(11, 101);
      this.btnStartDataTransfer.Name = "btnStartDataTransfer";
      this.btnStartDataTransfer.Size = new Size(120, 23);
      this.btnStartDataTransfer.TabIndex = 2;
      this.btnStartDataTransfer.Text = "Start Data Transfer";
      this.btnStartDataTransfer.Click += new EventHandler(this.btnStartDataTransfer_Click);
      this.btnActivateConfig.Location = new Point(11, 72);
      this.btnActivateConfig.Name = "btnActivateConfig";
      this.btnActivateConfig.Size = new Size(120, 23);
      this.btnActivateConfig.TabIndex = 1;
      this.btnActivateConfig.Text = "Activate Config.";
      this.btnActivateConfig.Click += new EventHandler(this.btnActivateConfig_Click);
      this.txtHandlingState.Location = new Point(11, 20);
      this.txtHandlingState.Name = "txtHandlingState";
      this.txtHandlingState.ReadOnly = true;
      this.txtHandlingState.Size = new Size(120, 20);
      this.txtHandlingState.TabIndex = 24;
      this.txtHandlingState.Text = "-";
      this.btnCreateConfiguration.Location = new Point(11, 48);
      this.btnCreateConfiguration.Name = "btnCreateConfiguration";
      this.btnCreateConfiguration.Size = new Size(120, 23);
      this.btnCreateConfiguration.TabIndex = 0;
      this.btnCreateConfiguration.Text = "Create Config.";
      this.btnCreateConfiguration.Click += new EventHandler(this.btnCreateConfiguration_Click);
      this.btnControllerRevision.Location = new Point(17, 18);
      this.btnControllerRevision.Name = "btnControllerRevision";
      this.btnControllerRevision.Size = new Size(336, 23);
      this.btnControllerRevision.TabIndex = 0;
      this.btnControllerRevision.Text = "Show Controller Revision Info";
      this.btnControllerRevision.UseVisualStyleBackColor = true;
      this.btnControllerRevision.Click += new EventHandler(this.btnControllerRevision_Click);
      this.grpInterbus.Controls.Add((Control) this.grpBusDiagnostic);
      this.grpInterbus.Controls.Add((Control) this.grpBusControl);
      this.grpInterbus.Controls.Add((Control) this.btnControllerRevision);
      this.grpInterbus.Location = new Point(0, 0);
      this.grpInterbus.Name = "grpInterbus";
      this.grpInterbus.Size = new Size(372, 280);
      this.grpInterbus.TabIndex = 0;
      this.grpInterbus.TabStop = false;
      this.grpBusDiagnostic.Controls.Add((Control) this.tabBusState);
      this.grpBusDiagnostic.Location = new Point(153, 57);
      this.grpBusDiagnostic.Name = "grpBusDiagnostic";
      this.grpBusDiagnostic.Size = new Size(211, 215);
      this.grpBusDiagnostic.TabIndex = 60;
      this.grpBusDiagnostic.TabStop = false;
      this.grpBusDiagnostic.Text = "INTERBUS Diagnostic";
      this.tabBusState.Controls.Add((Control) this.tabPageStateRegister);
      this.tabBusState.Controls.Add((Control) this.tabPageAllRegister);
      this.tabBusState.Location = new Point(7, 19);
      this.tabBusState.Name = "tabBusState";
      this.tabBusState.SelectedIndex = 0;
      this.tabBusState.Size = new Size(194, 190);
      this.tabBusState.TabIndex = 0;
      this.tabPageStateRegister.Controls.Add((Control) this.pnlRegister_1);
      this.tabPageStateRegister.Controls.Add((Control) this.pnlRegister_2);
      this.tabPageStateRegister.Location = new Point(4, 22);
      this.tabPageStateRegister.Name = "tabPageStateRegister";
      this.tabPageStateRegister.Padding = new Padding(3);
      this.tabPageStateRegister.Size = new Size(186, 164);
      this.tabPageStateRegister.TabIndex = 1;
      this.tabPageStateRegister.Text = "State Register";
      this.tabPageStateRegister.UseVisualStyleBackColor = true;
      this.pnlRegister_1.BorderStyle = BorderStyle.Fixed3D;
      this.pnlRegister_1.Controls.Add((Control) this.chkBusREADY);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusACTIVE);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusRUN);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusDETECT);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusCTRL);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusBUS);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusPF);
      this.pnlRegister_1.Controls.Add((Control) this.chkBusUSER);
      this.pnlRegister_1.Location = new Point(6, 8);
      this.pnlRegister_1.Name = "pnlRegister_1";
      this.pnlRegister_1.Size = new Size(83, 148);
      this.pnlRegister_1.TabIndex = 54;
      this.chkBusREADY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusREADY.AutoCheck = false;
      this.chkBusREADY.BackColor = Color.LightGreen;
      this.chkBusREADY.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusREADY.Location = new Point(-6, 126);
      this.chkBusREADY.Name = "chkBusREADY";
      this.chkBusREADY.Size = new Size(83, 18);
      this.chkBusREADY.TabIndex = 60;
      this.chkBusREADY.Text = "READY";
      this.chkBusREADY.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusREADY.UseVisualStyleBackColor = false;
      this.chkBusACTIVE.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusACTIVE.AutoCheck = false;
      this.chkBusACTIVE.BackColor = Color.LightGreen;
      this.chkBusACTIVE.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusACTIVE.Location = new Point(-6, 108);
      this.chkBusACTIVE.Name = "chkBusACTIVE";
      this.chkBusACTIVE.Size = new Size(83, 18);
      this.chkBusACTIVE.TabIndex = 59;
      this.chkBusACTIVE.Text = "ACTIVE";
      this.chkBusACTIVE.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusACTIVE.UseVisualStyleBackColor = false;
      this.chkBusRUN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusRUN.AutoCheck = false;
      this.chkBusRUN.BackColor = Color.LightGreen;
      this.chkBusRUN.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusRUN.Location = new Point(-6, 91);
      this.chkBusRUN.Name = "chkBusRUN";
      this.chkBusRUN.Size = new Size(83, 18);
      this.chkBusRUN.TabIndex = 58;
      this.chkBusRUN.Text = "RUN";
      this.chkBusRUN.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusRUN.UseVisualStyleBackColor = false;
      this.chkBusDETECT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusDETECT.AutoCheck = false;
      this.chkBusDETECT.BackColor = Color.Tomato;
      this.chkBusDETECT.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusDETECT.Location = new Point(-6, 72);
      this.chkBusDETECT.Name = "chkBusDETECT";
      this.chkBusDETECT.Size = new Size(83, 18);
      this.chkBusDETECT.TabIndex = 57;
      this.chkBusDETECT.Text = "DETECT";
      this.chkBusDETECT.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusDETECT.UseVisualStyleBackColor = false;
      this.chkBusCTRL.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusCTRL.AutoCheck = false;
      this.chkBusCTRL.BackColor = Color.Tomato;
      this.chkBusCTRL.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusCTRL.Location = new Point(-6, 54);
      this.chkBusCTRL.Name = "chkBusCTRL";
      this.chkBusCTRL.Size = new Size(83, 18);
      this.chkBusCTRL.TabIndex = 56;
      this.chkBusCTRL.Text = "CTRL";
      this.chkBusCTRL.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusCTRL.UseVisualStyleBackColor = false;
      this.chkBusBUS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusBUS.AutoCheck = false;
      this.chkBusBUS.BackColor = Color.Tomato;
      this.chkBusBUS.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusBUS.Location = new Point(-6, 36);
      this.chkBusBUS.Name = "chkBusBUS";
      this.chkBusBUS.Size = new Size(83, 18);
      this.chkBusBUS.TabIndex = 55;
      this.chkBusBUS.Text = "BUS";
      this.chkBusBUS.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusBUS.UseVisualStyleBackColor = false;
      this.chkBusPF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusPF.AutoCheck = false;
      this.chkBusPF.BackColor = Color.Tomato;
      this.chkBusPF.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusPF.Location = new Point(-6, 18);
      this.chkBusPF.Name = "chkBusPF";
      this.chkBusPF.Size = new Size(83, 18);
      this.chkBusPF.TabIndex = 54;
      this.chkBusPF.Text = "PF";
      this.chkBusPF.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusPF.UseVisualStyleBackColor = false;
      this.chkBusUSER.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusUSER.AutoCheck = false;
      this.chkBusUSER.BackColor = SystemColors.Control;
      this.chkBusUSER.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusUSER.Location = new Point(-6, 0);
      this.chkBusUSER.Name = "chkBusUSER";
      this.chkBusUSER.Size = new Size(83, 18);
      this.chkBusUSER.TabIndex = 53;
      this.chkBusUSER.Text = "USER";
      this.chkBusUSER.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusUSER.UseVisualStyleBackColor = false;
      this.pnlRegister_2.BorderStyle = BorderStyle.Fixed3D;
      this.pnlRegister_2.Controls.Add((Control) this.chkBusSDSI);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusQUALITY);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusWARNING);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusDC_RESULT);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusSY_RESULT);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusRESULT);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusBASP);
      this.pnlRegister_2.Controls.Add((Control) this.chkBusBSA);
      this.pnlRegister_2.Location = new Point(89, 8);
      this.pnlRegister_2.Name = "pnlRegister_2";
      this.pnlRegister_2.Size = new Size(91, 148);
      this.pnlRegister_2.TabIndex = 55;
      this.chkBusSDSI.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusSDSI.AutoCheck = false;
      this.chkBusSDSI.BackColor = SystemColors.Control;
      this.chkBusSDSI.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusSDSI.Location = new Point(-1, 126);
      this.chkBusSDSI.Name = "chkBusSDSI";
      this.chkBusSDSI.Size = new Size(88, 18);
      this.chkBusSDSI.TabIndex = 23;
      this.chkBusSDSI.Text = "SDSI";
      this.chkBusSDSI.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusSDSI.UseVisualStyleBackColor = false;
      this.chkBusQUALITY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusQUALITY.AutoCheck = false;
      this.chkBusQUALITY.BackColor = SystemColors.Control;
      this.chkBusQUALITY.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusQUALITY.Location = new Point(-1, 108);
      this.chkBusQUALITY.Name = "chkBusQUALITY";
      this.chkBusQUALITY.Size = new Size(88, 18);
      this.chkBusQUALITY.TabIndex = 22;
      this.chkBusQUALITY.Text = "QUALITY";
      this.chkBusQUALITY.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusQUALITY.UseVisualStyleBackColor = false;
      this.chkBusWARNING.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusWARNING.AutoCheck = false;
      this.chkBusWARNING.BackColor = SystemColors.Control;
      this.chkBusWARNING.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusWARNING.Location = new Point(-1, 90);
      this.chkBusWARNING.Name = "chkBusWARNING";
      this.chkBusWARNING.Size = new Size(88, 18);
      this.chkBusWARNING.TabIndex = 21;
      this.chkBusWARNING.Text = "WARNING";
      this.chkBusWARNING.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusWARNING.UseVisualStyleBackColor = false;
      this.chkBusDC_RESULT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusDC_RESULT.AutoCheck = false;
      this.chkBusDC_RESULT.BackColor = SystemColors.Control;
      this.chkBusDC_RESULT.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusDC_RESULT.Location = new Point(-1, 72);
      this.chkBusDC_RESULT.Name = "chkBusDC_RESULT";
      this.chkBusDC_RESULT.Size = new Size(88, 18);
      this.chkBusDC_RESULT.TabIndex = 20;
      this.chkBusDC_RESULT.Text = "DC-RESULT";
      this.chkBusDC_RESULT.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusDC_RESULT.UseVisualStyleBackColor = false;
      this.chkBusSY_RESULT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusSY_RESULT.AutoCheck = false;
      this.chkBusSY_RESULT.BackColor = SystemColors.Control;
      this.chkBusSY_RESULT.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusSY_RESULT.Location = new Point(-1, 54);
      this.chkBusSY_RESULT.Name = "chkBusSY_RESULT";
      this.chkBusSY_RESULT.Size = new Size(88, 18);
      this.chkBusSY_RESULT.TabIndex = 19;
      this.chkBusSY_RESULT.Text = "SY-RESULT";
      this.chkBusSY_RESULT.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusSY_RESULT.UseVisualStyleBackColor = false;
      this.chkBusRESULT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusRESULT.AutoCheck = false;
      this.chkBusRESULT.BackColor = SystemColors.Control;
      this.chkBusRESULT.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusRESULT.Location = new Point(-1, 36);
      this.chkBusRESULT.Name = "chkBusRESULT";
      this.chkBusRESULT.Size = new Size(88, 18);
      this.chkBusRESULT.TabIndex = 18;
      this.chkBusRESULT.Text = "RESULT";
      this.chkBusRESULT.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusRESULT.UseVisualStyleBackColor = false;
      this.chkBusBASP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusBASP.AutoCheck = false;
      this.chkBusBASP.BackColor = Color.Tomato;
      this.chkBusBASP.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusBASP.Location = new Point(-1, 18);
      this.chkBusBASP.Name = "chkBusBASP";
      this.chkBusBASP.Size = new Size(88, 18);
      this.chkBusBASP.TabIndex = 17;
      this.chkBusBASP.Text = "BASP";
      this.chkBusBASP.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusBASP.UseVisualStyleBackColor = false;
      this.chkBusBSA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBusBSA.AutoCheck = false;
      this.chkBusBSA.BackColor = SystemColors.Control;
      this.chkBusBSA.CheckAlign = ContentAlignment.MiddleRight;
      this.chkBusBSA.Location = new Point(-1, 0);
      this.chkBusBSA.Name = "chkBusBSA";
      this.chkBusBSA.Size = new Size(88, 18);
      this.chkBusBSA.TabIndex = 16;
      this.chkBusBSA.Text = "BSA";
      this.chkBusBSA.TextAlign = ContentAlignment.MiddleRight;
      this.chkBusBSA.UseVisualStyleBackColor = false;
      this.tabPageAllRegister.Controls.Add((Control) this.txtBusDiagnosticStatusRegister);
      this.tabPageAllRegister.Controls.Add((Control) this.label4);
      this.tabPageAllRegister.Controls.Add((Control) this.label6);
      this.tabPageAllRegister.Controls.Add((Control) this.label2);
      this.tabPageAllRegister.Controls.Add((Control) this.label1);
      this.tabPageAllRegister.Controls.Add((Control) this.txtProcessDataCycleTimeActual);
      this.tabPageAllRegister.Controls.Add((Control) this.txtBusDiagnosticParameterRegister2);
      this.tabPageAllRegister.Controls.Add((Control) this.lblDiagnosticParameterRegister2);
      this.tabPageAllRegister.Controls.Add((Control) this.label5);
      this.tabPageAllRegister.Controls.Add((Control) this.txtBusDiagnosticParameterRegister);
      this.tabPageAllRegister.Controls.Add((Control) this.lblDiagnosticParameterRegister);
      this.tabPageAllRegister.Controls.Add((Control) this.label3);
      this.tabPageAllRegister.Location = new Point(4, 22);
      this.tabPageAllRegister.Name = "tabPageAllRegister";
      this.tabPageAllRegister.Padding = new Padding(3);
      this.tabPageAllRegister.Size = new Size(186, 164);
      this.tabPageAllRegister.TabIndex = 0;
      this.tabPageAllRegister.Text = "All Register";
      this.tabPageAllRegister.UseVisualStyleBackColor = true;
      this.txtBusDiagnosticStatusRegister.Location = new Point(12, 21);
      this.txtBusDiagnosticStatusRegister.Name = "txtBusDiagnosticStatusRegister";
      this.txtBusDiagnosticStatusRegister.ReadOnly = true;
      this.txtBusDiagnosticStatusRegister.Size = new Size(88, 20);
      this.txtBusDiagnosticStatusRegister.TabIndex = 70;
      this.txtBusDiagnosticStatusRegister.Text = "-";
      this.label4.Location = new Point(12, 5);
      this.label4.Name = "label4";
      this.label4.Size = new Size(160, 16);
      this.label4.TabIndex = 69;
      this.label4.Text = "Diagnostic Status Register";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label6.Location = new Point(108, 21);
      this.label6.Name = "label6";
      this.label6.Size = new Size(72, 16);
      this.label6.TabIndex = 68;
      this.label6.Text = "(hex)";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(12, 121);
      this.label2.Name = "label2";
      this.label2.Size = new Size(168, 16);
      this.label2.TabIndex = 67;
      this.label2.Text = "Current INTERBUS Cycle Time";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(68, 137);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 16);
      this.label1.TabIndex = 66;
      this.label1.Text = "[ms]";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.txtProcessDataCycleTimeActual.Location = new Point(12, 137);
      this.txtProcessDataCycleTimeActual.Name = "txtProcessDataCycleTimeActual";
      this.txtProcessDataCycleTimeActual.ReadOnly = true;
      this.txtProcessDataCycleTimeActual.Size = new Size(48, 20);
      this.txtProcessDataCycleTimeActual.TabIndex = 65;
      this.txtProcessDataCycleTimeActual.Text = "-";
      this.txtBusDiagnosticParameterRegister2.Location = new Point(12, 98);
      this.txtBusDiagnosticParameterRegister2.Name = "txtBusDiagnosticParameterRegister2";
      this.txtBusDiagnosticParameterRegister2.ReadOnly = true;
      this.txtBusDiagnosticParameterRegister2.Size = new Size(88, 20);
      this.txtBusDiagnosticParameterRegister2.TabIndex = 64;
      this.txtBusDiagnosticParameterRegister2.Text = "-";
      this.lblDiagnosticParameterRegister2.Location = new Point(12, 82);
      this.lblDiagnosticParameterRegister2.Name = "lblDiagnosticParameterRegister2";
      this.lblDiagnosticParameterRegister2.Size = new Size(168, 16);
      this.lblDiagnosticParameterRegister2.TabIndex = 63;
      this.lblDiagnosticParameterRegister2.Text = "Diagnostic Parameter Register II";
      this.lblDiagnosticParameterRegister2.TextAlign = ContentAlignment.MiddleLeft;
      this.label5.Location = new Point(108, 98);
      this.label5.Name = "label5";
      this.label5.Size = new Size(72, 16);
      this.label5.TabIndex = 62;
      this.label5.Text = "(hex)";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.txtBusDiagnosticParameterRegister.Location = new Point(12, 59);
      this.txtBusDiagnosticParameterRegister.Name = "txtBusDiagnosticParameterRegister";
      this.txtBusDiagnosticParameterRegister.ReadOnly = true;
      this.txtBusDiagnosticParameterRegister.Size = new Size(88, 20);
      this.txtBusDiagnosticParameterRegister.TabIndex = 61;
      this.txtBusDiagnosticParameterRegister.Text = "-";
      this.lblDiagnosticParameterRegister.Location = new Point(12, 43);
      this.lblDiagnosticParameterRegister.Name = "lblDiagnosticParameterRegister";
      this.lblDiagnosticParameterRegister.Size = new Size(160, 16);
      this.lblDiagnosticParameterRegister.TabIndex = 59;
      this.lblDiagnosticParameterRegister.Text = "Diagnostic Parameter Register";
      this.lblDiagnosticParameterRegister.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.Location = new Point(108, 59);
      this.label3.Name = "label3";
      this.label3.Size = new Size(72, 16);
      this.label3.TabIndex = 60;
      this.label3.Text = "(hex)";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.Controls.Add((Control) this.grpInterbus);
      this.MaximumSize = new Size(372, 280);
      this.MinimumSize = new Size(372, 280);
      this.Name = nameof (ctrlIBS_Diag);
      this.Size = new Size(372, 280);
      this.Load += new EventHandler(this.ctrlIBS_Diag_Load);
      this.grpBusControl.ResumeLayout(false);
      this.grpBusControl.PerformLayout();
      this.grpInterbus.ResumeLayout(false);
      this.grpBusDiagnostic.ResumeLayout(false);
      this.tabBusState.ResumeLayout(false);
      this.tabPageStateRegister.ResumeLayout(false);
      this.pnlRegister_1.ResumeLayout(false);
      this.pnlRegister_2.ResumeLayout(false);
      this.tabPageAllRegister.ResumeLayout(false);
      this.tabPageAllRegister.PerformLayout();
      this.ResumeLayout(false);
    }

    private void ctrlIBS_Diag_Load(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.ControlText))
        return;
      this.ControlText = this.Name;
    }

    public string ControlText
    {
      get
      {
        return this.grpInterbus.Text;
      }
      set
      {
        this.grpInterbus.Text = value;
      }
    }

    public IInterbusG4 ControlerConnection
    {
      set
      {
        this._myController = value;
      }
    }

    private void ShowState()
    {
      if (this._myController != null)
      {
        this.grpInterbus.Enabled = true;
        try
        {
          this.chkBusUSER.Checked = this._myController.BusDiag.StatusRegister.USER;
          this.chkBusPF.Checked = this._myController.BusDiag.StatusRegister.PF;
          this.chkBusBUS.Checked = this._myController.BusDiag.StatusRegister.BUS;
          this.chkBusCTRL.Checked = this._myController.BusDiag.StatusRegister.CTRL;
          this.chkBusDETECT.Checked = this._myController.BusDiag.StatusRegister.DETECT;
          this.chkBusRUN.Checked = this._myController.BusDiag.StatusRegister.RUN;
          this.chkBusACTIVE.Checked = this._myController.BusDiag.StatusRegister.ACTIVE;
          this.chkBusREADY.Checked = this._myController.BusDiag.StatusRegister.READY;
          this.chkBusBSA.Checked = this._myController.BusDiag.StatusRegister.BSA;
          this.chkBusBASP.Checked = this._myController.BusDiag.StatusRegister.BASP;
          this.chkBusRESULT.Checked = this._myController.BusDiag.StatusRegister.RESULT;
          this.chkBusSY_RESULT.Checked = this._myController.BusDiag.StatusRegister.SY_RESULT;
          this.chkBusDC_RESULT.Checked = this._myController.BusDiag.StatusRegister.DC_RESULT;
          this.chkBusWARNING.Checked = this._myController.BusDiag.StatusRegister.WARNING;
          this.chkBusQUALITY.Checked = this._myController.BusDiag.StatusRegister.QUALITY;
          this.chkBusSDSI.Checked = this._myController.BusDiag.StatusRegister.SDSI;
          this.txtProcessDataCycleTimeActual.Text = this._myController.BusHandling.CurrentCycleTime.ToString();
          this.txtBusDiagnosticStatusRegister.Text = this._myController.BusDiag.StatusRegister.Value.ToString("X4");
          TextBox parameterRegister1 = this.txtBusDiagnosticParameterRegister;
          int parameterRegister2 = this._myController.BusDiag.ParameterRegister;
          string str1 = parameterRegister2.ToString("X4");
          parameterRegister1.Text = str1;
          TextBox parameterRegister2_1 = this.txtBusDiagnosticParameterRegister2;
          parameterRegister2 = this._myController.BusDiag.ExtendedParameterRegister;
          string str2 = parameterRegister2.ToString("X4");
          parameterRegister2_1.Text = str2;
        }
        catch
        {
        }
      }
      else
      {
        this.grpInterbus.Enabled = false;
        this.chkBusUSER.Checked = false;
        this.chkBusPF.Checked = false;
        this.chkBusBUS.Checked = false;
        this.chkBusCTRL.Checked = false;
        this.chkBusDETECT.Checked = false;
        this.chkBusRUN.Checked = false;
        this.chkBusACTIVE.Checked = false;
        this.chkBusREADY.Checked = false;
        this.chkBusBSA.Checked = false;
        this.chkBusBASP.Checked = false;
        this.chkBusRESULT.Checked = false;
        this.chkBusSY_RESULT.Checked = false;
        this.chkBusDC_RESULT.Checked = false;
        this.chkBusWARNING.Checked = false;
        this.chkBusQUALITY.Checked = false;
        this.chkBusSDSI.Checked = false;
        this.txtProcessDataCycleTimeActual.Text = "-";
        this.txtBusDiagnosticStatusRegister.Text = "-";
        this.txtBusDiagnosticParameterRegister.Text = "-";
        this.txtBusDiagnosticParameterRegister2.Text = "-";
      }
    }

    private void ShowData()
    {
      this.ShowState();
      if (this._myController != null)
      {
        try
        {
          this.txtHandlingState.Text = this._myController.BusHandling.HandlingState.ToString();
        }
        catch
        {
        }
      }
      else
        this.txtHandlingState.Text = "na.";
    }

    public Timer UpdateData
    {
      set
      {
        this._locTimer = value;
        this._locTimer.Tick += new EventHandler(this._locTimer_Tick);
      }
    }

    private void _locTimer_Tick(object sender, EventArgs e)
    {
      this.ShowData();
    }

    private void btnCreateConfiguration_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.CreateConfiguration();
    }

    private void btnActivateConfig_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.ActivateConfiguration();
    }

    private void btnStartDataTransfer_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.StartDataTransfer();
    }

    private void btnAlarmStop_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.AlarmStop();
    }

    private void btnConfirmPF_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.ConfirmPeripheralFault();
    }

    private void btnConfirmDiagnostic_Click(object sender, EventArgs e)
    {
      if (this._myController == null)
        return;
      this._myController.BusHandling.ConfirmDiagnostics();
    }

    private void btnControllerRevision_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show(this._myController.BusHandling.RevisionInfo.ToString(), Application.ProductName);
    }
  }
}
