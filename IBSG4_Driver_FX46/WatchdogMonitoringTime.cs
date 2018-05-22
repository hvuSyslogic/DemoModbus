// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.WatchdogMonitoringTime
// Assembly: IBSG4_Driver_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: BA38E233-77EA-4C5F-9C3F-E03C7CD687CE
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\IBSG4_Driver_FX46.dll

namespace PhoenixContact.DDI
{
  public enum WatchdogMonitoringTime
  {
    Intervall_8ms = 0,
    Intervall_16ms = 4,
    Intervall_32ms = 8,
    Intervall_65ms = 12, // 0x0000000C
    Intervall_131ms = 16, // 0x00000010
    Intervall_262ms = 20, // 0x00000014
    Intervall_524ms = 24, // 0x00000018
    Intervall_1048ms = 28, // 0x0000001C
  }
}
