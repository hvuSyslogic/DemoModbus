// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.EnvironmentInfo
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public static class EnvironmentInfo
  {
    public static string GetAllInformation(Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(EnvironmentInfo.GetErrorMessage(ex));
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(EnvironmentInfo.GetStackTraceInformation(ex));
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(EnvironmentInfo.GetSystemInformation());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(EnvironmentInfo.GetApplicationInformation());
      return stringBuilder.ToString();
    }

    public static string GetSystemInformation()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Machine Name:\t");
      try
      {
        stringBuilder.Append(Environment.MachineName);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Numer of CPUs:\t");
      try
      {
        stringBuilder.Append(Environment.ProcessorCount);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Operating System:\t");
      try
      {
        stringBuilder.Append((object) Environment.OSVersion);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("CLR Version:\t");
      try
      {
        stringBuilder.Append((object) Environment.Version);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Current User:\t");
      stringBuilder.Append(EnvironmentInfo.UserIdentity());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(EnvironmentInfo.GetCurrentIPs());
      return stringBuilder.ToString();
    }

    public static string GetApplicationInformation()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Application Domain:\t\t");
      try
      {
        stringBuilder.Append(AppDomain.CurrentDomain.FriendlyName);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Assembly Codebase:\t");
      try
      {
        stringBuilder.Append(EnvironmentInfo.ParentAssembly().CodeBase);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Assembly Full Name:\t");
      try
      {
        stringBuilder.Append(EnvironmentInfo.ParentAssembly().FullName);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Assembly Build Date:\t");
      try
      {
        stringBuilder.Append(EnvironmentInfo.AssemblyFileTime(EnvironmentInfo.ParentAssembly()));
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Assembly Memory Usage:\t");
      try
      {
        stringBuilder.Append(Environment.WorkingSet);
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(EnvironmentInfo.GetAssemblyReferences());
      return stringBuilder.ToString();
    }

    public static string GetErrorMessage(Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Error Message:\t ");
      try
      {
        stringBuilder.Append(ex.Message.ToString());
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      return stringBuilder.ToString();
    }

    public static string GetStackTraceInformation(Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      StackTrace stackTrace = new StackTrace(ex, true);
      StackFrame frame;
      MethodBase method;
      try
      {
        frame = stackTrace.GetFrame(0);
        method = frame.GetMethod();
        stringBuilder.Append("Stack Trace:");
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append("Error in:\t ");
        stringBuilder.Append(method.DeclaringType.ToString() + ", ");
      }
      catch
      {
        stringBuilder.Append("Error while reading Stack Trace informations!");
        stringBuilder.Append(Environment.NewLine);
        return stringBuilder.ToString();
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Method:\t ");
      try
      {
        stringBuilder.Append(method.Name + "(");
        ParameterInfo[] parameters = method.GetParameters();
        string str = "";
        foreach (ParameterInfo parameterInfo in parameters)
          str = str + ", " + parameterInfo.ParameterType.Name + " " + parameterInfo.Name;
        if (str.Length > 2)
          stringBuilder.Append(str.Substring(2));
        stringBuilder.Append(")");
      }
      catch
      {
        stringBuilder.Append("Error while reading Stack Trace method!");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Line:\t ");
      try
      {
        stringBuilder.Append(frame.GetFileLineNumber().ToString());
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("Row:\t ");
      try
      {
        stringBuilder.Append(frame.GetFileColumnNumber().ToString());
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("IL Offset:\t ");
      try
      {
        stringBuilder.Append(frame.GetILOffset().ToString());
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append("File:\t ");
      try
      {
        stringBuilder.Append(frame.GetFileName().ToString());
      }
      catch
      {
        stringBuilder.Append("-");
      }
      stringBuilder.Append(Environment.NewLine);
      return stringBuilder.ToString();
    }

    private static string UserIdentity()
    {
      string str;
      try
      {
        str = EnvironmentInfo.CurrentWindowsIdentity();
        if (str == "")
          str = EnvironmentInfo.CurrentEnvironmentIdentity();
      }
      catch
      {
        str = "-";
      }
      return str;
    }

    private static string CurrentWindowsIdentity()
    {
      try
      {
        return WindowsIdentity.GetCurrent().Name;
      }
      catch
      {
        return "-";
      }
    }

    private static string CurrentEnvironmentIdentity()
    {
      try
      {
        return Environment.UserDomainName + "\\" + Environment.UserName;
      }
      catch
      {
        return "-";
      }
    }

    private static string GetCurrentIPs()
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        for (int index = 0; index < addressList.Length; ++index)
        {
          stringBuilder.Append("IP Address " + index.ToString() + ":\t");
          stringBuilder.Append(addressList[index].ToString());
          stringBuilder.Append(Environment.NewLine);
        }
      }
      catch
      {
        stringBuilder.Append("-");
      }
      return stringBuilder.ToString();
    }

    private static Assembly ParentAssembly()
    {
      Assembly assembly = (Assembly) null;
      if (assembly == (Assembly) null)
        assembly = !(Assembly.GetEntryAssembly() == (Assembly) null) ? Assembly.GetEntryAssembly() : Assembly.GetCallingAssembly();
      return assembly;
    }

    private static string AssemblyFileTime(Assembly objAssembly)
    {
      try
      {
        return System.IO.File.GetLastWriteTime(objAssembly.Location).ToString();
      }
      catch
      {
        return DateTime.MaxValue.ToString();
      }
    }

    private static string GetAssemblyReferences()
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        AssemblyName[] referencedAssemblies = EnvironmentInfo.ParentAssembly().GetReferencedAssemblies();
        if (referencedAssemblies.Length != 0)
        {
          stringBuilder.Append("Assembly References:");
          stringBuilder.Append(Environment.NewLine);
          for (int index = 0; index < referencedAssemblies.Length; ++index)
          {
            stringBuilder.Append("\tRef. " + index.ToString() + ":\t");
            stringBuilder.Append(referencedAssemblies[index].FullName);
            stringBuilder.Append(Environment.NewLine);
          }
        }
        else
        {
          stringBuilder.Append("No Assembly References available!");
          stringBuilder.Append(Environment.NewLine);
        }
      }
      catch
      {
        stringBuilder.Append("Error while reading Assembly References!");
        stringBuilder.Append(Environment.NewLine);
      }
      return stringBuilder.ToString();
    }
  }
}
