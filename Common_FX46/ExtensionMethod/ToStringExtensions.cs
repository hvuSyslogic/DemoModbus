// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.ExtensionMethod.ToStringExtensions
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace PhoenixContact.Common.ExtensionMethod
{
  public static class ToStringExtensions
  {
    private static readonly object lockArrayToString = new object();
    private static readonly object lockByteToString = new object();
    private static readonly object lockBitArrayToString = new object();

    public static string ToStringW(this ushort[] array, char seperator)
    {
      lock (ToStringExtensions.lockArrayToString)
      {
        StringBuilder stringBuilder = new StringBuilder(string.Empty);
        if (array != null && array.Length != 0)
        {
          for (int index = 0; index < array.Length; ++index)
          {
            stringBuilder.Append(array[index].ToString("X4"));
            if (index < array.Length - 1)
              stringBuilder.Append(seperator);
          }
        }
        return stringBuilder.ToString();
      }
    }

    public static string ToStringW(this byte[] array, char seperator)
    {
      return array.ToStringW(seperator.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static string ToStringW(this byte[] array, string seperator)
    {
      lock (ToStringExtensions.lockByteToString)
      {
        StringBuilder stringBuilder = new StringBuilder(string.Empty);
        if (array != null)
        {
          for (int index = 0; index < array.Length; ++index)
          {
            stringBuilder.Append(array[index].ToString("X2"));
            if ((index + 1) % 2 == 0 && index > 0 && (index < array.Length - 1 && seperator != null))
              stringBuilder.Append(seperator);
          }
        }
        return stringBuilder.ToString();
      }
    }

    public static string ToString(this byte[] array, char seperator)
    {
      return array.ToString(seperator.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static string ToString(this byte[] array, string seperator)
    {
      lock (ToStringExtensions.lockByteToString)
      {
        StringBuilder stringBuilder = new StringBuilder(string.Empty);
        if (array != null)
        {
          for (int index = 0; index < array.Length; ++index)
          {
            stringBuilder.Append(array[index].ToString("X2"));
            if (index < array.Length - 1 && seperator != null)
              stringBuilder.Append(seperator);
          }
        }
        return stringBuilder.ToString();
      }
    }

    public static string ToAsciiString(this byte[] array)
    {
      lock (ToStringExtensions.lockByteToString)
      {
        if (array == null)
          return string.Empty;
        string str = new ASCIIEncoding().GetString(array, 0, array.Length);
        int length = str.IndexOf(char.MinValue);
        if (length >= 0)
          str = str.Substring(0, length);
        return str.TrimEnd(' ');
      }
    }

    public static string ToString(this BitArray array, uint bitGroups, char seperator)
    {
      lock (ToStringExtensions.lockBitArrayToString)
      {
        StringBuilder stringBuilder = new StringBuilder(string.Empty);
        if (array != null)
        {
          for (int index = array.Length - 1; index >= 0; --index)
          {
            stringBuilder.Append(array[index] ? "1" : "0");
            if (bitGroups > 0U && (long) index % (long) bitGroups == 0L && (index > 0 && index < array.Length))
              stringBuilder.Append(seperator);
          }
        }
        return stringBuilder.ToString();
      }
    }
  }
}
