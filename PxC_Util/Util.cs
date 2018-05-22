// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.Util
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PhoenixContact.PxC_Library.Util
{
  public static class Util
  {
    public static bool Int32ToBool(int Integer, int BitPosition)
    {
      bool flag = false;
      if (BitPosition < 32)
        flag = Convert.ToBoolean(Integer >> BitPosition & 1);
      return flag;
    }

    public static int ByteToInt32(byte Byte_0, byte Byte_1)
    {
      return (int) Byte_0 << 8 | (int) Byte_1;
    }

    public static int ByteToInt32(byte Byte_0, byte Byte_1, byte Byte_2, byte Byte_3)
    {
      return (((int) Byte_0 << 8 | (int) Byte_1) << 8 | (int) Byte_2) << 8 | (int) Byte_3;
    }

    public static byte Int32ToByte(int Integer, int BitPosition)
    {
      byte num = 0;
      if (BitPosition >= 0 && BitPosition <= 24)
        num = Convert.ToByte(Integer >> BitPosition & (int) byte.MaxValue);
      return num;
    }

    public static byte[] Int32ToByteArray(int Integer, int ByteArrayLength)
    {
      byte[] numArray = new byte[0];
      if (ByteArrayLength > 0 && ByteArrayLength <= 4)
      {
        numArray = new byte[ByteArrayLength];
        for (int index = 0; index < ByteArrayLength; ++index)
          numArray[ByteArrayLength - 1 - index] = Convert.ToByte(Integer >> index * 8 & (int) byte.MaxValue);
      }
      return numArray;
    }

    public static byte[] UInt64ToByteArray(ulong Integer, int ByteArrayLength)
    {
      byte[] numArray = new byte[0];
      if (ByteArrayLength > 0 && ByteArrayLength <= 8)
      {
        numArray = new byte[ByteArrayLength];
        for (int index = 0; index < ByteArrayLength; ++index)
          numArray[ByteArrayLength - 1 - index] = Convert.ToByte(Integer >> index * 8 & (ulong) byte.MaxValue);
      }
      return numArray;
    }

    public static byte[] GetByteArrayElements(byte[] Source, int StartAddress, int DestinationByteArrayLength)
    {
      byte[] numArray = new byte[0];
      if (Source != null && StartAddress < Source.Length && StartAddress >= 0)
      {
        numArray = new byte[DestinationByteArrayLength];
        for (int index = 0; index < DestinationByteArrayLength; ++index)
          numArray[index] = StartAddress + index >= Source.Length ? (byte) 0 : Source[StartAddress + index];
      }
      return numArray;
    }

    public static byte[] GetByteArrayElements(byte[] Source, int StartAddress)
    {
      byte[] numArray = new byte[0];
      if (Source != null)
      {
        int DestinationByteArrayLength = Source.Length - StartAddress;
        numArray = PhoenixContact.PxC_Library.Util.Util.GetByteArrayElements(Source, StartAddress, DestinationByteArrayLength);
      }
      return numArray;
    }

    public static byte[] GetByteArrayFromService(byte[] Source)
    {
      byte[] numArray = new byte[0];
      if (Source != null && Source.Length >= 4)
      {
        int DestinationByteArrayLength = PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Source[2], Source[3]) * 2 + 4;
        numArray = PhoenixContact.PxC_Library.Util.Util.GetByteArrayElements(Source, 0, DestinationByteArrayLength);
      }
      return numArray;
    }

    public static string Int32ToString(int Integer, int StringLength)
    {
      string str1 = string.Empty;
      string empty = string.Empty;
      if (StringLength > 0 && StringLength <= 8)
      {
        string str2 = Integer.ToString("X");
        if (str2.Length <= StringLength)
        {
          for (int index = 0; index < StringLength - str2.Length; ++index)
            str1 += "0";
          str1 += str2;
        }
        else
          str1 = str2.Substring(str2.Length - StringLength, StringLength);
      }
      return str1;
    }

    public static string ByteToHexString(byte Source)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      return Source.ToString("X").Length != 1 ? empty1 + Source.ToString("X") : empty1 + "0" + Source.ToString("X");
    }

    public static string ByteArrayToAsciiString(byte[] Source)
    {
      if (Source != null)
        return new ASCIIEncoding().GetString(Source, 0, Source.Length);
      return string.Empty;
    }

    public static byte[] AsciiStringToByteArray(string Source)
    {
      if (Source != null)
        return new ASCIIEncoding().GetBytes(Source);
      return new byte[0];
    }

    public static byte[] HexStringToByteArray(string Source)
    {
      if (Source == null)
        return new byte[0];
      byte[] numArray = new byte[Source.Length / 2 + (Source.Length % 2 == 0 ? 0 : 1)];
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (Source.Length >= index * 2 + 2)
          numArray[index] = Convert.ToByte(Source.Substring(index * 2, 2), 16);
        else if (Source.Length >= index * 2 + 1)
          numArray[index] = Convert.ToByte(Source.Substring(index * 2, 1), 16);
      }
      return numArray;
    }

    public static string ByteArrayToHexStringW(byte[] Source, char Seperator)
    {
      string empty = string.Empty;
      if (Source != null)
      {
        for (int index = 0; index < Source.Length; ++index)
        {
          empty += PhoenixContact.PxC_Library.Util.Util.ByteToHexString(Source[index]);
          if ((index + 1) % 2 == 0 && index > 0 && index < Source.Length - 1)
            empty += Seperator.ToString();
        }
      }
      return empty;
    }

    public static string StringJustification(string Text, int StartPosition, int LineLength)
    {
      StringBuilder stringBuilder = new StringBuilder(string.Empty);
      int num = 0;
      if (StartPosition < LineLength)
      {
        for (int startIndex = 0; startIndex < Text.Length; ++startIndex)
        {
          if (Text.Length > startIndex + 2 && Text.Substring(startIndex, 2).StartsWith("\r\n"))
          {
            num = 0;
            stringBuilder.Append("\r\n");
            for (int index = 0; index < StartPosition; ++index)
              stringBuilder.Append(" ");
            ++startIndex;
          }
          else
          {
            stringBuilder.Append(Text[startIndex]);
            ++num;
            if (Text[startIndex] == ' ' && num + Text.Substring(startIndex + 1).IndexOf(' ') > LineLength - StartPosition)
            {
              stringBuilder.Append("\r\n");
              for (int index = 0; index < StartPosition; ++index)
                stringBuilder.Append(" ");
              num = 0;
            }
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static bool CheckPcpConfirmation(int Service)
    {
      switch (Service)
      {
        case 32897:
        case 32898:
        case 32899:
        case 32900:
        case 32902:
        case 32903:
        case 32904:
        case 32905:
        case 32906:
        case 32907:
        case 32912:
        case 32913:
        case 32914:
        case 32915:
        case 32916:
        case 32917:
        case 32918:
        case 32919:
        case 32920:
        case 32928:
        case 32961:
          return true;
        default:
          return false;
      }
    }

    public static bool CheckPcpConfirmation(byte Byte_0, byte Byte_1)
    {
      return PhoenixContact.PxC_Library.Util.Util.CheckPcpConfirmation(PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Byte_0, Byte_1));
    }

    public static bool IsIpAddress(string IpAddress)
    {
      return Regex.IsMatch(IpAddress, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
    }

    public static bool IsSubnetMask(string SubnetMask)
    {
      if (string.IsNullOrEmpty(SubnetMask))
        return false;
      return new Regex("^((0|128|192|224|240|248|252|254|255?)\\.){3}(0|128|192|224|240|248|252|254|255?)$").IsMatch(SubnetMask);
    }

    public static bool IsMacAdress(string MacAddress)
    {
      if (string.IsNullOrEmpty(MacAddress))
        return false;
      return new Regex("^(?:[0-9a-fA-F][0-9a-fA-F][-:.]){5}[0-9a-fA-F][0-9a-fA-F]$").IsMatch(MacAddress);
    }

    public static bool IsValidGuid(string Guid)
    {
      if (string.IsNullOrEmpty(Guid))
        return false;
      return new Regex("^([0-9a-fA-F]){8}\\-([0-9a-fA-F]){4}\\-([0-9a-fA-F]){4}\\-([0-9a-fA-F]){4}\\-([0-9a-fA-F]){12}$").IsMatch(Guid);
    }
  }
}
