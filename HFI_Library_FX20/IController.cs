// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.IController
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;
using System.Collections;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public interface IController
  {
    string VersionInfo { get; }

    string Name { get; set; }

    string Description { get; set; }

    ControllerStartup Startup { get; set; }

    string SvcFileName { get; set; }

    int UpdateProcessDataCycleTime { get; set; }

    int UpdateMailboxTime { get; set; }

    bool Ready { get; }

    bool Error { get; }

    ArrayList InputObjectList { get; }

    ArrayList OutputObjectList { get; }

    ArrayList MessageObjectList { get; }

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

    int InputObjectCount { get; }

    int InputObjectStartAddress { get; }

    int InputObjectEndAddress { get; }

    int InputObjectLength { get; }

    int OutputObjectCount { get; }

    int OutputObjectStartAddress { get; }

    int OutputObjectEndAddress { get; }

    int OutputObjectLength { get; }

    event UpdateProcessDataHandler OnUpdateProcessData;

    int AddObject(MessageClient MessageObject);

    bool RemoveObject(MessageClient MessageObject);

    event UpdateMailboxHandler OnUpdateMailbox;

    event DiagnosticHandler OnDiagnostic;
  }
}
