// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.Ticker.ITick
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System.Threading;

namespace PhoenixContact.Common.Ticker
{
  public interface ITick
  {
    string Name { get; }

    bool Ready { get; }

    bool Error { get; }

    int Intervall { get; set; }

    ThreadPriority Priority { get; set; }

    bool Enable();

    bool Disable();

    bool DisableAsync();

    event TickerHandler OnEnable;

    event TickerHandler OnDisable;

    event TickerHandler OnTick;

    event TickerDiagnosticHandler OnDiagnostic;
  }
}
