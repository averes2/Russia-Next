// Decompiled with JetBrains decompiler
// Type: Russia.SpellBar
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

namespace ConsoleDA
{
    public class SpellBar
    {
        public SpellBar.IconColor Color;
        public ushort Icon;

        public SpellBar(ushort icon, byte color)
        {
            this.Icon = icon;
            this.Color = (SpellBar.IconColor)color;
        }

        public enum IconColor
        {
            Gone = 0,
            Blue = 1,
            Green = 2,
            Yellow = 3,
            Orange = 4,
            Red = 5,
            White = 6,
        }
    }
}
