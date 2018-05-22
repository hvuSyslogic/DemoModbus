// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.ExtensionMethod.StringExtensions
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PhoenixContact.Common.ExtensionMethod
{
  public static class StringExtensions
  {
    private static readonly object lockToByteArray = new object();

    public static string GetRandomString(int length)
    {
      byte[] data = new byte[4];
      new RNGCryptoServiceProvider().GetBytes(data);
      string element = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      Random random = new Random(BitConverter.ToInt32(data, 0));
      int count = length;
      return new string(Enumerable.Repeat<string>(element, count).Select<string, char>((Func<string, char>) (s => s[random.Next(s.Length)])).ToArray<char>());
    }

    public static byte[] ToByteArray(this string myString)
    {
      return Encoding.ASCII.GetBytes(myString);
    }

    public static byte[] ToHexByteArray(this string myString)
    {
      return Enumerable.Range(0, myString.Length).Where<int>((Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(myString.Substring(x, 2), 16))).ToArray<byte>();
    }

    public static string CryptString(this string input, string cryptKey)
    {
      int index = 0;
      string empty = string.Empty;
      foreach (int num1 in input)
      {
        int num2 = num1 + (int) cryptKey[index];
        if (num2 > (int) byte.MaxValue)
          num2 -= (int) byte.MaxValue;
        empty += ((char) num2).ToString();
        ++index;
        if (index > cryptKey.Length - 1)
          index = 0;
      }
      return empty;
    }

    public static string DecryptString(this string cryptString, string cryptKey)
    {
      int index = 0;
      string empty = string.Empty;
      foreach (int num1 in cryptString)
      {
        int num2 = num1 - (int) cryptKey[index];
        if (num2 < 0)
          num2 = (int) byte.MaxValue + num2;
        empty += ((char) num2).ToString();
        ++index;
        if (index > cryptKey.Length - 1)
          index = 0;
      }
      return empty;
    }
  }
}
