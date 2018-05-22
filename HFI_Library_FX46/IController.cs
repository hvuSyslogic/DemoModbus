// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.IController
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;
using System;
using System.Collections.ObjectModel;

namespace PhoenixContact.HFI.Inline
{
  public interface IController : IDisposable
  {
    string VersionInfo { get; }

    string Name { get; set; }

    string Description { get; set; }

    ControllerStartup Startup { get; set; }

    int UpdateProcessDataCycleTime { get; set; }

    int UpdateMailboxTime { get; set; }

    bool Connect { get; }

    bool Run { get; }

    bool Error { get; }

    string InternalState { get; }

    ReadOnlyCollection<VarInput> InputObjectList { get; }

    ReadOnlyCollection<VarOutput> OutputObjectList { get; }

    ReadOnlyCollection<MessageClient> MessageObjectList { get; }

    bool WatchdogOccurred { get; }

    WatchdogMonitoringTime WatchdogTimeout { get; set; }

    bool WatchdogDeactivate { get; set; }

    string Connection { get; set; }

    bool WatchdogClear();

    bool Enable();

    void Disable();

    bool AutoStart();

    int AddObject(VarInput InputObject);

    bool RemoveObject(VarInput InputObject);

    int AddObject(VarOutput OutputObject);

    bool RemoveObject(VarOutput OutputObject);

    int InputObjectStartAddress { get; }

    int InputObjectEndAddress { get; }

    int InputObjectLength { get; }

    int OutputObjectStartAddress { get; }

    int OutputObjectEndAddress { get; }

    int OutputObjectLength { get; }

    event UpdateProcessDataHandler OnUpdateProcessData;

    int AddObject(MessageClient MessageObject);

    bool RemoveObject(MessageClient MessageObject);

    event UpdateMailboxHandler OnUpdateMailbox;

    event ExceptionHandler OnException;

    event ControllerEventHandler OnRun;

    event ControllerEventHandler OnStop;

    event ControllerEventHandler OnConnect;

    event ControllerEventHandler OnDisable;
  }
}
