// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.AppConfig
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Configuration;
using System.IO;

namespace PhoenixContact.PxC_Library.Util
{
  public class AppConfig
  {
    private static readonly object m_lock = new object();
    private static volatile AppConfig instance = (AppConfig) null;
    private readonly string CryptKey = "5be9bb47-4e40-4453-add0-ed1e0210a8e5";
    private readonly object tLock = new object();
    private string _filePathAndName;
    private System.Configuration.Configuration config;

    private AppConfig()
    {
    }

    public static AppConfig GetInstance()
    {
      if (AppConfig.instance == null)
      {
        lock (AppConfig.m_lock)
        {
          if (AppConfig.instance == null)
            AppConfig.instance = new AppConfig();
        }
      }
      return AppConfig.instance;
    }

    public bool Open { get; private set; }

    public bool OpenConfigurationWithFile(string FilePathAndName)
    {
      if (!File.Exists(FilePathAndName))
        return false;
      this._filePathAndName = FilePathAndName;
      this.config = (System.Configuration.Configuration) null;
      this.config = ConfigurationManager.OpenExeConfiguration(this._filePathAndName);
      if (this.config != null)
      {
        this.Open = true;
        return true;
      }
      this.Open = false;
      return false;
    }

    public bool OpenConfigurationWithUserLevel(ConfigurationUserLevel Level)
    {
      this.config = (System.Configuration.Configuration) null;
      this.config = ConfigurationManager.OpenExeConfiguration(Level);
      if (this.config != null)
      {
        this.Open = true;
        return true;
      }
      this.Open = false;
      return false;
    }

    public void CloseConfiguration()
    {
      lock (this.tLock)
      {
        this.config = (System.Configuration.Configuration) null;
        this.Open = false;
      }
    }

    public string ReadValue(string Key)
    {
      if (this.config == null | !this.Open)
        return string.Empty;
      lock (this.tLock)
      {
        if (this.config.AppSettings.Settings[Key] != null)
          return this.config.AppSettings.Settings[Key].Value;
        return string.Empty;
      }
    }

    public string ReadCryptValue(string Key)
    {
      if (this.config == null | !this.Open)
        return string.Empty;
      lock (this.tLock)
      {
        string Output;
        if (this.DecryptString(this.ReadValue(Key), out Output) == null)
          return Output;
        return string.Empty;
      }
    }

    public bool InitCryptValue(string Key, string Value)
    {
      if (this.config == null | !this.Open)
        return false;
      lock (this.tLock)
      {
        string CryptString;
        if (this.CryptString(Value, out CryptString) == null)
          return this.InitValue(Key, CryptString);
        return false;
      }
    }

    public bool InitValue(string Key, string Value)
    {
      if (this.config == null | !this.Open)
        return false;
      lock (this.tLock)
      {
        if (this.config.AppSettings.Settings[Key] == null)
        {
          this.config.AppSettings.Settings.Add(Key, Value);
          this.config.Save(ConfigurationSaveMode.Modified);
          ConfigurationManager.RefreshSection("appSettings");
          return true;
        }
        if (!(this.config.AppSettings.Settings[Key].Value == string.Empty))
          return false;
        this.config.AppSettings.Settings[Key].Value = Value;
        this.config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
        return true;
      }
    }

    public bool WriteValue(string Key, string Value)
    {
      if (this.config == null | !this.Open)
        return false;
      lock (this.tLock)
      {
        if (this.config.AppSettings.Settings[Key] == null)
          return this.InitValue(Key, Value);
        this.config.AppSettings.Settings[Key].Value = Value;
        this.config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
        return true;
      }
    }

    public bool WriteCryptValue(string Key, string Value)
    {
      if (this.config == null | !this.Open)
        return false;
      lock (this.tLock)
      {
        string CryptString;
        if (this.CryptString(Value, out CryptString) == null)
          return this.WriteValue(Key, CryptString);
        return false;
      }
    }

    private Exception CryptString(string Input, out string CryptString)
    {
      int index = 0;
      CryptString = string.Empty;
      try
      {
        foreach (int num1 in Input)
        {
          int num2 = num1 + (int) this.CryptKey[index];
          if (num2 > (int) byte.MaxValue)
            num2 -= (int) byte.MaxValue;
          CryptString += ((char) num2).ToString();
          ++index;
          if (index > this.CryptKey.Length - 1)
            index = 0;
        }
        return (Exception) null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    private Exception DecryptString(string CryptString, out string Output)
    {
      int index = 0;
      Output = string.Empty;
      try
      {
        foreach (int num1 in CryptString)
        {
          int num2 = num1 - (int) this.CryptKey[index];
          if (num2 < 0)
            num2 = (int) byte.MaxValue + num2;
          Output += ((char) num2).ToString();
          ++index;
          if (index > this.CryptKey.Length - 1)
            index = 0;
        }
        return (Exception) null;
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
  }
}
