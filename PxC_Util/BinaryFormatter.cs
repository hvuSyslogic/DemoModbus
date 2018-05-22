// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.BinaryFormatter
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Globalization;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public class BinaryFormatter : IFormatProvider, ICustomFormatter
  {
    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
      BinaryFormatter.StringFormat format1 = this.GetFormat(arg, format);
      if (format1.VarBase == 0)
        return this.HandleOtherFormats(format, arg);
      if (arg is sbyte)
      {
        byte[] data = new byte[1]
        {
          byte.Parse(((sbyte) arg).ToString("X2"), NumberStyles.HexNumber)
        };
        return this.HandleByteFormat(format1, data);
      }
      if (arg is byte)
      {
        byte[] data = new byte[1]{ (byte) arg };
        return this.HandleByteFormat(format1, data);
      }
      if (arg is byte[])
      {
        byte[] data = arg as byte[];
        return this.HandleByteArrayFormat(format1, data);
      }
      if (arg is short)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((short) arg));
      if (arg is int)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((int) arg));
      if (arg is long)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((long) arg));
      if (arg is ushort)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((ushort) arg));
      if (arg is uint)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((uint) arg));
      if (arg is ulong)
        return this.HandleNumericFormat(format1, BitConverter.GetBytes((ulong) arg));
      try
      {
        return this.HandleOtherFormats(format, arg);
      }
      catch (FormatException ex)
      {
        throw new FormatException(string.Format("The format of '{0}' is invalid.", (object) format), (Exception) ex);
      }
    }

    public object GetFormat(Type formatType)
    {
      if (formatType == typeof (ICustomFormatter))
        return (object) this;
      return (object) null;
    }

    private BinaryFormatter.StringFormat GetFormat(object arg, string format)
    {
      BinaryFormatter.StringFormat stringFormat;
      stringFormat.VarBase = 0;
      stringFormat.WordFormat = false;
      string str = string.Empty;
      if (!string.IsNullOrEmpty(format))
      {
        str = format.Length > 1 ? format.Substring(0, 1) : format;
        if (format.Length >= 3)
          stringFormat.WordFormat = format.Substring(2, 1).ToUpper().CompareTo("W") != 1;
      }
      string upper = str.ToUpper();
      if (!(upper == "B"))
      {
        if (upper == "H")
        {
          stringFormat.VarBase = 16;
        }
        else
        {
          try
          {
            stringFormat.VarBase = 0;
          }
          catch (FormatException ex)
          {
            throw new FormatException(string.Format("The format of '{0}' is invalid.", (object) format), (Exception) ex);
          }
        }
      }
      else
        stringFormat.VarBase = 2;
      return stringFormat;
    }

    private string HandleByteFormat(BinaryFormatter.StringFormat format, byte[] data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (data.Length != 0)
      {
        stringBuilder.Append(this.FillByte(format, Convert.ToString(data[0], format.VarBase)));
        if (format.WordFormat)
          stringBuilder.Insert(0, "00");
      }
      return stringBuilder.ToString().Trim();
    }

    private string HandleByteArrayFormat(BinaryFormatter.StringFormat format, byte[] data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      for (int index = 0; index < data.Length; ++index)
      {
        if (format.WordFormat)
        {
          if (index % 2 == 0)
            stringBuilder.Append(" ");
        }
        else
          stringBuilder.Append(" ");
        if (index < data.Length)
          str = this.FillByte(format, Convert.ToString(data[index], format.VarBase));
        stringBuilder.Append(str);
        str = string.Empty;
      }
      return stringBuilder.ToString().Trim();
    }

    private string HandleNumericFormat(BinaryFormatter.StringFormat format, byte[] data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 1;
      for (int upperBound = data.GetUpperBound(0); upperBound >= data.GetLowerBound(0); --upperBound)
      {
        stringBuilder.Append(this.FillByte(format, Convert.ToString(data[upperBound], format.VarBase)));
        if (format.WordFormat)
        {
          if (num++ % 2 == 0)
            stringBuilder.Append(" ");
        }
        else
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString().Trim();
    }

    private string HandleOtherFormats(string format, object arg)
    {
      if (arg is IFormattable)
        return ((IFormattable) arg).ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
      if (arg != null)
        return arg.ToString();
      return string.Empty;
    }

    private string FillByte(BinaryFormatter.StringFormat format, string data)
    {
      string empty = string.Empty;
      return (format.VarBase != 2 ? new string('0', 2 - data.Length) : new string('0', 8 - data.Length)) + data;
    }

    private struct StringFormat
    {
      public int VarBase;
      public bool WordFormat;
    }
  }
}
