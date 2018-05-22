// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.SvcWriter
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace PhoenixContact.HFI
{
  internal class SvcWriter
  {
    private string _name;
    private MessageClient _msgClient;
    private bool _registerMsgClient;
    private bool _removeMsgClient;
    private Diagnostic _locDiagnostic;
    private ArrayList _lstRequests;
    private int _iLastMessageSend;
    private StreamWriter _swLogFile;
    private IController _locController;

    private event SvcWriterStartHandler _hdOnWriteStart;

    private event SvcWriterReadyHandler _hdOnWriteReady;

    internal SvcWriter(string Name, IController Controller)
    {
      if (Controller == null)
        return;
      this._name = Name;
      this._locController = Controller;
      this._locController.OnUpdateMailbox += new UpdateMailboxHandler(this._locController_OnUpdateMailbox);
      this._locDiagnostic = new Diagnostic(Name + " SVC-Writer");
      this._locDiagnostic.Activate = true;
      this._msgClient = new MessageClient(Name + " SVC-Writer");
      this._msgClient.OnConfirmationReceived += new ConfirmationReceiveHandler(this._msgClient_OnConfirmationReceived);
      this._msgClient.OnDiagnostic += new DiagnosticHandler(this._msgClient_OnDiagnostic);
    }

    public override string ToString()
    {
      return this._name;
    }

    internal string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    internal event SvcWriterStartHandler OnStart
    {
      add
      {
        this._hdOnWriteStart += value;
      }
      remove
      {
        this._hdOnWriteStart -= value;
      }
    }

    internal event SvcWriterReadyHandler OnFinish
    {
      add
      {
        this._hdOnWriteReady += value;
      }
      remove
      {
        this._hdOnWriteReady -= value;
      }
    }

    internal event DiagnosticHandler OnDiagnostic
    {
      add
      {
        this._locDiagnostic.OnDiagnostic += value;
      }
      remove
      {
        this._locDiagnostic.OnDiagnostic -= value;
      }
    }

    internal bool SvcFileDownload(string FileName, string LogFileName)
    {
      bool flag = false;
      if (this._msgClient != null && !this._registerMsgClient && (!this._removeMsgClient && this._locController.AddObject(this._msgClient) >= 0))
      {
        this._registerMsgClient = true;
        if (FileName.Length > 0 && this.Parse(FileName, LogFileName))
        {
          if (this._hdOnWriteStart != null)
            this._hdOnWriteStart((object) this);
          flag = true;
        }
      }
      return flag;
    }

    internal bool SvcFileDownload(string FileName)
    {
      return this.SvcFileDownload(FileName, (string) null);
    }

    private void _locController_OnUpdateMailbox(object Sender)
    {
      if (!this._removeMsgClient)
        return;
      this._removeMsgClient = false;
      if (this._registerMsgClient)
      {
        this._registerMsgClient = false;
        this._locController.RemoveObject(this._msgClient);
      }
      if (this._hdOnWriteReady == null)
        return;
      this._hdOnWriteReady((object) this);
    }

    private void _msgClient_OnConfirmationReceived(object Sender, byte[] Data)
    {
      if (this._swLogFile != null)
        this._swLogFile.WriteLine(Util.StringJustification("Response: " + Util.ByteArrayToHexStringW(Util.GetByteArrayFromService(Data), ' '), "Response: 0000 ".Length, 80));
      if (Data != null && Data.Length >= 8)
      {
        if (Util.CheckPcpConfirmation(Data[0], Data[1]))
        {
          if (Util.ByteToInt32(Data[6], Data[7]) != 0)
          {
            this.OnDiagnosticMsg((object) this, "Negativ PCP Confirmation", Data);
            return;
          }
        }
        else if (Util.ByteToInt32(Data[4], Data[5]) != 0)
        {
          this.OnDiagnosticMsg((object) this, "Negativ Firmware Confirmation", Data);
          return;
        }
      }
      if (this._lstRequests == null)
        return;
      ++this._iLastMessageSend;
      if (this._iLastMessageSend == this._lstRequests.Count)
      {
        this._lstRequests.Clear();
        this._iLastMessageSend = 0;
        if (this._swLogFile != null)
        {
          this._swLogFile.WriteLine("SVC Download finished.");
          this._swLogFile.Close();
          this._swLogFile = (StreamWriter) null;
        }
        this._removeMsgClient = true;
      }
      else
      {
        if (this._swLogFile != null)
          this._swLogFile.WriteLine(Util.StringJustification("Request : " + Util.ByteArrayToHexStringW(Util.GetByteArrayFromService(((SvcWriter.Request) this._lstRequests[this._iLastMessageSend]).Buf), ' '), "Request : 0000 ".Length, 80));
        this._msgClient.SendRequest(((SvcWriter.Request) this._lstRequests[this._iLastMessageSend]).Buf, ((SvcWriter.Request) this._lstRequests[this._iLastMessageSend]).Len);
      }
    }

    private void _msgClient_OnDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      this.OnDiagnosticMsg(Sender, DiagnosticCode.DiagCode.ToString(), DiagnosticCode.AddDiagCode);
    }

    private void OnDiagnosticMsg(object sender, string Message, byte[] Confirmation)
    {
      if (this._lstRequests == null)
        return;
      if (Confirmation == null)
        Confirmation = new byte[0];
      if (this._swLogFile != null)
      {
        string hexStringW = Util.ByteArrayToHexStringW(Util.GetByteArrayFromService(Confirmation), ' ');
        this._swLogFile.WriteLine(Util.StringJustification(Message + ": " + hexStringW, Message.Length + 2, 80));
        this._swLogFile.WriteLine("SVC Download halted.");
        this._swLogFile.Close();
        this._swLogFile = (StreamWriter) null;
      }
      this._locDiagnostic.SetDiagnostic(sender, (Enum) SvcWriterResult.WritingFailed, Util.Int32ToByteArray(((SvcWriter.Request) this._lstRequests[this._iLastMessageSend]).LineNo, 2));
      this._lstRequests.Clear();
      this._iLastMessageSend = 0;
      this._removeMsgClient = true;
    }

    internal bool Parse(string FileName)
    {
      return this.Parse(FileName, (string) null);
    }

    internal bool Parse(string FileName, string LogFile)
    {
      FileStream fileStream1 = (FileStream) null;
      FileStream fileStream2 = (FileStream) null;
      try
      {
        if (FileName.Length > 0)
          fileStream1 = new FileStream(FileName, FileMode.Open);
        else
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) SvcWriterResult.NoSvcFilename, (byte[]) null);
        if (LogFile.Length > 0)
          fileStream2 = new FileStream(LogFile, FileMode.Create);
        return this.Parse((Stream) fileStream1, (Stream) fileStream2, FileName);
      }
      catch
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) SvcWriterResult.FileNotFoundError, (byte[]) null);
        return false;
      }
    }

    internal bool Parse(Stream ParseStream, Stream LogStream, string FileName)
    {
      StreamWriter LogData = (StreamWriter) null;
      if (LogStream != null)
        LogData = new StreamWriter(LogStream);
      return this.Parse(new StreamReader(ParseStream), LogData, FileName);
    }

    private bool Parse(StreamReader SVCData, StreamWriter LogData, string FileName)
    {
      this._lstRequests = new ArrayList();
      byte[] numArray = new byte[1024];
      int index = 0;
      int num1 = 0;
      int Integer = 0;
      int num2 = 0;
      this._swLogFile = LogData;
      if (LogData != null)
        LogData.WriteLine("SVC Download log, SVC Writer Name: " + this.Name + "\r\nSVC Filename: " + FileName + "\r\nDate: " + (object) DateTime.Now + "\r\n");
      SvcWriter.Request request;
      try
      {
        while (true)
        {
          string str1;
          do
          {
            string str2;
            int num3;
            do
            {
              str2 = SVCData.ReadLine();
              ++Integer;
              if (str2 != null)
              {
                num3 = str2.LastIndexOf("#");
                if (str2 != "" && str2[0] != '#' && num3 != -1)
                  throw new Exception();
              }
              else
                goto label_13;
            }
            while (num3 == -1);
            str1 = str2.Substring(1, num3 - 1);
            switch (num1)
            {
              case 0:
                continue;
              case 1:
                goto label_9;
              default:
                continue;
            }
          }
          while (!(str1 == "CMD"));
          num2 = Integer;
          num1 = 1;
          continue;
label_9:
          if (str1 == "CMD")
          {
            request = new SvcWriter.Request();
            request.Len = index;
            request.Buf = numArray;
            request.LineNo = num2;
            num2 = Integer;
            this._lstRequests.Add((object) request);
            numArray = new byte[1024];
            index = 0;
          }
          else
          {
            int num3 = int.Parse(str1.Substring(2, str1.Length - 2), NumberStyles.AllowHexSpecifier);
            numArray[index] = Convert.ToByte(num3 >> 8 & (int) byte.MaxValue);
            numArray[index + 1] = Convert.ToByte(num3 & (int) byte.MaxValue);
            index += 2;
          }
        }
      }
      catch (Exception ex)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) SvcWriterResult.ParseError, Util.Int32ToByteArray(Integer, 2));
        this._lstRequests.Clear();
        this._iLastMessageSend = 0;
        return false;
      }
label_13:
      request = new SvcWriter.Request();
      request.Len = index;
      request.Buf = numArray;
      request.LineNo = num2;
      this._lstRequests.Add((object) request);
      request = (SvcWriter.Request) this._lstRequests[0];
      if (LogData != null)
        LogData.WriteLine(Util.StringJustification("Request  " + Util.ByteArrayToHexStringW(Util.GetByteArrayFromService(request.Buf), ' '), "Request  0000 ".Length, 80));
      this._msgClient.SendRequest(request.Buf, request.Len);
      return true;
    }

    internal void Dispose()
    {
      if (this._locDiagnostic != null)
      {
        this._locDiagnostic.Dispose();
        this._locDiagnostic = (Diagnostic) null;
      }
      GC.SuppressFinalize((object) this);
    }

    private struct Request
    {
      internal int Len;
      internal byte[] Buf;
      internal int LineNo;
    }
  }
}
