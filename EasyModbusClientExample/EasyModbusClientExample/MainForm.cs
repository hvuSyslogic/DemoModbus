// Decompiled with JetBrains decompiler
// Type: EasyModbusClientExample.MainForm
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using EasyModbus;
using EasyModbusClientExample.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace EasyModbusClientExample
{
  public partial class MainForm : Form
  {
    private string receiveData = (string) null;
    private string sendData = (string) null;
    private bool listBoxPrepareCoils = false;
    private bool listBoxPrepareRegisters = false;
    private ModbusClient modbusClient;
    
    public MainForm()
    {
      this.InitializeComponent();
      this.modbusClient = new ModbusClient();
      this.modbusClient.LogFileFilename = "hvuModbusClient.txt";
      this.modbusClient.ReceiveDataChanged += new ModbusClient.ReceiveDataChangedHandler(this.UpdateReceiveData);
      this.modbusClient.SendDataChanged += new ModbusClient.SendDataChangedHandler(this.UpdateSendData);
      this.modbusClient.ConnectedChanged += new ModbusClient.ConnectedChangedHandler(this.UpdateConnectedChanged);
    }

    private void UpdateReceiveData(object sender)
    {
      this.receiveData = "Rx: " + BitConverter.ToString(this.modbusClient.receiveData).Replace("-", " ") + Environment.NewLine;
      new Thread(new ThreadStart(this.updateReceiveTextBox)).Start();
    }

    private void updateReceiveTextBox()
    {
      if (this.textBox1.InvokeRequired)
        this.Invoke((Delegate) new MainForm.UpdateReceiveDataCallback(this.updateReceiveTextBox), new object[0]);
      else
        this.textBox1.AppendText(this.receiveData);
    }

    private void UpdateSendData(object sender)
    {
      this.sendData = "Tx: " + BitConverter.ToString(this.modbusClient.sendData).Replace("-", " ") + Environment.NewLine;
      new Thread(new ThreadStart(this.updateSendTextBox)).Start();
    }

    private void updateSendTextBox()
    {
      if (this.textBox1.InvokeRequired)
        this.Invoke((Delegate) new MainForm.UpdateReceiveDataCallback(this.updateSendTextBox), new object[0]);
      else
        this.textBox1.AppendText(this.sendData);
    }

    private void BtnConnectClick(object sender, EventArgs e)
    {
      this.modbusClient.IPAddress = this.txtIpAddressInput.Text;
      this.modbusClient.Port = int.Parse(this.txtPortInput.Text);
      this.modbusClient.Connect();
    }

    private void BtnReadCoilsClick(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        bool[] flagArray = this.modbusClient.ReadCoils(checked (int.Parse(this.txtStartingAddressInput.Text) - 1), int.Parse(this.txtNumberOfValuesInput.Text));
        this.lsbAnswerFromServer.Items.Clear();
        int index = 0;
        while (index < flagArray.Length)
        {
          this.lsbAnswerFromServer.Items.Add((object) flagArray[index]);
          checked { ++index; }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadDiscreteInputs_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        bool[] flagArray = this.modbusClient.ReadDiscreteInputs(checked (int.Parse(this.txtStartingAddressInput.Text) - 1), int.Parse(this.txtNumberOfValuesInput.Text));
        this.lsbAnswerFromServer.Items.Clear();
        int index = 0;
        while (index < flagArray.Length)
        {
          this.lsbAnswerFromServer.Items.Add((object) flagArray[index]);
          checked { ++index; }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadHoldingRegisters_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        int[] numArray = this.modbusClient.ReadHoldingRegisters(checked (int.Parse(this.txtStartingAddressInput.Text) - 1), int.Parse(this.txtNumberOfValuesInput.Text));
        this.lsbAnswerFromServer.Items.Clear();
        int index = 0;
        while (index < numArray.Length)
        {
          this.lsbAnswerFromServer.Items.Add((object) numArray[index]);
          checked { ++index; }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadInputRegisters_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        int[] numArray = this.modbusClient.ReadInputRegisters(checked (int.Parse(this.txtStartingAddressInput.Text) - 1), int.Parse(this.txtNumberOfValuesInput.Text));
        this.lsbAnswerFromServer.Items.Clear();
        int index = 0;
        while (index < numArray.Length)
        {
          this.lsbAnswerFromServer.Items.Add((object) numArray[index]);
          checked { ++index; }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.EasyModbusTCP.net");
    }

    private void cbbSelctionModbus_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.modbusClient.Connected)
        this.modbusClient.Disconnect();
      if (this.cbbSelctionModbus.SelectedIndex == 0)
      {
        this.txtIpAddress.Visible = true;
        this.txtIpAddressInput.Visible = true;
        this.txtPort.Visible = true;
        this.txtPortInput.Visible = true;
        this.txtCOMPort.Visible = false;
        this.cbbSelectComPort.Visible = false;
        this.txtSlaveAddress.Visible = false;
        this.txtSlaveAddressInput.Visible = false;
        this.lblBaudrate.Visible = false;
        this.lblParity.Visible = false;
        this.lblStopbits.Visible = false;
        this.txtBaudrate.Visible = false;
        this.cbbParity.Visible = false;
        this.cbbStopbits.Visible = false;
      }
      if (this.cbbSelctionModbus.SelectedIndex != 1)
        return;
      this.cbbSelectComPort.SelectedIndex = 0;
      this.cbbParity.SelectedIndex = 0;
      this.cbbStopbits.SelectedIndex = 0;
      if (this.cbbSelectComPort.SelectedText == "")
        this.cbbSelectComPort.SelectedItem.ToString();
      this.txtIpAddress.Visible = false;
      this.txtIpAddressInput.Visible = false;
      this.txtPort.Visible = false;
      this.txtPortInput.Visible = false;
      this.txtCOMPort.Visible = true;
      this.cbbSelectComPort.Visible = true;
      this.txtSlaveAddress.Visible = true;
      this.txtSlaveAddressInput.Visible = true;
      this.lblBaudrate.Visible = true;
      this.lblParity.Visible = true;
      this.lblStopbits.Visible = true;
      this.txtBaudrate.Visible = true;
      this.cbbParity.Visible = true;
      this.cbbStopbits.Visible = true;
    }

    private void cbbSelectComPort_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.modbusClient.Connected)
        this.modbusClient.Disconnect();
      this.modbusClient.SerialPort = this.cbbSelectComPort.SelectedItem.ToString();
      this.modbusClient.UnitIdentifier = byte.Parse(this.txtSlaveAddressInput.Text);
    }

    private void TxtSlaveAddressInputTextChanged(object sender, EventArgs e)
    {
      try
      {
        this.modbusClient.UnitIdentifier = byte.Parse(this.txtSlaveAddressInput.Text);
      }
      catch (FormatException ex)
      {
      }
    }

    private void btnPrepareCoils_Click(object sender, EventArgs e)
    {
      if (!this.listBoxPrepareCoils)
        this.lsbAnswerFromServer.Items.Clear();
      this.listBoxPrepareCoils = true;
      this.listBoxPrepareRegisters = false;
      this.lsbWriteToServer.Items.Add((object) this.txtCoilValue.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (!this.listBoxPrepareRegisters)
        this.lsbAnswerFromServer.Items.Clear();
      this.listBoxPrepareRegisters = true;
      this.listBoxPrepareCoils = false;
      this.lsbWriteToServer.Items.Add((object) int.Parse(this.txtRegisterValue.Text));
    }

    private void btnWriteSingleCoil_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        this.modbusClient.WriteSingleCoil(checked (int.Parse(this.txtStartingAddressOutput.Text) - 1), bool.Parse(this.lsbWriteToServer.Items[0].ToString()));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception writing values to Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteSingleRegister_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        this.modbusClient.WriteSingleRegister(checked (int.Parse(this.txtStartingAddressOutput.Text) - 1), int.Parse(this.lsbWriteToServer.Items[0].ToString()));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception writing values to Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteMultipleCoils_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        bool[] values = new bool[this.lsbWriteToServer.Items.Count];
        int index = 0;
        while (index < this.lsbWriteToServer.Items.Count)
        {
          values[index] = bool.Parse(this.lsbWriteToServer.Items[index].ToString());
          checked { ++index; }
        }
        this.modbusClient.WriteMultipleCoils(checked (int.Parse(this.txtStartingAddressOutput.Text) - 1), values);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception writing values to Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteMultipleRegisters_Click(object sender, EventArgs e)
    {
      try
      {
        if (!this.modbusClient.Connected)
          this.button3_Click((object) null, (EventArgs) null);
        int[] values = new int[this.lsbWriteToServer.Items.Count];
        int index = 0;
        while (index < this.lsbWriteToServer.Items.Count)
        {
          values[index] = int.Parse(this.lsbWriteToServer.Items[index].ToString());
          checked { ++index; }
        }
        this.modbusClient.WriteMultipleRegisters(checked (int.Parse(this.txtStartingAddressOutput.Text) - 1), values);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Exception writing values to Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void lsbAnswerFromServer_DoubleClick(object sender, EventArgs e)
    {
      int selectedIndex = this.lsbAnswerFromServer.SelectedIndex;
    }

    private void txtCoilValue_DoubleClick(object sender, EventArgs e)
    {
      if (this.txtCoilValue.Text.Equals("FALSE"))
        this.txtCoilValue.Text = "TRUE";
      else
        this.txtCoilValue.Text = "FALSE";
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.lsbWriteToServer.Items.Clear();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.lsbWriteToServer.SelectedIndex;
      if (selectedIndex < 0)
        return;
      this.lsbWriteToServer.Items.RemoveAt(selectedIndex);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtRegisterValue_TextChanged(object sender, EventArgs e)
    {
    }

    private void button3_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.modbusClient.Connected)
          this.modbusClient.Disconnect();
        if (this.cbbSelctionModbus.SelectedIndex == 0)
        {
          this.modbusClient.IPAddress = this.txtIpAddressInput.Text;
          this.modbusClient.Port = int.Parse(this.txtPortInput.Text);
          this.modbusClient.SerialPort = (string) null;
          this.modbusClient.Connect();
        }
        if (this.cbbSelctionModbus.SelectedIndex != 1)
          return;
        this.modbusClient.SerialPort = this.cbbSelectComPort.SelectedItem.ToString();
        this.modbusClient.UnitIdentifier = byte.Parse(this.txtSlaveAddressInput.Text);
        this.modbusClient.Baudrate = int.Parse(this.txtBaudrate.Text);
        if (this.cbbParity.SelectedIndex == 0)
          this.modbusClient.Parity = Parity.Even;
        if (this.cbbParity.SelectedIndex == 1)
          this.modbusClient.Parity = Parity.Odd;
        if (this.cbbParity.SelectedIndex == 2)
          this.modbusClient.Parity = Parity.None;
        if (this.cbbStopbits.SelectedIndex == 0)
          this.modbusClient.StopBits = StopBits.One;
        if (this.cbbStopbits.SelectedIndex == 1)
          this.modbusClient.StopBits = StopBits.OnePointFive;
        if (this.cbbStopbits.SelectedIndex == 2)
          this.modbusClient.StopBits = StopBits.Two;
        this.modbusClient.Connect();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Unable to connect to Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void UpdateConnectedChanged(object sender)
    {
      if (this.modbusClient.Connected)
      {
        this.txtConnectedStatus.Text = "Connected to Server";
        this.txtConnectedStatus.BackColor = Color.Green;
      }
      else
      {
        this.txtConnectedStatus.Text = "Not Connected to Server";
        this.txtConnectedStatus.BackColor = Color.Red;
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.modbusClient.Disconnect();
    }

    private void txtBaudrate_TextChanged(object sender, EventArgs e)
    {
      if (this.modbusClient.Connected)
        this.modbusClient.Disconnect();
      this.modbusClient.Baudrate = int.Parse(this.txtBaudrate.Text);
    }

     private delegate void UpdateReceiveDataCallback();
  }
}
