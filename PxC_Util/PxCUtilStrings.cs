// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.PxCUtilStrings
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PhoenixContact.PxC_Library.Util
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class PxCUtilStrings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal PxCUtilStrings()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (PxCUtilStrings.resourceMan == null)
          PxCUtilStrings.resourceMan = new ResourceManager("PhoenixContact.PxC_Library.Util.PxCUtilStrings", typeof (PxCUtilStrings).Assembly);
        return PxCUtilStrings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return PxCUtilStrings.resourceCulture;
      }
      set
      {
        PxCUtilStrings.resourceCulture = value;
      }
    }

    internal static string AddErrorCode
    {
      get
      {
        return PxCUtilStrings.ResourceManager.GetString(nameof (AddErrorCode), PxCUtilStrings.resourceCulture);
      }
    }

    internal static string ErrorCode
    {
      get
      {
        return PxCUtilStrings.ResourceManager.GetString(nameof (ErrorCode), PxCUtilStrings.resourceCulture);
      }
    }

    internal static string ErrorSource
    {
      get
      {
        return PxCUtilStrings.ResourceManager.GetString(nameof (ErrorSource), PxCUtilStrings.resourceCulture);
      }
    }

    internal static string InnerErrors
    {
      get
      {
        return PxCUtilStrings.ResourceManager.GetString(nameof (InnerErrors), PxCUtilStrings.resourceCulture);
      }
    }

    internal static string Timestamp
    {
      get
      {
        return PxCUtilStrings.ResourceManager.GetString(nameof (Timestamp), PxCUtilStrings.resourceCulture);
      }
    }
  }
}
