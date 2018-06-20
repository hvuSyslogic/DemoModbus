// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.MqttClient
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Internal;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Session;
using uPLibrary.Networking.M2Mqtt.Utility;

namespace uPLibrary.Networking.M2Mqtt
{
  public class MqttClient
  {
    private ushort messageIdCounter = 0;
    private string brokerHostName;
    private int brokerPort;
    private bool isRunning;
    private AutoResetEvent receiveEventWaitHandle;
    private AutoResetEvent inflightWaitHandle;
    private AutoResetEvent syncEndReceiving;
    private MqttMsgBase msgReceived;
    private Exception exReceiving;
    private int keepAlivePeriod;
    private AutoResetEvent keepAliveEvent;
    private AutoResetEvent keepAliveEventEnd;
    private int lastCommTime;
    private IMqttNetworkChannel channel;
    private System.Collections.Queue inflightQueue;
    private System.Collections.Queue internalQueue;
    private System.Collections.Queue eventQueue;
    private MqttClientSession session;
    private MqttSettings settings;
    private bool isConnectionClosing;

    public event MqttClient.MqttMsgPublishEventHandler MqttMsgPublishReceived;

    public event MqttClient.MqttMsgPublishedEventHandler MqttMsgPublished;

    public event MqttClient.MqttMsgSubscribedEventHandler MqttMsgSubscribed;

    public event MqttClient.MqttMsgUnsubscribedEventHandler MqttMsgUnsubscribed;

    public event MqttClient.ConnectionClosedEventHandler ConnectionClosed;

    public bool IsConnected { get; private set; }

    public string ClientId { get; private set; }

    public bool CleanSession { get; private set; }

    public bool WillFlag { get; private set; }

    public byte WillQosLevel { get; private set; }

    public string WillTopic { get; private set; }

    public string WillMessage { get; private set; }

    public MqttProtocolVersion ProtocolVersion { get; set; }

    public MqttSettings Settings
    {
      get
      {
        return this.settings;
      }
    }

    [Obsolete("Use this ctor MqttClient(string brokerHostName) insted")]
    public MqttClient(IPAddress brokerIpAddress)
      : this(brokerIpAddress, 1883, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None)
    {
    }

    [Obsolete("Use this ctor MqttClient(string brokerHostName, int brokerPort, bool secure, X509Certificate caCert) insted")]
    public MqttClient(IPAddress brokerIpAddress, int brokerPort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol)
    {
      this.Init(brokerIpAddress.ToString(), brokerPort, secure, caCert, clientCert, sslProtocol, (RemoteCertificateValidationCallback) null, (LocalCertificateSelectionCallback) null);
    }

    public MqttClient(string brokerHostName)
      : this(brokerHostName, 1883, false, (X509Certificate) null, (X509Certificate) null, MqttSslProtocols.None)
    {
    }

    public MqttClient(string brokerHostName, int brokerPort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol)
    {
      this.Init(brokerHostName, brokerPort, secure, caCert, clientCert, sslProtocol, (RemoteCertificateValidationCallback) null, (LocalCertificateSelectionCallback) null);
    }

    public MqttClient(string brokerHostName, int brokerPort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback)
      : this(brokerHostName, brokerPort, secure, caCert, clientCert, sslProtocol, userCertificateValidationCallback, (LocalCertificateSelectionCallback) null)
    {
    }

    public MqttClient(string brokerHostName, int brokerPort, bool secure, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
      : this(brokerHostName, brokerPort, secure, (X509Certificate) null, (X509Certificate) null, sslProtocol, userCertificateValidationCallback, userCertificateSelectionCallback)
    {
    }

    public MqttClient(string brokerHostName, int brokerPort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
    {
      this.Init(brokerHostName, brokerPort, secure, caCert, clientCert, sslProtocol, userCertificateValidationCallback, userCertificateSelectionCallback);
    }

    private void Init(string brokerHostName, int brokerPort, bool secure, X509Certificate caCert, X509Certificate clientCert, MqttSslProtocols sslProtocol, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
    {
      this.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;
      this.brokerHostName = brokerHostName;
      this.brokerPort = brokerPort;
      this.settings = MqttSettings.Instance;
      if (!secure)
        this.settings.Port = this.brokerPort;
      else
        this.settings.SslPort = this.brokerPort;
      this.syncEndReceiving = new AutoResetEvent(false);
      this.keepAliveEvent = new AutoResetEvent(false);
      this.inflightWaitHandle = new AutoResetEvent(false);
      this.inflightQueue = new System.Collections.Queue();
      this.receiveEventWaitHandle = new AutoResetEvent(false);
      this.eventQueue = new System.Collections.Queue();
      this.internalQueue = new System.Collections.Queue();
      this.session = (MqttClientSession) null;
      this.channel = (IMqttNetworkChannel) new MqttNetworkChannel(this.brokerHostName, this.brokerPort, secure, caCert, clientCert, sslProtocol, userCertificateValidationCallback, userCertificateSelectionCallback);
    }

    public byte Connect(string clientId)
    {
      return this.Connect(clientId, (string) null, (string) null, false, (byte) 0, false, (string) null, (string) null, true, (ushort) 60);
    }

    public byte Connect(string clientId, string username, string password)
    {
      return this.Connect(clientId, username, password, false, (byte) 0, false, (string) null, (string) null, true, (ushort) 60);
    }

    public byte Connect(string clientId, string username, string password, bool cleanSession, ushort keepAlivePeriod)
    {
      return this.Connect(clientId, username, password, false, (byte) 0, false, (string) null, (string) null, cleanSession, keepAlivePeriod);
    }

    public byte Connect(string clientId, string username, string password, bool willRetain, byte willQosLevel, bool willFlag, string willTopic, string willMessage, bool cleanSession, ushort keepAlivePeriod)
    {
      MqttMsgConnect mqttMsgConnect = new MqttMsgConnect(clientId, username, password, willRetain, willQosLevel, willFlag, willTopic, willMessage, cleanSession, keepAlivePeriod, checked ((byte) (uint) this.ProtocolVersion));
      try
      {
        this.channel.Connect();
      }
      catch (Exception ex)
      {
        throw new MqttConnectionException("Exception connecting to the broker", ex);
      }
      this.lastCommTime = 0;
      this.isRunning = true;
      this.isConnectionClosing = false;
      Fx.StartThread(new ThreadStart(this.ReceiveThread));
      MqttMsgConnack mqttMsgConnack = (MqttMsgConnack) this.SendReceive((MqttMsgBase) mqttMsgConnect);
      if (mqttMsgConnack.ReturnCode == (byte) 0)
      {
        this.ClientId = clientId;
        this.CleanSession = cleanSession;
        this.WillFlag = willFlag;
        this.WillTopic = willTopic;
        this.WillMessage = willMessage;
        this.WillQosLevel = willQosLevel;
        this.keepAlivePeriod = checked ((int) keepAlivePeriod * 1000);
        this.RestoreSession();
        if ((uint) this.keepAlivePeriod > 0U)
          Fx.StartThread(new ThreadStart(this.KeepAliveThread));
        Fx.StartThread(new ThreadStart(this.DispatchEventThread));
        Fx.StartThread(new ThreadStart(this.ProcessInflightThread));
        this.IsConnected = true;
      }
      return mqttMsgConnack.ReturnCode;
    }

    public void Disconnect()
    {
      this.Send((MqttMsgBase) new MqttMsgDisconnect());
      this.OnConnectionClosing();
    }

    private void Close()
    {
      this.isRunning = false;
      if (this.receiveEventWaitHandle != null)
        this.receiveEventWaitHandle.Set();
      if (this.inflightWaitHandle != null)
        this.inflightWaitHandle.Set();
      this.keepAliveEvent.Set();
      if (this.keepAliveEventEnd != null)
        this.keepAliveEventEnd.WaitOne();
      this.inflightQueue.Clear();
      this.internalQueue.Clear();
      this.eventQueue.Clear();
      this.channel.Close();
      this.IsConnected = false;
    }

    private MqttMsgPingResp Ping()
    {
      MqttMsgPingReq mqttMsgPingReq = new MqttMsgPingReq();
      try
      {
        return (MqttMsgPingResp) this.SendReceive((MqttMsgBase) mqttMsgPingReq, this.keepAlivePeriod);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(TraceLevel.Error, "Exception occurred: {0}", (object) ex.ToString());
        this.OnConnectionClosing();
        return (MqttMsgPingResp) null;
      }
    }

    public ushort Subscribe(string[] topics, byte[] qosLevels)
    {
      MqttMsgSubscribe mqttMsgSubscribe = new MqttMsgSubscribe(topics, qosLevels);
      mqttMsgSubscribe.MessageId = this.GetMessageId();
      this.EnqueueInflight((MqttMsgBase) mqttMsgSubscribe, MqttMsgFlow.ToPublish);
      return mqttMsgSubscribe.MessageId;
    }

    public ushort Unsubscribe(string[] topics)
    {
      MqttMsgUnsubscribe mqttMsgUnsubscribe = new MqttMsgUnsubscribe(topics);
      mqttMsgUnsubscribe.MessageId = this.GetMessageId();
      this.EnqueueInflight((MqttMsgBase) mqttMsgUnsubscribe, MqttMsgFlow.ToPublish);
      return mqttMsgUnsubscribe.MessageId;
    }

    public ushort Publish(string topic, byte[] message)
    {
      return this.Publish(topic, message, (byte) 0, false);
    }

    public ushort Publish(string topic, byte[] message, byte qosLevel, bool retain)
    {
      MqttMsgPublish mqttMsgPublish = new MqttMsgPublish(topic, message, false, qosLevel, retain);
      mqttMsgPublish.MessageId = this.GetMessageId();
      if (this.EnqueueInflight((MqttMsgBase) mqttMsgPublish, MqttMsgFlow.ToPublish))
        return mqttMsgPublish.MessageId;
      throw new MqttClientException(MqttClientErrorCode.InflightQueueFull);
    }

    private void OnInternalEvent(InternalEvent internalEvent)
    {
      lock (this.eventQueue)
        this.eventQueue.Enqueue((object) internalEvent);
      this.receiveEventWaitHandle.Set();
    }

    private void OnConnectionClosing()
    {
      if (this.isConnectionClosing)
        return;
      this.isConnectionClosing = true;
      this.receiveEventWaitHandle.Set();
    }

    private void OnMqttMsgPublishReceived(MqttMsgPublish publish)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.MqttMsgPublishReceived == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.MqttMsgPublishReceived((object) this, new MqttMsgPublishEventArgs(publish.Topic, publish.Message, publish.DupFlag, publish.QosLevel, publish.Retain));
    }

    private void OnMqttMsgPublished(ushort messageId, bool isPublished)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.MqttMsgPublished == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.MqttMsgPublished((object) this, new MqttMsgPublishedEventArgs(messageId, isPublished));
    }

    private void OnMqttMsgSubscribed(MqttMsgSuback suback)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.MqttMsgSubscribed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.MqttMsgSubscribed((object) this, new MqttMsgSubscribedEventArgs(suback.MessageId, suback.GrantedQoSLevels));
    }

    private void OnMqttMsgUnsubscribed(ushort messageId)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.MqttMsgUnsubscribed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.MqttMsgUnsubscribed((object) this, new MqttMsgUnsubscribedEventArgs(messageId));
    }

    private void OnConnectionClosed()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ConnectionClosed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ConnectionClosed((object) this, EventArgs.Empty);
    }

    private void Send(byte[] msgBytes)
    {
      try
      {
        this.channel.Send(msgBytes);
        this.lastCommTime = Environment.TickCount;
      }
      catch (Exception ex)
      {
        Trace.WriteLine(TraceLevel.Error, "Exception occurred: {0}", (object) ex.ToString());
        throw new MqttCommunicationException(ex);
      }
    }

    private void Send(MqttMsgBase msg)
    {
      Trace.WriteLine(TraceLevel.Frame, "SEND {0}", (object) msg);
      this.Send(msg.GetBytes(checked ((byte) (uint) this.ProtocolVersion)));
    }

    private MqttMsgBase SendReceive(byte[] msgBytes)
    {
      return this.SendReceive(msgBytes, 30000);
    }

    private MqttMsgBase SendReceive(byte[] msgBytes, int timeout)
    {
      this.syncEndReceiving.Reset();
      try
      {
        this.channel.Send(msgBytes);
        this.lastCommTime = Environment.TickCount;
      }
      catch (Exception ex)
      {
        if (typeof (SocketException) == ex.GetType() && ((SocketException) ex).SocketErrorCode == SocketError.ConnectionReset)
          this.IsConnected = false;
        Trace.WriteLine(TraceLevel.Error, "Exception occurred: {0}", (object) ex.ToString());
        throw new MqttCommunicationException(ex);
      }
      if (!this.syncEndReceiving.WaitOne(timeout))
        throw new MqttCommunicationException();
      if (this.exReceiving == null)
        return this.msgReceived;
      throw this.exReceiving;
    }

    private MqttMsgBase SendReceive(MqttMsgBase msg)
    {
      return this.SendReceive(msg, 30000);
    }

    private MqttMsgBase SendReceive(MqttMsgBase msg, int timeout)
    {
      Trace.WriteLine(TraceLevel.Frame, "SEND {0}", (object) msg);
      return this.SendReceive(msg.GetBytes(checked ((byte) (uint) this.ProtocolVersion)), timeout);
    }

    private bool EnqueueInflight(MqttMsgBase msg, MqttMsgFlow flow)
    {
      bool flag = true;
      if (msg.Type == (byte) 3 && msg.QosLevel == (byte) 2)
      {
        lock (this.inflightQueue)
        {
          MqttMsgContext mqttMsgContext = (MqttMsgContext) this.inflightQueue.Get(new QueueExtension.QueuePredicate(new MqttClient.MqttMsgContextFinder(msg.MessageId, MqttMsgFlow.ToAcknowledge).Find));
          if (mqttMsgContext != null)
          {
            mqttMsgContext.State = MqttMsgState.QueuedQos2;
            mqttMsgContext.Flow = MqttMsgFlow.ToAcknowledge;
            flag = false;
          }
        }
      }
      if (flag)
      {
        MqttMsgState mqttMsgState = MqttMsgState.QueuedQos0;
        switch (msg.QosLevel)
        {
          case 0:
            mqttMsgState = MqttMsgState.QueuedQos0;
            break;
          case 1:
            mqttMsgState = MqttMsgState.QueuedQos1;
            break;
          case 2:
            mqttMsgState = MqttMsgState.QueuedQos2;
            break;
        }
        if (msg.Type == (byte) 8)
          mqttMsgState = MqttMsgState.SendSubscribe;
        else if (msg.Type == (byte) 10)
          mqttMsgState = MqttMsgState.SendUnsubscribe;
        MqttMsgContext mqttMsgContext = new MqttMsgContext() { Message = msg, State = mqttMsgState, Flow = flow, Attempt = 0 };
        lock (this.inflightQueue)
        {
          flag = this.inflightQueue.Count < this.settings.InflightQueueSize;
          if (flag)
          {
            this.inflightQueue.Enqueue((object) mqttMsgContext);
            Trace.WriteLine(TraceLevel.Queuing, "enqueued {0}", (object) msg);
            if (msg.Type == (byte) 3)
            {
              if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish && (msg.QosLevel == (byte) 1 || msg.QosLevel == (byte) 2))
              {
                if (this.session != null)
                  this.session.InflightMessages.Add((object) mqttMsgContext.Key, (object) mqttMsgContext);
              }
              else if (mqttMsgContext.Flow == MqttMsgFlow.ToAcknowledge && msg.QosLevel == (byte) 2 && this.session != null)
                this.session.InflightMessages.Add((object) mqttMsgContext.Key, (object) mqttMsgContext);
            }
          }
        }
      }
      this.inflightWaitHandle.Set();
      return flag;
    }

    private void EnqueueInternal(MqttMsgBase msg)
    {
      bool flag = true;
      if (msg.Type == (byte) 6)
      {
        lock (this.inflightQueue)
        {
          if ((MqttMsgContext) this.inflightQueue.Get(new QueueExtension.QueuePredicate(new MqttClient.MqttMsgContextFinder(msg.MessageId, MqttMsgFlow.ToAcknowledge).Find)) == null)
          {
            MqttMsgPubcomp mqttMsgPubcomp = new MqttMsgPubcomp();
            mqttMsgPubcomp.MessageId = msg.MessageId;
            this.Send((MqttMsgBase) mqttMsgPubcomp);
            flag = false;
          }
        }
      }
      else if (msg.Type == (byte) 7)
      {
        lock (this.inflightQueue)
        {
          if ((MqttMsgContext) this.inflightQueue.Get(new QueueExtension.QueuePredicate(new MqttClient.MqttMsgContextFinder(msg.MessageId, MqttMsgFlow.ToPublish).Find)) == null)
            flag = false;
        }
      }
      else if (msg.Type == (byte) 5)
      {
        lock (this.inflightQueue)
        {
          if ((MqttMsgContext) this.inflightQueue.Get(new QueueExtension.QueuePredicate(new MqttClient.MqttMsgContextFinder(msg.MessageId, MqttMsgFlow.ToPublish).Find)) == null)
            flag = false;
        }
      }
      if (!flag)
        return;
      lock (this.internalQueue)
      {
        this.internalQueue.Enqueue((object) msg);
        Trace.WriteLine(TraceLevel.Queuing, "enqueued {0}", (object) msg);
        this.inflightWaitHandle.Set();
      }
    }

    private void ReceiveThread()
    {
      byte[] buffer = new byte[1];
      while (this.isRunning)
      {
        try
        {
          if (this.channel.Receive(buffer) > 0)
          {
            switch (checked ((byte) (((int) buffer[0] & 240) >> 4)))
            {
              case 1:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              case 2:
                this.msgReceived = (MqttMsgBase) MqttMsgConnack.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) this.msgReceived);
                this.syncEndReceiving.Set();
                break;
              case 3:
                MqttMsgPublish mqttMsgPublish = MqttMsgPublish.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgPublish);
                this.EnqueueInflight((MqttMsgBase) mqttMsgPublish, MqttMsgFlow.ToAcknowledge);
                break;
              case 4:
                MqttMsgPuback mqttMsgPuback = MqttMsgPuback.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgPuback);
                this.EnqueueInternal((MqttMsgBase) mqttMsgPuback);
                break;
              case 5:
                MqttMsgPubrec mqttMsgPubrec = MqttMsgPubrec.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgPubrec);
                this.EnqueueInternal((MqttMsgBase) mqttMsgPubrec);
                break;
              case 6:
                MqttMsgPubrel mqttMsgPubrel = MqttMsgPubrel.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgPubrel);
                this.EnqueueInternal((MqttMsgBase) mqttMsgPubrel);
                break;
              case 7:
                MqttMsgPubcomp mqttMsgPubcomp = MqttMsgPubcomp.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgPubcomp);
                this.EnqueueInternal((MqttMsgBase) mqttMsgPubcomp);
                break;
              case 8:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              case 9:
                MqttMsgSuback mqttMsgSuback = MqttMsgSuback.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgSuback);
                this.EnqueueInternal((MqttMsgBase) mqttMsgSuback);
                break;
              case 10:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              case 11:
                MqttMsgUnsuback mqttMsgUnsuback = MqttMsgUnsuback.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) mqttMsgUnsuback);
                this.EnqueueInternal((MqttMsgBase) mqttMsgUnsuback);
                break;
              case 12:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              case 13:
                this.msgReceived = (MqttMsgBase) MqttMsgPingResp.Parse(buffer[0], checked ((byte) (uint) this.ProtocolVersion), this.channel);
                Trace.WriteLine(TraceLevel.Frame, "RECV {0}", (object) this.msgReceived);
                this.syncEndReceiving.Set();
                break;
              case 14:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              default:
                throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
            }
            this.exReceiving = (Exception) null;
          }
          else
            this.OnConnectionClosing();
        }
        catch (Exception ex)
        {
          Trace.WriteLine(TraceLevel.Error, "Exception occurred: {0}", (object) ex.ToString());
          this.exReceiving = (Exception) new MqttCommunicationException(ex);
          bool flag = false;
          if (ex.GetType() == typeof (MqttClientException))
          {
            MqttClientException mqttClientException = ex as MqttClientException;
            flag = mqttClientException.ErrorCode == MqttClientErrorCode.InvalidFlagBits || mqttClientException.ErrorCode == MqttClientErrorCode.InvalidProtocolName || mqttClientException.ErrorCode == MqttClientErrorCode.InvalidConnectFlags;
          }
          else if (ex.GetType() == typeof (IOException) || ex.GetType() == typeof (SocketException) || ex.InnerException != null && ex.InnerException.GetType() == typeof (SocketException))
            flag = true;
          if (flag)
            this.OnConnectionClosing();
        }
      }
    }

    private void KeepAliveThread()
    {
      int millisecondsTimeout = this.keepAlivePeriod;
      this.keepAliveEventEnd = new AutoResetEvent(false);
      while (this.isRunning)
      {
        this.keepAliveEvent.WaitOne(millisecondsTimeout);
        if (this.isRunning)
        {
          int num = checked (Environment.TickCount - this.lastCommTime);
          if (num >= this.keepAlivePeriod)
          {
            this.Ping();
            millisecondsTimeout = this.keepAlivePeriod;
          }
          else
            millisecondsTimeout = checked (this.keepAlivePeriod - num);
        }
      }
      this.keepAliveEventEnd.Set();
    }

    private void DispatchEventThread()
    {
      while (this.isRunning)
      {
        if (this.eventQueue.Count == 0 && !this.isConnectionClosing)
          this.receiveEventWaitHandle.WaitOne();
        if (this.isRunning)
        {
          InternalEvent internalEvent = (InternalEvent) null;
          lock (this.eventQueue)
          {
            if (this.eventQueue.Count > 0)
              internalEvent = (InternalEvent) this.eventQueue.Dequeue();
          }
          if (internalEvent != null)
          {
            MqttMsgBase message = ((MsgInternalEvent) internalEvent).Message;
            if (message != null)
            {
              switch (message.Type)
              {
                case 1:
                  throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
                case 3:
                  if (internalEvent.GetType() == typeof (MsgPublishedInternalEvent))
                  {
                    this.OnMqttMsgPublished(message.MessageId, false);
                    break;
                  }
                  this.OnMqttMsgPublishReceived((MqttMsgPublish) message);
                  break;
                case 4:
                  this.OnMqttMsgPublished(message.MessageId, true);
                  break;
                case 6:
                  this.OnMqttMsgPublishReceived((MqttMsgPublish) message);
                  break;
                case 7:
                  this.OnMqttMsgPublished(message.MessageId, true);
                  break;
                case 8:
                  throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
                case 9:
                  this.OnMqttMsgSubscribed((MqttMsgSuback) message);
                  break;
                case 10:
                  throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
                case 11:
                  this.OnMqttMsgUnsubscribed(message.MessageId);
                  break;
                case 14:
                  throw new MqttClientException(MqttClientErrorCode.WrongBrokerMessage);
              }
            }
          }
          if (this.eventQueue.Count == 0 && this.isConnectionClosing)
          {
            this.Close();
            this.OnConnectionClosed();
          }
        }
      }
    }

    private void ProcessInflightThread()
    {
      MqttMsgContext mqttMsgContext = (MqttMsgContext) null;
      bool flag1 = false;
      int millisecondsTimeout = -1;
      try
      {
        while (this.isRunning)
        {
          this.inflightWaitHandle.WaitOne(millisecondsTimeout);
          if (this.isRunning)
          {
            lock (this.inflightQueue)
            {
              bool flag2 = false;
              flag1 = false;
              MqttMsgBase msg = (MqttMsgBase) null;
              millisecondsTimeout = int.MaxValue;
              int count = this.inflightQueue.Count;
              while (count > 0)
              {
                checked { --count; }
                flag1 = false;
                msg = (MqttMsgBase) null;
                if (this.isRunning)
                {
                  mqttMsgContext = (MqttMsgContext) this.inflightQueue.Dequeue();
                  MqttMsgBase message = mqttMsgContext.Message;
                  switch (mqttMsgContext.State)
                  {
                    case MqttMsgState.QueuedQos0:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                        this.Send(message);
                      else if (mqttMsgContext.Flow == MqttMsgFlow.ToAcknowledge)
                        this.OnInternalEvent((InternalEvent) new MsgInternalEvent(message));
                      Trace.WriteLine(TraceLevel.Queuing, "processed {0}", (object) message);
                      break;
                    case MqttMsgState.QueuedQos1:
                    case MqttMsgState.SendSubscribe:
                    case MqttMsgState.SendUnsubscribe:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        mqttMsgContext.Timestamp = Environment.TickCount;
                        checked { ++mqttMsgContext.Attempt; }
                        if (message.Type == (byte) 3)
                        {
                          mqttMsgContext.State = MqttMsgState.WaitForPuback;
                          if (mqttMsgContext.Attempt > 1)
                            message.DupFlag = true;
                        }
                        else if (message.Type == (byte) 8)
                          mqttMsgContext.State = MqttMsgState.WaitForSuback;
                        else if (message.Type == (byte) 10)
                          mqttMsgContext.State = MqttMsgState.WaitForUnsuback;
                        this.Send(message);
                        millisecondsTimeout = this.settings.DelayOnRetry < millisecondsTimeout ? this.settings.DelayOnRetry : millisecondsTimeout;
                        this.inflightQueue.Enqueue((object) mqttMsgContext);
                        break;
                      }
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToAcknowledge)
                      {
                        MqttMsgPuback mqttMsgPuback = new MqttMsgPuback();
                        mqttMsgPuback.MessageId = message.MessageId;
                        this.Send((MqttMsgBase) mqttMsgPuback);
                        this.OnInternalEvent((InternalEvent) new MsgInternalEvent(message));
                        Trace.WriteLine(TraceLevel.Queuing, "processed {0}", (object) message);
                        break;
                      }
                      break;
                    case MqttMsgState.QueuedQos2:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        mqttMsgContext.Timestamp = Environment.TickCount;
                        checked { ++mqttMsgContext.Attempt; }
                        mqttMsgContext.State = MqttMsgState.WaitForPubrec;
                        if (mqttMsgContext.Attempt > 1)
                          message.DupFlag = true;
                        this.Send(message);
                        millisecondsTimeout = this.settings.DelayOnRetry < millisecondsTimeout ? this.settings.DelayOnRetry : millisecondsTimeout;
                        this.inflightQueue.Enqueue((object) mqttMsgContext);
                        break;
                      }
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToAcknowledge)
                      {
                        MqttMsgPubrec mqttMsgPubrec = new MqttMsgPubrec();
                        mqttMsgPubrec.MessageId = message.MessageId;
                        mqttMsgContext.State = MqttMsgState.WaitForPubrel;
                        this.Send((MqttMsgBase) mqttMsgPubrec);
                        this.inflightQueue.Enqueue((object) mqttMsgContext);
                        break;
                      }
                      break;
                    case MqttMsgState.WaitForPuback:
                    case MqttMsgState.WaitForSuback:
                    case MqttMsgState.WaitForUnsuback:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        bool flag3 = false;
                        lock (this.internalQueue)
                        {
                          if (this.internalQueue.Count > 0)
                            msg = (MqttMsgBase) this.internalQueue.Peek();
                        }
                        if (msg != null && (msg.Type == (byte) 4 && message.Type == (byte) 3 && (int) msg.MessageId == (int) message.MessageId || msg.Type == (byte) 9 && message.Type == (byte) 8 && (int) msg.MessageId == (int) message.MessageId || msg.Type == (byte) 11 && message.Type == (byte) 10 && (int) msg.MessageId == (int) message.MessageId))
                        {
                          lock (this.internalQueue)
                          {
                            this.internalQueue.Dequeue();
                            flag3 = true;
                            flag2 = true;
                            Trace.WriteLine(TraceLevel.Queuing, "dequeued {0}", (object) msg);
                          }
                          this.OnInternalEvent(msg.Type != (byte) 4 ? (InternalEvent) new MsgInternalEvent(msg) : (InternalEvent) new MsgPublishedInternalEvent(msg, true));
                          if (message.Type == (byte) 3 && this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                            this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                          Trace.WriteLine(TraceLevel.Queuing, "processed {0}", (object) message);
                        }
                        if (!flag3)
                        {
                          int num1 = checked (Environment.TickCount - mqttMsgContext.Timestamp);
                          if (num1 >= this.settings.DelayOnRetry)
                          {
                            if (mqttMsgContext.Attempt < this.settings.AttemptsOnRetry)
                            {
                              mqttMsgContext.State = MqttMsgState.QueuedQos1;
                              this.inflightQueue.Enqueue((object) mqttMsgContext);
                              millisecondsTimeout = 0;
                            }
                            else if (message.Type == (byte) 3)
                            {
                              if (this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                                this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                              this.OnInternalEvent((InternalEvent) new MsgPublishedInternalEvent(message, false));
                            }
                          }
                          else
                          {
                            this.inflightQueue.Enqueue((object) mqttMsgContext);
                            int num2 = checked (this.settings.DelayOnRetry - num1);
                            millisecondsTimeout = num2 < millisecondsTimeout ? num2 : millisecondsTimeout;
                          }
                        }
                        break;
                      }
                      break;
                    case MqttMsgState.WaitForPubrec:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        bool flag3 = false;
                        lock (this.internalQueue)
                        {
                          if (this.internalQueue.Count > 0)
                            msg = (MqttMsgBase) this.internalQueue.Peek();
                        }
                        if (msg != null && msg.Type == (byte) 5 && (int) msg.MessageId == (int) message.MessageId)
                        {
                          lock (this.internalQueue)
                          {
                            this.internalQueue.Dequeue();
                            flag3 = true;
                            flag2 = true;
                            Trace.WriteLine(TraceLevel.Queuing, "dequeued {0}", (object) msg);
                          }
                          MqttMsgPubrel mqttMsgPubrel = new MqttMsgPubrel();
                          mqttMsgPubrel.MessageId = message.MessageId;
                          mqttMsgContext.State = MqttMsgState.WaitForPubcomp;
                          mqttMsgContext.Timestamp = Environment.TickCount;
                          mqttMsgContext.Attempt = 1;
                          this.Send((MqttMsgBase) mqttMsgPubrel);
                          millisecondsTimeout = this.settings.DelayOnRetry < millisecondsTimeout ? this.settings.DelayOnRetry : millisecondsTimeout;
                          this.inflightQueue.Enqueue((object) mqttMsgContext);
                        }
                        if (!flag3)
                        {
                          int num1 = checked (Environment.TickCount - mqttMsgContext.Timestamp);
                          if (num1 >= this.settings.DelayOnRetry)
                          {
                            if (mqttMsgContext.Attempt < this.settings.AttemptsOnRetry)
                            {
                              mqttMsgContext.State = MqttMsgState.QueuedQos2;
                              this.inflightQueue.Enqueue((object) mqttMsgContext);
                              millisecondsTimeout = 0;
                            }
                            else
                            {
                              if (this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                                this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                              this.OnInternalEvent((InternalEvent) new MsgPublishedInternalEvent(message, false));
                            }
                          }
                          else
                          {
                            this.inflightQueue.Enqueue((object) mqttMsgContext);
                            int num2 = checked (this.settings.DelayOnRetry - num1);
                            millisecondsTimeout = num2 < millisecondsTimeout ? num2 : millisecondsTimeout;
                          }
                        }
                        break;
                      }
                      break;
                    case MqttMsgState.WaitForPubrel:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToAcknowledge)
                      {
                        lock (this.internalQueue)
                        {
                          if (this.internalQueue.Count > 0)
                            msg = (MqttMsgBase) this.internalQueue.Peek();
                        }
                        if (msg != null && msg.Type == (byte) 6)
                        {
                          if ((int) msg.MessageId == (int) message.MessageId)
                          {
                            lock (this.internalQueue)
                            {
                              this.internalQueue.Dequeue();
                              flag2 = true;
                              Trace.WriteLine(TraceLevel.Queuing, "dequeued {0}", (object) msg);
                            }
                            MqttMsgPubcomp mqttMsgPubcomp = new MqttMsgPubcomp();
                            mqttMsgPubcomp.MessageId = message.MessageId;
                            this.Send((MqttMsgBase) mqttMsgPubcomp);
                            this.OnInternalEvent((InternalEvent) new MsgInternalEvent(message));
                            if (message.Type == (byte) 3 && this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                              this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                            Trace.WriteLine(TraceLevel.Queuing, "processed {0}", (object) message);
                          }
                          else
                            this.inflightQueue.Enqueue((object) mqttMsgContext);
                        }
                        else
                          this.inflightQueue.Enqueue((object) mqttMsgContext);
                        break;
                      }
                      break;
                    case MqttMsgState.WaitForPubcomp:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        bool flag3 = false;
                        lock (this.internalQueue)
                        {
                          if (this.internalQueue.Count > 0)
                            msg = (MqttMsgBase) this.internalQueue.Peek();
                        }
                        if (msg != null && msg.Type == (byte) 7)
                        {
                          if ((int) msg.MessageId == (int) message.MessageId)
                          {
                            lock (this.internalQueue)
                            {
                              this.internalQueue.Dequeue();
                              flag3 = true;
                              flag2 = true;
                              Trace.WriteLine(TraceLevel.Queuing, "dequeued {0}", (object) msg);
                            }
                            this.OnInternalEvent((InternalEvent) new MsgPublishedInternalEvent(msg, true));
                            if (message.Type == (byte) 3 && this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                              this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                            Trace.WriteLine(TraceLevel.Queuing, "processed {0}", (object) message);
                          }
                        }
                        else if (msg != null && msg.Type == (byte) 5 && (int) msg.MessageId == (int) message.MessageId)
                        {
                          lock (this.internalQueue)
                          {
                            this.internalQueue.Dequeue();
                            flag3 = true;
                            flag2 = true;
                            Trace.WriteLine(TraceLevel.Queuing, "dequeued {0}", (object) msg);
                            this.inflightQueue.Enqueue((object) mqttMsgContext);
                          }
                        }
                        if (!flag3)
                        {
                          int num1 = checked (Environment.TickCount - mqttMsgContext.Timestamp);
                          if (num1 >= this.settings.DelayOnRetry)
                          {
                            if (mqttMsgContext.Attempt < this.settings.AttemptsOnRetry)
                            {
                              mqttMsgContext.State = MqttMsgState.SendPubrel;
                              this.inflightQueue.Enqueue((object) mqttMsgContext);
                              millisecondsTimeout = 0;
                            }
                            else
                            {
                              if (this.session != null && this.session.InflightMessages.ContainsKey((object) mqttMsgContext.Key))
                                this.session.InflightMessages.Remove((object) mqttMsgContext.Key);
                              this.OnInternalEvent((InternalEvent) new MsgPublishedInternalEvent(message, false));
                            }
                          }
                          else
                          {
                            this.inflightQueue.Enqueue((object) mqttMsgContext);
                            int num2 = checked (this.settings.DelayOnRetry - num1);
                            millisecondsTimeout = num2 < millisecondsTimeout ? num2 : millisecondsTimeout;
                          }
                        }
                        break;
                      }
                      break;
                    case MqttMsgState.SendPubrel:
                      if (mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
                      {
                        MqttMsgPubrel mqttMsgPubrel = new MqttMsgPubrel();
                        mqttMsgPubrel.MessageId = message.MessageId;
                        mqttMsgContext.State = MqttMsgState.WaitForPubcomp;
                        mqttMsgContext.Timestamp = Environment.TickCount;
                        checked { ++mqttMsgContext.Attempt; }
                        if (this.ProtocolVersion == MqttProtocolVersion.Version_3_1 && mqttMsgContext.Attempt > 1)
                          mqttMsgPubrel.DupFlag = true;
                        this.Send((MqttMsgBase) mqttMsgPubrel);
                        millisecondsTimeout = this.settings.DelayOnRetry < millisecondsTimeout ? this.settings.DelayOnRetry : millisecondsTimeout;
                        this.inflightQueue.Enqueue((object) mqttMsgContext);
                        break;
                      }
                      break;
                  }
                }
                else
                  break;
              }
              if (millisecondsTimeout == int.MaxValue)
                millisecondsTimeout = -1;
              if (msg != null && !flag2)
              {
                this.internalQueue.Dequeue();
                Trace.WriteLine(TraceLevel.Queuing, "dequeued {0} orphan", (object) msg);
              }
            }
          }
        }
      }
      catch (MqttCommunicationException ex)
      {
        if (mqttMsgContext != null)
          this.inflightQueue.Enqueue((object) mqttMsgContext);
        Trace.WriteLine(TraceLevel.Error, "Exception occurred: {0}", (object) ex.ToString());
        this.OnConnectionClosing();
      }
    }

    private void RestoreSession()
    {
      if (!this.CleanSession)
      {
        if (this.session != null)
        {
          lock (this.inflightQueue)
          {
            foreach (MqttMsgContext mqttMsgContext in (IEnumerable) this.session.InflightMessages.Values)
            {
              this.inflightQueue.Enqueue((object) mqttMsgContext);
              if (mqttMsgContext.Message.Type == (byte) 3 && mqttMsgContext.Flow == MqttMsgFlow.ToPublish)
              {
                if (mqttMsgContext.Message.QosLevel == (byte) 1 && mqttMsgContext.State == MqttMsgState.WaitForPuback)
                  mqttMsgContext.State = MqttMsgState.QueuedQos1;
                else if (mqttMsgContext.Message.QosLevel == (byte) 2)
                {
                  if (mqttMsgContext.State == MqttMsgState.WaitForPubrec)
                    mqttMsgContext.State = MqttMsgState.QueuedQos2;
                  else if (mqttMsgContext.State == MqttMsgState.WaitForPubcomp)
                    mqttMsgContext.State = MqttMsgState.SendPubrel;
                }
              }
            }
          }
          this.inflightWaitHandle.Set();
        }
        else
          this.session = new MqttClientSession(this.ClientId);
      }
      else if (this.session != null)
        this.session.Clear();
    }

    private ushort GetMessageId()
    {
      this.messageIdCounter = (int) this.messageIdCounter % (int) ushort.MaxValue != 0 ? checked ((ushort) ((int) this.messageIdCounter + 1)) : (ushort) 1;
      return this.messageIdCounter;
    }

    public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);

    public delegate void MqttMsgPublishedEventHandler(object sender, MqttMsgPublishedEventArgs e);

    public delegate void MqttMsgSubscribedEventHandler(object sender, MqttMsgSubscribedEventArgs e);

    public delegate void MqttMsgUnsubscribedEventHandler(object sender, MqttMsgUnsubscribedEventArgs e);

    public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);

    internal class MqttMsgContextFinder
    {
      internal ushort MessageId { get; set; }

      internal MqttMsgFlow Flow { get; set; }

      internal MqttMsgContextFinder(ushort messageId, MqttMsgFlow flow)
      {
        this.MessageId = messageId;
        this.Flow = flow;
      }

      internal bool Find(object item)
      {
        MqttMsgContext mqttMsgContext = (MqttMsgContext) item;
        return mqttMsgContext.Message.Type == (byte) 3 && (int) mqttMsgContext.Message.MessageId == (int) this.MessageId && mqttMsgContext.Flow == this.Flow;
      }
    }
  }
}
