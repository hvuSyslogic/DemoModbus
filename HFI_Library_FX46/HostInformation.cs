// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.HostInformation
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  public class HostInformation
  {
    public string Type { get; private set; }

    public string Version { get; private set; }

    public string State { get; private set; }

    public string Date { get; private set; }

    public string Time { get; private set; }

    internal void SetData(byte[] data)
    {
      this.Type = data.CopyConfToFwInfo(28, 47);
      this.Version = data.CopyConfToFwInfo(48, 51);
      this.State = data.CopyConfToFwInfo(52, 57);
      this.Date = data.CopyConfToFwInfo(58, 63);
      this.Time = data.CopyConfToFwInfo(64, 69);
    }

    internal void Delete()
    {
      this.Type = string.Empty;
      this.Version = string.Empty;
      this.State = string.Empty;
      this.Date = string.Empty;
      this.Time = string.Empty;
    }

    public override string ToString()
    {
      return "Host:\r\n\tType: " + this.Type + "\r\n\tVersion: " + this.Version + "\r\n\tState: " + this.State + "\r\n\tDate: " + this.Date + "\r\n\tTime: " + this.Time;
    }
  }
}
