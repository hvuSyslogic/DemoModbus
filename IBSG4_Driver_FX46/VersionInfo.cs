// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.VersionInfo
// Assembly: IBSG4_Driver_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: BA38E233-77EA-4C5F-9C3F-E03C7CD687CE
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\IBSG4_Driver_FX46.dll

using System.Text;

namespace PhoenixContact.DDI
{
  public struct VersionInfo
  {
    public string Vendor;
    public string Name;
    public string Revision;
    public string DateTime;
    public int RevNumber;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Vendor:        " + this.Vendor);
      stringBuilder.AppendLine("Name:          " + this.Name);
      stringBuilder.AppendLine("Revision:      " + this.Revision);
      stringBuilder.AppendLine("Date and time: " + this.DateTime);
      stringBuilder.AppendLine("Revision nr.:  " + (object) this.RevNumber);
      return stringBuilder.ToString();
    }
  }
}
