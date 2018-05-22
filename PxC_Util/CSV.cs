// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.CSV
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PhoenixContact.PxC_Library.Util
{
  public static class CSV
  {
    private static readonly object readLock = new object();

    public static Exception Read(Stream InputData, char[] Seperator, out List<string[]> CsvData)
    {
      lock (CSV.readLock)
      {
        CsvData = new List<string[]>();
        StreamReader streamReader = new StreamReader(InputData);
        bool flag = false;
        int num = 0;
        try
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string[] strArray = str.Split(Seperator);
            if (!flag)
            {
              flag = true;
              num = strArray.Length;
            }
            else if (num != strArray.Length)
            {
              CsvData = (List<string[]>) null;
              return Diagnostic.NewException("CSV Class", string.Format((IFormatProvider) CultureInfo.InvariantCulture, CsvStrings.RowCountError, (object) num, (object) strArray.Length));
            }
            CsvData.Add(strArray);
          }
        }
        catch (Exception ex)
        {
          CsvData = (List<string[]>) null;
          return Diagnostic.NewException("CSV Class", CsvStrings.ErrorWhileParsingData, ex);
        }
        return (Exception) null;
      }
    }
  }
}
