// Decompiled with JetBrains decompiler
// Type: Russia.User
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;

namespace ConsoleDA
{
    /// <summary>
    /// Check to see what this class is. If its soley for keeping track of other users whispers make sure its
    /// from a client.
    /// </summary>
    public class User
    {
        public string Name { get; set; }

        public DateTime LastWhispered { get; set; }
    }
}
