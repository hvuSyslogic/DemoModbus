// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.WinUtils
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace PhoenixContact.PxC_Library.Util
{
  public static class WinUtils
  {
    private const string REGISTRY_HKCU_AUTOSTART = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

    public static void RestartSystem(int pTime)
    {
      if (pTime >= 256)
        return;
      WinUtils.StartShutDown(string.Format("-r -t {0} -f ", (object) pTime));
    }

    public static void LogOffUser()
    {
      WinUtils.StartShutDown("-l");
    }

    public static void ShutDownSystem(int pTime)
    {
      if (pTime >= 256)
        return;
      WinUtils.StartShutDown(string.Format("-s -t {0} -f", (object) pTime));
    }

    private static void StartShutDown(string pConfig)
    {
      try
      {
        Process.Start(new ProcessStartInfo()
        {
          FileName = "cmd",
          Arguments = "/C shutdown " + pConfig,
          UseShellExecute = true,
          WindowStyle = ProcessWindowStyle.Hidden
        });
      }
      catch
      {
      }
    }

    public static bool SetAutostart(string pKeyName, string pPathAndName, string pArguments)
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (!File.Exists(pPathAndName))
          return false;
        registryKey.SetValue(pKeyName, (object) (pPathAndName + (pArguments != null ? " " + pArguments : "")));
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool RemoveAutostart(string pKeyName)
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (registryKey.GetValue(pKeyName) == null)
          return false;
        registryKey.DeleteValue(pKeyName);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
