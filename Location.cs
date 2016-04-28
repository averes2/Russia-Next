// Decompiled with JetBrains decompiler
// Type: Russia.Location
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    public class Location
    {
        public Direction Facing;
        public DateTime LastActive;
        public ushort X;
        public ushort Y;

        public static int operator - (Location l1, Location l2) {
            int x_z = Math.Abs(l1.X - l2.X);
            int y_z = Math.Abs(l1.Y - l2.Y);
            return (x_z+y_z);
        }

        public override string ToString()
        {
            return (string)(object)this.X + (object)"," + (string)(object)this.Y + " Direction: " + (string)(object)this.Facing;
        }
    }
}
