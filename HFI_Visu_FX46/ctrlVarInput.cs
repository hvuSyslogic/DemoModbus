// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.ctrlVarInput
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
  [ToolboxBitmap(typeof (ctrlVarInput), "picInput.bmp")]
  public class ctrlVarInput : UserControl
  {
    private List<VarInput> _varInput;
    private VarInput _myInput;
    private Timer _locTimer;
    private GroupBox grpInputs;
    private GroupBox grpProperties;
    private TextBox txtVarType;
    private Label lblVarType;
    private Label lblVariableLength;
    private TextBox txtVariableLength;
    private Label lblMinValue;
    private TextBox txtMinValue;
    private Label lblMaxValue;
    private TextBox txtMaxValue;
    private GroupBox grpByteArray;
    private Label lblByteArray;
    private TextBox txtByteArray;
    private GroupBox grpValueState;
    private Label lblActualState;
    private TextBox txtActualState;
    private Label lblActualValue;
    private TextBox txtActualValue;
    private Label lblBitOffset;
    private TextBox txtBitOffset;
    private Label lblBaseAddress;
    private TextBox txtBaseAddress;
    private Label lblByteLength;
    private TextBox txtByteLength;
    private CheckBox chkHexView;
    private GroupBox groupBox1;
    private Label lblObjects;
    private ListBox listInputs;
    private Button btnOffsetUp;
    private TextBox tbxOffset;
    private Button btnOffsetDown;
    private Label label1;
    private Container components;

    public ctrlVarInput()
    {
      this.InitializeComponent();
      this._varInput = new List<VarInput>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpInputs = new GroupBox();
      this.groupBox1 = new GroupBox();
      this.listInputs = new ListBox();
      this.lblObjects = new Label();
      this.grpByteArray = new GroupBox();
      this.btnOffsetUp = new Button();
      this.tbxOffset = new TextBox();
      this.btnOffsetDown = new Button();
      this.label1 = new Label();
      this.txtByteArray = new TextBox();
      this.lblByteArray = new Label();
      this.grpValueState = new GroupBox();
      this.lblActualValue = new Label();
      this.chkHexView = new CheckBox();
      this.lblActualState = new Label();
      this.txtActualState = new TextBox();
      this.txtActualValue = new TextBox();
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
      this.grpInputs.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.grpByteArray.SuspendLayout();
      this.grpValueState.SuspendLayout();
      this.grpProperties.SuspendLayout();
      this.SuspendLayout();
      this.grpInputs.Controls.Add((Control) this.groupBox1);
      this.grpInputs.Controls.Add((Control) this.grpByteArray);
      this.grpInputs.Controls.Add((Control) this.grpValueState);
      this.grpInputs.Controls.Add((Control) this.grpProperties);
      this.grpInputs.Location = new Point(0, 0);
      this.grpInputs.Name = "grpInputs";
      this.grpInputs.Size = new Size(370, 288);
      this.grpInputs.TabIndex = 0;
      this.grpInputs.TabStop = false;
      this.groupBox1.Controls.Add((Control) this.listInputs);
      this.groupBox1.Controls.Add((Control) this.lblObjects);
      this.groupBox1.Location = new Point(6, 16);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(158, 192);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Input Object List:";
      this.listInputs.Location = new Point(4, 38);
      this.listInputs.Name = "listInputs";
      this.listInputs.Size = new Size(148, 147);
      this.listInputs.TabIndex = 5;
      this.listInputs.SelectedIndexChanged += new EventHandler(this.listInputs_SelectedIndexChanged);
      this.lblObjects.Location = new Point(3, 19);
      this.lblObjects.Name = "lblObjects";
      this.lblObjects.Size = new Size(152, 16);
      this.lblObjects.TabIndex = 8;
      this.lblObjects.Text = "Available Objects: 0";
      this.lblObjects.TextAlign = ContentAlignment.BottomLeft;
      this.grpByteArray.Controls.Add((Control) this.txtByteArray);
      this.grpByteArray.Controls.Add((Control) this.btnOffsetUp);
      this.grpByteArray.Controls.Add((Control) this.tbxOffset);
      this.grpByteArray.Controls.Add((Control) this.lblByteArray);
      this.grpByteArray.Controls.Add((Control) this.label1);
      this.grpByteArray.Controls.Add((Control) this.btnOffsetDown);
      this.grpByteArray.Location = new Point(8, 208);
      this.grpByteArray.Name = "grpByteArray";
      this.grpByteArray.Size = new Size(356, 72);
      this.grpByteArray.TabIndex = 1;
      this.grpByteArray.TabStop = false;
      this.grpByteArray.Text = "Data";
      this.grpByteArray.Visible = false;
      this.btnOffsetUp.BackgroundImage = (Image) Resources.navigate_up;
      this.btnOffsetUp.BackgroundImageLayout = ImageLayout.Center;
      this.btnOffsetUp.Location = new Point(61, 36);
      this.btnOffsetUp.Name = "btnOffsetUp";
      this.btnOffsetUp.Size = new Size(19, 19);
      this.btnOffsetUp.TabIndex = 1;
      this.btnOffsetUp.UseVisualStyleBackColor = true;
      this.btnOffsetUp.Click += new EventHandler(this.btnOffsetUp_Click);
      this.tbxOffset.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tbxOffset.Location = new Point(29, 36);
      this.tbxOffset.Multiline = true;
      this.tbxOffset.Name = "tbxOffset";
      this.tbxOffset.ReadOnly = true;
      this.tbxOffset.Size = new Size(30, 19);
      this.tbxOffset.TabIndex = 22;
      this.tbxOffset.TabStop = false;
      this.tbxOffset.Text = "0";
      this.tbxOffset.TextAlign = HorizontalAlignment.Center;
      this.btnOffsetDown.BackgroundImage = (Image) Resources.navigate_down;
      this.btnOffsetDown.BackgroundImageLayout = ImageLayout.Center;
      this.btnOffsetDown.Location = new Point(8, 36);
      this.btnOffsetDown.Name = "btnOffsetDown";
      this.btnOffsetDown.Size = new Size(19, 19);
      this.btnOffsetDown.TabIndex = 0;
      this.btnOffsetDown.UseVisualStyleBackColor = true;
      this.btnOffsetDown.Click += new EventHandler(this.btnOffsetDown_Click);
      this.label1.Location = new Point(5, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 16);
      this.label1.TabIndex = 19;
      this.label1.Text = "Byte offset:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.txtByteArray.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtByteArray.Location = new Point(86, 36);
      this.txtByteArray.Name = "txtByteArray";
      this.txtByteArray.ReadOnly = true;
      this.txtByteArray.Size = new Size(262, 20);
      this.txtByteArray.TabIndex = 3;
      this.txtByteArray.TabStop = false;
      this.lblByteArray.Location = new Point(85, 19);
      this.lblByteArray.Name = "lblByteArray";
      this.lblByteArray.Size = new Size(96, 16);
      this.lblByteArray.TabIndex = 18;
      this.lblByteArray.Text = "Byte Array (hex):";
      this.lblByteArray.TextAlign = ContentAlignment.MiddleLeft;
      this.grpValueState.Controls.Add((Control) this.lblActualValue);
      this.grpValueState.Controls.Add((Control) this.chkHexView);
      this.grpValueState.Controls.Add((Control) this.lblActualState);
      this.grpValueState.Controls.Add((Control) this.txtActualState);
      this.grpValueState.Controls.Add((Control) this.txtActualValue);
      this.grpValueState.Location = new Point(8, 208);
      this.grpValueState.Name = "grpValueState";
      this.grpValueState.Size = new Size(356, 72);
      this.grpValueState.TabIndex = 22;
      this.grpValueState.TabStop = false;
      this.grpValueState.Text = "Data";
      this.lblActualValue.Location = new Point(8, 16);
      this.lblActualValue.Name = "lblActualValue";
      this.lblActualValue.Size = new Size(72, 23);
      this.lblActualValue.TabIndex = 17;
      this.lblActualValue.Text = "Actual Value:";
      this.lblActualValue.TextAlign = ContentAlignment.MiddleRight;
      this.chkHexView.Location = new Point(192, 16);
      this.chkHexView.Name = "chkHexView";
      this.chkHexView.Size = new Size(48, 24);
      this.chkHexView.TabIndex = 20;
      this.chkHexView.Text = "hex";
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
      this.txtActualValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtActualValue.Location = new Point(88, 16);
      this.txtActualValue.Name = "txtActualValue";
      this.txtActualValue.ReadOnly = true;
      this.txtActualValue.Size = new Size(88, 20);
      this.txtActualValue.TabIndex = 16;
      this.txtActualValue.Text = "-";
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
      this.grpProperties.Text = "Input Properties";
      this.lblByteLength.Location = new Point(8, 126);
      this.lblByteLength.Name = "lblByteLength";
      this.lblByteLength.Size = new Size(96, 20);
      this.lblByteLength.TabIndex = 17;
      this.lblByteLength.Text = "Byte Length:";
      this.lblByteLength.TextAlign = ContentAlignment.MiddleRight;
      this.txtByteLength.Location = new Point(112, 126);
      this.txtByteLength.Name = "txtByteLength";
      this.txtByteLength.ReadOnly = true;
      this.txtByteLength.Size = new Size(76, 20);
      this.txtByteLength.TabIndex = 16;
      this.txtByteLength.TabStop = false;
      this.txtByteLength.Text = "-";
      this.lblBitOffset.Location = new Point(8, 148);
      this.lblBitOffset.Name = "lblBitOffset";
      this.lblBitOffset.Size = new Size(96, 20);
      this.lblBitOffset.TabIndex = 15;
      this.lblBitOffset.Text = "Bit Offset:";
      this.lblBitOffset.TextAlign = ContentAlignment.MiddleRight;
      this.txtBitOffset.Location = new Point(112, 148);
      this.txtBitOffset.Name = "txtBitOffset";
      this.txtBitOffset.ReadOnly = true;
      this.txtBitOffset.Size = new Size(76, 20);
      this.txtBitOffset.TabIndex = 14;
      this.txtBitOffset.TabStop = false;
      this.txtBitOffset.Text = "-";
      this.lblBaseAddress.Location = new Point(8, 104);
      this.lblBaseAddress.Name = "lblBaseAddress";
      this.lblBaseAddress.Size = new Size(96, 20);
      this.lblBaseAddress.TabIndex = 13;
      this.lblBaseAddress.Text = "Base Address:";
      this.lblBaseAddress.TextAlign = ContentAlignment.MiddleRight;
      this.txtBaseAddress.Location = new Point(112, 104);
      this.txtBaseAddress.Name = "txtBaseAddress";
      this.txtBaseAddress.ReadOnly = true;
      this.txtBaseAddress.Size = new Size(76, 20);
      this.txtBaseAddress.TabIndex = 12;
      this.txtBaseAddress.TabStop = false;
      this.txtBaseAddress.Text = "-";
      this.lblMaxValue.Location = new Point(8, 82);
      this.lblMaxValue.Name = "lblMaxValue";
      this.lblMaxValue.Size = new Size(96, 20);
      this.lblMaxValue.TabIndex = 11;
      this.lblMaxValue.Text = "Maximum Value:";
      this.lblMaxValue.TextAlign = ContentAlignment.MiddleRight;
      this.txtMaxValue.Location = new Point(112, 82);
      this.txtMaxValue.Name = "txtMaxValue";
      this.txtMaxValue.ReadOnly = true;
      this.txtMaxValue.Size = new Size(76, 20);
      this.txtMaxValue.TabIndex = 10;
      this.txtMaxValue.TabStop = false;
      this.txtMaxValue.Text = "-";
      this.lblMinValue.Location = new Point(8, 60);
      this.lblMinValue.Name = "lblMinValue";
      this.lblMinValue.Size = new Size(96, 20);
      this.lblMinValue.TabIndex = 9;
      this.lblMinValue.Text = "Minimum Value:";
      this.lblMinValue.TextAlign = ContentAlignment.MiddleRight;
      this.txtMinValue.Location = new Point(112, 60);
      this.txtMinValue.Name = "txtMinValue";
      this.txtMinValue.ReadOnly = true;
      this.txtMinValue.Size = new Size(76, 20);
      this.txtMinValue.TabIndex = 8;
      this.txtMinValue.TabStop = false;
      this.txtMinValue.Text = "-";
      this.lblVariableLength.Location = new Point(8, 38);
      this.lblVariableLength.Name = "lblVariableLength";
      this.lblVariableLength.Size = new Size(96, 20);
      this.lblVariableLength.TabIndex = 7;
      this.lblVariableLength.Text = "Variable Length:";
      this.lblVariableLength.TextAlign = ContentAlignment.MiddleRight;
      this.txtVariableLength.Location = new Point(112, 38);
      this.txtVariableLength.Name = "txtVariableLength";
      this.txtVariableLength.ReadOnly = true;
      this.txtVariableLength.Size = new Size(76, 20);
      this.txtVariableLength.TabIndex = 6;
      this.txtVariableLength.TabStop = false;
      this.txtVariableLength.Text = "-";
      this.lblVarType.Location = new Point(8, 16);
      this.lblVarType.Name = "lblVarType";
      this.lblVarType.Size = new Size(96, 20);
      this.lblVarType.TabIndex = 1;
      this.lblVarType.Text = "Variable Type:";
      this.lblVarType.TextAlign = ContentAlignment.MiddleRight;
      this.txtVarType.Location = new Point(112, 16);
      this.txtVarType.Name = "txtVarType";
      this.txtVarType.ReadOnly = true;
      this.txtVarType.Size = new Size(76, 20);
      this.txtVarType.TabIndex = 0;
      this.txtVarType.TabStop = false;
      this.txtVarType.Text = "-";
      this.Controls.Add((Control) this.grpInputs);
      this.Name = nameof (ctrlVarInput);
      this.Size = new Size(371, 288);
      this.Load += new EventHandler(this.ctrlVarInput_Load);
      this.grpInputs.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.grpByteArray.ResumeLayout(false);
      this.grpByteArray.PerformLayout();
      this.grpValueState.ResumeLayout(false);
      this.grpValueState.PerformLayout();
      this.grpProperties.ResumeLayout(false);
      this.grpProperties.PerformLayout();
      this.ResumeLayout(false);
    }

    public string ControlText
    {
      get
      {
        return this.grpInputs.Text;
      }
      set
      {
        this.grpInputs.Text = value;
      }
    }

    public void AddObject(VarInput InputObject)
    {
      this._varInput.Add(InputObject);
      int num = this.listInputs.Items.Add((object) InputObject.ToString());
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varInput.Count.ToString());
      if (num != 0)
        return;
      this._myInput = this._varInput[0];
      this.listInputs.SelectedIndex = 0;
      this.ShowProperties();
    }

    public void ClearObject()
    {
      this._varInput.Clear();
      this.listInputs.Items.Clear();
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varInput.Count.ToString());
      this.ShowProperties();
    }

    private void ctrlVarInput_Load(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.ControlText))
        return;
      this.ControlText = this.Name;
    }

    private void listInputs_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listInputs.SelectedIndex <= -1)
        return;
      this._myInput = this._varInput[this.listInputs.SelectedIndex];
      this.ShowProperties();
    }

    private void ShowProperties()
    {
      if (this._myInput != null)
      {
        this.txtVarType.Text = this._myInput.VarType.ToString();
        this.txtBaseAddress.Text = this._myInput.BaseAddress.ToString();
        this.txtByteLength.Text = this._myInput.ByteLength.ToString();
        if (this._myInput.VarType == VarType.ByteArray)
        {
          this.txtVariableLength.Visible = false;
          this.txtMinValue.Visible = false;
          this.txtMaxValue.Visible = false;
          this.txtBitOffset.Visible = false;
          this.grpByteArray.Visible = true;
          this.grpValueState.Visible = false;
          this.txtByteArray.BackColor = Color.LightGray;
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
          this.txtActualValue.BackColor = Color.LightGray;
          this.txtActualState.BackColor = Color.LightGray;
          this.txtVariableLength.Text = this._myInput.Length.ToString();
          TextBox txtMinValue = this.txtMinValue;
          ulong num = this._myInput.MinValue;
          string str1 = num.ToString();
          txtMinValue.Text = str1;
          TextBox txtMaxValue = this.txtMaxValue;
          num = this._myInput.MaxValue;
          string str2 = num.ToString();
          txtMaxValue.Text = str2;
          this.txtBitOffset.Text = this._myInput.BitOffset.ToString();
          if (this.chkHexView.Checked)
          {
            TextBox txtActualValue = this.txtActualValue;
            num = this._myInput.Value;
            string str3 = num.ToString("X");
            txtActualValue.Text = str3;
          }
          else
          {
            TextBox txtActualValue = this.txtActualValue;
            num = this._myInput.Value;
            string str3 = num.ToString();
            txtActualValue.Text = str3;
          }
          this.txtActualState.Text = this._myInput.State.ToString();
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
        this.txtActualValue.BackColor = Color.LightGray;
        this.txtActualState.BackColor = Color.LightGray;
        this.txtVariableLength.Text = "-";
        this.txtMinValue.Text = "-";
        this.txtMaxValue.Text = "-";
        this.txtBitOffset.Text = "-";
        if (this.chkHexView.Checked)
          this.txtActualValue.Text = "-";
        else
          this.txtActualValue.Text = "-";
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
      this.txtActualValue.BackColor = Color.LightGreen;
      this.txtActualState.BackColor = Color.LightGreen;
      this.txtByteArray.BackColor = Color.LightGreen;
      if (this._myInput == null)
        return;
      if (this._myInput.VarType == VarType.ByteArray)
      {
        this.ShowByteArray();
      }
      else
      {
        if (this.chkHexView.Checked)
          this.txtActualValue.Text = this._myInput.Value.ToString("X");
        else
          this.txtActualValue.Text = this._myInput.Value.ToString();
        this.txtActualState.Text = this._myInput.State.ToString();
      }
    }

    private void ShowByteArray()
    {
      this.txtByteArray.Text = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      if (this._myInput == null || this._myInput.ByteArray == null)
        return;
      int num = Convert.ToInt32(this.tbxOffset.Text);
      if (num >= this._myInput.ByteArray.Length)
      {
        num = 0;
        this.tbxOffset.Text = "0";
      }
      for (int index = num; index < this._myInput.ByteArray.Length; ++index)
      {
        stringBuilder.Append(this._myInput.ByteArray[index].ToString("X2"));
        if (index < this._myInput.ByteArray.Length - 1)
          stringBuilder.Append(",");
      }
      this.txtByteArray.Text = stringBuilder.ToString();
    }

    private void btnOffsetUp_Click(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(this.tbxOffset.Text);
      if (int32 >= this._myInput.ByteArray.Length - 1)
        return;
      this.tbxOffset.Text = (int32 + 1).ToString();
    }

    private void btnOffsetDown_Click(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(this.tbxOffset.Text);
      if (int32 <= 0)
        return;
      this.tbxOffset.Text = (int32 - 1).ToString();
    }
  }
}
