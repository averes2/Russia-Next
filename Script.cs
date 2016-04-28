// Decompiled with JetBrains decompiler
// Type: Dean.Script
// Assembly: Dean, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Dean\Dean.exe

namespace ConsoleDA
{
    public abstract class Script
    {
        public bool Running;
        public Client client;

        public virtual void OnMessage(string msg)
        {
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
