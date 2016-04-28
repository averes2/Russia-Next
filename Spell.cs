// Decompiled with JetBrains decompiler
// Type: Russia.Spell
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

namespace ConsoleDA
{
    public class Spell
    {
        public byte Lines;
        public string Name;
        public byte Slot;
        public SpellType type;

        public Spell(string name, byte slot, byte lines)
        {
            this.Name = name;
            this.Slot = slot;
            this.Lines = lines;
        }
    }
}
