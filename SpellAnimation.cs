// Decompiled with JetBrains decompiler
// Type: Russia.SpellAnimation
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    public class SpellAnimation
    {
        public uint CastedFrom;
        public uint CastedTo;
        public ushort Number;
        public uint Speed;
        public DateTime Time;

        public SpellAnimation(uint To, uint From, ushort number, uint Speed = 100U)
        {
            this.Time = DateTime.Now;
            this.CastedTo = To;
            this.CastedFrom = From;
            this.Number = number;
            this.Speed = Speed;
        }
    }
}
