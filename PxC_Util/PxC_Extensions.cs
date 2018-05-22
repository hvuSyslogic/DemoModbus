// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.PxC_Extensions
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public static class PxC_Extensions
  {
    public static readonly string ExDataDateTime = "DateTime";
    public static readonly string ExDataAddMessage = "AddMessage";

    public static bool Compare(this byte[] pByte, byte[] pByteArray)
    {
      if (pByte == null || pByteArray == null || (pByte.Length != pByteArray.Length || pByteArray.Length == 0))
        return false;
      for (int index = 0; index < pByteArray.Length; ++index)
      {
        if ((int) pByte[index] != (int) pByteArray[index])
          return false;
      }
      return true;
    }

    public static PingReply SendPing(this string ipAddress)
    {
      Ping ping = new Ping();
      try
      {
        return ping.Send(ipAddress);
      }
      catch (Exception ex)
      {
        return (PingReply) null;
      }
    }

    public static DateTime GetDateTime(this Exception exception)
    {
      object obj;
      if (exception != null && (obj = exception.Data[(object) PxC_Extensions.ExDataDateTime]) != null)
        return (DateTime) obj;
      return DateTime.MinValue;
    }

    public static void InitDateTime(this Exception exception, DateTime dateTime)
    {
      if (!(exception.GetDateTime() == DateTime.MinValue))
        return;
      exception.SetDateTime(dateTime);
    }

    public static void SetDateTime(this Exception exception, DateTime dateTime)
    {
      exception.Data[(object) PxC_Extensions.ExDataDateTime] = (object) dateTime;
    }

    public static byte[] GetAddMessage(this Exception exception)
    {
      object obj;
      if ((obj = exception.Data[(object) PxC_Extensions.ExDataAddMessage]) != null)
        return obj as byte[];
      return (byte[]) null;
    }

    public static void InitAddMessage(this Exception exception, byte[] value)
    {
      if (exception.GetAddMessage() != null)
        return;
      exception.SetAddMessage(value);
    }

    public static void SetAddMessage(this Exception exception, byte[] data)
    {
      if (data == null || data.Length == 0)
        return;
      exception.Data[(object) PxC_Extensions.ExDataAddMessage] = (object) data;
    }

    public static string GetExceptionMessage(this Exception exception)
    {
      StringBuilder exceptionString1 = new StringBuilder();
      StringBuilder exceptionString2 = exception.WriteExceptionData(exceptionString1);
      if (exception.InnerException != null)
      {
        exceptionString2.AppendLine();
        exceptionString2.AppendLine(PxCUtilStrings.InnerErrors);
        exceptionString2 = exception.InnerException.WriteInnerExceptions(exceptionString2);
      }
      if (exception.GetDateTime() != DateTime.MinValue)
      {
        exceptionString2.AppendLine();
        exceptionString2.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.Timestamp, (object) exception.GetDateTime().ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      }
      return exceptionString2.ToString();
    }

    private static StringBuilder WriteExceptionData(this Exception exception, StringBuilder exceptionString)
    {
      exceptionString.AppendLine(exception.Message);
      if (!string.IsNullOrEmpty(exception.Source))
        exceptionString.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.ErrorSource, (object) exception.Source));
      if (exception.GetAddMessage() != null)
      {
        exceptionString.AppendLine();
        string hexStringW = PhoenixContact.PxC_Library.Util.Util.ByteArrayToHexStringW(exception.GetAddMessage(), ' ');
        exceptionString.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.AddErrorCode, (object) hexStringW));
      }
      return exceptionString;
    }

    private static StringBuilder WriteInnerExceptions(this Exception exception, StringBuilder exceptionString)
    {
      if (exception.InnerException == null)
      {
        exception.WriteExceptionData(exceptionString);
      }
      else
      {
        exceptionString = exception.InnerException.WriteInnerExceptions(exceptionString);
        exception.WriteExceptionData(exceptionString);
      }
      return exceptionString;
    }

    public static List<Exception> GetAllExceptions(this Exception exception)
    {
      List<Exception> list = new List<Exception>();
      return exception.GetInnerExceptions(list);
    }

    private static List<Exception> GetInnerExceptions(this Exception exception, List<Exception> list)
    {
      if (exception.InnerException == null)
      {
        list.Add(exception);
      }
      else
      {
        list = exception.InnerException.GetInnerExceptions(list);
        list.Add(exception);
      }
      return list;
    }
  }
}
