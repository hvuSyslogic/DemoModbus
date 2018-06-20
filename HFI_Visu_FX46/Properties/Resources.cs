// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Visualization.Properties.Resources
// Assembly: HFI_Visu_FX46, Version=3.2.6053.23250, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: A9FB10B7-9AE3-4F4C-88CF-1D5F3BF257DC
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Visu_FX46.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PhoenixContact.HFI.Visualization.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (PhoenixContact.HFI.Visualization.Properties.Resources.resourceMan == null)
          PhoenixContact.HFI.Visualization.Properties.Resources.resourceMan = new ResourceManager("PhoenixContact.HFI.Visualization.Properties.Resources", typeof (PhoenixContact.HFI.Visualization.Properties.Resources).Assembly);
        return PhoenixContact.HFI.Visualization.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture;
      }
      set
      {
        PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Bitmap navigate_down
    {
      get
      {
        return (Bitmap) PhoenixContact.HFI.Visualization.Properties.Resources.ResourceManager.GetObject(nameof (navigate_down), PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap navigate_up
    {
      get
      {
        return (Bitmap) PhoenixContact.HFI.Visualization.Properties.Resources.ResourceManager.GetObject(nameof (navigate_up), PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap redo
    {
      get
      {
        return (Bitmap) PhoenixContact.HFI.Visualization.Properties.Resources.ResourceManager.GetObject(nameof (redo), PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap undo
    {
      get
      {
        return (Bitmap) PhoenixContact.HFI.Visualization.Properties.Resources.ResourceManager.GetObject(nameof (undo), PhoenixContact.HFI.Visualization.Properties.Resources.resourceCulture);
      }
    }
  }
}
