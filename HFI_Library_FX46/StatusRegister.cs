// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.StatusRegister
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;
using System;

namespace PhoenixContact.HFI.Inline
{
  public struct StatusRegister
  {
    public bool USER;
    public bool PF;
    public bool BUS;
    public bool CTRL;
    public bool DETECT;
    public bool RUN;
    public bool ACTIVE;
    public bool READY;
    public bool BSA;
    public bool BASP;
    public bool RESULT;
    public bool SY_RESULT;
    public bool DC_RESULT;
    public bool WARNING;
    public bool QUALITY;
    public bool SDSI;
    public int Value;

    public override string ToString()
    {
      return string.Format((IFormatProvider) new BinaryFormatter(), "{0:B}", (object) (ushort) this.Value);
    }
  }
}
