// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Util
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PhoenixContact.HFI
{
  public sealed class Util
  {
    private static int _maxMessageBoxes = 10;
    private static object _sender;
    private static DiagnosticArgs _diagnostic;
    private static int _msgCounter;
    private static bool _threadLock;

    private Util()
    {
    }

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
        numArray = Util.GetByteArrayElements(Source, StartAddress, DestinationByteArrayLength);
      }
      return numArray;
    }

    public static byte[] GetByteArrayFromService(byte[] Source)
    {
      byte[] numArray = new byte[0];
      if (Source != null && Source.Length >= 4)
      {
        int DestinationByteArrayLength = Util.ByteToInt32(Source[2], Source[3]) * 2 + 4;
        numArray = Util.GetByteArrayElements(Source, 0, DestinationByteArrayLength);
      }
      return numArray;
    }

    public static string Int32ToString(int Integer, int StringLength)
    {
      string str1 = "";
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
      string str = "";
      return Source.ToString("X").Length != 1 ? str + Source.ToString("X") : str + "0" + Source.ToString("X");
    }

    public static string ByteArrayToAsciiString(byte[] Source)
    {
      if (Source != null)
        return new ASCIIEncoding().GetString(Source, 0, Source.Length);
      return "";
    }

    public static byte[] AsciiStringToByteArray(string Source)
    {
      if (Source != null)
        return new ASCIIEncoding().GetBytes(Source);
      return new byte[0];
    }

    public static string ByteArrayToHexStringW(byte[] Source, char Seperator)
    {
      string str = "";
      if (Source != null)
      {
        for (int index = 0; index < Source.Length; ++index)
        {
          str += Util.ByteToHexString(Source[index]);
          if ((index + 1) % 2 == 0 && index > 0 && index < Source.Length - 1)
            str += (string) (object) Seperator;
        }
      }
      return str;
    }

    public static string StringJustification(string Text, int StartPosition, int LineLength)
    {
      StringBuilder stringBuilder = new StringBuilder("");
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
      return Util.CheckPcpConfirmation(Util.ByteToInt32(Byte_0, Byte_1));
    }

    public static int MaxMessageBoxes
    {
      get
      {
        return Util._maxMessageBoxes;
      }
      set
      {
        if (value > 100)
          return;
        Util._maxMessageBoxes = value;
      }
    }

    public static void MessageBoxShow(object Sender, DiagnosticArgs Diagnostic)
    {
      if (Util._msgCounter >= Util._maxMessageBoxes)
        return;
      while (Util._threadLock)
        Thread.Sleep(10);
      if (Util._threadLock)
        return;
      Util._threadLock = true;
      Util._sender = Sender;
      Util._diagnostic = Diagnostic;
      ++Util._msgCounter;
      new Thread(new ThreadStart(Util.ShowMessage))
      {
        Priority = ThreadPriority.Normal
      }.Start();
    }

    private static void ShowMessage()
    {
      string name = Util._diagnostic.Name;
      string str1 = Util._diagnostic.DiagCode.ToString();
      string hexStringW = Util.ByteArrayToHexStringW(Util._diagnostic.AddDiagCode, ' ');
      string str2 = Util._diagnostic.DateTime.ToString();
      string str3 = Util._msgCounter.ToString() + " : " + Util._sender.ToString();
      Util._threadLock = false;
      int num = (int) MessageBox.Show("Name: " + name + "\r\nDiagCode: " + str1 + "\r\nAddDiagCode (hex): " + hexStringW + "\r\n\r\nDate: " + str2, "Fehlermeldung " + str3, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      --Util._msgCounter;
    }
  }
}
