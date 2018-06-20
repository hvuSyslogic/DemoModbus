// Decompiled with JetBrains decompiler
// Type: EasyModbus.EasyModbus2Mqtt
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace EasyModbus
{
  public class EasyModbus2Mqtt
  {
    private ModbusClient modbusClient = new ModbusClient();
    private List<ReadOrder> readOrders = new List<ReadOrder>();
    private string mqttBrokerAddress = "www.mqtt-dashboard.com";
    private int mqttBrokerPort = 1883;
    private string mqttRootTopic = "easymodbusclient";
    private string MqttBrokerAddressPublish = "";
    private object lockProcessData = new object();
    private MqttClient mqttClient;
    private volatile bool shouldStop;

    public bool AutomaticReconnect { get; set; } = true;

    public string MqttUserName { get; set; }

    public string MqttPassword { get; set; }

    public bool RetainMessages { get; set; }

    public void AddReadOrder(ReadOrder readOrder)
    {
      if (readOrder.FunctionCode == (FunctionCode) 0)
        throw new ArgumentOutOfRangeException("FunctionCode must be initialized");
      if (readOrder.Quantity == 0)
        throw new ArgumentOutOfRangeException("Quantity cannot be 0");
      if (readOrder.Topic != null && readOrder.Topic.Length != readOrder.Quantity)
        throw new ArgumentOutOfRangeException("Size of the Topic array must mach with quantity");
      if (readOrder.Retain != null && readOrder.Retain.Length != readOrder.Quantity)
        throw new ArgumentOutOfRangeException("Size of the Retain array must mach with quantity");
      if (readOrder.Hysteresis != null && readOrder.Hysteresis.Length != readOrder.Quantity)
        throw new ArgumentOutOfRangeException("Size of the Hysteresis array must mach with quantity");
      if (readOrder.Scale != null && readOrder.Scale.Length != readOrder.Quantity)
        throw new ArgumentOutOfRangeException("Size of the Scale array must mach with quantity");
      if (readOrder.Retain != null && readOrder.Retain.Length != readOrder.Quantity)
        throw new ArgumentOutOfRangeException("Size of the Retain array must mach with quantity");
      if (readOrder.CylceTime == 0)
        readOrder.CylceTime = 500;
      if (readOrder.Topic == null)
      {
        readOrder.Topic = new string[readOrder.Quantity];
        int index = 0;
        while (index < readOrder.Quantity)
        {
          if (readOrder.FunctionCode == FunctionCode.ReadCoils)
            readOrder.Topic[index] = this.mqttRootTopic + "/coils/" + checked (index + readOrder.StartingAddress).ToString();
          if (readOrder.FunctionCode == FunctionCode.ReadDiscreteInputs)
            readOrder.Topic[index] = this.mqttRootTopic + "/discreteinputs/" + checked (index + readOrder.StartingAddress).ToString();
          if (readOrder.FunctionCode == FunctionCode.ReadHoldingRegisters)
            readOrder.Topic[index] = this.mqttRootTopic + "/holdingregisters/" + checked (index + readOrder.StartingAddress).ToString();
          if (readOrder.FunctionCode == FunctionCode.ReadInputRegisters)
            readOrder.Topic[index] = this.mqttRootTopic + "/inputregisters/" + checked (index + readOrder.StartingAddress).ToString();
          checked { ++index; }
        }
      }
      readOrder.oldvalue = new object[readOrder.Quantity];
      this.readOrders.Add(readOrder);
    }

    public void AddReadOrder(FunctionCode functionCode, int quantity, int startingAddress, int cycleTime)
    {
      ReadOrder readOrder = new ReadOrder();
      readOrder.CylceTime = cycleTime;
      readOrder.FunctionCode = functionCode;
      readOrder.Quantity = quantity;
      readOrder.StartingAddress = startingAddress;
      readOrder.Topic = new string[quantity];
      readOrder.Retain = new bool[quantity];
      readOrder.oldvalue = new object[quantity];
      int index = 0;
      while (index < quantity)
      {
        if (functionCode == FunctionCode.ReadCoils)
          readOrder.Topic[index] = this.mqttRootTopic + "/coils/" + checked (index + readOrder.StartingAddress).ToString();
        if (functionCode == FunctionCode.ReadDiscreteInputs)
          readOrder.Topic[index] = this.mqttRootTopic + "/discreteinputs/" + checked (index + readOrder.StartingAddress).ToString();
        if (functionCode == FunctionCode.ReadHoldingRegisters)
          readOrder.Topic[index] = this.mqttRootTopic + "/holdingregisters/" + checked (index + readOrder.StartingAddress).ToString();
        if (functionCode == FunctionCode.ReadInputRegisters)
          readOrder.Topic[index] = this.mqttRootTopic + "/inputregisters/" + checked (index + readOrder.StartingAddress).ToString();
        checked { ++index; }
      }
      this.readOrders.Add(readOrder);
    }

    public void AddReadOrder(FunctionCode functionCode, int quantity, int startingAddress, int cycleTime, string[] topic)
    {
      this.AddReadOrder(new ReadOrder()
      {
        FunctionCode = functionCode,
        Quantity = quantity,
        StartingAddress = startingAddress,
        CylceTime = cycleTime,
        Topic = topic,
        Retain = new bool[quantity]
      });
    }

    public void start()
    {
      this.shouldStop = false;
      if (this.mqttBrokerAddress == null)
        throw new ArgumentOutOfRangeException("Mqtt Broker Address not initialized");
      this.mqttClient = new MqttClient(this.mqttBrokerAddress, this.mqttBrokerPort, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None);
      string clientId = new Guid().ToString();
      try
      {
        if (this.MqttUserName == null || this.MqttPassword == null)
        {
          int num1 = (int) this.mqttClient.Connect(clientId);
        }
        else
        {
          int num2 = (int) this.mqttClient.Connect(clientId, this.MqttUserName, this.MqttPassword);
        }
        if (!this.modbusClient.Connected)
          this.modbusClient.Connect();
      }
      catch (Exception ex)
      {
        if (!this.AutomaticReconnect)
          throw ex;
      }
      int index = 0;
      while (index < this.readOrders.Count)
      {
        this.readOrders[index].thread = new Thread(new ParameterizedThreadStart(this.ProcessData));
        this.readOrders[index].thread.Start((object) this.readOrders[index]);
        checked { ++index; }
      }
    }

    public void publish(string[] topic, string[] payload, string mqttBrokerAddress)
    {
      if (this.mqttClient != null && !mqttBrokerAddress.Equals(this.MqttBrokerAddressPublish) & this.mqttClient.IsConnected)
        this.mqttClient.Disconnect();
      if (topic.Length != payload.Length)
        throw new ArgumentOutOfRangeException("Array topic and payload must be the same size");
      this.mqttClient = new MqttClient(mqttBrokerAddress, this.mqttBrokerPort, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None);
      string clientId = Guid.NewGuid().ToString();
      if (!this.mqttClient.IsConnected)
      {
        if (this.MqttUserName == null || this.MqttPassword == null)
        {
          int num1 = (int) this.mqttClient.Connect(clientId);
        }
        else
        {
          int num2 = (int) this.mqttClient.Connect(clientId, this.MqttUserName, this.MqttPassword);
        }
      }
      int index = 0;
      while (index < payload.Length)
      {
        int num3 = (int) this.mqttClient.Publish(topic[index], Encoding.UTF8.GetBytes(payload[index]), (byte) 2, this.RetainMessages);
        checked { ++index; }
      }
    }

    public void publish(string topic, string payload, string mqttBrokerAddress)
    {
      if (this.mqttClient != null && !mqttBrokerAddress.Equals(this.MqttBrokerAddressPublish) & this.mqttClient.IsConnected)
        this.mqttClient.Disconnect();
      this.mqttClient = new MqttClient(mqttBrokerAddress, this.mqttBrokerPort, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None);
      string clientId = Guid.NewGuid().ToString();
      if (!this.mqttClient.IsConnected)
      {
        if (this.MqttUserName == null || this.MqttPassword == null)
        {
          int num1 = (int) this.mqttClient.Connect(clientId);
        }
        else
        {
          int num2 = (int) this.mqttClient.Connect(clientId, this.MqttUserName, this.MqttPassword);
        }
      }
      if (payload != null)
      {
        int num3 = (int) this.mqttClient.Publish(topic, Encoding.UTF8.GetBytes(payload), (byte) 2, this.RetainMessages);
      }
      else
      {
        int num4 = (int) this.mqttClient.Publish(topic, new byte[0], (byte) 0, this.RetainMessages);
      }
    }

    public void Disconnect()
    {
      this.mqttClient.Disconnect();
    }

    public void stop()
    {
      this.modbusClient.Disconnect();
      this.mqttClient.Disconnect();
      this.shouldStop = true;
    }

    private void ProcessData(object param)
    {
      while (!this.shouldStop)
      {
        try
        {
          if (!this.mqttClient.IsConnected)
          {
            this.mqttClient = new MqttClient(this.mqttBrokerAddress, this.mqttBrokerPort, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None);
            string clientId = Guid.NewGuid().ToString();
            if (this.MqttUserName == null || this.MqttPassword == null)
            {
              int num1 = (int) this.mqttClient.Connect(clientId);
            }
            else
            {
              int num2 = (int) this.mqttClient.Connect(clientId, this.MqttUserName, this.MqttPassword);
            }
          }
        }
        catch (Exception ex)
        {
          if (!this.AutomaticReconnect)
            throw ex;
        }
        ReadOrder readOrder = (ReadOrder) param;
        lock (this.lockProcessData)
        {
          try
          {
            if (readOrder.FunctionCode == FunctionCode.ReadCoils)
            {
              bool[] flagArray = this.modbusClient.ReadCoils(readOrder.StartingAddress, readOrder.Quantity);
              int index = 0;
              while (index < flagArray.Length)
              {
                if (readOrder.oldvalue[index] == null)
                {
                  int num1 = (int) this.mqttClient.Publish(readOrder.Topic[index], Encoding.UTF8.GetBytes(flagArray[index].ToString()), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                }
                else if ((bool) readOrder.oldvalue[index] != flagArray[index])
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], Encoding.UTF8.GetBytes(flagArray[index].ToString()), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                }
                readOrder.oldvalue[index] = (object) flagArray[index];
                checked { ++index; }
              }
            }
            if (readOrder.FunctionCode == FunctionCode.ReadDiscreteInputs)
            {
              bool[] flagArray = this.modbusClient.ReadDiscreteInputs(readOrder.StartingAddress, readOrder.Quantity);
              int index = 0;
              while (index < flagArray.Length)
              {
                if (readOrder.oldvalue[index] == null)
                {
                  int num1 = (int) this.mqttClient.Publish(readOrder.Topic[index], Encoding.UTF8.GetBytes(flagArray[index].ToString()), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                }
                else if ((bool) readOrder.oldvalue[index] != flagArray[index])
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], Encoding.UTF8.GetBytes(flagArray[index].ToString()), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                }
                readOrder.oldvalue[index] = (object) flagArray[index];
                checked { ++index; }
              }
            }
            if (readOrder.FunctionCode == FunctionCode.ReadHoldingRegisters)
            {
              int[] numArray = this.modbusClient.ReadHoldingRegisters(readOrder.StartingAddress, readOrder.Quantity);
              int index = 0;
              while (index < numArray.Length)
              {
                float num1 = readOrder.Scale != null ? ((double) readOrder.Scale[index] == 0.0 ? 1f : readOrder.Scale[index]) : 1f;
                if (readOrder.oldvalue[index] == null)
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], readOrder.Unit == null ? Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString()) : Encoding.UTF8.GetBytes(((double) numArray[index] * (double) num1).ToString() + " " + readOrder.Unit[index]), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                  readOrder.oldvalue[index] = (object) numArray[index];
                }
                else if ((int) readOrder.oldvalue[index] != numArray[index] && (readOrder.Hysteresis == null || numArray[index] < checked ((int) readOrder.oldvalue[index] - readOrder.Hysteresis[index]) | numArray[index] > checked ((int) readOrder.oldvalue[index] + readOrder.Hysteresis[index])))
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], readOrder.Unit == null ? Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString()) : Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString() + " " + readOrder.Unit[index]), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                  readOrder.oldvalue[index] = (object) numArray[index];
                }
                checked { ++index; }
              }
            }
            if (readOrder.FunctionCode == FunctionCode.ReadInputRegisters)
            {
              int[] numArray = this.modbusClient.ReadInputRegisters(readOrder.StartingAddress, readOrder.Quantity);
              int index = 0;
              while (index < numArray.Length)
              {
                float num1 = readOrder.Scale != null ? ((double) readOrder.Scale[index] == 0.0 ? 1f : readOrder.Scale[index]) : 1f;
                if (readOrder.oldvalue[index] == null)
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], readOrder.Unit == null ? Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString()) : Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString() + " " + readOrder.Unit[index]), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                  readOrder.oldvalue[index] = (object) numArray[index];
                }
                else if ((int) readOrder.oldvalue[index] != numArray[index] && (readOrder.Hysteresis == null || numArray[index] < checked ((int) readOrder.oldvalue[index] - readOrder.Hysteresis[index]) | numArray[index] > checked ((int) readOrder.oldvalue[index] + readOrder.Hysteresis[index])))
                {
                  int num2 = (int) this.mqttClient.Publish(readOrder.Topic[index], readOrder.Unit == null ? Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString()) : Encoding.UTF8.GetBytes(((float) numArray[index] * num1).ToString() + " " + readOrder.Unit[index]), (byte) 2, readOrder.Retain != null && readOrder.Retain[index]);
                  readOrder.oldvalue[index] = (object) numArray[index];
                }
                checked { ++index; }
              }
            }
          }
          catch (Exception ex1)
          {
            this.modbusClient.Disconnect();
            Thread.Sleep(2000);
            if (!this.AutomaticReconnect)
              throw ex1;
            Debug.WriteLine(ex1.StackTrace);
            if (!this.modbusClient.Connected)
            {
              try
              {
                this.modbusClient.Connect();
              }
              catch (Exception ex2)
              {
              }
            }
          }
        }
        Thread.Sleep(readOrder.CylceTime);
      }
    }

    public string MqttBrokerAddress
    {
      get
      {
        return this.mqttBrokerAddress;
      }
      set
      {
        this.mqttBrokerAddress = value;
      }
    }

    public int MqttBrokerPort
    {
      get
      {
        return this.mqttBrokerPort;
      }
      set
      {
        this.mqttBrokerPort = value;
      }
    }

    public string MqttRootTopic
    {
      get
      {
        return this.mqttRootTopic;
      }
      set
      {
        this.mqttRootTopic = value;
      }
    }

    public string IPAddress
    {
      get
      {
        return this.modbusClient.IPAddress;
      }
      set
      {
        this.modbusClient.IPAddress = value;
      }
    }

    public string ModbusIPAddress
    {
      get
      {
        return this.modbusClient.IPAddress;
      }
      set
      {
        this.modbusClient.IPAddress = value;
      }
    }

    public int Port
    {
      get
      {
        return this.modbusClient.Port;
      }
      set
      {
        this.modbusClient.Port = value;
      }
    }

    public int ModbusPort
    {
      get
      {
        return this.modbusClient.Port;
      }
      set
      {
        this.modbusClient.Port = value;
      }
    }

    public byte UnitIdentifier
    {
      get
      {
        return this.modbusClient.UnitIdentifier;
      }
      set
      {
        this.modbusClient.UnitIdentifier = value;
      }
    }

    public int Baudrate
    {
      get
      {
        return this.modbusClient.Baudrate;
      }
      set
      {
        this.modbusClient.Baudrate = value;
      }
    }

    public Parity Parity
    {
      get
      {
        if (this.modbusClient.SerialPort != null)
          return this.modbusClient.Parity;
        return Parity.Even;
      }
      set
      {
        if (this.modbusClient.SerialPort == null)
          return;
        this.modbusClient.Parity = value;
      }
    }

    public StopBits StopBits
    {
      get
      {
        if (this.modbusClient.SerialPort != null)
          return this.modbusClient.StopBits;
        return StopBits.One;
      }
      set
      {
        if (this.modbusClient.SerialPort == null)
          return;
        this.modbusClient.StopBits = value;
      }
    }

    public int ConnectionTimeout
    {
      get
      {
        return this.modbusClient.ConnectionTimeout;
      }
      set
      {
        this.modbusClient.ConnectionTimeout = value;
      }
    }

    public string SerialPort
    {
      get
      {
        return this.modbusClient.SerialPort;
      }
      set
      {
        this.SerialPort = value;
      }
    }
  }
}
