// Decompiled with JetBrains decompiler
// Type: MyResources.Resources
// Assembly: DAWalkerWorldMapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D5A0B7E-C7CF-42F2-8A86-EC18BB1CD35B
// Assembly location: C:\Users\adm\Downloads\DAWalker\DAWalkerWorldMapEditor.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ConsoleDA
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [CompilerGenerated]
    [DebuggerNonUserCode]
    internal class Resources
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals((object)Resources.resourceMan, (object)null))
                    Resources.resourceMan = new ResourceManager("ConsoleDA.Properties.Resources", typeof(Resources).Assembly);
                return Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return Resources.resourceCulture;
            }
            set
            {
                Resources.resourceCulture = value;
            }
        }

        internal static Bitmap texTRANS
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("texTRANS1", Resources.resourceCulture);
            }
        }
        internal static Bitmap textPLAYER
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textPLAYER", Resources.resourceCulture);
            }
        }
        internal static Bitmap textMONSTER
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textMONSTER", Resources.resourceCulture);
            }
        }
        internal static Bitmap textITEM
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textITEM", Resources.resourceCulture);
            }
        }
        internal static Bitmap textOTHER
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textOTHER", Resources.resourceCulture);
            }
        }
        internal static Bitmap textME
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textME", Resources.resourceCulture);
            }
        }
        internal static Bitmap textSPECIALENTITY
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textSPECENTITY", Resources.resourceCulture);
            }
        }
        internal static Bitmap textBLOCK
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textBLOCK", Resources.resourceCulture);
            }
        }
        internal static Bitmap textDOOR
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textDOOR", Resources.resourceCulture);
            }
        }
        internal static Bitmap textWAYPOINT
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("textWAYPOINT", Resources.resourceCulture);
            }
        }
        internal static Bitmap customCARROT
        {
            get
            {
                return (Bitmap)Resources.ResourceManager.GetObject("customCARROT", Resources.resourceCulture);
            }
        }
        internal Resources()
        {
        }
    }
}
