// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.RegistryAccess
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using Microsoft.Win32;
using System;
using System.Globalization;

namespace PhoenixContact.PxC_Library.Util
{
  public class RegistryAccess
  {
    private static RegistryKey GetRegistryRootKey(RegistryRootKeys pRootKey)
    {
      RegistryKey registryKey = (RegistryKey) null;
      switch (pRootKey)
      {
        case RegistryRootKeys.HKEY_CLASSES_ROOT:
          registryKey = Registry.ClassesRoot;
          break;
        case RegistryRootKeys.HKEY_CURRENT_CONFIG:
          registryKey = Registry.CurrentConfig;
          break;
        case RegistryRootKeys.HKEY_CURRENT_USER:
          registryKey = Registry.CurrentUser;
          break;
        case RegistryRootKeys.HKEY_LOCAL_MACHINE:
          registryKey = Registry.LocalMachine;
          break;
        case RegistryRootKeys.HKEY_PERFORMANCE_DATA:
          registryKey = Registry.PerformanceData;
          break;
        case RegistryRootKeys.HKEY_USERS:
          registryKey = Registry.Users;
          break;
      }
      return registryKey;
    }

    public static object ReadValue(RegistryRootKeys pRootKey, string pKeyPath, string pValueName, object pDefaultValue)
    {
      RegistryKey registryKey = RegistryAccess.GetRegistryRootKey(pRootKey).OpenSubKey(pKeyPath);
      object obj = pDefaultValue;
      if (registryKey != null)
        obj = registryKey.GetValue(pValueName);
      return obj;
    }

    public static void WriteValue(RegistryRootKeys pRootKey, string pKeyPath, string valueName, object pValue, bool pCreateIfNotExist)
    {
      RegistryKey registryKey1 = RegistryAccess.GetRegistryRootKey(pRootKey);
      string[] strArray = pKeyPath.Split('\\');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (registryKey1 != null)
        {
          RegistryKey registryKey2 = registryKey1.OpenSubKey(strArray[index], true);
          if (registryKey2 == null & pCreateIfNotExist)
            registryKey2 = registryKey1.CreateSubKey(strArray[index]);
          registryKey1 = registryKey2;
        }
      }
      if (registryKey1 == null)
        throw new Exception(string.Format((IFormatProvider) CultureInfo.InvariantCulture, RegistryAccessStrings.WriteValueError, (object) pKeyPath, (object) pRootKey.ToString()));
      registryKey1.SetValue(valueName, pValue);
    }

    public static void DeleteValue(RegistryRootKeys pRootKey, string pKeyPath, string valueName)
    {
      RegistryKey registryKey = RegistryAccess.GetRegistryRootKey(pRootKey).OpenSubKey(pKeyPath, true);
      if (registryKey == null)
        throw new Exception(string.Format((IFormatProvider) CultureInfo.InvariantCulture, RegistryAccessStrings.DeleteValueError, (object) pKeyPath, (object) pRootKey.ToString()));
      registryKey.DeleteValue(valueName, false);
    }

    public static void DeleteKey(RegistryRootKeys pRootKey, string pKeyPath)
    {
      int length = pKeyPath.LastIndexOf("\\");
      string name = pKeyPath.Substring(0, length);
      string subkey = pKeyPath.Substring(length + 1, pKeyPath.Length - length - 1);
      RegistryKey registryKey = RegistryAccess.GetRegistryRootKey(pRootKey).OpenSubKey(name, true);
      if (registryKey == null)
        throw new Exception(string.Format((IFormatProvider) CultureInfo.InvariantCulture, RegistryAccessStrings.DeleteKeyError, (object) pKeyPath, (object) pRootKey.ToString()));
      registryKey.DeleteSubKey(subkey);
    }
  }
}
