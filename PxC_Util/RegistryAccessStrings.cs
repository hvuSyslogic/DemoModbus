// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.RegistryAccessStrings
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
  internal class RegistryAccessStrings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal RegistryAccessStrings()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (RegistryAccessStrings.resourceMan == null)
          RegistryAccessStrings.resourceMan = new ResourceManager("PhoenixContact.PxC_Library.Util.RegistryAccessStrings", typeof (RegistryAccessStrings).Assembly);
        return RegistryAccessStrings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return RegistryAccessStrings.resourceCulture;
      }
      set
      {
        RegistryAccessStrings.resourceCulture = value;
      }
    }

    internal static string DeleteKeyError
    {
      get
      {
        return RegistryAccessStrings.ResourceManager.GetString(nameof (DeleteKeyError), RegistryAccessStrings.resourceCulture);
      }
    }

    internal static string DeleteValueError
    {
      get
      {
        return RegistryAccessStrings.ResourceManager.GetString(nameof (DeleteValueError), RegistryAccessStrings.resourceCulture);
      }
    }

    internal static string WriteValueError
    {
      get
      {
        return RegistryAccessStrings.ResourceManager.GetString(nameof (WriteValueError), RegistryAccessStrings.resourceCulture);
      }
    }
  }
}
