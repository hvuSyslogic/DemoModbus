// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.ctrlVarOutput
// Assembly: HFI_Visu_FX46, Version=3.2.6053.23250, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: A9FB10B7-9AE3-4F4C-88CF-1D5F3BF257DC
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Visu_FX46.dll

using PhoenixContact.HFI.Inline;
using PhoenixContact.HFI.Visualization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PhoenixContact.HFI.Visualization
{
  [ToolboxBitmap(typeof (ctrlVarOutput), "picOutput.bmp")]
  public class ctrlVarOutput : UserControl
  {
    private List<VarOutput> _varOutput;
    private VarOutput _myOutput;
    private Timer _locTimer;
    private bool _editActive;
    private bool _editMode;
    private GroupBox grpProperties;
    private TextBox txtVarType;
    private Label lblVarType;
    private Label lblVariableLength;
    private TextBox txtVariableLength;
    private Label lblMinValue;
    private TextBox txtMinValue;
    private Label lblMaxValue;
    private TextBox txtMaxValue;
    private Label lblObjects;
    private GroupBox grpByteArray;
    private Label lblByteArray;
    private TextBox tbxByteArray;
    private GroupBox grpValueState;
    private Label lblActualState;
    private TextBox txtActualState;
    private Label lblActualValue;
    private TextBox tbxActualValue;
    private GroupBox grpOutputs;
    private Label lblBitOffset;
    private TextBox txtBitOffset;
    private Label lblBaseAddress;
    private TextBox txtBaseAddress;
    private Label lblByteLength;
    private TextBox txtByteLength;
    private CheckBox cbxHexView;
    private GroupBox gbxOutputObjectList;
    private ListBox listOutputs;
    private Button btnOffsetUp;
    private TextBox tbxOffset;
    private Label label1;
    private Button btnOffsetDown;
    private ErrorProvider errorProvider1;
    private IContainer components;

    public ctrlVarOutput()
    {
      this.InitializeComponent();
      this._varOutput = new List<VarOutput>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.grpOutputs = new GroupBox();
      this.gbxOutputObjectList = new GroupBox();
      this.listOutputs = new ListBox();
      this.lblObjects = new Label();
      this.grpByteArray = new GroupBox();
      this.btnOffsetUp = new Button();
      this.tbxOffset = new TextBox();
      this.label1 = new Label();
      this.btnOffsetDown = new Button();
      this.lblByteArray = new Label();
      this.tbxByteArray = new TextBox();
      this.grpProperties = new GroupBox();
      this.lblByteLength = new Label();
      this.txtByteLength = new TextBox();
      this.lblBitOffset = new Label();
      this.txtBitOffset = new TextBox();
      this.lblBaseAddress = new Label();
      this.txtBaseAddress = new TextBox();
      this.lblMaxValue = new Label();
      this.txtMaxValue = new TextBox();
      this.lblMinValue = new Label();
      this.txtMinValue = new TextBox();
      this.lblVariableLength = new Label();
      this.txtVariableLength = new TextBox();
      this.lblVarType = new Label();
      this.txtVarType = new TextBox();
      this.grpValueState = new GroupBox();
      this.cbxHexView = new CheckBox();
      this.lblActualState = new Label();
      this.txtActualState = new TextBox();
      this.lblActualValue = new Label();
      this.tbxActualValue = new TextBox();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.grpOutputs.SuspendLayout();
      this.gbxOutputObjectList.SuspendLayout();
      this.grpByteArray.SuspendLayout();
      this.grpProperties.SuspendLayout();
      this.grpValueState.SuspendLayout();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.SuspendLayout();
      this.grpOutputs.Controls.Add((Control) this.gbxOutputObjectList);
      this.grpOutputs.Controls.Add((Control) this.grpByteArray);
      this.grpOutputs.Controls.Add((Control) this.grpProperties);
      this.grpOutputs.Controls.Add((Control) this.grpValueState);
      this.grpOutputs.Location = new Point(0, 0);
      this.grpOutputs.Name = "grpOutputs";
      this.grpOutputs.Size = new Size(370, 288);
      this.grpOutputs.TabIndex = 4;
      this.grpOutputs.TabStop = false;
      this.gbxOutputObjectList.Controls.Add((Control) this.listOutputs);
      this.gbxOutputObjectList.Controls.Add((Control) this.lblObjects);
      this.gbxOutputObjectList.Location = new Point(6, 16);
      this.gbxOutputObjectList.Name = "gbxOutputObjectList";
      this.gbxOutputObjectList.Size = new Size(158, 192);
      this.gbxOutputObjectList.TabIndex = 19;
      this.gbxOutputObjectList.TabStop = false;
      this.gbxOutputObjectList.Text = "Output Object List:";
      this.listOutputs.Location = new Point(3, 29);
      this.listOutputs.Name = "listOutputs";
      this.listOutputs.Size = new Size(152, 160);
      this.listOutputs.TabIndex = 5;
      this.listOutputs.SelectedIndexChanged += new EventHandler(this.listOutputs_SelectedIndexChanged);
      this.lblObjects.Location = new Point(3, 11);
      this.lblObjects.Name = "lblObjects";
      this.lblObjects.Size = new Size(140, 16);
      this.lblObjects.TabIndex = 7;
      this.lblObjects.Text = "Available Objects: {0}";
      this.lblObjects.TextAlign = ContentAlignment.BottomLeft;
      this.grpByteArray.Controls.Add((Control) this.btnOffsetUp);
      this.grpByteArray.Controls.Add((Control) this.tbxOffset);
      this.grpByteArray.Controls.Add((Control) this.label1);
      this.grpByteArray.Controls.Add((Control) this.btnOffsetDown);
      this.grpByteArray.Controls.Add((Control) this.lblByteArray);
      this.grpByteArray.Controls.Add((Control) this.tbxByteArray);
      this.grpByteArray.Location = new Point(8, 208);
      this.grpByteArray.Name = "grpByteArray";
      this.grpByteArray.Size = new Size(356, 72);
      this.grpByteArray.TabIndex = 21;
      this.grpByteArray.TabStop = false;
      this.grpByteArray.Text = "Data";
      this.grpByteArray.Visible = false;
      this.btnOffsetUp.BackgroundImage = (Image) Resources.navigate_up;
      this.btnOffsetUp.BackgroundImageLayout = ImageLayout.Center;
      this.btnOffsetUp.Location = new Point(63, 37);
      this.btnOffsetUp.Name = "btnOffsetUp";
      this.btnOffsetUp.Size = new Size(19, 19);
      this.btnOffsetUp.TabIndex = 24;
      this.btnOffsetUp.UseVisualStyleBackColor = true;
      this.btnOffsetUp.Click += new EventHandler(this.btnOffsetUp_Click);
      this.tbxOffset.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tbxOffset.Location = new Point(31, 37);
      this.tbxOffset.Multiline = true;
      this.tbxOffset.Name = "tbxOffset";
      this.tbxOffset.ReadOnly = true;
      this.tbxOffset.Size = new Size(30, 19);
      this.tbxOffset.TabIndex = 26;
      this.tbxOffset.TabStop = false;
      this.tbxOffset.Text = "0";
      this.tbxOffset.TextAlign = HorizontalAlignment.Center;
      this.label1.Location = new Point(7, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 16);
      this.label1.TabIndex = 25;
      this.label1.Text = "Byte offset:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOffsetDown.BackgroundImage = (Image) Resources.navigate_down;
      this.btnOffsetDown.BackgroundImageLayout = ImageLayout.Center;
      this.btnOffsetDown.Location = new Point(10, 37);
      this.btnOffsetDown.Name = "btnOffsetDown";
      this.btnOffsetDown.Size = new Size(19, 19);
      this.btnOffsetDown.TabIndex = 23;
      this.btnOffsetDown.UseVisualStyleBackColor = true;
      this.btnOffsetDown.Click += new EventHandler(this.btnOffsetDown_Click);
      this.lblByteArray.Location = new Point(85, 19);
      this.lblByteArray.Name = "lblByteArray";
      this.lblByteArray.Size = new Size(96, 16);
      this.lblByteArray.TabIndex = 18;
      this.lblByteArray.Text = "Byte Array (hex):";
      this.lblByteArray.TextAlign = ContentAlignment.MiddleLeft;
      this.tbxByteArray.BackColor = SystemColors.Window;
      this.tbxByteArray.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tbxByteArray.Location = new Point(88, 36);
      this.tbxByteArray.Name = "tbxByteArray";
      this.tbxByteArray.Size = new Size(250, 20);
      this.tbxByteArray.TabIndex = 17;
      this.tbxByteArray.KeyDown += new KeyEventHandler(this.txtByteArray_KeyDown);
      this.tbxByteArray.Leave += new EventHandler(this.txtByteArray_Leave);
      this.tbxByteArray.MouseDown += new MouseEventHandler(this.txtByteArray_MouseDown);
      this.tbxByteArray.KeyPress += new KeyPressEventHandler(this.txtByteArray_KeyPress);
      this.tbxByteArray.Enter += new EventHandler(this.txtByteArray_Enter);
      this.grpProperties.Controls.Add((Control) this.lblByteLength);
      this.grpProperties.Controls.Add((Control) this.txtByteLength);
      this.grpProperties.Controls.Add((Control) this.lblBitOffset);
      this.grpProperties.Controls.Add((Control) this.txtBitOffset);
      this.grpProperties.Controls.Add((Control) this.lblBaseAddress);
      this.grpProperties.Controls.Add((Control) this.txtBaseAddress);
      this.grpProperties.Controls.Add((Control) this.lblMaxValue);
      this.grpProperties.Controls.Add((Control) this.txtMaxValue);
      this.grpProperties.Controls.Add((Control) this.lblMinValue);
      this.grpProperties.Controls.Add((Control) this.txtMinValue);
      this.grpProperties.Controls.Add((Control) this.lblVariableLength);
      this.grpProperties.Controls.Add((Control) this.txtVariableLength);
      this.grpProperties.Controls.Add((Control) this.lblVarType);
      this.grpProperties.Controls.Add((Control) this.txtVarType);
      this.grpProperties.Location = new Point(168, 16);
      this.grpProperties.Name = "grpProperties";
      this.grpProperties.Size = new Size(196, 192);
      this.grpProperties.TabIndex = 5;
      this.grpProperties.TabStop = false;
      this.grpProperties.Text = "Output Properties";
      this.lblByteLength.Location = new Point(7, 126);
      this.lblByteLength.Name = "lblByteLength";
      this.lblByteLength.Size = new Size(87, 20);
      this.lblByteLength.TabIndex = 17;
      this.lblByteLength.Text = "Byte Length:";
      this.lblByteLength.TextAlign = ContentAlignment.MiddleRight;
      this.txtByteLength.Location = new Point(102, 126);
      this.txtByteLength.Name = "txtByteLength";
      this.txtByteLength.ReadOnly = true;
      this.txtByteLength.Size = new Size(76, 20);
      this.txtByteLength.TabIndex = 16;
      this.txtByteLength.Text = "-";
      this.lblBitOffset.Location = new Point(7, 148);
      this.lblBitOffset.Name = "lblBitOffset";
      this.lblBitOffset.Size = new Size(87, 20);
      this.lblBitOffset.TabIndex = 15;
      this.lblBitOffset.Text = "Bit Offset:";
      this.lblBitOffset.TextAlign = ContentAlignment.MiddleRight;
      this.txtBitOffset.Location = new Point(102, 148);
      this.txtBitOffset.Name = "txtBitOffset";
      this.txtBitOffset.ReadOnly = true;
      this.txtBitOffset.Size = new Size(76, 20);
      this.txtBitOffset.TabIndex = 14;
      this.txtBitOffset.Text = "-";
      this.lblBaseAddress.Location = new Point(7, 104);
      this.lblBaseAddress.Name = "lblBaseAddress";
      this.lblBaseAddress.Size = new Size(87, 20);
      this.lblBaseAddress.TabIndex = 13;
      this.lblBaseAddress.Text = "Base Address:";
      this.lblBaseAddress.TextAlign = ContentAlignment.MiddleRight;
      this.txtBaseAddress.Location = new Point(102, 104);
      this.txtBaseAddress.Name = "txtBaseAddress";
      this.txtBaseAddress.ReadOnly = true;
      this.txtBaseAddress.Size = new Size(76, 20);
      this.txtBaseAddress.TabIndex = 12;
      this.txtBaseAddress.Text = "-";
      this.lblMaxValue.Location = new Point(7, 82);
      this.lblMaxValue.Name = "lblMaxValue";
      this.lblMaxValue.Size = new Size(87, 20);
      this.lblMaxValue.TabIndex = 11;
      this.lblMaxValue.Text = "Maximum Value:";
      this.lblMaxValue.TextAlign = ContentAlignment.MiddleRight;
      this.txtMaxValue.Location = new Point(102, 82);
      this.txtMaxValue.Name = "txtMaxValue";
      this.txtMaxValue.ReadOnly = true;
      this.txtMaxValue.Size = new Size(76, 20);
      this.txtMaxValue.TabIndex = 10;
      this.txtMaxValue.Text = "-";
      this.lblMinValue.Location = new Point(7, 60);
      this.lblMinValue.Name = "lblMinValue";
      this.lblMinValue.Size = new Size(87, 20);
      this.lblMinValue.TabIndex = 9;
      this.lblMinValue.Text = "Minimum Value:";
      this.lblMinValue.TextAlign = ContentAlignment.MiddleRight;
      this.txtMinValue.Location = new Point(102, 60);
      this.txtMinValue.Name = "txtMinValue";
      this.txtMinValue.ReadOnly = true;
      this.txtMinValue.Size = new Size(76, 20);
      this.txtMinValue.TabIndex = 8;
      this.txtMinValue.Text = "-";
      this.lblVariableLength.Location = new Point(7, 38);
      this.lblVariableLength.Name = "lblVariableLength";
      this.lblVariableLength.Size = new Size(87, 20);
      this.lblVariableLength.TabIndex = 7;
      this.lblVariableLength.Text = "Variable Length:";
      this.lblVariableLength.TextAlign = ContentAlignment.MiddleRight;
      this.txtVariableLength.Location = new Point(102, 38);
      this.txtVariableLength.Name = "txtVariableLength";
      this.txtVariableLength.ReadOnly = true;
      this.txtVariableLength.Size = new Size(76, 20);
      this.txtVariableLength.TabIndex = 6;
      this.txtVariableLength.Text = "-";
      this.lblVarType.Location = new Point(7, 16);
      this.lblVarType.Name = "lblVarType";
      this.lblVarType.Size = new Size(87, 20);
      this.lblVarType.TabIndex = 1;
      this.lblVarType.Text = "Variable Type:";
      this.lblVarType.TextAlign = ContentAlignment.MiddleRight;
      this.txtVarType.Location = new Point(102, 16);
      this.txtVarType.Name = "txtVarType";
      this.txtVarType.ReadOnly = true;
      this.txtVarType.Size = new Size(76, 20);
      this.txtVarType.TabIndex = 0;
      this.txtVarType.Text = "-";
      this.grpValueState.Controls.Add((Control) this.cbxHexView);
      this.grpValueState.Controls.Add((Control) this.lblActualState);
      this.grpValueState.Controls.Add((Control) this.txtActualState);
      this.grpValueState.Controls.Add((Control) this.lblActualValue);
      this.grpValueState.Controls.Add((Control) this.tbxActualValue);
      this.grpValueState.Location = new Point(8, 208);
      this.grpValueState.Name = "grpValueState";
      this.grpValueState.Size = new Size(356, 72);
      this.grpValueState.TabIndex = 22;
      this.grpValueState.TabStop = false;
      this.grpValueState.Text = "Data";
      this.cbxHexView.Location = new Point(192, 16);
      this.cbxHexView.Name = "cbxHexView";
      this.cbxHexView.Size = new Size(48, 24);
      this.cbxHexView.TabIndex = 21;
      this.cbxHexView.Text = "hex";
      this.lblActualState.Location = new Point(8, 40);
      this.lblActualState.Name = "lblActualState";
      this.lblActualState.Size = new Size(72, 23);
      this.lblActualState.TabIndex = 19;
      this.lblActualState.Text = "Actual State:";
      this.lblActualState.TextAlign = ContentAlignment.MiddleRight;
      this.txtActualState.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtActualState.Location = new Point(88, 40);
      this.txtActualState.Name = "txtActualState";
      this.txtActualState.ReadOnly = true;
      this.txtActualState.Size = new Size(88, 20);
      this.txtActualState.TabIndex = 18;
      this.txtActualState.Text = "-";
      this.lblActualValue.Location = new Point(8, 16);
      this.lblActualValue.Name = "lblActualValue";
      this.lblActualValue.Size = new Size(72, 23);
      this.lblActualValue.TabIndex = 17;
      this.lblActualValue.Text = "Actual Value:";
      this.lblActualValue.TextAlign = ContentAlignment.MiddleRight;
      this.tbxActualValue.Cursor = Cursors.IBeam;
      this.tbxActualValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tbxActualValue.Location = new Point(88, 16);
      this.tbxActualValue.Name = "tbxActualValue";
      this.tbxActualValue.Size = new Size(88, 20);
      this.tbxActualValue.TabIndex = 16;
      this.tbxActualValue.Text = "-";
      this.tbxActualValue.KeyDown += new KeyEventHandler(this.txtActualValue_KeyDown);
      this.tbxActualValue.Leave += new EventHandler(this.txtActualValue_Leave);
      this.tbxActualValue.MouseDown += new MouseEventHandler(this.txtActualValue_MouseDown);
      this.tbxActualValue.KeyPress += new KeyPressEventHandler(this.txtActualValue_KeyPress);
      this.tbxActualValue.Enter += new EventHandler(this.txtActualValue_Enter);
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      this.Controls.Add((Control) this.grpOutputs);
      this.Name = nameof (ctrlVarOutput);
      this.Size = new Size(370, 288);
      this.Load += new EventHandler(this.UserControl1_Load);
      this.grpOutputs.ResumeLayout(false);
      this.gbxOutputObjectList.ResumeLayout(false);
      this.grpByteArray.ResumeLayout(false);
      this.grpByteArray.PerformLayout();
      this.grpProperties.ResumeLayout(false);
      this.grpProperties.PerformLayout();
      this.grpValueState.ResumeLayout(false);
      this.grpValueState.PerformLayout();
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.ResumeLayout(false);
    }

    public string ControlText
    {
      get
      {
        return this.grpOutputs.Text;
      }
      set
      {
        this.grpOutputs.Text = value;
      }
    }

    public bool EditActivate
    {
      get
      {
        return this._editMode;
      }
      set
      {
        this._editMode = value;
      }
    }

    public void AddObject(VarOutput OutputObject)
    {
      this._varOutput.Add(OutputObject);
      int num = this.listOutputs.Items.Add((object) OutputObject.ToString());
      this.lblObjects.Text = string.Format(this.lblObjects.Text, (object) this._varOutput.Count);
      if (num != 0)
        return;
      this._myOutput = this._varOutput[0];
      this.listOutputs.SelectedIndex = 0;
      this.ShowProperties();
    }

    public void ClearObject()
    {
      this._varOutput.Clear();
      this.listOutputs.Items.Clear();
      this.lblObjects.Text = string.Format(this.lblObjects.Text, (object) this._varOutput.Count);
      this.ShowProperties();
    }

    private void UserControl1_Load(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.ControlText))
        return;
      this.ControlText = this.Name;
    }

    private void listOutputs_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listOutputs.SelectedIndex <= -1)
        return;
      this._myOutput = this._varOutput[this.listOutputs.SelectedIndex];
      this.ShowProperties();
    }

    private void ShowProperties()
    {
      if (this._myOutput != null)
      {
        try
        {
          this.txtVarType.Text = this._myOutput.VarType.ToString();
          this.txtBaseAddress.Text = this._myOutput.BaseAddress.ToString();
          this.txtByteLength.Text = this._myOutput.ByteLength.ToString();
          if (this._myOutput.VarType == VarType.ByteArray)
          {
            this.txtVariableLength.Visible = false;
            this.txtMinValue.Visible = false;
            this.txtMaxValue.Visible = false;
            this.txtBitOffset.Visible = false;
            this.grpByteArray.Visible = true;
            this.grpValueState.Visible = false;
            this.tbxByteArray.BackColor = Color.LightGray;
            this.ShowByteArray();
          }
          else
          {
            this.txtVariableLength.Visible = true;
            this.txtMinValue.Visible = true;
            this.txtMaxValue.Visible = true;
            this.txtBitOffset.Visible = true;
            this.grpByteArray.Visible = false;
            this.grpValueState.Visible = true;
            this.tbxActualValue.BackColor = Color.LightGray;
            this.txtActualState.BackColor = Color.LightGray;
            this.txtVariableLength.Text = this._myOutput.Length.ToString();
            TextBox txtMinValue = this.txtMinValue;
            ulong num = this._myOutput.MinValue;
            string str1 = num.ToString();
            txtMinValue.Text = str1;
            TextBox txtMaxValue = this.txtMaxValue;
            num = this._myOutput.MaxValue;
            string str2 = num.ToString();
            txtMaxValue.Text = str2;
            this.txtBitOffset.Text = this._myOutput.BitOffset.ToString();
            if (this.cbxHexView.Checked)
            {
              TextBox tbxActualValue = this.tbxActualValue;
              num = this._myOutput.Value;
              string str3 = num.ToString("X");
              tbxActualValue.Text = str3;
            }
            else
            {
              TextBox tbxActualValue = this.tbxActualValue;
              num = this._myOutput.Value;
              string str3 = num.ToString();
              tbxActualValue.Text = str3;
            }
            this.txtActualState.Text = this._myOutput.State.ToString();
          }
        }
        catch
        {
        }
      }
      else
      {
        this.txtVarType.Text = "-";
        this.txtBaseAddress.Text = "-";
        this.txtByteLength.Text = "-";
        this.txtVariableLength.Visible = true;
        this.txtMinValue.Visible = true;
        this.txtMaxValue.Visible = true;
        this.txtBitOffset.Visible = true;
        this.grpByteArray.Visible = false;
        this.grpValueState.Visible = true;
        this.tbxActualValue.BackColor = Color.LightGray;
        this.txtActualState.BackColor = Color.LightGray;
        this.txtVariableLength.Text = "-";
        this.txtMinValue.Text = "-";
        this.txtMaxValue.Text = "-";
        this.txtBitOffset.Text = "-";
        if (this.cbxHexView.Checked)
          this.tbxActualValue.Text = "-";
        else
          this.tbxActualValue.Text = "-";
        this.txtActualState.Text = "-";
      }
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
      if (this._myOutput == null)
        return;
      try
      {
        if (!this._editActive)
        {
          this.tbxByteArray.ReadOnly = true;
          this.tbxActualValue.BackColor = Color.LightGreen;
          this.txtActualState.BackColor = Color.LightGreen;
          this.tbxByteArray.BackColor = Color.LightGreen;
          if (this._myOutput.VarType == VarType.ByteArray)
          {
            this.ShowByteArray();
          }
          else
          {
            if (this.cbxHexView.Checked)
              this.tbxActualValue.Text = this._myOutput.Value.ToString("X");
            else
              this.tbxActualValue.Text = this._myOutput.Value.ToString();
            this.txtActualState.Text = this._myOutput.State.ToString();
          }
        }
        else
        {
          this.tbxByteArray.ReadOnly = false;
          this.tbxActualValue.BackColor = Color.Yellow;
          this.txtActualState.BackColor = Color.LightGray;
          this.tbxByteArray.BackColor = Color.Yellow;
        }
      }
      catch
      {
      }
    }

    private void txtActualValue_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      e.Handled = true;
    }

    private void txtActualValue_Enter(object sender, EventArgs e)
    {
      if (!this._editMode)
        return;
      this._editActive = true;
    }

    private void txtActualValue_MouseDown(object sender, MouseEventArgs e)
    {
      if (!this._editMode)
        return;
      this._editActive = true;
    }

    private void txtActualValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return && !this._editActive)
      {
        if (!this._editMode)
          return;
        this._editActive = true;
      }
      else if (e.KeyCode == Keys.Delete && !this._editActive)
      {
        if (!this._editMode)
          return;
        this._editActive = true;
      }
      else
      {
        if (e.KeyCode == Keys.Return)
        {
          if (this._editActive)
          {
            try
            {
              this.errorProvider1.Clear();
              this._myOutput.Value = !this.cbxHexView.Checked ? Convert.ToUInt64(this.tbxActualValue.Text, 10) : Convert.ToUInt64(this.tbxActualValue.Text, 16);
              this._editActive = false;
              this.ShowProperties();
              return;
            }
            catch
            {
              this.errorProvider1.SetError((Control) this.tbxActualValue, "Illegal variable format.");
              this._editActive = false;
              this.ShowProperties();
              return;
            }
          }
        }
        if (e.KeyCode != Keys.Escape || !this._editActive)
          return;
        this._editActive = false;
      }
    }

    private void txtActualValue_Leave(object sender, EventArgs e)
    {
      try
      {
        this.errorProvider1.Clear();
        this._myOutput.Value = !this.cbxHexView.Checked ? Convert.ToUInt64(this.tbxActualValue.Text, 10) : Convert.ToUInt64(this.tbxActualValue.Text, 16);
        this._editActive = false;
        this.ShowProperties();
      }
      catch
      {
        this.errorProvider1.SetError((Control) this.tbxActualValue, "Illegal variable format.");
        this._editActive = false;
        this.ShowProperties();
      }
    }

    private void txtByteArray_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      e.Handled = true;
    }

    private void txtByteArray_Enter(object sender, EventArgs e)
    {
      if (!this._editMode)
        return;
      this._editActive = true;
    }

    private void txtByteArray_MouseDown(object sender, MouseEventArgs e)
    {
      if (!this._editMode)
        return;
      this._editActive = true;
    }

    private void txtByteArray_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return && !this._editActive)
      {
        if (!this._editMode)
          return;
        this._editActive = true;
      }
      else if (e.KeyCode == Keys.Delete && !this._editActive)
      {
        if (!this._editMode)
          return;
        this._editActive = true;
      }
      else
      {
        if (e.KeyCode == Keys.Return)
        {
          if (this._editActive)
          {
            try
            {
              this.errorProvider1.Clear();
              if (!this.WriteTextboxToByteArray(this._myOutput))
                this.errorProvider1.SetError((Control) this.tbxByteArray, "Illegal format or length.");
              this._editActive = false;
              this.ShowProperties();
              return;
            }
            catch
            {
              this._editActive = false;
              this.ShowProperties();
              return;
            }
          }
        }
        if (e.KeyCode != Keys.Escape || !this._editActive)
          return;
        this._editActive = false;
      }
    }

    private void txtByteArray_Leave(object sender, EventArgs e)
    {
      try
      {
        this.errorProvider1.Clear();
        if (!this.WriteTextboxToByteArray(this._myOutput))
          this.errorProvider1.SetError((Control) this.tbxByteArray, "Illegal format or length.");
        this._editActive = false;
        this.ShowProperties();
      }
      catch
      {
        this._editActive = false;
        this.ShowProperties();
      }
    }

    private void btnOffsetDown_Click(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(this.tbxOffset.Text);
      if (int32 <= 0)
        return;
      this.tbxOffset.Text = (int32 - 1).ToString();
    }

    private void btnOffsetUp_Click(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(this.tbxOffset.Text);
      if (int32 >= this._myOutput.ByteArray.Length - 1)
        return;
      this.tbxOffset.Text = (int32 + 1).ToString();
    }

    private void ShowByteArray()
    {
      this.tbxByteArray.Text = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      if (this._myOutput == null || this._myOutput.ByteArray == null)
        return;
      int num = Convert.ToInt32(this.tbxOffset.Text);
      if (num >= this._myOutput.ByteArray.Length)
      {
        num = 0;
        this.tbxOffset.Text = "0";
      }
      for (int index = num; index < this._myOutput.ByteArray.Length; ++index)
      {
        stringBuilder.Append(this._myOutput.ByteArray[index].ToString("X2"));
        if (index < this._myOutput.ByteArray.Length - 1)
          stringBuilder.Append(",");
      }
      this.tbxByteArray.Text = stringBuilder.ToString();
    }

    private bool WriteTextboxToByteArray(VarOutput pVariable)
    {
      if (!string.IsNullOrEmpty(this.tbxByteArray.Text))
      {
        try
        {
          string[] strArray = this.tbxByteArray.Text.Split(',');
          if (strArray != null)
          {
            int num = Convert.ToInt32(this.tbxOffset.Text);
            if (num >= pVariable.ByteArray.Length)
            {
              num = 0;
              this.tbxOffset.Text = "0";
            }
            int index1 = 0;
            for (int index2 = num; index2 < pVariable.ByteArray.Length; ++index2)
            {
              pVariable.ByteArray[index2] = Convert.ToByte(strArray[index1], 16);
              if (index1 >= strArray.Length)
                return true;
              ++index1;
            }
            return true;
          }
        }
        catch
        {
        }
      }
      return false;
    }
  }
}
