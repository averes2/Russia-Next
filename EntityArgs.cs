// Decompiled with JetBrains decompiler
// Type: Russia.EntityArgs
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    public class EntityArgs : EventArgs
    {
        public Entity Entity { get; set; }

        public SpellAnimation Animation { get; set; }

        public DateTime Time { get; set; }

        public TimeSpan Elapsed
        {
            get
            {
                return DateTime.Now - this.Time;
            }
        }

        public EntityArgs(Entity entity, SpellAnimation animation)
        {
            this.Time = DateTime.Now;
            this.Entity = entity;
            this.Animation = animation;
        }
    }
}
