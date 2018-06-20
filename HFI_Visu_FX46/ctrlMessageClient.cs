// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.ctrlMessageClient
// Assembly: HFI_Visu_FX46, Version=3.2.6053.23250, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: A9FB10B7-9AE3-4F4C-88CF-1D5F3BF257DC
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Visu_FX46.dll

using PhoenixContact.HFI.Inline;
using PhoenixContact.PxC_Library.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PhoenixContact.HFI.Visualization
{
  [ToolboxBitmap(typeof (ctrlMessageClient), "picMessage.bmp")]
  public class ctrlMessageClient : UserControl
  {
    private List<MessageClient> _varMessageClient;
    private MessageClient _myMessageClient;
    private Timer _locTimer;
    private int _lastSendDataLength;
    private DateTime _lastSendDataTime;
    private int _lastReceiveDataLength;
    private DateTime _lastReceiveDataTime;
    private GroupBox grpProperties;
    private Label lblObjects;
    private Label lblOutputList;
    private Label lblName;
    private TextBox txtName;
    private Label label2;
    private ListBox listMessageClient;
    private Label lblReceiveDataTimeout;
    private Label lblDiagnosticActive;
    private TextBox txtDeviceDiagActive;
    private Label lblState;
    private TextBox txtState;
    private Label lblReceiveDataTime;
    private TextBox txtReceiveDataTime;
    private Label lblSendDataTime;
    private TextBox txtSendDataTime;
    private TextBox txtReceiveDataTimeout;
    private ListBox lstSendData;
    private ListBox lstReceiveData;
    private GroupBox grpMessageClient;
    private Label label1;
    private TextBox txtID;
    private Container components;

    public ctrlMessageClient()
    {
      this.InitializeComponent();
      this._varMessageClient = new List<MessageClient>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpMessageClient = new GroupBox();
      this.lblObjects = new Label();
      this.grpProperties = new GroupBox();
      this.label1 = new Label();
      this.txtID = new TextBox();
      this.lstReceiveData = new ListBox();
      this.lstSendData = new ListBox();
      this.lblSendDataTime = new Label();
      this.txtSendDataTime = new TextBox();
      this.lblReceiveDataTime = new Label();
      this.txtReceiveDataTime = new TextBox();
      this.lblState = new Label();
      this.txtState = new TextBox();
      this.label2 = new Label();
      this.lblReceiveDataTimeout = new Label();
      this.txtReceiveDataTimeout = new TextBox();
      this.lblDiagnosticActive = new Label();
      this.txtDeviceDiagActive = new TextBox();
      this.lblName = new Label();
      this.txtName = new TextBox();
      this.listMessageClient = new ListBox();
      this.lblOutputList = new Label();
      this.grpMessageClient.SuspendLayout();
      this.grpProperties.SuspendLayout();
      this.SuspendLayout();
      this.grpMessageClient.Controls.Add((Control) this.listMessageClient);
      this.grpMessageClient.Controls.Add((Control) this.lblObjects);
      this.grpMessageClient.Controls.Add((Control) this.grpProperties);
      this.grpMessageClient.Controls.Add((Control) this.lblOutputList);
      this.grpMessageClient.Location = new Point(0, 0);
      this.grpMessageClient.Name = "grpMessageClient";
      this.grpMessageClient.Size = new Size(512, 352);
      this.grpMessageClient.TabIndex = 4;
      this.grpMessageClient.TabStop = false;
      this.lblObjects.Location = new Point(8, 16);
      this.lblObjects.Name = "lblObjects";
      this.lblObjects.Size = new Size(152, 16);
      this.lblObjects.TabIndex = 7;
      this.lblObjects.Text = "Available MessageClients: 0";
      this.lblObjects.TextAlign = ContentAlignment.BottomLeft;
      this.grpProperties.Controls.Add((Control) this.txtSendDataTime);
      this.grpProperties.Controls.Add((Control) this.txtReceiveDataTime);
      this.grpProperties.Controls.Add((Control) this.label1);
      this.grpProperties.Controls.Add((Control) this.txtID);
      this.grpProperties.Controls.Add((Control) this.lstReceiveData);
      this.grpProperties.Controls.Add((Control) this.lstSendData);
      this.grpProperties.Controls.Add((Control) this.lblState);
      this.grpProperties.Controls.Add((Control) this.txtState);
      this.grpProperties.Controls.Add((Control) this.label2);
      this.grpProperties.Controls.Add((Control) this.lblReceiveDataTimeout);
      this.grpProperties.Controls.Add((Control) this.txtReceiveDataTimeout);
      this.grpProperties.Controls.Add((Control) this.lblDiagnosticActive);
      this.grpProperties.Controls.Add((Control) this.txtDeviceDiagActive);
      this.grpProperties.Controls.Add((Control) this.lblName);
      this.grpProperties.Controls.Add((Control) this.txtName);
      this.grpProperties.Controls.Add((Control) this.lblSendDataTime);
      this.grpProperties.Controls.Add((Control) this.lblReceiveDataTime);
      this.grpProperties.Location = new Point(168, 16);
      this.grpProperties.Name = "grpProperties";
      this.grpProperties.Size = new Size(328, 328);
      this.grpProperties.TabIndex = 5;
      this.grpProperties.TabStop = false;
      this.grpProperties.Text = "Properties";
      this.label1.Location = new Point(5, 86);
      this.label1.Name = "label1";
      this.label1.Size = new Size(24, 18);
      this.label1.TabIndex = 43;
      this.label1.Text = "ID:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.txtID.Location = new Point(36, 88);
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      this.txtID.Size = new Size(48, 20);
      this.txtID.TabIndex = 42;
      this.txtID.Text = "-";
      this.lstReceiveData.Location = new Point(208, 155);
      this.lstReceiveData.Name = "lstReceiveData";
      this.lstReceiveData.SelectionMode = SelectionMode.MultiExtended;
      this.lstReceiveData.Size = new Size(104, 160);
      this.lstReceiveData.TabIndex = 41;
      this.lstSendData.AllowDrop = true;
      this.lstSendData.Location = new Point(48, 155);
      this.lstSendData.Name = "lstSendData";
      this.lstSendData.SelectionMode = SelectionMode.MultiExtended;
      this.lstSendData.Size = new Size(104, 160);
      this.lstSendData.TabIndex = 40;
      this.lblSendDataTime.Location = new Point(8, 112);
      this.lblSendDataTime.Name = "lblSendDataTime";
      this.lblSendDataTime.Size = new Size(136, 18);
      this.lblSendDataTime.TabIndex = 37;
      this.lblSendDataTime.Text = "Send Data Time:";
      this.lblSendDataTime.TextAlign = ContentAlignment.MiddleLeft;
      this.txtSendDataTime.Location = new Point(8, 130);
      this.txtSendDataTime.Name = "txtSendDataTime";
      this.txtSendDataTime.ReadOnly = true;
      this.txtSendDataTime.Size = new Size(144, 20);
      this.txtSendDataTime.TabIndex = 36;
      this.txtSendDataTime.Text = "-";
      this.lblReceiveDataTime.Location = new Point(168, 112);
      this.lblReceiveDataTime.Name = "lblReceiveDataTime";
      this.lblReceiveDataTime.Size = new Size(158, 18);
      this.lblReceiveDataTime.TabIndex = 35;
      this.lblReceiveDataTime.Text = "Estimated Receive Data Time:";
      this.lblReceiveDataTime.TextAlign = ContentAlignment.MiddleLeft;
      this.txtReceiveDataTime.Location = new Point(168, 130);
      this.txtReceiveDataTime.Name = "txtReceiveDataTime";
      this.txtReceiveDataTime.ReadOnly = true;
      this.txtReceiveDataTime.Size = new Size(144, 20);
      this.txtReceiveDataTime.TabIndex = 34;
      this.txtReceiveDataTime.Text = "-";
      this.lblState.Location = new Point(72, 88);
      this.lblState.Name = "lblState";
      this.lblState.Size = new Size(56, 18);
      this.lblState.TabIndex = 33;
      this.lblState.Text = "State:";
      this.lblState.TextAlign = ContentAlignment.MiddleRight;
      this.txtState.Location = new Point(136, 88);
      this.txtState.Name = "txtState";
      this.txtState.ReadOnly = true;
      this.txtState.Size = new Size(176, 20);
      this.txtState.TabIndex = 32;
      this.txtState.Text = "-";
      this.label2.Location = new Point(184, 64);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 18);
      this.label2.TabIndex = 31;
      this.label2.Text = "[s]";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblReceiveDataTimeout.Location = new Point(8, 64);
      this.lblReceiveDataTimeout.Name = "lblReceiveDataTimeout";
      this.lblReceiveDataTimeout.Size = new Size(120, 18);
      this.lblReceiveDataTimeout.TabIndex = 15;
      this.lblReceiveDataTimeout.Text = "Receive Data Timeout:";
      this.lblReceiveDataTimeout.TextAlign = ContentAlignment.MiddleRight;
      this.txtReceiveDataTimeout.Location = new Point(136, 64);
      this.txtReceiveDataTimeout.Name = "txtReceiveDataTimeout";
      this.txtReceiveDataTimeout.ReadOnly = true;
      this.txtReceiveDataTimeout.Size = new Size(48, 20);
      this.txtReceiveDataTimeout.TabIndex = 14;
      this.txtReceiveDataTimeout.Text = "-";
      this.lblDiagnosticActive.Location = new Point(8, 40);
      this.lblDiagnosticActive.Name = "lblDiagnosticActive";
      this.lblDiagnosticActive.Size = new Size(120, 18);
      this.lblDiagnosticActive.TabIndex = 11;
      this.lblDiagnosticActive.Text = "Diagnostic Active:";
      this.lblDiagnosticActive.TextAlign = ContentAlignment.MiddleRight;
      this.txtDeviceDiagActive.Location = new Point(136, 40);
      this.txtDeviceDiagActive.Name = "txtDeviceDiagActive";
      this.txtDeviceDiagActive.ReadOnly = true;
      this.txtDeviceDiagActive.Size = new Size(48, 20);
      this.txtDeviceDiagActive.TabIndex = 10;
      this.txtDeviceDiagActive.Text = "-";
      this.lblName.Location = new Point(8, 16);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(120, 18);
      this.lblName.TabIndex = 1;
      this.lblName.Text = "Name:";
      this.lblName.TextAlign = ContentAlignment.MiddleRight;
      this.txtName.Location = new Point(136, 16);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(176, 20);
      this.txtName.TabIndex = 0;
      this.txtName.Text = "-";
      this.listMessageClient.Location = new Point(8, 51);
      this.listMessageClient.Name = "listMessageClient";
      this.listMessageClient.Size = new Size(152, 290);
      this.listMessageClient.TabIndex = 4;
      this.listMessageClient.SelectedIndexChanged += new EventHandler(this.listController_SelectedIndexChanged);
      this.lblOutputList.Location = new Point(8, 33);
      this.lblOutputList.Name = "lblOutputList";
      this.lblOutputList.Size = new Size(112, 16);
      this.lblOutputList.TabIndex = 6;
      this.lblOutputList.Text = "Message Client List:";
      this.lblOutputList.TextAlign = ContentAlignment.BottomLeft;
      this.Controls.Add((Control) this.grpMessageClient);
      this.MaximumSize = new Size(512, 352);
      this.MinimumSize = new Size(512, 352);
      this.Name = nameof (ctrlMessageClient);
      this.Size = new Size(512, 352);
      this.Load += new EventHandler(this.ctrlMessageClient_Load);
      this.grpMessageClient.ResumeLayout(false);
      this.grpProperties.ResumeLayout(false);
      this.grpProperties.PerformLayout();
      this.ResumeLayout(false);
    }

    private void ctrlMessageClient_Load(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.ControlText))
        return;
      this.ControlText = this.Name;
    }

    public string ControlText
    {
      get
      {
        return this.grpMessageClient.Text;
      }
      set
      {
        this.grpMessageClient.Text = value;
      }
    }

    public void AddObject(MessageClient MessageObject)
    {
      this._varMessageClient.Add(MessageObject);
      int num = this.listMessageClient.Items.Add((object) MessageObject.Name);
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varMessageClient.Count.ToString());
      if (num != 0)
        return;
      this._myMessageClient = this._varMessageClient[0];
      this.listMessageClient.SelectedIndex = 0;
      this.ShowData();
    }

    public void ClearObject()
    {
      this._varMessageClient.Clear();
      this.listMessageClient.Items.Clear();
      this.lblObjects.Text = string.Format("Available Objects: {0}", (object) this._varMessageClient.Count.ToString());
      this.ShowData();
    }

    private void listController_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listMessageClient.SelectedIndex <= -1)
        return;
      this._myMessageClient = this._varMessageClient[this.listMessageClient.SelectedIndex];
      this.lstSendData.Items.Clear();
      this._lastSendDataLength = 0;
      this.lstReceiveData.Items.Clear();
      this._lastReceiveDataLength = 0;
      this.ShowProperties();
    }

    private void ShowProperties()
    {
      if (this._myMessageClient != null)
      {
        this.grpMessageClient.Enabled = true;
        try
        {
          this.txtName.Text = this._myMessageClient.Name;
          this.txtDeviceDiagActive.Text = this._myMessageClient.ActivateDiagnostic.ToString();
          TextBox receiveDataTimeout = this.txtReceiveDataTimeout;
          int num = this._myMessageClient.Timeout;
          string str1 = num.ToString();
          receiveDataTimeout.Text = str1;
          TextBox txtId = this.txtID;
          num = this._myMessageClient.ControllerId;
          string str2 = num.ToString();
          txtId.Text = str2;
          this.txtState.Text = this._myMessageClient.State.ToString();
          TextBox txtSendDataTime = this.txtSendDataTime;
          DateTime dateTime = this._myMessageClient.SendDataTime;
          string str3 = dateTime.ToString();
          txtSendDataTime.Text = str3;
          TextBox txtReceiveDataTime = this.txtReceiveDataTime;
          dateTime = this._myMessageClient.EstimatedReceiveDataTime;
          string str4 = dateTime.ToString();
          txtReceiveDataTime.Text = str4;
          this.WriteSendData();
          this.WriteReceiveData();
        }
        catch
        {
        }
      }
      else
      {
        this.txtName.Text = "-";
        this.txtDeviceDiagActive.Text = "-";
        this.txtReceiveDataTimeout.Text = "-";
        this.txtID.Text = "-";
        this.txtState.Text = "-";
        this.txtSendDataTime.Text = "-";
        this.txtReceiveDataTime.Text = "-";
        this.grpMessageClient.Enabled = false;
      }
    }

    private void WriteSendData()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._myMessageClient == null)
      {
        this.lstReceiveData.Items.Clear();
      }
      else
      {
        byte[] sendData = this._myMessageClient.GetSendData();
        if (sendData == null)
        {
          this.lstSendData.Items.Clear();
        }
        else
        {
          try
          {
            if (sendData.Length == this._lastSendDataLength && !(this._myMessageClient.SendDataTime != this._lastSendDataTime))
              return;
            this._lastSendDataLength = sendData.Length;
            this._lastSendDataTime = this._myMessageClient.SendDataTime;
            this.lstSendData.Items.Clear();
            this.lstSendData.Items.AddRange((object[]) string.Format((IFormatProvider) new BinaryFormatter(), "{0:H,W}", (object) sendData).Split(' '));
          }
          catch
          {
          }
        }
      }
    }

    private void WriteReceiveData()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._myMessageClient == null)
      {
        this.lstReceiveData.Items.Clear();
      }
      else
      {
        byte[] receiveRequest = this._myMessageClient.GetReceiveRequest();
        if (receiveRequest == null)
        {
          this.lstReceiveData.Items.Clear();
        }
        else
        {
          try
          {
            if (receiveRequest.Length == this._lastReceiveDataLength && !(this._myMessageClient.EstimatedReceiveDataTime != this._lastReceiveDataTime))
              return;
            this._lastReceiveDataLength = receiveRequest.Length;
            this._lastReceiveDataTime = this._myMessageClient.EstimatedReceiveDataTime;
            this.lstReceiveData.Items.Clear();
            this.lstReceiveData.Items.AddRange((object[]) string.Format((IFormatProvider) new BinaryFormatter(), "{0:H,W}", (object) receiveRequest).Split(' '));
          }
          catch
          {
          }
        }
      }
    }

    private void ShowData()
    {
      this.ShowProperties();
      this.WriteSendData();
      this.WriteReceiveData();
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
      if (this._myMessageClient == null)
        return;
      try
      {
        this.txtState.Text = this._myMessageClient.State.ToString();
        this.txtSendDataTime.Text = this._myMessageClient.SendDataTime.ToString();
        this.txtReceiveDataTime.Text = this._myMessageClient.EstimatedReceiveDataTime.ToString();
      }
      catch
      {
      }
      this.WriteSendData();
      this.WriteReceiveData();
    }
  }
}
