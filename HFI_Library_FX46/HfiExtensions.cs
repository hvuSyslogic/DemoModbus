// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.HfiExtensions
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  internal static class HfiExtensions
  {
    public static string CopyConfToFwInfo(this byte[] data, int fromWord, int toWord)
    {
      string str = "";
      if (fromWord >= toWord || toWord > data.Length)
        return str;
      for (int index = fromWord; index <= toWord && data[index] != (byte) 0; ++index)
        str += ((char) data[index]).ToString();
      return str;
    }
  }
}
