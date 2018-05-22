// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.FirmwareServiceList
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using System.Collections.Generic;

namespace PhoenixContact.HFI.Inline
{
  internal class FirmwareServiceList
  {
    private readonly List<FirmwareService> firmwareServiceList;
    private readonly object accessLock;

    internal FirmwareServiceList()
    {
      this.firmwareServiceList = new List<FirmwareService>();
      this.accessLock = new object();
    }

    internal int Count
    {
      get
      {
        return this.firmwareServiceList.Count;
      }
    }

    internal void AddFirmwareService(FirmwareService service)
    {
      lock (this.accessLock)
      {
        if (service == null)
          return;
        this.firmwareServiceList.Add(service);
      }
    }

    internal FirmwareService GetNextFirmwareService()
    {
      lock (this.accessLock)
      {
        if (this.firmwareServiceList.Count <= 0)
          return (FirmwareService) null;
        FirmwareService firmwareService = this.firmwareServiceList[0].Clone() as FirmwareService;
        this.firmwareServiceList.RemoveAt(0);
        return firmwareService;
      }
    }

    internal void DeleteAll()
    {
      lock (this.accessLock)
        this.firmwareServiceList.Clear();
    }
  }
}
