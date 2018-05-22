// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.ExceptionPxC
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Globalization;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public class ExceptionPxC : Exception
  {
    private int innerCount;

    public ExceptionPxC()
    {
      this.DateAndTime = DateTime.Now;
    }

    public ExceptionPxC(string pMessage)
      : base(pMessage)
    {
      this.DateAndTime = DateTime.Now;
    }

    public ExceptionPxC(string pMessage, ExceptionPxC InnerException)
      : base(pMessage, (Exception) InnerException)
    {
      this.DateAndTime = DateTime.Now;
    }

    public DateTime DateAndTime { get; private set; }

    public string GetAllMessages()
    {
      this.innerCount = 0;
      return this.GetExceptionMessage(this);
    }

    private string GetExceptionMessage(ExceptionPxC ExceptionData)
    {
      if (ExceptionData == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(ExceptionData.Message);
      stringBuilder.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.ErrorSource, (object) ExceptionData.Source));
      stringBuilder.AppendLine();
      if (ExceptionData.InnerException != null)
      {
        stringBuilder.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.InnerErrors, (object) ++this.innerCount));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(this.GetExceptionMessage((ExceptionPxC) ExceptionData.InnerException));
      }
      else
      {
        if (ExceptionData.DateAndTime != DateTime.MinValue)
          stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.Timestamp, (object) ExceptionData.DateAndTime.ToString()));
        this.innerCount = 0;
      }
      return stringBuilder.ToString();
    }
  }
}
