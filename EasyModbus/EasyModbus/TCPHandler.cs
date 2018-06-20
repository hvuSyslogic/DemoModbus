// Decompiled with JetBrains decompiler
// Type: EasyModbus.TCPHandler
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace EasyModbus
{
  internal class TCPHandler
  {
    private TcpListener server = (TcpListener) null;
    private List<TCPHandler.Client> tcpClientLastRequestList = new List<TCPHandler.Client>();
    public string ipAddress = (string) null;

    public event TCPHandler.DataChanged dataChanged;

    public event TCPHandler.NumberOfClientsChanged numberOfClientsChanged;

    public int NumberOfConnectedClients { get; set; }

    public TCPHandler(int port)
    {
      this.server = new TcpListener(IPAddress.Any, port);
      this.server.Start();
      this.server.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), (object) null);
    }

    public TCPHandler(string ipAddress, int port)
    {
      this.ipAddress = ipAddress;
      this.server = new TcpListener(IPAddress.Any, port);
      this.server.Start();
      this.server.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), (object) null);
    }

    private void AcceptTcpClientCallback(IAsyncResult asyncResult)
    {
      TcpClient tcpClient = new TcpClient();
      try
      {
        tcpClient = this.server.EndAcceptTcpClient(asyncResult);
        tcpClient.ReceiveTimeout = 4000;
        if (this.ipAddress != null)
        {
          if (tcpClient.Client.RemoteEndPoint.ToString().Split(':')[0] != this.ipAddress)
          {
            tcpClient.Client.Disconnect(false);
            return;
          }
        }
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.server.BeginAcceptTcpClient(new AsyncCallback(this.AcceptTcpClientCallback), (object) null);
        TCPHandler.Client client = new TCPHandler.Client(tcpClient);
        NetworkStream networkStream = client.NetworkStream;
        networkStream.ReadTimeout = 4000;
        networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, new AsyncCallback(this.ReadCallback), (object) client);
      }
      catch (Exception ex)
      {
      }
    }

    private int GetAndCleanNumberOfConnectedClients(TCPHandler.Client client)
    {
      lock (this)
      {
        bool flag = false;
        foreach (TCPHandler.Client clientLastRequest in this.tcpClientLastRequestList)
        {
          if (client.Equals((object) clientLastRequest))
            flag = true;
        }
        try
        {
          this.tcpClientLastRequestList.RemoveAll((Predicate<TCPHandler.Client>) (c => checked (DateTime.Now.Ticks - c.Ticks) > 40000000L));
        }
        catch (Exception ex)
        {
        }
        if (!flag)
          this.tcpClientLastRequestList.Add(client);
        return this.tcpClientLastRequestList.Count;
      }
    }

    private void ReadCallback(IAsyncResult asyncResult)
    {
      NetworkConnectionParameter connectionParameter = new NetworkConnectionParameter();
      TCPHandler.Client asyncState = asyncResult.AsyncState as TCPHandler.Client;
      asyncState.Ticks = DateTime.Now.Ticks;
      this.NumberOfConnectedClients = this.GetAndCleanNumberOfConnectedClients(asyncState);
      // ISSUE: reference to a compiler-generated field
      if (this.numberOfClientsChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.numberOfClientsChanged();
      }
      if (asyncState == null)
        return;
      NetworkStream networkStream;
      int count;
      try
      {
        networkStream = asyncState.NetworkStream;
        count = networkStream.EndRead(asyncResult);
      }
      catch (Exception ex)
      {
        return;
      }
      if (count == 0)
        return;
      byte[] numArray = new byte[count];
      Buffer.BlockCopy((Array) asyncState.Buffer, 0, (Array) numArray, 0, count);
      connectionParameter.bytes = numArray;
      connectionParameter.stream = networkStream;
      // ISSUE: reference to a compiler-generated field
      if (this.dataChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.dataChanged((object) connectionParameter);
      }
      try
      {
        networkStream.BeginRead(asyncState.Buffer, 0, asyncState.Buffer.Length, new AsyncCallback(this.ReadCallback), (object) asyncState);
      }
      catch (Exception ex)
      {
      }
    }

    public void Disconnect()
    {
      try
      {
        foreach (TCPHandler.Client clientLastRequest in this.tcpClientLastRequestList)
          clientLastRequest.NetworkStream.Close(0);
      }
      catch (Exception ex)
      {
      }
      this.server.Stop();
    }

    public delegate void DataChanged(object networkConnectionParameter);

    public delegate void NumberOfClientsChanged();

    internal class Client
    {
      private readonly TcpClient tcpClient;
      private readonly byte[] buffer;

      public long Ticks { get; set; }

      public Client(TcpClient tcpClient)
      {
        this.tcpClient = tcpClient;
        this.buffer = new byte[tcpClient.ReceiveBufferSize];
      }

      public TcpClient TcpClient
      {
        get
        {
          return this.tcpClient;
        }
      }

      public byte[] Buffer
      {
        get
        {
          return this.buffer;
        }
      }

      public NetworkStream NetworkStream
      {
        get
        {
          return this.tcpClient.GetStream();
        }
      }
    }
  }
}
