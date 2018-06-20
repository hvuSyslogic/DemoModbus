// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.MqttNetworkChannel
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace uPLibrary.Networking.M2Mqtt
{
  public class MqttNetworkChannel : IMqttNetworkChannel
  {
    private readonly RemoteCertificateValidationCallback userCertificateValidationCallback;
    private readonly LocalCertificateSelectionCallback userCertificateSelectionCallback;
    private string remoteHostName;
    private IPAddress remoteIpAddress;
    private int remotePort;
    private Socket socket;
    private bool secure;
    private X509Certificate caCert;
    private X509Certificate serverCert;
    private X509Certificate clientCert;
    private MqttSslProtocols sslProtocol;
    private SslStream sslStream;
    private NetworkStream netStream;

    public string RemoteHostName
    {
      get
      {
        return this.remoteHostName;
      }
    }

    public IPAddress RemoteIpAddress
    {
      get
      {
        return this.remoteIpAddress;
      }
    }

    public int RemotePort
    {
      get
      {
        return this.remotePort;
      }
    }

    public bool DataAvailable
    {
      get
      {
        if (this.secure)
          return this.netStream.DataAvailable;
        return this.socket.Available > 0;
      }
    }

    public MqttNetworkChannel(Socket socket)
      : this(socket, false, (X509Certificate) null, MqttSslProtocols.None, (RemoteCertificateValidationCallback) null, (LocalCertificateSelectionCallback) null)
    {
    }

    public MqttNetworkChannel(Socket socket, bool secure, X509Certificate serverCert, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
    {
      this.socket = socket;
      this.secure = secure;
      this.serverCert = serverCert;
      this.sslProtocol = sslProtocol;
      this.userCertificateValidationCallback = userCertificateValidationCallback;
      this.userCertificateSelectionCallback = userCertificateSelectionCallback;
    }

    public MqttNetworkChannel(string remoteHostName, int remotePort)
      : this(remoteHostName, remotePort, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None, (RemoteCertificateValidationCallback) null, (LocalCertificateSelectionCallback) null)
    {
    }

    public MqttNetworkChannel(string remoteHostName, int remotePort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
    {
      IPAddress ipAddress = (IPAddress) null;
      try
      {
        ipAddress = IPAddress.Parse(remoteHostName);
      }
      catch
      {
      }
      if (ipAddress == null)
      {
        IPHostEntry hostEntry = Dns.GetHostEntry(remoteHostName);
        if (hostEntry == null || (uint) hostEntry.AddressList.Length <= 0U)
          throw new Exception("No address found for the remote host name");
        int index = 0;
        while (hostEntry.AddressList[index] == null)
          checked { ++index; }
        ipAddress = hostEntry.AddressList[index];
      }
      this.remoteHostName = remoteHostName;
      this.remoteIpAddress = ipAddress;
      this.remotePort = remotePort;
      this.secure = secure;
      this.caCert = caCert;
      this.clientCert = clientCert;
      this.sslProtocol = sslProtocol;
      this.userCertificateValidationCallback = userCertificateValidationCallback;
      this.userCertificateSelectionCallback = userCertificateSelectionCallback;
    }

    public void Connect()
    {
      this.socket = new Socket(this.remoteIpAddress.GetAddressFamily(), SocketType.Stream, ProtocolType.Tcp);
      this.socket.Connect((EndPoint) new IPEndPoint(this.remoteIpAddress, this.remotePort));
      if (!this.secure)
        return;
      this.netStream = new NetworkStream(this.socket);
      this.sslStream = new SslStream((Stream) this.netStream, false, this.userCertificateValidationCallback, this.userCertificateSelectionCallback);
      X509CertificateCollection clientCertificates = (X509CertificateCollection) null;
      if (this.clientCert != null)
        clientCertificates = new X509CertificateCollection(new X509Certificate[1]
        {
          this.clientCert
        });
      this.sslStream.AuthenticateAsClient(this.remoteHostName, clientCertificates, MqttSslUtility.ToSslPlatformEnum(this.sslProtocol), false);
    }

    public int Send(byte[] buffer)
    {
      if (!this.secure)
        return this.socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
      this.sslStream.Write(buffer, 0, buffer.Length);
      this.sslStream.Flush();
      return buffer.Length;
    }

    public int Receive(byte[] buffer)
    {
      if (this.secure)
      {
        int offset = 0;
        while (offset < buffer.Length)
        {
          int num = this.sslStream.Read(buffer, offset, checked (buffer.Length - offset));
          if (num == 0)
            return 0;
          checked { offset += num; }
        }
        return buffer.Length;
      }
      int offset1 = 0;
      while (offset1 < buffer.Length)
      {
        int num = this.socket.Receive(buffer, offset1, checked (buffer.Length - offset1), SocketFlags.None);
        if (num == 0)
          return 0;
        checked { offset1 += num; }
      }
      return buffer.Length;
    }

    public int Receive(byte[] buffer, int timeout)
    {
      if (this.socket.Poll(checked (timeout * 1000), SelectMode.SelectRead))
        return this.Receive(buffer);
      return 0;
    }

    public void Close()
    {
      if (this.secure)
      {
        this.netStream.Close();
        this.sslStream.Close();
      }
      this.socket.Close();
    }

    public void Accept()
    {
      if (!this.secure)
        return;
      this.netStream = new NetworkStream(this.socket);
      this.sslStream = new SslStream((Stream) this.netStream, false, this.userCertificateValidationCallback, this.userCertificateSelectionCallback);
      this.sslStream.AuthenticateAsServer(this.serverCert, false, MqttSslUtility.ToSslPlatformEnum(this.sslProtocol), false);
    }
  }
}
