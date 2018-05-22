// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.IniFile
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System.Runtime.InteropServices;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public class IniFile
  {
    public string path;

    public IniFile(string INIFilePath)
    {
      this.path = INIFilePath;
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public void IniWriteValue(string Section, string Key, string Value)
    {
      IniFile.WritePrivateProfileString(Section, Key, Value, this.path);
    }

    public string IniReadValue(string Section, string Key)
    {
      StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
      IniFile.GetPrivateProfileString(Section, Key, "", retVal, (int) byte.MaxValue, this.path);
      return retVal.ToString();
    }
  }
}
