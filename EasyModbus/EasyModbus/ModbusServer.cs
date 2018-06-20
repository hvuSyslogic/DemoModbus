// Decompiled with JetBrains decompiler
// Type: EasyModbus.ModbusServer
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using EasyModbus.Exceptions;
using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasyModbus
{
  public class ModbusServer
  {
    private bool debug = false;
    private int port = 502;
    private ModbusProtocol sendData = new ModbusProtocol();
    private byte[] bytes = new byte[2100];
    private int numberOfConnections = 0;
    private int baudrate = 9600;
    private Parity parity = Parity.Even;
    private StopBits stopBits = StopBits.One;
    private string serialPort = "COM1";
    private byte unitIdentifier = 1;
    private ModbusProtocol[] modbusLogData = new ModbusProtocol[100];
    private object lockCoils = new object();
    private object lockHoldingRegisters = new object();
    internal object lockMQTT = new object();
    internal EasyModbus2Mqtt easyModbus2Mqtt = new EasyModbus2Mqtt();
    private bool dataReceived = false;
    private byte[] readBuffer = new byte[2094];
    private int nextSign = 0;
    private object lockProcessReceivedData = new object();
    private ModbusProtocol receiveData;
    public HoldingRegisters holdingRegisters;
    public InputRegisters inputRegisters;
    public Coils coils;
    public DiscreteInputs discreteInputs;
    private bool udpFlag;
    private bool serialFlag;
    private System.IO.Ports.SerialPort serialport;
    private int portIn;
    private IPAddress ipAddressIn;
    private UdpClient udpClient;
    private IPEndPoint iPEndPoint;
    private TCPHandler tcpHandler;
    private Thread listenerThread;
    private Thread clientConnectionThread;
    private volatile bool shouldStop;
    private DateTime lastReceive;

    public bool FunctionCode1Disabled { get; set; }

    public bool FunctionCode2Disabled { get; set; }

    public bool FunctionCode3Disabled { get; set; }

    public bool FunctionCode4Disabled { get; set; }

    public bool FunctionCode5Disabled { get; set; }

    public bool FunctionCode6Disabled { get; set; }

    public bool FunctionCode15Disabled { get; set; }

    public bool FunctionCode16Disabled { get; set; }

    public bool FunctionCode23Disabled { get; set; }

    public bool PortChanged { get; set; }

    public ModbusServer()
    {
      this.easyModbus2Mqtt.MqttRootTopic = "easymodbusserver";
      this.easyModbus2Mqtt.RetainMessages = true;
      this.holdingRegisters = new HoldingRegisters(this);
      this.inputRegisters = new InputRegisters(this);
      this.coils = new Coils(this);
      this.discreteInputs = new DiscreteInputs(this);
      this.easyModbus2Mqtt.MqttBrokerAddress = (string) null;
    }

    public event ModbusServer.CoilsChangedHandler CoilsChanged;

    public event ModbusServer.HoldingRegistersChangedHandler HoldingRegistersChanged;

    public event ModbusServer.NumberOfConnectedClientsChangedHandler NumberOfConnectedClientsChanged;

    public event ModbusServer.LogDataChangedHandler LogDataChanged;

    public void Listen()
    {
      this.listenerThread = new Thread(new ThreadStart(this.ListenerThread));
      this.listenerThread.Start();
    }

    public void StopListening()
    {
      if (this.SerialFlag & this.serialport != null)
      {
        if (this.serialport.IsOpen)
          this.serialport.Close();
        this.shouldStop = true;
      }
      try
      {
        this.tcpHandler.Disconnect();
        this.listenerThread.Abort();
      }
      catch (Exception ex)
      {
      }
      this.listenerThread.Join();
      try
      {
        this.clientConnectionThread.Abort();
      }
      catch (Exception ex)
      {
      }
    }

    private void ListenerThread()
    {
      if (!this.udpFlag & !this.serialFlag)
      {
        if (this.udpClient != null)
        {
          try
          {
            this.udpClient.Close();
          }
          catch (Exception ex)
          {
          }
        }
        this.tcpHandler = new TCPHandler(this.port);
        if (this.debug)
          StoreLogData.Instance.Store("EasyModbus Server listing for incomming data at Port " + (object) this.port, DateTime.Now);
        this.tcpHandler.dataChanged += new TCPHandler.DataChanged(this.ProcessReceivedData);
        this.tcpHandler.numberOfClientsChanged += new TCPHandler.NumberOfClientsChanged(this.numberOfClientsChanged);
      }
      else if (this.serialFlag)
      {
        if (this.serialport != null)
          return;
        if (this.debug)
          StoreLogData.Instance.Store("EasyModbus RTU-Server listing for incomming data at Serial Port " + this.serialPort, DateTime.Now);
        this.serialport = new System.IO.Ports.SerialPort();
        this.serialport.PortName = this.serialPort;
        this.serialport.BaudRate = this.baudrate;
        this.serialport.Parity = this.parity;
        this.serialport.StopBits = this.stopBits;
        this.serialport.WriteTimeout = 10000;
        this.serialport.ReadTimeout = 1000;
        this.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
        this.serialport.Open();
      }
      else
      {
        while (!this.shouldStop)
        {
          if (this.udpFlag)
          {
            if (this.udpClient == null | this.PortChanged)
            {
              this.udpClient = new UdpClient(this.port);
              if (this.debug)
                StoreLogData.Instance.Store("EasyModbus Server listing for incomming data at Port " + (object) this.port, DateTime.Now);
              this.udpClient.Client.ReceiveTimeout = 1000;
              this.iPEndPoint = new IPEndPoint(IPAddress.Any, this.port);
              this.PortChanged = false;
            }
            if (this.tcpHandler != null)
              this.tcpHandler.Disconnect();
            try
            {
              this.bytes = this.udpClient.Receive(ref this.iPEndPoint);
              this.portIn = this.iPEndPoint.Port;
              NetworkConnectionParameter connectionParameter = new NetworkConnectionParameter();
              connectionParameter.bytes = this.bytes;
              this.ipAddressIn = this.iPEndPoint.Address;
              connectionParameter.portIn = this.portIn;
              connectionParameter.ipAddressIn = this.ipAddressIn;
              new Thread(new ParameterizedThreadStart(this.ProcessReceivedData)).Start((object) connectionParameter);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
    }

    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
      if (checked (DateTime.Now.Ticks - this.lastReceive.Ticks) > checked (10000L * (long) unchecked (4000 / this.baudrate)))
        this.nextSign = 0;
      System.IO.Ports.SerialPort serialPort = (System.IO.Ports.SerialPort) sender;
      int bytesToRead = serialPort.BytesToRead;
      byte[] buffer = new byte[bytesToRead];
      serialPort.Read(buffer, 0, bytesToRead);
      Array.Copy((Array) buffer, 0, (Array) this.readBuffer, this.nextSign, buffer.Length);
      this.lastReceive = DateTime.Now;
      this.nextSign = checked (bytesToRead + this.nextSign);
      if (ModbusClient.DetectValidModbusFrame(this.readBuffer, this.nextSign))
      {
        this.dataReceived = true;
        this.nextSign = 0;
        new Thread(new ParameterizedThreadStart(this.ProcessReceivedData)).Start((object) new NetworkConnectionParameter()
        {
          bytes = this.readBuffer
        });
        this.dataReceived = false;
      }
      else
        this.dataReceived = false;
    }

    private void numberOfClientsChanged()
    {
      this.numberOfConnections = this.tcpHandler.NumberOfConnectedClients;
      // ISSUE: reference to a compiler-generated field
      if (this.NumberOfConnectedClientsChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.NumberOfConnectedClientsChanged();
    }

    private void ProcessReceivedData(object networkConnectionParameter)
    {
      lock (this.lockProcessReceivedData)
      {
        byte[] numArray1 = new byte[((NetworkConnectionParameter) networkConnectionParameter).bytes.Length];
        if (this.debug)
          StoreLogData.Instance.Store("Received Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        NetworkStream stream = ((NetworkConnectionParameter) networkConnectionParameter).stream;
        int portIn = ((NetworkConnectionParameter) networkConnectionParameter).portIn;
        IPAddress ipAddressIn = ((NetworkConnectionParameter) networkConnectionParameter).ipAddressIn;
        Array.Copy((Array) ((NetworkConnectionParameter) networkConnectionParameter).bytes, 0, (Array) numArray1, 0, ((NetworkConnectionParameter) networkConnectionParameter).bytes.Length);
        ModbusProtocol receiveData = new ModbusProtocol();
        ModbusProtocol sendData = new ModbusProtocol();
        try
        {
          ushort[] numArray2 = new ushort[1];
          byte[] numArray3 = new byte[2];
          receiveData.timeStamp = DateTime.Now;
          receiveData.request = true;
          if (!this.serialFlag)
          {
            numArray3[1] = numArray1[0];
            numArray3[0] = numArray1[1];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.transactionIdentifier = numArray2[0];
            numArray3[1] = numArray1[2];
            numArray3[0] = numArray1[3];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.protocolIdentifier = numArray2[0];
            numArray3[1] = numArray1[4];
            numArray3[0] = numArray1[5];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.length = numArray2[0];
          }
          receiveData.unitIdentifier = numArray1[checked (6 - 6 * Convert.ToInt32(this.serialFlag))];
          if ((int) receiveData.unitIdentifier != (int) this.unitIdentifier & receiveData.unitIdentifier > (byte) 0)
            return;
          receiveData.functionCode = numArray1[checked (7 - 6 * Convert.ToInt32(this.serialFlag))];
          numArray3[1] = numArray1[checked (8 - 6 * Convert.ToInt32(this.serialFlag))];
          numArray3[0] = numArray1[checked (9 - 6 * Convert.ToInt32(this.serialFlag))];
          Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
          receiveData.startingAdress = numArray2[0];
          if (receiveData.functionCode <= (byte) 4)
          {
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.quantity = numArray2[0];
          }
          if (receiveData.functionCode == (byte) 5)
          {
            receiveData.receiveCoilValues = new ushort[1];
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) receiveData.receiveCoilValues, 0, 2);
          }
          if (receiveData.functionCode == (byte) 6)
          {
            receiveData.receiveRegisterValues = new ushort[1];
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) receiveData.receiveRegisterValues, 0, 2);
          }
          if (receiveData.functionCode == (byte) 15)
          {
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.quantity = numArray2[0];
            receiveData.byteCount = numArray1[checked (12 - 6 * Convert.ToInt32(this.serialFlag))];
            receiveData.receiveCoilValues = (uint) receiveData.byteCount % 2U <= 0U ? new ushort[(int) receiveData.byteCount / 2] : new ushort[checked (unchecked ((int) receiveData.byteCount / 2) + 1)];
            Buffer.BlockCopy((Array) numArray1, checked (13 - 6 * Convert.ToInt32(this.serialFlag)), (Array) receiveData.receiveCoilValues, 0, (int) receiveData.byteCount);
          }
          if (receiveData.functionCode == (byte) 16)
          {
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.quantity = numArray2[0];
            receiveData.byteCount = numArray1[checked (12 - 6 * Convert.ToInt32(this.serialFlag))];
            receiveData.receiveRegisterValues = new ushort[(int) receiveData.quantity];
            int num = 0;
            while (num < (int) receiveData.quantity)
            {
              numArray3[1] = numArray1[checked (13 + num * 2 - 6 * Convert.ToInt32(this.serialFlag))];
              numArray3[0] = numArray1[checked (14 + num * 2 - 6 * Convert.ToInt32(this.serialFlag))];
              Buffer.BlockCopy((Array) numArray3, 0, (Array) receiveData.receiveRegisterValues, checked (num * 2), 2);
              checked { ++num; }
            }
          }
          if (receiveData.functionCode == (byte) 23)
          {
            numArray3[1] = numArray1[checked (8 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (9 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.startingAddressRead = numArray2[0];
            numArray3[1] = numArray1[checked (10 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (11 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.quantityRead = numArray2[0];
            numArray3[1] = numArray1[checked (12 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (13 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.startingAddressWrite = numArray2[0];
            numArray3[1] = numArray1[checked (14 - 6 * Convert.ToInt32(this.serialFlag))];
            numArray3[0] = numArray1[checked (15 - 6 * Convert.ToInt32(this.serialFlag))];
            Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray2, 0, 2);
            receiveData.quantityWrite = numArray2[0];
            receiveData.byteCount = numArray1[checked (16 - 6 * Convert.ToInt32(this.serialFlag))];
            receiveData.receiveRegisterValues = new ushort[(int) receiveData.quantityWrite];
            int num = 0;
            while (num < (int) receiveData.quantityWrite)
            {
              numArray3[1] = numArray1[checked (17 + num * 2 - 6 * Convert.ToInt32(this.serialFlag))];
              numArray3[0] = numArray1[checked (18 + num * 2 - 6 * Convert.ToInt32(this.serialFlag))];
              Buffer.BlockCopy((Array) numArray3, 0, (Array) receiveData.receiveRegisterValues, checked (num * 2), 2);
              checked { ++num; }
            }
          }
        }
        catch (Exception ex)
        {
        }
        this.CreateAnswer(receiveData, sendData, stream, portIn, ipAddressIn);
        this.CreateLogData(receiveData, sendData);
        // ISSUE: reference to a compiler-generated field
        if (this.LogDataChanged == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.LogDataChanged();
      }
    }

    private void CreateAnswer(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      switch (receiveData.functionCode)
      {
        case 1:
          if (!this.FunctionCode1Disabled)
          {
            this.ReadCoils(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 2:
          if (!this.FunctionCode2Disabled)
          {
            this.ReadDiscreteInputs(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 3:
          if (!this.FunctionCode3Disabled)
          {
            this.ReadHoldingRegisters(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 4:
          if (!this.FunctionCode4Disabled)
          {
            this.ReadInputRegisters(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 5:
          if (!this.FunctionCode5Disabled)
          {
            this.WriteSingleCoil(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 6:
          if (!this.FunctionCode6Disabled)
          {
            this.WriteSingleRegister(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 15:
          if (!this.FunctionCode15Disabled)
          {
            this.WriteMultipleCoils(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 16:
          if (!this.FunctionCode16Disabled)
          {
            this.WriteMultipleRegisters(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        case 23:
          if (!this.FunctionCode23Disabled)
          {
            this.ReadWriteMultipleRegisters(receiveData, sendData, stream, portIn, ipAddressIn);
            break;
          }
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
        default:
          sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
          sendData.exceptionCode = (byte) 1;
          this.sendException((int) sendData.errorCode, (int) sendData.exceptionCode, receiveData, sendData, stream, portIn, ipAddressIn);
          break;
      }
      sendData.timeStamp = DateTime.Now;
    }

    private void ReadCoils(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      if (receiveData.quantity < (ushort) 1 | receiveData.quantity > (ushort) 2000)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        sendData.byteCount = (int) receiveData.quantity % 8 != 0 ? checked ((byte) (unchecked ((int) receiveData.quantity / 8) + 1)) : checked ((byte) unchecked ((int) receiveData.quantity / 8));
        sendData.sendCoilValues = new bool[(int) receiveData.quantity];
        lock (this.lockCoils)
          Array.Copy((Array) this.coils.localArray, checked ((int) receiveData.startingAdress + 1), (Array) sendData.sendCoilValues, 0, (int) receiveData.quantity);
      }
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      numArray1[8] = sendData.byteCount;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendCoilValues = (bool[]) null;
      }
      if (sendData.sendCoilValues != null)
      {
        int num1 = 0;
        while (num1 < (int) sendData.byteCount)
        {
          byte[] numArray3 = new byte[2];
          int num2 = 0;
          while (num2 < 8)
          {
            byte num3 = !sendData.sendCoilValues[checked (num1 * 8 + num2)] ? (byte) 0 : (byte) 1;
            numArray3[1] = checked ((byte) ((int) numArray3[1] | (int) num3 << num2));
            if (checked (num1 * 8 + num2 + 1) < sendData.sendCoilValues.Length)
              checked { ++num2; }
            else
              break;
          }
          numArray1[checked (9 + num1)] = numArray3[1];
          checked { ++num1; }
        }
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (!this.debug)
            return;
          byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void ReadDiscreteInputs(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      if (receiveData.quantity < (ushort) 1 | receiveData.quantity > (ushort) 2000)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        sendData.byteCount = (int) receiveData.quantity % 8 != 0 ? checked ((byte) (unchecked ((int) receiveData.quantity / 8) + 1)) : checked ((byte) unchecked ((int) receiveData.quantity / 8));
        sendData.sendCoilValues = new bool[(int) receiveData.quantity];
        Array.Copy((Array) this.discreteInputs.localArray, checked ((int) receiveData.startingAdress + 1), (Array) sendData.sendCoilValues, 0, (int) receiveData.quantity);
      }
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      numArray1[8] = sendData.byteCount;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendCoilValues = (bool[]) null;
      }
      if (sendData.sendCoilValues != null)
      {
        int num1 = 0;
        while (num1 < (int) sendData.byteCount)
        {
          byte[] numArray3 = new byte[2];
          int num2 = 0;
          while (num2 < 8)
          {
            byte num3 = !sendData.sendCoilValues[checked (num1 * 8 + num2)] ? (byte) 0 : (byte) 1;
            numArray3[1] = checked ((byte) ((int) numArray3[1] | (int) num3 << num2));
            if (checked (num1 * 8 + num2 + 1) < sendData.sendCoilValues.Length)
              checked { ++num2; }
            else
              break;
          }
          numArray1[checked (9 + num1)] = numArray3[1];
          checked { ++num1; }
        }
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (!this.debug)
            return;
          byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void ReadHoldingRegisters(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      if (receiveData.quantity < (ushort) 1 | receiveData.quantity > (ushort) 125)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        sendData.byteCount = checked ((byte) (2 * (int) receiveData.quantity));
        sendData.sendRegisterValues = new short[(int) receiveData.quantity];
        lock (this.lockHoldingRegisters)
          Buffer.BlockCopy((Array) this.holdingRegisters.localArray, checked ((int) receiveData.startingAdress * 2 + 2), (Array) sendData.sendRegisterValues, 0, checked ((int) receiveData.quantity * 2));
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? checked ((ushort) (3 + (int) sendData.byteCount)) : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      numArray1[8] = sendData.byteCount;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      if (sendData.sendRegisterValues != null)
      {
        int index = 0;
        while (index < (int) sendData.byteCount / 2)
        {
          byte[] bytes4 = BitConverter.GetBytes(sendData.sendRegisterValues[index]);
          numArray1[checked (9 + index * 2)] = bytes4[1];
          numArray1[checked (10 + index * 2)] = bytes4[0];
          checked { ++index; }
        }
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (!this.debug)
            return;
          byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void ReadInputRegisters(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      if (receiveData.quantity < (ushort) 1 | receiveData.quantity > (ushort) 125)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        sendData.byteCount = checked ((byte) (2 * (int) receiveData.quantity));
        sendData.sendRegisterValues = new short[(int) receiveData.quantity];
        Buffer.BlockCopy((Array) this.inputRegisters.localArray, checked ((int) receiveData.startingAdress * 2 + 2), (Array) sendData.sendRegisterValues, 0, checked ((int) receiveData.quantity * 2));
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? checked ((ushort) (3 + (int) sendData.byteCount)) : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      numArray1[8] = sendData.byteCount;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      if (sendData.sendRegisterValues != null)
      {
        int index = 0;
        while (index < (int) sendData.byteCount / 2)
        {
          byte[] bytes4 = BitConverter.GetBytes(sendData.sendRegisterValues[index]);
          numArray1[checked (9 + index * 2)] = bytes4[1];
          numArray1[checked (10 + index * 2)] = bytes4[0];
          checked { ++index; }
        }
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (!this.debug)
            return;
          byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void WriteSingleCoil(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      sendData.startingAdress = receiveData.startingAdress;
      sendData.receiveCoilValues = receiveData.receiveCoilValues;
      if (receiveData.receiveCoilValues[0] > (ushort) 0 & receiveData.receiveCoilValues[0] != (ushort) 65280)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        if (receiveData.receiveCoilValues[0] == (ushort) 65280)
        {
          lock (this.lockCoils)
            this.coils[checked ((int) receiveData.startingAdress + 1)] = true;
        }
        if (receiveData.receiveCoilValues[0] == (ushort) 0)
        {
          lock (this.lockCoils)
            this.coils[checked ((int) receiveData.startingAdress + 1)] = false;
        }
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? (ushort) 6 : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (12 + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      else
      {
        byte[] bytes4 = BitConverter.GetBytes((int) receiveData.startingAdress);
        numArray1[8] = bytes4[1];
        numArray1[9] = bytes4[0];
        byte[] bytes5 = BitConverter.GetBytes((int) receiveData.receiveCoilValues[0]);
        numArray1[10] = bytes5[1];
        numArray1[11] = bytes5[0];
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
            Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
            if (this.debug)
              StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
      // ISSUE: reference to a compiler-generated field
      if (this.CoilsChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.CoilsChanged(checked ((int) receiveData.startingAdress + 1), 1);
    }

    private void WriteSingleRegister(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      sendData.startingAdress = receiveData.startingAdress;
      sendData.receiveRegisterValues = receiveData.receiveRegisterValues;
      if (receiveData.receiveRegisterValues[0] < (ushort) 0 | receiveData.receiveRegisterValues[0] > ushort.MaxValue)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        lock (this.lockHoldingRegisters)
          this.holdingRegisters[checked ((int) receiveData.startingAdress + 1)] = (short) receiveData.receiveRegisterValues[0];
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? (ushort) 6 : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (12 + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      else
      {
        byte[] bytes4 = BitConverter.GetBytes((int) receiveData.startingAdress);
        numArray1[8] = bytes4[1];
        numArray1[9] = bytes4[0];
        byte[] bytes5 = BitConverter.GetBytes((int) receiveData.receiveRegisterValues[0]);
        numArray1[10] = bytes5[1];
        numArray1[11] = bytes5[0];
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
            Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
            if (this.debug)
              StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
      // ISSUE: reference to a compiler-generated field
      if (this.HoldingRegistersChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.HoldingRegistersChanged(checked ((int) receiveData.startingAdress + 1), 1);
    }

    private void WriteMultipleCoils(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      sendData.startingAdress = receiveData.startingAdress;
      sendData.quantity = receiveData.quantity;
      if (receiveData.quantity == (ushort) 0 | receiveData.quantity > (ushort) 1968)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        lock (this.lockCoils)
        {
          int num1 = 0;
          while (num1 < (int) receiveData.quantity)
          {
            int num2 = 1 << num1 % 16;
            this.coils[checked ((int) receiveData.startingAdress + num1 + 1)] = ((int) receiveData.receiveCoilValues[num1 / 16] & (int) checked ((ushort) num2)) != 0;
            checked { ++num1; }
          }
        }
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? (ushort) 6 : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (12 + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      else
      {
        byte[] bytes4 = BitConverter.GetBytes((int) receiveData.startingAdress);
        numArray1[8] = bytes4[1];
        numArray1[9] = bytes4[0];
        byte[] bytes5 = BitConverter.GetBytes((int) receiveData.quantity);
        numArray1[10] = bytes5[1];
        numArray1[11] = bytes5[0];
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
            Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
            if (this.debug)
              StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
      // ISSUE: reference to a compiler-generated field
      if (this.CoilsChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.CoilsChanged(checked ((int) receiveData.startingAdress + 1), (int) receiveData.quantity);
    }

    private void WriteMultipleRegisters(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      sendData.startingAdress = receiveData.startingAdress;
      sendData.quantity = receiveData.quantity;
      if (receiveData.quantity == (ushort) 0 | receiveData.quantity > (ushort) 1968)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAdress + 1 + (int) receiveData.quantity) > (int) ushort.MaxValue | receiveData.startingAdress < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        lock (this.lockHoldingRegisters)
        {
          int index = 0;
          while (index < (int) receiveData.quantity)
          {
            this.holdingRegisters[checked ((int) receiveData.startingAdress + index + 1)] = (short) receiveData.receiveRegisterValues[index];
            checked { ++index; }
          }
        }
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? (ushort) 6 : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (12 + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      else
      {
        byte[] bytes4 = BitConverter.GetBytes((int) receiveData.startingAdress);
        numArray1[8] = bytes4[1];
        numArray1[9] = bytes4[0];
        byte[] bytes5 = BitConverter.GetBytes((int) receiveData.quantity);
        numArray1[10] = bytes5[1];
        numArray1[11] = bytes5[0];
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
            Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
            if (this.debug)
              StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
      // ISSUE: reference to a compiler-generated field
      if (this.HoldingRegistersChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.HoldingRegistersChanged(checked ((int) receiveData.startingAdress + 1), (int) receiveData.quantity);
    }

    private void ReadWriteMultipleRegisters(ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = this.unitIdentifier;
      sendData.functionCode = receiveData.functionCode;
      if (receiveData.quantityRead < (ushort) 1 | receiveData.quantityRead > (ushort) 125 | receiveData.quantityWrite < (ushort) 1 | receiveData.quantityWrite > (ushort) 121 | (int) receiveData.byteCount != checked ((int) receiveData.quantityWrite * 2))
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 3;
      }
      if (checked ((int) receiveData.startingAddressRead + 1 + (int) receiveData.quantityRead) > (int) ushort.MaxValue | checked ((int) receiveData.startingAddressWrite + 1 + (int) receiveData.quantityWrite) > (int) ushort.MaxValue | receiveData.quantityWrite < (ushort) 0 | receiveData.quantityRead < (ushort) 0)
      {
        sendData.errorCode = checked ((byte) ((int) receiveData.functionCode + 128));
        sendData.exceptionCode = (byte) 2;
      }
      if (sendData.exceptionCode == (byte) 0)
      {
        sendData.sendRegisterValues = new short[(int) receiveData.quantityRead];
        lock (this.lockHoldingRegisters)
          Buffer.BlockCopy((Array) this.holdingRegisters.localArray, checked ((int) receiveData.startingAddressRead * 2 + 2), (Array) sendData.sendRegisterValues, 0, checked ((int) receiveData.quantityRead * 2));
        lock (this.holdingRegisters)
        {
          int index = 0;
          while (index < (int) receiveData.quantityWrite)
          {
            this.holdingRegisters[checked ((int) receiveData.startingAddressWrite + index + 1)] = (short) receiveData.receiveRegisterValues[index];
            checked { ++index; }
          }
        }
        sendData.byteCount = checked ((byte) (2 * (int) receiveData.quantityRead));
      }
      sendData.length = sendData.exceptionCode <= (byte) 0 ? Convert.ToUInt16(checked (3 + 2 * (int) receiveData.quantityRead)) : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.functionCode;
      numArray1[8] = sendData.byteCount;
      if (sendData.exceptionCode > (byte) 0)
      {
        numArray1[7] = sendData.errorCode;
        numArray1[8] = sendData.exceptionCode;
        sendData.sendRegisterValues = (short[]) null;
      }
      else if (sendData.sendRegisterValues != null)
      {
        int index = 0;
        while (index < (int) sendData.byteCount / 2)
        {
          byte[] bytes4 = BitConverter.GetBytes(sendData.sendRegisterValues[index]);
          numArray1[checked (9 + index * 2)] = bytes4[1];
          numArray1[checked (10 + index * 2)] = bytes4[0];
          checked { ++index; }
        }
      }
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
            Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
            if (this.debug)
              StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
      // ISSUE: reference to a compiler-generated field
      if (this.HoldingRegistersChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.HoldingRegistersChanged(checked ((int) receiveData.startingAddressWrite + 1), (int) receiveData.quantityWrite);
    }

    private void sendException(int errorCode, int exceptionCode, ModbusProtocol receiveData, ModbusProtocol sendData, NetworkStream stream, int portIn, IPAddress ipAddressIn)
    {
      sendData.response = true;
      sendData.transactionIdentifier = receiveData.transactionIdentifier;
      sendData.protocolIdentifier = receiveData.protocolIdentifier;
      sendData.unitIdentifier = receiveData.unitIdentifier;
      sendData.errorCode = checked ((byte) errorCode);
      sendData.exceptionCode = checked ((byte) exceptionCode);
      sendData.length = sendData.exceptionCode <= (byte) 0 ? checked ((ushort) (3 + (int) sendData.byteCount)) : (ushort) 3;
      byte[] numArray1 = sendData.exceptionCode <= (byte) 0 ? new byte[checked (9 + (int) sendData.byteCount + 2 * Convert.ToInt32(this.serialFlag))] : new byte[checked (9 + 2 * Convert.ToInt32(this.serialFlag))];
      byte[] numArray2 = new byte[2];
      sendData.length = (ushort) checked ((byte) (numArray1.Length - 6));
      byte[] bytes1 = BitConverter.GetBytes((int) sendData.transactionIdentifier);
      numArray1[0] = bytes1[1];
      numArray1[1] = bytes1[0];
      byte[] bytes2 = BitConverter.GetBytes((int) sendData.protocolIdentifier);
      numArray1[2] = bytes2[1];
      numArray1[3] = bytes2[0];
      byte[] bytes3 = BitConverter.GetBytes((int) sendData.length);
      numArray1[4] = bytes3[1];
      numArray1[5] = bytes3[0];
      numArray1[6] = sendData.unitIdentifier;
      numArray1[7] = sendData.errorCode;
      numArray1[8] = sendData.exceptionCode;
      try
      {
        if (this.serialFlag)
        {
          if (!this.serialport.IsOpen)
            throw new SerialPortNotOpenedException("serial port not opened");
          sendData.crc = ModbusClient.calculateCRC(numArray1, Convert.ToUInt16(checked (numArray1.Length - 8)), 6);
          byte[] bytes4 = BitConverter.GetBytes((int) sendData.crc);
          numArray1[checked (numArray1.Length - 2)] = bytes4[0];
          numArray1[checked (numArray1.Length - 1)] = bytes4[1];
          this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
          if (!this.debug)
            return;
          byte[] numArray3 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray3, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        else if (this.udpFlag)
        {
          IPEndPoint endPoint = new IPEndPoint(ipAddressIn, portIn);
          this.udpClient.Send(numArray1, numArray1.Length, endPoint);
        }
        else
        {
          stream.Write(numArray1, 0, numArray1.Length);
          if (this.debug)
            StoreLogData.Instance.Store("Send Data: " + BitConverter.ToString(numArray1), DateTime.Now);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void CreateLogData(ModbusProtocol receiveData, ModbusProtocol sendData)
    {
      int num = 0;
      while (num < 98)
      {
        this.modbusLogData[checked (99 - num)] = this.modbusLogData[checked (99 - num - 2)];
        checked { ++num; }
      }
      this.modbusLogData[0] = receiveData;
      this.modbusLogData[1] = sendData;
    }

    public void DeleteRetainedMessages(string topic)
    {
      if (this.MqttBrokerAddress == null)
        return;
      this.easyModbus2Mqtt.publish(topic, (string) null, this.MqttBrokerAddress);
    }

    public int NumberOfConnections
    {
      get
      {
        return this.numberOfConnections;
      }
    }

    public ModbusProtocol[] ModbusLogData
    {
      get
      {
        return this.modbusLogData;
      }
    }

    public int Port
    {
      get
      {
        return this.port;
      }
      set
      {
        this.port = value;
      }
    }

    public bool UDPFlag
    {
      get
      {
        return this.udpFlag;
      }
      set
      {
        this.udpFlag = value;
      }
    }

    public bool SerialFlag
    {
      get
      {
        return this.serialFlag;
      }
      set
      {
        this.serialFlag = value;
      }
    }

    public int Baudrate
    {
      get
      {
        return this.baudrate;
      }
      set
      {
        this.baudrate = value;
      }
    }

    public Parity Parity
    {
      get
      {
        return this.parity;
      }
      set
      {
        this.parity = value;
      }
    }

    public StopBits StopBits
    {
      get
      {
        return this.stopBits;
      }
      set
      {
        this.stopBits = value;
      }
    }

    public string SerialPort
    {
      get
      {
        return this.serialPort;
      }
      set
      {
        this.serialPort = value;
        if (this.serialPort != null)
          this.serialFlag = true;
        else
          this.serialFlag = false;
      }
    }

    public byte UnitIdentifier
    {
      get
      {
        return this.unitIdentifier;
      }
      set
      {
        this.unitIdentifier = value;
      }
    }

    public string LogFileFilename
    {
      get
      {
        return StoreLogData.Instance.Filename;
      }
      set
      {
        StoreLogData.Instance.Filename = value;
        if (StoreLogData.Instance.Filename != null)
          this.debug = true;
        else
          this.debug = false;
      }
    }

    public string MqttBrokerAddress
    {
      get
      {
        return this.easyModbus2Mqtt.MqttBrokerAddress;
      }
      set
      {
        this.easyModbus2Mqtt.MqttBrokerAddress = value;
      }
    }

    public string MqttRootTopic
    {
      get
      {
        return this.easyModbus2Mqtt.MqttRootTopic;
      }
      set
      {
        this.easyModbus2Mqtt.MqttRootTopic = value;
      }
    }

    public string MqttUserName
    {
      get
      {
        return this.easyModbus2Mqtt.MqttUserName;
      }
      set
      {
        this.easyModbus2Mqtt.MqttUserName = value;
      }
    }

    public string MqttPassword
    {
      get
      {
        return this.easyModbus2Mqtt.MqttPassword;
      }
      set
      {
        this.easyModbus2Mqtt.MqttPassword = value;
      }
    }

    public bool MqttRetainMessages
    {
      get
      {
        return this.easyModbus2Mqtt.RetainMessages;
      }
      set
      {
        this.easyModbus2Mqtt.RetainMessages = value;
      }
    }

    public int MqttBrokerPort
    {
      get
      {
        return this.easyModbus2Mqtt.MqttBrokerPort;
      }
      set
      {
        this.easyModbus2Mqtt.MqttBrokerPort = value;
      }
    }

    public delegate void CoilsChangedHandler(int coil, int numberOfCoils);

    public delegate void HoldingRegistersChangedHandler(int register, int numberOfRegisters);

    public delegate void NumberOfConnectedClientsChangedHandler();

    public delegate void LogDataChangedHandler();
  }
}
