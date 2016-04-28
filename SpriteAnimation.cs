// Decompiled with JetBrains decompiler
// Type: Russia.SpriteAnimation
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

namespace ConsoleDA
{
    public class SpriteAnimation
    {
        public uint Serial { get; set; }

        public byte Animation { get; set; }

        public SpriteAnimation(uint _Serial, byte _Animation)
        {
            this.Serial = _Serial;
            this.Animation = _Animation;
        }
    }
}
