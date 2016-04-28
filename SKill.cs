// Decompiled with JetBrains decompiler
// Type: Dean.SKill
// Assembly: Dean, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia.exe

using System;

namespace ConsoleDA
{
    public class SKill
    {
        public byte Slot { get; set; }

        public ushort Icon { get; set; }

        public string Name { get; set; }

        public DateTime LastUsed { get; set; }

        public SKill(string _name, byte _slot, ushort _icon)
        {
            this.Name = _name;
            this.Slot = _slot;
            this.Icon = _icon;
        }
    }
}
