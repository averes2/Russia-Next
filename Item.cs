// Decompiled with JetBrains decompiler
// Type: Russia.Item
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    public class Item
    {
        public byte Slot { get; set; }

        public ushort IconSet { get; set; }

        public byte Icon { get; set; }

        public string Name { get; set; }

        public uint Amount { get; set; }

        public bool Stackable { get; set; }

        public uint CurrentDurability { get; set; }

        public uint MaximumDurability { get; set; }

        public bool Equipped { get; set; }

        public DateTime LastUsed { get; set; }
    }
}
