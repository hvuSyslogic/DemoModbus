// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.RevisionInformation
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  public class RevisionInformation
  {
    public RevisionInformation()
    {
      this.Firmware = new FirmwareInformation();
      this.Host = new HostInformation();
      this.StartFirmware = new StartFirmwareInformation();
      this.Hardware = new HardwareInformation();
      this.Delete();
    }

    public FirmwareInformation Firmware { get; private set; }

    public HostInformation Host { get; private set; }

    public StartFirmwareInformation StartFirmware { get; private set; }

    public HardwareInformation Hardware { get; private set; }

    internal void SetData(byte[] data)
    {
      this.Firmware.SetData(data);
      this.Host.SetData(data);
      this.StartFirmware.SetData(data);
      this.Hardware.SetData(data);
    }

    internal void Delete()
    {
      this.Firmware.Delete();
      this.Host.Delete();
      this.StartFirmware.Delete();
      this.Hardware.Delete();
    }

    public override string ToString()
    {
      return this.Firmware.ToString() + "\r\n" + (object) this.Host + "\r\n" + (object) this.StartFirmware + "\r\n" + (object) this.Hardware;
    }
  }
}
