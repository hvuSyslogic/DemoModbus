// Decompiled with JetBrains decompiler
// Type: EasyModbus.ModbusClient
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using EasyModbus.Exceptions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace EasyModbus
{
  public class ModbusClient
  {
    private bool debug = false;
    private string ipAddress = "192.168.0.1";
    private int port = 502;
    private uint transactionIdentifierInternal = 0;
    private byte[] transactionIdentifier = new byte[2];
    private byte[] protocolIdentifier = new byte[2];
    private byte[] crc = new byte[2];
    private byte[] length = new byte[2];
    private byte unitIdentifier = 1;
    private byte[] startingAddress = new byte[2];
    private byte[] quantity = new byte[2];
    private bool udpFlag = false;
    private int baudRate = 9600;
    private int connectTimeout = 1000;
    private Parity parity = Parity.Even;
    private StopBits stopBits = StopBits.One;
    private bool connected = false;
    private int countRetries = 0;
    private string mqttRootTopic = "easymodbusclient";
    private bool dataReceived = false;
    private bool receiveActive = false;
    private byte[] readBuffer = new byte[256];
    private int bytesToRead = 0;
    private int akjjjctualPositionToRead = 0;
    private TcpClient tcpClient;
    private byte functionCode;
    private int portOut;
    public byte[] receiveData;
    public byte[] sendData;
    private System.IO.Ports.SerialPort serialport;
    private bool[] mqttCoilsOldValues;
    private bool[] mqttDiscreteInputsOldValues;
    private int[] mqttInputRegistersOldValues;
    private int[] mqttHoldingRegistersOldValues;
    private EasyModbus2Mqtt easyModbus2Mqtt;
    private bool mqttRetainMessages;
    private NetworkStream stream;
    private DateTime dateTimeLastRead;

    public int NumberOfRetries { get; set; } = 3;

    public string MqttUserName { get; set; }

    public string MqttPassword { get; set; }

    public bool MqttPushOnChange { get; set; } = true;

    public int MqttBrokerPort { get; set; } = 1883;

    public event ModbusClient.ReceiveDataChangedHandler ReceiveDataChanged;

    public event ModbusClient.SendDataChangedHandler SendDataChanged;

    public event ModbusClient.ConnectedChangedHandler ConnectedChanged;

    public ModbusClient(string ipAddress, int port)
    {
      if (this.debug)
        StoreLogData.Instance.Store("EasyModbus library initialized for Modbus-TCP, IPAddress: " + ipAddress + ", Port: " + (object) port, DateTime.Now);
      Console.WriteLine("EasyModbus Client Library Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
      Console.WriteLine("Copyright (c) Stefan Rossmann Engineering Solutions");
      Console.WriteLine();
      this.ipAddress = ipAddress;
      this.port = port;
    }

    public ModbusClient(string serialPort)
    {
      if (this.debug)
        StoreLogData.Instance.Store("EasyModbus library initialized for Modbus-RTU, COM-Port: " + serialPort, DateTime.Now);
      Console.WriteLine("EasyModbus Client Library Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
      Console.WriteLine("Copyright (c) Stefan Rossmann Engineering Solutions");
      Console.WriteLine();
      this.serialport = new System.IO.Ports.SerialPort();
      this.serialport.PortName = serialPort;
      this.serialport.BaudRate = this.baudRate;
      this.serialport.Parity = this.parity;
      this.serialport.StopBits = this.stopBits;
      this.serialport.WriteTimeout = 10000;
      this.serialport.ReadTimeout = this.connectTimeout;
      this.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
    }

    public ModbusClient()
    {
      if (this.debug)
        StoreLogData.Instance.Store("EasyModbus library initialized for Modbus-TCP", DateTime.Now);
      Console.WriteLine("EasyModbus Client Library Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
      Console.WriteLine("Copyright (c) Stefan Rossmann Engineering Solutions");
      Console.WriteLine();
    }

    public void Connect()
    {
      if (this.serialport != null)
      {
        if (!this.serialport.IsOpen)
        {
          if (this.debug)
            StoreLogData.Instance.Store("Open Serial port " + this.serialport.PortName, DateTime.Now);
          this.serialport.BaudRate = this.baudRate;
          this.serialport.Parity = this.parity;
          this.serialport.StopBits = this.stopBits;
          this.serialport.WriteTimeout = 10000;
          this.serialport.ReadTimeout = this.connectTimeout;
          this.serialport.Open();
          this.connected = true;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.ConnectedChanged == null)
          return;
        try
        {
          // ISSUE: reference to a compiler-generated field
          this.ConnectedChanged((object) this);
        }
        catch
        {
        }
      }
      else
      {
        if (!this.udpFlag)
        {
          if (this.debug)
            StoreLogData.Instance.Store("Open TCP-Socket, IP-Address: " + this.ipAddress + ", Port: " + (object) this.port, DateTime.Now);
          this.tcpClient = new TcpClient();
          IAsyncResult asyncResult = this.tcpClient.BeginConnect(this.ipAddress, this.port, (AsyncCallback) null, (object) null);
          if (!asyncResult.AsyncWaitHandle.WaitOne(this.connectTimeout))
            throw new ConnectionException("connection timed out");
          this.tcpClient.EndConnect(asyncResult);
          this.stream = this.tcpClient.GetStream();
          this.stream.ReadTimeout = this.connectTimeout;
          this.connected = true;
        }
        else
        {
          this.tcpClient = new TcpClient();
          this.connected = true;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.ConnectedChanged == null)
          return;
        try
        {
          // ISSUE: reference to a compiler-generated field
          this.ConnectedChanged((object) this);
        }
        catch
        {
        }
      }
    }

    public void Connect(string ipAddress, int port)
    {
      if (!this.udpFlag)
      {
        if (this.debug)
          StoreLogData.Instance.Store("Open TCP-Socket, IP-Address: " + ipAddress + ", Port: " + (object) port, DateTime.Now);
        this.tcpClient = new TcpClient();
        IAsyncResult asyncResult = this.tcpClient.BeginConnect(ipAddress, port, (AsyncCallback) null, (object) null);
        if (!asyncResult.AsyncWaitHandle.WaitOne(this.connectTimeout))
          throw new ConnectionException("connection timed out");
        this.tcpClient.EndConnect(asyncResult);
        this.stream = this.tcpClient.GetStream();
        this.stream.ReadTimeout = this.connectTimeout;
        this.connected = true;
      }
      else
      {
        this.tcpClient = new TcpClient();
        this.connected = true;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.ConnectedChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ConnectedChanged((object) this);
    }

    public static float ConvertRegistersToFloat(int[] registers)
    {
      if (registers.Length != 2)
        throw new ArgumentException("Input Array length invalid - Array langth must be '2'");
      int register1 = registers[1];
      int register2 = registers[0];
      byte[] bytes1 = BitConverter.GetBytes(register1);
      byte[] bytes2 = BitConverter.GetBytes(register2);
      return BitConverter.ToSingle(new byte[4]{ bytes2[0], bytes2[1], bytes1[0], bytes1[1] }, 0);
    }

    public static float ConvertRegistersToFloat(int[] registers, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers1 = new int[2]{ registers[0], registers[1] };
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        registers1 = new int[2]
        {
          registers[1],
          registers[0]
        };
      return ModbusClient.ConvertRegistersToFloat(registers1);
    }

    public static int ConvertRegistersToInt(int[] registers)
    {
      if (registers.Length != 2)
        throw new ArgumentException("Input Array length invalid - Array langth must be '2'");
      int register1 = registers[1];
      int register2 = registers[0];
      byte[] bytes1 = BitConverter.GetBytes(register1);
      byte[] bytes2 = BitConverter.GetBytes(register2);
      return BitConverter.ToInt32(new byte[4]{ bytes2[0], bytes2[1], bytes1[0], bytes1[1] }, 0);
    }

    public static int ConvertRegistersToInt(int[] registers, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers1 = new int[2]{ registers[0], registers[1] };
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        registers1 = new int[2]
        {
          registers[1],
          registers[0]
        };
      return ModbusClient.ConvertRegistersToInt(registers1);
    }

    public static long ConvertRegistersToLong(int[] registers)
    {
      if (registers.Length != 4)
        throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
      int register1 = registers[3];
      int register2 = registers[2];
      int register3 = registers[1];
      int register4 = registers[0];
      byte[] bytes1 = BitConverter.GetBytes(register1);
      byte[] bytes2 = BitConverter.GetBytes(register2);
      byte[] bytes3 = BitConverter.GetBytes(register3);
      byte[] bytes4 = BitConverter.GetBytes(register4);
      return BitConverter.ToInt64(new byte[8]{ bytes4[0], bytes4[1], bytes3[0], bytes3[1], bytes2[0], bytes2[1], bytes1[0], bytes1[1] }, 0);
    }

    public static long ConvertRegistersToLong(int[] registers, ModbusClient.RegisterOrder registerOrder)
    {
      if (registers.Length != 4)
        throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
      int[] registers1 = new int[4]{ registers[0], registers[1], registers[2], registers[3] };
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        registers1 = new int[4]
        {
          registers[3],
          registers[2],
          registers[1],
          registers[0]
        };
      return ModbusClient.ConvertRegistersToLong(registers1);
    }

    public static double ConvertRegistersToDouble(int[] registers)
    {
      if (registers.Length != 4)
        throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
      int register1 = registers[3];
      int register2 = registers[2];
      int register3 = registers[1];
      int register4 = registers[0];
      byte[] bytes1 = BitConverter.GetBytes(register1);
      byte[] bytes2 = BitConverter.GetBytes(register2);
      byte[] bytes3 = BitConverter.GetBytes(register3);
      byte[] bytes4 = BitConverter.GetBytes(register4);
      return BitConverter.ToDouble(new byte[8]{ bytes4[0], bytes4[1], bytes3[0], bytes3[1], bytes2[0], bytes2[1], bytes1[0], bytes1[1] }, 0);
    }

    public static double ConvertRegistersToDouble(int[] registers, ModbusClient.RegisterOrder registerOrder)
    {
      if (registers.Length != 4)
        throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
      int[] registers1 = new int[4]{ registers[0], registers[1], registers[2], registers[3] };
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        registers1 = new int[4]
        {
          registers[3],
          registers[2],
          registers[1],
          registers[0]
        };
      return ModbusClient.ConvertRegistersToDouble(registers1);
    }

    public static int[] ConvertFloatToRegisters(float floatValue)
    {
      byte[] bytes = BitConverter.GetBytes(floatValue);
      byte[] numArray = new byte[4]{ bytes[2], bytes[3], (byte) 0, (byte) 0 };
      return new int[2]{ BitConverter.ToInt32(new byte[4]{ bytes[0], bytes[1], (byte) 0, (byte) 0 }, 0), BitConverter.ToInt32(numArray, 0) };
    }

    public static int[] ConvertFloatToRegisters(float floatValue, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers = ModbusClient.ConvertFloatToRegisters(floatValue);
      int[] numArray = registers;
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        numArray = new int[2]{ registers[1], registers[0] };
      return numArray;
    }

    public static int[] ConvertIntToRegisters(int intValue)
    {
      byte[] bytes = BitConverter.GetBytes(intValue);
      byte[] numArray = new byte[4]{ bytes[2], bytes[3], (byte) 0, (byte) 0 };
      return new int[2]{ BitConverter.ToInt32(new byte[4]{ bytes[0], bytes[1], (byte) 0, (byte) 0 }, 0), BitConverter.ToInt32(numArray, 0) };
    }

    public static int[] ConvertIntToRegisters(int intValue, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers = ModbusClient.ConvertIntToRegisters(intValue);
      int[] numArray = registers;
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        numArray = new int[2]{ registers[1], registers[0] };
      return numArray;
    }

    public static int[] ConvertLongToRegisters(long longValue)
    {
      byte[] bytes = BitConverter.GetBytes(longValue);
      byte[] numArray1 = new byte[4]{ bytes[6], bytes[7], (byte) 0, (byte) 0 };
      byte[] numArray2 = new byte[4]{ bytes[4], bytes[5], (byte) 0, (byte) 0 };
      byte[] numArray3 = new byte[4]{ bytes[2], bytes[3], (byte) 0, (byte) 0 };
      return new int[4]{ BitConverter.ToInt32(new byte[4]{ bytes[0], bytes[1], (byte) 0, (byte) 0 }, 0), BitConverter.ToInt32(numArray3, 0), BitConverter.ToInt32(numArray2, 0), BitConverter.ToInt32(numArray1, 0) };
    }

    public static int[] ConvertLongToRegisters(long longValue, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers = ModbusClient.ConvertLongToRegisters(longValue);
      int[] numArray = registers;
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        numArray = new int[4]
        {
          registers[3],
          registers[2],
          registers[1],
          registers[0]
        };
      return numArray;
    }

    public static int[] ConvertDoubleToRegisters(double doubleValue)
    {
      byte[] bytes = BitConverter.GetBytes(doubleValue);
      byte[] numArray1 = new byte[4]{ bytes[6], bytes[7], (byte) 0, (byte) 0 };
      byte[] numArray2 = new byte[4]{ bytes[4], bytes[5], (byte) 0, (byte) 0 };
      byte[] numArray3 = new byte[4]{ bytes[2], bytes[3], (byte) 0, (byte) 0 };
      return new int[4]{ BitConverter.ToInt32(new byte[4]{ bytes[0], bytes[1], (byte) 0, (byte) 0 }, 0), BitConverter.ToInt32(numArray3, 0), BitConverter.ToInt32(numArray2, 0), BitConverter.ToInt32(numArray1, 0) };
    }

    public static int[] ConvertDoubleToRegisters(double doubleValue, ModbusClient.RegisterOrder registerOrder)
    {
      int[] registers = ModbusClient.ConvertDoubleToRegisters(doubleValue);
      int[] numArray = registers;
      if (registerOrder == ModbusClient.RegisterOrder.HighLow)
        numArray = new int[4]
        {
          registers[3],
          registers[2],
          registers[1],
          registers[0]
        };
      return numArray;
    }

    public static string ConvertRegistersToString(int[] registers, int offset, int stringLength)
    {
      byte[] bytes1 = new byte[stringLength];
      byte[] numArray = new byte[2];
      int num = 0;
      while (num < stringLength / 2)
      {
        byte[] bytes2 = BitConverter.GetBytes(registers[checked (offset + num)]);
        bytes1[checked (num * 2)] = bytes2[0];
        bytes1[checked (num * 2 + 1)] = bytes2[1];
        checked { ++num; }
      }
      return Encoding.Default.GetString(bytes1);
    }

    public static int[] ConvertStringToRegisters(string stringToConvert)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(stringToConvert);
      int[] numArray = new int[checked (unchecked (stringToConvert.Length / 2) + unchecked (stringToConvert.Length % 2))];
      int index = 0;
      while (index < numArray.Length)
      {
        numArray[index] = (int) bytes[checked (index * 2)];
        if (checked (index * 2 + 1) < bytes.Length)
          numArray[index] = numArray[index] | (int) bytes[checked (index * 2 + 1)] << 8;
        checked { ++index; }
      }
      return numArray;
    }

    public static ushort calculateCRC(byte[] data, ushort numberOfBytes, int startByte)
    {
      byte[] numArray1 = new byte[256]{ (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 1, (byte) 192, (byte) 128, (byte) 65, (byte) 0, (byte) 193, (byte) 129, (byte) 64 };
      byte[] numArray2 = new byte[256]{ (byte) 0, (byte) 192, (byte) 193, (byte) 1, (byte) 195, (byte) 3, (byte) 2, (byte) 194, (byte) 198, (byte) 6, (byte) 7, (byte) 199, (byte) 5, (byte) 197, (byte) 196, (byte) 4, (byte) 204, (byte) 12, (byte) 13, (byte) 205, (byte) 15, (byte) 207, (byte) 206, (byte) 14, (byte) 10, (byte) 202, (byte) 203, (byte) 11, (byte) 201, (byte) 9, (byte) 8, (byte) 200, (byte) 216, (byte) 24, (byte) 25, (byte) 217, (byte) 27, (byte) 219, (byte) 218, (byte) 26, (byte) 30, (byte) 222, (byte) 223, (byte) 31, (byte) 221, (byte) 29, (byte) 28, (byte) 220, (byte) 20, (byte) 212, (byte) 213, (byte) 21, (byte) 215, (byte) 23, (byte) 22, (byte) 214, (byte) 210, (byte) 18, (byte) 19, (byte) 211, (byte) 17, (byte) 209, (byte) 208, (byte) 16, (byte) 240, (byte) 48, (byte) 49, (byte) 241, (byte) 51, (byte) 243, (byte) 242, (byte) 50, (byte) 54, (byte) 246, (byte) 247, (byte) 55, (byte) 245, (byte) 53, (byte) 52, (byte) 244, (byte) 60, (byte) 252, (byte) 253, (byte) 61, byte.MaxValue, (byte) 63, (byte) 62, (byte) 254, (byte) 250, (byte) 58, (byte) 59, (byte) 251, (byte) 57, (byte) 249, (byte) 248, (byte) 56, (byte) 40, (byte) 232, (byte) 233, (byte) 41, (byte) 235, (byte) 43, (byte) 42, (byte) 234, (byte) 238, (byte) 46, (byte) 47, (byte) 239, (byte) 45, (byte) 237, (byte) 236, (byte) 44, (byte) 228, (byte) 36, (byte) 37, (byte) 229, (byte) 39, (byte) 231, (byte) 230, (byte) 38, (byte) 34, (byte) 226, (byte) 227, (byte) 35, (byte) 225, (byte) 33, (byte) 32, (byte) 224, (byte) 160, (byte) 96, (byte) 97, (byte) 161, (byte) 99, (byte) 163, (byte) 162, (byte) 98, (byte) 102, (byte) 166, (byte) 167, (byte) 103, (byte) 165, (byte) 101, (byte) 100, (byte) 164, (byte) 108, (byte) 172, (byte) 173, (byte) 109, (byte) 175, (byte) 111, (byte) 110, (byte) 174, (byte) 170, (byte) 106, (byte) 107, (byte) 171, (byte) 105, (byte) 169, (byte) 168, (byte) 104, (byte) 120, (byte) 184, (byte) 185, (byte) 121, (byte) 187, (byte) 123, (byte) 122, (byte) 186, (byte) 190, (byte) 126, (byte) 127, (byte) 191, (byte) 125, (byte) 189, (byte) 188, (byte) 124, (byte) 180, (byte) 116, (byte) 117, (byte) 181, (byte) 119, (byte) 183, (byte) 182, (byte) 118, (byte) 114, (byte) 178, (byte) 179, (byte) 115, (byte) 177, (byte) 113, (byte) 112, (byte) 176, (byte) 80, (byte) 144, (byte) 145, (byte) 81, (byte) 147, (byte) 83, (byte) 82, (byte) 146, (byte) 150, (byte) 86, (byte) 87, (byte) 151, (byte) 85, (byte) 149, (byte) 148, (byte) 84, (byte) 156, (byte) 92, (byte) 93, (byte) 157, (byte) 95, (byte) 159, (byte) 158, (byte) 94, (byte) 90, (byte) 154, (byte) 155, (byte) 91, (byte) 153, (byte) 89, (byte) 88, (byte) 152, (byte) 136, (byte) 72, (byte) 73, (byte) 137, (byte) 75, (byte) 139, (byte) 138, (byte) 74, (byte) 78, (byte) 142, (byte) 143, (byte) 79, (byte) 141, (byte) 77, (byte) 76, (byte) 140, (byte) 68, (byte) 132, (byte) 133, (byte) 69, (byte) 135, (byte) 71, (byte) 70, (byte) 134, (byte) 130, (byte) 66, (byte) 67, (byte) 131, (byte) 65, (byte) 129, (byte) 128, (byte) 64 };
      ushort num1 = numberOfBytes;
      byte maxValue = byte.MaxValue;
      byte num2 = byte.MaxValue;
      int num3 = 0;
      while (num1 > (ushort) 0)
      {
        checked { --num1; }
        if (checked (num3 + startByte) < data.Length)
        {
          int index = (int) num2 ^ (int) data[checked (num3 + startByte)];
          num2 = checked ((byte) ((int) maxValue ^ (int) numArray1[index]));
          maxValue = numArray2[index];
        }
        checked { ++num3; }
      }
      return checked ((ushort) ((int) maxValue << 8 | (int) num2));
    }

    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
      this.serialport.DataReceived -= new SerialDataReceivedEventHandler(this.DataReceivedHandler);
      this.receiveActive = true;
      System.IO.Ports.SerialPort serialPort = (System.IO.Ports.SerialPort) sender;
      if (this.bytesToRead == 0)
      {
        serialPort.DiscardInBuffer();
        this.receiveActive = false;
        this.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
      }
      else
      {
        this.readBuffer = new byte[256];
        int destinationIndex = 0;
        DateTime now = DateTime.Now;
        do
        {
          try
          {
            now = DateTime.Now;
            while (serialPort.BytesToRead == 0)
            {
              Thread.Sleep(10);
              if (checked (DateTime.Now.Ticks - now.Ticks) > 20000000L)
                break;
            }
            int bytesToRead = serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            serialPort.Read(buffer, 0, bytesToRead);
            Array.Copy((Array) buffer, 0, (Array) this.readBuffer, destinationIndex, checked (destinationIndex + buffer.Length) <= this.bytesToRead ? buffer.Length : checked (this.bytesToRead - destinationIndex));
            checked { destinationIndex += buffer.Length; }
          }
          catch (Exception ex)
          {
          }
        }
        while (this.bytesToRead > destinationIndex && (!(ModbusClient.DetectValidModbusFrame(this.readBuffer, destinationIndex < this.readBuffer.Length ? destinationIndex : this.readBuffer.Length) | this.bytesToRead <= destinationIndex) && checked (DateTime.Now.Ticks - now.Ticks) < 20000000L));
        this.receiveData = new byte[destinationIndex];
        Array.Copy((Array) this.readBuffer, 0, (Array) this.receiveData, 0, destinationIndex < this.readBuffer.Length ? destinationIndex : this.readBuffer.Length);
        if (this.debug)
          StoreLogData.Instance.Store("Received Serial-Data: " + BitConverter.ToString(this.readBuffer), DateTime.Now);
        this.bytesToRead = 0;
        this.dataReceived = true;
        this.receiveActive = false;
        this.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
        // ISSUE: reference to a compiler-generated field
        if (this.ReceiveDataChanged == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.ReceiveDataChanged((object) this);
      }
    }

    public static bool DetectValidModbusFrame(byte[] readBuffer, int length)
    {
      if (length < 6 || readBuffer[0] < (byte) 1 | readBuffer[0] > (byte) 247)
        return false;
      byte[] numArray = new byte[2];
      byte[] bytes = BitConverter.GetBytes(ModbusClient.calculateCRC(readBuffer, checked ((ushort) (length - 2)), 0));
      return !((int) bytes[0] != (int) readBuffer[checked (length - 2)] | (int) bytes[1] != (int) readBuffer[checked (length - 1)]);
    }

    public bool[] ReadDiscreteInputs(int startingAddress, int quantity, string mqttBrokerAddress)
    {
      bool[] flagArray = this.ReadDiscreteInputs(startingAddress, quantity);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (this.MqttPushOnChange && this.mqttDiscreteInputsOldValues == null)
        this.mqttDiscreteInputsOldValues = new bool[(int) ushort.MaxValue];
      int index = 0;
      while (index < flagArray.Length)
      {
        if (this.mqttDiscreteInputsOldValues == null || this.mqttDiscreteInputsOldValues[index] != flagArray[index])
        {
          stringList1.Add(this.mqttRootTopic + "/discreteinputs/" + checked (index + startingAddress).ToString());
          stringList2.Add(flagArray[index].ToString());
          this.mqttDiscreteInputsOldValues[index] = flagArray[index];
        }
        checked { ++index; }
      }
      if (this.easyModbus2Mqtt == null)
        this.easyModbus2Mqtt = new EasyModbus2Mqtt();
      this.easyModbus2Mqtt.MqttBrokerPort = this.MqttBrokerPort;
      this.easyModbus2Mqtt.MqttUserName = this.MqttUserName;
      this.easyModbus2Mqtt.MqttPassword = this.MqttPassword;
      this.easyModbus2Mqtt.RetainMessages = this.mqttRetainMessages;
      this.easyModbus2Mqtt.publish(stringList1.ToArray(), stringList2.ToArray(), mqttBrokerAddress);
      return flagArray;
    }

    public bool[] ReadDiscreteInputs(int startingAddress, int quantity)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC2 (Read Discrete Inputs from Master device), StartingAddress: " + (object) startingAddress + ", Quantity: " + (object) quantity, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      if (startingAddress > (int) ushort.MaxValue | quantity > 2000)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ArgumentException Throwed", DateTime.Now);
        throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 2000");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 2;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      this.quantity = BitConverter.GetBytes(quantity);
      byte[] numArray1 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], this.quantity[1], this.quantity[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      numArray1[12] = this.crc[0];
      numArray1[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = quantity % 8 != 0 ? checked (6 + unchecked (quantity / 8)) : checked (5 + unchecked (quantity / 8));
        this.serialport.Write(numArray1, 6, 8);
        if (this.debug)
        {
          byte[] numArray2 = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray1 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
            maxValue = numArray1[6];
          }
          else
            break;
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[2100];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 130 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 130 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 130 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 130 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport != null)
      {
        this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) ((int) numArray1[8] + 3)), 6));
        if (((int) this.crc[0] != (int) numArray1[checked ((int) numArray1[8] + 9)] | (int) this.crc[1] != (int) numArray1[checked ((int) numArray1[8] + 10)]) & this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new CRCCheckFailedException("Response CRC check failed");
          }
          checked { ++this.countRetries; }
          return this.ReadDiscreteInputs(startingAddress, quantity);
        }
        if (!this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new TimeoutException("No Response from Modbus Slave");
          }
          checked { ++this.countRetries; }
          return this.ReadDiscreteInputs(startingAddress, quantity);
        }
      }
      bool[] flagArray = new bool[quantity];
      int index = 0;
      while (index < quantity)
      {
        int num = (int) numArray1[checked (9 + unchecked (index / 8))];
        int int32 = Convert.ToInt32(Math.Pow(2.0, (double) (index % 8)));
        flagArray[index] = Convert.ToBoolean((num & int32) / int32);
        checked { ++index; }
      }
      return flagArray;
    }

    public bool[] ReadCoils(int startingAddress, int quantity, string mqttBrokerAddress)
    {
      bool[] flagArray = this.ReadCoils(startingAddress, quantity);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (this.MqttPushOnChange && this.mqttCoilsOldValues == null)
        this.mqttCoilsOldValues = new bool[(int) ushort.MaxValue];
      int index = 0;
      while (index < flagArray.Length)
      {
        if (this.mqttCoilsOldValues == null || this.mqttCoilsOldValues[index] != flagArray[index])
        {
          stringList1.Add(this.mqttRootTopic + "/coils/" + checked (index + startingAddress).ToString());
          stringList2.Add(flagArray[index].ToString());
          this.mqttCoilsOldValues[index] = flagArray[index];
        }
        checked { ++index; }
      }
      if (this.easyModbus2Mqtt == null)
        this.easyModbus2Mqtt = new EasyModbus2Mqtt();
      this.easyModbus2Mqtt.MqttBrokerPort = this.MqttBrokerPort;
      this.easyModbus2Mqtt.MqttUserName = this.MqttUserName;
      this.easyModbus2Mqtt.MqttPassword = this.MqttPassword;
      this.easyModbus2Mqtt.RetainMessages = this.mqttRetainMessages;
      this.easyModbus2Mqtt.publish(stringList1.ToArray(), stringList2.ToArray(), mqttBrokerAddress);
      return flagArray;
    }

    public bool[] ReadCoils(int startingAddress, int quantity)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC1 (Read Coils from Master device), StartingAddress: " + (object) startingAddress + ", Quantity: " + (object) quantity, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      if (startingAddress > (int) ushort.MaxValue | quantity > 2000)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ArgumentException Throwed", DateTime.Now);
        throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 2000");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 1;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      this.quantity = BitConverter.GetBytes(quantity);
      byte[] numArray1 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], this.quantity[1], this.quantity[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      numArray1[12] = this.crc[0];
      numArray1[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = quantity % 8 != 0 ? checked (6 + unchecked (quantity / 8)) : checked (5 + unchecked (quantity / 8));
        this.serialport.Write(numArray1, 6, 8);
        if (this.debug)
        {
          byte[] numArray2 = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray1 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
            maxValue = numArray1[6];
          }
          else
            break;
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send MocbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[2100];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 129 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 129 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 129 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 129 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport != null)
      {
        this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) ((int) numArray1[8] + 3)), 6));
        if (((int) this.crc[0] != (int) numArray1[checked ((int) numArray1[8] + 9)] | (int) this.crc[1] != (int) numArray1[checked ((int) numArray1[8] + 10)]) & this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new CRCCheckFailedException("Response CRC check failed");
          }
          checked { ++this.countRetries; }
          return this.ReadCoils(startingAddress, quantity);
        }
        if (!this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new TimeoutException("No Response from Modbus Slave");
          }
          checked { ++this.countRetries; }
          return this.ReadCoils(startingAddress, quantity);
        }
      }
      bool[] flagArray = new bool[quantity];
      int index = 0;
      while (index < quantity)
      {
        int num = (int) numArray1[checked (9 + unchecked (index / 8))];
        int int32 = Convert.ToInt32(Math.Pow(2.0, (double) (index % 8)));
        flagArray[index] = Convert.ToBoolean((num & int32) / int32);
        checked { ++index; }
      }
      return flagArray;
    }

    public int[] ReadHoldingRegisters(int startingAddress, int quantity, string mqttBrokerAddress)
    {
      int[] numArray = this.ReadHoldingRegisters(startingAddress, quantity);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (this.MqttPushOnChange && this.mqttHoldingRegistersOldValues == null)
        this.mqttHoldingRegistersOldValues = new int[(int) ushort.MaxValue];
      int index = 0;
      while (index < numArray.Length)
      {
        if (this.mqttHoldingRegistersOldValues == null || this.mqttHoldingRegistersOldValues[index] != numArray[index])
        {
          stringList1.Add(this.mqttRootTopic + "/holdingregisters/" + checked (index + startingAddress).ToString());
          stringList2.Add(numArray[index].ToString());
          this.mqttHoldingRegistersOldValues[index] = numArray[index];
        }
        checked { ++index; }
      }
      if (this.easyModbus2Mqtt == null)
        this.easyModbus2Mqtt = new EasyModbus2Mqtt();
      this.easyModbus2Mqtt.MqttBrokerPort = this.MqttBrokerPort;
      this.easyModbus2Mqtt.MqttUserName = this.MqttUserName;
      this.easyModbus2Mqtt.MqttPassword = this.MqttPassword;
      this.easyModbus2Mqtt.RetainMessages = this.mqttRetainMessages;
      this.easyModbus2Mqtt.publish(stringList1.ToArray(), stringList2.ToArray(), mqttBrokerAddress);
      return numArray;
    }

    public int[] ReadHoldingRegisters(int startingAddress, int quantity)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC3 (Read Holding Registers from Master device), StartingAddress: " + (object) startingAddress + ", Quantity: " + (object) quantity, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      if (startingAddress > (int) ushort.MaxValue | quantity > 125)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ArgumentException Throwed", DateTime.Now);
        throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 125");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 3;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      this.quantity = BitConverter.GetBytes(quantity);
      byte[] numArray1 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], this.quantity[1], this.quantity[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      numArray1[12] = this.crc[0];
      numArray1[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = checked (5 + 2 * quantity);
        this.serialport.Write(numArray1, 6, 8);
        if (this.debug)
        {
          byte[] numArray2 = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray1 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
            maxValue = numArray1[6];
          }
          else
            break;
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[256];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 131 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 131 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 131 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 131 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport != null)
      {
        this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) ((int) numArray1[8] + 3)), 6));
        if (((int) this.crc[0] != (int) numArray1[checked ((int) numArray1[8] + 9)] | (int) this.crc[1] != (int) numArray1[checked ((int) numArray1[8] + 10)]) & this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new CRCCheckFailedException("Response CRC check failed");
          }
          checked { ++this.countRetries; }
          return this.ReadHoldingRegisters(startingAddress, quantity);
        }
        if (!this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new TimeoutException("No Response from Modbus Slave");
          }
          checked { ++this.countRetries; }
          return this.ReadHoldingRegisters(startingAddress, quantity);
        }
      }
      int[] numArray3 = new int[quantity];
      int index = 0;
      while (index < quantity)
      {
        byte num1 = numArray1[checked (9 + index * 2)];
        byte num2 = numArray1[checked (9 + index * 2 + 1)];
        numArray1[checked (9 + index * 2)] = num2;
        numArray1[checked (9 + index * 2 + 1)] = num1;
        numArray3[index] = (int) BitConverter.ToInt16(numArray1, checked (9 + index * 2));
        checked { ++index; }
      }
      return numArray3;
    }

    public int[] ReadInputRegisters(int startingAddress, int quantity, string mqttBrokerAddress)
    {
      int[] numArray = this.ReadInputRegisters(startingAddress, quantity);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      if (this.MqttPushOnChange && this.mqttInputRegistersOldValues == null)
        this.mqttInputRegistersOldValues = new int[(int) ushort.MaxValue];
      int index = 0;
      while (index < numArray.Length)
      {
        if (this.mqttInputRegistersOldValues == null || this.mqttInputRegistersOldValues[index] != numArray[index])
        {
          stringList1.Add(this.mqttRootTopic + "/inputregisters/" + checked (index + startingAddress).ToString());
          stringList2.Add(numArray[index].ToString());
          this.mqttInputRegistersOldValues[index] = numArray[index];
        }
        checked { ++index; }
      }
      if (this.easyModbus2Mqtt == null)
        this.easyModbus2Mqtt = new EasyModbus2Mqtt();
      this.easyModbus2Mqtt.MqttBrokerPort = this.MqttBrokerPort;
      this.easyModbus2Mqtt.MqttUserName = this.MqttUserName;
      this.easyModbus2Mqtt.MqttPassword = this.MqttPassword;
      this.easyModbus2Mqtt.RetainMessages = this.mqttRetainMessages;
      this.easyModbus2Mqtt.publish(stringList1.ToArray(), stringList2.ToArray(), mqttBrokerAddress);
      return numArray;
    }

    public int[] ReadInputRegisters(int startingAddress, int quantity)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC4 (Read Input Registers from Master device), StartingAddress: " + (object) startingAddress + ", Quantity: " + (object) quantity, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      if (startingAddress > (int) ushort.MaxValue | quantity > 125)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ArgumentException Throwed", DateTime.Now);
        throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 125");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 4;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      this.quantity = BitConverter.GetBytes(quantity);
      byte[] numArray1 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], this.quantity[1], this.quantity[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      numArray1[12] = this.crc[0];
      numArray1[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = checked (5 + 2 * quantity);
        this.serialport.Write(numArray1, 6, 8);
        if (this.debug)
        {
          byte[] numArray2 = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray1 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
            maxValue = numArray1[6];
          }
          else
            break;
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[2100];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 132 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 132 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 132 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 132 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport != null)
      {
        this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) ((int) numArray1[8] + 3)), 6));
        if (((int) this.crc[0] != (int) numArray1[checked ((int) numArray1[8] + 9)] | (int) this.crc[1] != (int) numArray1[checked ((int) numArray1[8] + 10)]) & this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new CRCCheckFailedException("Response CRC check failed");
          }
          checked { ++this.countRetries; }
          return this.ReadInputRegisters(startingAddress, quantity);
        }
        if (!this.dataReceived)
        {
          if (this.debug)
            StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
          if (this.NumberOfRetries <= this.countRetries)
          {
            this.countRetries = 0;
            throw new TimeoutException("No Response from Modbus Slave");
          }
          checked { ++this.countRetries; }
          return this.ReadInputRegisters(startingAddress, quantity);
        }
      }
      int[] numArray3 = new int[quantity];
      int index = 0;
      while (index < quantity)
      {
        byte num1 = numArray1[checked (9 + index * 2)];
        byte num2 = numArray1[checked (9 + index * 2 + 1)];
        numArray1[checked (9 + index * 2)] = num2;
        numArray1[checked (9 + index * 2 + 1)] = num1;
        numArray3[index] = (int) BitConverter.ToInt16(numArray1, checked (9 + index * 2));
        checked { ++index; }
      }
      return numArray3;
    }

    public void WriteSingleCoil(int startingAddress, bool value)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC5 (Write single coil to Master device), StartingAddress: " + (object) startingAddress + ", Value: " + value.ToString(), DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      byte[] numArray1 = new byte[2];
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 5;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      byte[] numArray2 = !value ? BitConverter.GetBytes(0) : BitConverter.GetBytes(65280);
      byte[] numArray3 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], numArray2[1], numArray2[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray3, (ushort) 6, 6));
      numArray3[12] = this.crc[0];
      numArray3[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = 8;
        this.serialport.Write(numArray3, 6, 8);
        if (this.debug)
        {
          byte[] numArray4 = new byte[8];
          Array.Copy((Array) numArray3, 6, (Array) numArray4, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray4), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray3, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray3 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray3 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray3, 6, this.readBuffer.Length);
            maxValue = numArray3[6];
            this.countRetries = 0;
          }
          else
            break;
        }
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray3, checked (numArray3.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray3 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray3, 0, checked (numArray3.Length - 2));
          if (this.debug)
          {
            byte[] numArray4 = new byte[checked (numArray3.Length - 2)];
            Array.Copy((Array) numArray3, 0, (Array) numArray4, 0, checked (numArray3.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray4), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray3.Length - 2)];
            Array.Copy((Array) numArray3, 0, (Array) this.sendData, 0, checked (numArray3.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray3 = new byte[2100];
          int length = this.stream.Read(numArray3, 0, numArray3.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray3, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray3[7] == (byte) 133 & numArray3[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray3[7] == (byte) 133 & numArray3[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray3[7] == (byte) 133 & numArray3[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray3[7] == (byte) 133 & numArray3[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport == null)
        return;
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray3, (ushort) 6, 6));
      if (((int) this.crc[0] != (int) numArray3[12] | (int) this.crc[1] != (int) numArray3[13]) & this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new CRCCheckFailedException("Response CRC check failed");
        }
        checked { ++this.countRetries; }
        this.WriteSingleCoil(startingAddress, value);
      }
      else if (!this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new TimeoutException("No Response from Modbus Slave");
        }
        checked { ++this.countRetries; }
        this.WriteSingleCoil(startingAddress, value);
      }
    }

    public void WriteSingleRegister(int startingAddress, int value)
    {
      if (this.debug)
        StoreLogData.Instance.Store("FC6 (Write single register to Master device), StartingAddress: " + (object) startingAddress + ", Value: " + (object) value, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      byte[] numArray1 = new byte[2];
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 6;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      byte[] bytes = BitConverter.GetBytes(value);
      byte[] numArray2 = new byte[14]{ this.transactionIdentifier[1], this.transactionIdentifier[0], this.protocolIdentifier[1], this.protocolIdentifier[0], this.length[1], this.length[0], this.unitIdentifier, this.functionCode, this.startingAddress[1], this.startingAddress[0], bytes[1], bytes[0], this.crc[0], this.crc[1] };
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray2, (ushort) 6, 6));
      numArray2[12] = this.crc[0];
      numArray2[13] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = 8;
        this.serialport.Write(numArray2, 6, 8);
        if (this.debug)
        {
          byte[] numArray3 = new byte[8];
          Array.Copy((Array) numArray2, 6, (Array) numArray3, 0, 8);
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[8];
          Array.Copy((Array) numArray2, 6, (Array) this.sendData, 0, 8);
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray2 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now1 = DateTime.Now;
        byte maxValue = byte.MaxValue;
        while (true)
        {
          int num1 = (int) maxValue != (int) this.unitIdentifier ? 1 : 0;
          DateTime now2 = DateTime.Now;
          int num2 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
          if ((num1 & num2) != 0)
          {
            while (true)
            {
              int num3 = !this.dataReceived ? 1 : 0;
              now2 = DateTime.Now;
              int num4 = checked (now2.Ticks - now1.Ticks) <= checked (10000L * (long) this.connectTimeout) ? 1 : 0;
              if ((num3 & num4) != 0)
                Thread.Sleep(1);
              else
                break;
            }
            numArray2 = new byte[2100];
            Array.Copy((Array) this.readBuffer, 0, (Array) numArray2, 6, this.readBuffer.Length);
            maxValue = numArray2[6];
          }
          else
            break;
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray2 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray2, checked (numArray2.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray2 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray2, 0, checked (numArray2.Length - 2));
          if (this.debug)
          {
            byte[] numArray3 = new byte[checked (numArray2.Length - 2)];
            Array.Copy((Array) numArray2, 0, (Array) numArray3, 0, checked (numArray2.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray3), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray2.Length - 2)];
            Array.Copy((Array) numArray2, 0, (Array) this.sendData, 0, checked (numArray2.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray2 = new byte[2100];
          int length = this.stream.Read(numArray2, 0, numArray2.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray2, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray2[7] == (byte) 134 & numArray2[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray2[7] == (byte) 134 & numArray2[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray2[7] == (byte) 134 & numArray2[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray2[7] == (byte) 134 & numArray2[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport == null)
        return;
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray2, (ushort) 6, 6));
      if (((int) this.crc[0] != (int) numArray2[12] | (int) this.crc[1] != (int) numArray2[13]) & this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new CRCCheckFailedException("Response CRC check failed");
        }
        checked { ++this.countRetries; }
        this.WriteSingleRegister(startingAddress, value);
      }
      else if (!this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new TimeoutException("No Response from Modbus Slave");
        }
        checked { ++this.countRetries; }
        this.WriteSingleRegister(startingAddress, value);
      }
    }

    public void WriteMultipleCoils(int startingAddress, bool[] values)
    {
      string str = "";
      int index1 = 0;
      while (index1 < values.Length)
      {
        str = str + values[index1].ToString() + " ";
        checked { ++index1; }
      }
      if (this.debug)
        StoreLogData.Instance.Store("FC15 (Write multiple coils to Master device), StartingAddress: " + (object) startingAddress + ", Values: " + str, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      byte num1 = values.Length % 8 != 0 ? checked ((byte) (unchecked (values.Length / 8) + 1)) : checked ((byte) unchecked (values.Length / 8));
      byte[] bytes = BitConverter.GetBytes(values.Length);
      byte num2 = 0;
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(checked (7 + (int) num1));
      this.functionCode = (byte) 15;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
            byte[] numArray1;
            if (unchecked(values.Length % 8 != 0))
            {
                numArray1 = new byte[checked(16 + unchecked(values.Length / 8))];
            }
            else
            {
                numArray1 = new byte[checked(16 + (unchecked(values.Length / 8) - 1))];
            }
            //       new byte[checked (16 +  ? unchecked (values.Length / 8) : unchecked (values.Length / 8) - 1)];
            numArray1[0] = this.transactionIdentifier[1];
      numArray1[1] = this.transactionIdentifier[0];
      numArray1[2] = this.protocolIdentifier[1];
      numArray1[3] = this.protocolIdentifier[0];
      numArray1[4] = this.length[1];
      numArray1[5] = this.length[0];
      numArray1[6] = this.unitIdentifier;
      numArray1[7] = this.functionCode;
      numArray1[8] = this.startingAddress[1];
      numArray1[9] = this.startingAddress[0];
      numArray1[10] = bytes[1];
      numArray1[11] = bytes[0];
      numArray1[12] = num1;
      int index2 = 0;
      while (index2 < values.Length)
      {
        if (index2 % 8 == 0)
          num2 = (byte) 0;
        num2 = checked ((byte) ((!values[index2] ? 0 : 1) << unchecked (index2 % 8) | (int) num2));
        numArray1[checked (13 + unchecked (index2 / 8))] = num2;
        checked { ++index2; }
      }
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) (numArray1.Length - 8)), 6));
      numArray1[checked (numArray1.Length - 2)] = this.crc[0];
      numArray1[checked (numArray1.Length - 1)] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = 8;
        this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
        if (this.debug)
        {
          byte[] numArray2 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, checked (numArray1.Length - 6));
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now = DateTime.Now;
        byte maxValue;
        for (maxValue = byte.MaxValue; (int) maxValue != (int) this.unitIdentifier & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout); maxValue = numArray1[6])
        {
          while (!this.dataReceived & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout))
            Thread.Sleep(1);
          numArray1 = new byte[2100];
          Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[2100];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 143 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 143 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 143 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 143 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport == null)
        return;
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      if (((int) this.crc[0] != (int) numArray1[12] | (int) this.crc[1] != (int) numArray1[13]) & this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new CRCCheckFailedException("Response CRC check failed");
        }
        checked { ++this.countRetries; }
        this.WriteMultipleCoils(startingAddress, values);
      }
      else if (!this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new TimeoutException("No Response from Modbus Slave");
        }
        checked { ++this.countRetries; }
        this.WriteMultipleCoils(startingAddress, values);
      }
    }

    public void WriteMultipleRegisters(int startingAddress, int[] values)
    {
      string str = "";
      int index1 = 0;
      while (index1 < values.Length)
      {
        str = str + (object) values[index1] + " ";
        checked { ++index1; }
      }
      if (this.debug)
        StoreLogData.Instance.Store("FC16 (Write multiple Registers to Server device), StartingAddress: " + (object) startingAddress + ", Values: " + str, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      byte num = checked ((byte) (values.Length * 2));
      byte[] bytes1 = BitConverter.GetBytes(values.Length);
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(checked (7 + values.Length * 2));
      this.functionCode = (byte) 16;
      this.startingAddress = BitConverter.GetBytes(startingAddress);
      byte[] numArray1 = new byte[checked (15 + values.Length * 2)];
      numArray1[0] = this.transactionIdentifier[1];
      numArray1[1] = this.transactionIdentifier[0];
      numArray1[2] = this.protocolIdentifier[1];
      numArray1[3] = this.protocolIdentifier[0];
      numArray1[4] = this.length[1];
      numArray1[5] = this.length[0];
      numArray1[6] = this.unitIdentifier;
      numArray1[7] = this.functionCode;
      numArray1[8] = this.startingAddress[1];
      numArray1[9] = this.startingAddress[0];
      numArray1[10] = bytes1[1];
      numArray1[11] = bytes1[0];
      numArray1[12] = num;
      int index2 = 0;
      while (index2 < values.Length)
      {
        byte[] bytes2 = BitConverter.GetBytes(values[index2]);
        numArray1[checked (13 + index2 * 2)] = bytes2[1];
        numArray1[checked (14 + index2 * 2)] = bytes2[0];
        checked { ++index2; }
      }
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, checked ((ushort) (numArray1.Length - 8)), 6));
      numArray1[checked (numArray1.Length - 2)] = this.crc[0];
      numArray1[checked (numArray1.Length - 1)] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = 8;
        this.serialport.Write(numArray1, 6, checked (numArray1.Length - 6));
        if (this.debug)
        {
          byte[] numArray2 = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) numArray2, 0, checked (numArray1.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[checked (numArray1.Length - 6)];
          Array.Copy((Array) numArray1, 6, (Array) this.sendData, 0, checked (numArray1.Length - 6));
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray1 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now = DateTime.Now;
        byte maxValue;
        for (maxValue = byte.MaxValue; (int) maxValue != (int) this.unitIdentifier & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout); maxValue = numArray1[6])
        {
          while (!this.dataReceived & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout))
            Thread.Sleep(1);
          numArray1 = new byte[2100];
          Array.Copy((Array) this.readBuffer, 0, (Array) numArray1, 6, this.readBuffer.Length);
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray1 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray1, checked (numArray1.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray1 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray1, 0, checked (numArray1.Length - 2));
          if (this.debug)
          {
            byte[] numArray2 = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, checked (numArray1.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray2), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray1.Length - 2)];
            Array.Copy((Array) numArray1, 0, (Array) this.sendData, 0, checked (numArray1.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray1 = new byte[2100];
          int length = this.stream.Read(numArray1, 0, numArray1.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray1, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray1[7] == (byte) 144 & numArray1[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray1[7] == (byte) 144 & numArray1[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray1[7] == (byte) 144 & numArray1[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray1[7] == (byte) 144 & numArray1[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      if (this.serialport == null)
        return;
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray1, (ushort) 6, 6));
      if (((int) this.crc[0] != (int) numArray1[12] | (int) this.crc[1] != (int) numArray1[13]) & this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("CRCCheckFailedException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new CRCCheckFailedException("Response CRC check failed");
        }
        checked { ++this.countRetries; }
        this.WriteMultipleRegisters(startingAddress, values);
      }
      else if (!this.dataReceived)
      {
        if (this.debug)
          StoreLogData.Instance.Store("TimeoutException Throwed", DateTime.Now);
        if (this.NumberOfRetries <= this.countRetries)
        {
          this.countRetries = 0;
          throw new TimeoutException("No Response from Modbus Slave");
        }
        checked { ++this.countRetries; }
        this.WriteMultipleRegisters(startingAddress, values);
      }
    }

    public int[] ReadWriteMultipleRegisters(int startingAddressRead, int quantityRead, int startingAddressWrite, int[] values)
    {
      string str = "";
      int index1 = 0;
      while (index1 < values.Length)
      {
        str = str + (object) values[index1] + " ";
        checked { ++index1; }
      }
      if (this.debug)
        StoreLogData.Instance.Store("FC23 (Read and Write multiple Registers to Server device), StartingAddress Read: " + (object) startingAddressRead + ", Quantity Read: " + (object) quantityRead + ", startingAddressWrite: " + (object) startingAddressWrite + ", Values: " + str, DateTime.Now);
      checked { ++this.transactionIdentifierInternal; }
      byte[] numArray1 = new byte[2];
      byte[] numArray2 = new byte[2];
      byte[] numArray3 = new byte[2];
      byte[] numArray4 = new byte[2];
      if (this.serialport != null && !this.serialport.IsOpen)
      {
        if (this.debug)
          StoreLogData.Instance.Store("SerialPortNotOpenedException Throwed", DateTime.Now);
        throw new SerialPortNotOpenedException("serial port not opened");
      }
      if (this.tcpClient == null & !this.udpFlag & this.serialport == null)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ConnectionException Throwed", DateTime.Now);
        throw new ConnectionException("connection error");
      }
      if (startingAddressRead > (int) ushort.MaxValue | quantityRead > 125 | startingAddressWrite > (int) ushort.MaxValue | values.Length > 121)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ArgumentException Throwed", DateTime.Now);
        throw new ArgumentException("Starting address must be 0 - 65535; quantity must be 0 - 2000");
      }
      this.transactionIdentifier = BitConverter.GetBytes(this.transactionIdentifierInternal);
      this.protocolIdentifier = BitConverter.GetBytes(0);
      this.length = BitConverter.GetBytes(6);
      this.functionCode = (byte) 23;
      byte[] bytes1 = BitConverter.GetBytes(startingAddressRead);
      byte[] bytes2 = BitConverter.GetBytes(quantityRead);
      byte[] bytes3 = BitConverter.GetBytes(startingAddressWrite);
      byte[] bytes4 = BitConverter.GetBytes(values.Length);
      byte num1 = Convert.ToByte(checked (values.Length * 2));
      byte[] numArray5 = new byte[checked (19 + values.Length * 2)];
      numArray5[0] = this.transactionIdentifier[1];
      numArray5[1] = this.transactionIdentifier[0];
      numArray5[2] = this.protocolIdentifier[1];
      numArray5[3] = this.protocolIdentifier[0];
      numArray5[4] = this.length[1];
      numArray5[5] = this.length[0];
      numArray5[6] = this.unitIdentifier;
      numArray5[7] = this.functionCode;
      numArray5[8] = bytes1[1];
      numArray5[9] = bytes1[0];
      numArray5[10] = bytes2[1];
      numArray5[11] = bytes2[0];
      numArray5[12] = bytes3[1];
      numArray5[13] = bytes3[0];
      numArray5[14] = bytes4[1];
      numArray5[15] = bytes4[0];
      numArray5[16] = num1;
      int index2 = 0;
      while (index2 < values.Length)
      {
        byte[] bytes5 = BitConverter.GetBytes(values[index2]);
        numArray5[checked (17 + index2 * 2)] = bytes5[1];
        numArray5[checked (18 + index2 * 2)] = bytes5[0];
        checked { ++index2; }
      }
      this.crc = BitConverter.GetBytes(ModbusClient.calculateCRC(numArray5, checked ((ushort) (numArray5.Length - 8)), 6));
      numArray5[checked (numArray5.Length - 2)] = this.crc[0];
      numArray5[checked (numArray5.Length - 1)] = this.crc[1];
      if (this.serialport != null)
      {
        this.dataReceived = false;
        this.bytesToRead = checked (5 + 2 * quantityRead);
        this.serialport.Write(numArray5, 6, checked (numArray5.Length - 6));
        if (this.debug)
        {
          byte[] numArray6 = new byte[checked (numArray5.Length - 6)];
          Array.Copy((Array) numArray5, 6, (Array) numArray6, 0, checked (numArray5.Length - 6));
          if (this.debug)
            StoreLogData.Instance.Store("Send Serial-Data: " + BitConverter.ToString(numArray6), DateTime.Now);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.SendDataChanged != null)
        {
          this.sendData = new byte[checked (numArray5.Length - 6)];
          Array.Copy((Array) numArray5, 6, (Array) this.sendData, 0, checked (numArray5.Length - 6));
          // ISSUE: reference to a compiler-generated field
          this.SendDataChanged((object) this);
        }
        numArray5 = new byte[2100];
        this.readBuffer = new byte[256];
        DateTime now = DateTime.Now;
        byte maxValue;
        for (maxValue = byte.MaxValue; (int) maxValue != (int) this.unitIdentifier & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout); maxValue = numArray5[6])
        {
          while (!this.dataReceived & checked (DateTime.Now.Ticks - now.Ticks) <= checked (10000L * (long) this.connectTimeout))
            Thread.Sleep(1);
          numArray5 = new byte[2100];
          Array.Copy((Array) this.readBuffer, 0, (Array) numArray5, 6, this.readBuffer.Length);
        }
        if ((int) maxValue != (int) this.unitIdentifier)
          numArray5 = new byte[2100];
        else
          this.countRetries = 0;
      }
      else if (this.tcpClient.Client.Connected | this.udpFlag)
      {
        if (this.udpFlag)
        {
          UdpClient udpClient = new UdpClient();
          IPEndPoint endPoint = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.port);
          udpClient.Send(numArray5, checked (numArray5.Length - 2), endPoint);
          this.portOut = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;
          udpClient.Client.ReceiveTimeout = 5000;
          IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.ipAddress), this.portOut);
          numArray5 = udpClient.Receive(ref remoteEP);
        }
        else
        {
          this.stream.Write(numArray5, 0, checked (numArray5.Length - 2));
          if (this.debug)
          {
            byte[] numArray6 = new byte[checked (numArray5.Length - 2)];
            Array.Copy((Array) numArray5, 0, (Array) numArray6, 0, checked (numArray5.Length - 2));
            if (this.debug)
              StoreLogData.Instance.Store("Send ModbusTCP-Data: " + BitConverter.ToString(numArray6), DateTime.Now);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.SendDataChanged != null)
          {
            this.sendData = new byte[checked (numArray5.Length - 2)];
            Array.Copy((Array) numArray5, 0, (Array) this.sendData, 0, checked (numArray5.Length - 2));
            // ISSUE: reference to a compiler-generated field
            this.SendDataChanged((object) this);
          }
          numArray5 = new byte[2100];
          int length = this.stream.Read(numArray5, 0, numArray5.Length);
          // ISSUE: reference to a compiler-generated field
          if (this.ReceiveDataChanged != null)
          {
            this.receiveData = new byte[length];
            Array.Copy((Array) numArray5, 0, (Array) this.receiveData, 0, length);
            if (this.debug)
              StoreLogData.Instance.Store("Receive ModbusTCP-Data: " + BitConverter.ToString(this.receiveData), DateTime.Now);
            // ISSUE: reference to a compiler-generated field
            this.ReceiveDataChanged((object) this);
          }
        }
      }
      if (numArray5[7] == (byte) 151 & numArray5[8] == (byte) 1)
      {
        if (this.debug)
          StoreLogData.Instance.Store("FunctionCodeNotSupportedException Throwed", DateTime.Now);
        throw new FunctionCodeNotSupportedException("Function code not supported by master");
      }
      if (numArray5[7] == (byte) 151 & numArray5[8] == (byte) 2)
      {
        if (this.debug)
          StoreLogData.Instance.Store("StartingAddressInvalidException Throwed", DateTime.Now);
        throw new StartingAddressInvalidException("Starting address invalid or starting address + quantity invalid");
      }
      if (numArray5[7] == (byte) 151 & numArray5[8] == (byte) 3)
      {
        if (this.debug)
          StoreLogData.Instance.Store("QuantityInvalidException Throwed", DateTime.Now);
        throw new QuantityInvalidException("quantity invalid");
      }
      if (numArray5[7] == (byte) 151 & numArray5[8] == (byte) 4)
      {
        if (this.debug)
          StoreLogData.Instance.Store("ModbusException Throwed", DateTime.Now);
        throw new ModbusException("error reading");
      }
      int[] numArray7 = new int[quantityRead];
      int index3 = 0;
      while (index3 < quantityRead)
      {
        byte num2 = numArray5[checked (9 + index3 * 2)];
        byte num3 = numArray5[checked (9 + index3 * 2 + 1)];
        numArray5[checked (9 + index3 * 2)] = num3;
        numArray5[checked (9 + index3 * 2 + 1)] = num2;
        numArray7[index3] = (int) BitConverter.ToInt16(numArray5, checked (9 + index3 * 2));
        checked { ++index3; }
      }
      return numArray7;
    }

    public void Disconnect()
    {
      if (this.debug)
        StoreLogData.Instance.Store(nameof (Disconnect), DateTime.Now);
      if (this.serialport != null)
      {
        if (this.serialport.IsOpen & !this.receiveActive)
          this.serialport.Close();
        // ISSUE: reference to a compiler-generated field
        if (this.ConnectedChanged == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.ConnectedChanged((object) this);
      }
      else
      {
        if (this.stream != null)
          this.stream.Close();
        if (this.tcpClient != null)
          this.tcpClient.Close();
        this.connected = false;
        // ISSUE: reference to a compiler-generated field
        if (this.ConnectedChanged == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.ConnectedChanged((object) this);
      }
    }

    ~ModbusClient()
    {
      if (this.debug)
        StoreLogData.Instance.Store("Destructor called - automatically disconnect", DateTime.Now);
      if (this.serialport != null)
      {
        if (!this.serialport.IsOpen)
          return;
        this.serialport.Close();
      }
      else
      {
        if (!(this.tcpClient != null & !this.udpFlag))
          return;
        if (this.stream != null)
          this.stream.Close();
        this.tcpClient.Close();
      }
    }

    public bool Connected
    {
      get
      {
        if (this.serialport != null)
          return this.serialport.IsOpen;
        if (this.udpFlag & this.tcpClient != null)
          return true;
        if (this.tcpClient == null)
          return false;
        return this.connected;
      }
    }

    public bool Available(int timeout)
    {
      Ping ping = new Ping();
      System.Net.IPAddress address = System.Net.IPAddress.Parse(this.ipAddress);
      byte[] bytes = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
      return ping.Send(address, timeout, bytes).Status == IPStatus.Success;
    }

    public string IPAddress
    {
      get
      {
        return this.ipAddress;
      }
      set
      {
        this.ipAddress = value;
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

    public int Baudrate
    {
      get
      {
        return this.baudRate;
      }
      set
      {
        this.baudRate = value;
      }
    }

    public Parity Parity
    {
      get
      {
        if (this.serialport != null)
          return this.parity;
        return Parity.Even;
      }
      set
      {
        if (this.serialport == null)
          return;
        this.parity = value;
      }
    }

    public StopBits StopBits
    {
      get
      {
        if (this.serialport != null)
          return this.stopBits;
        return StopBits.One;
      }
      set
      {
        if (this.serialport == null)
          return;
        this.stopBits = value;
      }
    }

    public int ConnectionTimeout
    {
      get
      {
        return this.connectTimeout;
      }
      set
      {
        this.connectTimeout = value;
      }
    }

    public string SerialPort
    {
      get
      {
        return this.serialport.PortName;
      }
      set
      {
        if (value == null)
        {
          this.serialport = (System.IO.Ports.SerialPort) null;
        }
        else
        {
          if (this.serialport != null)
            this.serialport.Close();
          this.serialport = new System.IO.Ports.SerialPort();
          this.serialport.PortName = value;
          this.serialport.BaudRate = this.baudRate;
          this.serialport.Parity = this.parity;
          this.serialport.StopBits = this.stopBits;
          this.serialport.WriteTimeout = 10000;
          this.serialport.ReadTimeout = this.connectTimeout;
          this.serialport.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
        }
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

    public bool MqttRetainMessages
    {
      get
      {
        return this.mqttRetainMessages;
      }
      set
      {
        this.mqttRetainMessages = value;
      }
    }

    public enum RegisterOrder
    {
      LowHigh,
      HighLow,
    }

    public delegate void ReceiveDataChangedHandler(object sender);

    public delegate void SendDataChangedHandler(object sender);

    public delegate void ConnectedChangedHandler(object sender);
  }
}
