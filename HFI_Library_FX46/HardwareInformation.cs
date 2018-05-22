// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.HardwareInformation
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  public class HardwareInformation
  {
    public string ArticleNumber { get; private set; }

    public string Name { get; private set; }

    public string MotherboardId { get; private set; }

    public string Version { get; private set; }

    public string Vendor { get; private set; }

    public string SerialNumber { get; private set; }

    public string Date { get; private set; }

    internal void SetData(byte[] data)
    {
      this.ArticleNumber = data.CopyConfToFwInfo(92, 99);
      this.Name = data.CopyConfToFwInfo(100, 129);
      this.MotherboardId = data.CopyConfToFwInfo(130, 133);
      this.Version = data.CopyConfToFwInfo(134, 135);
      this.Vendor = data.CopyConfToFwInfo(136, 155);
      this.SerialNumber = data.CopyConfToFwInfo(156, 167);
      this.Date = data.CopyConfToFwInfo(168, 173);
    }

    internal void Delete()
    {
      this.ArticleNumber = string.Empty;
      this.Name = string.Empty;
      this.MotherboardId = string.Empty;
      this.Version = string.Empty;
      this.Vendor = string.Empty;
      this.SerialNumber = string.Empty;
      this.Date = string.Empty;
    }

    public override string ToString()
    {
      return "Hardware:\r\n\tArticleNumber: " + this.ArticleNumber + "\r\n\tName: " + this.Name + "\r\n\tMotherboard ID: " + this.MotherboardId + "\r\n\tVersion: " + this.Version + "\r\n\tDate: " + this.Date;
    }
  }
}
