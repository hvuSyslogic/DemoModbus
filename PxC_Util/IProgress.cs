// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.IProgress
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

namespace PhoenixContact.PxC_Library.Util
{
  internal interface IProgress
  {
    long CountActual { get; }

    long CountMax { get; }

    Progress InnerProgress { get; set; }

    string Name { get; set; }

    string Parameter { get; }

    int Percent { get; }

    Progress.States State { get; }

    bool AbortProgress(string pParameter);

    event Progress.ChangeHandler OnChange;

    bool Reset();

    bool SetCoutMax(long pCountMax);

    void SetProgress(long pCountActual);

    void SetProgress(long pCountActual, long pCountMax, string pParameter);

    void SetProgress(long pCountActual, string pParameter);

    string ToString();
  }
}
