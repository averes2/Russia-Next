// Decompiled with JetBrains decompiler
// Type: Russia.BarMessage
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    public class BarMessage
    {
        public DateTime Date;
        public string Message;
        public byte Type;

        public TimeSpan TimeElapsed
        {
            get
            {
                return DateTime.Now - this.Date;
            }
        }

        public BarMessage(byte type, string message)
        {
            this.Type = type;
            this.Message = message;
            this.Date = DateTime.Now;
        }
    }
}
